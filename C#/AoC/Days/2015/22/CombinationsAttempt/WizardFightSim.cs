namespace AoC.Days;

internal class WizardFightSim
{
    private readonly int _bossMaxHP;
    private readonly int _bossBaseDamage;
    private int BossHP { get; set; }

    private const int _heroMaxHP = 50;
    private int HeroArmor { get; set; } = 0;
    private int HeroHP { get; set; }
    private int HeroMP { get; set; }

    private HashSet<EffectInstance> ActiveEffects { get; } = [];
    private const int shieldMaxDuration = 6;
    private const int poisonMaxDuration = 6;
    private const int rechargeMaxDuration = 5;

    private bool hardModeDelayer = false;

    public int ManaSpent { get; set; }

    public WizardFightSim(string[] inputLines)
    {
        _bossMaxHP = int.Parse(inputLines[0].Split(": ")[1]);
        _bossBaseDamage = int.Parse(inputLines[1].Split(": ")[1]);
    }

    public bool Attempt(Spell[] strategy, bool hardMode)
    {
        ManaSpent = 0;
        BossHP = _bossMaxHP;
        HeroHP = _heroMaxHP;
        HeroMP = SpellStat.HeroMana;
        HeroArmor = 0;
        ActiveEffects.Clear();

        if (hardMode)
        {
            ActiveEffects.Add(new(Effect.HardMode, int.MaxValue));
            hardModeDelayer = false;
        }
        
        int spellIndex = -1;
        while (BossHP > 0 && HeroHP > 0)
        {
            if(ProcessEffects())
            {
                break;
            }
            spellIndex++;
            if (spellIndex == strategy.Length)
            {
                return false;
            }
            if (!CastSpell(strategy[spellIndex]))
            {
                return false;
            }
            ProcessEffects();
            BossAttack();
        }

        if (BossHP <= 0)
        {
            return true;
        }
        return false;
    }

    private void BossAttack()
    {
        HeroHP -= Math.Max(1, _bossBaseDamage - HeroArmor);
    }

    private bool ProcessEffects()
    {
        foreach (EffectInstance effect in ActiveEffects.ToHashSet())
        {
            switch (effect.Type)
            {
                case Effect.Shield:
                    if(--effect.RemainingTurns == 0)
                    {
                        HeroArmor -= 7;
                        ActiveEffects.Remove(effect);
                    }
                    break;

                case Effect.Poison:
                    BossHP -= 3;
                    if(--effect.RemainingTurns == 0)
                    {
                        ActiveEffects.Remove(effect);
                    }
                    break;

                case Effect.Recharge:
                    HeroMP += 101;
                    if (--effect.RemainingTurns == 0)
                    {
                        ActiveEffects.Remove(effect);
                    }
                    break;

                case Effect.HardMode:
                    if (hardModeDelayer)
                    {
                        hardModeDelayer = false;
                    }
                    else
                    {
                        HeroHP--;
                        hardModeDelayer = true;
                    }
                    break;

                default:
                    throw new Exception("Unexpected effect");
            }
        }
        if (BossHP <= 0 || HeroHP <= 0)
        {
            return true;
        }
        return false;
    }
    
    private bool CastSpell(Spell spell)
    {
        if (SpellStat.Cost[spell] > HeroMP)
        {
            return false;
        }
        HeroMP -= SpellStat.Cost[spell];
        ManaSpent += SpellStat.Cost[spell];
        switch (spell)
        {
            case Spell.MagicMissile:
                BossHP -= 4;
                return true;

            case Spell.Drain:
                BossHP -= 2;
                HeroHP += 2;
                return true;

            case Spell.Shield:
                if(!ActiveEffects.Add(new(Effect.Shield, shieldMaxDuration)))
                {
                    return false;
                }
                HeroArmor += 7;
                return true;

            case Spell.Poison:
                if (!ActiveEffects.Add(new(Effect.Poison, poisonMaxDuration)))
                {
                    return false;
                }
                return true;

            case Spell.Recharge:
                if (!ActiveEffects.Add(new(Effect.Recharge, rechargeMaxDuration)))
                {
                    return false;
                }
                return true;

            default:
                throw new Exception("spell didnt match any case");
        }
    }
}

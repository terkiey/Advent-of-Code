namespace AoC.Days;

internal static class SpellStat
{
    public const int HeroMana = 500;

    public static Dictionary<Spell, int> Cost = new()
    {
        {Spell.MagicMissile, 53},
        {Spell.Drain, 73},
        {Spell.Shield, 113},
        {Spell.Poison, 173},
        {Spell.Recharge, 229}
    };
}

internal enum Spell
{
    MagicMissile = 0,
    Drain = 1,
    Shield = 2,
    Poison = 3,
    Recharge = 4,
}

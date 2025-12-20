namespace AoC.Days;

internal class SpellRotationGenerator
{
    private readonly int startMana = SpellStat.HeroMana;
    private const int rechargeMana = 505;
    private readonly Dictionary<Spell, int> spellCosts = SpellStat.Cost;

    public IEnumerable<Spell[]> GetSpellRotations(int rechargeCasts)
    {
        int lastMaxManaSpend = startMana + ((rechargeCasts - 1) * rechargeMana);
        int maxManaSpend = startMana + (rechargeCasts * rechargeMana);
        int cheapestSpell = spellCosts.Values.Min();
        int maxSpells = maxManaSpend / cheapestSpell;

        int minManaSpend = lastMaxManaSpend + spellCosts[Spell.Recharge];
        int costliestSpell = spellCosts.Values.Max();
        int minSpells = minManaSpend / costliestSpell;
        if (rechargeCasts == 0)
        {
            minManaSpend = 0;
        }

        for (int spellCount = minSpells; spellCount <= maxSpells; spellCount++)
        {
            var spellRotations = TakeSpellsWithReplacement(spellCount - rechargeCasts);
            var spellRotationsWithRecharges = rechargeCasts == 0 ? spellRotations : InsertRechargeCasts(spellRotations, rechargeCasts);
            var validSpellRotations = spellRotationsWithRecharges.Where(rotation =>
            {
                int manaSpend = rotation.Sum(spell => spellCosts[spell]);
                return manaSpend >= minManaSpend && manaSpend <= maxManaSpend;
            });
            var spellRotationsSorted = validSpellRotations.OrderBy(rotation => rotation.Sum(spell => spellCosts[spell]));
            foreach (var rotation in spellRotationsSorted)
            {
                 yield return rotation;
            }
        }
    }

    private static IEnumerable<Spell[]> InsertRechargeCasts(IEnumerable<Spell[]> spellRotations, int rechargeCasts)
    {
        foreach(var rotation in spellRotations)
        {
            foreach( var newRotation in InsertRechargeCastsIntoRotation(rotation, rechargeCasts))
            {
                yield return newRotation;
            }
        }
    }

    private static IEnumerable<Spell[]> InsertRechargeCastsIntoRotation(Spell[] inputRotation, int rechargeCasts)
    {
        IEnumerable<List<Spell>> current = [ inputRotation.ToList() ];
        for (int i = 0; i < rechargeCasts; i++)
        {
            current = current.SelectMany(rotation =>
                Enumerable.Range(0, rotation.Count + 1)
                    .Select(insertIndex =>
                    {
                        var copy = new List<Spell>(rotation);
                        copy.Insert(insertIndex, Spell.Recharge);
                        return copy;
                    }));
        }

        foreach (var r in current)
        {
            yield return r.ToArray();
        }
    }

    private static IEnumerable<Spell[]> TakeSpellsWithReplacement(int length)
    {
        var values = Enum.GetValues(typeof(Spell)).Cast<Spell>().Where(spell => spell != Spell.Recharge).ToArray();
        int n = values.Length;

        var indices = new int[length];

        while (true)
        {
            yield return indices.Select(i => values[i]).ToArray();

            int pos = length - 1;
            while (pos >= 0 && ++indices[pos] == n)
            {
                indices[pos] = 0;
                pos--;
            }

            if (pos < 0)
                break;
        }
    }
}

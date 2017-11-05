namespace EzEvade_Port.SpecialSpells
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Spells;
    using SpellData = Spells.SpellData;

    class Yorick : ChampionPlugin
    {
        public void LoadSpecialSpell(SpellData spellData)
        {
            if (spellData.SpellName == "YorickE")
            {
                SpellDetector.OnProcessSpecialSpell += SpellDetector_OnProcessSpecialSpell;
            }
        }

        private void SpellDetector_OnProcessSpecialSpell(Obj_AI_Base hero, Obj_AI_BaseMissileClientDataEventArgs args, SpellData spellData, SpecialSpellEventArgs specialSpellArgs)
        {
            if (spellData.SpellName == "YorickE")
            {
                var end = args.End;
                var start = args.Start;
                var direction = (end - start).Normalized();

                if (start.Distance(end) > spellData.Range)
                {
                    end = start + (end - start).Normalized() * spellData.Range;
                }

                var spellStart = end.Extend(hero.ServerPosition, 100);
                var spellEnd = spellStart + direction * 1;

                SpellDetector.CreateSpellData(hero, spellStart, spellEnd, spellData);
                specialSpellArgs.NoProcess = true;
            }
        }
    }
}
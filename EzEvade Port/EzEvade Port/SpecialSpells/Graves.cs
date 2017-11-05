namespace EzEvade_Port.SpecialSpells
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Core;
    using Spells;
    using SpellData = Spells.SpellData;

    class Graves : ChampionPlugin
    {
        public void LoadSpecialSpell(SpellData spellData)
        {
            if (spellData.SpellName == "GravesQLineSpell")
            {
                SpellDetector.OnProcessSpecialSpell += SpellDetector_OnProcessSpecialSpell;
            }
        }

        private void SpellDetector_OnProcessSpecialSpell(Obj_AI_Base hero, Obj_AI_BaseMissileClientDataEventArgs args, SpellData spellData, SpecialSpellEventArgs specialSpellArgs)
        {
            if (spellData.SpellName == "GravesQLineSpell")
            {
                var newData = (SpellData) spellData.Clone();
                newData.IsPerpendicular = true;
                newData.SecondaryRadius = 255f;
                newData.UpdatePosition = false;
                newData.ExtraEndTime = 1300;

                var end = args.End;
                var start = args.Start;

                if (end.Distance(start) > newData.Range)
                {
                    end = args.Start + (args.End - args.Start).Normalized() * newData.Range;
                }

                if (end.Distance(start) < newData.Range)
                {
                    end = args.Start + (args.End - args.Start).Normalized() * newData.Range;
                }

                var w = EvadeHelper.GetNearWallPoint(start, end);
                if (w != default(Vector3))
                {
                    end = w;
                }

                //SpellDetector.CreateSpellData(hero, hero.ServerPosition, end, spellData);
                SpellDetector.CreateSpellData(hero, hero.ServerPosition, end, newData);
            }
        }
    }
}
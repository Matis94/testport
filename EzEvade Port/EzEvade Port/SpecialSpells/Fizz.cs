namespace EzEvade_Port.SpecialSpells
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Helpers;
    using Spells;
    using SpellData = Spells.SpellData;

    class Fizz : ChampionPlugin
    {
        public void LoadSpecialSpell(SpellData spellData)
        {
            if (spellData.SpellName == "FizzR")
            {
                GameObject.OnCreate += obj => OnCreateObj_FizzMarinerDoom(obj, spellData);
                GameObject.OnDestroy += obj => OnDeleteObj_FizzMarinerDoom(obj, spellData);
                SpellDetector.OnProcessSpecialSpell += ProcessSPellFizzMarinerDoom;
            }

            if (spellData.SpellName == "FizzQ")
            {
                SpellDetector.OnProcessSpecialSpell += ProcessSpell_FizzPiercingStrike;
            }
        }

        private void ProcessSPellFizzMarinerDoom(Obj_AI_Base hero, Obj_AI_BaseMissileClientDataEventArgs args, SpellData spellData, SpecialSpellEventArgs specialSpellArgs)
        {
            if (spellData.SpellName == "FizzR")
            {
                var start = args.Start;
                var endPos = args.End;

                if (start.Distance(endPos) > spellData.Range)
                {
                    endPos = start + (endPos - start).Normalized() * spellData.Range;
                }

                var dist = start.Distance(endPos);
                var radius = dist > 910 ? 400 : (dist >= 455 ? 300 : 200);

                var data = (SpellData) spellData.Clone();
                data.SecondaryRadius = radius;

                specialSpellArgs.SpellData = data;
            }
        }

        private static void ProcessSpell_FizzPiercingStrike(Obj_AI_Base hero, Obj_AI_BaseMissileClientDataEventArgs args, SpellData spellData, SpecialSpellEventArgs specialSpellArgs)
        {
            if (spellData.SpellName == "FizzQ")
            {
                if (args.Target != null && args.Target.IsMe)
                {
                    SpellDetector.CreateSpellData(hero, args.Start, args.End, spellData);
                }

                specialSpellArgs.NoProcess = true;
            }
        }

        private static void OnDeleteObj_FizzMarinerDoom(GameObject obj, SpellData spellData)
        {
            //need to track where bait is attached to
            if (obj.IsValid)
            {
                if (obj.Type != GameObjectType.MissileClient)
                {
                    return;
                }
            }

            var missile = (MissileClient) obj;

            var dist = missile.StartPosition.Distance(missile.EndPosition);
            var radius = dist > 910 ? 400 : (dist >= 455 ? 300 : 200);

            if (missile.SpellCaster != null && missile.SpellCaster.CheckTeam() && missile.SpellData.Name == "FizzRMissile")
            {
                SpellDetector.CreateSpellData(missile.SpellCaster, missile.StartPosition, missile.EndPosition, spellData, null, 1000, true, SpellType.Circular, false, radius);
            }
        }

        private static void OnCreateObj_FizzMarinerDoom(GameObject obj, SpellData spellData)
        {
            // need to fix obj.isvalid on a previous thing.. I set it to just check the type
            if (obj.IsValid)
            {
                if (obj.Type != GameObjectType.MissileClient)
                {
                    return;
                }
            }

            var missile = (MissileClient) obj;

            var dist = missile.StartPosition.Distance(missile.EndPosition);
            var radius = dist > 910 ? 400 : (dist >= 455 ? 300 : 200);

            if (missile.SpellCaster != null && missile.SpellCaster.CheckTeam() && missile.SpellData.Name == "FizzRMissile")
            {
                SpellDetector.CreateSpellData(missile.SpellCaster, missile.StartPosition, missile.EndPosition, spellData, null, 500, true, SpellType.Circular, false, radius);
            }
        }
    }
}
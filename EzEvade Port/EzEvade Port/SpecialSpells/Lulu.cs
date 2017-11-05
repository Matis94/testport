namespace EzEvade_Port.SpecialSpells
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Helpers;
    using Spells;
    using Utils;
    using SpellData = Spells.SpellData;

    class Lulu : ChampionPlugin
    {
        public void LoadSpecialSpell(SpellData spellData)
        {
            if (spellData.SpellName == "LuluQ")
            {
                SpellDetector.OnProcessSpecialSpell += ProcessSpell_LuluQ;
                GetLuluPix();
            }
        }

        private static void GetLuluPix()
        {
            var gotObj = false;

            foreach (var obj in ObjectManager.Get<Obj_AI_Minion>())
            {
                if (obj != null && obj.IsValid && obj.UnitSkinName == "lulufaerie" && obj.CheckTeam())
                {
                    gotObj = true;

                    if (!ObjectTracker.ObjTracker.ContainsKey(obj.NetworkId))
                    {
                        ObjectTracker.ObjTracker.Add(obj.NetworkId, new ObjectTrackerInfo(obj, "RobotBuddy"));
                    }
                }
            }

            if (gotObj == false)
            {
                DelayAction.Add(5000, () => GetLuluPix());
            }
        }

        private static void ProcessSpell_LuluQ(Obj_AI_Base hero, Obj_AI_BaseMissileClientDataEventArgs args, SpellData spellData, SpecialSpellEventArgs specialSpellArgs)
        {
            if (spellData.SpellName == "LuluQ")
            {
                foreach (var entry in ObjectTracker.ObjTracker)
                {
                    var info = entry.Value;

                    if (entry.Value.Name == "RobotBuddy")
                    {
                        if (info.Obj == null || !info.Obj.IsValid || info.Obj.IsDead || info.Obj.IsVisible) { }
                        else
                        {
                            var endPos2 = info.Obj.Position.Extend(args.End, spellData.Range);
                            SpellDetector.CreateSpellData(hero, info.Obj.Position, endPos2, spellData, null, 0, false);
                        }
                    }
                }
            }
        }
    }
}
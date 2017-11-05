namespace EzEvade_Port.SpecialSpells
{
    using System.Collections.Generic;
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util;
    using Helpers;
    using Spells;
    using Utils;
    using DelayAction = Utils.DelayAction;
    using SpellData = Spells.SpellData;

    class AllChampions : ChampionPlugin
    {
        public static Dictionary<string, bool> pDict = new Dictionary<string, bool>();

        static AllChampions() { }

        public void LoadSpecialSpell(SpellData spellData)
        {
            if (!pDict.ContainsKey("Game_OnWndProc"))
            {
                Game.OnWndProc += Game_OnWndProc;
                pDict["Game_OnWndProc"] = true;
            }

            if (spellData.HasTrap && !pDict.ContainsKey("GameObject_OnCreate"))
            {
                GameObject.OnCreate += GameObject_OnCreate;
                pDict["GameObject_OnCreate"] = true;
            }

            if (spellData.HasTrap && !pDict.ContainsKey("GameObject_OnDelete"))
            {
                GameObject.OnDestroy += GameObject_OnDelete;
                pDict["GameObject_OnDelete"] = true;
            }

            if (spellData.HasTrap && !pDict.ContainsKey("Game_OnUpdate"))
            {
                Game.OnUpdate += Game_OnUpdate;
                pDict["Game_OnUpdate"] = true;
            }

            if (spellData.IsThreeWay && !pDict.ContainsKey("ProcessSpell_ProcessThreeWay"))
            {
                SpellDetector.OnProcessSpecialSpell += ProcessSpell_ThreeWay;
                pDict["ProcessSpell_ProcessThreeWay"] = true;
            }
        }

        private void GameObject_OnCreate(GameObject sender)
        {
            //var emitter = sender as Obj_GeneralParticleEmitter;
            //if (emitter != null && emitter.CheckTeam())
            //{
            //    SpellData spellData;

            //    if (SpellDetector.onProcessTraps.TryGetValue(emitter.Name.ToLower(), out spellData))
            //    {
            //        var trapData = (SpellData) spellData.Clone();

            //        if (!trapData.spellName.Contains("_trap"))
            //             trapData.spellName = trapData.spellName + "_trap";

            //        SpellDetector.CreateSpellData(null, emitter.Position, emitter.Position, trapData, emitter, 1337f);
            //    }
            //}

            var aiBase = sender as Obj_AI_Base;
            if (aiBase != null && aiBase.CheckTeam())
            {
                SpellData spellData;

                if (SpellDetector.OnProcessTraps.TryGetValue(aiBase.UnitSkinName.ToLower(), out spellData))
                {
                    var trapData = (SpellData) spellData.Clone();

                    if (!trapData.SpellName.Contains("_trap"))
                    {
                        trapData.SpellName = trapData.SpellName + "_trap";
                    }

                    SpellDetector.CreateSpellData(aiBase, aiBase.ServerPosition, aiBase.ServerPosition, trapData, aiBase, 1337f);
                }
            }
        }

        private void GameObject_OnDelete(GameObject sender)
        {
            //var emitter = sender as Obj_GeneralParticleEmitter;
            //if (emitter != null && emitter.CheckTeam())
            //{
            //    SpellData spellData;

            //    if (SpellDetector.onProcessTraps.TryGetValue(emitter.Name.ToLower(), out spellData))
            //    {
            //        foreach (var entry in SpellDetector.detectedSpells.Where(x => x.Value.info.trapTroyName.ToLower() == emitter.Name.ToLower()))
            //        {
            //            DelayAction.Add(1, () => SpellDetector.DeleteSpell(entry.Key));
            //            entry.Value.spellObject = null;
            //        }
            //    }
            //}

            var aiBase = sender as Obj_AI_Base;
            if (aiBase != null && aiBase.CheckTeam())
            {
                SpellData spellData;

                if (SpellDetector.OnProcessTraps.TryGetValue(aiBase.UnitSkinName.ToLower(), out spellData))
                {
                    foreach (var entry in SpellDetector.DetectedSpells.Where(x => x.Value.Info.TrapBaseName.ToLower() == aiBase.UnitSkinName.ToLower()))
                    {
                        DelayAction.Add(1, () => SpellDetector.DeleteSpell(entry.Key));
                        entry.Value.SpellObject = null;
                    }
                }
            }
        }

        private void Game_OnUpdate()
        {
            foreach (var entry in SpellDetector.DetectedSpells.Where(x => x.Value.Info.SpellName.Contains("_trap")))
            {
                var spell = entry.Value;
                if (spell.SpellObject == null)
                {
                    continue;
                }

                if (spell.SpellObject.IsDead || !spell.SpellObject.IsValid)
                {
                    DelayAction.Add(1, () => SpellDetector.DeleteSpell(entry.Key));
                    entry.Value.SpellObject = null;
                }
            }
        }

        private void Game_OnWndProc(WndProcEventArgs e)
        {
            if (!ObjectCache.MenuCache.Cache["ClickRemove"].Enabled)
            {
                return;
            }

            if (e.Message != (uint) WindowsMessages.WM_LBUTTONDOWN)
            {
                return;
            }

            foreach (var entry in SpellDetector.DetectedSpells.Where(x => Game.CursorPos.To2D().InSkillShot(x.Value, 50 + x.Value.Info.Radius, false)))
            {
                var spell = entry.Value;
                if (spell.Info.Range > 9000 /*global*/ || spell.Info.SpellName.Contains("_trap"))
                {
                    DelayAction.Add(1, () => SpellDetector.DeleteSpell(entry.Key));
                }
            }
        }

        private static void ProcessSpell_ThreeWay(Obj_AI_Base hero, Obj_AI_BaseMissileClientDataEventArgs args, SpellData spellData, SpecialSpellEventArgs specialSpellArgs)
        {
            if (spellData.IsThreeWay)
            {
                var endPos2 = MathUtils.RotateVector(args.Start.To2D(), args.End.To2D(), spellData.Angle).To3D();
                SpellDetector.CreateSpellData(hero, args.Start, endPos2, spellData, null, 0, false);

                var endPos3 = MathUtils.RotateVector(args.Start.To2D(), args.End.To2D(), -spellData.Angle).To3D();
                SpellDetector.CreateSpellData(hero, args.Start, endPos3, spellData, null, 0, false);
            }
        }
    }
}
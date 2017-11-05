namespace EzEvade_Port.SpecialSpells
{
    using System.Collections.Generic;
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util.Cache;
    using Helpers;
    using Spells;
    using Utils;
    using SpellData = Spells.SpellData;

    class JarvanIV : ChampionPlugin
    {
        private static readonly Dictionary<float, Vector3> _eSpots = new Dictionary<float, Vector3>();

        static JarvanIV() { }

        public void LoadSpecialSpell(SpellData spellData)
        {
            if (spellData.SpellName == "JarvanIVDragonStrike")
            {
                var jarvaniv = GameObjects.Heroes.FirstOrDefault(h => h.ChampionName == "JarvanIV");
                if (jarvaniv != null && jarvaniv.CheckTeam())
                {
                    Game.OnUpdate += Game_OnUpdate;
                    Obj_AI_Base.OnProcessSpellCast += ProcessSpell_JarvanIVDemacianStandard;
                    SpellDetector.OnProcessSpecialSpell += ProcessSpell_JarvanIVDragonStrike;
                    GameObject.OnCreate += OnCreateObj_JarvanIVDragonStrike;
                    GameObject.OnDestroy += OnDeleteObj_JarvanIVDragonStrike;
                }
            }
        }

        private void Game_OnUpdate()
        {
            foreach (var spot in _eSpots.ToArray())
            {
                var flag = spot.Key;
                if (Game.ClockTime - flag >= 1.2f * 0.6f)
                {
                    _eSpots.Remove(flag);
                }
            }
        }

        private static void ProcessSpell_JarvanIVDemacianStandard(Obj_AI_Base hero, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (hero.IsEnemy && args.SpellData.Name == "JarvanIVDemacianStandard")
            {
                ObjectTracker.AddObjTrackerPosition("Beacon", args.End, 1000);
            }
        }

        private static void OnDeleteObj_JarvanIVDragonStrike(GameObject obj)
        {
            if (obj.Name == "Beacon")
            {
                ObjectTracker.ObjTracker.Remove(obj.NetworkId);
            }
        }

        private static void OnCreateObj_JarvanIVDragonStrike(GameObject obj)
        {
            if (obj.Name == "Beacon")
            {
                ObjectTracker.ObjTracker.Add(obj.NetworkId, new ObjectTrackerInfo(obj));
            }
        }

        private static void ProcessSpell_JarvanIVDragonStrike(Obj_AI_Base hero, Obj_AI_BaseMissileClientDataEventArgs args, SpellData spellData, SpecialSpellEventArgs specialSpellArgs)
        {
            if (spellData.SpellName == "JarvanIVDemacianStandard")
            {
                var end = args.End;
                if (args.Start.Distance(end) > spellData.Range)
                {
                    end = args.Start + (args.End - args.Start).Normalized() * spellData.Range;
                }

                _eSpots.Add(Game.ClockTime, end);
            }

            if (spellData.SpellName == "JarvanIVDragonStrike")
            {
                if (SpellDetector.OnProcessSpells.TryGetValue("jarvanivdragonstrike2", out spellData))
                {
                    foreach (var entry in _eSpots)
                    {
                        var flagPosition = entry.Value;

                        if (args.End.To2D().Distance(flagPosition) < 300)
                        {
                            var dir = (flagPosition.To2D() - args.Start.To2D()).Normalized();
                            var endPosition = flagPosition.To2D() + dir * 110;

                            SpellDetector.CreateSpellData(hero, args.Start, endPosition.To3D(), spellData);
                            specialSpellArgs.NoProcess = true;
                            return;
                        }
                    }

                    foreach (var entry in ObjectTracker.ObjTracker)
                    {
                        var info = entry.Value;

                        if (info.Name == "Beacon" || info.Obj.Name == "Beacon")
                        {
                            if (info.UsePosition == false && (info.Obj == null || !info.Obj.IsValid || info.Obj.IsDead))
                            {
                                DelayAction.Add(1, () => ObjectTracker.ObjTracker.Remove(info.Obj.NetworkId));
                                continue;
                            }

                            var objPosition = info.UsePosition ? info.Position.To2D() : info.Obj.Position.To2D();

                            if (args.End.To2D().Distance(objPosition) < 300)
                            {
                                var dir = (objPosition - args.Start.To2D()).Normalized();
                                var endPosition = objPosition + dir * 110;

                                SpellDetector.CreateSpellData(hero, args.Start, endPosition.To3D(), spellData);
                                specialSpellArgs.NoProcess = true;
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}
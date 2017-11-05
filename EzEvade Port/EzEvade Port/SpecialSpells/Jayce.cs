namespace EzEvade_Port.SpecialSpells
{
    using System;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Spells;
    using Utils;
    using SpellData = Spells.SpellData;

    class Jayce : ChampionPlugin
    {
        public void LoadSpecialSpell(SpellData spellData)
        {
            /*if (spellData.spellName == "JayceShockBlastWall")
            {
                Obj_AI_Hero hero = HeroManager.Enemies.FirstOrDefault(h => h.ChampionName == "Jayce");
                if (hero == null)
                {
                    return;
                }

                //Obj_AI_Minion.OnCreate += (obj, args) => OnCreateObj_jayceshockblast(obj, args, hero, spellData);
                //Obj_AI_Hero.OnProcessSpellCast += OnProcessSpell_jayceshockblast;
                //SpellDetector.OnProcessSpecialSpell += ProcessSpell_jayceshockblast;
            }*/
        }

        private static void OnCreateObj_jayceshockblast(GameObject obj, EventArgs args, Obj_AI_Hero hero, SpellData spellData)
        {
            if (obj.Type == GameObjectType.obj_GeneralParticleEmitter && obj.Name.Contains("Jayce") && obj.Name.Contains("accel_gate_end") && obj.Name.Contains("RED"))
            {
                foreach (var tracker in ObjectTracker.ObjTracker)
                {
                    var gateObj = tracker.Value;

                    if (gateObj.Name == "AccelGate")
                    {
                        DelayAction.Add(0, () => ObjectTracker.ObjTracker.Remove(tracker.Key));
                    }
                }
            }

            if (obj.Type == GameObjectType.obj_GeneralParticleEmitter && obj.Name.Contains("Jayce") && obj.Name.Contains("accel_gate_start") && obj.Name.Contains("RED"))
            {
                //var particle = obj as Obj_GeneralParticleEmitter;                

                /*var dir = obj.Orientation.To2D();
                var pos1 = obj.Position.To2D() - dir * 470;
                var pos2 = obj.Position.To2D() + dir * 470;

                //Draw.RenderObjects.Add(new Draw.RenderLine(pos1, pos2, 3500));
                Draw.RenderObjects.Add(new Draw.RenderCircle(pos1, 3500));*/

                var gateTracker = new ObjectTrackerInfo(obj, "AccelGate");
                //gateTracker.Orientation = dir.To3D();

                ObjectTracker.ObjTracker.Add(obj.NetworkId, gateTracker);

                foreach (var entry in SpellDetector.Spells) //check currently moving skillshot
                {
                    var spell = entry.Value;

                    if (spell.Info.SpellName == "JayceShockBlast")
                    {
                        var tHero = spell.HeroId;

                        Vector2 int1, int2;
                        var intersection = MathUtils.FindLineCircleIntersections(obj.Position.To2D(), 470, spell.StartPos, spell.EndPos, out int1, out int2);
                        var projection = obj.Position.To2D().ProjectOn(spell.StartPos, spell.EndPos);

                        //var intersection = spell.startPos.Intersection(spell.endPos, pos1, pos2);
                        //var projection = intersection.Point.ProjectOn(spell.startPos, spell.endPos);

                        //if (intersection.Intersects && projection.IsOnSegment)
                        if (intersection > 0 && projection.IsOnSegment)
                        {
                            SpellDetector.CreateSpellData(hero, projection.SegmentPoint.To3D(), spell.EndPos.To3D(), spellData, spell.SpellObject);

                            DelayAction.Add(1, () => SpellDetector.DeleteSpell(entry.Key));
                            break;
                        }
                    }
                }

                //SpellDetector.CreateSpellData(hero, pos1.To3D(), pos2.To3D(), spellData, null, 0);     
            }
        }

        private static void OnProcessSpell_jayceshockblast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (sender.IsEnemy && args.SpellData.Name == "jayceaccelerationgate")
            {
                //ObjectTracker.objTracker.Add(obj.NetworkId, new ObjectTrackerInfo(obj, "RobotBuddy"));

                //AddObjectTracker.objTrackerPosition
            }
        }

        private static void ProcessSpell_jayceshockblast(Obj_AI_Base hero, Obj_AI_BaseMissileClientDataEventArgs args, SpellData spellDataOld, SpecialSpellEventArgs specialSpellArgs)
        {
            if (spellDataOld.SpellName == "jayceshockblast")
            {
                SpellData spellData;

                if (SpellDetector.OnProcessSpells.TryGetValue("JayceShockBlastWall", out spellData))
                {
                    foreach (var tracker in ObjectTracker.ObjTracker)
                    {
                        var gateObj = tracker.Value;

                        if (gateObj.Name == "AccelGate")
                        {
                            if (gateObj.Obj == null)
                            {
                                DelayAction.Add(0, () => ObjectTracker.ObjTracker.Remove(tracker.Key));
                            }
                            else
                            {
                                var startPos = args.Start.To2D();
                                var endPos = args.End.To2D();
                                var dir = (endPos - startPos).Normalized();
                                endPos = startPos + dir * spellDataOld.Range;

                                var obj = gateObj.Obj;

                                Vector2 int1, int2;
                                var intersection = MathUtils.FindLineCircleIntersections(obj.Position.To2D(), 470, startPos, endPos, out int1, out int2);
                                var projection = obj.Position.To2D().ProjectOn(startPos, endPos);

                                if (intersection > 0 && projection.IsOnSegment)
                                {
                                    SpellDetector.CreateSpellData(hero, startPos.To3D(), endPos.To3D(), spellData);

                                    specialSpellArgs.NoProcess = true;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
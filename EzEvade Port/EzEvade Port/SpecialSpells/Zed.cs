namespace EzEvade_Port.SpecialSpells
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Spells;
    using Utils;
    using SpellData = Spells.SpellData;

    class Zed : ChampionPlugin
    {
        public void LoadSpecialSpell(SpellData spellData)
        {
            if (spellData.SpellName == "ZedQ")
            {
                SpellDetector.OnProcessSpecialSpell += ProcessSpell_ZedShuriken;
                GameObject.OnCreate += SpellMissile_ZedShadowDash;
                GameObject.OnCreate += OnCreateObj_ZedShuriken;
                GameObject.OnDestroy += OnDeleteObj_ZedShuriken;
            }
        }

        private static void OnCreateObj_ZedShuriken(GameObject obj)
        {
            if (obj.Name == "Shadow" && obj.IsEnemy)
            {
                if (!ObjectTracker.ObjTracker.ContainsKey(obj.NetworkId))
                {
                    ObjectTracker.ObjTracker.Add(obj.NetworkId, new ObjectTrackerInfo(obj));

                    foreach (var entry in ObjectTracker.ObjTracker)
                    {
                        var info = entry.Value;

                        if (info.Name == "Shadow" && info.UsePosition && info.Position.Distance(obj.Position) < 5)
                        {
                            info.Name = "Shadow";
                            info.UsePosition = false;
                            info.Obj = obj;
                        }
                    }
                }
            }
        }

        private static void OnDeleteObj_ZedShuriken(GameObject obj)
        {
            if (obj != null && obj.Name == "Shadow")
            {
                ObjectTracker.ObjTracker.Remove(obj.NetworkId);
            }
        }

        private static void ProcessSpell_ZedShuriken(Obj_AI_Base hero, Obj_AI_BaseMissileClientDataEventArgs args, SpellData spellData, SpecialSpellEventArgs specialSpellArgs)
        {
            if (spellData.SpellName == "ZedQ")
            {
                foreach (var entry in ObjectTracker.ObjTracker)
                {
                    var info = entry.Value;

                    if (info.Name == "Shadow")
                    {
                        if (info.UsePosition == false && (info.Obj == null || !info.Obj.IsValid || info.Obj.IsDead))
                        {
                            DelayAction.Add(1, () => ObjectTracker.ObjTracker.Remove(info.Obj.NetworkId));
                        }
                        else
                        {
                            Vector3 endPos2;
                            if (info.UsePosition == false)
                            {
                                endPos2 = info.Obj.Position.Extend(args.End, spellData.Range);
                                SpellDetector.CreateSpellData(hero, info.Obj.Position, endPos2, spellData, null, 0, false);
                            }
                            else
                            {
                                endPos2 = info.Position.Extend(args.End, spellData.Range);
                                SpellDetector.CreateSpellData(hero, info.Position, endPos2, spellData, null, 0, false);
                            }
                        }
                    }
                }
            }
        }

        private static void SpellMissile_ZedShadowDash(GameObject obj)
        {
            if (!obj.IsValid && obj.Type == GameObjectType.MissileClient)
            {
                return;
            }

            var missile = (MissileClient) obj;

            if (missile.SpellCaster.IsEnemy && missile.SpellData.Name == "ZedWMissile")
            {
                if (!ObjectTracker.ObjTracker.ContainsKey(obj.NetworkId))
                {
                    var info = new ObjectTrackerInfo(obj);
                    info.Name = "Shadow";
                    info.OwnerNetworkId = missile.SpellCaster.NetworkId;
                    info.UsePosition = true;
                    info.Position = missile.EndPosition;

                    ObjectTracker.ObjTracker.Add(obj.NetworkId, info);

                    DelayAction.Add(1000, () => ObjectTracker.ObjTracker.Remove(obj.NetworkId));
                }
            }
        }
    }
}
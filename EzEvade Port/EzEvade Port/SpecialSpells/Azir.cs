namespace EzEvade_Port.SpecialSpells
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util.Cache;
    using Helpers;
    using Spells;
    using Utils;
    using SpellData = Spells.SpellData;

    class Azir : ChampionPlugin
    {
        public void LoadSpecialSpell(SpellData spellData)
        {
            if (spellData.SpellName == "AzirQWrapper")
            {
                var hero = GameObjects.Heroes.FirstOrDefault(h => h.ChampionName == "Azir");
                if (hero == null || !hero.CheckTeam())
                {
                    return;
                }

                var info = new ObjectTrackerInfo(hero) {Name = "AzirQSoldier", OwnerNetworkId = hero.NetworkId};

                ObjectTracker.ObjTracker.Add(hero.NetworkId, info);
                GameObject.OnCreate += OnCreateObj_AzirSoldier;
                GameObject.OnDestroy += OnDeleteObj_AzirSoldier;
                SpellDetector.OnProcessSpecialSpell += ProcessSpell_AzirSoldier;
            }
        }

        private static void OnCreateObj_AzirSoldier(GameObject obj)
        {
            if (obj.Name == "AzirSoldier")
            {
                foreach (var entry in ObjectTracker.ObjTracker)
                {
                    var info = entry.Value;
                    if (info.Name == "AzirQSoldier")
                    {
                        info.UsePosition = false;
                        info.ObjList.Add(obj.NetworkId, obj);

                        DelayAction.Add(8900,
                                        () =>
                                        {
                                            if (info.ObjList.ContainsKey(obj.NetworkId))
                                            {
                                                info.ObjList.Remove(obj.NetworkId);
                                            }
                                        });
                    }
                }
            }
        }

        private static void OnDeleteObj_AzirSoldier(GameObject obj)
        {
            if (obj.Name == "AzirSoldier")
            {
                foreach (var entry in ObjectTracker.ObjTracker)
                {
                    var info = entry.Value;
                    if (info.Name == "AzirQSoldier")
                    {
                        if (info.ObjList.ContainsKey(obj.NetworkId))
                        {
                            info.ObjList.Remove(obj.NetworkId);
                        }
                    }
                }
            }
        }

        private static void ProcessSpell_AzirSoldier(Obj_AI_Base hero, Obj_AI_BaseMissileClientDataEventArgs args, SpellData spellData, SpecialSpellEventArgs specialSpellArgs)
        {
            if (spellData.SpellName == "AzirQWrapper")
            {
                foreach (var entry in ObjectTracker.ObjTracker)
                {
                    var info = entry.Value;
                    if (info.Name == "AzirQSoldier")
                    {
                        foreach (var objEntry in info.ObjList)
                        {
                            var soldier = objEntry.Value;
                            if (soldier == null || !soldier.IsValid || soldier.IsDead)
                            {
                                continue;
                            }

                            var maxMenuSliderange = 875 + hero.Distance(soldier.Position);
                            var start = soldier.Position;
                            var end = args.End;

                            if (start.Distance(end) > maxMenuSliderange)
                            {
                                end = start + (end - start).Normalized() * maxMenuSliderange;
                            }

                            SpellDetector.CreateSpellData(hero, start, end, spellData, soldier);
                        }
                    }
                }

                specialSpellArgs.NoProcess = true;
            }
        }
    }
}
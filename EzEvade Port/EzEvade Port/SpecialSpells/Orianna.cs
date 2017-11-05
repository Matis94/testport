namespace EzEvade_Port.SpecialSpells
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Util.Cache;
    using Helpers;
    using Spells;
    using SpellData = Spells.SpellData;

    class Orianna : ChampionPlugin
    {
        public void LoadSpecialSpell(SpellData spellData)
        {
            if (spellData.SpellName == "OrianaIzunaCommand")
            {
                var hero = GameObjects.Heroes.FirstOrDefault(h => h.ChampionName == "Orianna");
                if (hero != null && hero.CheckTeam())
                {
                    var info = new ObjectTrackerInfo(hero) {Name = "TheDoomBall", OwnerNetworkId = hero.NetworkId};

                    ObjectTracker.ObjTracker.Add(hero.NetworkId, info);

                    GameObject.OnCreate += obj => OnCreateObj_OrianaIzunaCommand(obj, hero);
                    //Obj_AI_Minion.OnDelete += (obj, args) => OnDeleteObj_OrianaIzunaCommand(obj, args, hero);
                    Obj_AI_Base.OnProcessSpellCast += ProcessSpell_OrianaRedactCommand;
                    SpellDetector.OnProcessSpecialSpell += ProcessSpell_OrianaIzunaCommand;
                    BuffManager.OnAddBuff += Obj_AI_Base_OnBuffAdd;
                }
            }
        }

        private void Obj_AI_Base_OnBuffAdd(Obj_AI_Base sender, Buff buff)
        {
            var hero = sender as Obj_AI_Hero;
            if (hero != null && hero.CheckTeam())
            {
                if (buff.Name == "orianaghostself")
                {
                    foreach (var entry in ObjectTracker.ObjTracker)
                    {
                        var info = entry.Value;
                        if (entry.Value.Name == "TheDoomBall")
                        {
                            info.UsePosition = false;
                            info.Obj = hero;
                        }
                    }
                }
            }
        }

        private static void OnCreateObj_OrianaIzunaCommand(GameObject obj, Obj_AI_Hero hero)
        {
            if (obj.Name.Contains("Orianna") && obj.Name.Contains("Ball_Flash_Reverse") && obj.CheckTeam())
            {
                foreach (var entry in ObjectTracker.ObjTracker)
                {
                    var info = entry.Value;
                    if (entry.Value.Name == "TheDoomBall")
                    {
                        info.UsePosition = false;
                        info.Obj = hero;
                    }
                }
            }
        }

        private static void OnDeleteObj_OrianaIzunaCommand(GameObject obj, Obj_AI_Hero hero)
        {
            if (obj.Name.Contains("Orianna") && obj.Name.Contains("ball_glow_red") && obj.CheckTeam())
            {
                foreach (var entry in ObjectTracker.ObjTracker)
                {
                    var info = entry.Value;

                    if (entry.Value.Name == "TheDoomBall")
                    {
                        info.UsePosition = false;
                        info.Obj = hero;
                    }
                }
            }
        }

        private static void ProcessSpell_OrianaRedactCommand(Obj_AI_Base hero, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!hero.IsValid && hero.Type == GameObjectType.obj_AI_Hero)
            {
                return;
            }

            var champ = (Obj_AI_Hero) hero;

            if (champ.ChampionName == "Orianna" && champ.CheckTeam())
            {
                if (args.SpellData.Name == "OrianaRedactCommand")
                {
                    foreach (var entry in ObjectTracker.ObjTracker)
                    {
                        var info = entry.Value;

                        if (entry.Value.Name == "TheDoomBall")
                        {
                            info.UsePosition = false;
                            info.Obj = args.Target;
                        }
                    }
                }
            }
        }

        private static void ProcessSpell_OrianaIzunaCommand(Obj_AI_Base hero, Obj_AI_BaseMissileClientDataEventArgs args, SpellData spellData, SpecialSpellEventArgs specialSpellArgs)
        {
            if (spellData.SpellName == "OrianaIzunaCommand")
            {
                foreach (var entry in ObjectTracker.ObjTracker)
                {
                    var info = entry.Value;

                    if (entry.Value.Name == "TheDoomBall")
                    {
                        if (info.UsePosition)
                        {
                            SpellDetector.CreateSpellData(hero, info.Position, args.End, spellData, null, 0, false);
                            SpellDetector.CreateSpellData(hero, info.Position, args.End, spellData, null, 150, true, SpellType.Circular, false, spellData.SecondaryRadius);
                        }
                        else
                        {
                            if (info.Obj != null && info.Obj.IsValid && !info.Obj.IsDead)
                            {
                                SpellDetector.CreateSpellData(hero, info.Obj.Position, args.End, spellData, null, 0, false);
                                SpellDetector.CreateSpellData(hero, info.Obj.Position, args.End, spellData, null, 150, true, SpellType.Circular, false, spellData.SecondaryRadius);
                            }
                        }

                        info.Position = args.End;
                        info.UsePosition = true;
                    }
                }

                specialSpellArgs.NoProcess = true;
            }

            if (spellData.SpellName == "OrianaDetonateCommand" || spellData.SpellName == "OrianaDissonanceCommand")
            {
                foreach (var entry in ObjectTracker.ObjTracker)
                {
                    var info = entry.Value;

                    if (entry.Value.Name == "TheDoomBall")
                    {
                        if (info.UsePosition)
                        {
                            var endPos2 = info.Position;
                            SpellDetector.CreateSpellData(hero, endPos2, endPos2, spellData);
                        }
                        else
                        {
                            if (info.Obj != null && info.Obj.IsValid && !info.Obj.IsDead)
                            {
                                var endPos2 = info.Obj.Position;
                                SpellDetector.CreateSpellData(hero, endPos2, endPos2, spellData);
                            }
                        }
                    }
                }

                specialSpellArgs.NoProcess = true;
            }
        }
    }
}
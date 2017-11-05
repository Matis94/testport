namespace EzEvade_Port.EvadeSpells
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Events;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Menu.Components;
    using Core;
    using Helpers;
    using Spells;
    using Utils;
    using Spell = Spells.Spell;

    class EvadeSpell
    {
        public delegate void Callback();

        public static List<EvadeSpellData> EvadeSpells = new List<EvadeSpellData>();
        public static List<EvadeSpellData> ItemSpells = new List<EvadeSpellData>();

        public static EvadeCommand LastSpellEvadeCommand = new EvadeCommand {IsProcessed = true, Timestamp = Environment.TickCount};

        public static Menu Menu;

        public EvadeSpell(Menu mainMenu)
        {
            Menu = mainMenu;

            Evade.EvadeSpellMenu = new Menu("EvadeSpells", "Evade Spells");
            Menu.Add(Evade.EvadeSpellMenu);

            LoadEvadeSpellList();
            DelayAction.Add(100, CheckForItems);
        }

        private static Obj_AI_Hero MyHero => ObjectManager.GetLocalPlayer();

        public static void CheckDashing()
        {
            if (!(Environment.TickCount - LastSpellEvadeCommand.Timestamp < 250) || !MyHero.IsDashing() || LastSpellEvadeCommand.EvadeSpellData.EvadeType != EvadeType.Dash)
            {
                return;
            }
            var dashInfo = MyHero.GetDashInfo();

            LastSpellEvadeCommand.TargetPosition = dashInfo.EndPos;
        }

        private static void CheckForItems()
        {
            foreach (var spell in ItemSpells)
            {
                var hasItem = MyHero.HasItem(spell.ItemId);

                if (!hasItem || EvadeSpells.Exists(s => s.SpellName == spell.SpellName))
                {
                    continue;
                }

                EvadeSpells.Add(spell);

                var newSpellMenu = CreateEvadeSpellMenu(spell);
                Evade.Menu.Add(newSpellMenu);
            }

            DelayAction.Add(5000, CheckForItems);
        }

        private static Menu CreateEvadeSpellMenu(EvadeSpellData spell)
        {
            var menuName = spell.Name + " (" + spell.SpellKey + ") Settings";

            if (spell.IsItem)
            {
                menuName = spell.Name + " Settings";
            }

            var newSpellMenu = new Menu(spell.CharName + spell.Name + "EvadeSpellSettings", menuName)
            {
                new MenuBool(spell.Name + "UseEvadeSpell", "Use Spell"),
                new MenuList(spell.Name + "EvadeSpellDangerLevel", "Danger Level", new[] {"Low", "Normal", "High", "Extreme"}, spell.Dangerlevel - 1),
                new MenuList(spell.Name + "EvadeSpellMode", "Spell Mode", new[] {"Undodgeable", "Activation Time", "Always"}, GetDefaultSpellMode(spell))
            };

            Evade.EvadeSpellMenu.Add(newSpellMenu);
            ObjectCache.MenuCache.AddMenuToCache(newSpellMenu);

            return newSpellMenu;
        }

        public static int GetDefaultSpellMode(EvadeSpellData spell)
        {
            if (spell.Dangerlevel > 3)
            {
                return 0;
            }

            return 1;
        }

        public static bool PreferEvadeSpell()
        {
            if (!Situation.ShouldUseEvadeSpell())
            {
                return false;
            }

            return SpellDetector.Spells.Select(entry => entry.Value).
                Where(spell => ObjectCache.MyHeroCache.ServerPos2D.InSkillShot(spell, ObjectCache.MyHeroCache.BoundingRadius)).
                Any(spell => ActivateEvadeSpell(spell, true));
        }

        public static void UseEvadeSpell()
        {
            if (!Situation.ShouldUseEvadeSpell())
            {
                return;
            }

            if (Environment.TickCount - LastSpellEvadeCommand.Timestamp < 1000)
            {
                return;
            }

            if (!SpellDetector.Spells.Select(entry => entry.Value).Where(ShouldActivateEvadeSpell).Any(spell => ActivateEvadeSpell(spell)))
            {
                return;
            }
            Evade.SetAllUndodgeable();
        }

        public static bool ActivateEvadeSpell(Spell spell, bool checkSpell = false)
        {
            if (spell.Info.SpellName.Contains("_trap"))
            {
                return false;
            }

            var sortedEvadeSpells = EvadeSpells.OrderBy(s => s.Dangerlevel);

            var extraDelayBuffer = ObjectCache.MenuCache.Cache["ExtraPingBuffer"].As<MenuSlider>().Value;
            var spellActivationTime = ObjectCache.MenuCache.Cache["SpellActivationTime"].As<MenuSlider>().Value + ObjectCache.GamePing + extraDelayBuffer;

            if (ObjectCache.MenuCache.Cache["CalculateWindupDelay"].Enabled)
            {
                var extraWindupDelay = Evade.LastWindupTime - Environment.TickCount;
                if (extraWindupDelay > 0)
                {
                    return false;
                }
            }

            foreach (var evadeSpell in sortedEvadeSpells)
            {
                var processSpell = true;

                if (!Evade.EvadeSpellMenu[evadeSpell.CharName + evadeSpell.Name + "EvadeSpellSettings"][evadeSpell.Name + "UseEvadeSpell"].Enabled ||
                    GetSpellDangerLevel(evadeSpell) > spell.GetSpellDangerLevel() || !MyHero.SpellBook.CanUseSpell(evadeSpell.SpellKey) ||
                    evadeSpell.CheckSpellName && MyHero.SpellBook.GetSpell(evadeSpell.SpellKey).Name != evadeSpell.SpellName)
                {
                    continue;
                }

                spell.CanHeroEvade(MyHero, out var evadeTime, out var spellHitTime);

                var finalEvadeTime = spellHitTime - evadeTime;

                if (checkSpell)
                {
                    var mode = Evade.EvadeSpellMenu[evadeSpell.CharName + evadeSpell.Name + "EvadeSpellSettings"][evadeSpell.Name + "EvadeSpellMode"].As<MenuList>().Value;

                    switch (mode)
                    {
                        case 0: continue;
                        case 1:
                            if (spellActivationTime < finalEvadeTime)
                            {
                                continue;
                            }
                            break;
                    }
                }
                else
                {
                    if (evadeSpell.SpellDelay <= 50 && evadeSpell.EvadeType != EvadeType.Dash)
                    {
                        var path = MyHero.Path;
                        if (path.Length > 0)
                        {
                            var movePos = path[path.Length - 1].To2D();
                            var posInfo = EvadeHelper.CanHeroWalkToPos(movePos, ObjectCache.MyHeroCache.MoveSpeed, 0, 0);

                            if (GetSpellDangerLevel(evadeSpell) > posInfo.PosDangerLevel)
                            {
                                continue;
                            }
                        }
                    }
                }

                if (evadeSpell.EvadeType != EvadeType.Dash && spellHitTime > evadeSpell.SpellDelay + 100 + Game.Ping + ObjectCache.MenuCache.Cache["ExtraPingBuffer"].As<MenuSlider>().Value)
                {
                    processSpell = false;

                    if (checkSpell == false)
                    {
                        continue;
                    }
                }

                if (evadeSpell.IsSpecial)
                {
                    if (evadeSpell.UseSpellFunc == null)
                    {
                        continue;
                    }
                    if (evadeSpell.UseSpellFunc(evadeSpell, processSpell))
                    {
                        return true;
                    }
                }
                else
                {
                    switch (evadeSpell.EvadeType)
                    {
                        case EvadeType.Blink:
                            if (evadeSpell.CastType == CastType.Position)
                            {
                                var posInfo = EvadeHelper.GetBestPositionBlink();
                                if (posInfo != null)
                                {
                                    if (processSpell)
                                    {
                                        MyHero.SpellBook.CastSpell(evadeSpell.SpellKey, posInfo.Position.To3D());
                                    }
                                    //CastEvadeSpell(() => EvadeCommand.CastSpell(evadeSpell, posInfo.position), processSpell);
                                    //DelayAction.Add(50, () => myHero.IssueOrder(OrderType.MoveTo, posInfo.position.To3D()));
                                    return true;
                                }
                            }
                            else if (evadeSpell.CastType == CastType.Target)
                            {
                                var posInfo = EvadeHelper.GetBestPositionTargetedDash(evadeSpell);
                                if (posInfo != null && posInfo.Target != null && posInfo.PosDangerLevel == 0)
                                {
                                    if (processSpell)
                                    {
                                        MyHero.SpellBook.CastSpell(evadeSpell.SpellKey, posInfo.Target);
                                    }
                                    //CastEvadeSpell(() => EvadeCommand.CastSpell(evadeSpell, posInfo.target), processSpell);
                                    //DelayAction.Add(50, () => myHero.IssueOrder(OrderType.MoveTo, posInfo.position.To3D()));
                                    return true;
                                }
                            }
                            break;
                        case EvadeType.Dash:
                            if (evadeSpell.CastType == CastType.Position)
                            {
                                var posInfo = EvadeHelper.GetBestPositionDash(evadeSpell);
                                if (posInfo != null && CompareEvadeOption(posInfo, checkSpell))
                                {
                                    if (evadeSpell.IsReversed)
                                    {
                                        var dir = (posInfo.Position - ObjectCache.MyHeroCache.ServerPos2D).Normalized();
                                        var range = ObjectCache.MyHeroCache.ServerPos2D.Distance(posInfo.Position);
                                        var pos = ObjectCache.MyHeroCache.ServerPos2D - dir * range;

                                        posInfo.Position = pos;
                                    }

                                    if (processSpell)
                                    {
                                        MyHero.SpellBook.CastSpell(evadeSpell.SpellKey, posInfo.Position.To3D());
                                    }

                                    return true;
                                }
                            }
                            else if (evadeSpell.CastType == CastType.Target)
                            {
                                var posInfo = EvadeHelper.GetBestPositionTargetedDash(evadeSpell);
                                if (posInfo != null && posInfo.Target != null && posInfo.PosDangerLevel == 0)
                                {
                                    if (processSpell)
                                    {
                                        MyHero.SpellBook.CastSpell(evadeSpell.SpellKey, posInfo.Target);
                                    }

                                    return true;
                                }
                            }
                            break;
                        case EvadeType.WindWall:
                            if (spell.HasProjectile() || evadeSpell.SpellName == "FioraW")
                            {
                                var dir = (spell.StartPos - ObjectCache.MyHeroCache.ServerPos2D).Normalized();
                                var pos = ObjectCache.MyHeroCache.ServerPos2D + dir * 100;

                                if (processSpell)
                                {
                                    MyHero.SpellBook.CastSpell(evadeSpell.SpellKey, pos.To3D());
                                }

                                return true;
                            }
                            break;
                        case EvadeType.SpellShield:
                            if (evadeSpell.IsItem)
                            {
                                if (processSpell)
                                {
                                    MyHero.SpellBook.CastSpell(evadeSpell.SpellKey);
                                }
                                //CastEvadeSpell(() => myHero.SpellBook.CastSpell(evadeSpell.spellKey), processSpell);
                                return true;
                            }

                            switch (evadeSpell.CastType)
                            {
                                case CastType.Target:
                                    if (processSpell)
                                    {
                                        MyHero.SpellBook.CastSpell(evadeSpell.SpellKey, MyHero);
                                    }
                                    // CastEvadeSpell(() => EvadeCommand.CastSpell(evadeSpell, myHero), processSpell);
                                    return true;
                                case CastType.Self:
                                    if (processSpell)
                                    {
                                        MyHero.SpellBook.CastSpell(evadeSpell.SpellKey);
                                    }
                                    //CastEvadeSpell(() => EvadeCommand.CastSpell(evadeSpell), processSpell);
                                    return true;
                            }

                            break;
                        case EvadeType.MovementSpeedBuff:
                            if (evadeSpell.IsItem)
                            {
                                var posInfo = EvadeHelper.GetBestPosition();
                                if (posInfo != null)
                                {
                                    if (processSpell)
                                    {
                                        MyHero.SpellBook.CastSpell(evadeSpell.SpellKey);
                                    }
                                    //CastEvadeSpell(() => myHero.SpellBook.CastSpell(evadeSpell.spellKey), processSpell);
                                    DelayAction.Add(5, () => EvadeCommand.MoveTo(posInfo.Position));
                                    return true;
                                }
                            }
                            else
                            {
                                switch (evadeSpell.CastType)
                                {
                                    case CastType.Self:
                                    {
                                        var posInfo = EvadeHelper.GetBestPosition();
                                        if (posInfo != null)
                                        {
                                            if (processSpell)
                                            {
                                                MyHero.SpellBook.CastSpell(evadeSpell.SpellKey);
                                            }
                                            //CastEvadeSpell(() => EvadeCommand.CastSpell(evadeSpell), processSpell);
                                            DelayAction.Add(5, () => EvadeCommand.MoveTo(posInfo.Position));
                                            return true;
                                        }
                                        break;
                                    }
                                    case CastType.Position:
                                    {
                                        var posInfo = EvadeHelper.GetBestPosition();
                                        if (posInfo != null)
                                        {
                                            if (processSpell)
                                            {
                                                MyHero.SpellBook.CastSpell(evadeSpell.SpellKey, posInfo.Position.To3D());
                                            }
                                            //CastEvadeSpell(() => EvadeCommand.CastSpell(evadeSpell, posInfo.position), processSpell);
                                            DelayAction.Add(5, () => EvadeCommand.MoveTo(posInfo.Position));
                                            return true;
                                        }
                                        break;
                                    }
                                    case CastType.Target: break;
                                    default: throw new ArgumentOutOfRangeException();
                                }
                            }
                            break;
                    }
                }
            }

            return false;
        }

        public static void CastEvadeSpell(Callback func, bool process = true)
        {
            if (process)
            {
                func();
            }
        }

        public static bool CompareEvadeOption(PositionInfo posInfo, bool checkSpell = false)
        {
            if (!checkSpell)
            {
                return posInfo.IsBetterMovePos();
            }
            if (posInfo.PosDangerLevel == 0)
            {
                return true;
            }

            return posInfo.IsBetterMovePos();
        }

        private static bool ShouldActivateEvadeSpell(Spell spell)
        {
            if (Evade.LastPosInfo == null)
            {
                return false;
            }

            if (ObjectCache.MenuCache.Cache["DodgeSkillShots"].As<MenuKeyBind>().Enabled)
            {
                if (Evade.LastPosInfo.UndodgeableSpells.Contains(spell.SpellId) && ObjectCache.MyHeroCache.ServerPos2D.InSkillShot(spell, ObjectCache.MyHeroCache.BoundingRadius))
                {
                    return true;
                }
            }
            else
            {
                if (ObjectCache.MyHeroCache.ServerPos2D.InSkillShot(spell, ObjectCache.MyHeroCache.BoundingRadius))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool ShouldUseMovementBuff(Spell spell)
        {
            var sortedEvadeSpells = EvadeSpells.Where(s => s.EvadeType == EvadeType.MovementSpeedBuff).OrderBy(s => s.Dangerlevel);

            return sortedEvadeSpells.All(evadeSpell => Evade.EvadeSpellMenu[evadeSpell.CharName + evadeSpell.Name + "EvadeSpellSettings"][evadeSpell.Name + "UseEvadeSpell"].Enabled &&
                                                       GetSpellDangerLevel(evadeSpell) <= spell.GetSpellDangerLevel() && (evadeSpell.IsItem || !MyHero.SpellBook.CanUseSpell(evadeSpell.SpellKey)) &&
                                                       (!evadeSpell.IsItem || MyHero.SpellBook.CanUseSpell(evadeSpell.SpellKey)) &&
                                                       (!evadeSpell.CheckSpellName || MyHero.SpellBook.GetSpell(evadeSpell.SpellKey).Name == evadeSpell.SpellName));
        }

        public static int GetSpellDangerLevel(EvadeSpellData spell)
        {
            var dangerStr = Evade.EvadeSpellMenu[spell.CharName + spell.Name + "EvadeSpellSettings"][spell.Name + "EvadeSpellDangerLevel"].As<MenuList>().SelectedItem;

            var dangerlevel = 1;

            switch (dangerStr)
            {
                case "Low":
                    dangerlevel = 1;
                    break;
                case "High":
                    dangerlevel = 3;
                    break;
                case "Extreme":
                    dangerlevel = 4;
                    break;
                default:
                    dangerlevel = 2;
                    break;
            }

            return dangerlevel;
        }

        private SpellSlot GetSummonerSlot(string spellName)
        {
            if (MyHero.SpellBook.GetSpell(SpellSlot.Summoner1).Name == spellName)
            {
                return SpellSlot.Summoner1;
            }
            if (MyHero.SpellBook.GetSpell(SpellSlot.Summoner2).SpellData.Name == spellName)
            {
                return SpellSlot.Summoner2;
            }

            return SpellSlot.Unknown;
        }

        private void LoadEvadeSpellList()
        {
            foreach (var spell in EvadeSpellDatabase.Spells.Where(s => s.CharName == MyHero.ChampionName || s.CharName == "AllChampions"))
            {
                if (spell.IsSummonerSpell)
                {
                    var spellKey = GetSummonerSlot(spell.SpellName);
                    if (spellKey == SpellSlot.Unknown)
                    {
                        continue;
                    }

                    spell.SpellKey = spellKey;
                }

                if (spell.IsItem)
                {
                    ItemSpells.Add(spell);
                    continue;
                }

                if (spell.IsSpecial)
                {
                    SpecialEvadeSpell.LoadSpecialSpell(spell);
                }

                EvadeSpells.Add(spell);

                CreateEvadeSpellMenu(spell);
            }

            EvadeSpells.Sort((a, b) => a.Dangerlevel.CompareTo(b.Dangerlevel));
        }
    }
}
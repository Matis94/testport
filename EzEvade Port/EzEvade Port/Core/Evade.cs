namespace EzEvade_Port.Core
{
    using System;
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Menu.Components;
    using Aimtec.SDK.Orbwalking;
    using Aimtec.SDK.Util;
    using EvadeSpells;
    using Helpers;
    using Spells;
    using Tests;
    using Utils;

    class Evade
    {
        public static SpellDetector SpellDetector;
        private static PingTester _pingTester;
        private static SpellTester _spellTester;

        public static SpellSlot LastSpellCast;
        public static float LastSpellCastTime;

        public static float LastWindupTime;

        public static float LastTickCount;
        public static float LastStopEvadeTime;

        public static Vector3 LastMovementBlockPos = Vector3.Zero;
        public static float LastMovementBlockTime;

        public static float LastEvadeOrderTime;
        public static float LastIssueOrderGameTime;
        public static float LastIssueOrderTime;
        public static Obj_AI_BaseIssueOrderEventArgs LastIssueOrderArgs;

        public static Vector2 LastMoveToPosition = Vector2.Zero;
        public static Vector2 LastMoveToServerPos = Vector2.Zero;
        public static Vector2 LastStopPosition = Vector2.Zero;

        public static DateTime AssemblyLoadTime = DateTime.Now;

        public static bool IsDodging;
        public static bool DodgeOnlyDangerous;

        public static bool DevModeOn = false;
        public static bool HasGameEnded;
        public static bool IsChanneling;
        public static Vector2 ChannelPosition = Vector2.Zero;

        public static PositionInfo LastPosInfo;

        public static EvadeCommand LastEvadeCommand = new EvadeCommand {IsProcessed = true, Timestamp = Environment.TickCount};

        public static EvadeCommand LastBlockedUserMoveTo = new EvadeCommand {IsProcessed = true, Timestamp = Environment.TickCount};

        public static float LastDodgingEndTime;

        public static Menu Menu, MiscMenu, KeyMenu, MainMenu, LimiterMenu, BufferMenu, FastEvadeMenu, SpellMenu, DrawMenu, AutoSetPingMenu, EvadeSpellMenu;

        public static float SumCalculationTime = 0;
        public static float NumCalculationTime = 0;
        public static float AvgCalculationTime = 0;

        public Evade()
        {
            if (Game.Mode == GameMode.Running)
            {
                Game_OnGameLoad();
            }
            else
            {
                Game.OnStart += Game_OnGameLoad;
            }
        }

        public static Obj_AI_Hero MyHero => ObjectManager.GetLocalPlayer();

        private void Game_OnGameLoad()
        {
            try
            {
                Obj_AI_Base.OnIssueOrder += Game_OnIssueOrder;
                SpellBook.OnCastSpell += Game_OnCastSpell;
                Game.OnUpdate += Game_OnGameUpdate;

                Obj_AI_Base.OnProcessSpellCast += Game_OnProcessSpell;

                Game.OnEnd += Game_OnGameEnd;
                SpellDetector.OnProcessDetectedSpells += SpellDetector_OnProcessDetectedSpells;
                var orbwalkerInst = Orbwalker.OrbwalkerInstances.FirstOrDefault();
                if (orbwalkerInst != null)
                {
                    orbwalkerInst.PreAttack += Orbwalker_PreAttack;
                }

                Menu = new Menu("ezevadeeeeeeeee", "EzEvade", true);

                MainMenu = new Menu("MainMenu", "Main Menu")
                {
                    new MenuKeyBind("DodgeSkillShots", "Dodge SkillShots", KeyCode.K, KeybindType.Toggle),
                    new MenuBool("DodgeDangerous", "Dodge Only Dangerous", false),
                    new MenuBool("DodgeCircularSpells", "Dodge Circular Spells"),
                    new MenuKeyBind("ActivateEvadeSpells", "Activate Evade Spells", KeyCode.K, KeybindType.Toggle),
                    new MenuBool("DodgeFOWSpells", "Dodge FOW Spells")
                };
                Menu.Add(MainMenu);

                KeyMenu = new Menu("KeyMenu", "Key Menu")
                {
                    new MenuBool("DodgeOnlyOnComboKeyEnabled", "Dodge Only On Combo Key Enabled", false),
                    new MenuKeyBind("DodgeComboKey", "Dodge Combo Key", KeyCode.Space, KeybindType.Press),
                    new MenuBool("DodgeDangerousKeyEnabled", "Enable Dodge Only Dangerous Keys", false),
                    new MenuKeyBind("DodgeDangerousKey", "Dodge Only Dangerous Key", KeyCode.Space, KeybindType.Press),
                    new MenuKeyBind("DodgeDangerousKey2", "Dodge Only Dangerous Key 2", KeyCode.V, KeybindType.Press),
                    new MenuBool("DontDodgeKeyEnabled", "Dont Dodge Key Enabled", false),
                    new MenuKeyBind("DontDodgeKey", "Dodge Combo Key", KeyCode.Z, KeybindType.Press)
                };
                Menu.Add(KeyMenu);

                var loadTestMenu = new Menu("LoadTests", "Tests") {new MenuBool("LoadPingTester", "Load Ping Tester", false), new MenuBool("LoadSpellTester", "Load Spell Tester", false)};

                loadTestMenu["LoadPingTester"].OnValueChanged += OnLoadPingTesterChange;
                loadTestMenu["LoadSpellTester"].OnValueChanged += OnLoadSpellTesterChange;

                MiscMenu = new Menu("MiscMenu", "Misc Menu")
                {
                    new MenuBool("HigherPrecision", "Higher Precision"),
                    new MenuBool("RecalculatePosition", "Recalculate Position"),
                    new MenuBool("ContinueMovement", "Continue Previous Movement"),
                    new MenuBool("ClickRemove", "Click Remove"),
                    new MenuBool("CalculateWindupDelay", "Calculate Windup Delay"),
                    new MenuBool("AdvancedSpellDetection", "Advanced Spell Detection"),
                    new MenuBool("CheckSpellCollision", "Check Spell Collision"),
                    new MenuList("EvadeMode", "Evade Profile", new[] {"Smooth", "Very Smooth", "Fastest", "Hawk", "Kurisu", "GuessWho"}, 0),
                    new MenuBool("PreventDodgingUnderTower", "Prevent Dodging Under Tower"),
                    new MenuBool("PreventDodgingNearEnemy", "Prevent Dodging Near Enemy"),
                    loadTestMenu
                };
                Menu.Add(MiscMenu);

                MiscMenu["EvadeMode"].OnValueChanged += OnEvadeModeChange;

                BufferMenu = new Menu("BufferMenu", "Buffer Menu")
                {
                    new MenuSlider("ExtraSpellRadius", "Extra Spell Radius", 0),
                    new MenuSlider("ExtraPingBuffer", "Extra Ping Buffer", 65, 0, 200),
                    new MenuSlider("ExtraAvoidDistance", "Extra Avoid Distance", 50, 0, 300),
                    new MenuSlider("ExtraEvadeDistance", "Extra Evade Distance", 100, 0, 300),
                    new MenuSlider("ExtraCPADistance", "Extra Collision Distance", 10, 0, 150),
                    new MenuSlider("MinComfortZone", "Min Distance to Champion", 550, 0, 1000)
                };

                Menu.Add(BufferMenu);

                LimiterMenu = new Menu("LimiterMenu", "Humanizer Menu")
                {
                    new MenuSlider("SpellDetectionTime", "Spell Detection Time", 0, 0, 1000),
                    new MenuSlider("ReactionTime", "Reaction Time", 0, 0, 500),
                    new MenuSlider("DodgeInterval", "Dodge Interval Time", 0, 0, 2000),
                    new MenuSlider("TickLimiter", "Tick Limiter", 100, 0, 500),
                    new MenuBool("EnableEvadeDistance", "Extended Evade"),
                    new MenuBool("ClickOnlyOnce", "Only Click Once")
                };
                Menu.Add(LimiterMenu);

                FastEvadeMenu = new Menu("FastEvade", "Fast Evade Menu")
                {
                    new MenuBool("FastMovementBlock", "Fast Movement Block"),
                    new MenuSlider("FastEvadeActivationTime", "FastEvade Activation Time", 65, 0, 500),
                    new MenuSlider("SpellActivationTime", "Spell Activation Time", 400, 0, 1000),
                    new MenuSlider("RejectMinDistance", "Collision Distance Buffer", 10)
                };
                Menu.Add(FastEvadeMenu);

                SpellDetector = new SpellDetector(Menu);
                new EvadeSpell(Menu);

                Menu.Attach();

                new SpellDrawer(Menu);

                if (DevModeOn)
                {
                    var evadeTester = new Menu("ezevade: Test", "ezEvadeTest", true);
                    var o = new EvadeTester(evadeTester);
                    evadeTester.Attach();
                }

                Console.WriteLine("ezevade Loaded");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void OnEvadeModeChange(MenuComponent sender, ValueChangedArgs e)
        {
            var mode = e.GetNewValue<MenuList>().SelectedItem;

            switch (mode)
            {
                case "Fastest":
                    Menu["FastMovementBlock"].As<MenuBool>().Value = true;
                    Menu["FastEvadeActivationTime"].As<MenuSlider>().Value = 120;
                    Menu["RejectMinDistance"].As<MenuSlider>().Value = 25;
                    Menu["ExtraCPADistance"].As<MenuSlider>().Value = 25;
                    Menu["ExtraPingBuffer"].As<MenuSlider>().Value = 80;
                    Menu["TickLimiter"].As<MenuSlider>().Value = 100;
                    Menu["SpellDetectionTime"].As<MenuSlider>().Value = 0;
                    Menu["ReactionTime"].As<MenuSlider>().Value = 0;
                    Menu["DodgeInterval"].As<MenuSlider>().Value = 0;
                    break;
                case "Very Smooth":
                    Menu["FastEvadeActivationTime"].As<MenuSlider>().Value = 0;
                    Menu["RejectMinDistance"].As<MenuSlider>().Value = 0;
                    Menu["ExtraCPADistance"].As<MenuSlider>().Value = 0;
                    Menu["ExtraPingBuffer"].As<MenuSlider>().Value = 40;
                    break;
                case "Smooth":
                    Menu["FastMovementBlock"].As<MenuBool>().Value = true;
                    Menu["FastEvadeActivationTime"].As<MenuSlider>().Value = 65;
                    Menu["RejectMinDistance"].As<MenuSlider>().Value = 10;
                    Menu["ExtraCPADistance"].As<MenuSlider>().Value = 10;
                    Menu["ExtraPingBuffer"].As<MenuSlider>().Value = 65;
                    break;
                case "Hawk":
                    Menu["DodgeDangerous"].As<MenuBool>().Value = false;
                    Menu["DodgeFOWSpells"].As<MenuBool>().Value = false;
                    Menu["DodgeCircularSpells"].As<MenuBool>().Value = false;
                    Menu["DodgeDangerousKeyEnabled"].As<MenuBool>().Value = true;
                    Menu["HigherPrecision"].As<MenuBool>().Value = true;
                    Menu["RecalculatePosition"].As<MenuBool>().Value = true;
                    Menu["ContinueMovement"].As<MenuBool>().Value = true;
                    Menu["CalculateWindupDelay"].As<MenuBool>().Value = true;
                    Menu["CheckSpellCollision"].As<MenuBool>().Value = true;
                    Menu["PreventDodgingUnderTower"].As<MenuBool>().Value = false;
                    Menu["PreventDodgingNearEnemy"].As<MenuBool>().Value = true;
                    Menu["AdvancedSpellDetection"].As<MenuBool>().Value = true;
                    Menu["ClickOnlyOnce"].As<MenuBool>().Value = true;
                    Menu["EnableEvadeDistance"].As<MenuBool>().Value = true;
                    Menu["TickLimiter"].As<MenuSlider>().Value = 200;
                    Menu["SpellDetectionTime"].As<MenuSlider>().Value = 375;
                    Menu["ReactionTime"].As<MenuSlider>().Value = 285;
                    Menu["DodgeInterval"].As<MenuSlider>().Value = 235;
                    Menu["FastEvadeActivationTime"].As<MenuSlider>().Value = 0;
                    Menu["SpellActivationTime"].As<MenuSlider>().Value = 200;
                    Menu["RejectMinDistance"].As<MenuSlider>().Value = 0;
                    Menu["ExtraPingBuffer"].As<MenuSlider>().Value = 65;
                    Menu["ExtraCPADistance"].As<MenuSlider>().Value = 0;
                    Menu["ExtraSpellRadius"].As<MenuSlider>().Value = 0;
                    Menu["ExtraEvadeDistance"].As<MenuSlider>().Value = 200;
                    Menu["ExtraAvoidDistance"].As<MenuSlider>().Value = 200;
                    Menu["MinComfortZone"].As<MenuSlider>().Value = 550;
                    break;
                case "Kurisu":
                    Menu["DodgeFOWSpells"].As<MenuBool>().Value = false;
                    Menu["DodgeDangerousKeyEnabled"].As<MenuBool>().Value = true;
                    Menu["RecalculatePosition"].As<MenuBool>().Value = true;
                    Menu["ContinueMovement"].As<MenuBool>().Value = true;
                    Menu["CalculateWindupDelay"].As<MenuBool>().Value = true;
                    Menu["PreventDodgingUnderTower"].As<MenuBool>().Value = true;
                    Menu["PreventDodgingNearEnemy"].As<MenuBool>().Value = true;
                    Menu["MinComfortZone"].As<MenuSlider>().Value = 850;
                    break;
                case "GuessWho":
                    Menu["DodgeDangerousKeyEnabled"].As<MenuBool>().Value = true;
                    Menu["HigherPrecision"].As<MenuBool>().Value = true;
                    Menu["CheckSpellCollision"].As<MenuBool>().Value = true;
                    Menu["PreventDodgingUnderTower"].As<MenuBool>().Value = true;
                    Menu["ShowStatus"].As<MenuBool>().Value = false;
                    Menu["DrawSpellPos"].As<MenuBool>().Value = true;
                    break;
            }
        }

        private static void OnLoadPingTesterChange(MenuComponent sender, ValueChangedArgs e)
        {
            if (_pingTester == null)
            {
                _pingTester = new PingTester();
            }
        }

        private static void OnLoadSpellTesterChange(MenuComponent sender, ValueChangedArgs e)
        {
            if (_spellTester == null)
            {
                _spellTester = new SpellTester();
            }
        }

        private static void Game_OnGameEnd(GameObjectTeam team)
        {
            HasGameEnded = true;
        }

        private void Game_OnCastSpell(Obj_AI_Base sender, SpellBookCastSpellEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            var sData = sender.SpellBook.GetSpell(args.Slot);

            if (SpellDetector.ChanneledSpells.TryGetValue(sData.Name, out _))
            {
                LastStopEvadeTime = Environment.TickCount + ObjectCache.GamePing + 100;
            }

            if (EvadeSpell.LastSpellEvadeCommand != null && EvadeSpell.LastSpellEvadeCommand.Timestamp + ObjectCache.GamePing + 150 > Environment.TickCount)
            {
                args.Process = false;
            }

            LastSpellCast = args.Slot;
            LastSpellCastTime = Environment.TickCount;

            if (Situation.ShouldDodge())
            {
                if (IsDodging && SpellDetector.Spells.Any())
                {
                    if (SpellDetector.WindupSpells.Select(entry => entry.Value).Any(spellData => spellData.SpellKey == args.Slot))
                    {
                        args.Process = false;
                        return;
                    }
                }
            }

            foreach (var evadeSpell in EvadeSpell.EvadeSpells)
            {
                if (evadeSpell.IsItem || evadeSpell.SpellKey != args.Slot || evadeSpell.Untargetable)
                {
                    continue;
                }

                switch (evadeSpell.EvadeType)
                {
                    case EvadeType.Blink:
                    {
                        var blinkPos = args.Start.To2D();

                        var posInfo = EvadeHelper.CanHeroWalkToPos(blinkPos, evadeSpell.Speed, ObjectCache.GamePing, 0);
                        if (posInfo != null && posInfo.PosDangerLevel == 0)
                        {
                            EvadeCommand.MoveTo(posInfo.Position);
                            LastStopEvadeTime = Environment.TickCount + ObjectCache.GamePing + evadeSpell.SpellDelay;
                        }
                        break;
                    }
                    case EvadeType.Dash:
                    {
                        var dashPos = args.Start.To2D();

                        if (args.Target != null)
                        {
                            dashPos = args.Target.Position.To2D();
                        }

                        if (evadeSpell.FixedRange || dashPos.Distance(MyHero.ServerPosition.To2D()) > evadeSpell.Range)
                        {
                            var dir = (dashPos - MyHero.ServerPosition.To2D()).Normalized();
                            dashPos = MyHero.ServerPosition.To2D() + dir * evadeSpell.Range;
                        }

                        var posInfo = EvadeHelper.CanHeroWalkToPos(dashPos, evadeSpell.Speed, ObjectCache.GamePing, 0);
                        if (posInfo != null && posInfo.PosDangerLevel > 0)
                        {
                            args.Process = false;
                            return;
                        }

                        if (IsDodging || Environment.TickCount < LastDodgingEndTime + 500)
                        {
                            EvadeCommand.MoveTo(Game.CursorPos.To2D());
                            LastStopEvadeTime = Environment.TickCount + ObjectCache.GamePing + 100;
                        }
                        break;
                    }
                }

                return;
            }
        }

        private void Game_OnIssueOrder(Obj_AI_Base hero, Obj_AI_BaseIssueOrderEventArgs args)
        {
            if (!hero.IsMe)
            {
                return;
            }

            if (!Situation.ShouldDodge())
            {
                return;
            }

            if (args.OrderType == OrderType.MoveTo)
            {
                if (IsDodging && SpellDetector.Spells.Any())
                {
                    var limitDelay = ObjectCache.MenuCache.Cache["TickLimiter"].As<MenuSlider>(); //Tick limiter                
                    if (Environment.TickCount - LastTickCount < limitDelay.Value)
                    {
                        LastTickCount = Environment.TickCount;
                        args.ProcessEvent = false;
                        return;
                    }

                    CheckHeroInDanger();

                    LastBlockedUserMoveTo = new EvadeCommand
                    {
                        Order = EvadeOrderCommand.MoveTo,
                        TargetPosition = args.Position.To2D(),
                        Timestamp = Environment.TickCount,
                        IsProcessed = false
                    };

                    var posInfoTest = EvadeHelper.CanHeroWalkToPos(args.Position.To2D(), ObjectCache.MyHeroCache.MoveSpeed, 0, 0, false);

                    if (posInfoTest.IsDangerousPos)
                    {
                        args.ProcessEvent = false;
                    }
                    else
                    {
                        LastPosInfo.Position = args.Position.To2D();
                        args.ProcessEvent = true;
                    }
                }
                else
                {
                    var movePos = args.Position.To2D();
                    var extraDelay = ObjectCache.MenuCache.Cache["ExtraPingBuffer"].As<MenuSlider>().Value;

                    if (EvadeHelper.CheckMovePath(movePos, ObjectCache.GamePing + extraDelay))
                    {
                        LastBlockedUserMoveTo = new EvadeCommand
                        {
                            Order = EvadeOrderCommand.MoveTo,
                            TargetPosition = args /*.Target*/.Position.To2D(),
                            Timestamp = Environment.TickCount,
                            IsProcessed = false
                        };

                        args.ProcessEvent = false;

                        if (Environment.TickCount - LastMovementBlockTime < 500 && LastMovementBlockPos.Distance(args. /*Target.*/Position) < 100)
                        {
                            return;
                        }

                        LastMovementBlockPos = args. /*Target.*/Position;
                        LastMovementBlockTime = Environment.TickCount;

                        var posInfo = EvadeHelper.GetBestPositionMovementBlock(movePos);
                        if (posInfo != null)
                        {
                            EvadeCommand.MoveTo(posInfo.Position);
                        }
                        return;
                    }
                    LastBlockedUserMoveTo.IsProcessed = true;
                }
            }
            else
            {
                if (IsDodging)
                {
                    args.ProcessEvent = false;
                }
                else
                {
                    if (args.OrderType == OrderType.AttackUnit)
                    {
                        var target = args.Target;
                        if (target != null && target.IsValid)
                        {
                            var baseTarget = target as Obj_AI_Base;
                            if (baseTarget != null && ObjectCache.MyHeroCache.ServerPos2D.Distance(baseTarget.ServerPosition.To2D()) >
                                MyHero.AttackRange + ObjectCache.MyHeroCache.BoundingRadius + baseTarget.BoundingRadius)
                            {
                                var movePos = args.Position.To2D();
                                var extraDelay = ObjectCache.MenuCache.Cache["ExtraPingBuffer"].As<MenuSlider>().Value;
                                if (EvadeHelper.CheckMovePath(movePos, ObjectCache.GamePing + extraDelay))
                                {
                                    args.ProcessEvent = false;
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            if (!args.ProcessEvent)
            {
                return;
            }

            LastIssueOrderGameTime = Game.ClockTime * 1000;
            LastIssueOrderTime = Environment.TickCount;
            LastIssueOrderArgs = args;

            switch (args.OrderType)
            {
                case OrderType.MoveTo:
                    LastMoveToPosition = args /*.Target*/.Position.To2D();
                    LastMoveToServerPos = MyHero.ServerPosition.To2D();
                    break;
                case OrderType.Stop:
                    LastStopPosition = MyHero.ServerPosition.To2D();
                    break;
            }
        }

        private static void Orbwalker_PreAttack(object sender, PreAttackEventArgs e)
        {
            if (IsDodging)
            {
                e.Cancel = true;
            }
        }

        private void Game_OnProcessSpell(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (SpellDetector.ChanneledSpells.TryGetValue(args.SpellData.Name.ToLower(), out _))
            {
                IsChanneling = true;
                ChannelPosition = MyHero.ServerPosition.To2D();
            }

            if (!ObjectCache.MenuCache.Cache["CalculateWindupDelay"].As<MenuBool>().Enabled)
            {
                return;
            }

            var castTime = (sender.SpellBook.CastEndTime - Game.ClockTime) * 1000;

            if (!(castTime > 0) || !args.SpellData.ConsideredAsAutoAttack || !(Math.Abs(castTime - MyHero.AttackCastDelay * 1000) > 1))
            {
                return;
            }

            LastWindupTime = Environment.TickCount + castTime - Game.Ping / 2f;

            if (IsDodging)
            {
                SpellDetector_OnProcessDetectedSpells();
            }
        }

        private void Game_OnGameUpdate()
        {
            try
            {
            
                ObjectCache.MyHeroCache.UpdateInfo();
                //CheckHeroInDanger();

                //if (IsChanneling && ChannelPosition.Distance(ObjectCache.MyHeroCache.ServerPos2D) > 50 && !MyHero.SpellBook.IsChanneling)
                //{
                //    IsChanneling = false;
                //}

                //var limitDelay = ObjectCache.MenuCache.Cache["TickLimiter"].As<MenuSlider>(); //Tick limiter                
                //if (EvadeHelper.FastEvadeMode || Environment.TickCount - LastTickCount > limitDelay.Value)
                //{
                //    if (Environment.TickCount > LastStopEvadeTime)
                //    {
                //        DodgeSkillShots();
                //        ContinueLastBlockedCommand();
                //    }

                //    LastTickCount = Environment.TickCount;
                //}

                //EvadeSpell.UseEvadeSpell();
                //CheckDodgeOnlyDangerous();
                //RecalculatePath();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void RecalculatePath()
        {
            if (!ObjectCache.MenuCache.Cache["RecalculatePosition"].As<MenuBool>().Enabled || !IsDodging)
            {
                return;
            }

            if (LastPosInfo == null || LastPosInfo.RecalculatedPath)
            {
                return;
            }

            var path = MyHero.Path;
            if (path.Length <= 0)
            {
                return;
            }

            var movePos = path.Last().To2D();

            if (!(movePos.Distance(LastPosInfo.Position) < 5))
            {
                return;
            }

            var posInfo = EvadeHelper.CanHeroWalkToPos(movePos, ObjectCache.MyHeroCache.MoveSpeed, 0, 0, false);
            if (posInfo.PosDangerCount <= LastPosInfo.PosDangerCount)
            {
                return;
            }

            LastPosInfo.RecalculatedPath = true;

            if (EvadeSpell.PreferEvadeSpell())
            {
                LastPosInfo = PositionInfo.SetAllUndodgeable();
            }
            else
            {
                var newPosInfo = EvadeHelper.GetBestPosition();
                if (newPosInfo.PosDangerCount >= posInfo.PosDangerCount)
                {
                    return;
                }

                LastPosInfo = newPosInfo;
                CheckHeroInDanger();
                DodgeSkillShots();
            }
        }

        private static void ContinueLastBlockedCommand()
        {
            if (!ObjectCache.MenuCache.Cache["ContinueMovement"].As<MenuBool>().Enabled || !Situation.ShouldDodge())
            {
                return;
            }

            var movePos = LastBlockedUserMoveTo.TargetPosition;
            var extraDelay = ObjectCache.MenuCache.Cache["ExtraPingBuffer"].As<MenuSlider>().Value;

            if (IsDodging || LastBlockedUserMoveTo.IsProcessed || !(Environment.TickCount - LastEvadeCommand.Timestamp > ObjectCache.GamePing + extraDelay) ||
                !(Environment.TickCount - LastBlockedUserMoveTo.Timestamp < 1500))
            {
                return;
            }

            movePos = movePos + (movePos - ObjectCache.MyHeroCache.ServerPos2D).Normalized() * EvadeUtils.Random.Next(1, 65);

            if (EvadeHelper.CheckMovePath(movePos, ObjectCache.GamePing + extraDelay))
            {
                return;
            }

            EvadeCommand.MoveTo(movePos);
            LastBlockedUserMoveTo.IsProcessed = true;
        }

        private static void CheckHeroInDanger()
        {
            var playerInDanger = false;

            foreach (var entry in SpellDetector.Spells)
            {
                var spell = entry.Value;

                if (LastPosInfo == null || !LastPosInfo.DodgeableSpells.Contains(spell.SpellId))
                {
                    continue;
                }

                if (MyHero.ServerPosition.To2D().InSkillShot(spell, ObjectCache.MyHeroCache.BoundingRadius))
                {
                    playerInDanger = true;
                    break;
                }

                if (!ObjectCache.MenuCache.Cache["EnableEvadeDistance"].As<MenuBool>().Enabled || !(Environment.TickCount < LastPosInfo.EndTime))
                {
                    continue;
                }

                playerInDanger = true;
                break;
            }

            if (IsDodging && !playerInDanger)
            {
                LastDodgingEndTime = Environment.TickCount;
            }

            if (IsDodging == false && !Situation.ShouldDodge())
            {
                return;
            }

            IsDodging = playerInDanger;
        }

        private static void DodgeSkillShots()
        {
            if (!Situation.ShouldDodge())
            {
                IsDodging = false;
                return;
            }

            if (IsDodging)
            {
                if (LastPosInfo == null)
                {
                    return;
                }

                var lastBestPosition = LastPosInfo.Position;

                if (!ObjectCache.MenuCache.Cache["ClickOnlyOnce"].Enabled && MyHero.Path.Count() > 0 && LastPosInfo.Position.Distance(MyHero.Path.Last().To2D()) < 5)
                {
                    return;
                }

                EvadeCommand.MoveTo(lastBestPosition);
                LastEvadeOrderTime = Environment.TickCount;
            }
            else
            {
                //Check if hero will walk into a skillshot
                var path = MyHero.Path;

                if (path.Length <= 0)
                {
                    return;
                }

                var movePos = path[path.Length - 1].To2D();

                if (!EvadeHelper.CheckMovePath(movePos))
                {
                    return;
                }

                var posInfo = EvadeHelper.GetBestPositionMovementBlock(movePos);
                if (posInfo != null)
                {
                    EvadeCommand.MoveTo(posInfo.Position);
                }
            }
        }

        public void CheckLastMoveTo()
        {
            if (!EvadeHelper.FastEvadeMode && !ObjectCache.MenuCache.Cache["FastMovementBlock"].As<MenuBool>().Enabled)
            {
                return;
            }

            if (IsDodging)
            {
                return;
            }

            if (LastIssueOrderArgs == null || LastIssueOrderArgs.OrderType != OrderType.MoveTo)
            {
                return;
            }

            if (!(Game.ClockTime * 1000 - LastIssueOrderGameTime < 500))
            {
                return;
            }

            Game_OnIssueOrder(MyHero, LastIssueOrderArgs);
            LastIssueOrderArgs = null;
        }

        public static bool IsDodgeDangerousEnabled()
        {
            if (ObjectCache.MenuCache.Cache["DodgeDangerous"].Enabled)
            {
                return true;
            }

            if (!ObjectCache.MenuCache.Cache["DodgeDangerousKeyEnabled"].Enabled)
            {
                return false;
            }

            if (ObjectCache.MenuCache.Cache["DodgeDangerousKey"].As<MenuKeyBind>().Enabled || ObjectCache.MenuCache.Cache["DodgeDangerousKey2"].As<MenuKeyBind>().Enabled)
            {
                return true;
            }

            return false;
        }

        public static void CheckDodgeOnlyDangerous() //Dodge only dangerous event
        {
            var bDodgeOnlyDangerous = IsDodgeDangerousEnabled();

            if (DodgeOnlyDangerous == false && bDodgeOnlyDangerous)
            {
                SpellDetector.RemoveNonDangerousSpells();
                DodgeOnlyDangerous = true;
            }
            else
            {
                DodgeOnlyDangerous = bDodgeOnlyDangerous;
            }
        }

        public static void SetAllUndodgeable()
        {
            LastPosInfo = PositionInfo.SetAllUndodgeable();
        }

        private void SpellDetector_OnProcessDetectedSpells()
        {
            ObjectCache.MyHeroCache.UpdateInfo();

            if (!ObjectCache.MenuCache.Cache["DodgeSkillShots"].As<MenuKeyBind>().Enabled)
            {
                LastPosInfo = PositionInfo.SetAllUndodgeable();
                EvadeSpell.UseEvadeSpell();
                return;
            }

            if (ObjectCache.MyHeroCache.ServerPos2D.CheckDangerousPos(0) || ObjectCache.MyHeroCache.ServerPos2DExtra.CheckDangerousPos(0))
            {
                if (EvadeSpell.PreferEvadeSpell())
                {
                    LastPosInfo = PositionInfo.SetAllUndodgeable();
                }
                else
                {
                    var posInfo = EvadeHelper.GetBestPosition();

                    if (posInfo != null)
                    {
                        LastPosInfo = posInfo.CompareLastMovePos();

                        var travelTime = ObjectCache.MyHeroCache.ServerPos2DPing.Distance(LastPosInfo.Position) / MyHero.MoveSpeed;

                        LastPosInfo.EndTime = Environment.TickCount + travelTime * 1000 - 100;
                    }

                    CheckHeroInDanger();

                    if (Environment.TickCount > LastStopEvadeTime)
                    {
                        DodgeSkillShots();
                    }

                    CheckLastMoveTo();
                    EvadeSpell.UseEvadeSpell();
                }
            }
            else
            {
                LastPosInfo = PositionInfo.SetAllDodgeable();
                CheckLastMoveTo();
            }
        }
    }
}
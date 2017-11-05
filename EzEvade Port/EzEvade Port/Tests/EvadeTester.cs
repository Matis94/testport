namespace EzEvade_Port.Tests
{
    using System;
    using System.Drawing;
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Events;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Menu.Components;
    using Aimtec.SDK.Util;
    using Aimtec.SDK.Util.Cache;
    using Core;
    using Draw;
    using Helpers;
    using Spells;
    using Utils;
    using DelayAction = Utils.DelayAction;
    using Spell = Spells.Spell;

    class EvadeTester
    {
        public static Menu TestMenu;

        private static Vector2 _circleRenderPos;

        private static float _startWalkTime;

        private static float _lastStopingTime;

        public static float LastProcessPacketTime = 0;

        private static float _lastRightMouseClickTime;

        private static float _lastSpellCastTime;
        private static float _lastHeroSpellCastTime;

        private static MissileClient _testMissile;
        private static float _testMissileStartTime;

        public EvadeTester(Menu mainMenu)
        {
            Render.OnPresent += Render_OnPresent;
            Obj_AI_Base.OnIssueOrder += Game_OnIssueOrder;
            Game.OnUpdate += Game_OnGameUpdate;

            GameObject.OnDestroy += Game_OnDelete;

            GameObject.OnCreate += SpellMissile_OnCreate;

            Obj_AI_Base.OnProcessSpellCast += Game_ProcessSpell;
            SpellBook.OnCastSpell += Game_OnCastSpell;

            AttackableUnit.OnDamage += Game_OnDamage;

            Game.OnWndProc += Game_OnWndProc;

            Obj_AI_Base.OnPerformCast += Game_OnDoCast;

            Obj_AI_Base.OnNewPath += ObjAiHeroOnOnNewPath;

            SpellDetector.OnProcessDetectedSpells += SpellDetector_OnProcessDetectedSpells;

            TestMenu = new Menu("Test", "Test")
            {
                new MenuBool("(TestWall)", "TestWall"),
                new MenuBool("(TestPath)", "TestPath"),
                new MenuBool("(TestTracker)", "TestTracker"),
                new MenuBool("(TestHeroPos)", "TestHeroPos"),
                new MenuBool("(DrawHeroPos)", "DrawHeroPos"),
                new MenuBool("(TestSpellEndTime)", "TestSpellEndTime"),
                new MenuBool("(ShowBuffs)", "ShowBuffs"),
                new MenuBool("(ShowDashInfo)", "ShowDashInfo"),
                new MenuBool("(ShowProcessSpell)", "ShowProcessSpell"),
                new MenuBool("(ShowDoCastInfo)", "ShowDoCastInfo"),
                new MenuBool("(ShowMissileInfo)", "ShowMissileInfo"),
                new MenuBool("(ShowWindupTime)", "ShowWindupTime"),
                new MenuKeyBind("(TestMoveTo)", "TestMoveTo", KeyCode.L, KeybindType.Toggle),
                new MenuBool("(EvadeTesterPing)", "EvadeTesterPing")
            };
            Evade.Menu.Add(TestMenu);

            Game_OnGameLoad();
        }

        private static Obj_AI_Hero MyHero => ObjectManager.GetLocalPlayer();

        private static float GetGameTimer => Game.ClockTime * 1000;
        private static float GetTickCountTimer => Environment.TickCount & int.MaxValue;
        private static float GetWatchTimer => Environment.TickCount;

        private void Game_OnDoCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!TestMenu["(ShowDoCastInfo)"].Enabled)
            {
                return;
            }

            ConsolePrinter.Print("DoCast " + sender.Name + ": " + args.SpellData.Name);
        }

        private void Game_OnWndProc(WndProcEventArgs args)
        {
            if (args.Message == (uint) WindowsMessages.WM_RBUTTONDOWN)
            {
                _lastRightMouseClickTime = Environment.TickCount;
            }
        }

        private void Game_OnGameLoad()
        {
            ConsolePrinter.Print("EvadeTester loaded");
        }

        private static void ObjAiHeroOnOnNewPath(Obj_AI_Base unit, Obj_AI_BaseNewPathEventArgs args)
        {
            if (unit.Type != GameObjectType.obj_AI_Hero)
            {
                return;
            }

            if (!args.IsDash || !TestMenu["(ShowDashInfo)"].Enabled)
            {
                return;
            }
            var dist = args.Path.First().Distance(args.Path.Last());
            ConsolePrinter.Print("Dash Speed: " + args.Speed + " Dash dist: " + dist);
        }

        private void Game_OnCastSpell(Obj_AI_Base sender, SpellBookCastSpellEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (TestMenu["TestPath"].Enabled)
            {
                RenderObjects.Add(new RenderCircle(args.End.To2D(), 500));
            }
        }

        private void SpellDetector_OnProcessDetectedSpells()
        {
            EvadeHelper.GetBestPositionTest();
            _circleRenderPos = Evade.LastPosInfo.Position;

            _lastSpellCastTime = Environment.TickCount;
        }

        private void Game_OnDelete(GameObject sender)
        {
            if (!TestMenu["(ShowMissileInfo)"].Enabled)
            {
                return;
            }

            if (_testMissile == null || _testMissile.NetworkId != sender.NetworkId)
            {
                return;
            }

            var range = sender.Position.To2D().Distance(_testMissile.StartPosition.To2D());
            ConsolePrinter.Print("[" + _testMissile.SpellData.Name + "]: Est.Missile range: " + range);
            ConsolePrinter.Print("[" + _testMissile.SpellData.Name + "]: Est.Missile speed: " + 1000 * (range / (Environment.TickCount - _testMissileStartTime)));
        }

        private void SpellMissile_OnCreate(GameObject obj)
        {
            if (obj.IsValid && obj.Type == GameObjectType.MissileClient)
            {
                var mis = (MissileClient) obj;

                if (mis.SpellCaster is Obj_AI_Hero && mis.SpellData.ConsideredAsAutoAttack)
                {
                    ConsolePrinter.Print("[" + mis.SpellData.Name + "]: Missile Speed " + mis.SpellData.MissileSpeed);
                    ConsolePrinter.Print("[" + mis.SpellData.Name + "]: LineWidth " + mis.SpellData.LineWidth);
                    ConsolePrinter.Print("[" + mis.SpellData.Name + "]: Range " + mis.SpellData.CastRange);
                    ConsolePrinter.Print("[" + mis.SpellData.Name + "]: Accel " + mis.SpellData.MissileAccel);
                }
            }

            if (!obj.IsValid || obj.Type != GameObjectType.MissileClient)
            {
                return;
            }

            if (!TestMenu["ShowMissileInfo"].Enabled)
            {
                return;
            }

            var missile = (MissileClient) obj;

            if (!(missile.SpellCaster is Obj_AI_Hero))
            {
                return;
            }

            var testMissileSpeedStartTime = Environment.TickCount;
            var testMissileSpeedStartPos = missile.Position.To2D();

            DelayAction.Add(250,
                            () =>
                            {
                                if (!missile.IsValid || missile.IsDead)
                                {
                                    return;
                                }

                                testMissileSpeedStartTime = Environment.TickCount;
                                testMissileSpeedStartPos = missile.Position.To2D();
                            });

            _testMissile = missile;
            _testMissileStartTime = Environment.TickCount;

            ConsolePrinter.Print("[" + missile.SpellData.Name + "]: Est.CastTime: " + (Environment.TickCount - _lastHeroSpellCastTime));
            ConsolePrinter.Print("[" + missile.SpellData.Name + "]: Missile Name " + missile.SpellData.Name);
            ConsolePrinter.Print("[" + missile.SpellData.Name + "]: Missile Speed " + missile.SpellData.MissileSpeed);
            ConsolePrinter.Print("[" + missile.SpellData.Name + "]: Accel " + missile.SpellData.MissileAccel);
            ConsolePrinter.Print("[" + missile.SpellData.Name + "]: Max Speed " + missile.SpellData.MissileMaxSpeed);
            ConsolePrinter.Print("[" + missile.SpellData.Name + "]: LineWidth " + missile.SpellData.LineWidth);
            ConsolePrinter.Print("[" + missile.SpellData.Name + "]: Range " + missile.SpellData.CastRange);

            RenderObjects.Add(new RenderCircle(missile.StartPosition.To2D(), 500));
            RenderObjects.Add(new RenderCircle(missile.EndPosition.To2D(), 500));

            DelayAction.Add(750,
                            () =>
                            {
                                if (!missile.IsValid || missile.IsDead)
                                {
                                    return;
                                }

                                var dist = missile.Position.To2D().Distance(testMissileSpeedStartPos);
                                ConsolePrinter.Print("[" + missile.SpellData.Name + "]: Est.Missile speed: " + 1000 * (dist / (Environment.TickCount - testMissileSpeedStartTime)));
                            });

            if (missile.SpellCaster == null || missile.SpellCaster.Team == MyHero.Team || missile.SpellData.Name == null ||
                !SpellDetector.OnMissileSpells.TryGetValue(missile.SpellData.Name, out var spellData) || missile.StartPosition == null || missile.EndPosition == null)
            {
                return;
            }

            if (!(missile.StartPosition.Distance(MyHero.Position) < spellData.Range + 1000))
            {
                return;
            }

            var hero = missile.SpellCaster;

            if (!hero.IsVisible)
            {
                return;
            }

            foreach (var entry in SpellDetector.Spells)
            {
                var spell = entry.Value;

                if (spell.Info.MissileName != missile.SpellData.Name || spell.HeroId != missile.SpellCaster.NetworkId)
                {
                    continue;
                }

                if (spell.Info.IsThreeWay == false && spell.Info.IsSpecial == false)
                {
                    ConsolePrinter.Print("Acquired: " + (Environment.TickCount - spell.StartTime));
                }
            }
        }

        private void Game_ProcessSpell(Obj_AI_Base hero, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!(hero is Obj_AI_Hero))
            {
                return;
            }

            if (TestMenu["(ShowProcessSpell)"].Enabled)
            {
                ConsolePrinter.Print(args.SpellData.Name + " CastTime: " + (hero.SpellBook.CastEndTime - Game.ClockTime));

                ConsolePrinter.Print("CastRadius: " + args.SpellData.CastRadius);
            }

            if (args.SpellData.Name == "YasuoQW")
            {
                RenderObjects.Add(new RenderCircle(args.Start.To2D(), 500));
                RenderObjects.Add(new RenderCircle(args.End.To2D(), 500));
            }

            _lastHeroSpellCastTime = Environment.TickCount;

            foreach (var entry in SpellDetector.Spells)
            {
                var spell = entry.Value;

                if (spell.Info.SpellName != args.SpellData.Name || spell.HeroId != hero.NetworkId)
                {
                    continue;
                }

                if (spell.Info.IsThreeWay == false && spell.Info.IsSpecial == false)
                {
                    ConsolePrinter.Print("Time diff: " + (Environment.TickCount - spell.StartTime));
                }
            }

            if (hero.IsMe)
            {
                _lastSpellCastTime = Environment.TickCount;
            }
        }

        private void CompareSpellLocation(Spell spell, Vector2 pos, float time)
        {
            var pos2 = spell.CurrentSpellPosition;
            if (spell.SpellObject != null)
            {
                ConsolePrinter.Print("Compare: " + pos2.Distance(pos) / (Environment.TickCount - time));
            }
        }

        private void CompareSpellLocation2(Spell spell)
        {
            var pos1 = spell.CurrentSpellPosition;
            var timeNow = Environment.TickCount;

            if (spell.SpellObject != null)
            {
                ConsolePrinter.Print("start distance: " + spell.StartPos.Distance(pos1));
            }

            DelayAction.Add(250, () => CompareSpellLocation(spell, pos1, timeNow));
        }

        private void Game_OnGameUpdate()
        {
            if (_startWalkTime > 0)
            {
                if (Environment.TickCount - _startWalkTime > 500 && MyHero.HasPath == false)
                {
                    _startWalkTime = 0;
                }
            }

            if (TestMenu["(ShowWindupTime)"].Enabled)
            {
                if (MyHero.HasPath && _lastStopingTime > 0)
                {
                    ConsolePrinter.Print("WindupTime: " + (Environment.TickCount - _lastStopingTime));
                    _lastStopingTime = 0;
                }
                else if (!MyHero.HasPath && _lastStopingTime == 0)
                {
                    _lastStopingTime = Environment.TickCount;
                }
            }

            if (!TestMenu["(ShowDashInfo)"].Enabled)
            {
                return;
            }

            if (!MyHero.IsDashing())
            {
                return;
            }

            var dashInfo = MyHero.GetDashInfo();
            ConsolePrinter.Print("Dash Speed: " + dashInfo.Speed + " Dash dist: " + dashInfo.EndPos.Distance(dashInfo.StartPos));
        }

        private void Game_OnDamage(AttackableUnit sender, AttackableUnitDamageEventArgs args)
        {
            if (!TestMenu["(TestSpellEndTime)"].Enabled)
            {
                return;
            }

            if (!sender.IsMe)
            {
                return;
            }

            ConsolePrinter.Print("Damage taken time: " + (Environment.TickCount - _lastSpellCastTime));
        }

        private void Game_OnIssueOrder(Obj_AI_Base hero, Obj_AI_BaseIssueOrderEventArgs args)
        {
            if (!hero.IsMe)
            {
                return;
            }

            if (TestMenu["(TestPath)"].Enabled)
            {
                var tPath = MyHero.GetPath(args.Target.Position);

                foreach (var point in tPath)
                {
                    var point2D = point.To2D();
                    RenderObjects.Add(new RenderCircle(point2D, 500));
                }
            }

            if (args.OrderType != OrderType.MoveTo)
            {
                return;
            }

            if (TestMenu["(EvadeTesterPing)"].Enabled)
            {
                ConsolePrinter.Print("Sending Path ClickTime: " + (Environment.TickCount - _lastRightMouseClickTime));
            }

            var heroPos = ObjectCache.MyHeroCache.ServerPos2D;
            var pos = args.Target.Position.To2D();
            var speed = ObjectCache.MyHeroCache.MoveSpeed;

            _startWalkTime = Environment.TickCount;

            foreach (var entry in SpellDetector.Spells)
            {
                var spell = entry.Value;
                var walkDir = (pos - heroPos).Normalized();

                var spellTime = Environment.TickCount - spell.StartTime - spell.Info.SpellDelay;
                var spellPos = spell.StartPos + spell.Direction * spell.Info.ProjectileSpeed * (spellTime / 1000);
                //ConsolePrinter.Print("aaaa" + spellTime);

                var movingCollisionTime = MathUtils.GetCollisionTime(heroPos,
                                                                     spellPos,
                                                                     walkDir * (speed - 25),
                                                                     spell.Direction * (spell.Info.ProjectileSpeed - 200),
                                                                     ObjectCache.MyHeroCache.BoundingRadius,
                                                                     spell.Radius,
                                                                     out var isCollision);
                if (!isCollision)
                {
                    continue;
                }
                if (true)
                {
                    ConsolePrinter.Print("movingCollisionTime: " + movingCollisionTime);
                }
            }
        }

        private void Render_OnPresent()
        {
            foreach (var entry in SpellDetector.DrawSpells)
            {
                var spell = entry.Value;

                if (spell.SpellType != SpellType.Line)
                {
                    continue;
                }

                var spellPos = spell.CurrentSpellPosition;

                Render.Circle(new Vector3(spellPos.X, spellPos.Y, MyHero.Position.Z), spell.Info.Radius, 50, Color.White);
            }

            if (TestMenu["(TestHeroPos)"].Enabled)
            {
                var path = MyHero.Path;
                if (path.Length > 0)
                {
                    var heroPos2 = EvadeHelper.GetRealHeroPos(ObjectCache.GamePing + 50);
                    var heroPos1 = ObjectCache.MyHeroCache.ServerPos2D;

                    Render.Circle(new Vector3(heroPos2.X, heroPos2.Y, MyHero.ServerPosition.Z), ObjectCache.MyHeroCache.BoundingRadius, 50, Color.Red);
                    Render.Circle(new Vector3(MyHero.ServerPosition.X, MyHero.ServerPosition.Y, MyHero.ServerPosition.Z), ObjectCache.MyHeroCache.BoundingRadius, 50, Color.White);

                    Render.WorldToScreen(ObjectManager.GetLocalPlayer().Position, out var heroPos);

                    Render.Text($" {(int) heroPos2.Distance(heroPos1)}", new Vector2(heroPos.X - 10, heroPos.Y), RenderTextFlags.Center, Color.Red);

                    Render.Circle(new Vector3(_circleRenderPos.X, _circleRenderPos.Y, MyHero.ServerPosition.Z), 10, 50, Color.Red);
                }
            }

            if (TestMenu["(DrawHeroPos)"].Enabled)
            {
                Render.Circle(new Vector3(MyHero.ServerPosition.X, MyHero.ServerPosition.Y, MyHero.ServerPosition.Z), ObjectCache.MyHeroCache.BoundingRadius, 50, Color.White);
            }

            if (TestMenu["(TestMoveTo)"].As<MenuKeyBind>().Enabled)
            {
                TestMenu["(TestMoveTo)"].As<MenuKeyBind>().Value = false;

                MyHero.IssueOrder(OrderType.MoveTo, Game.CursorPos);

                var dir = (Game.CursorPos - MyHero.Position).Normalized();

                var pos2 = Game.CursorPos.To2D() - dir.To2D() * 75;

                DelayAction.Add(20, () => MyHero.IssueOrder(OrderType.MoveTo, pos2.To3D()));
            }

            if (TestMenu["(TestPath)"].Enabled)
            {
                var tPath = MyHero.GetPath(Game.CursorPos);

                foreach (var point in tPath)
                {
                    var point2D = point.To2D();
                    Render.Circle(new Vector3(point.X, point.Y, point.Z), ObjectCache.MyHeroCache.BoundingRadius, 50, Color.Violet);
                }
            }

            if (TestMenu["(TestPath)"].Enabled)
            {
                foreach (var entry in SpellDetector.Spells)
                {
                    var spell = entry.Value;

                    var to = Game.CursorPos.To2D();
                    var dir = (to - MyHero.Position.To2D()).Normalized();

                    var cpa = MathUtilsCpa.CPAPointsEx(MyHero.Position.To2D(), dir * ObjectCache.MyHeroCache.MoveSpeed, spell.EndPos, spell.Direction * spell.Info.ProjectileSpeed, to, spell.EndPos);

                    if (cpa < ObjectCache.MyHeroCache.BoundingRadius + spell.Radius) { }
                }
            }

            if (TestMenu["(ShowBuffs)"].Enabled)
            {
                var target = MyHero;

                foreach (var hero in GameObjects.EnemyHeroes)
                {
                    target = hero;
                }

                var buffs = target.Buffs;

                if (!target.IsTargetable)
                {
                    ConsolePrinter.Print("invul" + Environment.TickCount);
                }

                var height = 20;

                foreach (var buff in buffs)
                {
                    if (!buff.IsValid)
                    {
                        continue;
                    }

                    Render.Text(buff.Name, new Vector2(10, height), RenderTextFlags.Center, Color.White);
                    height += 20;

                    ConsolePrinter.Print(buff.Name);
                }
            }

            if (TestMenu["(TestTracker)"].Enabled)
            {
                foreach (var entry in ObjectTracker.ObjTracker)
                {
                    var info = entry.Value;

                    var endPos2 = !info.UsePosition ? info.Obj.Position : info.Position;

                    Render.Circle(new Vector3(endPos2.X, endPos2.Y, MyHero.Position.Z), 50, 50, Color.Green);
                }
            }

            if (!TestMenu["(TestWall)"].Enabled)
            {
                return;
            }

            var posChecked = 0;
            const int maxPosToCheck = 50;
            const int posRadius = 50;
            var radiusIndex = 0;

            var heroPoint = ObjectCache.MyHeroCache.ServerPos2D;

            while (posChecked < maxPosToCheck)
            {
                radiusIndex++;

                var curRadius = radiusIndex * 2 * posRadius;
                var curCircleChecks = (int) Math.Ceiling(2 * Math.PI * curRadius / (2 * (double) posRadius));

                for (var i = 1; i < curCircleChecks; i++)
                {
                    posChecked++;
                    var cRadians = 2 * Math.PI / (curCircleChecks - 1) * i; //check decimals
                    var pos = new Vector2((float) Math.Floor(heroPoint.X + curRadius * Math.Cos(cRadians)), (float) Math.Floor(heroPoint.Y + curRadius * Math.Sin(cRadians)));

                    if (!EvadeHelper.CheckPathCollision(MyHero, pos))
                    {
                        Render.Circle(new Vector3(pos.X, pos.Y, MyHero.Position.Z), 25, 50, Color.White);
                    }
                }
            }
        }
    }
}
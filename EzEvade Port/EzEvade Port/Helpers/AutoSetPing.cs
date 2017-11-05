namespace EzEvade_Port.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Menu.Components;
    using Core;
    using Draw;
    using Utils;

    class AutoSetPing
    {
        private static float _numExtraDelayTime;

        private static float _maxExtraDelayTime;

        private static Obj_AI_BaseIssueOrderEventArgs _lastIssueOrderArgs;
        private static Vector2 _lastMoveToServerPos;

        private static SpellBookCastSpellEventArgs _lastSpellCastArgs;
        private static Vector2 _lastSpellCastServerPos;

        private static bool _checkPing = true;

        private static readonly List<float> PingList = new List<float>();

        public static Menu Menu;

        public AutoSetPing(Menu mainMenu)
        {
            Obj_AI_Base.OnNewPath += Hero_OnNewPath;
            Obj_AI_Base.OnIssueOrder += Hero_OnIssueOrder;

            SpellBook.OnCastSpell += Game_OnCastSpell;
            GameObject.OnCreate += Game_OnCreateObj;

            Evade.AutoSetPingMenu = new Menu("AutoSetPing", "AutoSetPingMenu")
            {
                new MenuBool("AutoSetPingOn", "Auto Set Ping"),
                new MenuSlider("AutoSetPercentile", "Auto Set Percentile", 75)
            };

            mainMenu.Add(Evade.AutoSetPingMenu);

            Menu = mainMenu;
        }

        public static Obj_AI_Hero MyHero => ObjectManager.GetLocalPlayer();

        private static void Game_OnCastSpell(Obj_AI_Base sender, SpellBookCastSpellEventArgs e)
        {
            _checkPing = false;

            if (!sender.IsMe)
            {
                return;
            }

            _lastSpellCastArgs = e;

            if (!MyHero.HasPath || !MyHero.Path.Any())
            {
                return;
            }

            _lastSpellCastServerPos = EvadeUtils.GetGamePosition(MyHero, Game.Ping);
            MyHero.Path.Last().To2D();
            _checkPing = true;

            RenderObjects.Add(new RenderCircle(_lastSpellCastServerPos, 1000, Color.Green, 10));
        }

        private static void Game_OnCreateObj(GameObject sender)
        {
            var missile = sender as MissileClient;
            if (missile == null || !missile.SpellCaster.IsMe)
            {
                return;
            }

            if (!_lastSpellCastArgs.Process)
            {
                return;
            }

            RenderObjects.Add(new RenderCircle(missile.StartPosition.To2D(), 1000, Color.Red, 10));

            var distance = _lastSpellCastServerPos.Distance(missile.StartPosition.To2D());
            var moveTime = 1000 * distance / MyHero.MoveSpeed;
            Console.WriteLine("Extra Delay: " + moveTime);
        }

        private static void Hero_OnIssueOrder(Obj_AI_Base hero, Obj_AI_BaseIssueOrderEventArgs args)
        {
            _checkPing = false;

            if (!ObjectCache.MenuCache.Cache["AutoSetPingOn"].Enabled)
            {
                return;
            }

            if (!hero.IsMe)
            {
                return;
            }

            _lastIssueOrderArgs = args;

            if (args.OrderType != OrderType.MoveTo)
            {
                return;
            }

            if (!MyHero.HasPath || !MyHero.Path.Any())
            {
                return;
            }

            _lastMoveToServerPos = MyHero.ServerPosition.To2D();
            MyHero.Path.Last().To2D();
            _checkPing = true;
        }

        private static void Hero_OnNewPath(Obj_AI_Base hero, Obj_AI_BaseNewPathEventArgs args)
        {
            if (!ObjectCache.MenuCache.Cache["AutoSetPingOn"].Enabled)
            {
                return;
            }

            if (!hero.IsMe)
            {
                return;
            }

            var path = args.Path;

            if (path.Length <= 1 || args.IsDash)
            {
                return;
            }

            var movePos = path.Last().To2D();

            if (_checkPing && _lastIssueOrderArgs.ProcessEvent && _lastIssueOrderArgs.OrderType == OrderType.MoveTo && _lastIssueOrderArgs.Target.Position.To2D().Distance(movePos) < 3 &&
                MyHero.Path.Length == 1 && args.Path.Length == 2 && MyHero.HasPath)
            {
                RenderObjects.Add(new RenderLine(args.Path.First().To2D(), args.Path.Last().To2D(), 1000));
                RenderObjects.Add(new RenderLine(MyHero.Position.To2D(), MyHero.Path.Last().To2D(), 1000));

                var distanceTillEnd = MyHero.Path.Last().To2D().Distance(MyHero.Position.To2D());
                var moveTimeTillEnd = 1000 * distanceTillEnd / MyHero.MoveSpeed;

                if (moveTimeTillEnd < 500)
                {
                    return;
                }

                var myHeroPosition = new Vector3(MyHero.Position.X, MyHero.Position.Y, MyHero.Position.Z);

                var dir1 = (MyHero.Path.Last().To2D() - MyHero.Position.To2D()).Normalized();

                var ray1Startpos = new Vector3(myHeroPosition.X, myHeroPosition.Y, 0);
                var ray1Dir = new Vector3(dir1.X, dir1.Y, 0);

                var dir2 = (args.Path.First().To2D() - args.Path.Last().To2D()).Normalized();

                var argsPathFirst = new Vector3(args.Path.First().X, args.Path.First().Y, args.Path.First().Z);

                var ray2Startpos = new Vector3(argsPathFirst.X, argsPathFirst.Y, 0);
                var ray2Dir = new Vector3(dir2.X, dir2.Y, 0);

                var intersection = ray2Startpos.To2D().
                    Intersection(ray2Startpos.To2D().ExtendDir(ray2Dir.To2D(), args.Path.Length), ray1Startpos.To2D(), ray1Startpos.To2D().ExtendDir(ray1Dir.To2D(), args.Path.Length));

                if (intersection.Intersects)
                {
                    var intersection3 = intersection.Point.To3D();

                    var x = intersection3.To2D().X;
                    var y = intersection3.To2D().Y;

                    var intersectionAt = new Vector2(x, y);

                    var projection = intersectionAt.ProjectOn(MyHero.Path.Last().To2D(), MyHero.Position.To2D());

                    if (projection.IsOnSegment && dir1.AngleBetween(dir2) > 20 && dir1.AngleBetween(dir2) < 160)
                    {
                        RenderObjects.Add(new RenderCircle(intersectionAt, 1000, Color.Red, 10));

                        var distance =
                            _lastMoveToServerPos.Distance(intersectionAt);
                        var moveTime = 1000 * distance / MyHero.MoveSpeed;

                        if (moveTime < 1000)
                        {
                            if (_numExtraDelayTime > 0)
                            {
                                PingList.Add(moveTime);
                            }
                            _numExtraDelayTime += 1;

                            if (_maxExtraDelayTime == 0)
                            {
                                _maxExtraDelayTime = ObjectCache.MenuCache.Cache["ExtraPingBuffer"].As<MenuSlider>().Value;
                            }

                            if (_numExtraDelayTime % 100 == 0)
                            {
                                PingList.Sort();

                                var percentile = ObjectCache.MenuCache.Cache["AutoSetPercentile"].As<MenuSlider>().Value;
                                var percentIndex = (int) Math.Floor(PingList.Count * (percentile / 100f)) - 1;
                                _maxExtraDelayTime = Math.Max(PingList.ElementAt(percentIndex) - Game.Ping, 0);
                                ObjectCache.MenuCache.Cache["ExtraPingBuffer"].As<MenuSlider>().Value = (int) _maxExtraDelayTime; //(new MenuSlider((int)maxExtraDelayTime, 0, 200));

                                PingList.Clear();

                                Console.WriteLine("Max Extra Delay: " + _maxExtraDelayTime);
                            }

                            Console.WriteLine("Extra Delay: " + Math.Max(moveTime - Game.Ping, 0));
                        }
                    }
                }
            }

            _checkPing = false;
        }
    }
}
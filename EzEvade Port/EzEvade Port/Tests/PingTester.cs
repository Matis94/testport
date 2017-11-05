namespace EzEvade_Port.Tests
{
    using System;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Menu.Components;
    using Helpers;
    using Utils;

    class PingTester
    {
        public static Menu TestMenu;

        private static bool _lastRandomMoveCoeff;

        private static float _sumPingTime;
        private static float _averagePingTime = ObjectCache.GamePing;
        private static int _testCount;
        private static int _autoTestCount;
        private static float _maxPingTime = ObjectCache.GamePing;

        private static bool _autoTestPing;

        private static EvadeCommand _lastTestMoveToCommand;

        public PingTester()
        {
            Game.OnUpdate += Game_OnGameUpdate;

            TestMenu = new Menu("PingTest", "Ping Tester", true);
            TestMenu.Add(new MenuBool("AutoSetPing", "Auto Set Ping"));
            TestMenu.Add(new MenuBool("TestMoveTime", "Test Ping"));
            TestMenu.Add(new MenuBool("SetMaxPing", "Set Max Ping"));
            TestMenu.Add(new MenuBool("SetAvgPing", "Set Avg Ping"));
            TestMenu.Add(new MenuBool("Test20MoveTime", "Test Ping x20"));
            TestMenu.Add(new MenuBool("PrintResults", "Print Results"));
            TestMenu.Attach();
        }

        private static Obj_AI_Hero MyHero => ObjectManager.GetLocalPlayer();

        private void IssueTestMove(int recursionCount)
        {
            var movePos = ObjectCache.MyHeroCache.ServerPos2D;

            var rand = new Random();

            _lastRandomMoveCoeff = !_lastRandomMoveCoeff;
            if (_lastRandomMoveCoeff)
            {
                movePos.X += 65 + rand.Next(0, 20);
            }
            else
            {
                movePos.X -= 65 + rand.Next(0, 20);
            }

            _lastTestMoveToCommand = new EvadeCommand {Order = EvadeOrderCommand.MoveTo, TargetPosition = movePos, Timestamp = Environment.TickCount, IsProcessed = false};
            MyHero.IssueOrder(OrderType.MoveTo, movePos.To3D());

            if (recursionCount > 1)
            {
                DelayAction.Add(500, () => IssueTestMove(recursionCount - 1));
            }
        }

        private void SetPing(int ping)
        {
            ObjectCache.MenuCache.Cache["ExtraPingBuffer"].As<MenuSlider>().Value = ping;
        }

        private void Game_OnGameUpdate()
        {
            if (TestMenu["AutoSetPing"].Enabled)
            {
                Console.WriteLine("Testing Ping...Please wait 10 seconds");

                var testAmount = 20;

                TestMenu["AutoSetPing"].As<MenuBool>().Value = false;
                IssueTestMove(testAmount);
                _autoTestCount = _testCount + testAmount;
                _autoTestPing = true;
            }

            if (TestMenu["PrintResults"].Enabled)
            {
                TestMenu["PrintResults"].As<MenuBool>().Value = false;

                Console.WriteLine("Average Extra Delay: " + _averagePingTime);
                Console.WriteLine("Max Extra Delay: " + _maxPingTime);
            }

            if (_autoTestPing && _testCount >= _autoTestCount)
            {
                Console.WriteLine("Auto Set Ping Complete");

                Console.WriteLine("Average Extra Delay: " + _averagePingTime);
                Console.WriteLine("Max Extra Delay: " + _maxPingTime);

                SetPing((int) (_averagePingTime + 10));
                Console.WriteLine("Set Average extra ping + 10: " + (_averagePingTime + 10));

                _autoTestPing = false;
            }

            if (TestMenu["TestMoveTime"].Enabled)
            {
                TestMenu["TestMoveTime"].As<MenuBool>().Value = false;
                IssueTestMove(1);
            }

            if (TestMenu["Test20MoveTime"].Enabled)
            {
                TestMenu["Test20MoveTime"].As<MenuBool>().Value = false;
                IssueTestMove(20);
            }

            if (TestMenu["SetMaxPing"].Enabled)
            {
                TestMenu["SetMaxPing"].As<MenuBool>().Value = false;

                if (_testCount < 10)
                {
                    Console.WriteLine("Please test 10 times before setting ping");
                }
                else
                {
                    Console.WriteLine("Set Max extra ping: " + _maxPingTime);
                    SetPing((int) _maxPingTime);
                }
            }

            if (TestMenu["SetAvgPing"].Enabled)
            {
                TestMenu["SetAvgPing"].As<MenuBool>().Value = false;

                if (_testCount < 10)
                {
                    Console.WriteLine("Please test 10 times before setting ping");
                }
                else
                {
                    Console.WriteLine("Set Average extra ping: " + _averagePingTime);
                    SetPing((int) _averagePingTime);
                }
            }

            if (MyHero.HasPath)
            {
                if (_lastTestMoveToCommand != null && _lastTestMoveToCommand.IsProcessed == false && _lastTestMoveToCommand.Order == EvadeOrderCommand.MoveTo)
                {
                    var path = MyHero.Path;

                    if (path.Length > 0)
                    {
                        var movePos = path[path.Length - 1].To2D();

                        if (movePos.Distance(_lastTestMoveToCommand.TargetPosition) < 10)
                        {
                            var moveTime = Environment.TickCount - _lastTestMoveToCommand.Timestamp - ObjectCache.GamePing;
                            Console.WriteLine("Extra Delay: " + moveTime);
                            _lastTestMoveToCommand.IsProcessed = true;

                            _sumPingTime += moveTime;
                            _testCount += 1;
                            _averagePingTime = _sumPingTime / _testCount;
                            _maxPingTime = Math.Max(_maxPingTime, moveTime);
                        }
                    }
                }
            }
        }
    }
}
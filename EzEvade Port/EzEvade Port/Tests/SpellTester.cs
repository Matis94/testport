namespace EzEvade_Port.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Threading;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Menu.Components;
    using Aimtec.SDK.Util;
    using Draw;
    using Helpers;
    using Spells;
    using Utils;
   // using DelayAction = Utils.DelayAction;
    using SpellData = Spells.SpellData;

    class SpellTester
    {
        public static Menu Menu;
        public static Menu SelectSpellMenu;

        private static readonly Dictionary<string, Dictionary<string, SpellData>> SpellCache = new Dictionary<string, Dictionary<string, SpellData>>();

        public static Vector3 SpellStartPosition = MyHero.ServerPosition;

        public static Vector3 SpellEndPostion = MyHero.ServerPosition + (MyHero.Orientation.To2D().Perpendicular() * 500).To3D();

        public static float LastSpellFireTime;

        public SpellTester()
        {
            Menu = new Menu("DummySpellTester", "Spell Tester", true);

            SelectSpellMenu = new Menu("SelectSpellMenu", "Select Spell");
            Menu.Add(SelectSpellMenu);

            var setSpellPositionMenu = new Menu("SetPositionMenu", "Set Spell Position")
            {
                new MenuBool("SetDummySpellStartPosition", "Set Start Position"),
                new MenuBool("SetDummySpellEndPosition", "Set End Position")
            };
            setSpellPositionMenu["SetDummySpellStartPosition"].OnValueChanged += OnSpellStartChange;
            setSpellPositionMenu["SetDummySpellEndPosition"].OnValueChanged += OnSpellEndChange;

            Menu.Add(setSpellPositionMenu);

            var fireDummySpellMenu = new Menu("FireDummySpellMenu", "Fire Dummy Spell")
            {
                new MenuKeyBind("FireDummySpell", "Fire Dummy Spell Key", KeyCode.O, KeybindType.Press),
                new MenuSlider("SpellInterval", "Spell Interval", 2500, 0, 5000)
            };


            Menu.Add(fireDummySpellMenu);
            ObjectCache.MenuCache.AddMenuToCache(Menu);
            Menu.Attach();

            LoadSpellDictionary();

            Game.OnUpdate += Game_OnGameUpdate;
            Render.OnPresent += Render_OnPresent;
        }

        private static Obj_AI_Hero MyHero => ObjectManager.GetLocalPlayer();

        private void Render_OnPresent()
        {
            foreach (var spell in SpellDetector.DrawSpells.Values)
            {
                var spellPos = spell.CurrentSpellPosition;

                if (spell.HeroId != MyHero.NetworkId)
                {
                    continue;
                }

                switch (spell.SpellType)
                {
                    case SpellType.Line:
                        if (Vector2.Distance(spellPos, MyHero.ServerPosition.To2D()) <= MyHero.BoundingRadius + spell.Radius && Environment.TickCount - spell.StartTime > spell.Info.SpellDelay &&
                            Vector2.Distance(spell.StartPos, MyHero.ServerPosition.To2D()) < spell.Info.Range)
                        {
                            RenderObjects.Add(new RenderCircle(spellPos, 1000, Color.Red, (int) spell.Radius, 10));
                            Aimtec.SDK.Util.DelayAction.Queue(100, ()=> SpellDetector.DeleteSpell(spell.SpellId), new CancellationToken(false));
                           // DelayAction.Add(1, () => SpellDetector.DeleteSpell(spell.SpellId));
                        }
                        else
                        {
                            Render.Circle(new Vector3(spellPos.X, spellPos.Y, MyHero.Position.Z), (int) spell.Radius, 50, Color.White);
                        }
                        break;
                    case SpellType.Circular:
                        if (Environment.TickCount - spell.StartTime >= spell.EndTime - spell.StartTime)
                        {
                            if (!MyHero.ServerPosition.To2D().InSkillShot(spell, MyHero.BoundingRadius))
                            {
                                continue;
                            }

                            RenderObjects.Add(new RenderCircle(spellPos, 1000, Color.Red, (int) spell.Radius));
                            Aimtec.SDK.Util.DelayAction.Queue(100, () => SpellDetector.DeleteSpell(spell.SpellId), new CancellationToken(false));
                        }
                        break;
                    case SpellType.Cone:
                        // SPELL TESTER
                        if (Environment.TickCount - spell.StartTime >= spell.EndTime - spell.StartTime)
                        {
                            if (MyHero.ServerPosition.To2D().InSkillShot(spell, MyHero.BoundingRadius))
                            {
                                Aimtec.SDK.Util.DelayAction.Queue(100, () => SpellDetector.DeleteSpell(spell.SpellId), new CancellationToken(false));
                            }
                        }
                        break;
                }
            }
        }

        private void Game_OnGameUpdate()
        {
            if (!Menu["FireDummySpellMenu"]["FireDummySpell"].Enabled)
            {
                return;
            }

            if (Environment.TickCount - LastSpellFireTime > Menu["SpellInterval"].As<MenuSlider>().Value)
            {
                var charName = SelectSpellMenu["DummySpellHero"].As<MenuList>().SelectedItem;
                var spellName = SelectSpellMenu["DummySpellList"].As<MenuList>().SelectedItem;
                var spellData = SpellCache[charName][spellName];

                if (!ObjectCache.MenuCache.Cache.ContainsKey(spellName + "DodgeSpell"))
                {
                    SpellDetector.LoadDummySpell(spellData);
                }

                SpellDetector.CreateSpellData(MyHero, SpellStartPosition, SpellEndPostion, spellData);
                LastSpellFireTime = Environment.TickCount;
            }
        }

        private void OnSpellEndChange(MenuComponent sender, ValueChangedArgs e)
        {
            
            SpellEndPostion = MyHero.ServerPosition;
            RenderObjects.Add(new RenderCircle(SpellEndPostion.To2D(), 1000, Color.Red, 100, 20));
        }

        private void OnSpellStartChange(MenuComponent sender, ValueChangedArgs e)
        {
          
            SpellStartPosition = MyHero.ServerPosition;
            RenderObjects.Add(new RenderCircle(SpellStartPosition.To2D(), 1000, Color.Red, 100, 20));
        }

        private void LoadSpellDictionary()
        {
            foreach (var spell in SpellDatabase.Spells)
            {
                if (SpellCache.ContainsKey(spell.CharName))
                {
                    var spellList = SpellCache[spell.CharName];
                    if (spellList != null && !spellList.ContainsKey(spell.SpellName))
                    {
                        spellList.Add(spell.SpellName, spell);
                    }
                }
                else
                {
                    SpellCache.Add(spell.CharName, new Dictionary<string, SpellData>());
                    var spellList = SpellCache[spell.CharName];
                    if (spellList != null && !spellList.ContainsKey(spell.SpellName))
                    {
                        spellList.Add(spell.SpellName, spell);
                    }
                }
            }

            SelectSpellMenu.Add(new MenuBool("DummySpellDescription", "    -- Select A Dummy Spell To Fire --    "));

            var heroList = SpellCache.Keys.ToArray();
            SelectSpellMenu.Add(new MenuList("DummySpellHero", "Hero", heroList, 0));

            var selectedHeroStr = SelectSpellMenu["DummySpellHero"].As<MenuList>().SelectedItem;
            var selectedHero = SpellCache[selectedHeroStr];
            var selectedHeroList = selectedHero.Keys.ToArray();

            SelectSpellMenu.Add(new MenuList("DummySpellList", "Spell", selectedHeroList, 0));

            SelectSpellMenu["DummySpellHero"].OnValueChanged += OnSpellHeroChange;
        }

        private void OnSpellHeroChange(MenuComponent sender, ValueChangedArgs args)
        {
            //var previousHeroStr = e.GetOldValue<MenuList>().SelectedValue;
            var selectedHeroStr = args.GetNewValue<MenuList>().SelectedItem;
            var selectedHero = SpellCache[selectedHeroStr];
            var selectedHeroList = selectedHero.Keys.ToArray();

            SelectSpellMenu["DummySpellList"].As<MenuList>().Items = selectedHeroList;
        }
    }
}
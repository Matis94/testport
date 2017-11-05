namespace EzEvade_Port.Helpers
{
    using System;
    using System.Collections.Generic;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Menu.Components;
    using Core;
    using Utils;

    public class HeroInfo
    {
        public float BoundingRadius;
        public Vector2 CurrentPosition;
        public bool HasPath;
        public Obj_AI_Hero Hero;
        public float MoveSpeed;
        public Vector2 ServerPos2D;
        public Vector2 ServerPos2DExtra;
        public Vector2 ServerPos2DPing;

        public HeroInfo(Obj_AI_Hero hero)
        {
            this.Hero = hero;
            Game.OnUpdate += Game_OnGameUpdate;
        }

        private void Game_OnGameUpdate()
        {
            UpdateInfo();
        }

        public void UpdateInfo()
        {
            try
            {
               
                ServerPos2D = Hero.ServerPosition.To2D(); //CalculatedPosition.GetPosition(hero, Game.Ping);
                ServerPos2DExtra = EvadeUtils.GetGamePosition(Hero, Game.Ping + ObjectCache.MenuCache.Cache["ExtraPingBuffer"].Value);
                ServerPos2DPing = EvadeUtils.GetGamePosition(Hero, Game.Ping);
                //CalculatedPosition.GetPosition(hero, Game.Ping + extraDelayBuffer);            
                CurrentPosition = Hero.Position.To2D(); //CalculatedPosition.GetPosition(hero, 0); 
                BoundingRadius = Hero.BoundingRadius;
                MoveSpeed = Hero.MoveSpeed;
                HasPath = Hero.HasPath;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public class MenuCache
    {
        public Dictionary<string, MenuComponent> Cache = new Dictionary<string, MenuComponent>();
        public Menu Menu;

        public MenuCache(Menu menu)
        {
            this.Menu = menu;

            AddMenuToCache(menu);
        }

        public void AddMenuToCache(Menu newMenu)
        {
            foreach (var item in ReturnAllItems(newMenu))
            {
                AddMenuComponentToCache(item);
            }
        }

        public void AddMenuComponentToCache(MenuComponent item)
        {
            if (item != null && !Cache.ContainsKey(item.InternalName))
            {
                Cache.Add(item.InternalName, item);
            }
        }

        public static List<MenuComponent> ReturnAllItems(Menu menu)
        {
            var menuList = new List<MenuComponent>();

            foreach (var item in menu.Children.Values)
            {
                if (item != null)
                {
                    Console.WriteLine(item.InternalName);
                    menuList.Add(item);
                }

                var asmenu = item as Menu;

                if (asmenu == null)
                {
                    continue;
                }

                foreach (var item2 in asmenu.Children.Values)
                {
                    if (item2 == item || item2 == null)
                    {
                        continue;
                    }

                    Console.WriteLine(item2.InternalName);
                    menuList.Add(item2);
                }
            }
        
            return menuList;
        }
    }

    public static class ObjectCache
    {
        public static Dictionary<int, Obj_AI_Turret> Turrets = new Dictionary<int, Obj_AI_Turret>();

        public static HeroInfo MyHeroCache = new HeroInfo(ObjectManager.GetLocalPlayer());
        public static MenuCache MenuCache = new MenuCache(Evade.Menu);

        public static float GamePing;

        static ObjectCache()
        {
            InitializeCache();
            Game.OnUpdate += Game_OnGameUpdate;
        }

        private static void Game_OnGameUpdate()
        {
            GamePing = Game.Ping;
        }

        private static void InitializeCache()
        {
            foreach (var obj in ObjectManager.Get<Obj_AI_Turret>())
            {
                if (!Turrets.ContainsKey(obj.NetworkId))
                {
                    Turrets.Add(obj.NetworkId, obj);
                }
            }
        }
    }
}
namespace EzEvade_Port.Spells
{
    using System.Drawing;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Menu.Components;
    using Core;
    using Helpers;

    internal class SpellDrawer
    {
        public static Menu Menu;

        public SpellDrawer(Menu mainMenu)
        {
            Render.OnPresent += Render_OnPresent;

            Menu = mainMenu;
            Game_OnGameLoad();
        }

        private static Obj_AI_Hero MyHero => ObjectManager.GetLocalPlayer();

        private static void Game_OnGameLoad()
        {
            Evade.DrawMenu = new Menu("Draw", "Draw")
            {
                new MenuBool("DrawSkillShots", "Draw SkillShots"),
                new MenuBool("ShowStatus", "Show Evade Status"),
                new MenuBool("DrawSpellPos", "Draw Spell Position"),
                new MenuBool("DrawEvadePosition", "Draw Evade Position")
            };

            var dangerMenu = new Menu("DangerLevelDrawings", "Danger Level Drawings")
            {
                new MenuSlider("LowWidth", "Line Width", 3, 1, 15),
                new MenuSlider("NormalWidth", "Line Width", 3, 1, 15),
                new MenuSlider("HighWidth", "Line Width", 3, 1, 15),
                new MenuSlider("ExtremeWidth", "Line Width", 4, 1, 15)
            };

            Evade.DrawMenu.Add(dangerMenu);
            ObjectCache.MenuCache.AddMenuToCache(Evade.DrawMenu);
            Menu.Add(Evade.DrawMenu);
        }

        private static void DrawLineRectangle(Vector2 start, Vector2 end, int radius, int width, Color color)
        {
            var rectangle = new Geometry.Rectangle(start.To3D(), end.To3D(), radius);
            rectangle.ToPolygon().Draw(color);
        }

        private static void DrawLineTriangle(Vector2 start, Vector2 end, int radius, int width, Color color)
        {
            var dir = (end - start).Normalized();
            var pDir = dir.Perpendicular();

            var initStartPos = start + dir;
            var rightEndPos = end + pDir * radius;
            var leftEndPos = end - pDir * radius;

            Render.WorldToScreen(new Vector3(initStartPos.X, initStartPos.Y, MyHero.Position.Z), out var iStartPos);
            Render.WorldToScreen(new Vector3(rightEndPos.X, rightEndPos.Y, MyHero.Position.Z), out var rEndPos);
            Render.WorldToScreen(new Vector3(leftEndPos.X, leftEndPos.Y, MyHero.Position.Z), out var lEndPos);

            Render.Line(iStartPos, rEndPos, color);
            Render.Line(iStartPos, lEndPos, color);
            Render.Line(rEndPos, lEndPos, color);
        }

        private static void DrawEvadeStatus()
        {
            if (!ObjectCache.MenuCache.Cache["ShowStatus"].Enabled)
            {
                return;
            }

            Render.WorldToScreen(ObjectManager.GetLocalPlayer().Position, out var heroPos);

            if (ObjectCache.MenuCache.Cache["DodgeSkillShots"].Enabled)
            {
                if (Evade.IsDodging)
                {
                    Render.Text("Evade: ON", new Vector2(heroPos.X - 10, heroPos.Y), RenderTextFlags.Center, Color.Red);
                }
                else
                {
                    if (ObjectCache.MenuCache.Cache["DodgeOnlyOnComboKeyEnabled"].Enabled && !ObjectCache.MenuCache.Cache["DodgeComboKey"].As<MenuKeyBind>().Enabled)
                    {
                        Render.Text("Evade: OFF", new Vector2(heroPos.X - 10, heroPos.Y), RenderTextFlags.Center, Color.Gray);
                    }
                    else
                    {
                        if (ObjectCache.MenuCache.Cache["DontDodgeKeyEnabled"].Enabled && ObjectCache.MenuCache.Cache["DontDodgeKey"].As<MenuKeyBind>().Enabled)
                        {
                            Render.Text("Evade: OFF", new Vector2(heroPos.X - 10, heroPos.Y), RenderTextFlags.Center, Color.Gray);
                        }
                        else if (Evade.IsDodgeDangerousEnabled())
                        {
                            Render.Text("Evade: ON", new Vector2(heroPos.X - 10, heroPos.Y), RenderTextFlags.Center, Color.Yellow);
                        }
                        else
                        {
                            Render.Text("Evade: ON", new Vector2(heroPos.X - 10, heroPos.Y), RenderTextFlags.Center, Color.Lime);
                        }
                    }
                }
            }
            else
            {
                if (ObjectCache.MenuCache.Cache["ActivateEvadeSpells"].As<MenuKeyBind>().Enabled)
                {
                    if (ObjectCache.MenuCache.Cache["DodgeOnlyOnComboKeyEnabled"].Enabled && !ObjectCache.MenuCache.Cache["DodgeComboKey"].As<MenuKeyBind>().Enabled)
                    {
                        Render.Text("Evade: OFF", new Vector2(heroPos.X - 10, heroPos.Y), RenderTextFlags.Center, Color.Gray);
                    }
                    else
                    {
                        Render.Text("Evade: Spell", new Vector2(heroPos.X - 10, heroPos.Y), RenderTextFlags.Center, Evade.IsDodgeDangerousEnabled() ? Color.Yellow : Color.DeepSkyBlue);
                    }
                }
                else
                {
                    Render.Text("Evade: OFF", new Vector2(heroPos.X - 10, heroPos.Y), RenderTextFlags.Center, Color.Gray);
                }
            }
        }

        private static void Render_OnPresent()
        {
            if (ObjectCache.MenuCache.Cache["DrawEvadePosition"].Enabled)
            {
                if (Evade.LastPosInfo != null)
                {
                    var pos = Evade.LastPosInfo.Position; //Evade.lastEvadeCommand.targetPosition;
                    Render.WorldToScreen(new Vector3(pos.X, pos.Y, MyHero.Position.Z), out var screenPos);
                    Render.Circle(screenPos.To3D(), 65, 10, Color.Red);
                }
            }

            DrawEvadeStatus();

            if (!ObjectCache.MenuCache.Cache["DrawSkillShots"].Enabled)
            {
                return;
            }

            foreach (var entry in SpellDetector.DrawSpells)
            {
                if (entry.Value == null)
                {
                    continue;
                }

                var spell = entry.Value;

                var dangerStr = spell.GetSpellDangerString();

                var spellDrawingWidth = ObjectCache.MenuCache.Cache["DangerLevelDrawings"][dangerStr + "Width"].As<MenuSlider>().Value;
               
                if (!Evade.SpellMenu[spell.Info.CharName + spell.Info.SpellName + "Settings"][spell.Info.SpellName + "DrawSpell"].Enabled)
                {
                    continue;
                }

                var canEvade = !(Evade.LastPosInfo != null && Evade.LastPosInfo.UndodgeableSpells.Contains(spell.SpellId)) || !Evade.DevModeOn;

                switch (spell.SpellType)
                {
                    case SpellType.Line:
                        var spellPos = spell.CurrentSpellPosition;
                        var spellEndPos = spell.GetSpellEndPosition();

                        DrawLineRectangle(spellPos, spellEndPos, (int) spell.Radius, spellDrawingWidth, !canEvade ? Color.Yellow : Color.White);

                        if (ObjectCache.MenuCache.Cache["DrawSpellPos"].Enabled)
                        {
                            Render.Circle(new Vector3(spellPos.To3D().X, spellPos.To3D().Y, spell.Height), (int) spell.Radius, 50, !canEvade ? Color.Yellow : Color.White);
                        }
                        break;
                    case SpellType.Circular:
                        Render.Circle(new Vector3(spell.EndPos.To3D().X, spell.EndPos.To3D().Y, spell.Height), (int) spell.Radius, 50, !canEvade ? Color.Yellow : Color.White);

                        switch (spell.Info.SpellName)
                        {
                            case "VeigarEventHorizon":
                                Render.Circle(new Vector3(spell.EndPos.To3D().X, spell.EndPos.To3D().Y, spell.Height), (int) spell.Radius - 125, 50, !canEvade ? Color.Yellow : Color.White);
                                break;
                            case "DariusCleave":
                                Render.Circle(new Vector3(spell.EndPos.To3D().X, spell.EndPos.To3D().Y, spell.Height), (int) spell.Radius - 220, 50, !canEvade ? Color.Yellow : Color.White);
                                break;
                        }
                        break;
                    case SpellType.Cone: DrawLineTriangle(spell.StartPos, spell.EndPos, (int) spell.Radius, spellDrawingWidth, !canEvade ? Color.Yellow : Color.White);
                        break;
                }
            }
        }
    }
}
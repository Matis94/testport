namespace EzEvade_Port.Helpers
{
    using System;
    using Aimtec;
    using Aimtec.SDK.Events;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu.Components;
    using Core;
    using Utils;

    public static class Situation
    {
        private static Obj_AI_Hero MyHero => ObjectManager.GetLocalPlayer();

        public static bool CheckTeam(this Obj_AI_Base unit)
        {
            return unit.Team != MyHero.Team || Evade.DevModeOn;
        }

        public static bool CheckTeam(this GameObject unit)
        {
            return unit.Team != MyHero.Team || Evade.DevModeOn;
        }

        public static string EmitterColor()
        {
            return Evade.DevModeOn ? "green" : "red";
        }

        public static string EmitterTeam()
        {
            return Evade.DevModeOn ? "ally" : "enemy";
        }

        public static bool IsNearEnemy(this Vector2 pos, float distance, bool alreadyNear = true)
        {
            if (!ObjectCache.MenuCache.Cache["PreventDodgingNearEnemy"].Enabled)
            {
                return false;
            }

            var curDistToEnemies = ObjectCache.MyHeroCache.ServerPos2D.GetDistanceToChampions();
            var posDistToEnemies = pos.GetDistanceToChampions();

            if (curDistToEnemies < distance)
            {
                if (curDistToEnemies > posDistToEnemies)
                {
                    return true;
                }
            }
            else
            {
                if (posDistToEnemies < distance)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsUnderTurret(this Vector2 pos, bool checkEnemy = true)
        {
            if (!ObjectCache.MenuCache.Cache["PreventDodgingUnderTower"].Enabled)
            {
                return false;
            }

            var turretRange = 875 + ObjectCache.MyHeroCache.BoundingRadius;

            foreach (var entry in ObjectCache.Turrets)
            {
                var turret = entry.Value;
                if (turret == null || !turret.IsValid || turret.IsDead)
                {
                    DelayAction.Add(1, () => ObjectCache.Turrets.Remove(entry.Key));
                    continue;
                }

                if (checkEnemy && turret.IsAlly)
                {
                    continue;
                }

                var distToTurret = pos.Distance(turret.Position.To2D());
                if (distToTurret <= turretRange)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool ShouldDodge()
        {
            // fix
            if (ObjectCache.MenuCache.Cache["DontDodgeKeyEnabled"].Enabled && ObjectCache.MenuCache.Cache["DontDodgeKey"].As<MenuKeyBind>().Enabled)
            {
                return false;
            }

            return ObjectCache.MenuCache.Cache["DodgeSkillShots"].Enabled && !CommonChecks();
        }

        public static bool ShouldUseEvadeSpell()
        {
            // fix
            if (ObjectCache.MenuCache.Cache["DontDodgeKeyEnabled"].Enabled && ObjectCache.MenuCache.Cache["DontDodgeKey"].As<MenuKeyBind>().Enabled)
            {
                return false;
            }

            return ObjectCache.MenuCache.Cache["ActivateEvadeSpells"].Enabled && !CommonChecks() && !(Evade.LastWindupTime - Environment.TickCount > 0);
        }

        public static bool CommonChecks()
        {
            return Evade.IsChanneling || !ObjectCache.MenuCache.Cache["DodgeOnlyOnComboKeyEnabled"].Enabled && !ObjectCache.MenuCache.Cache["DodgeComboKey"].As<MenuKeyBind>().Enabled ||
                   MyHero.IsDead || MyHero.IsInvulnerable || MyHero.IsTargetable == false || HasSpellShield(MyHero) || ChampionSpecificChecks() || MyHero.IsDashing() || Evade.HasGameEnded;
        }

        public static bool ChampionSpecificChecks()
        {
            return MyHero.ChampionName == "Sion" && MyHero.HasBuff("SionR");
        }

        //from Evade by Esk0r
        public static bool HasSpellShield(Obj_AI_Hero unit)
        {
            if (ObjectManager.GetLocalPlayer().HasBuffOfType(BuffType.SpellShield))
            {
                return true;
            }

            if (ObjectManager.GetLocalPlayer().HasBuffOfType(BuffType.SpellImmunity))
            {
                return true;
            }

            if (unit.LastCastedSpellName() == "SivirE" && Environment.TickCount - Evade.LastSpellCastTime < 300)
            {
                return true;
            }

            if (unit.LastCastedSpellName() == "BlackShield" && Environment.TickCount - Evade.LastSpellCastTime < 300)
            {
                return true;
            }

            return unit.LastCastedSpellName() == "NocturneShit" && Environment.TickCount - Evade.LastSpellCastTime < 300;
        }
    }
}
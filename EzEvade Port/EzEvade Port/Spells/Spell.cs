namespace EzEvade_Port.Spells
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu.Components;
    using Aimtec.SDK.Util.Cache;
    using Core;
    using EvadeSpells;
    using Helpers;
    using Utils;

    public class Spell
    {
        public Vector2 CnLeft;
        public Vector2 CnRight;
        public Vector2 CnStart;
        public Vector2 CurrentNegativePosition = Vector2.Zero;
        public Vector2 CurrentSpellPosition = Vector2.Zero;
        public int Dangerlevel = 1;
        public Vector2 Direction;
        public Vector2 EndPos;
        public float EndTime;

        public float EvadeTime = float.MinValue;
        public float Height;
        public int HeroId;
        public SpellData Info;
        public Vector2 PredictedEndPos = Vector2.Zero;
        public int ProjectileId;

        public float Radius = 0;
        public float SpellHitTime = float.MinValue;
        public int SpellId;
        public GameObject SpellObject = null;
        public SpellType SpellType;
        public Vector2 StartPos;
        public float StartTime;
    }

    public static class SpellExtensions
    {
        public static float GetSpellRadius(this Spell spell)
        {
            var radius = Evade.SpellMenu[spell.Info.CharName + spell.Info.SpellName + "Settings"][spell.Info.SpellName + "SpellRadius"].As<MenuSlider>().Value;
            var extraRadius = ObjectCache.MenuCache.Cache["ExtraSpellRadius"].As<MenuSlider>().Value;

            if (spell.Info.HasEndExplosion && spell.SpellType == SpellType.Circular)
            {
                return spell.Info.SecondaryRadius + extraRadius;
            }

            if (spell.SpellType == SpellType.Arc)
            {
                var spellRange = spell.StartPos.Distance(spell.EndPos);
                var arcRadius = spell.Info.Radius * (1 + spellRange / 100) + extraRadius;

                return arcRadius;
            }

            return radius + extraRadius;
        }

        public static int GetSpellDangerLevel(this Spell spell)
        {
            var dangerStr = Evade.SpellMenu[spell.Info.CharName + spell.Info.SpellName + "Settings"][spell.Info.SpellName + "DangerLevel"].As<MenuList>().SelectedItem;

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

        public static string GetSpellDangerString(this Spell spell)
        {
            switch (spell.GetSpellDangerLevel())
            {
                case 1: return "Low";
                case 3: return "High";
                case 4: return "Extreme";
                default: return "Normal";
            }
        }

        public static bool HasProjectile(this Spell spell)
        {
            return spell.Info.ProjectileSpeed > 0 && spell.Info.ProjectileSpeed != float.MaxValue;
        }

        public static Vector2 GetSpellProjection(this Spell spell, Vector2 pos, bool predictPos = false)
        {
            switch (spell.SpellType)
            {
                case SpellType.Line:
                    if (predictPos)
                    {
                        var spellPos = spell.CurrentSpellPosition;
                        var spellEndPos = spell.GetSpellEndPosition();

                        return pos.ProjectOn(spellPos, spellEndPos).SegmentPoint;
                    }

                    return pos.ProjectOn(spell.StartPos, spell.EndPos).SegmentPoint;
                case SpellType.Arc:
                    if (predictPos)
                    {
                        var spellPos = spell.CurrentSpellPosition;
                        var spellEndPos = spell.GetSpellEndPosition();

                        return pos.ProjectOn(spellPos, spellEndPos).SegmentPoint;
                    }

                    return pos.ProjectOn(spell.StartPos, spell.EndPos).SegmentPoint;
                case SpellType.Circular: return spell.EndPos;
                case SpellType.Cone: break;
            }

            return Vector2.Zero;
        }

        public static Obj_AI_Base CheckSpellCollision(this Spell spell, bool ignoreSelf = true)
        {
            if (!spell.Info.CollisionObjects.Any())
            {
                return null;
            }

            var collisionCandidates = new List<Obj_AI_Base>();
            var spellPos = spell.CurrentSpellPosition;
            var distanceToHero = spellPos.Distance(ObjectCache.MyHeroCache.ServerPos2D);

            if (spell.Info.CollisionObjects.Contains(CollisionObjectType.EnemyChampions))
            {
                collisionCandidates.AddRange(GameObjects.AllyHeroes.Where(h => h.IsValidTarget(distanceToHero, false, true, spellPos.To3D())).
                                                 Where(hero => !ignoreSelf || !hero.IsMe).
                                                 Cast<Obj_AI_Base>());
            }

            if (spell.Info.CollisionObjects.Contains(CollisionObjectType.EnemyMinions))
            {
                collisionCandidates.AddRange(ObjectManager.Get<Obj_AI_Minion>().
                                                 Where(h => h.Team == Evade.MyHero.Team && h.IsValidTarget(distanceToHero, false, true, spellPos.To3D())).
                                                 Where(minion => minion.UnitSkinName.ToLower() != "teemomushroom" && minion.UnitSkinName.ToLower() != "shacobox").
                                                 Cast<Obj_AI_Base>());
            }

            var sortedCandidates = collisionCandidates.OrderBy(h => h.Distance(spellPos));

            return sortedCandidates.FirstOrDefault(candidate => candidate.ServerPosition.To2D().InSkillShot(spell, candidate.BoundingRadius, false));
        }

        public static float GetSpellHitTime(this Spell spell, Vector2 pos)
        {
            switch (spell.SpellType)
            {
                case SpellType.Line:
                    if (spell.Info.ProjectileSpeed >= float.MaxValue)
                    {
                        return Math.Max(0, spell.EndTime - Environment.TickCount - ObjectCache.GamePing);
                    }

                    var spellPos = spell.GetCurrentSpellPosition(true, ObjectCache.GamePing);
                    return 1000 * spellPos.Distance(pos) / spell.Info.ProjectileSpeed;
                case SpellType.Cone:
                case SpellType.Circular: return Math.Max(0, spell.EndTime - Environment.TickCount - ObjectCache.GamePing);
            }

            return float.MaxValue;
        }

        public static bool CanHeroEvade(this Spell spell, Obj_AI_Base hero, out float rEvadeTime, out float rSpellHitTime)
        {
            var heroPos = hero.ServerPosition.To2D();
            float evadeTime = 0;
            float spellHitTime = 0;
            var speed = hero.MoveSpeed;
            float delay = 0;

            var moveBuff = EvadeSpell.EvadeSpells.OrderBy(s => s.Dangerlevel).FirstOrDefault(s => s.EvadeType == EvadeType.MovementSpeedBuff);
            if (moveBuff != null && EvadeSpell.ShouldUseMovementBuff(spell))
            {
                speed += speed * moveBuff.SpeedArray[ObjectManager.GetLocalPlayer().GetSpell(moveBuff.SpellKey).Level - 1] / 100;
                delay += (moveBuff.SpellDelay > 50 ? moveBuff.SpellDelay : 0) + ObjectCache.GamePing;
            }

            switch (spell.SpellType)
            {
                case SpellType.Line:
                    var projection = heroPos.ProjectOn(spell.StartPos, spell.EndPos).SegmentPoint;
                    evadeTime = 1000 * (spell.Radius - heroPos.Distance(projection) + hero.BoundingRadius) / speed;
                    spellHitTime = spell.GetSpellHitTime(projection);
                    break;
                case SpellType.Circular:
                    evadeTime = 1000 * (spell.Radius - heroPos.Distance(spell.EndPos)) / speed;
                    spellHitTime = spell.GetSpellHitTime(heroPos);
                    break;
                case SpellType.Cone:
                    var sides = new[]
                    {
                        heroPos.ProjectOn(spell.CnStart, spell.CnLeft).SegmentPoint,
                        heroPos.ProjectOn(spell.CnLeft, spell.CnRight).SegmentPoint,
                        heroPos.ProjectOn(spell.CnRight, spell.CnStart).SegmentPoint
                    };

                    var p = sides.OrderBy(x => x.Distance(x)).First();
                    evadeTime = 1000 * (spell.Info.Range / 2 - heroPos.Distance(p) + hero.BoundingRadius) / speed;
                    spellHitTime = spell.GetSpellHitTime(heroPos);
                    break;
            }

            rEvadeTime = evadeTime;
            rSpellHitTime = spellHitTime;

            return spellHitTime - delay > evadeTime;
        }

        public static BoundingBox GetLinearSpellBoundingBox(this Spell spell)
        {
            var myBoundingRadius = ObjectCache.MyHeroCache.BoundingRadius;
            var spellDir = spell.Direction;
            var pSpellDir = spell.Direction.Perpendicular();
            var spellRadius = spell.Radius;
            var spellPos = spell.CurrentSpellPosition - spellDir * myBoundingRadius;
            var endPos = spell.GetSpellEndPosition() + spellDir * myBoundingRadius;

            var startRightPos = spellPos + pSpellDir * (spellRadius + myBoundingRadius);
            var endLeftPos = endPos - pSpellDir * (spellRadius + myBoundingRadius);

            return new BoundingBox(new Vector3(endLeftPos.X, endLeftPos.Y, -1), new Vector3(startRightPos.X, startRightPos.Y, 1));
        }

        public static Vector2 GetSpellEndPosition(this Spell spell)
        {
            return spell.PredictedEndPos == Vector2.Zero ? spell.EndPos : spell.PredictedEndPos;
        }

        public static void UpdateSpellInfo(this Spell spell)
        {
            spell.CurrentSpellPosition = spell.GetCurrentSpellPosition();
            spell.CurrentNegativePosition = spell.GetCurrentSpellPosition(true);
            spell.Dangerlevel = spell.GetSpellDangerLevel();
        }

        public static Vector2 GetCurrentSpellPosition(this Spell spell, bool allowNegative = false, float delay = 0, float extraDistance = 0)
        {
            var spellPos = spell.StartPos;

            if (spell.Info.UpdatePosition == false)
            {
                return spellPos;
            }

            if (spell.SpellType == SpellType.Line || spell.SpellType == SpellType.Arc)
            {
                var spellTime = Environment.TickCount - spell.StartTime - spell.Info.SpellDelay - Math.Max(0, spell.Info.ExtraEndTime);

                if (spell.Info.ProjectileSpeed == float.MaxValue)
                {
                    return spell.StartPos;
                }

                if (spellTime >= 0 || allowNegative)
                {
                    spellPos = spell.StartPos + spell.Direction * spell.Info.ProjectileSpeed * (spellTime / 1000);
                }
            }
            else if (spell.SpellType == SpellType.Circular || spell.SpellType == SpellType.Cone)
            {
                spellPos = spell.EndPos;
            }

            if (spell.SpellObject != null && spell.SpellObject.IsValid && spell.SpellObject.IsVisible &&
                spell.SpellObject.Position.To2D().Distance(ObjectCache.MyHeroCache.ServerPos2D) < spell.Info.Range + 1000)
            {
                spellPos = spell.SpellObject.Position.To2D();
            }

            if (delay > 0 && spell.Info.ProjectileSpeed != float.MaxValue && spell.SpellType == SpellType.Line)
            {
                spellPos = spellPos + spell.Direction * spell.Info.ProjectileSpeed * (delay / 1000);
            }

            if (extraDistance > 0 && spell.Info.ProjectileSpeed != float.MaxValue && spell.SpellType == SpellType.Line)
            {
                spellPos = spellPos + spell.Direction * extraDistance;
            }

            return spellPos;
        }

        public static bool LineIntersectLinearSpell(this Spell spell, Vector2 a, Vector2 b)
        {
            var myBoundingRadius = ObjectManager.GetLocalPlayer().BoundingRadius;
            var spellDir = spell.Direction;
            var pSpellDir = spell.Direction.Perpendicular();
            var spellRadius = spell.Radius;
            var spellPos = spell.CurrentSpellPosition; 
            var endPos = spell.GetSpellEndPosition();

            var startRightPos = spellPos + pSpellDir * (spellRadius + myBoundingRadius);
            var startLeftPos = spellPos - pSpellDir * (spellRadius + myBoundingRadius);
            var endRightPos = endPos + pSpellDir * (spellRadius + myBoundingRadius);
            var endLeftPos = endPos - pSpellDir * (spellRadius + myBoundingRadius);

            var int1 = MathUtils.CheckLineIntersection(a, b, startRightPos, startLeftPos);
            var int2 = MathUtils.CheckLineIntersection(a, b, endRightPos, endLeftPos);
            var int3 = MathUtils.CheckLineIntersection(a, b, startRightPos, endRightPos);
            var int4 = MathUtils.CheckLineIntersection(a, b, startLeftPos, endLeftPos);

            if (int1 || int2 || int3 || int4)
            {
                return true;
            }

            return false;
        }

        public static bool LineIntersectLinearSpellEx(this Spell spell, Vector2 a, Vector2 b, out Vector2 intersection) //edited
        {
            var myBoundingRadius = ObjectManager.GetLocalPlayer().BoundingRadius;
            var spellDir = spell.Direction;
            var pSpellDir = spell.Direction.Perpendicular();
            var spellRadius = spell.Radius;
            var spellPos = spell.CurrentSpellPosition - spellDir * myBoundingRadius;
            var endPos = spell.GetSpellEndPosition() + spellDir * myBoundingRadius; //leave some space at the front of spell

            var startRightPos = spellPos + pSpellDir * (spellRadius + myBoundingRadius);
            var startLeftPos = spellPos - pSpellDir * (spellRadius + myBoundingRadius);
            var endRightPos = endPos + pSpellDir * (spellRadius + myBoundingRadius);
            var endLeftPos = endPos - pSpellDir * (spellRadius + myBoundingRadius);

            var intersects = new List<Vector2Extensions.IntersectionResult>();
            var heroPos = ObjectManager.GetLocalPlayer().ServerPosition.To2D();

            intersects.Add(a.Intersection(b, startRightPos, startLeftPos));
            intersects.Add(a.Intersection(b, endRightPos, endLeftPos));
            intersects.Add(a.Intersection(b, startRightPos, endRightPos));
            intersects.Add(a.Intersection(b, startLeftPos, endLeftPos));

            var sortedIntersects = intersects.Where(i => i.Intersects).OrderBy(i => i.Point.Distance(heroPos)); //Get first intersection

            if (sortedIntersects.Any())
            {
                intersection = sortedIntersects.First().Point;
                return true;
            }

            intersection = Vector2.Zero;
            return false;
        }
    }
}
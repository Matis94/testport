namespace EzEvade_Port.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Events;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu.Components;
    using Aimtec.SDK.Util.Cache;
    using EvadeSpells;
    using Helpers;
    using Spells;
    using Utils;
    using Spell = Spells.Spell;

    class EvadeHelper
    {
        public static bool FastEvadeMode;
        private static Obj_AI_Hero MyHero => ObjectManager.GetLocalPlayer();

        public static bool PlayerInSkillShot(Spell spell)
        {
            return ObjectCache.MyHeroCache.ServerPos2D.InSkillShot(spell, ObjectCache.MyHeroCache.BoundingRadius);
        }

        public static PositionInfo InitPositionInfo(Vector2 pos, float extraDelayBuffer, float extraEvadeDistance, Vector2 lastMovePos, Spell lowestEvadeTimeSpell)
        {
            if (!ObjectCache.MyHeroCache.HasPath && ObjectCache.MyHeroCache.ServerPos2D.Distance(pos) <= 75)
            {
                pos = ObjectCache.MyHeroCache.ServerPos2D;
            }

            var extraDist = ObjectCache.MenuCache.Cache["ExtraCPADistance"].As<MenuSlider>().Value;

            PositionInfo posInfo;
            posInfo = CanHeroWalkToPos(pos, ObjectCache.MyHeroCache.MoveSpeed, extraDelayBuffer + ObjectCache.GamePing, extraDist);
            posInfo.IsDangerousPos = pos.CheckDangerousPos(6);
            posInfo.HasExtraDistance = extraEvadeDistance > 0 && pos.CheckDangerousPos(extraEvadeDistance);
            posInfo.ClosestDistance = posInfo.DistanceToMouse;
            posInfo.DistanceToMouse = pos.GetPositionValue();
            posInfo.PosDistToChamps = pos.GetDistanceToChampions();
            posInfo.Speed = ObjectCache.MyHeroCache.MoveSpeed;

            if (ObjectCache.MenuCache.Cache["RejectMinDistance"].As<MenuSlider>().Value > 0 &&
                ObjectCache.MenuCache.Cache["RejectMinDistance"].As<MenuSlider>().Value > posInfo.ClosestDistance) //reject closestdistance
            {
                posInfo.RejectPosition = true;
            }

            if (ObjectCache.MenuCache.Cache["MinComfortZone"].As<MenuSlider>().Value > posInfo.PosDistToChamps)
            {
                posInfo.HasComfortZone = false;
            }

            return posInfo;
        }

        public static IOrderedEnumerable<PositionInfo> GetBestPositionTest()
        {
            var posChecked = 0;
            var maxPosToCheck = 50;
            var posRadius = 50;
            var radiusIndex = 0;

            var heroPoint = ObjectCache.MyHeroCache.ServerPos2D;
            var lastMovePos = Game.CursorPos.To2D();

            var extraDelayBuffer = ObjectCache.MenuCache.Cache["ExtraPingBuffer"].As<MenuSlider>().Value;
            var extraEvadeDistance = ObjectCache.MenuCache.Cache["ExtraEvadeDistance"].As<MenuSlider>().Value;

            if (ObjectCache.MenuCache.Cache["HigherPrecision"].Enabled)
            {
                maxPosToCheck = 150;
                posRadius = 25;
            }

            var fastestPositions = GetFastestPositions();

            SpellDetector.GetLowestEvadeTime(out var lowestEvadeTimeSpell);

            var posTable = fastestPositions.Select(pos => InitPositionInfo(pos, extraDelayBuffer, extraEvadeDistance, lastMovePos, lowestEvadeTimeSpell)).ToList();

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

                    posTable.Add(InitPositionInfo(pos, extraDelayBuffer, extraEvadeDistance, lastMovePos, lowestEvadeTimeSpell));
                }
            }

            var sortedPosTable = posTable.OrderBy(p => p.IsDangerousPos).ThenBy(p => p.PosDangerLevel).ThenBy(p => p.PosDangerCount).ThenBy(p => p.DistanceToMouse);

            return sortedPosTable;
        }

        public static PositionInfo GetBestPosition()
        {
            var posChecked = 0;
            var maxPosToCheck = 50;
            var posRadius = 50;
            var radiusIndex = 0;

            var extraDelayBuffer = ObjectCache.MenuCache.Cache["ExtraPingBuffer"].As<MenuSlider>().Value;
            var extraEvadeDistance = ObjectCache.MenuCache.Cache["ExtraEvadeDistance"].As<MenuSlider>().Value;

            SpellDetector.UpdateSpells();
            CalculateEvadeTime();

            if (ObjectCache.MenuCache.Cache["CalculateWindupDelay"].Enabled)
            {
                var extraWindupDelay = Evade.LastWindupTime - Environment.TickCount;
                if (extraWindupDelay > 0)
                {
                    extraDelayBuffer += (int) extraWindupDelay;
                }
            }

            extraDelayBuffer += (int) Evade.AvgCalculationTime;

            if (ObjectCache.MenuCache.Cache["HigherPrecision"].Enabled)
            {
                maxPosToCheck = 150;
                posRadius = 25;
            }

            var heroPoint = ObjectCache.MyHeroCache.ServerPos2D;
            var lastMovePos = Game.CursorPos.To2D();

            var lowestEvadeTime = SpellDetector.GetLowestEvadeTime(out var lowestEvadeTimeSpell);

            var fastestPositions = GetFastestPositions();

            var posTable = fastestPositions.Select(pos => InitPositionInfo(pos, extraDelayBuffer, extraEvadeDistance, lastMovePos, lowestEvadeTimeSpell)).ToList();

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
                    posTable.Add(InitPositionInfo(pos, extraDelayBuffer, extraEvadeDistance, lastMovePos, lowestEvadeTimeSpell));
                }
            }

            IOrderedEnumerable<PositionInfo> sortedPosTable;

            if (ObjectCache.MenuCache.Cache["EvadeMode"].As<MenuList>().SelectedItem == "Fastest")
            {
                sortedPosTable = posTable.OrderBy(p => p.IsDangerousPos).ThenByDescending(p => p.IntersectionTime).ThenBy(p => p.PosDangerLevel).ThenBy(p => p.PosDangerCount);

                FastEvadeMode = true;
            }
            else if (FastEvadeMode)
            {
                sortedPosTable = posTable.OrderBy(p => p.IsDangerousPos).ThenByDescending(p => p.IntersectionTime).ThenBy(p => p.PosDangerLevel).ThenBy(p => p.PosDangerCount);
            }
            else if (ObjectCache.MenuCache.Cache["FastEvadeActivationTime"].As<MenuSlider>().Value > 0 &&
                     ObjectCache.MenuCache.Cache["FastEvadeActivationTime"].As<MenuSlider>().Value + ObjectCache.GamePing + extraDelayBuffer > lowestEvadeTime)
            {
                sortedPosTable = posTable.OrderBy(p => p.IsDangerousPos).ThenByDescending(p => p.IntersectionTime).ThenBy(p => p.PosDangerLevel).ThenBy(p => p.PosDangerCount);

                FastEvadeMode = true;
            }
            else
            {
                sortedPosTable = posTable.OrderBy(p => p.RejectPosition).ThenBy(p => p.PosDangerLevel).ThenBy(p => p.PosDangerCount).ThenBy(p => p.DistanceToMouse);

                if (sortedPosTable.First().PosDangerCount != 0) //if can't dodge smoothly, dodge fast
                {
                    var sortedPosTableFastest = posTable.OrderBy(p => p.IsDangerousPos).ThenByDescending(p => p.IntersectionTime).ThenBy(p => p.PosDangerLevel).ThenBy(p => p.PosDangerCount);

                    if (sortedPosTableFastest.First().PosDangerCount == 0)
                    {
                        sortedPosTable = sortedPosTableFastest;
                        FastEvadeMode = true;
                    }
                }
            }

            foreach (var posInfo in sortedPosTable)
            {
                if (CheckPathCollision(MyHero, posInfo.Position))
                {
                    continue;
                }
                if (FastEvadeMode)
                {
                    posInfo.Position = GetExtendedSafePosition(ObjectCache.MyHeroCache.ServerPos2D, posInfo.Position, extraEvadeDistance);
                    return CanHeroWalkToPos(posInfo.Position, ObjectCache.MyHeroCache.MoveSpeed, ObjectCache.GamePing, 0);
                }

                if (!PositionInfoStillValid(posInfo))
                {
                    continue;
                }

                if (posInfo.Position.CheckDangerousPos(extraEvadeDistance)) //extra evade distance, no multiple skillshots
                {
                    posInfo.Position = GetExtendedSafePosition(ObjectCache.MyHeroCache.ServerPos2D, posInfo.Position, extraEvadeDistance);
                }

                return posInfo;
            }

            return PositionInfo.SetAllUndodgeable();
        }

        public static PositionInfo GetBestPositionMovementBlock(Vector2 movePos)
        {
            var posChecked = 0;
            var maxPosToCheck = 50;
            var posRadius = 50;
            var radiusIndex = 0;

            var extraEvadeDistance = ObjectCache.MenuCache.Cache["ExtraAvoidDistance"].As<MenuSlider>().Value;

            var heroPoint = ObjectCache.MyHeroCache.ServerPos2D;

            var posTable = new List<PositionInfo>();

            var extraDist = ObjectCache.MenuCache.Cache["ExtraCPADistance"].As<MenuSlider>().Value;
            var extraDelayBuffer = ObjectCache.MenuCache.Cache["ExtraPingBuffer"].As<MenuSlider>().Value;

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

                    var posInfo = CanHeroWalkToPos(pos, ObjectCache.MyHeroCache.MoveSpeed, extraDelayBuffer + ObjectCache.GamePing, extraDist);
                    posInfo.IsDangerousPos = pos.CheckDangerousPos(6) || CheckMovePath(pos);
                    posInfo.DistanceToMouse = pos.GetPositionValue();
                    posInfo.HasExtraDistance = extraEvadeDistance > 0 && pos.HasExtraAvoidDistance(extraEvadeDistance);

                    posTable.Add(posInfo);
                }
            }

            var sortedPosTable = posTable.OrderBy(p => p.IsDangerousPos).ThenBy(p => p.PosDangerLevel).ThenBy(p => p.HasExtraDistance).ThenBy(p => p.DistanceToMouse);

            return sortedPosTable.FirstOrDefault(posInfo => CheckPathCollision(MyHero, posInfo.Position) == false);
        }

        public static PositionInfo GetBestPositionBlink()
        {
            var posChecked = 0;
            const int maxPosToCheck = 100;
            const int posRadius = 50;
            var radiusIndex = 0;

            var extraEvadeDistance = Math.Max(100, ObjectCache.MenuCache.Cache["ExtraEvadeDistance"].As<MenuSlider>().Value);

            var heroPoint = ObjectCache.MyHeroCache.ServerPos2DPing;
            var lastMovePos = Game.CursorPos.To2D();

            var minComfortZone = ObjectCache.MenuCache.Cache["MinComfortZone"].As<MenuSlider>().Value;

            var posTable = new List<PositionInfo>();

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

                    var isDangerousPos = pos.CheckDangerousPos(6);
                    var dist = pos.GetPositionValue();

                    var posInfo = new PositionInfo(pos, isDangerousPos, dist)
                    {
                        HasExtraDistance = extraEvadeDistance > 0 && pos.CheckDangerousPos(extraEvadeDistance),
                        PosDistToChamps = pos.GetDistanceToChampions()
                    };


                    if (minComfortZone < posInfo.PosDistToChamps)
                    {
                        posTable.Add(posInfo);
                    }
                }
            }

            var sortedPosTable = posTable.OrderBy(p => p.IsDangerousPos).ThenBy(p => p.HasExtraDistance).ThenBy(p => p.DistanceToMouse);

            return sortedPosTable.FirstOrDefault(posInfo => CheckPointCollision(MyHero, posInfo.Position) == false);
        }

        public static PositionInfo GetBestPositionDash(EvadeSpellData spell)
        {
            var posChecked = 0;
            const int maxPosToCheck = 100;
            const int posRadius = 50;
            var radiusIndex = 0;

            var extraDelayBuffer = ObjectCache.MenuCache.Cache["ExtraPingBuffer"].As<MenuSlider>().Value;
            var extraEvadeDistance = Math.Max(100, ObjectCache.MenuCache.Cache["ExtraEvadeDistance"].As<MenuSlider>().Value);
            var extraDist = ObjectCache.MenuCache.Cache["ExtraCPADistance"].As<MenuSlider>().Value;

            var heroPoint = ObjectCache.MyHeroCache.ServerPos2DPing;

            var posTable = new List<PositionInfo>();
            var spellList = SpellDetector.GetSpellList();

            var minDistance = 50; //Math.Min(spell.range, minDistance)
            var maxDistance = int.MaxValue;

            if (spell.FixedRange)
            {
                minDistance = maxDistance = (int) spell.Range;
            }

            while (posChecked < maxPosToCheck)
            {
                radiusIndex++;

                var curRadius = radiusIndex * 2 * posRadius + (minDistance - 2 * posRadius);
                var curCircleChecks = (int) Math.Ceiling(2 * Math.PI * curRadius / (2 * (double) posRadius));

                for (var i = 1; i < curCircleChecks; i++)
                {
                    posChecked++;
                    var cRadians = 2 * Math.PI / (curCircleChecks - 1) * i; //check decimals
                    var pos = new Vector2((float) Math.Floor(heroPoint.X + curRadius * Math.Cos(cRadians)), (float) Math.Floor(heroPoint.Y + curRadius * Math.Sin(cRadians)));

                    var posInfo = CanHeroWalkToPos(pos, spell.Speed, extraDelayBuffer + ObjectCache.GamePing, extraDist);
                    posInfo.IsDangerousPos = pos.CheckDangerousPos(6);
                    posInfo.HasExtraDistance = extraEvadeDistance > 0 && pos.CheckDangerousPos(extraEvadeDistance); // ? 1 : 0;                    
                    posInfo.DistanceToMouse = pos.GetPositionValue();
                    posInfo.SpellList = spellList;

                    posInfo.PosDistToChamps = pos.GetDistanceToChampions();

                    posTable.Add(posInfo);
                }

                if (curRadius >= maxDistance)
                {
                    break;
                }
            }

            var sortedPosTable = posTable.OrderBy(p => p.IsDangerousPos).ThenBy(p => p.PosDangerLevel).ThenBy(p => p.PosDangerCount).ThenBy(p => p.HasExtraDistance).ThenBy(p => p.DistanceToMouse);

            return sortedPosTable.Where(posInfo => CheckPathCollision(MyHero, posInfo.Position) == false).FirstOrDefault(posInfo => PositionInfoStillValid(posInfo, spell.Speed));
        }

        public static PositionInfo GetBestPositionTargetedDash(EvadeSpellData spell)
        {
            var extraDelayBuffer = ObjectCache.MenuCache.Cache["ExtraPingBuffer"].As<MenuSlider>().Value;
            var extraEvadeDistance = Math.Max(100, ObjectCache.MenuCache.Cache["ExtraEvadeDistance"].As<MenuSlider>().Value);
            var extraDist = ObjectCache.MenuCache.Cache["ExtraCPADistance"].As<MenuSlider>().Value;

            var heroPoint = ObjectCache.MyHeroCache.ServerPos2DPing;

            var posTable = new List<PositionInfo>();
            var spellList = SpellDetector.GetSpellList();

            var collisionCandidates = new List<Obj_AI_Base>();

            if (spell.SpellTargets.Contains(SpellTargets.Targetables))
            {
                collisionCandidates.AddRange(ObjectManager.Get<Obj_AI_Base>().Where(h => !h.IsMe && h.IsValidTarget(spell.Range)).Where(obj => obj.Type != GameObjectType.obj_AI_Turret));
            }
            else
            {
                var heroList = new List<Obj_AI_Hero>(); // Maybe change to IEnumerable

                if (spell.SpellTargets.Contains(SpellTargets.EnemyChampions) && spell.SpellTargets.Contains(SpellTargets.AllyChampions))
                {
                    heroList = GameObjects.Heroes.ToList();
                }
                else if (spell.SpellTargets.Contains(SpellTargets.EnemyChampions))
                {
                    heroList = GameObjects.EnemyHeroes.ToList();
                }
                else if (spell.SpellTargets.Contains(SpellTargets.AllyChampions))
                {
                    heroList = GameObjects.AllyHeroes.ToList();
                }

                collisionCandidates.AddRange(heroList.Where(h => !h.IsMe && h.IsValidTarget(spell.Range)).Cast<Obj_AI_Base>());

                var minionList = new List<Obj_AI_Minion>();

                if (spell.SpellTargets.Contains(SpellTargets.EnemyMinions) && spell.SpellTargets.Contains(SpellTargets.AllyMinions))
                {
                    minionList = GameObjects.Minions.Where(m => m.Distance(MyHero.ServerPosition) <= spell.Range).ToList();
                }
                else if (spell.SpellTargets.Contains(SpellTargets.EnemyMinions))
                {
                    minionList = GameObjects.EnemyMinions.Where(m => m.Distance(MyHero.ServerPosition) <= spell.Range).ToList();
                }
                else if (spell.SpellTargets.Contains(SpellTargets.AllyMinions))
                {
                    minionList = GameObjects.AllyMinions.Where(m => m.Distance(MyHero.ServerPosition) <= spell.Range).ToList();
                }

                collisionCandidates.AddRange(minionList.Where(h => h.IsValidTarget(spell.Range)).Cast<Obj_AI_Base>());
            }

            foreach (var candidate in collisionCandidates)
            {
                var pos = candidate.ServerPosition.To2D();

                PositionInfo posInfo;

                if (spell.SpellName == "YasuoDashWrapper")
                {
                    var hasDashBuff = candidate.Buffs.Any(buff => buff.Name == "YasuoDashWrapper");

                    if (hasDashBuff)
                    {
                        continue;
                    }
                }

                if (spell.BehindTarget)
                {
                    var dir = (pos - heroPoint).Normalized();
                    pos = pos + dir * (candidate.BoundingRadius + ObjectCache.MyHeroCache.BoundingRadius);
                }

                if (spell.InfrontTarget)
                {
                    var dir = (pos - heroPoint).Normalized();
                    pos = pos - dir * (candidate.BoundingRadius + ObjectCache.MyHeroCache.BoundingRadius);
                }

                if (spell.FixedRange)
                {
                    var dir = (pos - heroPoint).Normalized();
                    pos = heroPoint + dir * spell.Range;
                }

                if (spell.EvadeType == EvadeType.Dash)
                {
                    posInfo = CanHeroWalkToPos(pos, spell.Speed, extraDelayBuffer + ObjectCache.GamePing, extraDist);
                    posInfo.IsDangerousPos = pos.CheckDangerousPos(6);
                    posInfo.DistanceToMouse = pos.GetPositionValue();
                    posInfo.SpellList = spellList;
                }
                else
                {
                    var isDangerousPos = pos.CheckDangerousPos(6);
                    var dist = pos.GetPositionValue();

                    posInfo = new PositionInfo(pos, isDangerousPos, dist);
                }

                posInfo.Target = candidate;
                posTable.Add(posInfo);
            }

            if (spell.EvadeType == EvadeType.Dash)
            {
                var sortedPosTable = posTable.OrderBy(p => p.IsDangerousPos).ThenBy(p => p.PosDangerLevel).ThenBy(p => p.PosDangerCount).ThenBy(p => p.DistanceToMouse);

                var first = sortedPosTable.FirstOrDefault();
                if (first != null && Evade.LastPosInfo != null && first.IsDangerousPos == false && Evade.LastPosInfo.PosDangerLevel > first.PosDangerLevel)
                {
                    return first;
                }
            }
            else
            {
                var sortedPosTable = posTable.OrderBy(p => p.IsDangerousPos).ThenBy(p => p.DistanceToMouse);

                var first = sortedPosTable.FirstOrDefault();

                return first;
            }

            return null;
        }

        public static bool CheckWindupTime(float windupTime)
        {
            return SpellDetector.Spells.Select(entry => entry.Value).Select(spell => spell.GetSpellHitTime(ObjectCache.MyHeroCache.ServerPos2D)).Any(hitTime => hitTime < windupTime);
        }

        public static float GetMovementBlockPositionValue(Vector2 pos, Vector2 movePos)
        {
            float value = 0; // pos.Distance(movePos);

            foreach (var entry in SpellDetector.Spells)
            {
                var spell = entry.Value;
                var spellPos = spell.GetCurrentSpellPosition(true, ObjectCache.GamePing);
                var extraDist = 100 + spell.Radius;

                value -= Math.Max(0, -(10 * ((float) 0.8 * extraDist) / pos.Distance(spell.GetSpellProjection(pos))) + extraDist);
            }

            return value;
        }

        public static bool PositionInfoStillValid(PositionInfo posInfo, float moveSpeed = 0)
        {
            return true; //too buggy
        }

        public static List<Vector2> GetExtendedPositions(Vector2 from, Vector2 to, float extendDistance)
        {
            var direction = (to - from).Normalized();
            var positions = new List<Vector2>();
            const float sectorDistance = 50;

            for (var i = sectorDistance; i < extendDistance; i += sectorDistance)
            {
                var pos = to + direction * i;

                positions.Add(pos);
            }

            return positions;
        }

        public static Vector2 GetExtendedSafePosition(Vector2 from, Vector2 to, float extendDistance)
        {
            var direction = (to - from).Normalized();
            var lastPosition = to;
            const float sectorDistance = 50;

            for (var i = sectorDistance; i <= extendDistance; i += sectorDistance)
            {
                var pos = to + direction * i;

                if (pos.CheckDangerousPos(6) || CheckPathCollision(MyHero, pos))
                {
                    return lastPosition;
                }

                lastPosition = pos;
            }

            return lastPosition;
        }

        public static void CalculateEvadeTime()
        {
            foreach (var entry in SpellDetector.Spells)
            {
                var spell = entry.Value;
                spell.CanHeroEvade(MyHero, out var evadeTime, out var spellHitTime);

                spell.SpellHitTime = spellHitTime;
                spell.EvadeTime = evadeTime;
            }
        }

        public static Vector2 GetFastestPosition(Spell spell)
        {
            var heroPos = ObjectCache.MyHeroCache.ServerPos2D;

            switch (spell.SpellType)
            {
                case SpellType.Line:
                    var projection = heroPos.ProjectOn(spell.StartPos, spell.EndPos).SegmentPoint;
                    return projection.Extend(heroPos, spell.Radius + ObjectCache.MyHeroCache.BoundingRadius + 10);
                case SpellType.Circular: return spell.EndPos.Extend(heroPos, spell.Radius + 10);
            }

            return Vector2.Zero;
        }

        public static List<Vector2> GetFastestPositions()
        {
            return SpellDetector.Spells.Select(entry => entry.Value).Select(GetFastestPosition).Where(pos => pos != Vector2.Zero).ToList();
        }

        public static float CompareFastestPosition(Spell spell, Vector2 start, Vector2 movePos)
        {
            var fastestPos = GetFastestPosition(spell);
            var moveDir = (movePos - start).Normalized();
            var fastestDir = (GetFastestPosition(spell) - start).Normalized();

            return moveDir.AngleBetween(fastestDir); // * (180 / ((float)Math.PI));
        }

        public static float GetMinCpaDistance(Vector2 movePos)
        {
            return SpellDetector.Spells.Values.Select(spell => GetClosestDistanceApproach(spell, movePos, ObjectCache.MyHeroCache.MoveSpeed, ObjectCache.GamePing, ObjectCache.MyHeroCache.ServerPos2DPing, 0)).Concat(new[] {float.MaxValue}).Min();
        }

        public static float GetCombinedIntersectionDistance(Vector2 movePos)
        {
            var heroPoint = ObjectCache.MyHeroCache.ServerPos2D;

            return (from spell in SpellDetector.Spells.Values let intersectDist = GetIntersectDistance(spell, heroPoint, movePos) select intersectDist * spell.Dangerlevel).Sum();
        }

        public static Vector3 GetNearWallPoint(Vector3 start, Vector3 end)
        {
            var direction = (end - start).Normalized();
            var distance = start.Distance(end);
            for (var i = 20; i < distance; i += 20)
            {
                var v = end - direction * i;

                if (!NavMesh.WorldToCell(v).Flags.HasFlag(NavCellFlags.Wall | NavCellFlags.Building))
                {
                    return v;
                }
            }

            return Vector3.Zero;
        }

        public static Vector3 GetNearWallPoint(Vector2 start, Vector2 end)
        {
            var direction = (end - start).Normalized();
            var distance = start.Distance(end);
            for (var i = 20; i < distance; i += 20)
            {
                var v = end - direction * i;
          
                if (!NavMesh.WorldToCell(v.To3D()).Flags.HasFlag(NavCellFlags.Wall | NavCellFlags.Building))
                {
                    return v.To3D();
                }
            }

            return Vector3.Zero;
        }

        public static float GetIntersectDistance(Spell spell, Vector2 start, Vector2 end)
        {
            if (spell == null)
            {
                return float.MaxValue;
            }

            switch (spell.SpellType)
            {
                case SpellType.Line:
                    var hasIntersection = spell.LineIntersectLinearSpellEx(start, end, out var intersection);
                    if (hasIntersection)
                    {
                        return start.Distance(intersection);
                    }
                    break;
                case SpellType.Circular:
                    if (end.InSkillShot(spell, ObjectCache.MyHeroCache.BoundingRadius) == false)
                    {
                        MathUtils.FindLineCircleIntersections(spell.EndPos, spell.Radius, start, end, out var intersection1, out var intersection2);

                        if (intersection1.X != float.NaN && MathUtils.isPointOnLineSegment(intersection1, start, end))
                        {
                            return start.Distance(intersection1);
                        }
                        if (intersection2.X != float.NaN && MathUtils.isPointOnLineSegment(intersection2, start, end))
                        {
                            return start.Distance(intersection2);
                        }
                    }
                    break;
            }

            return float.MaxValue;
        }

        public static PositionInfo CanHeroWalkToPos(Vector2 pos, float speed, float delay, float extraDist, bool useServerPosition = true)
        {
            var posDangerLevel = 0;
            var posDangerCount = 0;
            var closestDistance = float.MaxValue;
            var dodgeableSpells = new List<int>();
            var undodgeableSpells = new List<int>();

            var heroPos = ObjectCache.MyHeroCache.ServerPos2D;

            var minComfortDistance = ObjectCache.MenuCache.Cache["MinComfortZone"].As<MenuSlider>().Value;

            if (useServerPosition == false)
            {
                heroPos = MyHero.Position.To2D();
            }

            foreach (var entry in SpellDetector.Spells)
            {
                var spell = entry.Value;

                closestDistance = Math.Min(closestDistance, GetClosestDistanceApproach(spell, pos, speed, delay, heroPos, extraDist));

                if (pos.InSkillShot(spell, ObjectCache.MyHeroCache.BoundingRadius - 8) || PredictSpellCollision(spell, pos, speed, delay, heroPos, extraDist, useServerPosition) ||
                    spell.Info.SpellType != SpellType.Line && pos.IsNearEnemy(minComfortDistance))
                {
                    posDangerLevel = Math.Max(posDangerLevel, spell.Dangerlevel);
                    posDangerCount += spell.Dangerlevel;
                    undodgeableSpells.Add(spell.SpellId);
                }
                else
                {
                    dodgeableSpells.Add(spell.SpellId);
                }
            }

            return new PositionInfo(pos, posDangerLevel, posDangerCount, posDangerCount > 0, closestDistance, dodgeableSpells, undodgeableSpells);
        }

        public static float GetClosestDistanceApproach(Spell spell, Vector2 pos, float speed, float delay, Vector2 heroPos, float extraDist)
        {
            var walkDir = (pos - heroPos).Normalized();

            switch (spell.SpellType)
            {
                case SpellType.Line when spell.Info.ProjectileSpeed != float.MaxValue:
                {
                    var spellPos = spell.GetCurrentSpellPosition(true, delay);
                    var spellEndPos = spell.GetSpellEndPosition();
                    var extendedPos = pos.ExtendDir(walkDir, ObjectCache.MyHeroCache.BoundingRadius + speed * delay / 1000);

                    var cpa2 = MathUtils.GetCollisionDistanceEx(heroPos,
                                                                walkDir * speed,
                                                                ObjectCache.MyHeroCache.BoundingRadius,
                                                                spellPos,
                                                                spell.Direction * spell.Info.ProjectileSpeed,
                                                                spell.Radius + extraDist,
                                                                out var cHeroPos,
                                                                out var cSpellPos);

                    var cHeroPosProjection = cHeroPos.ProjectOn(heroPos, extendedPos);
                    var cSpellPosProjection = cSpellPos.ProjectOn(spellPos, spellEndPos);

                    if (cSpellPosProjection.IsOnSegment && cHeroPosProjection.IsOnSegment && cpa2 != float.MaxValue)
                    {
                        return 0;
                    }

                    var cpa = MathUtilsCpa.CPAPointsEx(heroPos, walkDir * speed, spellPos, spell.Direction * spell.Info.ProjectileSpeed, pos, spellEndPos, out cHeroPos, out cSpellPos);

                    cHeroPosProjection = cHeroPos.ProjectOn(heroPos, extendedPos);
                    cSpellPosProjection = cSpellPos.ProjectOn(spellPos, spellEndPos);

                    var checkDist = ObjectCache.MyHeroCache.BoundingRadius + spell.Radius + extraDist;

                    if (cSpellPosProjection.IsOnSegment && cHeroPosProjection.IsOnSegment)
                    {
                        return Math.Max(0, cpa - checkDist);
                    }

                    return checkDist;

                    //return MathUtils.ClosestTimeOfApproach(heroPos, walkDir * speed, spellPos, spell.Orientation * spell.info.projectileSpeed);
                }
                case SpellType.Line when spell.Info.ProjectileSpeed == float.MaxValue:
                {
                    var spellHitTime = Math.Max(0, spell.EndTime - Environment.TickCount - delay); //extraDelay
                    var walkRange = heroPos.Distance(pos);
                    var predictedRange = speed * (spellHitTime / 1000);
                    var tHeroPos = heroPos + walkDir * Math.Min(predictedRange, walkRange); //Hero predicted pos

                    var projection = tHeroPos.ProjectOn(spell.StartPos, spell.EndPos);

                    return Math.Max(0, tHeroPos.Distance(projection.SegmentPoint) - (spell.Radius + ObjectCache.MyHeroCache.BoundingRadius + extraDist)); //+ dodgeBuffer
                }
                case SpellType.Circular:
                {
                    var spellHitTime = Math.Max(0, spell.EndTime - Environment.TickCount - delay); //extraDelay
                    var walkRange = heroPos.Distance(pos);
                    var predictedRange = speed * (spellHitTime / 1000);
                    var tHeroPos = heroPos + walkDir * Math.Min(predictedRange, walkRange); //Hero predicted pos

                    switch (spell.Info.SpellName)
                        {
                        case "VeigarEventHorizon":
                        {
                            const int wallRadius = 65;
                            var midRadius = spell.Radius - wallRadius;

                            if (spellHitTime == 0)
                            {
                                return 0;
                            }

                            return tHeroPos.Distance(spell.EndPos) >= spell.Radius
                                       ? Math.Max(0, tHeroPos.Distance(spell.EndPos) - midRadius - wallRadius)
                                       : Math.Max(0, midRadius - tHeroPos.Distance(spell.EndPos) - wallRadius);
                        }
                        case "DariusCleave":
                        {
                            const int wallRadius = 115;
                            var midRadius = spell.Radius - wallRadius;

                            if (spellHitTime == 0)
                            {
                                return 0;
                            }

                            return tHeroPos.Distance(spell.EndPos) >= spell.Radius
                                       ? Math.Max(0, tHeroPos.Distance(spell.EndPos) - midRadius - wallRadius)
                                       : Math.Max(0, midRadius - tHeroPos.Distance(spell.EndPos) - wallRadius);
                        }
                    }

                    var closestDist = Math.Max(0, tHeroPos.Distance(spell.EndPos) - (spell.Radius + extraDist));
                    if (spell.Info.ExtraEndTime > 0 && closestDist != 0)
                    {
                        var remainingTime = Math.Max(0, spell.EndTime + spell.Info.ExtraEndTime - Environment.TickCount - delay);
                        var predictedRange2 = speed * (remainingTime / 1000);
                        var tHeroPos2 = heroPos + walkDir * Math.Min(predictedRange2, walkRange);

                        if (CheckMoveToDirection(tHeroPos, tHeroPos2))
                        {
                            return 0;
                        }
                    }
                    else
                    {
                        return closestDist;
                    }
                    break;
                }
                case SpellType.Arc:
                {
                    var spellPos = spell.GetCurrentSpellPosition(true, delay);
                    var spellEndPos = spell.GetSpellEndPosition();

                    var pDir = spell.Direction.Perpendicular();
                    spellPos = spellPos - pDir * spell.Radius / 2;
                    spellEndPos = spellEndPos - pDir * spell.Radius / 2;

                    var extendedPos = pos.ExtendDir(walkDir, ObjectCache.MyHeroCache.BoundingRadius);

                    var cpa = MathUtilsCpa.CPAPointsEx(heroPos, walkDir * speed, spellPos, spell.Direction * spell.Info.ProjectileSpeed, pos, spellEndPos, out var cHeroPos, out var cSpellPos);

                    var cHeroPosProjection = cHeroPos.ProjectOn(heroPos, extendedPos);
                    var cSpellPosProjection = cSpellPos.ProjectOn(spellPos, spellEndPos);

                    var checkDist = spell.Radius + extraDist;

                    if (cHeroPos.InSkillShot(spell, ObjectCache.MyHeroCache.BoundingRadius))
                    {
                        if (cSpellPosProjection.IsOnSegment && cHeroPosProjection.IsOnSegment)
                        {
                            return Math.Max(0, cpa - checkDist);
                        }
                        return checkDist;
                    }
                    break;
                }
                case SpellType.Cone:
                {
                    var spellHitTime = Math.Max(0, spell.EndTime - Environment.TickCount - delay); //extraDelay
                    var walkRange = heroPos.Distance(pos);
                    var predictedRange = speed * (spellHitTime / 1000);
                    var tHeroPos = heroPos + walkDir * Math.Min(predictedRange, walkRange); //Hero predicted pos

                    var sides = new[]
                    {
                        heroPos.ProjectOn(spell.CnStart, spell.CnLeft).SegmentPoint,
                        heroPos.ProjectOn(spell.CnLeft, spell.CnRight).SegmentPoint,
                        heroPos.ProjectOn(spell.CnRight, spell.CnStart).SegmentPoint
                    };

                    var p = sides.OrderBy(x => x.Distance(x)).First();

                    return Math.Max(0, tHeroPos.Distance(p) - (spell.Radius + ObjectCache.MyHeroCache.BoundingRadius + extraDist));
                }
            }

            return 1;
        }

        public static bool PredictSpellCollision(Spell spell, Vector2 pos, float speed, float delay, Vector2 heroPos, float extraDist, bool useServerPosition = true)
        {
            extraDist = extraDist + 10;

            if (useServerPosition == false)
            {
                return GetClosestDistanceApproach(spell, pos, speed, 0, ObjectCache.MyHeroCache.ServerPos2D, 0) == 0;
            }

            return GetClosestDistanceApproach(spell,
                                              pos,
                                              speed,
                                              delay,
                                              ObjectCache.MyHeroCache.ServerPos2DPing,
                                              extraDist) == 0 || GetClosestDistanceApproach(spell,
                                                                                            pos,
                                                                                            speed,
                                                                                            ObjectCache.GamePing, //Game.Ping
                                                                                            ObjectCache.MyHeroCache.ServerPos2DPing,
                                                                                            extraDist) == 0;
        }

        public static Vector2 GetRealHeroPos(float delay = 0)
        {
            var path = MyHero.Path;
            if (path.Length < 1)
            {
                return ObjectCache.MyHeroCache.ServerPos2D;
            }

            var serverPos = ObjectCache.MyHeroCache.ServerPos2D;
            var heroPos = MyHero.Position.To2D();

            var walkDir = (path[0].To2D() - heroPos).Normalized();
            var realPos = heroPos + walkDir * ObjectCache.MyHeroCache.MoveSpeed * (delay / 1000);

            return realPos;
        }

        public static bool CheckPathCollision(Obj_AI_Base unit, Vector2 movePos)
        {
            var path = unit.GetPath(ObjectCache.MyHeroCache.ServerPos2D.To3D(), movePos.To3D());

            if (path.Length <= 0)
            {
                return false;
            }

            return movePos.Distance(path[path.Length - 1].To2D()) > 5 || path.Length > 2;
        }

        public static bool CheckPointCollision(Obj_AI_Base unit, Vector2 movePos)
        {
      
            var path = unit.GetPath(movePos.To3D());

            if (path.Length <= 0)
            {
                return false;
            }

            if (movePos.Distance(path[path.Length - 1].To2D()) > 5)
            {
                return true;
            }

            return false;
        }

        public static bool CheckMovePath(Vector2 movePos, float extraDelay = 0)
        {
            var startPoint = MyHero.Position;

            if (MyHero.IsDashing())
            {
                var dashItem = MyHero.GetDashInfo();
                startPoint = dashItem.EndPos.To3D();
            }

            var poopy = MyHero.GetPath(startPoint, movePos.To3D()); //from serverpos

            var lastPoint = new Vector2();

            foreach (var point in poopy)
            {
                var point2D = point.To2D();
                if (lastPoint != Vector2.Zero && CheckMoveToDirection(lastPoint, point2D, extraDelay))
                {
                    return true;
                }

                lastPoint = lastPoint != Vector2.Zero ? point2D : MyHero.ServerPosition.To2D();
            }

            return false;
        }

        public static bool LineIntersectLinearSegment(Vector2 a1, Vector2 b1, Vector2 a2, Vector2 b2)
        {
            const int segmentRadius = 55;

            var myBoundingRadius = ObjectManager.GetLocalPlayer().BoundingRadius;
            var segmentDir = (b1 - a1).Normalized().Perpendicular();
            var segmentStart = a1;
            var segmentEnd = b1;

            var startRightPos = segmentStart + segmentDir * (segmentRadius + myBoundingRadius);
            var startLeftPos = segmentStart - segmentDir * (segmentRadius + myBoundingRadius);
            var endRightPos = segmentEnd + segmentDir * (segmentRadius + myBoundingRadius);
            var endLeftPos = segmentEnd - segmentDir * (segmentRadius + myBoundingRadius);

            var int1 = MathUtils.CheckLineIntersection(a2, b2, startRightPos, startLeftPos);
            var int2 = MathUtils.CheckLineIntersection(a2, b2, endRightPos, endLeftPos);
            var int3 = MathUtils.CheckLineIntersection(a2, b2, startRightPos, endRightPos);
            var int4 = MathUtils.CheckLineIntersection(a2, b2, startLeftPos, endLeftPos);

            return int1 || int2 || int3 || int4;
        }

        public static bool CheckMoveToDirection(Vector2 from, Vector2 movePos, float extraDelay = 0)
        {
            var dir = (movePos - from).Normalized();
           
            foreach (var entry in SpellDetector.Spells)
            {
                var spell = entry.Value;

                if (!from.InSkillShot(spell, ObjectCache.MyHeroCache.BoundingRadius))
                {
                    var spellPos = spell.CurrentSpellPosition;

                    switch (spell.SpellType)
                    {
                        case SpellType.Line:
                            if (spell.LineIntersectLinearSpell(@from, movePos))
                            {
                                return true;
                            }
                            break;
                        case SpellType.Circular:
                            switch (spell.Info.SpellName)
                            {
                                case "VeigarEventHorizon":
                                {
                                    var cpa2 = MathUtilsCpa.CPAPointsEx(@from, dir * ObjectCache.MyHeroCache.MoveSpeed, spell.EndPos, new Vector2(0, 0), movePos, spell.EndPos);

                                    if (@from.Distance(spell.EndPos) < spell.Radius && !(@from.Distance(spell.EndPos) < spell.Radius - 135 && movePos.Distance(spell.EndPos) < spell.Radius - 135))
                                    {
                                        return true;
                                    }
                                    if (@from.Distance(spell.EndPos) > spell.Radius && cpa2 < spell.Radius + 10)
                                    {
                                        return true;
                                    }
                                    break;
                                }
                                case "DariusCleave":
                                    var cpa3 = MathUtilsCpa.CPAPointsEx(@from, dir * ObjectCache.MyHeroCache.MoveSpeed, spell.EndPos, new Vector2(0, 0), movePos, spell.EndPos);

                                    if (@from.Distance(spell.EndPos) < spell.Radius && !(@from.Distance(spell.EndPos) < spell.Radius - 230 && movePos.Distance(spell.EndPos) < spell.Radius - 230))
                                    {
                                        return true;
                                    }
                                    if (@from.Distance(spell.EndPos) > spell.Radius && cpa3 < spell.Radius + 10)
                                    {
                                        return true;
                                    }
                                    break;
                                default:
                                {
                                    var cpa2 = MathUtils.GetCollisionDistanceEx(@from, dir * ObjectCache.MyHeroCache.MoveSpeed, 1, spell.EndPos, new Vector2(0, 0), spell.Radius, out var cHeroPos, out _);

                                    if (spell.Info.SpellName.Contains("_trap") && !(cpa2 < spell.Radius + 10))
                                    {
                                        continue;
                                    }

                                    var cHeroPosProjection = cHeroPos.ProjectOn(@from, movePos);
                                    if (cHeroPosProjection.IsOnSegment && cpa2 != float.MaxValue)
                                    {
                                        return true;
                                    }
                                    break;
                                }
                            }
                            break;
                        case SpellType.Arc:
                            if (@from.IsLeftOfLineSegment(spell.StartPos, spell.EndPos))
                            {
                                return MathUtils.CheckLineIntersection(@from, movePos, spell.StartPos, spell.EndPos);
                            }

                            var spellRange = spell.StartPos.Distance(spell.EndPos);
                            var midPoint = spell.StartPos + spell.Direction * (spellRange / 2);

                            var cpa = MathUtilsCpa.CPAPointsEx(@from, dir * ObjectCache.MyHeroCache.MoveSpeed, midPoint, new Vector2(0, 0), movePos, midPoint);

                            if (cpa < spell.Radius + 10)
                            {
                                return true;
                            }
                            break;
                        case SpellType.Cone:
                            if (LineIntersectLinearSegment(spell.CnStart, spell.CnLeft, @from, movePos) || LineIntersectLinearSegment(spell.CnLeft, spell.CnRight, @from, movePos) ||
                                LineIntersectLinearSegment(spell.CnRight, spell.CnStart, @from, movePos))
                            {
                                return true;
                            }
                            break;
                    }
                }
                else if (from.InSkillShot(spell, ObjectCache.MyHeroCache.BoundingRadius))
                {
                    if (movePos.InSkillShot(spell, ObjectCache.MyHeroCache.BoundingRadius))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
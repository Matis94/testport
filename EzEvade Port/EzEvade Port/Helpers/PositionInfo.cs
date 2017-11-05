namespace EzEvade_Port.Helpers
{
    using System;
    using System.Collections.Generic;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Core;
    using Spells;
    using Utils;

    public class PositionInfo
    {
        public float ClosestDistance = float.MaxValue;
        public float DistanceToMouse;
        public List<int> DodgeableSpells = new List<int>();
        public float EndTime = 0;
        public bool HasComfortZone = true;
        public bool HasExtraDistance = false;
        public float IntersectionTime = float.MaxValue;
        public bool IsDangerousPos;
        public int PosDangerCount;

        public int PosDangerLevel;
        public float PosDistToChamps = float.MaxValue;
        public Vector2 Position;
        public bool RecalculatedPath = false;
        public bool RejectPosition = false;
        public float Speed = 0;
        public List<int> SpellList = new List<int>();
        public Obj_AI_Base Target = null;
        public float Timestamp;
        public List<int> UndodgeableSpells = new List<int>();

        public PositionInfo(Vector2 position, int posDangerLevel, int posDangerCount, bool isDangerousPos, float distanceToMouse, List<int> dodgeableSpells, List<int> undodgeableSpells)
        {
            this.Position = position;
            this.PosDangerLevel = posDangerLevel;
            this.PosDangerCount = posDangerCount;
            this.IsDangerousPos = isDangerousPos;
            this.DistanceToMouse = distanceToMouse;
            this.DodgeableSpells = dodgeableSpells;
            this.UndodgeableSpells = undodgeableSpells;
            Timestamp = Environment.TickCount;
        }

        public PositionInfo(Vector2 position, bool isDangerousPos, float distanceToMouse)
        {
            this.Position = position;
            this.IsDangerousPos = isDangerousPos;
            this.DistanceToMouse = distanceToMouse;
            Timestamp = Environment.TickCount;
        }

        private static Obj_AI_Hero MyHero => ObjectManager.GetLocalPlayer();

        public static PositionInfo SetAllDodgeable()
        {
            return SetAllDodgeable(MyHero.Position.To2D());
        }

        public static PositionInfo SetAllDodgeable(Vector2 position)
        {
            var dodgeableSpells = new List<int>();
            var undodgeableSpells = new List<int>();

            foreach (var entry in SpellDetector.Spells)
            {
                var spell = entry.Value;
                dodgeableSpells.Add(entry.Key);
            }

            return new PositionInfo(position, 0, 0, true, 0, dodgeableSpells, undodgeableSpells);
        }

        public static PositionInfo SetAllUndodgeable()
        {
            var dodgeableSpells = new List<int>();
            var undodgeableSpells = new List<int>();

            var posDangerLevel = 0;
            var posDangerCount = 0;

            foreach (var entry in SpellDetector.Spells)
            {
                var spell = entry.Value;
                undodgeableSpells.Add(entry.Key);

                var spellDangerLevel = spell.Dangerlevel;

                posDangerLevel = Math.Max(posDangerLevel, spellDangerLevel);
                posDangerCount += spellDangerLevel;
            }

            return new PositionInfo(MyHero.Position.To2D(), posDangerLevel, posDangerCount, true, 0, dodgeableSpells, undodgeableSpells);
        }
    }

    public static class PositionInfoExtensions
    {
        public static Obj_AI_Hero MyHero => ObjectManager.GetLocalPlayer();

        public static int GetHighestSpellId(this PositionInfo posInfo)
        {
            if (posInfo == null)
            {
                return 0;
            }

            var highest = 0;

            foreach (var spellId in posInfo.UndodgeableSpells)
            {
                highest = Math.Max(highest, spellId);
            }

            foreach (var spellId in posInfo.DodgeableSpells)
            {
                highest = Math.Max(highest, spellId);
            }

            return highest;
        }

        public static bool IsSamePosInfo(this PositionInfo posInfo1, PositionInfo posInfo2)
        {
            return new HashSet<int>(posInfo1.SpellList).SetEquals(posInfo2.SpellList);
        }

        public static bool IsBetterMovePos(this PositionInfo newPosInfo)
        {
            PositionInfo posInfo = null;
            var path = MyHero.Path;
            if (path.Length > 0)
            {
                var movePos = path[path.Length - 1].To2D();
                posInfo = EvadeHelper.CanHeroWalkToPos(movePos, ObjectCache.MyHeroCache.MoveSpeed, 0, 0, false);
            }
            else
            {
                posInfo = EvadeHelper.CanHeroWalkToPos(ObjectCache.MyHeroCache.ServerPos2D, ObjectCache.MyHeroCache.MoveSpeed, 0, 0, false);
            }

            if (posInfo.PosDangerCount < newPosInfo.PosDangerCount)
            {
                return false;
            }

            return true;
        }

        public static PositionInfo CompareLastMovePos(this PositionInfo newPosInfo)
        {
            PositionInfo posInfo = null;
            var path = MyHero.Path;
            if (path.Length > 0)
            {
                var movePos = path[path.Length - 1].To2D();
                posInfo = EvadeHelper.CanHeroWalkToPos(movePos, ObjectCache.MyHeroCache.MoveSpeed, 0, 0, false);
            }
            else
            {
                posInfo = EvadeHelper.CanHeroWalkToPos(ObjectCache.MyHeroCache.ServerPos2D, ObjectCache.MyHeroCache.MoveSpeed, 0, 0, false);
            }

            if (posInfo.PosDangerCount < newPosInfo.PosDangerCount)
            {
                return posInfo;
            }

            return newPosInfo;
        }
    }
}
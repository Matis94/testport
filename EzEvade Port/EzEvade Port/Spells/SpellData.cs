namespace EzEvade_Port.Spells
{
    using System;
    using Aimtec;

    public enum SpellType
    {
        Line,
        Circular,
        Cone,
        Arc,
        None
    }

    public enum CollisionObjectType
    {
        EnemyChampions,
        EnemyMinions,
        YasuoWall
    }

    public class SpellData : ICloneable
    {
        public float Angle;
        public string CharName;
        public CollisionObjectType[] CollisionObjects = { };
        public int Dangerlevel = 1;
        public bool DefaultOff = false;
        public float ExtraDelay = 0;
        public float ExtraDistance = 0;
        public float ExtraDrawHeight = 0;
        public float ExtraEndTime = 0;
        public string[] ExtraMissileNames = { };
        public string[] ExtraSpellNames = { };
        public bool FixedRange = false;
        public bool HasEndExplosion = false;
        public bool HasTrap = false;
        public bool Invert = false;
        public bool IsPerpendicular = false;
        public bool IsSpecial = false;
        public bool IsThreeWay = false;
        public bool IsWall = false;
        public string MissileName = "";
        public string Name;
        public bool NoProcess = false;
        public float ProjectileSpeed = float.MaxValue;
        public float Radius;
        public float Range;
        public float SecondaryRadius;
        public float SideRadius;
        public float SpellDelay = 250;
        public SpellSlot SpellKey = SpellSlot.Q;
        public string SpellName;
        public SpellType SpellType;
        public string TrapBaseName = "";
        public string TrapTroyName = "";
        public bool UpdatePosition = true;

        public bool UseEndPosition = false;

        //public int splits; no idea when this was added xd
        public bool UsePackets = false;

        public SpellData() { }

        public SpellData(string charName, string spellName, string name, int range, int radius, int dangerlevel, SpellType spellType)
        {
            CharName = charName;
            SpellName = spellName;
            Name = name;
            Range = range;
            Radius = radius;
            Dangerlevel = dangerlevel;
            SpellType = spellType;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
namespace EzEvade_Port.EvadeSpells
{
    using Aimtec;

    public delegate bool UseSpellFunc(EvadeSpellData evadeSpell, bool process = true);

    public enum CastType
    {
        Position,
        Target,
        Self
    }

    public enum SpellTargets
    {
        AllyMinions,
        EnemyMinions,

        AllyChampions,
        EnemyChampions,

        Targetables
    }

    public enum EvadeType
    {
        Blink,
        Dash,
        Invulnerability,
        MovementSpeedBuff,
        Shield,
        SpellShield,
        WindWall
    }

    public class EvadeSpellData
    {
        public bool BehindTarget = false;
        public CastType CastType = CastType.Position;
        public string CharName;
        public bool CheckSpellName = false;
        public int Dangerlevel = 1;
        public EvadeType EvadeType;
        public bool FixedRange = false;
        public bool InfrontTarget = false;
        public bool IsItem = false;
        public bool IsReversed = false;
        public bool IsSpecial = false;
        public bool IsSummonerSpell = false;
        public uint ItemId;
        public string Name;
        public float Range;
        public float Speed = 0;
        public float[] SpeedArray = {0f, 0f, 0f, 0f, 0f};
        public float SpellDelay = 250;
        public SpellSlot SpellKey = SpellSlot.Q;
        public string SpellName;
        public SpellTargets[] SpellTargets = { };
        public bool Untargetable = false;
        public UseSpellFunc UseSpellFunc = null;

        public EvadeSpellData() { }

        public EvadeSpellData(string charName, string name, SpellSlot spellKey, EvadeType evadeType, int dangerlevel)
        {
            CharName = charName;
            Name = name;
            SpellKey = spellKey;
            EvadeType = evadeType;
            Dangerlevel = dangerlevel;
        }
    }
}
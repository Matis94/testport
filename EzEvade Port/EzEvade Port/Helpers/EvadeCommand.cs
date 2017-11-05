namespace EzEvade_Port.Helpers
{
    using System;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Core;
    using EvadeSpells;
    using Utils;

    public enum EvadeOrderCommand
    {
        None,
        MoveTo,
        Attack,
        CastSpell
    }

    class EvadeCommand
    {
        public EvadeSpellData EvadeSpellData;
        public bool IsProcessed;

        public EvadeOrderCommand Order;
        public Obj_AI_Base Target;
        public Vector2 TargetPosition;
        public float Timestamp;

        public EvadeCommand()
        {
            Timestamp = Environment.TickCount;
            IsProcessed = false;
        }

        private static Obj_AI_Hero MyHero => ObjectManager.GetLocalPlayer();

        public static void MoveTo(Vector2 movePos)
        {
            // fix 
            if (!Situation.ShouldDodge())
            {
                return;
            }

            Evade.LastEvadeCommand = new EvadeCommand {Order = EvadeOrderCommand.MoveTo, TargetPosition = movePos, Timestamp = Environment.TickCount, IsProcessed = false};

            Evade.LastMoveToPosition = movePos;
            Evade.LastMoveToServerPos = MyHero.ServerPosition.To2D();

            MyHero.IssueOrder(OrderType.MoveTo, movePos.To3D(), false);
        }

        public static void Attack(EvadeSpellData spellData, Obj_AI_Base target)
        {
            EvadeSpell.LastSpellEvadeCommand = new EvadeCommand {Order = EvadeOrderCommand.Attack, Target = target, EvadeSpellData = spellData, Timestamp = Environment.TickCount, IsProcessed = false};

            MyHero.IssueOrder(OrderType.AttackUnit, target);
        }

        public static void CastSpell(EvadeSpellData spellData, Obj_AI_Base target)
        {
            EvadeSpell.LastSpellEvadeCommand =
                new EvadeCommand {Order = EvadeOrderCommand.CastSpell, Target = target, EvadeSpellData = spellData, Timestamp = Environment.TickCount, IsProcessed = false};

            MyHero.SpellBook.CastSpell(spellData.SpellKey, target);
        }

        public static void CastSpell(EvadeSpellData spellData, Vector2 movePos)
        {
            EvadeSpell.LastSpellEvadeCommand = new EvadeCommand
            {
                Order = EvadeOrderCommand.CastSpell,
                TargetPosition = movePos,
                EvadeSpellData = spellData,
                Timestamp = Environment.TickCount,
                IsProcessed = false
            };

            MyHero.SpellBook.CastSpell(spellData.SpellKey, movePos.To3D());
        }

        public static void CastSpell(EvadeSpellData spellData)
        {
            EvadeSpell.LastSpellEvadeCommand = new EvadeCommand {Order = EvadeOrderCommand.CastSpell, EvadeSpellData = spellData, Timestamp = Environment.TickCount, IsProcessed = false};

            MyHero.SpellBook.CastSpell(spellData.SpellKey);
        }
    }
}
namespace EzEvade_Port
{
    using System;
    using System.Collections.Generic;
    using Aimtec;

    static class Extensions
    {
        public static bool IsOnScreen(this Vector3 vector)
        {
            Render.WorldToScreen(vector, out var screen);

            if (screen.X < 0 || screen.X > Render.Width || screen.Y < 0 || screen.Y > Render.Height)
            {
                return false;
            }
            return true;
        }

        public static bool IsOnScreen(this Vector2 vector)
        {
            var screen = vector;
            if (screen.X < 0 || screen.X > Render.Width || screen.Y < 0 || screen.Y > Render.Height)
            {
                return false;
            }
            return true;
        }

        public static Vector3 SetZ(this Vector3 vector, float value)
        {
            vector.Z = value;
            return vector;
        }

        public static Vector3 Perpendicular(this Vector3 v)
        {
            return new Vector3(-v.Z, v.Y, v.X);
        }
    }

    /// <summary>
    ///     Represents a last casted spell.
    /// </summary>
    public class LastCastedSpellEntry
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LastCastedSpellEntry" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="tick">The tick.</param>
        /// <param name="target">The target.</param>
        public LastCastedSpellEntry(string name, int tick, Obj_AI_Base target)
        {
            Name = name;
            Tick = tick;
            Target = target;
        }

        #endregion

        #region Fields

        /// <summary>
        ///     The name
        /// </summary>
        public string Name;

        /// <summary>
        ///     The target
        /// </summary>
        public Obj_AI_Base Target;

        /// <summary>
        ///     The tick
        /// </summary>
        public int Tick;

        #endregion
    }

    /// <summary>
    ///     Represents the last cast packet sent.
    /// </summary>
    public class LastCastPacketSentEntry
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LastCastPacketSentEntry" /> class.
        /// </summary>
        /// <param name="slot">The slot.</param>
        /// <param name="tick">The tick.</param>
        /// <param name="targetNetworkId">The target network identifier.</param>
        public LastCastPacketSentEntry(SpellSlot slot, int tick, int targetNetworkId)
        {
            Slot = slot;
            Tick = tick;
            TargetNetworkId = targetNetworkId;
        }

        #endregion

        #region Fields

        /// <summary>
        ///     The slot
        /// </summary>
        public SpellSlot Slot;

        /// <summary>
        ///     The target network identifier
        /// </summary>
        public int TargetNetworkId;

        /// <summary>
        ///     The tick
        /// </summary>
        public int Tick;

        #endregion
    }

    /// <summary>
    ///     Gets the last casted spell of the unit.
    /// </summary>
    public static class LastCastedSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="LastCastedSpell" /> class.
        /// </summary>
        static LastCastedSpell()
        {
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            SpellBook.OnCastSpell += OnCastSpell;
        }

        #endregion

        #region Static Fields

        /// <summary>
        ///     The last cast packet sent
        /// </summary>
        public static LastCastPacketSentEntry LastCastPacketSent;

        /// <summary>
        ///     The casted spells
        /// </summary>
        internal static readonly Dictionary<int, LastCastedSpellEntry> CastedSpells = new Dictionary<int, LastCastedSpellEntry>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the last casted spell.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static LastCastedSpellEntry LastCastedspell(this Obj_AI_Hero unit)
        {
            return CastedSpells.ContainsKey(unit.NetworkId) ? CastedSpells[unit.NetworkId] : null;
        }

        /// <summary>
        ///     Gets the last casted spell name.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static string LastCastedSpellName(this Obj_AI_Hero unit)
        {
            return CastedSpells.ContainsKey(unit.NetworkId) ? CastedSpells[unit.NetworkId].Name : string.Empty;
        }

        /// <summary>
        ///     Gets the last casted spell tick.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static int LastCastedSpellT(this Obj_AI_Hero unit)
        {
            return CastedSpells.ContainsKey(unit.NetworkId) ? CastedSpells[unit.NetworkId].Tick : (Environment.TickCount > 0 ? 0 : int.MinValue);
        }

        /// <summary>
        ///     Gets the last casted spell's target.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static Obj_AI_Base LastCastedSpellTarget(this Obj_AI_Hero unit)
        {
            return CastedSpells.ContainsKey(unit.NetworkId) ? CastedSpells[unit.NetworkId].Target : null;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Fired when the game processes the spell cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="Obj_AI_BaseMissileClientDataEventArgs" /> instance containing the event data.</param>
        private static void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!(sender is Obj_AI_Hero))
            {
                return;
            }

            var entry = new LastCastedSpellEntry(args.SpellData.Name, Environment.TickCount, ObjectManager.GetLocalPlayer());
            if (CastedSpells.ContainsKey(sender.NetworkId))
            {
                CastedSpells[sender.NetworkId] = entry;
            }
            else
            {
                CastedSpells.Add(sender.NetworkId, entry);
            }
        }

        /// <summary>
        ///     Fired then a spell is casted.
        /// </summary>
        private static void OnCastSpell(Obj_AI_Base sender, SpellBookCastSpellEventArgs spellBookCastSpellEventArgs)
        {
            if (sender.IsMe)
            {
                LastCastPacketSent = new LastCastPacketSentEntry(spellBookCastSpellEventArgs.Slot,
                                                                 Environment.TickCount,
                                                                 spellBookCastSpellEventArgs.Target is Obj_AI_Base ? spellBookCastSpellEventArgs.Target.NetworkId : 0);
            }
        }

        #endregion
    }
}
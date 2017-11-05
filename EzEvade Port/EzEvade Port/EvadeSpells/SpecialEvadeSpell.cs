namespace EzEvade_Port.EvadeSpells
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Core;
    using Helpers;
    using Utils;

    class SpecialEvadeSpell
    {
        private static Obj_AI_Hero MyHero => ObjectManager.GetLocalPlayer();

        public static void LoadSpecialSpell(EvadeSpellData spellData)
        {
            if (spellData.SpellName == "EkkoEAttack")
            {
                spellData.UseSpellFunc = UseEkkoE2;
            }

            if (spellData.SpellName == "EkkoR")
            {
                spellData.UseSpellFunc = UseEkkoR;
            }

            if (spellData.SpellName == "EliseSpiderEInitial")
            {
                spellData.UseSpellFunc = UseRappel;
            }

            if (spellData.SpellName == "Pounce")
            {
                spellData.UseSpellFunc = UsePounce;
            }

            if (spellData.SpellName == "RivenTriCleave")
            {
                spellData.UseSpellFunc = UseBrokenWings;
            }
        }

        public static bool UseRappel(EvadeSpellData evadeSpell, bool process = true)
        {
            if (MyHero.UnitSkinName != "Elise")
            {
                EvadeSpell.CastEvadeSpell(() => EvadeCommand.CastSpell(evadeSpell, MyHero), process);
                return true;
            }

            if (MyHero.UnitSkinName == "Elise")
            {
                if (MyHero.SpellBook.CanUseSpell(SpellSlot.R))
                {
                    MyHero.SpellBook.CastSpell(SpellSlot.R);
                }
            }

            return false;
        }

        public static bool UsePounce(EvadeSpellData evadeSpell, bool process = true)
        {
            if (MyHero.UnitSkinName != "Nidalee")
            {
                var posInfo = EvadeHelper.GetBestPositionDash(evadeSpell);
                if (posInfo != null)
                {
                    EvadeSpell.CastEvadeSpell(() => EvadeCommand.CastSpell(evadeSpell), process);
                    return true;
                }
            }

            return false;
        }

        public static bool UseBrokenWings(EvadeSpellData evadeSpell, bool process = false)
        {
            var posInfo = EvadeHelper.GetBestPositionDash(evadeSpell);
            if (posInfo != null)
            {
                EvadeCommand.MoveTo(posInfo.Position);
                DelayAction.Add(50, () => EvadeSpell.CastEvadeSpell(() => EvadeCommand.CastSpell(evadeSpell), process));
                return true;
            }

            return false;
        }

        public static bool UseEkkoE2(EvadeSpellData evadeSpell, bool process = true)
        {
            if (MyHero.HasBuff("ekkoeattackbuff"))
            {
                var posInfo = EvadeHelper.GetBestPositionTargetedDash(evadeSpell);
                if (posInfo != null && posInfo.Target != null)
                {
                    EvadeSpell.CastEvadeSpell(() => EvadeCommand.Attack(evadeSpell, posInfo.Target), process);
                    //DelayAction.Add(50, () => myHero.IssueOrder(OrderType.MoveTo, posInfo.position.To3D()));
                    return true;
                }
            }

            return false;
        }

        public static bool UseEkkoR(EvadeSpellData evadeSpell, bool process = true)
        {
            foreach (var obj in ObjectManager.Get<Obj_AI_Minion>())
            {
                if (obj != null && obj.IsValid && !obj.IsDead && obj.Name == "Ekko" && obj.IsAlly)
                {
                    var blinkPos = obj.ServerPosition.To2D();
                    if (!blinkPos.CheckDangerousPos(10))
                    {
                        EvadeSpell.CastEvadeSpell(() => EvadeCommand.CastSpell(evadeSpell), process);
                        //DelayAction.Add(50, () => myHero.IssueOrder(OrderType.MoveTo, posInfo.position.To3D()));
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
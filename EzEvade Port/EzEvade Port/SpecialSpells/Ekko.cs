namespace EzEvade_Port.SpecialSpells
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Helpers;
    using Spells;
    using SpellData = Spells.SpellData;

    class Ekko : ChampionPlugin
    {
        public void LoadSpecialSpell(SpellData spellData)
        {
            if (spellData.SpellName == "EkkoR")
            {
                SpellDetector.OnProcessSpecialSpell += ProcessSpell_EkkoR;
            }
        }

        private static void ProcessSpell_EkkoR(Obj_AI_Base hero, Obj_AI_BaseMissileClientDataEventArgs args, SpellData spellData, SpecialSpellEventArgs specialSpellArgs)
        {
            if (spellData.SpellName == "EkkoR")
            {
                foreach (var obj in ObjectManager.Get<Obj_AI_Minion>())
                {
                    if (obj != null && obj.IsValid && !obj.IsDead && obj.Name == "Ekko" && obj.CheckTeam())
                    {
                        var blinkPos = obj.ServerPosition.To2D();

                        SpellDetector.CreateSpellData(hero, args.Start, blinkPos.To3D(), spellData);
                    }
                }

                specialSpellArgs.NoProcess = true;
            }
        }
    }
}
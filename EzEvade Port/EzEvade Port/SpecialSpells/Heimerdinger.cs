namespace EzEvade_Port.SpecialSpells
{
    using Aimtec;
    using Spells;
    using SpellData = Spells.SpellData;

    class Heimerdinger : ChampionPlugin
    {
        public void LoadSpecialSpell(SpellData spellData)
        {
            if (spellData.SpellName == "HeimerdingerTurretEnergyBlast" || spellData.SpellName == "HeimerdingerTurretBigEnergyBlast")
            {
                SpellDetector.OnProcessSpecialSpell += ProcessSpell_HeimerdingerTurretEnergyBlast;
            }

            if (spellData.SpellName == "HeimerdingerW")
            {
                //SpellDetector.OnProcessSpecialSpell += ProcessSpell_HeimerdingerW;
            }
        }

        private void ProcessSpell_HeimerdingerW(Obj_AI_Base hero, Obj_AI_BaseMissileClientDataEventArgs args, SpellData spellData, SpecialSpellEventArgs specialSpellArgs)
        {
            if (spellData.SpellName == "HeimerdingerW")
            {
                specialSpellArgs.NoProcess = true;
            }
        }

        private static void ProcessSpell_HeimerdingerTurretEnergyBlast(Obj_AI_Base hero, Obj_AI_BaseMissileClientDataEventArgs args, SpellData spellData, SpecialSpellEventArgs specialSpellArgs)
        {
            if (spellData.SpellName == "HeimerdingerTurretEnergyBlast" || spellData.SpellName == "HeimerdingerTurretBigEnergyBlast")
            {
                SpellDetector.CreateSpellData(hero, args.Start, args.End, spellData);

                specialSpellArgs.NoProcess = true;
            }
        }
    }
}
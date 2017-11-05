namespace EzEvade_Port.SpecialSpells
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Spells;
    using Utils;
    using SpellData = Spells.SpellData;

    class Ashe : ChampionPlugin
    {
        public void LoadSpecialSpell(SpellData spellData)
        {
            if (spellData.SpellName == "Volley")
            {
                SpellDetector.OnProcessSpecialSpell += ProcessSpell_AsheVolley;
            }
        }

        private static void ProcessSpell_AsheVolley(Obj_AI_Base hero, Obj_AI_BaseMissileClientDataEventArgs args, SpellData spellData, SpecialSpellEventArgs specialSpellArgs)
        {
            if (spellData.SpellName == "Volley")
            {
                for (var i = -4; i < 5; i++)
                {
                    var endPos2 = MathUtils.RotateVector(args.Start.To2D(), args.End.To2D(), i * spellData.Angle).To3D();
                    if (i != 0)
                    {
                        SpellDetector.CreateSpellData(hero, args.Start, endPos2, spellData, null, 0, false);
                    }
                }
            }
        }
    }
}
namespace EzEvade_Port.SpecialSpells
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Helpers;
    using Spells;
    using SpellData = Spells.SpellData;

    class Lucian : ChampionPlugin
    {
        public void LoadSpecialSpell(SpellData spellData)
        {
            if (spellData.SpellName == "LucianQ")
            {
                SpellDetector.OnProcessSpecialSpell += ProcessSpell_LucianQ;
            }
        }

        private static void ProcessSpell_LucianQ(Obj_AI_Base hero, Obj_AI_BaseMissileClientDataEventArgs args, SpellData spellData, SpecialSpellEventArgs specialSpellArgs)
        {
            if (spellData.SpellName == "LucianQ")
            {
                if (args.Target.IsValid && args.Target.Type == GameObjectType.obj_AI_Base)
                {
                    var target = args.Target as Obj_AI_Base;

                    var spellDelay = (350 - ObjectCache.GamePing) / 1000;
                    var heroWalkDir = (target.ServerPosition - target.Position).Normalized();
                    var predictedHeroPos = target.Position + heroWalkDir * target.MoveSpeed * spellDelay;

                    SpellDetector.CreateSpellData(hero, args.Start, predictedHeroPos, spellData);

                    specialSpellArgs.NoProcess = true;
                }
            }
        }
    }
}
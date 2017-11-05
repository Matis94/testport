namespace EzEvade_Port.SpecialSpells
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Util.Cache;
    using Helpers;
    using SpellData = Spells.SpellData;

    class Yasuo : ChampionPlugin
    {
        public void LoadSpecialSpell(SpellData spellData)
        {
            if (spellData.SpellName == "YasuoQW" || spellData.SpellName == "YasuoQ3W")
            {
                var hero = GameObjects.Heroes.FirstOrDefault(h => h.ChampionName == "Yasuo");
                if (hero != null && hero.CheckTeam())
                {
                    Obj_AI_Base.OnProcessSpellCast += (sender, args) => ProcessSpell_YasuoQW(sender, args, spellData);
                }
            }
        }

        private static void ProcessSpell_YasuoQW(Obj_AI_Base hero, Obj_AI_BaseMissileClientDataEventArgs args, SpellData spellData)
        {
            if (hero.IsEnemy && args.SpellData.Name == "YasuoQ")
            {
                // Not sure with castendtime
                var castTime = (hero.SpellBook.CastEndTime - Game.ClockTime) * 1000;

                if (castTime > 0)
                {
                    spellData.SpellDelay = castTime;
                }
            }
        }
    }
}
namespace EzEvade_Port.SpecialSpells
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util.Cache;
    using Helpers;
    using Spells;
    using SpellData = Spells.SpellData;

    class Ahri : ChampionPlugin
    {
        public void LoadSpecialSpell(SpellData spellData)
        {
            if (spellData.SpellName == "AhriOrbofDeception2")
            {
                var hero = GameObjects.Heroes.FirstOrDefault(x => x.ChampionName == "Ahri");
                if (hero != null && hero.CheckTeam())
                {
                    Game.OnUpdate += () => Game_OnUpdate(hero);
                }
            }
        }

        private void Game_OnUpdate(Obj_AI_Hero hero)
        {
            foreach (var spell in SpellDetector.DetectedSpells.Where(s => s.Value.HeroId == hero.NetworkId && s.Value.Info.SpellName.ToLower() == "ahriorbofdeception2"))
            {
                spell.Value.EndPos = hero.ServerPosition.To2D();
            }
        }
    }
}
namespace EzEvade_Port.SpecialSpells
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util.Cache;
    using Helpers;
    using Spells;
    using SpellData = Spells.SpellData;

    class Darius : ChampionPlugin
    {
        public void LoadSpecialSpell(SpellData spellData)
        {
            if (spellData.SpellName == "DariusCleave")
            {
                var hero = GameObjects.Heroes.FirstOrDefault(x => x.ChampionName == "Darius");
                if (hero != null && hero.CheckTeam())
                {
                    Game.OnUpdate += () => Game_OnUpdate(hero);
                }
            }
        }

        private void Game_OnUpdate(Obj_AI_Base hero)
        {
            foreach (var spell in SpellDetector.DetectedSpells.Where(x => x.Value.HeroId == hero.NetworkId))
            {
                if (spell.Value.Info.SpellName == "DariusCleave")
                {
                    spell.Value.StartPos = hero.ServerPosition.To2D();
                    spell.Value.EndPos = hero.ServerPosition.To2D() + spell.Value.Direction * spell.Value.Info.Range;
                }
            }
        }
    }
}
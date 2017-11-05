namespace EzEvade_Port.SpecialSpells
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util.Cache;
    using Helpers;
    using Spells;
    using SpellData = Spells.SpellData;

    class Taric : ChampionPlugin
    {
        public void LoadSpecialSpell(SpellData spellData)
        {
            if (spellData.SpellName == "TaricE")
            {
                var hero = GameObjects.Heroes.FirstOrDefault(x => x.ChampionName == "Taric");
                if (hero != null)
                {
                    Game.OnUpdate += () => Game_OnUpdate(hero);
                    SpellDetector.OnProcessSpecialSpell += SpellDetector_OnProcessSpecialSpell;
                }
            }
        }

        private void Game_OnUpdate(Obj_AI_Hero hero)
        {
            if (hero != null && hero.CheckTeam())
            {
                foreach (var spell in SpellDetector.DetectedSpells.Where(x => x.Value.HeroId == hero.NetworkId))
                {
                    if (spell.Value.Info.SpellName.ToLower() == "tarice")
                    {
                        spell.Value.StartPos = hero.ServerPosition.To2D();
                        spell.Value.EndPos = hero.ServerPosition.To2D() + spell.Value.Direction * spell.Value.Info.Range;
                    }
                }

                var partner = GameObjects.Heroes.FirstOrDefault(x => x.HasBuff("taricwleashactive"));
                if (partner != null && partner.CheckTeam())
                {
                    foreach (var spell in SpellDetector.DetectedSpells.Where(x => x.Value.HeroId == partner.NetworkId))
                    {
                        if (spell.Value.Info.SpellName.ToLower() == "tarice")
                        {
                            spell.Value.StartPos = partner.ServerPosition.To2D();
                            spell.Value.EndPos = partner.ServerPosition.To2D() + spell.Value.Direction * spell.Value.Info.Range;
                        }
                    }
                }
            }
        }

        private void SpellDetector_OnProcessSpecialSpell(Obj_AI_Base hero, Obj_AI_BaseMissileClientDataEventArgs args, SpellData spellData, SpecialSpellEventArgs specialSpellArgs)
        {
            if (spellData.SpellName == "TaricE")
            {
                var partner = GameObjects.Heroes.FirstOrDefault(x => x.ChampionName != "Taric" && x.HasBuff("taricwleashactive"));
                if (partner != null && partner.CheckTeam())
                {
                    var start = partner.ServerPosition.To2D();
                    var direction = (args.End.To2D() - start).Normalized();
                    var end = start + direction * spellData.Range;

                    SpellDetector.CreateSpellData(partner, start.To3D(), end.To3D(), spellData);
                }
            }
        }
    }
}
namespace EzEvade_Port.SpecialSpells
{
    using System;
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util.Cache;
    using Helpers;
    using Spells;
    using Utils;
    using SpellData = Spells.SpellData;

    class Sion : ChampionPlugin
    {
        public void LoadSpecialSpell(SpellData spellData)
        {
            if (spellData.SpellName == "SionR")
            {
                var hero = GameObjects.Heroes.FirstOrDefault(x => x.ChampionName == "Sion");
                if (hero != null && hero.CheckTeam())
                {
                    Game.OnUpdate += () => Game_OnUpdate(hero);
                    SpellDetector.OnProcessSpecialSpell += SpellDetector_OnProcessSpecialSpell;
                }
            }
        }

        private void SpellDetector_OnProcessSpecialSpell(Obj_AI_Base hero, Obj_AI_BaseMissileClientDataEventArgs args, SpellData spellData, SpecialSpellEventArgs specialSpellArgs)
        {
            if (spellData.SpellName == "SionR")
            {
                spellData.ProjectileSpeed = hero.MoveSpeed;
                specialSpellArgs.SpellData = spellData;
            }
        }

        private void Game_OnUpdate(Obj_AI_Hero hero)
        {
            foreach (var spell in SpellDetector.DetectedSpells.Where(x => x.Value.HeroId == hero.NetworkId && x.Value.Info.SpellName == "SionR"))
            {
                var facingPos = hero.ServerPosition.To2D() + hero.Orientation.To2D().Perpendicular();
                var endPos = hero.ServerPosition.To2D() + (facingPos - hero.ServerPosition.To2D()).Normalized() * 450;

                spell.Value.StartPos = hero.ServerPosition.To2D();
                spell.Value.EndPos = endPos;

                if (Environment.TickCount - spell.Value.StartTime >= 1000)
                {
                    SpellDetector.CreateSpellData(hero, hero.ServerPosition, endPos.To3D(), spell.Value.Info, null, 0, false, SpellType.Line, false);
                    spell.Value.StartTime = Environment.TickCount;
                    break;
                }
            }
        }
    }
}
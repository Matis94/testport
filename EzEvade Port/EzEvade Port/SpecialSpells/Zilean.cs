namespace EzEvade_Port.SpecialSpells
{
    using System.Collections.Generic;
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util.Cache;
    using Helpers;
    using Spells;
    using SpellData = Spells.SpellData;

    class Zilean : ChampionPlugin
    {
        internal const string ObjName = "TimeBombGround";
        private static readonly List<GameObject> Bombs = new List<GameObject>();
        private static readonly Dictionary<float, Vector3> QSpots = new Dictionary<float, Vector3>();

        public void LoadSpecialSpell(SpellData spellData)
        {
            if (spellData.SpellName != "ZileanQ")
            {
                return;
            }

            var hero = GameObjects.Heroes.FirstOrDefault(x => x.ChampionName == "Zilean");
            if (hero == null || !hero.CheckTeam())
            {
                return;
            }

            Game.OnUpdate += Game_OnUpdate;
            GameObject.OnCreate += GameObject_OnCreate;
            GameObject.OnDestroy += GameObject_OnDelete;
            SpellDetector.OnProcessSpecialSpell += SpellDetector_OnProcessSpecialSpell;
        }

        private void Game_OnUpdate()
        {
            Bombs.RemoveAll(i => !i.IsValid || i.IsDead || !i.IsVisible);

            foreach (var spot in QSpots.ToArray())
            {
                var timestamp = spot.Key;
                if (Game.ClockTime - timestamp >= 2.5f * 0.6f)
                {
                    QSpots.Remove(timestamp);
                }
            }
        }

        private void GameObject_OnCreate(GameObject bomb)
        {
            if (!bomb.Name.Contains(ObjName) || !bomb.CheckTeam())
            {
                return;
            }

            if (Bombs.Contains(bomb))
            {
                return;
            }

            RemovePairsNear(bomb.Position);
            Bombs.Add(bomb);
        }

        private void GameObject_OnDelete(GameObject bomb)
        {
            if (bomb.Name.Contains(ObjName) && bomb.CheckTeam())
            {
                Bombs.RemoveAll(i => i.NetworkId == bomb.NetworkId);
            }
        }

        private void SpellDetector_OnProcessSpecialSpell(Obj_AI_Base hero, Obj_AI_BaseMissileClientDataEventArgs args, SpellData spellData, SpecialSpellEventArgs specialSpellArgs)
        {
            if (spellData.SpellName != "ZileanQ")
            {
                return;
            }

            var end = args.End;
            if (args.Start.Distance(end) > spellData.Range)
            {
                end = args.Start + (args.End - args.Start).Normalized() * spellData.Range;
            }

            foreach (var bomb in Bombs.Where(b => b.IsValid && !b.IsDead && b.IsVisible))
            {
                var newData = (SpellData) spellData.Clone();
                newData.Radius = 350;

                if (!(end.Distance(bomb.Position) <= newData.Radius))
                {
                    continue;
                }

                SpellDetector.CreateSpellData(hero, hero.ServerPosition, bomb.Position, newData, null, 0, true, SpellType.Circular, false, newData.Radius);
                SpellDetector.CreateSpellData(hero, hero.ServerPosition, end, newData, null, 0, true, SpellType.Circular, false, newData.Radius);
                specialSpellArgs.NoProcess = true;
            }

            foreach (var bombPosition in QSpots.Values)
            {
                var newData = (SpellData) spellData.Clone();
                newData.Radius = 350;

                if (!(end.Distance(bombPosition) <= newData.Radius) || QSpots.Count <= 1)
                {
                    continue;
                }

                SpellDetector.CreateSpellData(hero, hero.ServerPosition, bombPosition, newData, null, 0, true, SpellType.Circular, false, newData.Radius);
                SpellDetector.CreateSpellData(hero, hero.ServerPosition, end, newData, null, 0, true, SpellType.Circular, false, newData.Radius);
                specialSpellArgs.NoProcess = true;
            }

            QSpots[Game.ClockTime] = end;
        }

        private static void RemovePairsNear(Vector3 pos)
        {
            foreach (var pair in QSpots.ToArray().Where(o => o.Value.Distance(pos) <= 30))
            {
                QSpots.Remove(pair.Key);
            }
        }
    }
}
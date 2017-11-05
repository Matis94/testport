namespace EzEvade_Port.SpecialSpells
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util.Cache;
    using Helpers;
    using Spells;
    using Utils;
    using SpellData = Spells.SpellData;

    class Lux : ChampionPlugin
    {
        public void LoadSpecialSpell(SpellData spellData)
        {
            if (spellData.SpellName == "LuxMaliceCannon")
            {
                var hero = GameObjects.Heroes.FirstOrDefault(h => h.ChampionName == "Lux");
                if (hero != null && hero.CheckTeam())
                {
                    ObjectTracker.HuiTrackerForceLoad();
                    GameObject.OnCreate += obj => OnCreateObj_LuxMaliceCannon(obj, hero, spellData);
                }
            }
        }

        private static void OnCreateObj_LuxMaliceCannon(GameObject obj, Obj_AI_Hero hero, SpellData spellData)
        {
            if (obj.Name.Contains("Lux") && obj.Name.Contains("R_mis_beam_middle"))
            {
                if (hero.IsVisible)
                {
                    return;
                }

                var objList = ObjectTracker.ObjTracker.Values.Where(o => o.Name == "hiu");
                if (objList.Count() >= 2)
                {
                    var dir = ObjectTracker.GetLastHiuOrientation();
                    var pos1 = obj.Position.To2D() - dir * 1750;
                    var pos2 = obj.Position.To2D() + dir * 1750;

                    SpellDetector.CreateSpellData(hero, pos1.To3D(), pos2.To3D(), spellData);

                    foreach (var gameObj in objList)
                    {
                        DelayAction.Add(1, () => ObjectTracker.ObjTracker.Remove(gameObj.Obj.NetworkId));
                    }
                }
            }
        }
    }
}
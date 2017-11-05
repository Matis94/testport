namespace EzEvade_Port.SpecialSpells
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Helpers;
    using Spells;
    using SpellData = Spells.SpellData;

    class Viktor : ChampionPlugin
    {
        public void LoadSpecialSpell(SpellData spellData)
        {
            if (spellData.SpellName == "ViktorDeathRay3")
            {
                GameObject.OnCreate += OnCreateObj_ViktorDeathRay3;
            }
        }

        private static void OnCreateObj_ViktorDeathRay3(GameObject obj)
        {
            if (!obj.IsValid)
            {
                return;
            }

            var missile = (MissileClient) obj;

            SpellData spellData;

            if (missile.SpellCaster != null && missile.SpellCaster.CheckTeam() && missile.SpellData.Name != null && missile.SpellData.Name.ToLower() == "viktoreaugmissile" &&
                SpellDetector.OnMissileSpells.TryGetValue("viktordeathray3", out spellData) && missile.StartPosition != null && missile.EndPosition != null)
            {
                var newData = (SpellData) spellData.Clone();
                var missileDist = missile.EndPosition.To2D().Distance(missile.StartPosition.To2D());

                newData.SpellDelay = missileDist / 1.5f + 1000;
                SpellDetector.CreateSpellData(missile.SpellCaster, missile.StartPosition, missile.EndPosition, newData);
            }
        }
    }
}
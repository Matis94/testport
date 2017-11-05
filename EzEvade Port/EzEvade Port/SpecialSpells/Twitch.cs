namespace EzEvade_Port.SpecialSpells
{
    using Aimtec;
    using Spells;
    using SpellData = Spells.SpellData;

    class Twitch : ChampionPlugin
    {
        public void LoadSpecialSpell(SpellData spellData)
        {
            if (spellData.SpellName == "TwitchSprayandPrayAttack")
            {
                SpellDetector.OnProcessSpecialSpell += ProcessSpell_TwitchSprayandPrayAttack;
            }
        }

        private void ProcessSpell_TwitchSprayandPrayAttack(Obj_AI_Base hero, Obj_AI_BaseMissileClientDataEventArgs args, SpellData spellData, SpecialSpellEventArgs specialSpellArgs)
        {
            if (spellData.SpellName == "TwitchSprayandPrayAttack")
            {
                if (args.Target != null)
                {
                    var start = hero.ServerPosition;
                    var end = hero.ServerPosition + (args.Target.Position - hero.ServerPosition) * spellData.Range;

                    var data = (SpellData) spellData.Clone();
                    data.SpellDelay = hero.AttackCastDelay * 1000;

                    SpellDetector.CreateSpellData(hero, start, end, data);
                }
            }
        }
    }
}
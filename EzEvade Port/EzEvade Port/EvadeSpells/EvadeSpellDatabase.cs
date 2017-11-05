namespace EzEvade_Port.EvadeSpells
{
    using System.Collections.Generic;
    using Aimtec;
    using Aimtec.SDK.Damage;

    class EvadeSpellDatabase
    {
        public static List<EvadeSpellData> Spells = new List<EvadeSpellData>();

        static EvadeSpellDatabase()
        {
            #region Ahri

            Spells.Add(new EvadeSpellData
            {
                CharName = "Ahri",
                Dangerlevel = 4,
                Name = "AhriTumble",
                SpellName = "AhriTumble",
                Range = 500,
                SpellDelay = 50,
                Speed = 1575,
                SpellKey = SpellSlot.R,
                EvadeType = EvadeType.Dash,
                CastType = CastType.Position
            });

            #endregion

            #region Akali

            //Spells.Add(
            //new EvadeSpellData
            //{
            //    charName = "Akali",
            //    dangerlevel = 4,
            //    name = "Twilight Shroud",
            //    spellName = "AkaliSmokeBomb",
            //    spellDelay = 850,
            //    spellKey = SpellSlot.W,
            //    speedArray = new[] { 20f, 40f, 60f, 80f, 100f },
            //    evadeType = EvadeType.MovementSpeedBuff,
            //    castType = CastType.Position
            //});

            #endregion

            #region Blitzcrank

            Spells.Add(new EvadeSpellData
            {
                CharName = "Blitzcrank",
                Dangerlevel = 3,
                Name = "Overdrive",
                SpellName = "Overdrive",
                SpellDelay = 250,
                SpellKey = SpellSlot.W,
                SpeedArray = new[] {70f, 75f, 80f, 85f, 90f},
                EvadeType = EvadeType.MovementSpeedBuff,
                CastType = CastType.Self
            });

            #endregion

            #region Caitlyn

            Spells.Add(new EvadeSpellData
            {
                CharName = "Caitlyn",
                Dangerlevel = 3,
                Name = "CaitlynEntrapment",
                SpellName = "CaitlynEntrapment",
                Range = 400,
                SpellDelay = 50,
                Speed = 975,
                IsReversed = true,
                FixedRange = true,
                SpellKey = SpellSlot.E,
                EvadeType = EvadeType.Dash,
                CastType = CastType.Position
            });

            #endregion

            #region Corki

            Spells.Add(new EvadeSpellData
            {
                CharName = "Corki",
                Dangerlevel = 3,
                Name = "CarpetBomb",
                SpellName = "CarpetBomb",
                Range = 790,
                SpellDelay = 50,
                Speed = 975,
                SpellKey = SpellSlot.W,
                EvadeType = EvadeType.Dash,
                CastType = CastType.Position
            });

            #endregion

            #region Draven

            Spells.Add(new EvadeSpellData
            {
                CharName = "Draven",
                Dangerlevel = 3,
                Name = "Blood Rush",
                SpellName = "DravenFury",
                SpellDelay = 250,
                SpellKey = SpellSlot.W,
                SpeedArray = new[] {40f, 45f, 50f, 55f, 60f},
                EvadeType = EvadeType.MovementSpeedBuff,
                CastType = CastType.Self
            });

            #endregion

            #region Ekko

            Spells.Add(new EvadeSpellData
            {
                CharName = "Ekko",
                Dangerlevel = 3,
                Name = "PhaseDive",
                SpellName = "EkkoE",
                Range = 350,
                FixedRange = true,
                SpellDelay = 50,
                Speed = 1150,
                SpellKey = SpellSlot.E,
                EvadeType = EvadeType.Dash,
                CastType = CastType.Position
            });

            Spells.Add(new EvadeSpellData
            {
                CharName = "Ekko",
                Dangerlevel = 3,
                Name = "PhaseDive2",
                SpellName = "EkkoEAttack",
                Range = 490,
                SpellDelay = 250,
                InfrontTarget = true,
                SpellKey = SpellSlot.Recall,
                EvadeType = EvadeType.Blink,
                CastType = CastType.Target,
                SpellTargets = new[] {SpellTargets.EnemyChampions, SpellTargets.EnemyMinions},
                IsSpecial = true
            });

            Spells.Add(new EvadeSpellData
            {
                CharName = "Ekko",
                Dangerlevel = 4,
                Name = "Chronobreak",
                SpellName = "EkkoR",
                Range = 20000,
                SpellDelay = 50,
                SpellKey = SpellSlot.R,
                EvadeType = EvadeType.Blink,
                CastType = CastType.Self,
                IsSpecial = true
            });

            #endregion

            #region Elise

            Spells.Add(new EvadeSpellData
            {
                CharName = "Elise",
                Dangerlevel = 4,
                Name = "Rappel",
                SpellName = "EliseSpiderEInitial",
                SpellDelay = 50,
                CheckSpellName = true,
                SpellKey = SpellSlot.E,
                EvadeType = EvadeType.SpellShield,
                CastType = CastType.Self,
                Untargetable = true,
                IsSpecial = true
            });

            #endregion

            #region Evelynn

            Spells.Add(new EvadeSpellData
            {
                CharName = "Evelynn",
                Dangerlevel = 3,
                Name = "Darl Frenzy",
                SpellName = "EvelynnW",
                SpellDelay = 250,
                SpellKey = SpellSlot.W,
                SpeedArray = new[] {30f, 45f, 50f, 60f, 70f},
                EvadeType = EvadeType.MovementSpeedBuff,
                CastType = CastType.Self
            });

            #endregion

            #region Ezreal

            Spells.Add(new EvadeSpellData
            {
                CharName = "Ezreal",
                Dangerlevel = 2,
                Name = "ArcaneShift",
                SpellName = "EzrealArcaneShift",
                Range = 450,
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                EvadeType = EvadeType.Blink,
                CastType = CastType.Position
            });

            #endregion

            #region Fiora

            Spells.Add(new EvadeSpellData
            {
                CharName = "Fiora",
                Dangerlevel = 3,
                Name = "FioraW",
                SpellName = "FioraW",
                Range = 750,
                SpellDelay = 100,
                SpellKey = SpellSlot.W,
                EvadeType = EvadeType.WindWall,
                CastType = CastType.Position
            });

            Spells.Add(new EvadeSpellData
            {
                CharName = "Fiora",
                Dangerlevel = 3,
                Name = "FioraQ",
                SpellName = "FioraQ",
                Range = 340,
                FixedRange = true,
                Speed = 1100,
                SpellDelay = 50,
                SpellKey = SpellSlot.Q,
                EvadeType = EvadeType.Dash,
                CastType = CastType.Position
            });

            #endregion

            #region Fizz

            Spells.Add(new EvadeSpellData
            {
                CharName = "Fizz",
                Dangerlevel = 3,
                Name = "FizzPiercingStrike",
                SpellName = "FizzPiercingStrike",
                Range = 550,
                Speed = 1400,
                FixedRange = true,
                SpellDelay = 50,
                SpellKey = SpellSlot.Q,
                EvadeType = EvadeType.Dash,
                CastType = CastType.Target,
                SpellTargets = new[] {SpellTargets.EnemyMinions, SpellTargets.EnemyChampions}
            });

            Spells.Add(new EvadeSpellData
            {
                CharName = "Fizz",
                Dangerlevel = 3,
                Name = "FizzJump",
                SpellName = "FizzJump",
                Range = 400,
                Speed = 1400,
                FixedRange = true,
                SpellDelay = 50,
                SpellKey = SpellSlot.E,
                EvadeType = EvadeType.Dash,
                CastType = CastType.Position,
                Untargetable = true
            });

            #endregion

            #region Galio

            Spells.Add(new EvadeSpellData
            {
                CharName = "Galio",
                Dangerlevel = 4,
                Name = "Righteous Gust",
                SpellName = "GalioRighteousGust",
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                SpeedArray = new[] {30f, 35f, 40f, 45f, 50f},
                EvadeType = EvadeType.MovementSpeedBuff,
                CastType = CastType.Position
            });

            #endregion

            #region Garen

            Spells.Add(new EvadeSpellData
            {
                CharName = "Garen",
                Dangerlevel = 3,
                Name = "Decisive Strike",
                SpellName = "GarenQ",
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpeedArray = new[] {35, 35f, 35f, 35f, 35f},
                EvadeType = EvadeType.MovementSpeedBuff,
                CastType = CastType.Self
            });

            #endregion

            #region Gragas

            Spells.Add(new EvadeSpellData
            {
                CharName = "Gragas",
                Dangerlevel = 2,
                Name = "BodySlam",
                SpellName = "GragasBodySlam",
                Range = 600,
                SpellDelay = 50,
                Speed = 900,
                SpellKey = SpellSlot.E,
                EvadeType = EvadeType.Dash,
                CastType = CastType.Position
            });

            #endregion

            #region Gnar

            Spells.Add(new EvadeSpellData
            {
                CharName = "Gnar",
                Dangerlevel = 3,
                Name = "GnarE",
                SpellName = "GnarE",
                Range = 475,
                SpellDelay = 50,
                Speed = 900,
                CheckSpellName = true,
                SpellKey = SpellSlot.E,
                EvadeType = EvadeType.Dash,
                CastType = CastType.Position
            });

            Spells.Add(new EvadeSpellData
            {
                CharName = "Gnar",
                Dangerlevel = 4,
                Name = "GnarBigE",
                SpellName = "gnarbige",
                Range = 475,
                SpellDelay = 50,
                Speed = 800,
                CheckSpellName = true,
                SpellKey = SpellSlot.E,
                EvadeType = EvadeType.Dash,
                CastType = CastType.Position
            });

            #endregion

            #region Graves

            Spells.Add(new EvadeSpellData
            {
                CharName = "Graves",
                Dangerlevel = 2,
                Name = "QuickDraw",
                SpellName = "GravesMove",
                Range = 425,
                SpellDelay = 50,
                Speed = 1250,
                SpellKey = SpellSlot.E,
                EvadeType = EvadeType.Dash,
                CastType = CastType.Position
            });

            #endregion

            #region Karma

            Spells.Add(new EvadeSpellData
            {
                CharName = "Karma",
                Dangerlevel = 3,
                Name = "Inspire",
                SpellName = "KarmaSolkimShield",
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                SpeedArray = new[] {40f, 45f, 50f, 55f, 60f},
                EvadeType = EvadeType.MovementSpeedBuff,
                CastType = CastType.Target
            });

            #endregion

            #region Kassadin

            Spells.Add(new EvadeSpellData
            {
                CharName = "Kassadin",
                Dangerlevel = 1,
                Name = "RiftWalk",
                Range = 450,
                SpellDelay = 250,
                SpellKey = SpellSlot.R,
                EvadeType = EvadeType.Blink,
                CastType = CastType.Position
            });

            #endregion

            #region Katarina

            Spells.Add(new EvadeSpellData
            {
                CharName = "Katarina",
                Dangerlevel = 3,
                Name = "KatarinaE",
                SpellName = "KatarinaE",
                Range = 700,
                Speed = float.MaxValue,
                SpellDelay = 50,
                SpellKey = SpellSlot.E,
                EvadeType = EvadeType.Blink, //behind target
                CastType = CastType.Target,
                SpellTargets = new[] {SpellTargets.Targetables}
            });

            #endregion

            #region Kayle

            Spells.Add(new EvadeSpellData
            {
                CharName = "Kayle",
                Dangerlevel = 3,
                Name = "Divine Blessing",
                SpellName = "JudicatorDivineBlessing",
                SpellDelay = 250,
                SpellKey = SpellSlot.W,
                SpeedArray = new[] {18f, 21f, 24f, 27f, 30f},
                EvadeType = EvadeType.MovementSpeedBuff,
                CastType = CastType.Target
            });

            Spells.Add(new EvadeSpellData
            {
                CharName = "Kayle",
                Dangerlevel = 4,
                Name = "Intervention",
                SpellName = "JudicatorIntervention",
                SpellDelay = 250,
                SpellKey = SpellSlot.R,
                EvadeType = EvadeType.SpellShield, //Invulnerability
                CastType = CastType.Target,
                SpellTargets = new[] {SpellTargets.AllyChampions}
            });

            #endregion

            #region Kennen

            Spells.Add(new EvadeSpellData
            {
                CharName = "Kennen",
                Dangerlevel = 4,
                Name = "Lightning Rush",
                SpellName = "KennenLightningRush",
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                SpeedArray = new[] {100f, 100f, 100f, 100f, 100f},
                EvadeType = EvadeType.MovementSpeedBuff,
                CastType = CastType.Self
            });

            #endregion

            #region Kindred

            Spells.Add(new EvadeSpellData
            {
                CharName = "Kindred",
                Dangerlevel = 1,
                Name = "KindredQ",
                SpellName = "KindredQ",
                Range = 300,
                FixedRange = true,
                Speed = 733,
                SpellDelay = 50,
                SpellKey = SpellSlot.Q,
                EvadeType = EvadeType.Dash,
                CastType = CastType.Position
            });

            #endregion

            #region Leblanc

            Spells.Add(new EvadeSpellData
            {
                CharName = "Leblanc",
                Dangerlevel = 2,
                Name = "Distortion",
                SpellName = "LeblancSlide",
                Range = 600,
                SpellDelay = 50,
                Speed = 1600,
                SpellKey = SpellSlot.W,
                EvadeType = EvadeType.Dash,
                CastType = CastType.Position
            });

            Spells.Add(new EvadeSpellData
            {
                CharName = "Leblanc",
                Dangerlevel = 2,
                Name = "DistortionR",
                SpellName = "LeblancSlideM",
                CheckSpellName = true,
                Range = 600,
                SpellDelay = 50,
                Speed = 1600,
                SpellKey = SpellSlot.R,
                EvadeType = EvadeType.Dash,
                CastType = CastType.Position
            });

            #endregion

            #region LeeSin

            Spells.Add(new EvadeSpellData
            {
                CharName = "LeeSin",
                Dangerlevel = 3,
                Name = "LeeSinW",
                SpellName = "BlindMonkWOne",
                Range = 700,
                Speed = 1400,
                SpellDelay = 50,
                SpellKey = SpellSlot.W,
                EvadeType = EvadeType.Shield,
                CastType = CastType.Target,
                SpellTargets = new[] {SpellTargets.AllyChampions, SpellTargets.AllyMinions}
            });

            #endregion

            #region Lucian

            Spells.Add(new EvadeSpellData
            {
                CharName = "Lucian",
                Dangerlevel = 1,
                Name = "RelentlessPursuit",
                SpellName = "LucianE",
                Range = 425,
                SpellDelay = 50,
                Speed = 1350,
                SpellKey = SpellSlot.E,
                EvadeType = EvadeType.Dash,
                CastType = CastType.Position
            });

            #endregion

            #region Lulu

            Spells.Add(new EvadeSpellData
            {
                CharName = "Lulu",
                Dangerlevel = 3,
                Name = "Whimsy",
                SpellName = "LuluW",
                SpellDelay = 250,
                SpellKey = SpellSlot.W,
                SpeedArray = new[] {30f, 30f, 30f, 35f, 40f},
                EvadeType = EvadeType.MovementSpeedBuff,
                CastType = CastType.Target
            });

            #endregion

            #region MasterYi

            Spells.Add(new EvadeSpellData
            {
                CharName = "MasterYi",
                Dangerlevel = 3,
                Name = "AlphaStrike",
                SpellName = "AlphaStrike",
                Range = 600,
                Speed = float.MaxValue,
                SpellDelay = 100,
                SpellKey = SpellSlot.Q,
                EvadeType = EvadeType.Blink,
                CastType = CastType.Target,
                SpellTargets = new[] {SpellTargets.EnemyChampions, SpellTargets.EnemyMinions},
                Untargetable = true
            });

            #endregion

            #region Morgana

            Spells.Add(new EvadeSpellData
            {
                CharName = "Morgana",
                Dangerlevel = 3,
                Name = "BlackShield",
                SpellName = "BlackShield",
                SpellDelay = 50,
                SpellKey = SpellSlot.E,
                EvadeType = EvadeType.SpellShield,
                CastType = CastType.Target,
                SpellTargets = new[] {SpellTargets.AllyChampions}
            });

            #endregion

            #region Nocturne

            Spells.Add(new EvadeSpellData
            {
                CharName = "Nocturne",
                Dangerlevel = 3,
                Name = "ShroudofDarkness",
                SpellName = "NocturneShroudofDarkness",
                SpellDelay = 50,
                SpellKey = SpellSlot.W,
                EvadeType = EvadeType.SpellShield,
                CastType = CastType.Self
            });

            #endregion

            #region Nunu

            Spells.Add(new EvadeSpellData
            {
                CharName = "Nunu",
                Dangerlevel = 2,
                Name = "BloodBoil",
                SpellName = "BloodBoil",
                SpellDelay = 250,
                SpellKey = SpellSlot.W,
                SpeedArray = new[] {8f, 9f, 10f, 11f, 12f},
                EvadeType = EvadeType.MovementSpeedBuff,
                CastType = CastType.Target
            });

            #endregion

            #region Nidalee

            Spells.Add(new EvadeSpellData
            {
                CharName = "Nidalee",
                Dangerlevel = 4,
                Name = "Pounce",
                SpellName = "Pounce",
                Range = 375,
                SpellDelay = 150,
                Speed = 1750,
                SpellKey = SpellSlot.W,
                EvadeType = EvadeType.Dash,
                CastType = CastType.Position,
                IsSpecial = true
            });

            #endregion

            #region Poppy

            Spells.Add(new EvadeSpellData
            {
                CharName = "Poppy",
                Dangerlevel = 3,
                Name = "Steadfast Presence",
                SpellName = "PoppyW",
                SpellDelay = 250,
                SpellKey = SpellSlot.W,
                SpeedArray = new[] {27f, 29f, 31f, 33f, 35f},
                EvadeType = EvadeType.MovementSpeedBuff,
                CastType = CastType.Self
            });

            #endregion

            #region Riven

            Spells.Add(new EvadeSpellData
            {
                CharName = "Riven",
                Dangerlevel = 1,
                Name = "BrokenWings",
                SpellName = "RivenTriCleave",
                Range = 260,
                FixedRange = true,
                SpellDelay = 50,
                Speed = 560,
                SpellKey = SpellSlot.Q,
                EvadeType = EvadeType.Dash,
                CastType = CastType.Position,
                IsSpecial = true
            });

            Spells.Add(new EvadeSpellData
            {
                CharName = "Riven",
                Dangerlevel = 1,
                Name = "Valor",
                SpellName = "RivenFeint",
                Range = 325,
                FixedRange = true,
                SpellDelay = 50,
                Speed = 1200,
                SpellKey = SpellSlot.E,
                EvadeType = EvadeType.Dash,
                CastType = CastType.Position
            });

            #endregion

            #region Rumble

            Spells.Add(new EvadeSpellData
            {
                CharName = "Rumble",
                Dangerlevel = 3,
                Name = "Scrap Shield",
                SpellName = "RumbleShield",
                SpellDelay = 50,
                SpellKey = SpellSlot.W,
                SpeedArray = new[] {10f, 15f, 20f, 25f, 30f},
                EvadeType = EvadeType.MovementSpeedBuff,
                CastType = CastType.Self
            });

            #endregion

            #region Sivir

            //Spells.Add(
            //new EvadeSpellData
            //{
            //    charName = "Sivir",
            //    dangerlevel = 4,
            //    name = "On The Hunt",
            //    spellName = "SivirR",
            //    spellDelay = 250,
            //    spellKey = SpellSlot.R,
            //    speedArray = new[] { 60f, 60f, 60f, 60f, 60f },
            //    evadeType = EvadeType.MovementSpeedBuff,
            //    castType = CastType.Self
            //});

            Spells.Add(new EvadeSpellData
            {
                CharName = "Sivir",
                Dangerlevel = 2,
                Name = "SivirE",
                SpellName = "SivirE",
                SpellDelay = 50,
                SpellKey = SpellSlot.E,
                EvadeType = EvadeType.SpellShield,
                CastType = CastType.Self
            });

            #endregion

            #region Skarner

            Spells.Add(new EvadeSpellData
            {
                CharName = "Skarner",
                Dangerlevel = 3,
                Name = "Exoskeleton",
                SpellName = "SkarnerExoskeleton",
                SpellDelay = 250,
                SpellKey = SpellSlot.W,
                SpeedArray = new[] {16f, 20f, 24f, 28f, 32f},
                EvadeType = EvadeType.MovementSpeedBuff,
                CastType = CastType.Self
            });

            #endregion

            #region Shyvana

            Spells.Add(new EvadeSpellData
            {
                CharName = "Shyvana",
                Dangerlevel = 3,
                Name = "Burnout",
                SpellName = "ShyvanaImmolationAura",
                SpellDelay = 50,
                SpellKey = SpellSlot.W,
                SpeedArray = new[] {30f, 35f, 40f, 45f, 50f},
                EvadeType = EvadeType.MovementSpeedBuff,
                CastType = CastType.Self
            });

            #endregion

            #region Shaco

            Spells.Add(new EvadeSpellData
            {
                CharName = "Shaco",
                Dangerlevel = 3,
                Name = "Deceive",
                SpellName = "Deceive",
                Range = 400,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                EvadeType = EvadeType.Blink,
                CastType = CastType.Position
            });

            /*Spells.Add(
            new EvadeSpellData
            {
                charName = "Shaco",
                dangerlevel = 3,
                name = "JackInTheBox",
                spellName = "JackInTheBox",
                range = 425,
                spellDelay = 250,
                spellKey = SpellSlot.W,
                evadeType = EvadeType.WindWall,
                castType = CastType.Position,
            });*/

            #endregion

            #region Sona

            Spells.Add(new EvadeSpellData
            {
                CharName = "Sona",
                Dangerlevel = 3,
                Name = "Song of Celerity",
                SpellName = "SonaE",
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                SpeedArray = new[] {13f, 14f, 15f, 16f, 25f},
                EvadeType = EvadeType.MovementSpeedBuff,
                CastType = CastType.Self
            });

            #endregion

            #region Talon

            Spells.Add(new EvadeSpellData
            {
                CharName = "Talon",
                Dangerlevel = 4,
                Name = "Shadow Assualt",
                SpellName = "TalonShadowAssault",
                SpellDelay = 250,
                SpellKey = SpellSlot.R,
                SpeedArray = new[] {40f, 40f, 40f, 40f, 40f},
                EvadeType = EvadeType.MovementSpeedBuff,
                CastType = CastType.Self
            });

            #endregion

            #region Teemo

            Spells.Add(new EvadeSpellData
            {
                CharName = "Teemo",
                Dangerlevel = 3,
                Name = "Move Quick",
                SpellName = "MoveQuick",
                SpellDelay = 250,
                SpellKey = SpellSlot.W,
                SpeedArray = new[] {10f, 14f, 18f, 22f, 26f},
                EvadeType = EvadeType.MovementSpeedBuff,
                CastType = CastType.Self
            });

            #endregion

            #region Trundle

            //Spells.Add(
            //new EvadeSpellData
            //{
            //    charName = "Trundle",
            //    dangerlevel = 4,
            //    name = "Frozen Domain",
            //    spellName = "TrundleW",
            //    spellDelay = 250,
            //    spellKey = SpellSlot.W,
            //    speedArray = new[] { 20f, 25f, 30f, 35f, 40f },
            //    evadeType = EvadeType.MovementSpeedBuff,
            //    castType = CastType.Position
            //});

            #endregion

            #region Tristana

            Spells.Add(new EvadeSpellData
            {
                CharName = "Tristana",
                Dangerlevel = 3,
                Name = "RocketJump",
                SpellName = "RocketJump",
                Range = 900,
                SpellDelay = 500,
                Speed = 1100,
                SpellKey = SpellSlot.W,
                EvadeType = EvadeType.Dash,
                CastType = CastType.Position
            });

            #endregion

            #region Tryndamare

            Spells.Add(new EvadeSpellData
            {
                CharName = "Tryndamare",
                Dangerlevel = 3,
                Name = "SpinningSlash",
                SpellName = "Slash",
                Range = 660,
                SpellDelay = 50,
                Speed = 900,
                SpellKey = SpellSlot.E,
                EvadeType = EvadeType.Dash,
                CastType = CastType.Position
            });

            #endregion

            #region Udyr

            Spells.Add(new EvadeSpellData
            {
                CharName = "Udyr",
                Dangerlevel = 3,
                Name = "Bear Stance",
                SpellName = "UdyrBearStance",
                SpellDelay = 50,
                SpellKey = SpellSlot.E,
                SpeedArray = new[] {15f, 20f, 25f, 30f, 35f},
                EvadeType = EvadeType.MovementSpeedBuff,
                CastType = CastType.Self
            });

            #endregion

            #region Vayne

            Spells.Add(new EvadeSpellData
            {
                CharName = "Vayne",
                Dangerlevel = 1,
                Name = "Tumble",
                SpellName = "VayneTumble",
                Range = 300,
                FixedRange = true,
                Speed = 900,
                SpellDelay = 50,
                SpellKey = SpellSlot.Q,
                EvadeType = EvadeType.Dash,
                CastType = CastType.Position
            });

            #endregion

            #region Yasuo

            Spells.Add(new EvadeSpellData
            {
                CharName = "Yasuo",
                Dangerlevel = 2,
                Name = "SweepingBlade",
                SpellName = "YasuoDashWrapper",
                Range = 475,
                FixedRange = true,
                Speed = 1000,
                SpellDelay = 50,
                SpellKey = SpellSlot.E,
                EvadeType = EvadeType.Dash,
                CastType = CastType.Target,
                SpellTargets = new[] {SpellTargets.EnemyChampions, SpellTargets.EnemyMinions}
            });

            Spells.Add(new EvadeSpellData
            {
                CharName = "Yasuo",
                Dangerlevel = 3,
                Name = "WindWall",
                SpellName = "YasuoWMovingWall",
                Range = 400,
                SpellDelay = 250,
                SpellKey = SpellSlot.W,
                EvadeType = EvadeType.WindWall,
                CastType = CastType.Position
            });

            #endregion

            #region Zillean

            Spells.Add(new EvadeSpellData
            {
                CharName = "Zilean",
                Dangerlevel = 3,
                Name = "Timewarp",
                SpellName = "ZileanE",
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                SpeedArray = new[] {40f, 55f, 70f, 85f, 99f},
                EvadeType = EvadeType.MovementSpeedBuff,
                CastType = CastType.Self
            });

            #endregion

            #region AllChampions

            Spells.Add(new EvadeSpellData
            {
                CharName = "AllChampions",
                Dangerlevel = 3,
                Name = "Talisman of Ascension",
                SpellName = "TalismanOfAscension",
                SpellDelay = 50,
                SpellKey = SpellSlot.Q,
                EvadeType = EvadeType.MovementSpeedBuff,
                SpeedArray = new[] {40f, 40f, 40f, 40f, 40f},
                CastType = CastType.Self,
                IsItem = true,
                ItemId = ItemId.TalismanofAscension
            });

            Spells.Add(new EvadeSpellData
            {
                CharName = "AllChampions",
                Dangerlevel = 3,
                Name = "Youmuu's Ghostblade",
                SpellName = "YoumuusGhostblade",
                SpellDelay = 50,
                SpellKey = SpellSlot.Q,
                EvadeType = EvadeType.MovementSpeedBuff,
                SpeedArray = new[] {20f, 20f, 20f, 20f, 20f},
                CastType = CastType.Self,
                IsItem = true,
                ItemId = ItemId.YoumuusGhostblade
            });

            Spells.Add(new EvadeSpellData
            {
                CharName = "AllChampions",
                Dangerlevel = 4,
                Name = "Flash",
                SpellName = "SummonerFlash",
                Range = 400,
                FixedRange = true, //test
                SpellDelay = 50,
                IsSummonerSpell = true,
                SpellKey = SpellSlot.R,
                EvadeType = EvadeType.Blink,
                CastType = CastType.Position
            });

            Spells.Add(new EvadeSpellData
            {
                CharName = "AllChampions",
                Dangerlevel = 4,
                Name = "Hourglass",
                SpellName = "ZhonyasHourglass",
                SpellDelay = 50,
                SpellKey = SpellSlot.Q,
                EvadeType = EvadeType.SpellShield, //Invulnerability
                CastType = CastType.Self,
                IsItem = true,
                ItemId = ItemId.ZhonyasHourglass
            });

            Spells.Add(new EvadeSpellData
            {
                CharName = "AllChampions",
                Dangerlevel = 4,
                Name = "Witchcap",
                SpellName = "Witchcap",
                SpellDelay = 50,
                SpellKey = SpellSlot.Q,
                EvadeType = EvadeType.SpellShield, //Invulnerability
                CastType = CastType.Self,
                IsItem = true,
                ItemId = ItemId.WoogletsWitchcap
            });

            #endregion AllChampions
        }
    }
}
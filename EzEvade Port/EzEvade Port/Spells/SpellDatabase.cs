namespace EzEvade_Port.Spells
{
    using System.Collections.Generic;
    using Aimtec;
    using Helpers;

    public static class SpellDatabase
    {
        public static List<SpellData> Spells = new List<SpellData>();

        static SpellDatabase()
        {
            #region AllChampions

            Spells.Add(new SpellData
            {
                CharName = "AllChampions",
                Dangerlevel = 1,
                MissileName = "summonersnowball",
                Name = "Mark",
                ProjectileSpeed = 1300,
                Radius = 60,
                Range = 1600,
                SpellDelay = 0,
                SpellKey = SpellSlot.Q,
                SpellName = "summonersnowball",
                ExtraSpellNames = new[] { "summonerporothrow" },
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions }
            });

            #endregion AllChampions

            #region Aatrox

            Spells.Add(new SpellData
            {
                CharName = "Aatrox",
                Dangerlevel = 3,
                MissileName = "AatroxQ",
                Name = "Dark Flight",
                Radius = 285,
                Range = 650,
                SpellDelay = 650,
                SpellKey = SpellSlot.Q,
                SpellName = "AatroxQ",
                SpellType = SpellType.Circular
            });

            Spells.Add(new SpellData
            {
                CharName = "Aatrox",
                Dangerlevel = 2,
                Name = "Blade of Torment [Beta]",
                ProjectileSpeed = 1200,
                Radius = 40,
                Range = 1075,
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                SpellName = "AatroxEConeMissile",
                SpellType = SpellType.Line
            });

            #endregion Aatrox

            #region Ahri

            Spells.Add(new SpellData
            {
                CharName = "Ahri",
                Dangerlevel = 2,
                MissileName = "AhriOrbMissile",
                Name = "Orb of Deception",
                ProjectileSpeed = 1750,
                Radius = 100,
                Range = 925,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "AhriOrbofDeception",
                SpellType = SpellType.Line
            });

            Spells.Add(new SpellData
            {
                CharName = "Ahri",
                Dangerlevel = 3,
                MissileName = "AhriSeduceMissile",
                Name = "Charm",
                ProjectileSpeed = 1550,
                Radius = 60,
                Range = 1000,
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                SpellName = "AhriSeduce",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions }
            });

            Spells.Add(new SpellData
            {
                CharName = "Ahri",
                Dangerlevel = 3,
                Name = "Orb of Deception Back",
                MissileName = "AhriOrbReturn",
                ProjectileSpeed = 915,
                Radius = 100,
                Range = 925,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "AhriOrbofDeception2",
                SpellType = SpellType.Line,
                IsSpecial = true
            });

            #endregion Ahri

            #region Alistar

            Spells.Add(new SpellData
            {
                CharName = "Alistar",
                DefaultOff = true,
                Dangerlevel = 3,
                Name = "Pulverize",
                Radius = 365,
                Range = 365,
                SpellKey = SpellSlot.Q,
                SpellName = "Pulverize",
                SpellType = SpellType.Circular
            });

            #endregion Alistar

            #region Amumu

            Spells.Add(new SpellData
            {
                CharName = "Amumu",
                Dangerlevel = 4,
                MissileName = "",
                Name = "Curse of the Sad Mummy",
                Radius = 560,
                Range = 560,
                SpellDelay = 250,
                SpellKey = SpellSlot.R,
                SpellName = "CurseoftheSadMummy",
                SpellType = SpellType.Circular
            });

            Spells.Add(new SpellData
            {
                CharName = "Amumu",
                Dangerlevel = 3,
                MissileName = "SadMummyBandageToss",
                Name = "Bandage Toss",
                ProjectileSpeed = 2000,
                Radius = 80,
                Range = 1100,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "BandageToss",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions }
            });

            #endregion Amumu

            #region Anivia

            Spells.Add(new SpellData
            {
                CharName = "Anivia",
                Dangerlevel = 3,
                MissileName = "FlashFrostSpell",
                Name = "Flash Frost",
                ProjectileSpeed = 850,
                Radius = 110,
                Range = 1250,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "FlashFrostSpell",
                SpellType = SpellType.Line
            });

            #endregion Anivia

            #region Annie

            Spells.Add(new SpellData
            {
                Angle = 25,
                CharName = "Annie",
                Dangerlevel = 2,
                Name = "Incinerate",
                Radius = 80,
                Range = 625,
                SpellDelay = 250,
                SpellKey = SpellSlot.W,
                SpellName = "Incinerate",
                SpellType = SpellType.Cone
            });

            Spells.Add(new SpellData
            {
                CharName = "Annie",
                Dangerlevel = 4,
                Name = "Summom: Tibbers",
                Radius = 290,
                Range = 600,
                SpellDelay = 250,
                SpellKey = SpellSlot.R,
                SpellName = "InfernalGuardian",
                SpellType = SpellType.Circular
            });

            #endregion Annie

            #region Ashe

            Spells.Add(new SpellData
            {
                CharName = "Ashe",
                Dangerlevel = 3,
                MissileName = "EnchantedCrystalArrow",
                Name = "Enchanted Crystal Arrow",
                ProjectileSpeed = 1600,
                Radius = 130,
                Range = 25000,
                SpellDelay = 250,
                SpellKey = SpellSlot.R,
                SpellName = "EnchantedCrystalArrow",
                SpellType = SpellType.Line
                //collisionObjects = new[] { CollisionObjectType.EnemyChampions },
            });

            Spells.Add(new SpellData
            {
                Angle = 5,
                CharName = "Ashe",
                Dangerlevel = 1,
                //missileName = "VolleyAttack",
                Name = "Volley",
                ProjectileSpeed = 1500,
                Radius = 20,
                Range = 1350,
                SpellDelay = 250,
                SpellKey = SpellSlot.W,
                SpellName = "Volley",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions },
                IsSpecial = true
            });

            #endregion Ashe

            #region Aurelion Sol

            Spells.Add(new SpellData
            {
                CharName = "AurelionSol",
                Dangerlevel = 2,
                MissileName = "AurelionSolQMissile",
                Name = "Starsurge",
                ProjectileSpeed = 850,
                Radius = 180,
                Range = 1500,
                FixedRange = true,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "AurelionSolQ",
                SpellType = SpellType.Line
            });

            Spells.Add(new SpellData
            {
                CharName = "AurelionSol",
                Dangerlevel = 4,
                MissileName = "AurelionSolRBeamMissile",
                Name = "Voice of Light",
                ProjectileSpeed = 4600,
                Radius = 120,
                Range = 1420,
                FixedRange = true,
                SpellDelay = 300,
                SpellKey = SpellSlot.R,
                SpellName = "AurelionSolR",
                SpellType = SpellType.Line
            });

            #endregion Aurelion Sol

            #region Azir

            Spells.Add(new SpellData
            {
                CharName = "Azir",
                Dangerlevel = 2,
                Name = "Conquering Sands",
                ProjectileSpeed = 1600,
                Radius = 80,
                Range = 1150, // estimate radius can q
                SpellDelay = 0,
                SpellKey = SpellSlot.Q,
                SpellName = "AzirQWrapper",
                SpellType = SpellType.Line,
                IsSpecial = true,
                NoProcess = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Azir",
                Dangerlevel = 3,
                Name = "Emperor's Divide",
                MissileName = "AzirSoldierRMissile",
                ProjectileSpeed = 1400,
                Radius = 450,
                Range = 700,
                SpellDelay = 250,
                SpellKey = SpellSlot.R,
                SpellName = "AzirR",
                SpellType = SpellType.Line
            });

            #endregion Azir

            #region Bard

            Spells.Add(new SpellData
            {
                CharName = "Bard",
                Dangerlevel = 3,
                MissileName = "BardQMissile",
                Name = "Cosmic Binding",
                ProjectileSpeed = 1600,
                Radius = 60,
                Range = 950,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "BardQ",
                SpellType = SpellType.Line
            });

            Spells.Add(new SpellData
            {
                CharName = "Bard",
                Dangerlevel = 2,
                Name = "Tempered Fate",
                MissileName = "BardR",
                ProjectileSpeed = 2100,
                Radius = 350,
                Range = 3400,
                SpellDelay = 250,
                SpellKey = SpellSlot.R,
                SpellName = "BardR",
                SpellType = SpellType.Circular
            });

            #endregion Bard

            #region Blitzcrank

            Spells.Add(new SpellData
            {
                CharName = "Blitzcrank",
                Dangerlevel = 3,
                ExtraDelay = 75,
                MissileName = "RocketGrabMissile",
                Name = "Rocket Grab",
                ProjectileSpeed = 1800,
                Radius = 70,
                Range = 1050,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "RocketGrab",
                ExtraSpellNames = new[] { "RocketGrabMissile" },
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions },
                FixedRange = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Blitzcrank",
                Dangerlevel = 2,
                Name = "StaticField",
                Radius = 600,
                Range = 600,
                SpellDelay = 250,
                SpellKey = SpellSlot.R,
                SpellName = "StaticField",
                SpellType = SpellType.Circular
            });

            #endregion Blitzcrank

            #region Brand

            Spells.Add(new SpellData
            {
                CharName = "Brand",
                Dangerlevel = 3,
                MissileName = "BrandQMissile",
                Name = "Sear",
                ProjectileSpeed = 1600,
                Radius = 60,
                Range = 1100,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "BrandQ",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions }
            });

            Spells.Add(new SpellData
            {
                CharName = "Brand",
                Dangerlevel = 2,
                Name = "Pillar of Flame",
                Radius = 250,
                Range = 1100,
                SpellDelay = 850,
                SpellKey = SpellSlot.W,
                SpellName = "BrandW",
                SpellType = SpellType.Circular
            });

            #endregion Brand

            #region Braum

            Spells.Add(new SpellData
            {
                CharName = "Braum",
                Dangerlevel = 4,
                MissileName = "braumrmissile",
                Name = "Glacial Fissure",
                ProjectileSpeed = 1125,
                Radius = 115,
                Range = 1250,
                SpellDelay = 500,
                SpellKey = SpellSlot.R,
                SpellName = "BraumRWrapper",
                SpellType = SpellType.Line
            });

            Spells.Add(new SpellData
            {
                CharName = "Braum",
                Dangerlevel = 3,
                MissileName = "BraumQMissile",
                Name = "Winter's Bite",
                ProjectileSpeed = 1200,
                SpellDelay = 250,
                Radius = 100,
                Range = 1000,
                SpellKey = SpellSlot.Q,
                SpellName = "BraumQ",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions }
            });

            #endregion Braum

            #region Caitlyn

            Spells.Add(new SpellData
            {
                CharName = "Caitlyn",
                Dangerlevel = 2,
                Name = "Piltover Peacemaker",
                ProjectileSpeed = 2200,
                Radius = 90,
                Range = 1300,
                SpellDelay = 625,
                SpellKey = SpellSlot.Q,
                SpellName = "CaitlynPiltoverPeacemaker",
                SpellType = SpellType.Line
            });

            Spells.Add(new SpellData
            {
                CharName = "Caitlyn",
                Dangerlevel = 3,
                Name = "Yordle Trap",
                Radius = 75,
                Range = 800,
                SpellKey = SpellSlot.W,
                SpellName = "CaitlynYordleTrap",
                TrapBaseName = "CaitlynTrap",
                SpellType = SpellType.Circular,
                HasTrap = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Caitlyn",
                Dangerlevel = 3,
                MissileName = "CaitlynEntrapmentMissile",
                Name = "90 Caliber Net",
                ProjectileSpeed = 2000,
                Radius = 80,
                Range = 950,
                SpellDelay = 125,
                SpellKey = SpellSlot.E,
                SpellName = "CaitlynEntrapment",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions }
            });

            #endregion Caitlyn

            #region Cassiopeia

            Spells.Add(new SpellData
            {
                Angle = 40,
                CharName = "Cassiopeia",
                Dangerlevel = 4,
                Name = "Petrifying Gaze",
                Radius = 145,
                Range = 825,
                SpellDelay = 500,
                SpellKey = SpellSlot.R,
                SpellName = "CassiopeiaR",
                SpellType = SpellType.Cone
            });

            //Spells.Add(
            //new SpellData
            //{
            //    charName = "Cassiopeia",
            //    dangerlevel = 1,
            //    name = "Miasama",
            //    //missileName = "CassiopeiaWMissile",
            //    //projectileSpeed = 2800,
            //    radius = 200,
            //    range = 900,
            //    spellDelay = 250,
            //    extraEndTime = 500,
            //    spellKey = SpellSlot.W,
            //    spellName = "CassiopeiaW",
            //    spellType = SpellType.Circular,
            //    trapTroyName = "cassiopeia_base_w_wcircle_tar_" + Situation.EmitterTeam() + ".troy",
            //    //updatePosition = false,
            //    hasTrap = true
            //});

            Spells.Add(new SpellData
            {
                CharName = "Cassiopeia",
                Dangerlevel = 1,
                MissileName = "CassiopeiaQ",
                Name = "Noxious Blast",
                Radius = 200,
                Range = 850,
                SpellDelay = 750,
                SpellKey = SpellSlot.Q,
                SpellName = "CassiopeiaQ",
                SpellType = SpellType.Circular
            });

            #endregion Cassiopeia

            #region Chogath

            Spells.Add(new SpellData
            {
                Angle = 30,
                CharName = "Chogath",
                Dangerlevel = 2,
                MissileName = "FeralScream",
                Name = "Feral Scream",
                Radius = 80,
                Range = 650,
                SpellDelay = 250,
                SpellKey = SpellSlot.W,
                SpellName = "FeralScream",
                SpellType = SpellType.Cone
            });

            Spells.Add(new SpellData
            {
                CharName = "Chogath",
                Dangerlevel = 3,
                MissileName = "Rupture",
                Name = "Rupture",
                Radius = 250,
                Range = 950,
                SpellDelay = 1200,
                SpellKey = SpellSlot.Q,
                SpellName = "Rupture",
                SpellType = SpellType.Circular,
                ExtraDrawHeight = 45
            });

            #endregion Chogath

            #region Corki

            Spells.Add(new SpellData
            {
                CharName = "Corki",
                Dangerlevel = 3,
                MissileName = "MissileBarrageMissile2",
                Name = "Missile Barrage Big",
                ProjectileSpeed = 2000,
                Radius = 40,
                Range = 1500,
                SpellDelay = 175,
                SpellKey = SpellSlot.R,
                SpellName = "MissileBarrage2",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions }
            });

            Spells.Add(new SpellData
            {
                CharName = "Corki",
                Dangerlevel = 2,
                MissileName = "PhosphorusBombMissile",
                Name = "Phosphorus Bomb",
                ProjectileSpeed = 1125,
                Radius = 270,
                Range = 825,
                SpellDelay = 500,
                SpellKey = SpellSlot.Q,
                SpellName = "PhosphorusBomb",
                SpellType = SpellType.Circular,
                ExtraDrawHeight = 110
            });

            Spells.Add(new SpellData
            {
                CharName = "Corki",
                Dangerlevel = 2,
                MissileName = "MissileBarrageMissile",
                Name = "Missile Barrage",
                ProjectileSpeed = 2000,
                Radius = 40,
                Range = 1300,
                SpellDelay = 175,
                SpellKey = SpellSlot.R,
                SpellName = "MissileBarrage",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions }
            });

            #endregion Corki

            #region Darius

            Spells.Add(new SpellData
            {
                CharName = "Darius",
                Dangerlevel = 2,
                Name = "Decimate [Beta]",
                Radius = 425,
                Range = 425,
                SpellDelay = 750,
                SpellKey = SpellSlot.Q,
                SpellName = "DariusCleave",
                SpellType = SpellType.Circular,
                DefaultOff = true
            });

            Spells.Add(new SpellData
            {
                Angle = 25,
                CharName = "Darius",
                Dangerlevel = 3,
                MissileName = "DariusAxeGrabCone",
                Name = "Axe Cone Grab",
                Radius = 55,
                Range = 570,
                SpellDelay = 320,
                SpellKey = SpellSlot.E,
                SpellName = "DariusAxeGrabCone",
                SpellType = SpellType.Cone
            });

            #endregion Darius

            #region Diana

            Spells.Add(new SpellData
            {
                CharName = "Diana",
                Dangerlevel = 3,
                Name = "Crescent Strike",
                ProjectileSpeed = 1400,
                Radius = 50,
                Range = 850,
                FixedRange = true,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "DianaArc",
                SpellType = SpellType.Arc,
                HasEndExplosion = true,
                SecondaryRadius = 195,
                ExtraEndTime = 250
            });

            #endregion Diana

            #region DrMundo

            Spells.Add(new SpellData
            {
                CharName = "DrMundo",
                Dangerlevel = 2,
                MissileName = "InfectedCleaverMissile",
                Name = "Infected Cleaver",
                ProjectileSpeed = 2000,
                Radius = 60,
                Range = 1050,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "InfectedCleaverMissileCast",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions }
            });

            #endregion DrMundo

            #region Draven

            Spells.Add(new SpellData
            {
                CharName = "Draven",
                Dangerlevel = 2,
                MissileName = "DravenR",
                Name = "Whirling Death",
                ProjectileSpeed = 2000,
                Radius = 160,
                Range = 25000,
                SpellDelay = 500,
                SpellKey = SpellSlot.R,
                SpellName = "DravenRCast",
                SpellType = SpellType.Line
            });

            Spells.Add(new SpellData
            {
                CharName = "Draven",
                Dangerlevel = 3,
                MissileName = "DravenDoubleShotMissile",
                Name = "Stand Aside",
                ProjectileSpeed = 1400,
                Radius = 130,
                Range = 1100,
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                SpellName = "DravenDoubleShot",
                SpellType = SpellType.Line
            });

            #endregion Draven

            #region Ekko

            Spells.Add(new SpellData
            {
                CharName = "Ekko",
                Dangerlevel = 3,
                MissileName = "ekkoqmis",
                Name = "Timewinder",
                ProjectileSpeed = 1650,
                Radius = 60,
                Range = 950,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "EkkoQ",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions }
            });

            Spells.Add(new SpellData
            {
                CharName = "Ekko",
                Dangerlevel = 3,
                MissileName = "ekkoqreturn", // todo: add special spell
                Name = "Timewinder (Return)",
                ProjectileSpeed = 2300,
                Radius = 100,
                Range = 1250,
                SpellDelay = 0,
                SpellKey = SpellSlot.Q,
                SpellName = "ekkoqreturn",
                SpellType = SpellType.Line
            });

            Spells.Add(new SpellData
            {
                CharName = "Ekko",
                Dangerlevel = 2,
                Name = "Parallel Convergence",
                ProjectileSpeed = 1650,
                Radius = 375,
                Range = 1750,
                SpellDelay = 3750,
                SpellKey = SpellSlot.W,
                SpellName = "ekkow",
                SpellType = SpellType.Circular
            });

            Spells.Add(new SpellData
            {
                CharName = "Ekko",
                Dangerlevel = 4,
                MissileName = "EkkoR",
                Name = "Chronobreak",
                ProjectileSpeed = 1650,
                Radius = 375,
                Range = 1600,
                SpellDelay = 250,
                SpellKey = SpellSlot.R,
                SpellName = "EkkoR",
                SpellType = SpellType.Circular,
                IsSpecial = true
            });

            #endregion Ekko

            #region Elise

            Spells.Add(new SpellData
            {
                CharName = "Elise",
                Dangerlevel = 3,
                MissileName = "EliseHumanE",
                Name = "Cocoon",
                ProjectileSpeed = 1600,
                Radius = 55,
                Range = 1100,
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                SpellName = "EliseHumanE",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions }
            });

            #endregion Elise

            #region Evelynn

            Spells.Add(new SpellData
            {
                CharName = "Evelynn",
                Dangerlevel = 4,
                Name = "Agony's Embrace",
                Radius = 350,
                Range = 650,
                SpellDelay = 250,
                SpellKey = SpellSlot.R,
                SpellName = "EvelynnR",
                SpellType = SpellType.Circular
            });

            #endregion Evelynn

            #region Ezreal

            Spells.Add(new SpellData
            {
                CharName = "Ezreal",
                Dangerlevel = 2,
                MissileName = "EzrealMysticShotMissile",
                Name = "Mystic Shot",
                ProjectileSpeed = 2000,
                Radius = 60,
                Range = 1200,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "EzrealMysticShot",
                ExtraSpellNames = new[] { "ezrealmysticshotwrapper" },
                ExtraMissileNames = new[] { "EzrealMysticShotPulseMissile" },
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions }
            });

            Spells.Add(new SpellData
            {
                CharName = "Ezreal",
                Dangerlevel = 2,
                MissileName = "EzrealTrueshotBarrage",
                Name = "Trueshot Barrage",
                ProjectileSpeed = 2000,
                Radius = 160,
                Range = 25000,
                SpellDelay = 1000,
                SpellKey = SpellSlot.R,
                SpellName = "EzrealTrueshotBarrage",
                SpellType = SpellType.Line
            });

            Spells.Add(new SpellData
            {
                CharName = "Ezreal",
                Dangerlevel = 1,
                MissileName = "EzrealEssenceFluxMissile",
                Name = "Essence Flux",
                ProjectileSpeed = 1600,
                Radius = 80,
                Range = 1050,
                SpellDelay = 250,
                SpellKey = SpellSlot.W,
                SpellName = "EzrealEssenceFlux",
                SpellType = SpellType.Line
            });

            #endregion Ezreal

            #region Fiora

            Spells.Add(new SpellData
            {
                CharName = "Fiora",
                Dangerlevel = 2,
                Name = "Riposte",
                ProjectileSpeed = 3200,
                Radius = 70,
                Range = 800,
                SpellDelay = 500,
                SpellKey = SpellSlot.W,
                SpellName = "FioraW",
                SpellType = SpellType.Line
            });

            #endregion Fiora

            #region Fizz

            Spells.Add(new SpellData
            {
                CharName = "Fizz",
                Dangerlevel = 2,
                Name = "Urchin Strike",
                ProjectileSpeed = 1400,
                Radius = 150,
                Range = 550,
                SpellDelay = 0,
                SpellKey = SpellSlot.Q,
                SpellName = "FizzQ",
                SpellType = SpellType.Line,
                IsSpecial = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Fizz",
                Dangerlevel = 4,
                MissileName = "FizzRMissile",
                Name = "Chum the Waters",
                ProjectileSpeed = 1350,
                Radius = 120,
                Range = 1300,
                SpellDelay = 250,
                SpellKey = SpellSlot.R,
                SpellName = "FizzR",
                SpellType = SpellType.Line,
                HasEndExplosion = true,
                UseEndPosition = true,
                IsSpecial = true,

                // this will be the radius if it come from FoW, 
                // if vissible will be updated accordingly
                SecondaryRadius = 300
            });

            #endregion Fizz

            #region Galio

            Spells.Add(new SpellData
            {
                CharName = "Galio",
                Dangerlevel = 2,
                MissileName = "GalioEMissile",
                Name = "Righteous Gust",
                ProjectileSpeed = 1400,
                Radius = 130,
                Range = 700,
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                SpellName = "GalioE",
                SpellType = SpellType.Line
            });

            Spells.Add(new SpellData
            {
                CharName = "Galio",
                Dangerlevel = 2,
                MissileName = "GalioQSuper",
                Name = "Resolute Smite",
                ProjectileSpeed = 1300,
                Radius = 200,
                Range = 825,
                SpellDelay = 250,
                ExtraEndTime = 3300,
                SpellKey = SpellSlot.Q,
                SpellName = "GalioQ",
                SpellType = SpellType.Circular
            });

            Spells.Add(new SpellData
            {
                CharName = "Galio",
                Dangerlevel = 4,
                Name = "Idol Of Durand",
                Radius = 575,
                ProjectileSpeed = 1000,
                Range = 4000,
                SpellDelay = 1000,
                SpellKey = SpellSlot.R,
                SpellName = "GalioR",
                SpellType = SpellType.Circular
            });

            #endregion Galio

            #region Gnar

            Spells.Add(new SpellData
            {
                CharName = "Gnar",
                Dangerlevel = 2,
                MissileName = "gnarbigqmissile",
                Name = "Boulder Toss",
                ProjectileSpeed = 2000,
                Radius = 90,
                Range = 1150,
                SpellDelay = 500,
                SpellKey = SpellSlot.Q,
                SpellName = "gnarbigq",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions }
            });

            Spells.Add(new SpellData
            {
                CharName = "Gnar",
                Dangerlevel = 4,
                ProjectileSpeed = 0,
                Name = "GNAR!",
                Radius = 500,
                Range = 500,
                SpellDelay = 250,
                SpellKey = SpellSlot.R,
                SpellName = "GnarR",
                SpellType = SpellType.Circular
            });

            Spells.Add(new SpellData
            {
                CharName = "Gnar",
                Dangerlevel = 3,
                Name = "Wallop",
                Radius = 100,
                Range = 525,
                SpellDelay = 600,
                SpellKey = SpellSlot.W,
                SpellName = "gnarbigw",
                SpellType = SpellType.Line
            });

            Spells.Add(new SpellData
            {
                CharName = "Gnar",
                Dangerlevel = 2,
                Name = "Boomerang Throw",
                MissileName = "GnarQMissile",
                ExtraMissileNames = new[] { "GnarQMissileReturn" }, // todo: special spell
                ProjectileSpeed = 2400,
                Radius = 60,
                Range = 1125,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "GnarQ",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions }
            });

            Spells.Add(new SpellData
            {
                CharName = "Gnar",
                Dangerlevel = 2,
                MissileName = "GnarE",
                Name = "Hop",
                ProjectileSpeed = 880,
                Radius = 150,
                Range = 475,
                SpellDelay = 0,
                SpellKey = SpellSlot.E,
                SpellName = "GnarE",
                SpellType = SpellType.Circular
            });

            Spells.Add(new SpellData
            {
                CharName = "Gnar",
                Dangerlevel = 2,
                MissileName = "gnarbige",
                Name = "Crunch",
                ProjectileSpeed = 800,
                Radius = 350,
                Range = 475,
                SpellDelay = 0,
                SpellKey = SpellSlot.E,
                SpellName = "gnarbige",
                SpellType = SpellType.Circular
            });

            #endregion Gnar

            #region Gragas

            Spells.Add(new SpellData
            {
                CharName = "Gragas",
                Dangerlevel = 2,
                MissileName = "GragasQMissile",
                Name = "Barrel Roll",
                ProjectileSpeed = 1000,
                Radius = 260,
                Range = 975,
                SpellDelay = 500,
                SpellKey = SpellSlot.Q,
                SpellName = "GragasQ",
                SpellType = SpellType.Circular,
                ExtraDrawHeight = 50
            });

            Spells.Add(new SpellData
            {
                CharName = "Gragas",
                Dangerlevel = 2,
                Name = "Barrel Roll",
                Radius = 270,
                Range = 975,
                SpellKey = SpellSlot.Q,
                SpellName = "GragasQ",
                SpellType = SpellType.Circular,
                ExtraDrawHeight = 45,
                TrapTroyName = "gragas_base_q_" + Situation.EmitterTeam() + ".troy",
                HasTrap = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Gragas",
                Dangerlevel = 3,
                MissileName = "GragasE",
                Name = "Body Slam",
                ProjectileSpeed = 1200,
                Radius = 200,
                Range = 950,
                SpellDelay = 0,
                SpellKey = SpellSlot.E,
                SpellName = "GragasE",
                SpellType = SpellType.Line
            });

            Spells.Add(new SpellData
            {
                CharName = "Gragas",
                Dangerlevel = 4,
                MissileName = "GragasR",
                Name = "Explosive Cask",
                ProjectileSpeed = 1750,
                Radius = 350,
                Range = 1050,
                SpellDelay = 250,
                SpellKey = SpellSlot.R,
                SpellName = "GragasR",
                SpellType = SpellType.Circular
            });

            #endregion Gragas

            #region Graves

            Spells.Add(new SpellData
            {
                CharName = "Graves",
                Dangerlevel = 3,
                MissileName = "GravesQLineMis",
                Name = "End of the Line",
                ProjectileSpeed = 3000,
                Radius = 72,
                Range = 800,
                SpellDelay = 250,
                ExtraEndTime = 1300,
                SpellKey = SpellSlot.Q,
                SpellName = "GravesQLineSpell",
                SpellType = SpellType.Line,
                FixedRange = true,
                IsSpecial = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Graves",
                Dangerlevel = 2,
                MissileName = "GravesQReturn",
                Name = "End of the Line (Return)",
                ProjectileSpeed = 1600,
                Radius = 100,
                Range = 900,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "GravesQLineSpell",
                SpellType = SpellType.Line
            });

            Spells.Add(new SpellData
            {
                CharName = "Graves",
                Dangerlevel = 1,
                DefaultOff = true,
                MissileName = "GravesSmokeGrenadeBoom",
                Name = "Smoke Screen",
                ProjectileSpeed = 1500,
                Radius = 250,
                Range = 900,
                SpellDelay = 250,
                SpellKey = SpellSlot.W,
                SpellName = "GravesSmokeGrenade",
                SpellType = SpellType.Circular
            });

            Spells.Add(new SpellData
            {
                CharName = "Graves",
                Dangerlevel = 4,
                MissileName = "GravesChargeShot",
                Name = "Collateral Damage",
                ProjectileSpeed = 2100,
                Radius = 100,
                Range = 1100,
                SpellDelay = 250,
                SpellKey = SpellSlot.R,
                SpellName = "GravesChargeShot",
                SpellType = SpellType.Line,
                FixedRange = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Graves",
                Dangerlevel = 4,
                MissileName = "GravesChargeShotFxMissile",
                Name = "Collateral Damage (Explosion)",
                ProjectileSpeed = 2115,
                Radius = 150,
                Range = 110,
                SpellDelay = 250,
                SpellKey = SpellSlot.R,
                SpellName = "GravesChargeShotFxMissile",
                ExtraMissileNames = new[] { "GravesChargeShotFxMissile2" },
                SpellType = SpellType.Line,
                FixedRange = true
            });

            #endregion Graves

            #region Hecarim

            Spells.Add(new SpellData
            {
                CharName = "Hecarim",
                Dangerlevel = 4,
                Name = "Onslaught of Shadows [Beta]",
                ProjectileSpeed = 1100,
                Radius = 60,
                Range = 1525,
                SpellDelay = 0,
                SpellKey = SpellSlot.R,
                SpellName = "HecarimUltMissile",
                ExtraMissileNames = new[] { "HecarimUltMissileSkn4R1", "HecarimUltMissileSkn4c", "HecarimUltMissileSkn4R2", "HecarimUltMissileSKn4L2", "HecarimUltMissileSkn4L1" },
                SpellType = SpellType.Line,
                UsePackets = true,
                FixedRange = true
            });

            #endregion Hecarim

            #region Heimerdinger

            Spells.Add(new SpellData
            {
                CharName = "Heimerdinger",
                Dangerlevel = 2,
                MissileName = "HeimerdingerW",
                ExtraMissileNames = new[] { "HeimerdingerWAttack2Ult" },
                Name = "Hextech Micro-Rockets",
                ProjectileSpeed = 1800,
                Radius = 70,
                Range = 1350,
                SpellDelay = 250,
                SpellKey = SpellSlot.W,
                SpellName = "HeimerdingerW",
                SpellType = SpellType.Line
            });

            Spells.Add(new SpellData
            {
                CharName = "Heimerdinger",
                Dangerlevel = 3,
                MissileName = "HeimerdingerESpell",
                Name = "CH-2 Electron Storm Grenade",
                ProjectileSpeed = 1200,
                Radius = 150,
                Range = 925,
                SpellDelay = 325,
                SpellKey = SpellSlot.E,
                SpellName = "HeimerdingerE",
                ExtraMissileNames = new[] { "heimerdingerespell_ult", "heimerdingerespell_ult2", "heimerdingerespell_ult3" },
                SpellType = SpellType.Circular
            });

            Spells.Add(new SpellData
            {
                CharName = "Heimerdinger",
                Dangerlevel = 2,
                MissileName = "HeimerdingerTurretEnergyBlast",
                Name = "Turret Energy Blast",
                ProjectileSpeed = 1650,
                Radius = 50,
                Range = 1000,
                SpellDelay = 435,
                SpellKey = SpellSlot.Q,
                SpellName = "HeimerdingerTurretEnergyBlast",
                SpellType = SpellType.Line,
                IsSpecial = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Heimerdinger",
                Dangerlevel = 3,
                MissileName = "HeimerdingerTurretBigEnergyBlast",
                Name = "Big Turret Energy Blast",
                ProjectileSpeed = 1800,
                Radius = 75,
                Range = 1000,
                SpellDelay = 350,
                SpellKey = SpellSlot.Q,
                SpellName = "HeimerdingerTurretBigEnergyBlast",
                SpellType = SpellType.Line
            });

            #endregion Heimerdinger

            #region Illaoi

            Spells.Add(new SpellData
            {
                CharName = "Illaoi",
                Dangerlevel = 3,
                MissileName = "IllaoiQ",
                Name = "Tentacle Smash",
                Radius = 100,
                Range = 850,
                SpellDelay = 750,
                SpellKey = SpellSlot.Q,
                SpellName = "IllaoiQ",
                SpellType = SpellType.Line,
                FixedRange = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Illaoi",
                Dangerlevel = 3,
                MissileName = "Illaoiemis",
                Name = "Test of Spirit",
                ProjectileSpeed = 1900,
                Radius = 50,
                Range = 950,
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                SpellName = "IllaoiE",
                SpellType = SpellType.Line,
                FixedRange = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Illaoi",
                Dangerlevel = 3,
                Name = "Leap of Faith",
                Range = 500,
                Radius = 450,
                SpellDelay = 500,
                SpellKey = SpellSlot.R,
                SpellName = "IllaoiR",
                SpellType = SpellType.Circular
            });

            #endregion Illaoi

            #region Irelia

            Spells.Add(new SpellData
            {
                CharName = "Irelia",
                Dangerlevel = 2,
                MissileName = "ireliatranscendentbladesspell",
                Name = "Transcendent Blades",
                ProjectileSpeed = 1600,
                Radius = 120,
                Range = 1200,
                SpellDelay = 0,
                SpellKey = SpellSlot.R,
                SpellName = "IreliaTranscendentBlades",
                SpellType = SpellType.Line,
                UsePackets = true
            });

            #endregion Irelia

            #region Ivern

            Spells.Add(new SpellData
            {
                CharName = "Ivern",
                Dangerlevel = 3,
                Name = "Rootcaller",
                ProjectileSpeed = 1300,
                Radius = 65,
                Range = 1150,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "IvernQ",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyMinions, CollisionObjectType.EnemyChampions }
            });

            #endregion Ivern

            #region Janna

            Spells.Add(new SpellData
            {
                CharName = "Janna",
                Dangerlevel = 2,
                MissileName = "HowlingGaleSpell",
                Name = "Howling Gale",
                ProjectileSpeed = 1100,
                Radius = 120,
                Range = 1700,
                SpellDelay = 0,
                SpellKey = SpellSlot.Q,
                SpellName = "HowlingGale",
                SpellType = SpellType.Line,
                UsePackets = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Janna",
                Dangerlevel = 3,
                MissileName = "HowlingGaleSpell",
                Name = "Howling Gale",
                ProjectileSpeed = 1100,
                Radius = 120,
                Range = 1700,
                SpellDelay = 0,
                SpellKey = SpellSlot.Q,
                SpellName = "HowlingGaleSpell",
                SpellType = SpellType.Line,
                UsePackets = true
            });

            #endregion Janna

            #region JarvanIV

            Spells.Add(new SpellData
            {
                CharName = "JarvanIV",
                Dangerlevel = 2,
                MissileName = "JarvanIVDragonStrike",
                Name = "Dragon Strike",
                ProjectileSpeed = 2000,
                Radius = 80,
                Range = 845,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "JarvanIVDragonStrike",
                SpellType = SpellType.Line
            });

            Spells.Add(new SpellData
            {
                CharName = "JarvanIV",
                Dangerlevel = 3,
                Name = "Dragon Strike EQ",
                ProjectileSpeed = 1800,
                Radius = 120,
                Range = 845,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "JarvanIVDragonStrike2",
                SpellType = SpellType.Line,
                UseEndPosition = true
            });

            Spells.Add(new SpellData
            {
                CharName = "JarvanIV",
                Dangerlevel = 1,
                Name = "Demacian Standard",
                Radius = 175,
                Range = 800,
                SpellDelay = 500,
                SpellKey = SpellSlot.E,
                SpellName = "JarvanIVDemacianStandard",
                SpellType = SpellType.Circular
            });

            Spells.Add(new SpellData
            {
                CharName = "JarvanIV",
                Dangerlevel = 3,
                Name = "Cataclysm",
                ProjectileSpeed = 1900,
                Radius = 350,
                Range = 825,
                SpellDelay = 0,
                SpellKey = SpellSlot.R,
                SpellName = "JarvanIVCataclysm",
                SpellType = SpellType.Circular,
                DefaultOff = true
            });

            #endregion JarvanIV

            #region Jayce

            Spells.Add(new SpellData
            {
                CharName = "Jayce",
                Dangerlevel = 2,
                Name = "Shock Blast",
                ProjectileSpeed = 1450,
                Radius = 70,
                Range = 1170,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                HasEndExplosion = true,
                SpellName = "jayceshockblastmis",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions },
                SecondaryRadius = 210
            });

            Spells.Add(new SpellData
            {
                CharName = "Jayce",
                Dangerlevel = 3,
                Name = "Shock Blast Fast",
                ProjectileSpeed = 2350,
                Radius = 70,
                Range = 1600,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                HasEndExplosion = true,
                SpellName = "jayceshockblastwallmis",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions },
                SecondaryRadius = 210,
                FixedRange = true
            });

            #endregion Jayce

            #region Jinx

            Spells.Add(new SpellData
            {
                CharName = "Jinx",
                Dangerlevel = 3,
                Name = "Super Mega Death Rocket!",
                ProjectileSpeed = 1700,
                Radius = 140,
                Range = 25000,
                SpellDelay = 600,
                SpellKey = SpellSlot.R,
                SpellName = "JinxR",
                ExtraMissileNames = new[] { "JinxRWrapper" },
                SpellType = SpellType.Line,
                FixedRange = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Jinx",
                Dangerlevel = 3,
                MissileName = "JinxWMissile",
                Name = "Zap!",
                ProjectileSpeed = 3300,
                Radius = 60,
                Range = 1500,
                SpellDelay = 600,
                SpellKey = SpellSlot.W,
                SpellName = "JinxWMissile",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions },
                FixedRange = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Jinx",
                Dangerlevel = 3,
                Name = "Flame Chompers!",
                //missileName = "JinxEHit",
                //projectileSpeed = 2300,
                Radius = 120,
                Range = 900,
                SpellDelay = 1200,
                SpellKey = SpellSlot.E,
                SpellName = "JinxE",
                SpellType = SpellType.Circular,
                HasTrap = true,
                TrapBaseName = "jinxmine",
                UpdatePosition = false
            });

            #endregion Jinx

            #region Jhin   

            Spells.Add(new SpellData
            {
                CharName = "Jhin",
                Dangerlevel = 3,
                //missileName = "JhinWMissile", there is no missile
                Name = "Deadly Flourish",
                Radius = 40,
                Range = 2550,
                SpellDelay = 650,
                SpellKey = SpellSlot.W,
                SpellName = "JhinW",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions },
                FixedRange = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Jhin",
                Dangerlevel = 2,
                MissileName = "JhinRShotMis",
                Name = "Curtain Call",
                ProjectileSpeed = 5000,
                Radius = 80,
                Range = 3500,
                SpellDelay = 250,
                SpellKey = SpellSlot.R,
                SpellName = "JhinRShot",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions },
                ExtraMissileNames = new[] { "JhinRShotMis4" },
                FixedRange = true
            });

            #endregion

            #region Kalista

            Spells.Add(new SpellData
            {
                CharName = "Kalista",
                Dangerlevel = 2,
                MissileName = "kalistamysticshotmis",
                Name = "Pierce",
                ProjectileSpeed = 1700,
                Radius = 45,
                Range = 1200,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "KalistaMysticShot",
                ExtraMissileNames = new[] { "kalistamysticshotmistrue" },
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions },
                FixedRange = true
            });

            //TODO: Add Kalista R?

            #endregion Kalista

            #region Karma

            Spells.Add(new SpellData
            {
                CharName = "Karma",
                Dangerlevel = 2,
                MissileName = "KarmaQMissile",
                Name = "Inner Flame",
                ProjectileSpeed = 1700,
                Radius = 60,
                Range = 1050,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "KarmaQ",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions },
                FixedRange = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Karma",
                Dangerlevel = 3,
                MissileName = "KarmaQMissileMantra",
                Name = "Soulflare (Mantra)",
                ProjectileSpeed = 1700,
                Radius = 80,
                Range = 1050,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "KarmaQMissileMantra",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions },
              
                SecondaryRadius = 310
            });

            #endregion Karma

            #region Karthus

            Spells.Add(new SpellData
            {
                CharName = "Karthus",
                Dangerlevel = 2,
                Name = "Lay Waste",
                Radius = 190,
                Range = 875,
                SpellDelay = 625,
                SpellKey = SpellSlot.Q,
                SpellName = "KarthusLayWasteA1",
                SpellType = SpellType.Circular,
                ExtraSpellNames = new[] { "karthuslaywastea2", "karthuslaywastea3", "karthuslaywastedeada1", "karthuslaywastedeada2", "karthuslaywastedeada3" }
            });

            #endregion Karthus

            #region Kassadin

            Spells.Add(new SpellData
            {
                CharName = "Kassadin",
                Dangerlevel = 1,
                MissileName = "RiftWalk",
                Name = "RiftWalk",
                Radius = 270,
                Range = 450,
                SpellDelay = 250,
                SpellKey = SpellSlot.R,
                SpellName = "RiftWalk",
                SpellType = SpellType.Circular
            });

            Spells.Add(new SpellData
            {
                Angle = 40,
                CharName = "Kassadin",
                Dangerlevel = 2,
                Name = "Force Pulse",
                Radius = 80,
                Range = 700,
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                SpellName = "ForcePulse",
                SpellType = SpellType.Cone
            });

            #endregion Kassadin

            #region Kennen

            Spells.Add(new SpellData
            {
                CharName = "Kennen",
                Dangerlevel = 2,
                MissileName = "KennenShurikenHurlMissile1",
                Name = "Thundering Shuriken",
                ProjectileSpeed = 1700,
                Radius = 50,
                Range = 1050,
                SpellDelay = 125,
                SpellKey = SpellSlot.Q,
                SpellName = "KennenShurikenHurlMissile1",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions },
                FixedRange = true
            });

            #endregion Kennen

            #region Khazix

            Spells.Add(new SpellData
            {
                CharName = "Khazix",
                Dangerlevel = 1,
                MissileName = "KhazixWMissile",
                Name = "Void Spike",
                ProjectileSpeed = 1700,
                Radius = 70,
                Range = 1100,
                SpellDelay = 250,
                SpellKey = SpellSlot.W,
                SpellName = "KhazixW",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions }
            });

            Spells.Add(new SpellData
            {
                Angle = 22,
                CharName = "Khazix",
                Dangerlevel = 2,
                IsThreeWay = true,
                Name = "Void Spike Evolved",
                ProjectileSpeed = 1700,
                Radius = 70,
                Range = 1025,
                SpellDelay = 250,
                SpellKey = SpellSlot.W,
                SpellName = "khazixwlong",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions }
            });

            Spells.Add(new SpellData
            {
                CharName = "Khazix",
                Dangerlevel = 1,
                MissileName = "khazixe",
                Name = "Leap",
                ProjectileSpeed = 1200,
                Radius = 300,
                Range = 700,
                SpellDelay = 0,
                SpellKey = SpellSlot.E,
                SpellName = "KhazixE",
                SpellType = SpellType.Circular
            });

            Spells.Add(new SpellData
            {
                CharName = "Khazix",
                Dangerlevel = 1,
                MissileName = "khazixelong",
                Name = "Leap Evolved",
                ProjectileSpeed = 1200,
                Radius = 300,
                Range = 900,
                SpellDelay = 0,
                SpellKey = SpellSlot.E,
                SpellName = "khazixelong",
                SpellType = SpellType.Circular
            });

            #endregion Khazix

            #region Kled

            Spells.Add(new SpellData
            {
                Angle = 5,
                CharName = "Kled",
                Dangerlevel = 2,
                IsThreeWay = true,
                MissileName = "KledRiderQMissile",
                Name = "Pocket Pistol",
                ProjectileSpeed = 3000,
                Radius = 40,
                Range = 700,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "KledRiderQ",
                SpellType = SpellType.Line
                //splits = 5 this "splits" is not even implemented
            });

            Spells.Add(new SpellData
            {
                CharName = "Kled",
                Dangerlevel = 3,
                MissileName = "KledQMissile",
                Name = "Beartrap on a Rope",
                ProjectileSpeed = 1600,
                Radius = 45,
                Range = 800,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "KledQ",
                SpellType = SpellType.Line
            });

            Spells.Add(new SpellData
            {
                CharName = "Kled",
                Dangerlevel = 2,
                Name = "Jousting",
                ProjectileSpeed = 945,
                Radius = 125,
                Range = 750,
                SpellDelay = 0,
                SpellKey = SpellSlot.E,
                SpellName = "KledE",
                SpellType = SpellType.Line
            });

            #endregion Kled

            #region KogMaw

            Spells.Add(new SpellData
            {
                CharName = "KogMaw",
                Dangerlevel = 2,
                MissileName = "KogMawQ",
                Name = "Caustic Spittle",
                ProjectileSpeed = 1650,
                Radius = 70,
                Range = 1200,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "KogMawQ",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions },
                FixedRange = true
            });

            Spells.Add(new SpellData
            {
                CharName = "KogMaw",
                Dangerlevel = 2,
                MissileName = "KogMawVoidOozeMissile",
                Name = "Void Ooze",
                ProjectileSpeed = 1350,
                Radius = 120,
                Range = 1360,
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                SpellName = "KogMawVoidOoze",
                SpellType = SpellType.Line,
                FixedRange = true
            });

            Spells.Add(new SpellData
            {
                CharName = "KogMaw",
                Dangerlevel = 2,
                MissileName = "KogMawLivingArtillery",
                Name = "Living Artillery",
                Radius = 235,
                Range = 2200,
                SpellDelay = 1100,
                SpellKey = SpellSlot.R,
                SpellName = "KogMawLivingArtillery",
                SpellType = SpellType.Circular
            });

            #endregion KogMaw

            #region Leblanc

            Spells.Add(new SpellData
            {
                CharName = "Leblanc",
                Dangerlevel = 3,
                Name = "Ethereal Chains [Beta]",
                MissileName = "LeblancEMissile",
                ProjectileSpeed = 1750,
                Radius = 55,
                Range = 960,
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                SpellName = "LeblancE",
                ExtraMissileNames = new[] { "LeblancRE" },
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions },
                FixedRange = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Leblanc",
                Dangerlevel = 2,
                Name = "Distortion [Beta]",
                ProjectileSpeed = 1450,
                Radius = 250,
                Range = 600,
                SpellDelay = 250,
                SpellKey = SpellSlot.W,
                SpellName = "LeblancW",
                SpellType = SpellType.Circular
            });

            #endregion Leblanc

            #region LeeSin

            Spells.Add(new SpellData
            {
                CharName = "LeeSin",
                Dangerlevel = 3,
                MissileName = "BlindMonkQOne",
                Name = "Sonic Wave",
                ProjectileSpeed = 1800,
                Radius = 60,
                Range = 1100,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "BlindMonkQOne",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions },
                FixedRange = true
            });

            //TODO: Add LeeSin R?
            //Spells.Add(
            //new SpellData
            //{
            //    charName = "LeeSin",
            //    dangerlevel = 3,
            //    missileName = "",
            //    name = "Dragon's Rage",
            //    projectileSpeed = 1000,
            //    radius = 0,
            //    range = 850,
            //    spellDelay = 250,
            //    spellKey = SpellSlot.R,
            //    isSpecial = true,
            //    spellName = "blindmonkrkick",
            //    spellType = SpellType.Line,
            //    noProcess = true
            //});

            #endregion LeeSin

            #region Leona

            Spells.Add(new SpellData
            {
                CharName = "Leona",
                Dangerlevel = 4,
                MissileName = "LeonaSolarFlare",
                Name = "Solar Flare",
                Radius = 250,
                Range = 1200,
                SpellDelay = 625,
                SpellKey = SpellSlot.R,
                SpellName = "LeonaSolarFlare",
                SpellType = SpellType.Circular
            });

            Spells.Add(new SpellData
            {
                CharName = "Leona",
                Dangerlevel = 3,
                ExtraDistance = 65,
                MissileName = "LeonaZenithBladeMissile",
                Name = "Zenith Blade",
                ProjectileSpeed = 2000,
                Radius = 70,
                Range = 905,
                SpellDelay = 200,
                SpellKey = SpellSlot.E,
                SpellName = "LeonaZenithBlade",
                SpellType = SpellType.Line,
                FixedRange = true
            });

            #endregion Leona

            #region Lissandra

            Spells.Add(new SpellData
            {
                CharName = "Lissandra",
                Dangerlevel = 3,
                MissileName = "LissandraW",
                Name = "Ring of Frost",
                Radius = 450,
                Range = 450,
                SpellDelay = 125,
                SpellKey = SpellSlot.W,
                SpellName = "LissandraW",
                SpellType = SpellType.Circular,
                DefaultOff = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Lissandra",
                Dangerlevel = 2,
                MissileName = "LissandraQMissile",
                Name = "Ice Shard",
                ProjectileSpeed = 2200,
                Radius = 75,
                Range = 825,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "LissandraQ",
                SpellType = SpellType.Line,
                FixedRange = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Lissandra",
                Dangerlevel = 2,
                Name = "Ice Shard Extended",
                ProjectileSpeed = 2200,
                Radius = 90,
                Range = 825,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "LissandraQShards",
                SpellType = SpellType.Line
            });

            Spells.Add(new SpellData
            {
                CharName = "Lissandra",
                Dangerlevel = 1,
                Name = "Glacial Path",
                MissileName = "LissandraEMissile",
                ProjectileSpeed = 850,
                Radius = 125,
                Range = 1025,
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                SpellName = "LissandraE",
                SpellType = SpellType.Line,
                FixedRange = true
            });

            #endregion Lissandra

            #region Lucian

            Spells.Add(new SpellData
            {
                CharName = "Lucian",
                Dangerlevel = 1,
                DefaultOff = true,
                MissileName = "lucianwmissile",
                Name = "Ardent Blaze",
                ProjectileSpeed = 1600,
                Radius = 80,
                Range = 1000,
                SpellDelay = 250,
                SpellKey = SpellSlot.W,
                SpellName = "lucianw",
                HasEndExplosion = true,
                SecondaryRadius = 145,
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions },
                FixedRange = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Lucian",
                Dangerlevel = 3,
                IsSpecial = true,
                MissileName = "LucianQ",
                Name = "Piercing Light",
                Radius = 65,
                Range = 1140,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "LucianQ",
                SpellType = SpellType.Line
            });

            Spells.Add(new SpellData
            {
                CharName = "Lucian",
                Dangerlevel = 2,
                MissileName = "lucianrmissile",
                Name = "The Culling",
                ProjectileSpeed = 2800,
                Radius = 110,
                Range = 1400,
                SpellDelay = 500,
                SpellKey = SpellSlot.R,
                SpellName = "lucianrmis",
                ExtraMissileNames = new[] { "lucianrmissileoffhand" },
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions },
                UsePackets = true,
                FixedRange = true
            });

            #endregion Lucian

            #region Lulu

            Spells.Add(new SpellData
            {
                CharName = "Lulu",
                Dangerlevel = 2,
                MissileName = "LuluQMissile",
                ExtraMissileNames = new[] { "LuluQMissileTwo" },
                Name = "Glitterlance",
                ProjectileSpeed = 1450,
                Radius = 60,
                Range = 925,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "LuluQ",
                ExtraSpellNames = new[] { "LuluQPix" },
                SpellType = SpellType.Line,
                IsSpecial = true
            });

            #endregion Lulu

            #region Lux

            Spells.Add(new SpellData
            {
                CharName = "Lux",
                Dangerlevel = 2,
                MissileName = "LuxLightStrikeKugel",
                Name = "Lucent Singularity",
                ProjectileSpeed = 1300,
                Radius = 330,
                Range = 1100,
                SpellDelay = 250,
                ExtraEndTime = 500,
                SpellKey = SpellSlot.E,
                SpellName = "LuxLightStrikeKugel",
                SpellType = SpellType.Circular
            });

            Spells.Add(new SpellData
            {
                CharName = "Lux",
                Dangerlevel = 2,
                Name = "Lucent Singularity",
                Radius = 330,
                Range = 1100,
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                SpellName = "LuxLightStrikeKugel",
                SpellType = SpellType.Circular,
                TrapTroyName = "lux_base_e_tar_aoe_" + Situation.EmitterColor() + ".troy",
                ExtraDrawHeight = -100,
                HasTrap = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Lux",
                Dangerlevel = 4,
                //missileName = "LuxRVfxMis", this missile is detected to late
                Name = "Final Spark",
                Radius = 190,
                Range = 3300,
                SpellDelay = 1000,
                SpellKey = SpellSlot.R,
                SpellName = "LuxMaliceCannon",
                SpellType = SpellType.Line,
                FixedRange = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Lux",
                Dangerlevel = 3,
                MissileName = "LuxLightBindingMis",
                Name = "Light Binding",
                ProjectileSpeed = 1200,
                Radius = 70,
                Range = 1300,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "LuxLightBinding",
                SpellType = SpellType.Line,
                FixedRange = true
            });

            #endregion Lux

            #region Maokai

            Spells.Add(new SpellData
            {
                CharName = "Maokai",
                Dangerlevel = 3,
                Name = "Arcane Smash",
                ProjectileSpeed = 1000,
                Radius = 110,
                Range = 600,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "MaokaiTrunkLine",
                SpellType = SpellType.Line
            });

            //TODO: Fix detection
            //Spells.Add(
            //new SpellData
            //{
            //    charName = "Maokai",
            //    dangerlevel = 3,
            //    name = "Arcane Smash KnockBack",
            //    radius = 100,
            //    range = 100,
            //    spellDelay = 250,
            //    spellKey = SpellSlot.Q,
            //    spellName = "MaokaiTrunkLine",
            //    spellType = SpellType.Circular,
            //});

            Spells.Add(new SpellData
            {
                CharName = "Maokai",
                Dangerlevel = 1,
                Name = "Sapling Toss",
                ProjectileSpeed = 1000,
                Radius = 250,
                Range = 1100,
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                SpellName = "MaokaiSapling2",
                SpellType = SpellType.Circular
            });

            #endregion Maokai

            #region Mordekaiser

            Spells.Add(new SpellData
            {
                Angle = 45,
                CharName = "Mordekaiser",
                Dangerlevel = 3,
                Name = "Syphon Of Destruction",
                Radius = 80,
                Range = 675,
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                SpellName = "MordekaiserSyphonOfDestruction",
                SpellType = SpellType.Cone
            });

            #endregion Mordekaiser

            #region Malphite

            Spells.Add(new SpellData
            {
                CharName = "Malphite",
                Dangerlevel = 4,
                MissileName = "UFSlash",
                Name = "Unstoppable Force",
                ProjectileSpeed = 2000,
                Radius = 300,
                Range = 1000,
                SpellDelay = 0,
                SpellKey = SpellSlot.R,
                SpellName = "UFSlash",
                SpellType = SpellType.Circular
            });

            #endregion Malphite

            #region Malzahar

            Spells.Add(new SpellData
            {
                CharName = "Malzahar",
                Dangerlevel = 2,
                IsSpecial = true,
                IsWall = true,
                Name = "Call of the Void",
                Radius = 85,
                Range = 900,
                SideRadius = 400,
                SpellDelay = 830,
                SpellKey = SpellSlot.Q,
                SpellName = "MalzaharQ",
                SpellType = SpellType.Line
            });

            #endregion Malzahar

            #region MonkeyKing

            Spells.Add(new SpellData
            {
                CharName = "MonkeyKing",
                Dangerlevel = 4,
                Name = "Cyclone [Beta]",
                ProjectileSpeed = 0,
                Radius = 250,
                Range = 250,
                SpellDelay = 125,
                SpellKey = SpellSlot.R,
                SpellName = "MonkeyKingSpinToWin",
                SpellType = SpellType.Circular,
                DefaultOff = true
            });

            #endregion MonkeyKing

            #region Morgana

            Spells.Add(new SpellData
            {
                CharName = "Morgana",
                Dangerlevel = 3,
                MissileName = "DarkBindingMissile",
                Name = "Dark Binding",
                ProjectileSpeed = 1200,
                Radius = 80,
                Range = 1300,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "DarkBindingMissile",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions },
                FixedRange = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Morgana",
                Dangerlevel = 2,
                Name = "Tormented Soil",
                Radius = 279,
                Range = 1300,
                SpellKey = SpellSlot.W,
                SpellName = "TormentedSoil",
                SpellType = SpellType.Circular,
                TrapTroyName = "morgana_base_w_tar_" + Situation.EmitterColor() + ".troy",
                HasTrap = true
            });

            #endregion Morgana

            #region Nami

            Spells.Add(new SpellData
            {
                CharName = "Nami",
                Dangerlevel = 3,
                MissileName = "namiqmissile",
                Name = "Aqua Prison",
                ProjectileSpeed = 2500,
                Radius = 200,
                Range = 875,
                SpellDelay = 450,
                SpellKey = SpellSlot.Q,
                SpellName = "namiq",
                SpellType = SpellType.Circular
            });

            Spells.Add(new SpellData
            {
                CharName = "Nami",
                Dangerlevel = 2,
                MissileName = "namirmissile",
                Name = "Tidal Wave",
                ProjectileSpeed = 850,
                Radius = 250,
                Range = 2750,
                SpellDelay = 500,
                SpellKey = SpellSlot.R,
                SpellName = "NamiR",
                SpellType = SpellType.Line,
                FixedRange = true
            });

            #endregion Nami

            #region Nautilus

            Spells.Add(new SpellData
            {
                CharName = "Nautilus",
                Dangerlevel = 3,
                MissileName = "NautilusAnchorDragMissile",
                Name = "Dredge Line",
                ProjectileSpeed = 2000,
                Radius = 90,
                Range = 1150,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "NautilusAnchorDrag",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions },
                FixedRange = true
            });

            #endregion Nautilus

            #region Nidalee

            Spells.Add(new SpellData
            {
                CharName = "Nidalee",
                Dangerlevel = 3,
                MissileName = "JavelinToss",
                Name = "Javelin Toss",
                ProjectileSpeed = 1300,
                Radius = 40,
                Range = 1500,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "JavelinToss",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions },
                FixedRange = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Nidalee",
                Dangerlevel = 3,
                Name = "Bushwhack",
                Radius = 80,
                Range = 1500,
                SpellKey = SpellSlot.W,
                SpellName = "Bushwhack",
                SpellType = SpellType.Circular,
                TrapTroyName = "nidalee_base_w_tc_" + Situation.EmitterColor() + ".troy",
                HasTrap = true
            });

            #endregion Nidalee

            #region Nocturne

            Spells.Add(new SpellData
            {
                CharName = "Nocturne",
                Dangerlevel = 2,
                MissileName = "NocturneDuskbringer",
                Name = "Duskbringer",
                ProjectileSpeed = 1400,
                Radius = 60,
                Range = 1200,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "NocturneDuskbringer",
                SpellType = SpellType.Line
            });

            #endregion Nocturne

            #region Olaf

            Spells.Add(new SpellData
            {
                CharName = "Olaf",
                Dangerlevel = 2,
                MissileName = "olafaxethrow",
                Name = "Axe Throw",
                ProjectileSpeed = 1600,
                Radius = 90,
                Range = 1000,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "OlafAxeThrowCast",
                SpellType = SpellType.Line
            });

            #endregion Olaf

            #region Orianna

            Spells.Add(new SpellData
            {
                CharName = "Orianna",
                Dangerlevel = 2,
                MissileName = "OrianaIzunaCommand",
                Name = "Commnad: Attack",
                ProjectileSpeed = 1400,
                Radius = 80,
                SecondaryRadius = 145,
                Range = 1500,
                SpellDelay = 0,
                SpellKey = SpellSlot.Q,
                UseEndPosition = true,
             
                SpellName = "OrianaIzunaCommand",
                SpellType = SpellType.Line,
                IsSpecial = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Orianna",
                Dangerlevel = 2,
                MissileName = "OrianaDissonanceCommand",
                Name = "Command: Dissonance",
                Radius = 250,
                Range = 1825,
                SpellDelay = 250,
                SpellKey = SpellSlot.W,
                SpellName = "OrianaDissonanceCommand",
                SpellType = SpellType.Circular,
                DefaultOff = true
            });

            //TODO: Add Orianna E

            Spells.Add(new SpellData
            {
                CharName = "Orianna",
                Dangerlevel = 4,
                MissileName = "OrianaDetonateCommand",
                Name = "Command: Shockwave",
                Radius = 410,
                Range = 410,
                SpellDelay = 500,
                SpellKey = SpellSlot.R,
                SpellName = "OrianaDetonateCommand",
                SpellType = SpellType.Circular
            });

            #endregion Orianna

            #region Pantheon

            Spells.Add(new SpellData
            {
                Angle = 35,
                CharName = "Pantheon",
                Dangerlevel = 2,
                Name = "Heartseeker",
                Radius = 80,
                Range = 600,
                ExtraEndTime = 750,
                SpellDelay = 1000,
                SpellKey = SpellSlot.E,
                SpellName = "PantheonE",
                SpellType = SpellType.Cone
            });

            #endregion Pantheon

            #region Poppy

            Spells.Add(new SpellData
            {
                CharName = "Poppy",
                Dangerlevel = 2,
                MissileName = "PoppyQ",
                Name = "Hammer Shock",
                Radius = 100,
                Range = 450,
                ExtraEndTime = 1000,
                SpellDelay = 500,
                SpellKey = SpellSlot.Q,
                SpellName = "PoppyQ",
                SpellType = SpellType.Line
            });

            Spells.Add(new SpellData
            {
                CharName = "Poppy",
                Dangerlevel = 4,
                Name = "Keeper's Verdict (Knockup)",
                Radius = 200,
                Range = 450,
                SpellDelay = 250,
                SpellKey = SpellSlot.R,
                SpellName = "PoppyRSpellInstant",
                SpellType = SpellType.Circular
            });

            Spells.Add(new SpellData
            {
                CharName = "Poppy",
                Dangerlevel = 3,
                Name = "Keeper's Verdict (Line)",
                Radius = 100,
                Range = 1150,
                ProjectileSpeed = 1750,
                SpellDelay = 300,
                SpellKey = SpellSlot.R,
                SpellName = "PoppyRSpell",
                MissileName = "PoppyRMissile",
                SpellType = SpellType.Line,
                FixedRange = true
            });

            #endregion

            #region Quinn

            Spells.Add(new SpellData
            {
                CharName = "Quinn",
                Dangerlevel = 3,
                MissileName = "QuinnQ",
                Name = "Blinding Assault",
                ProjectileSpeed = 1550,
                Radius = 60,
                Range = 1050,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "QuinnQ",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions },
                FixedRange = true
            });

            #endregion Quinn

            #region RekSai

            Spells.Add(new SpellData
            {
                CharName = "RekSai",
                Dangerlevel = 2,
                MissileName = "RekSaiQBurrowedMis",
                Name = "Prey Seeker",
                ProjectileSpeed = 1950,
                Radius = 65,
                Range = 1500,
                SpellDelay = 125,
                SpellKey = SpellSlot.Q,
                SpellName = "ReksaiQBurrowed",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions },
                FixedRange = true
            });

            Spells.Add(new SpellData
            {
                CharName = "RekSai",
                Dangerlevel = 3,
                Name = "Unburrow",
                ProjectileSpeed = 2300,
                Radius = 160,
                Range = 160,
                SpellDelay = 250f,
                ExtraEndTime = 500f,
                SpellKey = SpellSlot.W,
                SpellName = "ReksaiWBurrowed",
                SpellType = SpellType.Circular
            });

            #endregion RekSai

            #region Rengar

            Spells.Add(new SpellData
            {
                CharName = "Rengar",
                Dangerlevel = 3,
                Name = "Savagery [Beta]",
                Radius = 150,
                Range = 500,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "RengarQ2",
                ExtraSpellNames = new[] { "RengarQ2Emp" },
                SpellType = SpellType.Line
            });

            Spells.Add(new SpellData
            {
                CharName = "Rengar",
                Dangerlevel = 3,
                MissileName = "RengarEMis",
                Name = "Bola Strike [Beta]",
                ProjectileSpeed = 1500,
                Radius = 70,
                Range = 1000,
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                SpellName = "RengarE",
                ExtraSpellNames = new[] { "RengarEEmp" },
                ExtraMissileNames = new[] { "RengerEEmpMis" },
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions }
            });

            #endregion Rengar

            #region Riven

            Spells.Add(new SpellData
            {
                CharName = "Riven",
                Dangerlevel = 3,
                DefaultOff = true,
                MissileName = "RivenMartyr",
                Name = "Ki Burst",
                Radius = 280,
                Range = 650,
                SpellDelay = 0,
                SpellKey = SpellSlot.W,
                SpellName = "RivenMartyr",
                SpellType = SpellType.Circular
            });

            Spells.Add(new SpellData
            {
                Angle = 15,
                CharName = "Riven",
                Dangerlevel = 4,
                IsThreeWay = true,
                Name = "Wind Slash",
                ProjectileSpeed = 1600,
                Radius = 100,
                Range = 1100,
                SpellDelay = 250,
                SpellKey = SpellSlot.R,
                SpellName = "RivenIzunaBlade",
                SpellType = SpellType.Line,
                IsSpecial = true,
                FixedRange = true
            });

            #endregion Riven

            #region Rumble

            Spells.Add(new SpellData
            {
                CharName = "Rumble",
                Dangerlevel = 2,
                MissileName = "RumbleGrenadeMissile",
                Name = "Electro-Harpoon",
                ProjectileSpeed = 2000,
                Radius = 60,
                Range = 950,
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                SpellName = "RumbleGrenade",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions },
                FixedRange = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Rumble",
                Dangerlevel = 4,
                MissileName = "RumbleCarpetBombMissile",
                Name = "Carpet Bomb",
                ProjectileSpeed = 1600,
                Radius = 200,
                Range = 1200,
                SpellDelay = 0,
                SpellKey = SpellSlot.R,
                SpellName = "RumbleCarpetBomb",
                SpellType = SpellType.Line,
                UsePackets = true,
                FixedRange = true
            });

            #endregion Rumble

            #region Ryze

            Spells.Add(new SpellData
            {
                CharName = "Ryze",
                Dangerlevel = 2,
                MissileName = "RyzeQ",
                Name = "Overload",
                ProjectileSpeed = 1700,
                Radius = 60,
                Range = 900,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "RyzeQ",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions },
                FixedRange = true
            });

            #endregion Ryze

            #region Sejuani

            Spells.Add(new SpellData
            {
                CharName = "Sejuani",
                Dangerlevel = 3,
                Name = "Arctic Assault",
                ProjectileSpeed = 1500,
                Radius = 100,
                Range = 800,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "SejuaniQ",
                SpellType = SpellType.Line,
            });

            Spells.Add(new SpellData
            {
                CharName = "Sejuani",
                Dangerlevel = 1,
                ProjectileSpeed = 1250,
                Radius = 75,
                Range = 700,
                SpellDelay = 250,
                SpellKey = SpellSlot.W,
                SpellName = "SejuaniW",
                SpellType = SpellType.Line,
            });

            Spells.Add(new SpellData
            {
                CharName = "Sejuani",
                Dangerlevel = 4,
                MissileName = "SejuaniRMissile",
                Name = "Glacial Prison",
                ProjectileSpeed = 1600,
                Radius = 120,
                Range = 1300,
                SpellDelay = 250,
                HasEndExplosion = true,
                SecondaryRadius = 350,
                SpellKey = SpellSlot.R,
                SpellName = "SejuaniGlacialPrisonCast",
                ExtraSpellNames = new[] { "SejuaniGlacialPrison" },
                SpellType = SpellType.Line,
            });

            #endregion Sejuani

            #region Shen

            Spells.Add(new SpellData
            {
                CharName = "Shen",
                Dangerlevel = 3,
                MissileName = "ShenE",
                Name = "Shadow Dash",
                ProjectileSpeed = 1450,
                Radius = 50,
                Range = 600,
                SpellDelay = 0,
                SpellKey = SpellSlot.E,
                SpellName = "ShenE",
                SpellType = SpellType.Line,
            });

            #endregion Shen

            #region Shyvana

            Spells.Add(new SpellData
            {
                CharName = "Shyvana",
                Dangerlevel = 2,
                MissileName = "ShyvanaFireballMissile",
                Name = "Flame Breath",
                ProjectileSpeed = 1700,
                Radius = 60,
                Range = 950,
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                SpellName = "ShyvanaFireball",
                SpellType = SpellType.Line,
                FixedRange = true
            });

            Spells.Add(new SpellData
            {
                Angle = 10,
                CharName = "Shyvana",
                Dangerlevel = 2,
                IsThreeWay = true,
                MissileName = "ShyvanaFireballDragonFxMissile",
                Name = "Flame Breath Dragon",
                ProjectileSpeed = 2000,
                Radius = 70,
                Range = 850,
                ExtraEndTime = 200,
                SpellDelay = 250,
                SpellKey = SpellSlot.R,
                SpellName = "shyvanafireballdragon2",
                SpellType = SpellType.Line
                //splits = 5 this "splits" is not even implemented
            });

            Spells.Add(new SpellData
            {
                CharName = "Shyvana",
                Dangerlevel = 3,
                MissileName = "ShyvanaTransformCast",
                Name = "Dragon's Descent",
                ProjectileSpeed = 1250,
                Radius = 160,
                Range = 1000,
                SpellDelay = 250,
                SpellKey = SpellSlot.R,
                SpellName = "ShyvanaTransformCast",
                SpellType = SpellType.Line
            });

            #endregion Shyvana

            #region Sion

            //TODO: Sion Q, special code?

            Spells.Add(new SpellData
            {
                CharName = "Sion",
                Dangerlevel = 3,
                MissileName = "SionEMissile",
                Name = "Roar of the Slayer",
                ProjectileSpeed = 1800,
                Radius = 80,
                Range = 850,
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                SpellName = "SionE",
                SpellType = SpellType.Line,
                IsSpecial = true,
                FixedRange = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Sion",
                Dangerlevel = 3,
                MissileName = "SionR",
                Name = "Unstoppable Onslaught",
                ProjectileSpeed = 1000,
                Radius = 120,
                Range = 300,
                SpellDelay = 500,
                SpellKey = SpellSlot.R,
                SpellName = "SionR",
                SpellType = SpellType.None, // temp removal
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions },
                IsSpecial = true
            });

            #endregion Sion

            #region Sivir

            Spells.Add(new SpellData
            {
                CharName = "Sivir",
                Dangerlevel = 2,
                MissileName = "SivirQMissile",
                ExtraMissileNames = new[] { "SivirQMissileReturn" },
                Name = "Boomerang Blade",
                ProjectileSpeed = 1350,
                Radius = 100,
                Range = 1275,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "SivirQ",
                ExtraSpellNames = new[] { "SivirQReturn" },
                SpellType = SpellType.Line,
                FixedRange = true
            });

            #endregion Sivir

            #region Skarner

            Spells.Add(new SpellData
            {
                CharName = "Skarner",
                Dangerlevel = 3,
                MissileName = "SkarnerFractureMissile",
                Name = "Fracture",
                ProjectileSpeed = 1450,
                Radius = 70,
                Range = 1000,
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                SpellName = "SkarnerFracture",
                SpellType = SpellType.Line,
                FixedRange = true
            });

            #endregion Skarner

            #region Sona

            Spells.Add(new SpellData
            {
                CharName = "Sona",
                Dangerlevel = 4,
                MissileName = "SonaR",
                Name = "Crescendo",
                ProjectileSpeed = 2400,
                Radius = 150,
                Range = 1000,
                SpellDelay = 250,
                SpellKey = SpellSlot.R,
                SpellName = "SonaR",
                SpellType = SpellType.Line,
                FixedRange = true
            });

            #endregion Sona

            #region Soraka

            Spells.Add(new SpellData
            {
                CharName = "Soraka",
                Dangerlevel = 1,
                Name = "Starcall",
                MissileName = "SorakaQMissile",
                ProjectileSpeed = 1100,
                Radius = 260,
                Range = 970,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "SorakaQ",
                SpellType = SpellType.Circular
            });

            Spells.Add(new SpellData
            {
                CharName = "Soraka",
                Dangerlevel = 3,
                DefaultOff = true,
                Name = "Equinox",
                Radius = 260,
                Range = 875,
                SpellDelay = 1750,
                SpellKey = SpellSlot.E,
                SpellName = "SorakaE",
                SpellType = SpellType.Circular
            });

            #endregion Soraka

            #region Swain

            Spells.Add(new SpellData
            {
                CharName = "Swain",
                Dangerlevel = 3,
                Name = "Nevermove",
                MissileName = "SwainShadowGrasp",
                Radius = 250,
                Range = 900,
                SpellDelay = 1100,
                SpellKey = SpellSlot.W,
                SpellName = "SwainShadowGrasp",
                SpellType = SpellType.Circular
            });

            #endregion Swain

            #region Syndra

            Spells.Add(new SpellData
            {
                Angle = 45,
                CharName = "Syndra",
                Dangerlevel = 3,
                Name = "Scatter the Weak",
                MissileName = "SyndraESphereMissile",
                ProjectileSpeed = 2000,
                Radius = 100,
                Range = 950,
                SpellDelay = 0f,
                SpellKey = SpellSlot.E,
                SpellName = "SyndraE",
                ExtraSpellNames = new[] { "SyndraEMissile2", "syndrae5" },
                SpellType = SpellType.Line,
                IsSpecial = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Syndra",
                Dangerlevel = 2,
                MissileName = "syndrawcast",
                Name = "Force of Will",
                ProjectileSpeed = 1450,
                Radius = 220,
                Range = 950,
                SpellDelay = 250,
                SpellKey = SpellSlot.W,
                SpellName = "syndrawcast",
                SpellType = SpellType.Circular
            });

            Spells.Add(new SpellData
            {
                CharName = "Syndra",
                Dangerlevel = 2,
                MissileName = "SyndraQSpell",
                Name = "Dark Sphere",
                Radius = 150,
                Range = 800,
                ProjectileSpeed = 1000,
                SpellDelay = 600,
                SpellKey = SpellSlot.Q,
                SpellName = "SyndraQ",
                SpellType = SpellType.Circular
            });

            #endregion Syndra

            #region TahmKench

            Spells.Add(new SpellData
            {
                CharName = "TahmKench",
                Dangerlevel = 3,
                MissileName = "tahmkenchqmissile",
                Name = "Tongue Lash",
                ProjectileSpeed = 2000,
                SpellDelay = 250,
                Radius = 70,
                Range = 800,
                SpellKey = SpellSlot.Q,
                SpellName = "TahmKenchQ",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions },
                FixedRange = true
            });

            #endregion TahmKench

            #region Talon

            Spells.Add(new SpellData
            {
                CharName = "Talon",
                Dangerlevel = 4,
                Name = "Shadow Assault [Beta]",
                ProjectileSpeed = 2400,
                Radius = 140,
                Range = 550,
                SpellKey = SpellSlot.R,
                SpellName = "talonrmisone",
                ExtraMissileNames = new[] { "talonrmistwo" },
                SpellType = SpellType.Line,
                FixedRange = true
            });

            Spells.Add(new SpellData
            {
                Angle = 14,
                CharName = "Talon",
                Dangerlevel = 3,
                IsThreeWay = true,
                MissileName = "talonwmissile",
                Name = "Rake [Beta]",
                ProjectileSpeed = 2300,
                Radius = 75,
                Range = 900,
                SpellKey = SpellSlot.W,
                SpellName = "talonw",
                SpellType = SpellType.Line,
                FixedRange = true,
                IsSpecial = true
            });

            Spells.Add(new SpellData
            {
                Angle = 14,
                CharName = "Talon",
                Dangerlevel = 3,
                IsThreeWay = true,
                Name = "Rake Return [Beta]",
                ProjectileSpeed = 3000,
                Radius = 75,
                Range = 900,
                SpellKey = SpellSlot.W,
                SpellName = "talonwmissiletwo",
                SpellType = SpellType.Line,
                FixedRange = true,
                IsSpecial = true
            });

            #endregion Talon

            #region Taliyah

            Spells.Add(new SpellData
            {
                CharName = "Taliyah",
                Dangerlevel = 2,
                MissileName = "TaliyahQMis",
                ProjectileSpeed = 1450,
                Name = "Threaded Volley",
                Radius = 100,
                Range = 1000,
                FixedRange = true,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "TaliyahQ",
                SpellType = SpellType.Line,
                DefaultOff = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Taliyah",
                Dangerlevel = 3,
                Name = "Seismic Shove",
                Radius = 165,
                Range = 900,
                SpellDelay = 450,
                ExtraEndTime = 1000,
                SpellKey = SpellSlot.W,
                SpellName = "TaliyahWVC",
                ExtraSpellNames = new[] { "TaliyahW" },
                SpellType = SpellType.Circular
            });

            #endregion Taliyah

            #region Taric

            Spells.Add(new SpellData
            {
                CharName = "Taric",
                Dangerlevel = 2,
                MissileName = "TaricEMissile",
                Name = "Dazzle",
                Radius = 100,
                Range = 750,
                FixedRange = true,
                SpellDelay = 1000,
                SpellKey = SpellSlot.E,
                SpellName = "TaricE",
                SpellType = SpellType.Line,
                IsSpecial = true
            });

            #endregion

            #region Thresh

            Spells.Add(new SpellData
            {
                CharName = "Thresh",
                Dangerlevel = 2,
                MissileName = "ThreshQMissile",
                Name = "Death Sentence",
                ProjectileSpeed = 1900,
                Radius = 70,
                Range = 1200,
                SpellDelay = 500,
                SpellKey = SpellSlot.Q,
                SpellName = "ThreshQ",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions },
                FixedRange = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Thresh",
                Dangerlevel = 3,
                MissileName = "ThreshEMissile1",
                Name = "Flay",
                ProjectileSpeed = 2000,
                Radius = 110,
                Range = 1075,
                SpellDelay = 125,
                SpellKey = SpellSlot.E,
                SpellName = "ThreshE",
                ExtraSpellNames = new[] { "ThreshEFlay" },
                SpellType = SpellType.Line,
                FixedRange = true,
                UsePackets = true
            });

            #endregion Thresh

            #region Tristana

            Spells.Add(new SpellData
            {
                CharName = "Tristana",
                Dangerlevel = 1,
                MissileName = "RocketJump",
                Name = "Rocket Jump",
                ProjectileSpeed = 1000,
                Radius = 270,
                Range = 900,
                SpellDelay = 250,
                SpellKey = SpellSlot.W,
                SpellName = "TristanaW",
                SpellType = SpellType.Circular
            });

            #endregion Tristana

            #region Tryndamere

            Spells.Add(new SpellData
            {
                CharName = "Tryndamere",
                Dangerlevel = 2,
                MissileName = "slashCast",
                Name = "Spinning Slash",
                ProjectileSpeed = 1300,
                Radius = 95,
                Range = 660,
                SpellDelay = 0,
                SpellKey = SpellSlot.E,
                SpellName = "slashCast",
                SpellType = SpellType.Line
            });

            #endregion Tryndamere

            #region TwistedFate

            Spells.Add(new SpellData
            {
                Angle = 28,
                CharName = "TwistedFate",
                Dangerlevel = 2,
                IsThreeWay = true,
                MissileName = "SealFateMissile",
                Name = "Wild Cards",
                ProjectileSpeed = 1000,
                Radius = 40,
                Range = 1450,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "WildCards",
                SpellType = SpellType.Line
            });

            #endregion TwistedFate

            #region Twitch

            Spells.Add(new SpellData
            {
                CharName = "Twitch",
                Dangerlevel = 2,
                MissileName = "TwitchVenomCaskMissile",
                Name = "Venom Cask",
                ProjectileSpeed = 1400,
                Radius = 280,
                Range = 900,
                SpellDelay = 250,
                SpellKey = SpellSlot.W,
                SpellName = "TwitchVenomCask",
                SpellType = SpellType.Circular
            });

            Spells.Add(new SpellData
            {
                CharName = "Twitch",
                Dangerlevel = 3,
                MissileName = "TwitchSprayandPrayAttack",
                Name = "Spray and Pray",
                ProjectileSpeed = 4000,
                Radius = 65,
                Range = 1100,
                SpellDelay = 250,
                SpellKey = SpellSlot.R,
                SpellName = "TwitchSprayandPrayAttack",
                SpellType = SpellType.Line,
                IsSpecial = true
            });

            #endregion Twitch

            #region Urgot

            Spells.Add(new SpellData
            {
                CharName = "Urgot",
                Dangerlevel = 2,
                Name = "UrgotQ",
                ProjectileSpeed = 500,
                Radius = 200,
                Range = 800,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "UrgotQMissile",
                SpellType = SpellType.Line,
                //CollisionObjects = new[] {CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions},
                FixedRange = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Urgot",
                Dangerlevel = 3,
                MissileName = "urgote",
                Name = "UrgotE",
                ProjectileSpeed = 1500,
                Radius = 100,
                Range = 600,
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                SpellName = "urgote",
                SpellType = SpellType.Line,
            });

            Spells.Add(new SpellData
            {
                CharName = "Urgot",
                Dangerlevel = 4,
                MissileName = "urgotr",
                Name = "UrgotR",
                ProjectileSpeed = 3200,
                Radius = 80,
                Range = 1600,
                SpellDelay = 283,
                SpellKey = SpellSlot.R,
                SpellName = "urgotr",
                SpellType = SpellType.Line,
            });

            #endregion Urgot

            #region Varus

            Spells.Add(new SpellData
            {
                CharName = "Varus",
                Dangerlevel = 2,
                Name = "Hail of Arrows",
                ProjectileSpeed = 1750,
                Radius = 235,
                Range = 925,
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                SpellName = "VarusE",
                SpellType = SpellType.Circular
            });

            Spells.Add(new SpellData
            {
                CharName = "Varus",
                Dangerlevel = 2,
                MissileName = "varusqmissile",
                Name = "Piercing Arrow",
                ProjectileSpeed = 1850,
                Radius = 75,
                Range = 1650,
                SpellDelay = 0,
                SpellKey = SpellSlot.Q,
                SpellName = "varusq",
                SpellType = SpellType.Line,
                UsePackets = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Varus",
                Dangerlevel = 3,
                Name = "Chain of Corruption",
                MissileName = "VarusRMissile",
                ProjectileSpeed = 1950,
                Radius = 120,
                Range = 1250,
                SpellDelay = 250,
                SpellKey = SpellSlot.R,
                SpellName = "VarusR",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions },
                FixedRange = true
            });

            #endregion Varus

            #region Veigar

            Spells.Add(new SpellData
            {
                CharName = "Veigar",
                Dangerlevel = 2,
                MissileName = "VeigarBalefulStrikeMis",
                Name = "Baleful Strike",
                ProjectileSpeed = 2200,
                Radius = 70,
                Range = 950,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "VeigarBalefulStrike",
                SpellType = SpellType.Line,
                FixedRange = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Veigar",
                Dangerlevel = 2,
                MissileName = "VeigarDarkMatter",
                Name = "Dark Matter",
                Radius = 225,
                Range = 900,
                SpellDelay = 1200,
                SpellKey = SpellSlot.W,
                SpellName = "VeigarDarkMatter",
                SpellType = SpellType.Circular
            });

            Spells.Add(new SpellData
            {
                CharName = "Veigar",
                Dangerlevel = 3,
                Name = "Event Horizon",
                Radius = 375,
                Range = 700,
                SpellDelay = 500,
                ExtraEndTime = 3300,
                SpellKey = SpellSlot.E,
                SpellName = "VeigarEventHorizon",
                SpellType = SpellType.Circular,
                DefaultOff = true
            });

            #endregion Veigar

            #region Velkoz

            Spells.Add(new SpellData
            {
                CharName = "Velkoz",
                Dangerlevel = 3,
                Name = "Tectonic Disruption",
                ProjectileSpeed = 1500,
                Radius = 225,
                Range = 800,
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                SpellName = "VelkozE",
                SpellType = SpellType.Circular
            });

            Spells.Add(new SpellData
            {
                CharName = "Velkoz",
                Dangerlevel = 2,
                MissileName = "VelkozWMissile",
                Name = "Void Rift",
                ProjectileSpeed = 1700,
                Radius = 90,
                Range = 1150,
                ExtraEndTime = 1000,
                SpellDelay = 250,
                SpellKey = SpellSlot.W,
                SpellName = "VelkozW",
                SpellType = SpellType.Line,
                FixedRange = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Velkoz",
                Dangerlevel = 2,
                Name = "Plasma Fission (Split)",
                ProjectileSpeed = 2100,
                Radius = 50,
                Range = 1100,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "VelkozQMissileSplit",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions },
                UsePackets = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Velkoz",
                Dangerlevel = 2,
                MissileName = "VelkozQMissile",
                Name = "Plasma Fission",
                ProjectileSpeed = 1300,
                Radius = 55,
                Range = 1250,
                SpellDelay = 250f,
                SpellKey = SpellSlot.Q,
                SpellName = "VelkozQ",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions },
                FixedRange = true
            });

            #endregion Velkoz

            #region Vi

            Spells.Add(new SpellData
            {
                CharName = "Vi",
                Dangerlevel = 3,
                Name = "Vault Breaker",
                ProjectileSpeed = 1500,
                Radius = 90,
                Range = 775,
                SpellKey = SpellSlot.Q,
                SpellName = "ViQMissile",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions },
                UsePackets = true
            });

            #endregion Vi

            #region Viktor

            Spells.Add(new SpellData
            {
                CharName = "Viktor",
                Dangerlevel = 3,
                MissileName = "ViktorDeathRayMissile",
                Name = "Death Ray",
                ProjectileSpeed = 1050,
                Radius = 75,
                Range = 815,
                SpellKey = SpellSlot.E,
                SpellName = "ViktorDeathRay",
                ExtraMissileNames = new[] { "ViktorEAugMissile" },
                SpellType = SpellType.Line,
                UsePackets = true,
                FixedRange = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Viktor",
                Dangerlevel = 3,
                Name = "Death Ray Aftershock",
                SpellDelay = 500,
                Radius = 75,
                Range = 815,
                SpellKey = SpellSlot.E,
                SpellName = "ViktorDeathRay3",
                SpellType = SpellType.Line,
                FixedRange = true,
                UsePackets = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Viktor",
                Dangerlevel = 3,
                Name = "Graviton Field",
                Radius = 300,
                Range = 625,
                SpellDelay = 1500,
                SpellKey = SpellSlot.W,
                SpellName = "ViktorGravitonField",
                SpellType = SpellType.Circular,
                DefaultOff = true
            });

            #endregion Viktor

            #region Vladimir

            Spells.Add(new SpellData
            {
                CharName = "Vladimir",
                Dangerlevel = 3,
                MissileName = "VladimirR", // mage update
                Name = "Hemoplague",
                Radius = 375,
                Range = 700,
                SpellDelay = 250,
                SpellKey = SpellSlot.R,
                SpellName = "VladimirR", // mage update
                SpellType = SpellType.Circular
            });

            #endregion Vladimir

            #region Xerath

            Spells.Add(new SpellData
            {
                CharName = "Xerath",
                Dangerlevel = 2,
                MissileName = "XerathArcaneBarrage2",
                Name = "Eye of Destruction",
                Radius = 280,
                Range = 1000,
                SpellDelay = 750,
                SpellKey = SpellSlot.W,
                SpellName = "XerathArcaneBarrage2",
                SpellType = SpellType.Circular,
                ExtraDrawHeight = 45
            });

            Spells.Add(new SpellData
            {
                CharName = "Xerath",
                Dangerlevel = 2,
                MissileName = "XerathArcanopulse2",
                Name = "Arcanopulse",
                Radius = 70,
                Range = 1525,
                SpellDelay = 500,
                SpellKey = SpellSlot.Q,
                SpellName = "XerathArcanopulse2",
                UseEndPosition = true,
                SpellType = SpellType.Line
            });

            Spells.Add(new SpellData
            {
                CharName = "Xerath",
                Dangerlevel = 3,
                Name = "Rite of the Arcane",
                MissileName = "XerathLocusPulse",
                Radius = 200,
                Range = 5600,
                SpellDelay = 600,
                SpellKey = SpellSlot.R,
                SpellName = "xerathrmissilewrapper",
                ExtraSpellNames = new[] { "XerathLocusPulse" },
                SpellType = SpellType.Circular
            });

            Spells.Add(new SpellData
            {
                CharName = "Xerath",
                Dangerlevel = 3,
                MissileName = "XerathMageSpearMissile",
                Name = "Shocking Orb",
                ProjectileSpeed = 1600,
                Radius = 60,
                Range = 1125,
                SpellDelay = 200,
                SpellKey = SpellSlot.E,
                SpellName = "XerathMageSpear",
                SpellType = SpellType.Line,
                CollisionObjects = new[] { CollisionObjectType.EnemyChampions, CollisionObjectType.EnemyMinions },
                FixedRange = true
            });

            #endregion Xerath

            #region Yasuo

            Spells.Add(new SpellData
            {
                CharName = "Yasuo",
                Dangerlevel = 3,
                MissileName = "YasuoQ3Mis",
                Name = "Steel Tempest (Tornado)",
                ProjectileSpeed = 1250,
                Radius = 90,
                Range = 1150,
                SpellDelay = 300,
                SpellKey = SpellSlot.Q,
                SpellName = "YasuoQ3W",
                SpellType = SpellType.Line,
                FixedRange = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Yasuo",
                Dangerlevel = 2,
                MissileName = "yasuoq",
                ExtraMissileNames = new[] { "yasuoq2" },
                Name = "Steel Tempest",
                Radius = 40,
                Range = 550,
                FixedRange = true,
                SpellDelay = 400,
                SpellKey = SpellSlot.Q,
                SpellName = "YasuoQ",
                ExtraSpellNames = new[] { "YasuoQ2" },
                SpellType = SpellType.Line,
                Invert = true
            });

            #endregion Yasuo

            #region Yorick

            Spells.Add(new SpellData
            {
                CharName = "Yorick",
                Dangerlevel = 3,
                Name = "Dark Procession [Beta]",
                Radius = 250,
                Range = 600,
                SpellDelay = 500,
                SpellKey = SpellSlot.W,
                ExtraEndTime = 1000,
                SpellName = "YorickW",
                SpellType = SpellType.Circular
            });

            Spells.Add(new SpellData
            {
                CharName = "Yorick",
                Dangerlevel = 3,
                //missileName = "YorickEMissile",
                Name = "Mourning Mist [Beta]",
                ProjectileSpeed = 750,
                Radius = 125,
                Range = 580,
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                SpellName = "YorickE",
                SpellType = SpellType.Line,
                UpdatePosition = false,
                IsSpecial = true
            });

            #endregion Yorick

            #region Zac

            Spells.Add(new SpellData
            {
                CharName = "Zac",
                Dangerlevel = 3,
                MissileName = "ZacQ",
                Name = "Stretching Strike",
                Radius = 120,
                Range = 550,
                SpellDelay = 400,
                SpellKey = SpellSlot.Q,
                SpellName = "ZacQ",
                SpellType = SpellType.Line,
                FixedRange = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Zac",
                Dangerlevel = 3,
                Name = "Elastic Slingshot [Beta]",
                ProjectileSpeed = 1000,
                Radius = 300,
                Range = 1800,
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                SpellName = "ZacE",
                SpellType = SpellType.Circular
            });

            //TODO: Zac R

            #endregion Zac

            #region Zed

            Spells.Add(new SpellData
            {
                CharName = "Zed",
                Dangerlevel = 3,
                MissileName = "ZedQMissile",
                Name = "Razor Shuriken",
                ProjectileSpeed = 1700,
                Radius = 50,
                Range = 925,
                SpellDelay = 250,
                SpellKey = SpellSlot.Q,
                SpellName = "ZedQ",
                SpellType = SpellType.Line
            });

            //TODO: Add Zed E

            #endregion Zed

            #region Ziggs

            Spells.Add(new SpellData
            {
                CharName = "Ziggs",
                Dangerlevel = 1,
                MissileName = "ZiggsE",
                Name = "Hexplosive Minefield",
                ProjectileSpeed = 3000,
                Radius = 235,
                Range = 2000,
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                SpellName = "ZiggsE",
                SpellType = SpellType.Circular
            });

            Spells.Add(new SpellData
            {
                CharName = "Ziggs",
                Dangerlevel = 2,
                MissileName = "ZiggsW",
                Name = "Satchel Charge",
                ProjectileSpeed = 2000,
                Radius = 275,
                Range = 1000,
                SpellDelay = 250,
                ExtraEndTime = 1000,
                SpellKey = SpellSlot.W,
                SpellName = "ZiggsW",
                SpellType = SpellType.Circular
            });

            Spells.Add(new SpellData
            {
                CharName = "Ziggs",
                Dangerlevel = 2,
                Name = "Bouncing Bomb",
                ProjectileSpeed = 1700,
                Radius = 150,
                Range = 850,
                SpellDelay = 125,
                SpellKey = SpellSlot.Q,
                SpellName = "ZiggsQ",
                SpellType = SpellType.Circular,
                IsSpecial = true,
                NoProcess = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Ziggs",
                Dangerlevel = 4,
                MissileName = "ZiggsR",
                Name = "Mega Inferno Bomb",
                ProjectileSpeed = 1550,
                Radius = 500,
                Range = 5300,
                SpellDelay = 400,
                SpellKey = SpellSlot.R,
                SpellName = "ZiggsR",
                SpellType = SpellType.Circular,
                DefaultOff = true,
                IsSpecial = true
            });

            #endregion Ziggs

            #region Zilean

            Spells.Add(new SpellData
            {
                CharName = "Zilean",
                Dangerlevel = 3,
                MissileName = "ZileanQMissile",
                Name = "Time Bomb",
                Radius = 150,
                Range = 900,
                ExtraEndTime = 1000,
                SpellDelay = 650,
                SpellKey = SpellSlot.Q,
                SpellName = "ZileanQ",
                SpellType = SpellType.Circular,
                IsSpecial = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Zilean",
                Dangerlevel = 3,
                SpellName = "ZileanQ",
                Name = "Time Bomb",
                Radius = 160,
                Range = 900,
                SpellKey = SpellSlot.Q,
                SpellType = SpellType.Circular,
                TrapTroyName = "zilean_base_q_timebombground" + Situation.EmitterColor() + ".troy",
                ExtraDrawHeight = -100,
                HasTrap = true
            });

            #endregion Zilean

            #region Zyra

            Spells.Add(new SpellData
            {
                CharName = "Zyra",
                Dangerlevel = 3,
                Name = "Grasping Roots",
                MissileName = "ZyraEMissile",
                ProjectileSpeed = 1400, // 1150
                Radius = 70,
                Range = 1150,
                SpellDelay = 250,
                SpellKey = SpellSlot.E,
                SpellName = "ZyraE",
                SpellType = SpellType.Line,
                FixedRange = true
            });

            Spells.Add(new SpellData
            {
                CharName = "Zyra",
                Dangerlevel = 2,
                MissileName = "ZyraQ",
                Name = "Deadly Bloom",
                Radius = 140,
                Range = 800,
                SpellDelay = 850,
                SpellKey = SpellSlot.Q,
                SpellName = "ZyraQ",
                SpellType = SpellType.Line,
                IsPerpendicular = true,
                SecondaryRadius = 400
            });

            Spells.Add(new SpellData
            {
                CharName = "Zyra",
                Dangerlevel = 4,
                Name = "Stranglethorns",
                Radius = 525,
                Range = 700,
                ExtraEndTime = 2000,
                SpellDelay = 500,
                SpellKey = SpellSlot.R,
                SpellName = "ZyraR",
                ExtraSpellNames = new[] { "ZyraBrambleZone" },
                SpellType = SpellType.Circular,
                DefaultOff = true
            });

            #endregion Zyra
        }
    }
}
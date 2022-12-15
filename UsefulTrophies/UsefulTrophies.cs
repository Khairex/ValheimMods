using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace UsefulTrophies
{
    [BepInPlugin(Id, "Useful Trophies Mod", "1.2.0")]
    [BepInProcess("valheim.exe")]
    public class UsefulTrophies : BaseUnityPlugin
    {
        public const string Id = "gg.khairex.usefultrophies";

        public Harmony Harmony { get; } = new Harmony(Id);

        public static bool CanConsumeBosses = true;
        public static bool CanConsumeBossSummonItems = true;
        public static float BossPowerTime = 720f;

        // Dictionary of every enemy trophy type.
        // Full Item name would be $item_trophy_deer
        public static Dictionary<string, float> TrophyXPDict = new Dictionary<string, float>
        {
            { "deer", 8f },
            { "boar", 4f },
            { "neck", 10f },
            { "greydwarf", 5f },
            { "greydwarfbrute", 25f },
            { "greydwarfshaman", 15f },
            { "skeleton", 10f },
            { "skeletonpoison", 25f },
            { "troll", 30f },
            { "surtling", 25f },
            { "leech", 25f },
            { "draugr", 20f },
            { "draugrelite", 30f },
            { "blob", 20f },
            { "wraith", 30f },
            { "abomination", 100f },
            { "wolf", 35f },
            { "fenring", 40f },
            { "hatchling", 40f },
            { "sgolem", 50f },
            { "ulv", 50f },
            { "cultist", 50f },
            { "goblin", 50f },
            { "goblinbrute", 50f },
            { "goblinshaman", 50f },
            { "lox", 120f },
            { "growth", 50f },
            { "deathsquito", 80f },
            { "serpent", 150f },
            { "eikthyr", 30f },
            { "elder", 80f },
            { "bonemass", 300f },
            { "dragonqueen", 500f },
            { "goblinking", 700f },
            { "hare", 50f },
            { "gjall", 100f },
            { "tick", 80f },
            { "dvergr", 300f },
            { "seeker", 150f },
            { "seekerbrute", 300f },
            { "seekerqueen", 1000f },
        };

        // Boss trophies are optionally handled differently
        public static List<string> BossEnemies = new List<string>()
        {
            "eikthyr",
            "elder",
            "bonemass",
            "dragonqueen", 
            "goblinking",
            "seekerqueen",
        };
        
        public static Dictionary<string, string> BossPowerDict = new Dictionary<string, string>()
        {
            {"eikthyr", "GP_Eikthyr"},
            {"elder", "GP_TheElder"},
            {"bonemass", "GP_Bonemass"},
            {"dragonqueen", "GP_Moder"},
            {"goblinking", "GP_Yagluth"},
            {"seekerqueen", "GP_Queen"},
        };
        
        public static Dictionary<string, string> SecondaryPowerDict = new Dictionary<string, string>()
        {
            {"$item_trophy_deer", "GP_Eikthyr"},
            {"$item_ancientseed", "GP_TheElder"},
            {"$item_witheredbone", "GP_Bonemass"},
            {"$item_dragonegg", "GP_Moder"},
            {"$item_goblintotem", "GP_Yagluth"},
        };
        
        public static Dictionary<string, float> SecondaryPowerTime = new Dictionary<string, float>
        {
            {"$item_trophy_deer", 120},
            {"$item_ancientseed", 120},
            {"$item_witheredbone", 120},
            {"$item_dragonegg", 300},
            {"$item_goblintotem", 120},
        };
        
        void Awake()
        {
            // Setup/Apply Config file
            ConfigDefinition bossConsumption =
                new ConfigDefinition("BossConsumption", "CanConsumeBosses");
            CanConsumeBosses = Config.Bind(bossConsumption, true).Value;

            ConfigDefinition bossSummonConsumption =
                new ConfigDefinition("BossConsumption", "CanConsumeBossSummonItems");
            CanConsumeBossSummonItems = Config.Bind(bossSummonConsumption, true).Value;
            
            ConfigDefinition bossPowerTime =
                new ConfigDefinition("BossConsumption", "BossPowerTimeSeconds");
            BossPowerTime = Config.Bind(bossPowerTime, 720f).Value;

            // Secondary Time Config
            List<ConfigDefinition> secondaryTimeList = new List<ConfigDefinition>();
            foreach (var item in SecondaryPowerTime.Keys)
            {
                secondaryTimeList.Add(new ConfigDefinition("BossSummonItemTimeSeconds", item));
            }
            
            foreach (var config in secondaryTimeList)
            {
                SecondaryPowerTime[config.Key] = Config.Bind(config, SecondaryPowerTime[config.Key]).Value;
            }
            
            // Xp Config
            List<ConfigDefinition> xpConfigList = new List<ConfigDefinition>();

            foreach (var enemy in TrophyXPDict.Keys)
            {
                xpConfigList.Add(new ConfigDefinition("ExpScaling", enemy));
            }
            
            foreach (var config in xpConfigList)
            {
                TrophyXPDict[config.Key] = Config.Bind(config, TrophyXPDict[config.Key]).Value;
            }

            Harmony.PatchAll();
        }
    }
}

using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace UsefulTrophies
{
    [BepInPlugin(Id, "Useful Trophies Mod", "1.0.1")]
    [BepInProcess("valheim.exe")]
    public class UsefulTrophies : BaseUnityPlugin
    {
        public const string Id = "gg.khairex.usefultrophies";

        public Harmony Harmony { get; } = new Harmony(Id);

        public static bool CanConsumeBosses = true;

        public static bool CanConsumeAnything = false;

        // Dictionary of every enemy trophy type.
        // Full Item name would be $item_trophy_deer
        public static Dictionary<string, float> TrophyXPDict = new Dictionary<string, float>
        {
            { "deer", 4f },
            { "boar", 4f },
            { "neck", 5f },
            { "greydwarf", 5f },
            { "greydwarfbrute", 10f },
            { "greydwarfshaman", 10f },
            { "skeleton", 6f },
            { "skeletonpoison", 15f },
            { "troll", 15f },
            { "surtling", 10f },
            { "leech", 7f },
            { "draugr", 8f },
            { "draugrelite", 15f },
            { "blob", 12f },
            { "wraith", 30f },
            { "wolf", 12f },
            { "fenring", 35f },
            { "hatchling", 12f },
            { "sgolem", 25f },
            { "goblin", 15f },
            { "goblinbrute", 30f },
            { "goblinshaman", 20f },
            { "lox", 20f },
            { "deathsquito", 20f },
            { "serpent", 60f },
            { "eikthyr", 25f },
            { "elder", 35f },
            { "bonemass", 50f },
            { "dragonqueen", 50f },
            { "goblinking", 50f },
        };

        // Boss trophies are optionally handled differently
        public static List<string> BossEnemies = new List<string>()
        {
            "eikthyr",
            "elder",
            "bonemass",
            "dragonqueen",
            "goblinking",
        };

        void Awake()
        {
            // Setup/Apply Config file
            List<ConfigDefinition> configList = new List<ConfigDefinition>();

            foreach (var enemy in TrophyXPDict.Keys)
            {
                configList.Add(new ConfigDefinition("ExpScaling", enemy));
            }

            ConfigDefinition bossConsumption =
                new ConfigDefinition("BossConsumption", "CanConsumeBosses");

            ConfigDefinition consumeAnything =
                new ConfigDefinition("GluttonyTrashcan", "CanConsumeAnything");
            
            foreach (var config in configList)
            {
                TrophyXPDict[config.Key] = Config.Bind(config, TrophyXPDict[config.Key]).Value;
            }
            CanConsumeBosses = Config.Bind(bossConsumption, true).Value;
            CanConsumeAnything = Config.Bind(consumeAnything, false).Value;

            Harmony.PatchAll();
        }
    }
}

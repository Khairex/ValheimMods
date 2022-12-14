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

        // Dictionary of every enemy trophy type.
        // Full Item name would be $item_trophy_deer
        public static Dictionary<string, float> TrophyXPDict = new Dictionary<string, float>
        {
            { "deer", 8f },
            { "boar", 4f },
            { "neck", 5f },
            { "greydwarf", 5f },
            { "greydwarfbrute", 15f },
            { "greydwarfshaman", 15f },
            { "skeleton", 10f },
            { "skeletonpoison", 15f },
            { "troll", 30f },
            { "surtling", 25f },
            { "leech", 25f },
            { "draugr", 20f },
            { "draugrelite", 30f },
            { "blob", 20f },
            { "wraith", 30f },
            { "abomination", 40f },
            { "wolf", 35f },
            { "fenring", 40f },
            { "hatchling", 40f },
            { "sgolem", 50f },
            { "ulv", 50f },
            { "cultist", 50f },
            { "goblin", 50f },
            { "goblinbrute", 50f },
            { "goblinshaman", 50f },
            { "lox", 100f },
            { "growth", 50f },
            { "deathsquito", 80f },
            { "serpent", 100f },
            { "eikthyr", 25f },
            { "elder", 50f },
            { "bonemass", 150f },
            { "dragonqueen", 300f },
            { "goblinking", 500f },
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

            foreach (var config in configList)
            {
                TrophyXPDict[config.Key] = Config.Bind(config, TrophyXPDict[config.Key]).Value;
            }
            CanConsumeBosses = Config.Bind(bossConsumption, true).Value;

            Harmony.PatchAll();
        }
    }
}

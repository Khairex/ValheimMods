using System;
using BepInEx;
using HarmonyLib;

namespace PvPNoSkillDrain
{
    [BepInPlugin(Id, "PvP No Skill Drain Mod", "1.0.0")]
    [BepInProcess("valheim.exe")]
    public class PvPNoSkillDrain : BaseUnityPlugin
    {
        public const string Id = "gg.khairex.pvpnoskilldrain";

        public Harmony Harmony { get; } = new Harmony(Id);

        void Awake()
        {
            Harmony.PatchAll();
        }
    }
}

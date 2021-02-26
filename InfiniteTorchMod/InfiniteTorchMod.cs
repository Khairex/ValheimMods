using System;
using BepInEx;
using HarmonyLib;

namespace InfiniteTorchMod
{
    [BepInPlugin(Id, "Infinite Torch Mod", "1.0.0")]
    [BepInProcess("valheim.exe")]
    public class InfiniteTorchMod : BaseUnityPlugin
    {
        public const string Id = "gg.khairex.infinitetorchmod";

        public Harmony Harmony { get; } = new Harmony(Id);

        void Awake()
        {
            Harmony.PatchAll();
        }
    }
}

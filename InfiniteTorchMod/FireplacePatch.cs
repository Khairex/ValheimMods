using System;
using HarmonyLib;

namespace InfiniteTorchMod
{
    [HarmonyPatch(typeof(Fireplace), "Start")]
    class FireplaceStartFuelPatch
    {
        public static void Postfix(Fireplace __instance, ZNetView ___m_nview)
        {
            __instance.m_secPerFuel = 0.000001f;

            if (___m_nview != null)
            {
                ZDO zdo = ___m_nview.GetZDO();
                if (zdo != null)
                {
                    zdo.Set("fuel", __instance.m_maxFuel + 1);
                }
            }
        }
    }

    [HarmonyPatch(typeof(Fireplace), "GetTimeSinceLastUpdate")]
    class FireplaceZeroTimePatch
    {
        public static bool Prefix(double __result)
        {
            __result = 0;
            return false;
        }
    }
}

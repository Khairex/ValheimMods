using System;
using System.Collections.Generic;
using HarmonyLib;

namespace PvPNoSkillDrain
{
    [HarmonyPatch(typeof(Skills), "OnDeath")]
    class SkillsPvPPatch
    {
        public static bool Prefix(Skills __instance)
        {
            Player player = __instance.GetComponent<Player>();
            if (player != null)
            {
                return !player.IsPVPEnabled();
            }
            return true;

        }
    }
}

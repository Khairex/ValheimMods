using System;
using System.Collections.Generic;
using HarmonyLib;
using BepInEx.Configuration;
using UnityEngine;

namespace UsefulTrophies
{
    [HarmonyPatch(typeof(Humanoid), "UseItem")]
    class UseItemPatch
    {
        public static bool Prefix(Humanoid __instance, [HarmonyArgument(0)] Inventory inventory, [HarmonyArgument(1)] ItemDrop.ItemData item, 
            [HarmonyArgument(2)] bool fromInventoryGui, Inventory ___m_inventory, ZSyncAnimation ___m_zanim)
        {
            string itemName = item.m_shared.m_name;

            // Only Override function for Trophy Items
            if (itemName.Contains("$item_trophy_"))
            {
                string enemy = itemName.Substring(13);

                Debug.Log($"Use {enemy} trophy!");

                // Skip Boss Trophy if Unconsumable
                if (!UsefulTrophies.CanConsumeBosses && UsefulTrophies.BossEnemies.Contains(enemy))
                {
                    return true;
                }

                if (inventory == null)
                {
                    inventory = ___m_inventory;
                }
                if (!inventory.ContainsItem(item))
                {
                    return false;
                }
                
                // Prioritize Hover Objects (item stands/altars)
                GameObject hoverObject = __instance.GetHoverObject();
                Hoverable hoverable = hoverObject ? hoverObject.GetComponentInParent<Hoverable>() : null;
                if (hoverable != null && !fromInventoryGui)
                {
                    Interactable componentInParent = hoverObject.GetComponentInParent<Interactable>();
                    if (componentInParent != null && componentInParent.UseItem(__instance, item))
                    {
                        return false;
                    }
                }

                Debug.Log($"Consume {enemy} trophy!");

                // Get a Random Skill from the Player's Skill Pool
                List<Skills.Skill> skills = __instance.GetSkills().GetSkillList();
                 Skills.SkillType randomSkill = skills[UnityEngine.Random.Range(0, skills.Count)].m_info.m_skill;

                float skillFactor = 10f;
                if (UsefulTrophies.TrophyXPDict.TryGetValue(enemy, out float dictSkillFactor))
                {
                    skillFactor = dictSkillFactor;
                }
                else
                {
                    Debug.Log($"Unknown trophy for {enemy}!");
                }

                // Increase Skill by Configured Skill Factor
                __instance.RaiseSkill(randomSkill, skillFactor);
                Debug.Log($"Raised {randomSkill} by {skillFactor}");

                // Consume Item 
                inventory.RemoveOneItem(item);
                __instance.m_consumeItemEffects.Create(Player.m_localPlayer.transform.position, Quaternion.identity, null, 1f, -1);
                ___m_zanim.SetTrigger("eat");

                // Notify Player of the Stat Increase
                __instance.Message(MessageHud.MessageType.Center, $"You feel better at {randomSkill}", 0, null);
                return false;
            }

            return true;
        }
    }
}

using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;
using BuidlingUnleashed;
using UnityEngine;
using HarmonyLib;

namespace BuildingUnleashed
{
    [HarmonyPatch]
    public static class Patches
    {

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Player), nameof(Player.Start))]
        public static void Player_Awake(Player __instance)
        {

            if (Player.m_localPlayer == null) return;
            Player.m_localPlayer.m_placeDelay = Globals.configPlaceDelay.Value > 0f ? Globals.configPlaceDelay.Value : 0f;
            Player.m_localPlayer.m_removeDelay = Globals.configRemoveDelay.Value > 0f ? Globals.configRemoveDelay.Value : 0f;

        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(Player), nameof(Player.UpdatePlacement))]
        static IEnumerable<CodeInstruction> UpdatePlacementTranspiler(IEnumerable<CodeInstruction> instructions)
        {

            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            List<Vector2Int> brackets = new List<Vector2Int>();

            int haveStaminaCount = 0;
            int useStaminaCount = 0;

            for (var i = 0; i < codes.Count; i++)
            {
                MethodInfo method = codes[i].operand as MethodInfo;
                string str = method?.Name;

                // Capture both HaveStamina() Checks
                if (str == "HaveStamina" && haveStaminaCount < 2)
                {
                    brackets.Add(new Vector2Int(i - 5, i + 1));
                    haveStaminaCount++;
                }

                // Capture both UseStamina() and just offset to include rightItem.m_durability stuff
                if (str == "UseStamina" && useStaminaCount < 2)
                {
                    if (useStaminaCount == 0)
                    {
                        brackets.Add(new Vector2Int(i - 5, i + 13));
                        useStaminaCount++;
                    }
                    else if (useStaminaCount == 1)
                    {
                        brackets.Add(new Vector2Int(i - 5, i + 13));
                        useStaminaCount++;

                        // So it doesn't escape to the next else and print a msg_missingrequirement message
                        codes.Insert(i + 13, new CodeInstruction(OpCodes.Ret, null));
                    }
                }

                if (haveStaminaCount == 2 && useStaminaCount == 2)
                    break;
            }

            return RemoveBrackets(codes, brackets).AsEnumerable();

        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(Player), nameof(Player.Repair))]
        static IEnumerable<CodeInstruction> RepairTranspiler(IEnumerable<CodeInstruction> instructions)
        {

            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            List<Vector2Int> brackets = new List<Vector2Int>();

            for (var i = 0; i < codes.Count; i++)
            {
                MethodInfo method = codes[i].operand as MethodInfo;
                string str = method?.Name;

                // Capture both UseStamina() and just offset to include rightItem.m_durability stuff
                if (str == "UseStamina")
                {
                    brackets.Add(new Vector2Int(i - 5, i + 12));
                    break;
                }
            }

            return RemoveBrackets(codes, brackets).AsEnumerable();

        }

        static List<CodeInstruction> RemoveBrackets(List<CodeInstruction> instructions, List<Vector2Int> brackets)
        {

            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);

            for (int i = brackets.Count - 1; i >= 0; i--)
                codes.RemoveRange(brackets[i].x, brackets[i].y - brackets[i].x + 1);

            return codes;

        }
    }

}
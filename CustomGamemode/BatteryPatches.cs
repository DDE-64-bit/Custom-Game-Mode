using HarmonyLib;
using CustomGamemode;
using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace CustomGame
{
    [HarmonyPatch(typeof(Battery))]
    internal static class BatteryPatch
    {
        [HarmonyPatch(nameof(Battery.charge), MethodType.Setter)]
        [HarmonyPrefix]
        public static bool SetChargePrefix(ref float __0, Battery __instance)
        {
            if (CustomGamemode.CustomGamemode.Instance.GetNoBatteryUsage())
            {
                float customChargeValue = GetCustomChargeValue(__instance);
                __0 = Mathf.Min(customChargeValue, __instance.capacity);
            }

            return true;
        }

        private static float GetCustomChargeValue(Battery battery)
        {
            return 500f;
        }
    }
}

using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(RadiatePlayerInRange))]
public class RadiatePlayerInRangePatch
{
    [HarmonyPrefix]
    [HarmonyPatch("Radiate")]
    public static bool PrefixRadiate(RadiatePlayerInRange __instance)
    {
        if (CustomGamemode.CustomGamemode.Instance.GetDisableRadiation())
        {
            Player.main.SetRadiationAmount(0f);
            return false;
        }
        else
        {
            return true;
        }
    }
}
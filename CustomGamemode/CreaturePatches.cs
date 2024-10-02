using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using HarmonyLib.Tools;
using System.Linq;
using UnityEngine;

namespace CustomGame
{
    [HarmonyPatch(typeof(Creature), "Start")]
    public class CreaturePatch
    {
        public static bool Prefix(Creature __instance)
        {
            // Definieer de lijsten van wezens bovenaan
            string[] leviathans = {
                "ReaperLeviathan",
                "GhostLeviathan",
                "SeaDragon",
                "SeaEmperor",
                "SeaTreader",
                "GhostLeviathanJuvenile",
                "SeaDragonJuvenile",
                "SeaEmperorJuvenile",
                "SeaEmperorBaby",
                "GlowWhale",
                "Chelicerate",
                "VoidChelicerate",
                "ShadowLeviathan",
                "IceWorm",
                "Ventgarden",
                "JuvenileVentgarden",
                "Reefback",
                "ReefBack"
            };

            string[] aggressiveCreatures = {
                "Shocker",
                "Biter",
                "Blighter",
                "BoneShark",
                "CrabSnake",
                "CrabSquid",
                "LavaLizard",
                "Mesmer",
                "SpineEel",
                "SandShark",
                "Stalker",
                "Warper",
                "Bleeder",
                "Shuttlebug",
                "CaveCrawler",
                "GhostLeviathan",
                "GhostLeviathanJuvenile",
                "ReaperLeviathan",
                "SeaDragon"
            };

            string[] defensiveCreatures = {
                "Crash",
                "GasPod",
                "Floater",
                "GasoPod",
                "SeaTreader"
            };

            string[] passiveCreatures = {
                "Bladderfish",
                "Boomerang",
                "GhostRayRed",
                "CuteFish",
                "Eyeye",
                "Garryfish",
                "GhostRayBlue",
                "Holefish",
                "Hoopfish",
                "Hoverfish",
                "Jellyray",
                "LavaBoomerang",
                "Oculus",
                "Peeper",
                "RabbitRay",
                "LavaEyeye",
                "Reginald",
                "Skyray",
                "Spadefish",
                "Spinefish",
                "LargeFloater",
                "LavaLarva",
                "RockGrub",
                "Jumper",
                "Reefback",
                "SeaEmperorLeviathan"
            };

            string[] allCreatures = leviathans
                .Concat(aggressiveCreatures)
                .Concat(defensiveCreatures)
                .Concat(passiveCreatures)
                .Distinct() // Zorg ervoor dat er geen duplicaten zijn
                .ToArray();

            string name = __instance.GetType().Name;

            // Controleer de verschillende instellingen en pas het gedrag aan
            if (CustomGamemode.CustomGamemode.Instance.GetDestroyLeviathans() == true)
            {
                if (System.Array.Exists(leviathans, element => element == name))
                {
                    __instance.gameObject.SetActive(false);
                    return false;
                }
            }
            else if (System.Array.Exists(leviathans, element => element == name))
            {
                __instance.gameObject.SetActive(true);
            }

            if (CustomGamemode.CustomGamemode.Instance.GetDestroyAllAggressiveCreatures() == true)
            {
                if (System.Array.Exists(aggressiveCreatures, element => element == name))
                {
                    __instance.gameObject.SetActive(false);
                    return false;
                }
            }
            else if (System.Array.Exists(aggressiveCreatures, element => element == name))
            {
                __instance.gameObject.SetActive(true);
            }

            if (CustomGamemode.CustomGamemode.Instance.GetDestroyAllDefensiveCreatures() == true)
            {
                if (System.Array.Exists(defensiveCreatures, element => element == name))
                {
                    __instance.gameObject.SetActive(false);
                    return false;
                }
            }
            else if (System.Array.Exists(defensiveCreatures, element => element == name))
            {
                __instance.gameObject.SetActive(true);
            }

            if (CustomGamemode.CustomGamemode.Instance.GetDestroyAllPassiveCreatures() == true)
            {
                if (System.Array.Exists(passiveCreatures, element => element == name))
                {
                    __instance.gameObject.SetActive(false);
                    return false;
                }
            }
            else if (System.Array.Exists(passiveCreatures, element => element == name))
            {
                __instance.gameObject.SetActive(true);
            }

            if (CustomGamemode.CustomGamemode.Instance.GetDestroyAllCreatures() == true)
            {
                if (System.Array.Exists(allCreatures, element => element == name))
                {
                    __instance.gameObject.SetActive(false);
                    return false;
                }
            }
            else if (System.Array.Exists(allCreatures, element => element == name))
            {
                __instance.gameObject.SetActive(true);
            }

            return true;
        }
    }
}

using HarmonyLib;
using CustomGamemode;
using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;
using UWE;
using static HandReticle;
using UnityEngine.UI;
using System.Threading;

namespace CustomGame
{
    // Oxygen
    [HarmonyPatch(typeof(Survival), "ResetStats")]
    public class SurvivalPatchResetStats
    {
        public static bool Prefix(Survival __instance)
        {
            // oxygen cheat
            if (CustomGamemode.CustomGamemode.Instance.GetDisableOxygen())
            {
                GameModeUtils.ToggleCheat(GameModeOption.NoOxygen);
            }



            if (CustomGamemode.CustomGamemode.Instance.GetOnlyOneLife())
            {
                GameModeUtils.SetGameMode(GameModeOption.Hardcore, GameModeOption.None);
            }



            // start item cheat
            if (CustomGamemode.CustomGamemode.Instance.GetStartItems())
            {
                if (CustomGamemode.CustomGamemode.Instance.GetUseCustomItems())
                {
                    // Maak een dictionary aan om de configuratieinstelling aan het TechType te koppelen
                    var itemConfigMap = new Dictionary<Func<bool>, TechType[]>
        {
            { CustomGamemode.CustomGamemode.Instance.GetRadiationSuit, new [] { TechType.RadiationSuit, TechType.RadiationGloves, TechType.RadiationHelmet } },
            { CustomGamemode.CustomGamemode.Instance.GetReinforcedDiveSuit, new [] { TechType.ReinforcedDiveSuit } },
            //{ CustomGamemode.CustomGamemode.Instance.GetStillsuit, new [] { TechType.Stillsuit } },
            { CustomGamemode.CustomGamemode.Instance.GetFirstAidKit, new [] { TechType.FirstAidKit } },
            { CustomGamemode.CustomGamemode.Instance.GetFireExtinguisher, new [] { TechType.FireExtinguisher } },
            { CustomGamemode.CustomGamemode.Instance.GetRebreather, new [] { TechType.Rebreather } },
            { CustomGamemode.CustomGamemode.Instance.GetCompass, new [] { TechType.Compass } },
            { CustomGamemode.CustomGamemode.Instance.GetPipe, new [] { TechType.Pipe } },
            { CustomGamemode.CustomGamemode.Instance.GetPipeSurfaceFloater, new [] { TechType.PipeSurfaceFloater } },
            { CustomGamemode.CustomGamemode.Instance.GetScanner, new [] { TechType.Scanner } },
            { CustomGamemode.CustomGamemode.Instance.GetRepairTool, new [] { TechType.Welder } },
            { CustomGamemode.CustomGamemode.Instance.GetFlashlight, new [] { TechType.Flashlight } },
            { CustomGamemode.CustomGamemode.Instance.GetSurvivalKnife, new [] { TechType.Knife } },
            { CustomGamemode.CustomGamemode.Instance.GetPathfinderTool, new [] { TechType.DiveReel } },
            { CustomGamemode.CustomGamemode.Instance.GetAirBladder, new [] { TechType.AirBladder } },
            { CustomGamemode.CustomGamemode.Instance.GetFlare, new [] { TechType.Flare } },
            { CustomGamemode.CustomGamemode.Instance.GetHabitatBuilder, new [] { TechType.Builder } },
            { CustomGamemode.CustomGamemode.Instance.GetLaserCutter, new [] { TechType.LaserCutter } },
            { CustomGamemode.CustomGamemode.Instance.GetStasisRifle, new [] { TechType.StasisRifle } },
            { CustomGamemode.CustomGamemode.Instance.GetPropulsionCannon, new [] { TechType.PropulsionCannon } },
            { CustomGamemode.CustomGamemode.Instance.GetLightStick, new [] { TechType.LEDLight } },
            { CustomGamemode.CustomGamemode.Instance.GetTransfuser, new [] { TechType.Transfuser } },
            { CustomGamemode.CustomGamemode.Instance.GetSeaglide, new [] { TechType.Seaglide } },
            { CustomGamemode.CustomGamemode.Instance.GetMobileVehicleBay, new [] { TechType.Constructor } },
            { CustomGamemode.CustomGamemode.Instance.GetBeacon, new [] { TechType.Beacon } },
            { CustomGamemode.CustomGamemode.Instance.GetWaterproofLocker, new [] { TechType.SmallStorage } },
            { CustomGamemode.CustomGamemode.Instance.GetGravTrap, new [] { TechType.Gravsphere } },
            { CustomGamemode.CustomGamemode.Instance.GetCyclopsDepthModuleMk1, new [] { TechType.CyclopsHullModule1 } },
            { CustomGamemode.CustomGamemode.Instance.GetSeamoth, new [] { TechType.Seamoth } },
            { CustomGamemode.CustomGamemode.Instance.GetCyclops, new [] { TechType.Cyclops } },
            { CustomGamemode.CustomGamemode.Instance.GetPrawnSuit, new [] { TechType.Exosuit } },
            { CustomGamemode.CustomGamemode.Instance.GetNeptuneEscapeRocket, new [] { TechType.RocketBase } },
            { CustomGamemode.CustomGamemode.Instance.GetLithiumIonBattery, new [] { TechType.LithiumIonBattery } },
            { CustomGamemode.CustomGamemode.Instance.GetThermoblade, new [] { TechType.HeatBlade } },
            { CustomGamemode.CustomGamemode.Instance.GetLightweightHighCapacityTank, new [] { TechType.PlasteelTank } },
            { CustomGamemode.CustomGamemode.Instance.GetUltraHighCapacityTank, new [] { TechType.HighCapacityTank } },
            { CustomGamemode.CustomGamemode.Instance.GetUltraGlideFins, new [] { TechType.UltraGlideFins } },
            { CustomGamemode.CustomGamemode.Instance.GetSwimChargeFins, new [] { TechType.SwimChargeFins } },
            { CustomGamemode.CustomGamemode.Instance.GetRepulsionCannon, new [] { TechType.RepulsionCannon } },
            { CustomGamemode.CustomGamemode.Instance.GetCyclopsDepthModuleMk2, new [] { TechType.CyclopsHullModule2 } }
        };

                    // Itereer door de dictionary en voeg items toe aan de inventory als de configuratie-instelling waar is
                    foreach (var itemConfig in itemConfigMap)
                    {
                        if (itemConfig.Key())
                        {
                            foreach (var techType in itemConfig.Value)
                            {
                                CraftData.AddToInventory(techType, 1, true, false);
                            }
                        }
                    }
                }
                else
                {
                    CraftData.AddToInventory(TechType.Builder, 1, true, false);
                    CraftData.AddToInventory(TechType.Floater, 5, true, false);
                    CraftData.AddToInventory(TechType.PropulsionCannon, 1, true, false);
                    CraftData.AddToInventory(TechType.StasisRifle, 1, true, false);
                    CraftData.AddToInventory(TechType.Constructor, 1, true, false);
                    CraftData.AddToInventory(TechType.Flashlight, 1, true, false);
                    CraftData.AddToInventory(TechType.Seaglide, 1, true, false);
                    CraftData.AddToInventory(TechType.Knife, 1, true, false);
                    CraftData.AddToInventory(TechType.Fins, 1, true, false);
                }
            }



            //CraftData.AddToInventory(TechType.Builder, 1, false, true);

            __instance.food = CustomGamemode.CustomGamemode.Instance.GetStartFoodLevel();
            __instance.water = CustomGamemode.CustomGamemode.Instance.GetStartWaterLevel();

            return false;
        }
    }




    [HarmonyPatch(typeof(Survival), "UpdateStats")]
    public class SurvivalPatchUpdateStats
    {
        public static bool Prefix(Survival __instance, float timePassed, ref float __result)
        {
            float num = 0f;
            float prevVal = __instance.food;  // Verplaatst
            float prevVal2 = __instance.water; // Verplaatst

            if (timePassed > 1E-45f)
            {
                if (CustomGamemode.CustomGamemode.Instance.GetDisableFood() && CustomGamemode.CustomGamemode.Instance.GetDisableWater())
                {
                    GameModeUtils.ToggleCheat(GameModeOption.NoSurvival);
                }
                else if (!CustomGamemode.CustomGamemode.Instance.GetDisableFood())
                {
                    float num2 = timePassed / 2520f * 100f;
                    if (num2 > __instance.food)
                    {
                        num += (num2 - __instance.food) * 25f;
                    }
                    __instance.food = Mathf.Clamp(__instance.food - num2, 0f, 200f);
                }
                else if (!CustomGamemode.CustomGamemode.Instance.GetDisableWater())
                {
                    float num3 = timePassed / 1800f * 100f;
                    if (num3 > __instance.water)
                    {
                        num += (num3 - __instance.water) * 25f;
                    }
                    __instance.water = Mathf.Clamp(__instance.water - num3, 0f, 100f);
                }
            }

            // Integratie van UpdateWarningSounds
            if (__instance.food <= 0f && prevVal > 0f)
            {
                if (__instance.foodWarningSounds != null && __instance.foodWarningSounds.Length > 2 && __instance.foodWarningSounds[2] != null)
                {
                    __instance.foodWarningSounds[2].Play();
                }
            }
            else if (__instance.food < 10f && prevVal >= 10f)
            {
                if (__instance.foodWarningSounds != null && __instance.foodWarningSounds.Length > 1 && __instance.foodWarningSounds[1] != null)
                {
                    __instance.foodWarningSounds[1].Play();
                }
            }
            else if (__instance.food < 20f && prevVal >= 20f)
            {
                if (__instance.foodWarningSounds != null && __instance.foodWarningSounds.Length > 0 && __instance.foodWarningSounds[0] != null)
                {
                    __instance.foodWarningSounds[0].Play();
                }
            }

            if (__instance.water <= 0f && prevVal2 > 0f)
            {
                if (__instance.waterWarningSounds != null && __instance.waterWarningSounds.Length > 2 && __instance.waterWarningSounds[2] != null)
                {
                    __instance.waterWarningSounds[2].Play();
                }
            }
            else if (__instance.water < 10f && prevVal2 >= 10f)
            {
                if (__instance.waterWarningSounds != null && __instance.waterWarningSounds.Length > 1 && __instance.waterWarningSounds[1] != null)
                {
                    __instance.waterWarningSounds[1].Play();
                }
            }
            else if (__instance.water < 20f && prevVal2 >= 20f)
            {
                if (__instance.waterWarningSounds != null && __instance.waterWarningSounds.Length > 0 && __instance.waterWarningSounds[0] != null)
                {
                    __instance.waterWarningSounds[0].Play();
                }
            }

            __result = num;
            return false; // Stop de originele methode na het uitvoeren van de aangepaste logica
        }
    }


    [HarmonyPatch(typeof(Survival), "Use")]
    public static class SurvivalPatchUse
    {
        static bool Prefix(Survival __instance, GameObject useObj, ref bool __result)
        {
            // Haal de TechType van het gebruiksobject op
            TechType techType = CraftData.GetTechType(useObj);
            if (techType == TechType.None)
            {
                Pickupable component = useObj.GetComponent<Pickupable>();
                if (component != null)
                {
                    techType = component.GetTechType();
                }
            }

            // Als het een medische kit is, pas de hoeveelheid herstelde gezondheid aan
            if (techType == TechType.FirstAidKit)
            {
                // Haal de gemodificeerde health waarde op
                float medkitHealth = CustomGamemode.CustomGamemode.Instance.GetGetMedkitHealth();

                // Voeg gezondheid toe en controleer of het succesvol was
                if (Player.main.GetComponent<LiveMixin>().AddHealth(medkitHealth) > 0.1f)
                {
                    __result = true;
                }
                else
                {
                    ErrorMessage.AddMessage(Language.main.Get("HealthFull"));
                    __result = false;
                }

                // Speel het gebruiksgeluid af
                FMODUWE.PlayOneShot(TechData.GetSoundUse(techType), Player.main.transform.position, 1f);

                // Return false om de originele methode niet uit te voeren
                return false;
            }

            // Als het geen medische kit is, voer dan de originele methodelogica uit
            return true;
        }
    }


    [HarmonyPatch(typeof(Survival), "UpdateHunger")]
    public static class SurvivalPatchUpdateHunger
    {
        static void Postfix(Survival __instance)
        {
            if (CustomGamemode.CustomGamemode.Instance.GetEnableAutoRegen())
            {
                FieldInfo liveMixinField = typeof(Survival).GetField("liveMixin", BindingFlags.NonPublic | BindingFlags.Instance);
                FieldInfo kUpdateHungerIntervalField = typeof(Survival).GetField("kUpdateHungerInterval", BindingFlags.NonPublic | BindingFlags.Instance);

                LiveMixin liveMixin = (LiveMixin)liveMixinField.GetValue(__instance);
                float kUpdateHungerInterval = (float)kUpdateHungerIntervalField.GetValue(__instance);

                if (GameModeUtils.RequiresSurvival() && !__instance.freezeStats)
                {
                    float regenThresholdFood = CustomGamemode.CustomGamemode.Instance.GetRegenThresholdFood();
                    float regenThresholdWater = CustomGamemode.CustomGamemode.Instance.GetRegenThresholdWater();
                    float tmp = CustomGamemode.CustomGamemode.Instance.GetHealthRegenRate();
                    float healthRegenRate = tmp/10;

                    if (__instance.food >= regenThresholdFood && __instance.water >= regenThresholdWater && liveMixin != null)
                    {
                        liveMixin.AddHealth(healthRegenRate * kUpdateHungerInterval);
                    }
                }
            }
            if (CustomGamemode.CustomGamemode.Instance.GetDamageChallenge())
            {
                FieldInfo liveMixinField = typeof(Survival).GetField("liveMixin", BindingFlags.NonPublic | BindingFlags.Instance);
                FieldInfo kUpdateHungerIntervalField = typeof(Survival).GetField("kUpdateHungerInterval", BindingFlags.NonPublic | BindingFlags.Instance);
                FieldInfo playerField = typeof(Survival).GetField("player", BindingFlags.NonPublic | BindingFlags.Instance);

                LiveMixin liveMixin = (LiveMixin)liveMixinField.GetValue(__instance);
                float kUpdateHungerInterval = (float)kUpdateHungerIntervalField.GetValue(__instance);
                Player player = (Player)playerField.GetValue(__instance);

                if (GameModeUtils.RequiresSurvival() && !__instance.freezeStats && liveMixin != null && player != null)
                {
                    float damagePerInterval = 0.5f; // damage waarde X10

                    if (CustomGamemode.CustomGamemode.Instance.GetDamagePerInterval() == "impossible")
                    {
                        damagePerInterval = 2.5f;
                    }
                    else if (CustomGamemode.CustomGamemode.Instance.GetDamagePerInterval() == "medium")
                    {
                        damagePerInterval = 1.5f;
                    }

                    liveMixin.TakeDamage(damagePerInterval * kUpdateHungerInterval, player.transform.position, DamageType.Normal, null);
                }
            }
        }
    }
}

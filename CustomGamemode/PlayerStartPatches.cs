using HarmonyLib;
using UnityEngine;
using System.Collections;
using UWE;
using System;
using System.Collections.Generic;

namespace CustomGamemode
{
    public static class MobSpawner
    {
        public static bool spawnPassiveMobs = false;
        public static bool spawnAggressiveMobs = false;
        public static bool spawnLeviathans = false;

        private static readonly Dictionary<int, (TechType techType, string spawnName, string category)> MobSpawnTable = new Dictionary<int, (TechType, string, string)>()
        {
            { 1, (TechType.Shocker, "shocker", "aggressive") },
            { 2, (TechType.Biter, "biter", "aggressive") },
            { 3, (TechType.Blighter, "blighter", "aggressive") },
            { 4, (TechType.BoneShark, "boneshark", "aggressive") },
            { 5, (TechType.Crabsnake, "crabsnake", "aggressive") },
            { 6, (TechType.CrabSquid, "crabsquid", "aggressive") },
            { 7, (TechType.Crash, "crash", "aggressive") },
            { 8, (TechType.LavaLizard, "lavalizard", "aggressive") },
            { 9, (TechType.Mesmer, "mesmer", "aggressive") },
            { 10, (TechType.ReaperLeviathan, "reaperleviathan", "leviathan") },
            { 11, (TechType.SeaDragon, "seadragon", "leviathan") },
            { 12, (TechType.Sandshark, "sandshark", "aggressive") },
            { 13, (TechType.Stalker, "stalker", "aggressive") },
            { 14, (TechType.Warper, "warper", "aggressive") },
            { 15, (TechType.Bladderfish, "bladderfish", "passive") },
            { 16, (TechType.Boomerang, "boomerang", "passive") },
            { 17, (TechType.GhostRayRed, "ghostrayred", "passive") },
            { 18, (TechType.Cutefish, "cutefish", "passive") },
            { 19, (TechType.Eyeye, "eyeye", "passive") },
            { 20, (TechType.GarryFish, "garryfish", "passive") },
            { 21, (TechType.Gasopod, "gasopod", "passive") },
            { 22, (TechType.GhostRayBlue, "ghostrayblue", "passive") },
            { 23, (TechType.HoleFish, "holefish", "passive") },
            { 24, (TechType.Hoopfish, "hoopfish", "passive") },
            { 25, (TechType.Hoverfish, "hoverfish", "passive") },
            { 26, (TechType.Jellyray, "jellyray", "passive") },
            { 27, (TechType.LavaBoomerang, "lavaboomerang", "passive") },
            { 28, (TechType.Oculus, "oculus", "passive") },
            { 29, (TechType.Peeper, "peeper", "passive") },
            { 30, (TechType.RabbitRay, "rabbitray", "passive") },
            { 31, (TechType.LavaEyeye, "lavaeyeye", "passive") },
            { 32, (TechType.Reefback, "reefback", "leviathan") },
            { 33, (TechType.Reginald, "reginald", "passive") },
            { 34, (TechType.SeaTreader, "seatreader", "leviathan") },
            { 35, (TechType.Spadefish, "spadefish", "passive") },
            { 36, (TechType.Spinefish, "spinefish", "passive") },
            { 37, (TechType.Bleeder, "bleeder", "aggressive") },
            { 38, (TechType.Shuttlebug, "shuttlebug", "passive") },
            { 39, (TechType.CaveCrawler, "cavecrawler", "aggressive") },
            { 40, (TechType.Floater, "floater", "passive") },
            { 41, (TechType.LavaLarva, "lavalarva", "aggressive") },
            { 42, (TechType.Rockgrub, "rockgrub", "passive") },
            { 43, (TechType.Jumper, "jumper", "passive") }
        };

        public static IEnumerator SpawnMob(string spawnName, int quantity = 1, float maxDist = 15f) // afstand
        {
            TechType techType;
            if (TechTypeExtensions.FromString(spawnName, out techType, true))
            {
                if (CraftData.IsAllowed(techType))
                {
                    CoroutineTask<GameObject> request = CraftData.GetPrefabForTechTypeAsync(techType, true);
                    yield return request;
                    GameObject result = request.GetResult();

                    if (result != null)
                    {
                        for (int i = 0; i < quantity; i++)
                        {
                            GameObject gameObject = global::Utils.CreatePrefab(result, maxDist, i > 0);
                            LargeWorldEntity.Register(gameObject);
                            CrafterLogic.NotifyCraftEnd(gameObject, techType);
                            //ErrorMessage.AddDebug(string.Format($"Spawned: {gameObject} {techType}"));
                            gameObject.SendMessage("StartConstruction", SendMessageOptions.DontRequireReceiver);
                        }
                    }
                    else
                    {
                        Debug.LogError($"Could not find prefab for TechType = {techType}");
                    }
                }
                else
                {
                    Debug.LogError($"TechType {techType} is not allowed");
                }
            }
            else
            {
                Debug.LogError($"Could not parse {spawnName} as TechType");
            }
        }

        public static IEnumerator SpawnMobsPeriodically(int interval = 30) //interval
        {
            while (true)
            {
                System.Random random = new System.Random();
                int randomIndex = random.Next(1, MobSpawnTable.Count + 1);
                var mobData = MobSpawnTable[randomIndex];

                // Check the category and see if it matches with any of the enabled categories
                if ((mobData.category == "passive" && spawnPassiveMobs) ||
                    (mobData.category == "aggressive" && spawnAggressiveMobs) ||
                    (mobData.category == "leviathan" && spawnLeviathans))
                {
                    yield return CoroutineHost.StartCoroutine(SpawnMob(mobData.spawnName, 1, 15f));
                }

                yield return new WaitForSeconds(interval);
            }
        }
    }

    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("Update")]
    public static class PlayerStartPatch
    {
        private static bool isSpawningCoroutineRunning = false;

        static void Postfix()
        {
            // Configure which types of mobs should spawn
            MobSpawner.spawnPassiveMobs = CustomGamemode.Instance.GetUsePassiveCreatures();    // Allow passive mobs to spawn
            MobSpawner.spawnAggressiveMobs = CustomGamemode.Instance.GetUseAgressiveCreatures(); // Allow aggressive mobs to spawn
            MobSpawner.spawnLeviathans = CustomGamemode.Instance.GetUseLeviathans();     // Allow leviathans to spawn

            if (CustomGamemode.Instance.GetCreatureChallenge() && !isSpawningCoroutineRunning)
            {
                CoroutineHost.StartCoroutine(MobSpawner.SpawnMobsPeriodically());
                isSpawningCoroutineRunning = true;
            }
        }
    }
}

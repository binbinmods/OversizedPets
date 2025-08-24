using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
// using static Obeliskial_Essentials.Essentials;
using System;
using static OversizedPets.Plugin;
using static OversizedPets.CustomFunctions;
using static OversizedPets.OversizedPetsFunctions;
using System.Collections.Generic;
using static Functions;
using UnityEngine;
// using Photon.Pun;
using TMPro;
using System.Linq;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Diagnostics;
// using Unity.TextMeshPro;

// Make sure your namespace is the same everywhere
namespace OversizedPets
{

    [HarmonyPatch] // DO NOT REMOVE/CHANGE - This tells your plugin that this is part of the mod

    public class OversizedPetsPatches
    {
        public static bool devMode = false; //DevMode.Value;
        public static bool bSelectingPerk = false;





        [HarmonyPostfix]
        [HarmonyPatch(typeof(Character), "BeginRound")]
        public static void BeginRoundPostfix(ref Character __instance)
        {
            bool isValidHero = __instance != null && __instance.HeroData != null && __instance.HeroData.HeroSubClass != null && __instance.Alive && __instance.HeroItem != null && __instance.HeroItem.animatedTransform != null;
            bool isValidNPC = __instance != null && __instance.NpcData != null && __instance.Alive && __instance.NPCItem != null && __instance.NPCItem.animatedTransform != null;
            if (isValidHero)
            {
                string subclassID = __instance.HeroData.HeroSubClass.Id;
                if (IncreaseHeroSize.Value)
                {
                    float sizeIncrease = HeroSizeScaleFactor.Value;
                    ResizeTransform(ref __instance.HeroItem.animatedTransform, sizeIncrease, subclassID);

                }

                if (IncreasePetSize.Value && __instance.Pet != null)
                {
                    string petID = __instance.Pet;
                    float sizeIncrease = PetSizeScaleFactor.Value;

                    if (__instance.HeroItem.PetItem != null && __instance.HeroItem.PetItem.animatedTransform != null)
                    {
                        ResizeTransform(ref __instance.HeroItem.PetItem.animatedTransform, sizeIncrease, petID);
                    }
                    if (__instance.HeroItem.PetItemEnchantment != null && __instance.HeroItem.PetItemEnchantment.animatedTransform != null)
                    {
                        ResizeTransform(ref __instance.HeroItem.PetItemEnchantment.animatedTransform, sizeIncrease, petID);
                    }
                }

            }
            else if (IncreaseNPCSize.Value && isValidNPC)
            {
                float sizeIncrease = NPCSizeScaleFactor.Value;
                ResizeTransform(ref __instance.NPCItem.animatedTransform, sizeIncrease, __instance.NpcData.Id);
                if (IncreasePetSize.Value && __instance.Pet != null)
                {
                    string petID = __instance.Pet;
                    float sizeIncreasePet = PetSizeScaleFactor.Value;

                    if (__instance.NPCItem.PetItem != null && __instance.NPCItem.PetItem.animatedTransform != null)
                    {
                        ResizeTransform(ref __instance.NPCItem.PetItem.animatedTransform, sizeIncreasePet, petID);
                    }
                    if (__instance.NPCItem.PetItemEnchantment != null && __instance.NPCItem.PetItemEnchantment.animatedTransform != null)
                    {
                        ResizeTransform(ref __instance.NPCItem.PetItemEnchantment.animatedTransform, sizeIncreasePet, petID);
                    }
                }
            }
        }


    }
}
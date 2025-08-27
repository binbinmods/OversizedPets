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


        // [HarmonyPostfix]
        // [HarmonyPatch(typeof(Character), "BeginMatch")]
        public static void BeginMatchPostfix(ref Character __instance)
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

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MatchManager), "CreatePet")]

        public static void CreatePetPostfix(ref CardData cardPet, ref GameObject charGO, ref Hero _hero, ref NPC _npc, bool _fromEnchant = false, string _enchantName = "")
        {
            if (!IncreasePetSize.Value || cardPet == null || charGO == null || (_hero == null && _npc == null))
                return;
            float sizeIncreasePet = PetSizeScaleFactor.Value;
            if (cardPet.Id.StartsWith("sharpy") || cardPet.Id.StartsWith("harley") || cardPet.Id.StartsWith("inky"))
            {
                cardPet.PetOffset = new Vector2(cardPet.PetOffset.x, cardPet.PetOffset.y - cardPet.PetSize.y * (sizeIncreasePet * (cardPet.Id.StartsWith("inky") ? 1.5f : 1) - 1));
            }
            cardPet.PetSize *= sizeIncreasePet;
            // LogDebug("Creating pet " + cardPet.Id + " for " + (_hero != null ? _hero.HeroData.Id : _npc.NpcData.Id) + ", size increase: " + sizeIncreasePet.ToString());
            // if (_hero != null)
            // {

            //     if (!_fromEnchant)
            //     {
            //         ResizeTransform(ref _hero.HeroItem.PetItem.animatedTransform, sizeIncreasePet, cardPet.Id);
            //         // _hero.HeroItem.PetItemFront = cardPet.PetFront;
            //         // ResizeTransform(ref _hero.HeroItem.PetItemFront.animatedTransform, sizeIncreasePet, cardPet.Id);
            //     }
            //     else
            //     {
            //         // _hero.HeroItem.PetItemEnchantment = npcItem;
            //         ResizeTransform(ref _hero.HeroItem.PetItemEnchantment.animatedTransform, sizeIncreasePet, cardPet.Id);
            //         // _hero.HeroItem.PetItemEnchantmentFront = cardPet.PetFront;
            //     }
            //     _hero.HeroItem.DrawOrderSprites(true, _hero.Position * 2);
            // }
            // else
            // {
            //     if (_npc == null)
            //         return;
            //     if (!_fromEnchant)
            //     {
            //         ResizeTransform(ref _npc.NPCItem.PetItem.animatedTransform, sizeIncreasePet, cardPet.Id);
            //         // _hero.HeroItem.PetItemFront = cardPet.PetFront;
            //         // ResizeTransform(ref _hero.HeroItem.PetItemFront.animatedTransform, sizeIncreasePet, cardPet.Id);
            //     }
            //     else
            //     {
            //         // _hero.HeroItem.PetItemEnchantment = npcItem;
            //         ResizeTransform(ref _npc.NPCItem.PetItemEnchantment.animatedTransform, sizeIncreasePet, cardPet.Id);
            //         // _hero.HeroItem.PetItemEnchantmentFront = cardPet.PetFront;
            //     }
            //     _npc.NPCItem.DrawOrderSprites(true, _npc.Position * 2);
            // }
        }


    }
}
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
// using Obeliskial_Content;
// using Obeliskial_Essentials;
using System.IO;
using static UnityEngine.Mathf;
using UnityEngine.TextCore.LowLevel;
using static OversizedPets.Plugin;
using System.Collections.ObjectModel;
using UnityEngine;

namespace OversizedPets
{
    public class OversizedPetsFunctions
    {

        public static bool IsHost()
        {
            return GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster();
        }

        public static void ResizeTransform(ref Transform transform, float scale, string id = "")
        {
            transform.localScale = new Vector3(transform.localScale.x * scale, transform.localScale.y * scale, transform.localScale.z);
            LogDebug("Setting " + id + " size to: " + scale.ToString());
        }

    }
}


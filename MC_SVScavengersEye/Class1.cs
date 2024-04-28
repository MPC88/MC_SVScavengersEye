using HarmonyLib;
using BepInEx;
using UnityEngine;

namespace MC_SVScavengersEye
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class Main : BaseUnityPlugin
    {
        public const string pluginGuid = "mc.starvalor.scavengerseye";
        public const string pluginName = "SV Scavenger's Eye";
        public const string pluginVersion = "1.0.0";

        public void Awake()
        {
            Harmony.CreateAndPatchAll(typeof(Main));
        }

        [HarmonyPatch(typeof(GameManager), nameof(GameManager.SpawnDebrisField))]
        [HarmonyPostfix]
        private static void SparkMoar(DebrisField debrisField)
        {
            if (debrisField.type == 0)
                return;

            Transform effects = debrisField.debrisFieldControl.gameObject.transform.Find("Effects");
            if (effects == null)
                return;

            effects.localScale *= 1 + (PChar.Char.explorer / 10);
        }
    }
}

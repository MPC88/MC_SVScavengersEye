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
        public const string pluginVersion = "1.0.1";

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

            float scale = (PChar.Char.explorer / 10);
                        
            if(scale > 3 && PlayerControl.inst.GetSpaceShip.shipClass < 2)
                scale = 3;
            if(scale > 4 && PlayerControl.inst.GetSpaceShip.shipClass < 5)
                scale = 4;

            effects.localScale *= 1 + scale;
        }
    }
}

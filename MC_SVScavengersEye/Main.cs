using HarmonyLib;
using BepInEx;
using UnityEngine;
using BepInEx.Configuration;

namespace MC_SVScavengersEye
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class Main : BaseUnityPlugin
    {
        public const string pluginGuid = "mc.starvalor.scavengerseye";
        public const string pluginName = "SV Scavenger's Eye";
        public const string pluginVersion = "1.1.0";

        public static ConfigEntry<int> cfgShutYacMaxScale;
        public static ConfigEntry<int> cfgCorFrigMaxScale;
        public static ConfigEntry<int> cfgCruDreMaxScale;
        public static ConfigEntry<int> cfgExplorerDiv;

        public void Awake()
        {
            Harmony.CreateAndPatchAll(typeof(Main));

            Main.cfgExplorerDiv = Config.Bind<int>(
                "Scaling",
                "Explorer Knowledge Divisor",
                12,
                "Affects rate at which max scaling is reached.  Smaller value => faster increase.");
            Main.cfgShutYacMaxScale = Config.Bind<int>(
                "Scaling",
                "Shuttle / Yacht Max Scale",
                2,
                "Max scaling for shuttles and yachts.");
            Main.cfgCorFrigMaxScale = Config.Bind<int>(
                "Scaling",
                "Corvette / Frigate Max Scale",
                3,
                "Max scaling for corvettes and frigates.");
            Main.cfgCruDreMaxScale = Config.Bind<int>(
                "Scaling",
                "Cruiser / Dread Max Scale",
                4,
                "Max scaling for cruisers and dreads.");
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

            float scale = (PChar.Char.explorer / Main.cfgExplorerDiv.Value);

            if (scale > Main.cfgShutYacMaxScale.Value && PlayerControl.inst.GetSpaceShip.shipClass < 3)
                scale = Main.cfgShutYacMaxScale.Value;
            if (scale > Main.cfgCorFrigMaxScale.Value && PlayerControl.inst.GetSpaceShip.shipClass < 5)
                scale = Main.cfgCorFrigMaxScale.Value;
            if (scale > Main.cfgCruDreMaxScale.Value && PlayerControl.inst.GetSpaceShip.shipClass < 7)
                scale = Main.cfgCruDreMaxScale.Value;

            effects.localScale *= 1 + scale;
        }
    }
}

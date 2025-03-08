using System;
using System.Reflection;
using HarmonyLib;
using GorillaLocomotion;

namespace SpeedBoard
{
    public class HarmonyPatches
    {
        private static Harmony instance;

        public static bool IsPatched { get; private set; }
        public const string InstanceId = PluginInfo.GUID;

        internal static void ApplyHarmonyPatches()
        {
            if (!IsPatched)
            {
                if (instance == null)
                {
                    instance = new Harmony(InstanceId);
                }


                instance.PatchAll(Assembly.GetExecutingAssembly());


                PatchPlayerUpdate();

                IsPatched = true;
            }
        }

        internal static void RemoveHarmonyPatches()
        {
            if (instance != null && IsPatched)
            {
                instance.UnpatchSelf();
                IsPatched = false;
            }
        }

        private static void PatchPlayerUpdate()
        {

            var originalMethod = AccessTools.Method(typeof(Player), "Update");
            var postfixMethod = AccessTools.Method(typeof(Player_Update_Patch), "Postfix");


            instance.Patch(originalMethod, postfix: new HarmonyMethod(postfixMethod));
        }
    }


    public static class Player_Update_Patch
    {
        public static void Postfix(ref Player __instance)
        {
            if (Plugin.inRoom)
            {
                Traverse.Create(__instance).Field("hoverboardPaddleBoostMax").SetValue(1000);
                Traverse.Create(__instance).Field("hoverboardPaddleBoostMultiplier").SetValue(0.15f);
                Traverse.Create(__instance).Field("hoverMaxPaddleSpeed").SetValue(999);           
            }
            else
            {
                Traverse.Create(__instance).Field("hoverboardPaddleBoostMax").SetValue(10);
                Traverse.Create(__instance).Field("hoverboardPaddleBoostMultiplier").SetValue(0.1f);
                Traverse.Create(__instance).Field("hoverMaxPaddleSpeed").SetValue(35);
            }
        }
    }


}

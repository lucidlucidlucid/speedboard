using System;
using BepInEx;
using GorillaLocomotion;
using HarmonyLib;
using Utilla.Attributes;
using UnityEngine;

namespace SpeedBoard
{
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        // Make inRoom static and accessible via a public getter
        public static bool inRoom { get; private set; }

        void Start()
        {
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
        }

        void OnDisable()
        {
            HarmonyPatches.RemoveHarmonyPatches();
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            // Code after the game initializes
        }

        void Update()
        {
            if (inRoom)
            {
                ApplyBoostValues();
            }
        }

        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            inRoom = true;
            ApplyBoostValues();
        }

        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            inRoom = false;
            ResetBoostValues();
        }

        private static void ApplyBoostValues()
        {
            try
            {
                Player.Instance.GetType().GetField("hoverboardPaddleBoostMax", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(Player.Instance, 1000);
                Player.Instance.GetType().GetField("hoverboardPaddleBoostMultiplier", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(Player.Instance, 0.5f);
                Player.Instance.GetType().GetField("hoverMaxPaddleSpeed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(Player.Instance, 999);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error applying boost values: " + ex.Message);
            }
        }

        private static void ResetBoostValues()
        {
            try
            {
                Player.Instance.GetType().GetField("hoverboardPaddleBoostMax", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(Player.Instance, 10);
                Player.Instance.GetType().GetField("hoverboardPaddleBoostMultiplier", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(Player.Instance, 0.1f);
                Player.Instance.GetType().GetField("hoverMaxPaddleSpeed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(Player.Instance, 35);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error resetting boost values: " + ex.Message);
            }
        }
    }
}

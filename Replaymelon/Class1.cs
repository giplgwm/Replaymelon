using System;
using System.Runtime.CompilerServices;
using MelonLoader;
using UnityEngine;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Replaymelon
{
    public class ReplayMenu : MelonMod
    {
        public Rect windowRect = new Rect(20, 20, 150, 250);
        public static bool infiniteJumpEnabled = false;
        public static bool noSpikeDmg = false;
        public static float player_speed = 6f;
        
        public override void OnUpdate()
        {
            //
        }

        public override void OnGUI()
        {
            windowRect = GUI.Window(420024, windowRect, makeGUIWork, "RePlay Cheat Menu");
            
        }

        public void makeGUIWork(int windowID)
        {
            infiniteJumpEnabled = GUI.Toggle(new Rect(20, 20, 100, 30), infiniteJumpEnabled, "Infinite Jump");
            if (GUI.Button(new Rect(20, 50, 100, 30), "Teleport to Flag"))
            {
                tpPlayerToFlag();
            }
            player_speed = GUI.HorizontalSlider(new Rect(20, 90, 100, 30), player_speed, 0.0f, 100f);
            GUI.Label(new Rect(20, 100, 150, 30), "Player speed: " +  player_speed);
            noSpikeDmg = GUI.Toggle(new Rect(20, 120, 100, 30), noSpikeDmg, "No Spike Dmg");
        }

        public void tpPlayerToFlag()
        {
            GameObject ghost = GameObject.Find("Ghost");
            GameObject flag = GameObject.Find("Flag");
            ghost.transform.position = flag.transform.position;
        }

    }

    [HarmonyPatch(typeof(PlayerMovement), "Jump")]
    public static class JumpPatch
    {
        public static bool Prefix(ref int ___jumpsRemaining)
        {
            if (ReplayMenu.infiniteJumpEnabled)
            {
                ___jumpsRemaining += 3;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(PlayerMovement), "Update")]
    public static class SpeedPatch
    {
        public static bool Prefix(ref Vector2 ___maxVelocity, ref float ___playerSpeed)
        {
            float p_speed = ReplayMenu.player_speed;
            ___maxVelocity = new Vector2(p_speed, 15f);
            ___playerSpeed = p_speed;
            return true;
        }
    }

    [HarmonyPatch(typeof(PlayerMovement), "OnTriggerEnter2D")]
    public static class SpikePatch
    {
        public static bool Prefix()
        {
            return !ReplayMenu.noSpikeDmg;
        }
    }

}

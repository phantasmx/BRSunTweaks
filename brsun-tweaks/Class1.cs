using BepInEx;
using HarmonyLib;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using BR.General;
using BR.UI;
using ADV;
using System;
using System.IO;
using Shand;
using BepInEx.Unity.IL2CPP.UnityEngine;
using BepInEx.Unity.IL2CPP;

namespace brsun_tweaks
{
    [BepInPlugin("mod.spectre.brsuntweaks", "BRSun Tweaks", "1.04")]
    public class tweaks : BasePlugin
    {
        public override void Load()
        {
            Harmony harmony = new Harmony("mod.spectre.brsuntweaks");
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(StageResolutionCamera))]
    public static class StageResolutionCameraPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch("Update")]
        public static void Update()
        {
            //Framerate 60 Fix
            Parameter.TargetFrameRateType = Parameter.FrameRateType.FPS60;

            //Fullscreen
            if (Screen.fullScreen == false)
            {
                Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, true);
                Screen.fullScreen = true;
            }
            
            //FOV
            if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.UpArrow))
            {
                fov = fov - 0.5f;
            }
            if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.DownArrow))
            {
                fov = fov + 0.5f;
            }
            if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.LeftArrow))
            {
                fov = 20;
            }
            if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.RightArrow))
            {
                fov = 40;
            }
            GameObject cam = GameObject.Find("CameraPosition/Main Camera");
            if (cam)
            {
                cam.GetComponent<Camera>().fieldOfView = fov;

                DepthOfField depthOfField = null;
                VolumeProfile profile = cam.GetComponent<StageResolutionCamera>().Volume_stage.profile;
                if (profile.TryGet<DepthOfField>(out depthOfField))
                {
                    depthOfField.active = false;
                    depthOfField.mode.value = DepthOfFieldMode.Off;
                }

                profile = cam.GetComponent<StageResolutionCamera>().Volume_chara.profile;
                if (profile.TryGet<DepthOfField>(out depthOfField))
                {
                    depthOfField.active = false;
                    depthOfField.mode.value = DepthOfFieldMode.Off;
                }
            }
        }
        private static float fov = 40;
    }





}

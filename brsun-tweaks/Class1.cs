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
using BepInEx.Configuration;

namespace brsun_tweaks
{
    [BepInPlugin("mod.spectre.brsuntweaks", "BRSun Tweaks", "1.06")]
    public class tweaks : BasePlugin
    {
        public override void Load()
        {
            OConfig.InitializeConfig(base.Config);
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
            if (UnityEngine.Input.GetKeyUp(UnityEngine.KeyCode.F11))
            {
                forceFullScreen = !forceFullScreen;
                OConfig.SetForceFullScreen(forceFullScreen);
                Screen.fullScreen = forceFullScreen;
                if (!forceFullScreen)
                {
                    Screen.SetResolution(Display.main.systemWidth - 80, Display.main.systemHeight - 120, false);
                    Screen.fullScreen = false;
                }
                else
                {
                    Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, true);
                    Screen.fullScreen = true;
                }
            }
            if (forceFullScreen && !Screen.fullScreen)
            {
                Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, true);
                Screen.fullScreen = true;
            }
            
            //FOV
            if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.UpArrow))
            {
                fov = fov - 0.5f;
                OConfig.SetFov(fov);
            }
            if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.DownArrow))
            {
                fov = fov + 0.5f;
                OConfig.SetFov(fov);
            }
            if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.LeftArrow))
            {
                fov = 20;
                OConfig.SetFov(fov);
            }
            if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.RightArrow))
            {
                fov = 40;
                OConfig.SetFov(fov);
            }


            if (UnityEngine.Input.GetKeyUp(UnityEngine.KeyCode.F10))
            {
                disableDOF = !disableDOF;
                OConfig.SetDisableDOF(disableDOF);
            }


            GameObject cam = GameObject.Find("CameraPosition/Main Camera");
            if (cam)
            {
                cam.GetComponent<Camera>().fieldOfView = fov;

                if (disableDOF)
                {
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
                else
                {
                    DepthOfField depthOfField = null;
                    VolumeProfile profile = cam.GetComponent<StageResolutionCamera>().Volume_stage.profile;
                    if (profile.TryGet<DepthOfField>(out depthOfField))
                    {
                        depthOfField.active = true;
                        depthOfField.mode.value = DepthOfFieldMode.Bokeh;
                    }

                    profile = cam.GetComponent<StageResolutionCamera>().Volume_chara.profile;
                    if (profile.TryGet<DepthOfField>(out depthOfField))
                    {
                        depthOfField.active = true;
                        depthOfField.mode.value = DepthOfFieldMode.Bokeh;
                    }
                }
                
            }
        }
        private static float fov = OConfig.Fov();
        private static bool forceFullScreen = OConfig.ForceFullScreen();
        private static bool disableDOF = OConfig.DisableDOF();
    }


    internal static class OConfig
    {
        public static void InitializeConfig(ConfigFile currConfig)
        {
            OConfig.config = currConfig;
            
            OConfig.fov = OConfig.config.Bind<float>("Field of View", "Set FOV value", 40, new ConfigDescription("FOV", null, new object[0]));

            OConfig.disableDOF = OConfig.config.Bind<bool>("Disable Depth of Field", "Disable DOF", true, new ConfigDescription("Disable DOF", null, new object[0]));

            OConfig.forceFullScreen = OConfig.config.Bind<bool>("Force FullScreen", "Force FullScreen", true, new ConfigDescription("Force FullScreen", null, new object[0]));

            
        }

        public static float Fov()
        {
            return OConfig.fov.Value;
        }

        public static void SetFov(float fov)
        {
            OConfig.fov.Value = fov;
        }

        public static bool DisableDOF()
        {
            return OConfig.disableDOF.Value;
        }

        public static void SetDisableDOF(bool disableDOF)
        {
            OConfig.disableDOF.Value = disableDOF;
        }

        public static bool ForceFullScreen()
        {
            return OConfig.forceFullScreen.Value;
        }

        public static void SetForceFullScreen(bool forceFullScreen)
        {
            OConfig.forceFullScreen.Value = forceFullScreen;
        }

        public static ConfigFile config;

        private static ConfigEntry<float> fov;
        private static ConfigEntry<bool> disableDOF;
        private static ConfigEntry<bool> forceFullScreen;
    }


}

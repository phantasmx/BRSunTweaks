using BepInEx;
using HarmonyLib;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using BR.General;
using BR.UI;

namespace brsun_tweaks
{
    [BepInPlugin("mod.spectre.brsuntweaks", "BRSun Tweaks", "1.2")]
    public class tweaks : BaseUnityPlugin
    {
        private void Awake()
        {
            Harmony harmony = new Harmony("mod.spectre.brsuntweaks");
            harmony.PatchAll();
        }

        private void Update()
        {
            //Framerate 60 Fix
            Parameter.TargetFrameRateType = Parameter.FrameRateType.FPS60;

            //FOV
            if (Input.GetKey(KeyCode.UpArrow))
            {
                fov = fov - 0.5f;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                fov = fov + 0.5f;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                fov = 20;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                fov = 40;
            }
            Camera.main.fieldOfView = fov;
        }

        private float fov = 40;
    }



    
    [HarmonyPatch(typeof(StageResolutionCamera), "DepthOfField")]
    public class disableDof
    {
        [HarmonyPostfix]
        public static void noDof()
        {
            GameObject cam = GameObject.Find("CameraPosition/Main Camera");
            if (cam)
            {
                DepthOfField depthOfField = null;
                VolumeProfile profile = cam.GetComponent<StageResolutionCamera>().Volume_stage.profile;
                if (profile.TryGet<DepthOfField>(out depthOfField))
                {
                    depthOfField.active = false;
                }
                profile = cam.GetComponent<StageResolutionCamera>().Volume_chara.profile;
                if (profile.TryGet<DepthOfField>(out depthOfField))
                {
                    depthOfField.active = false;
                }
            }
        }
    }


    [HarmonyPatch(typeof(GameQualitySettings), "SetResolution", typeof(float))]
    public class resolutionFix
    {
        [HarmonyPrefix]
        public static bool resFix()
        {
            Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, true);
            return false;
        }
    }
}

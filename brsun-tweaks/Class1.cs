using BepInEx;
using HarmonyLib;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using BR.General;

namespace brsun_tweaks
{
    [BepInPlugin("mod.spectre.brsuntweaks", "BRSun Tweaks", "1.0")]
    public class tweaks : BaseUnityPlugin
    {
        private void Awake()
        {
            Harmony harmony = new Harmony("mod.spectre.brsuntweaks");
            harmony.PatchAll();
        }

        private void Update()
        {
            //Set main camera FoV
            GameObject cam = GameObject.Find("CameraPosition/Main Camera");
            if (cam)
            {
                cam.GetComponent<Camera>().fieldOfView = 40f;
            }
        }
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

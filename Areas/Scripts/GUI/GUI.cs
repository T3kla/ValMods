using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jotunn.Configs;
using Jotunn.Managers;
using Jotunn.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Areas
{
    public static class GUI
    {
        public static GameObject panel;
        public static GameObject btnCS;
        public static ButtonConfig ShowGUIButton;

        public static void Awake()
        {
            GUIManager.OnPixelFixCreated += OnPixelFix;

            ShowGUIButton = new ButtonConfig { Name = "AGUI_Keybinding", Config = Globals.Config.AGUIKeybinding };
            InputManager.Instance.AddButton(Main.GUID, ShowGUIButton);

            var bundle = AssetUtils.LoadAssetBundle(Globals.Path.AssetBundle);
            var contents = bundle.GetAllAssetNames();

            var reqMain = bundle.LoadAssetAsync<GameObject>("Panel");
            var reqBtn = bundle.LoadAssetAsync<GameObject>("BtnCS");
            reqMain.completed += (ao) => { panel = reqMain.asset as GameObject; };
            reqBtn.completed += (ao) => { btnCS = reqBtn.asset as GameObject; };
        }

        private static void OnPixelFix()
        {
            var scene = SceneManager.GetActiveScene();
            if (scene.name != "main") return;
            if (panel == null || btnCS == null) { Main.GLog.LogError($"Areas GUI assets didn't load correctly!"); return; }

            Transform pf = GUIManager.PixelFix.transform;
            var x = GameObject.Instantiate(panel, pf.position, Quaternion.identity, pf);
        }

    }
}

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
    public static class AGUI
    {
        public static GameObject lol;
        public static GameObject p_areasMain;
        public static GameObject p_btnSpawner;
        public static ButtonConfig ShowGUIButton;

        public static void Awake()
        {
            GUIManager.OnPixelFixCreated += OnPixelFix;

            ShowGUIButton = new ButtonConfig { Name = "AGUI_Keybinding", Config = Globals.Config.AGUIKeybinding };
            InputManager.Instance.AddButton(Main.GUID, ShowGUIButton);

            var bundle = AssetUtils.LoadAssetBundle(Globals.Path.AssetBundle);
            var contents = bundle.GetAllAssetNames();
            Main.GLog.LogInfo($"Loaded AssetBundle {bundle} with assets: ");
            foreach (var asset in contents)
                Main.GLog.LogInfo($"    {asset}");

            var reqMain = bundle.LoadAssetAsync<GameObject>("Areas Main");
            var reqBtn = bundle.LoadAssetAsync<GameObject>("Button Spawner");
            reqMain.completed += (ao) => { p_areasMain = reqMain.asset as GameObject; };
            reqBtn.completed += (ao) => { p_btnSpawner = reqBtn.asset as GameObject; };
        }

        private static void OnPixelFix()
        {
            var scene = SceneManager.GetActiveScene();
            if (scene.name != "main") return;
            if (p_areasMain == null) { Main.GLog.LogError($"\"p_areasMain\" wasn't loaded correctly"); return; }
            if (p_btnSpawner == null) { Main.GLog.LogError($"\"p_btnSpawner\" wasn't loaded correctly"); return; }

            Transform pf = GUIManager.PixelFix.transform;
            var x = GameObject.Instantiate(p_areasMain, pf.position, Quaternion.identity, pf);
        }

    }
}

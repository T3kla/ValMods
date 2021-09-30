using Jotunn.Configs;
using Jotunn.Managers;
using Jotunn.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Areas
{
    public static class GUI
    {
        public static ButtonConfig TogglePanel;
        public static ButtonConfig ToggleMouse;

        public static void Awake()
        {
            GUIManager.OnCustomGUIAvailable += OnPixelFix;

            TogglePanel = new ButtonConfig { Name = "GUI_TogglePanel", Config = Configs.GUI_TogglePanel, ActiveInGUI = true };
            InputManager.Instance.AddButton(Main.GUID, TogglePanel);
            ToggleMouse = new ButtonConfig { Name = "GUI_ToggleMouse", Config = Configs.GUI_ToggleMouse, ActiveInGUI = true };
            InputManager.Instance.AddButton(Main.GUID, ToggleMouse);

            var bundle = AssetUtils.LoadAssetBundle(Global.Path.Assets);
            var contents = bundle.GetAllAssetNames();

            var reqMain = bundle.LoadAssetAsync<GameObject>("Panel");
            var reqMarker = bundle.LoadAssetAsync<GameObject>("Marker");
            var reqBtn = bundle.LoadAssetAsync<GameObject>("BtnCS");

            reqMain.completed += (a) => { Panel.Prefab = reqMain.asset as GameObject; };
            reqMarker.completed += (a) => { Marker.Prefab = reqMarker.asset as GameObject; };
            reqBtn.completed += (a) => { BtnCS.Prefab = reqBtn.asset as GameObject; };
        }

        private static void OnPixelFix()
        {
            var scene = SceneManager.GetActiveScene();

            if (scene.name is not "main")
                return;
            if (Panel.Prefab is null || Marker.Prefab is null || BtnCS.Prefab is null)
            {
                Main.Log.LogError($"Areas GUI assets didn't load correctly!\n");
                return;
            }

            Transform pf = GUIManager.CustomGUIFront.transform;
            GameObject.Instantiate(Panel.Prefab, pf.position, Quaternion.identity, pf);
        }
    }
}

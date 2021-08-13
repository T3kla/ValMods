using UnityEngine;
using UnityEngine.UI;

namespace Areas
{

    public class AGUI_CSButton : MonoBehaviour
    {
        [HideInInspector] private CreatureSpawner cs = null;
        [HideInInspector] public float distance = 0f;
        public Button button = null;
        public Text text = null;

        private bool custom = false;
        private string csInfo = "";

        private void Awake()
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClick);
        }

        public CreatureSpawner GetCS() => cs;

        public void SetCS(CreatureSpawner newCS)
        {
            cs = newCS;

            if (cs != null)
            {
                var ctName = cs.GetCtName();
                custom = string.IsNullOrEmpty(ctName) ? false : true;
                csInfo = custom ? ctName : cs.m_creaturePrefab.name;
                gameObject.SetActive(true);
            }
            else
            {
                custom = false;
                csInfo = "";
                gameObject.SetActive(false);
            }

            UpdateText();
        }

        public void UpdateText()
        {
            if (cs == null) { text.text = ""; return; }

            UpdateDistance();
            text.text = $"{(custom ? "C" : "V")} [{distance.ToString("F0")}] {csInfo}";
        }

        private void UpdateDistance() => distance = Distance2Player();

        private void OnClick() => AGUI_Main.Instance.Display(this);

        float Distance2Player() => cs != null ? Vector3.Distance(cs.transform.position, Player.m_localPlayer.transform.position) : 999f;
    }

}

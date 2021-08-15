using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Areas
{

    public class BtnCS : MonoBehaviour
    {
        public Button button = null;
        public Text text = null;

        private CreatureSpawner cs = null;
        private bool custom = false;
        private float distance = 0f;
        private string info = "";

        private void Awake()
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClick);
        }

        public void SetCS(CreatureSpawner newCS)
        {
            cs = newCS;

            gameObject.SetActive(cs != null);
            if (cs != null) custom = string.IsNullOrEmpty(cs.GetCtName()) ? false : true;

            UpdateText();
        }

        public void UpdateText()
        {
            UpdateDistance();
            if (cs == null) return;
            info = cs.m_creaturePrefab.GetCleanName();
            text.text = $"{(custom ? "C" : "V")} | [{distance.ToString("F0")}] | {info}";
        }

        public CreatureSpawner GetCS() => cs;
        public float GetDistance() => distance;
        private void OnClick() => Panel.Instance.Display(this);
        private void UpdateDistance() => distance = (cs != null) ? Vector3.Distance(cs.transform.position, Player.m_localPlayer.transform.position) : 999f;

        public static int CompareDesDistance(BtnCS x, BtnCS y)
        {
            if (!x.gameObject.activeSelf) return !y.gameObject.activeSelf ? 0 : -1;
            if (!y.gameObject.activeSelf) return 1;
            return x.distance > y.distance ? 1 : -1;
        }
    }

}

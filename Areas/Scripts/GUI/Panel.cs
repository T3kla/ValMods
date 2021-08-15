using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Jotunn.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Areas
{

    public class Panel : MonoBehaviour
    {
        public static Panel Instance = null;
        public static bool IsOpen = false;
        public static List<CreatureSpawner> Spawners = new List<CreatureSpawner>();

        [SerializeField] private GameObject visual = null;
        [SerializeField] private Transform dragger = null;
        [SerializeField] private Transform resizer = null;
        [SerializeField] private Transform btnRoot = null;
        [SerializeField] private Text displayHeader = null;
        [SerializeField] private Text displayText = null;
        [SerializeField] private Button btnRefresh = null;
        [SerializeField] private Button btnExit = null;

        private const float automaticButtonRefresh = 0.5f;

        private RectTransform self = null;
        private Canvas canvas = null;
        private List<BtnCS> csButtons = new List<BtnCS>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else { Destroy(gameObject); return; }

            Close(false);
            self = GetComponent<RectTransform>();
            canvas = SearchForCanvas();

            if (canvas == null) return;

            PanelButtonAssignment();

            // Position Window
            var defPos = Globals.Config.AGUIDefPosition.Value.Split(':');
            var defSize = Globals.Config.AGUIDefSize.Value.Split(':');
            int defPosX = 0, defPosY = 0, defSizeX = 1600, defSizeY = 800;
            int.TryParse(defPos[0], out defPosX); int.TryParse(defPos[1], out defPosY);
            int.TryParse(defSize[0], out defSizeX); int.TryParse(defSize[1], out defSizeY);
            self.sizeDelta = new Vector2(defSizeX, defSizeY);
            self.anchoredPosition = new Vector2(defPosX, defPosY);

            SetPointerEvents(dragger, (data) => Drag(data));
            SetPointerEvents(resizer, (data) => Resize(data));
        }

        private void SetPointerEvents(Transform obj, Action<PointerEventData> Method)
        {
            var trigger = obj.GetComponent<EventTrigger>();
            var entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Drag;
            entry.callback.AddListener((data) => { Method((PointerEventData)data); });
            trigger.triggers.Add(entry);
        }

        private void Update()
        {
            if (ZInput.instance == null) return;
            if (ZInput.GetButtonDown(GUI.ShowGUIButton.Name)) Toggle();
        }

        private void PanelButtonAssignment()
        {
            btnRefresh.onClick.RemoveAllListeners();
            btnRefresh.onClick.AddListener(FullRefresh);
            btnExit.onClick.RemoveAllListeners();
            btnExit.onClick.AddListener(Exit);
        }

        private void Exit() => Close();

        private void Toggle() { if (IsOpen) Close(); else Open(); }

        private void Open()
        {
            IsOpen = true;
            visual.SetActive(true);
            StartCoroutine(ButtonRefresher());
            GUIManager.BlockInput(true);
        }

        private void Close(bool savePosSize = true)
        {
            IsOpen = false;
            visual.SetActive(false);
            StopAllCoroutines();
            Spawners.Clear();
            GUIManager.BlockInput(false);

            if (!savePosSize) return;
            Globals.Config.AGUIDefPosition.Value = $"{self.anchoredPosition.x.ToString("F0")}:{self.anchoredPosition.y.ToString("F0")}";
            Globals.Config.AGUIDefSize.Value = $"{self.sizeDelta.x.ToString("F0")}:{self.sizeDelta.y.ToString("F0")}";
        }

        /// <summary> Execute spawner search, assignment into csButtons, csButtons refresh and csButtons sort. </summary>
        private void FullRefresh()
        {
            btnRoot.gameObject.SetActive(false);

            SearchSpawners();
            AssignButtons(); // Assign also update the button
            SortButtons();

            btnRoot.gameObject.SetActive(true);
        }

        /// <summary> Execute csButtons refresh and csButtons sort. </summary>
        private void AutomaticRefresh()
        {
            btnRoot.gameObject.SetActive(false);

            UpdateButtons();
            SortButtons();

            btnRoot.gameObject.SetActive(true);
        }

        /// <summary> Search for spawners, updating the spawners list. </summary>
        private void SearchSpawners()
        {
            Transform root = ZNetScene.instance.m_netSceneRoot.transform;
            Spawners.Clear();

            foreach (Transform obj in root)
            {
                CreatureSpawner cs = obj.GetComponent<CreatureSpawner>();
                if (cs == null) continue;
                if (Vector3.Distance(cs.transform.position, Player.m_localPlayer.transform.position) < 100f) Spawners.Add(cs);
            }
        }

        /// <summary> Refresh the buttons shown in the container based on the current list of detected spawners. </summary>
        private void AssignButtons()
        {
            // Add buttons if more are needed
            int buttons = btnRoot.childCount, spawners = Spawners.Count;
            if (buttons < spawners)
                for (int i = 0; i < spawners - buttons; i++)
                {
                    var btn = Instantiate(GUI.btnCS, Vector3.zero, Quaternion.identity, btnRoot);
                    csButtons.Add(btn.GetComponent<BtnCS>());
                }

            // Assign cs to the buttons
            for (int i = 0; i < csButtons.Count; i++)
                csButtons[i].SetCS((i < spawners) ? Spawners[i] : null);
        }

        /// <summary> Update text and distance in buttons. </summary>
        private void UpdateButtons() { foreach (var btn in csButtons) if (btn.gameObject.activeSelf) btn.UpdateText(); }

        /// <summary> Sort csButtons in the spawner list container by sorting csButtons list and calling SetAsLastSibling(). </summary>
        private void SortButtons()
        {
            csButtons.Sort(BtnCS.CompareDesDistance);
            foreach (var btn in csButtons) btn.transform.SetAsLastSibling();
        }

        /// <summary> Display information about the clicked button corresponding CreatureSpawner. </summary>
        public void Display(BtnCS csb) // TODO: display stuff
        {
            displayHeader.text = csb.text.text;
            displayText.text = "Not much for now...";
        }

        /// <summary> Search for a canvas by traveling up the parent ladder. </summary>
        private Canvas SearchForCanvas()
        {
            Transform t = transform;
            while (t.parent != null)
            {
                t = t.parent;
                Canvas c = t.GetComponent<Canvas>();
                if (c != null) return c;
            }
            return null;
        }

        private void Drag(in PointerEventData ed) => self.anchoredPosition += ed.delta / canvas.scaleFactor;
        private void Resize(in PointerEventData ed) { var f = ed.delta / canvas.scaleFactor; RPos(ed, f); RSize(ed, f); }
        private void RPos(in PointerEventData ed, Vector2 f) => self.anchoredPosition += f / 2;
        private void RSize(in PointerEventData ed, Vector2 f) => self.sizeDelta = new Vector2(self.sizeDelta.x + f.x, self.sizeDelta.y - f.y);

        private IEnumerator ButtonRefresher()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(automaticButtonRefresh);
                AutomaticRefresh();
            }
        }

    }

}

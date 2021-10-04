// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using Jotunn.Managers;
// using UnityEngine;
// using UnityEngine.EventSystems;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;
// using Log = System.Console;

// namespace Areas
// {
//     public class Panel : MonoBehaviour
//     {
//         public static Panel Instance = null;
//         public static GameObject Prefab = null;
//         public static bool IsOpen = false;
//         public static List<CreatureSpawner> Spawners = new();

//         [SerializeField] private GameObject visual = null;
//         [SerializeField] private Transform dragger = null;
//         [SerializeField] private Transform resizer = null;
//         [SerializeField] private Transform btnRoot = null;
//         [SerializeField] private Text displayHeader = null;
//         [SerializeField] private Text displayText = null;
//         [SerializeField] private Button btnAdd = null;
//         [SerializeField] private Button btnShowAll = null;
//         [SerializeField] private Button btnRefresh = null;
//         [SerializeField] private Button btnExit = null;

//         private const float AUTO_REFRESH = 0.5f;

//         private RectTransform self = null;
//         private Canvas canvas = null;
//         private VerticalLayoutGroup btnRootVLG = null;
//         private ContentSizeFitter btnRootCSF = null;

//         private void Awake()
//         {
//             if (Instance is null) Instance = this;
//             else { Destroy(gameObject); return; }

//             Close(false);
//             self = GetComponent<RectTransform>();
//             canvas = SearchForCanvas();
//             btnRootVLG = btnRoot.GetComponent<VerticalLayoutGroup>();
//             btnRootCSF = btnRoot.GetComponent<ContentSizeFitter>();

//             if (canvas is null) return;

//             Listeners();

//             // Position Window
//             var defPos = Configs.GUI_DefaultPosition.Value.Split(':');
//             var defSize = Configs.GUI_DefaultSize.Value.Split(':');
//             int.TryParse(defPos[0], out int defPosX); int.TryParse(defPos[1], out int defPosY);
//             int.TryParse(defSize[0], out int defSizeX); int.TryParse(defSize[1], out int defSizeY);
//             self.sizeDelta = new Vector2(defSizeX, defSizeY);
//             self.anchoredPosition = new Vector2(defPosX, defPosY);

//             SetDragEvents(dragger, (data) => Drag(data));
//             SetDragEvents(resizer, (data) => Resize(data));
//         }

//         private void Update()
//         {
//             if (ZInput.instance is null) return;
//             if (ZInput.GetButtonDown(GUI.TogglePanel.Name)) TogglePanel();
//             if (ZInput.GetButtonDown(GUI.ToggleMouse.Name)) ToggleMouse();
//         }

//         private Canvas SearchForCanvas()
//         {
//             Transform t = transform;
//             while (t.parent != null)
//             {
//                 t = t.parent;
//                 Canvas c = t.GetComponent<Canvas>();
//                 if (c != null) return c;
//             }
//             return null;
//         }

//         private void Open()
//         {
//             IsOpen = true;
//             visual.SetActive(true);
//             StartCoroutine(ButtonRefresher());
//             GUIManager.BlockInput(true);
//         }

//         private void Close(bool savePosSize = true)
//         {
//             IsOpen = false;
//             visual.SetActive(false);
//             StopAllCoroutines();
//             Spawners.Clear();
//             GUIManager.BlockInput(false);

//             if (!savePosSize) return;
//             Configs.GUI_DefaultPosition.Value = $"{self.anchoredPosition.x:F0}:{self.anchoredPosition.y:F0}";
//             Configs.GUI_DefaultSize.Value = $"{self.sizeDelta.x:F0}:{self.sizeDelta.y:F0}";
//         }

//         #region UI Events

//         private void TogglePanel() { if (IsOpen) Close(); else Open(); }
//         private void ToggleMouse() { GUIManager.BlockInput(!Cursor.visible); }

//         private void SetDragEvents(Transform obj, Action<PointerEventData> Method)
//         {
//             var trigger = obj.GetComponent<EventTrigger>();
//             var entry = new EventTrigger.Entry { eventID = EventTriggerType.Drag };
//             entry.callback.AddListener((data) => { Method((PointerEventData)data); });
//             trigger.triggers.Add(entry);
//         }

//         private void Drag(in PointerEventData ed) => self.anchoredPosition += ed.delta / canvas.scaleFactor;
//         private void Resize(in PointerEventData ed) { var f = ed.delta / canvas.scaleFactor; RPos(f); RSize(f); }
//         private void RPos(Vector2 f) => self.anchoredPosition += f / 2;
//         private void RSize(Vector2 f) => self.sizeDelta = new Vector2(self.sizeDelta.x + f.x, self.sizeDelta.y - f.y);

//         private void Listeners()
//         {
//             btnAdd.onClick.RemoveAllListeners();
//             btnAdd.onClick.AddListener(OnClickAddBtn);
//             btnShowAll.onClick.RemoveAllListeners();
//             btnShowAll.onClick.AddListener(OnClickShowAll);
//             btnRefresh.onClick.RemoveAllListeners();
//             btnRefresh.onClick.AddListener(OnClickRefresh);
//             btnExit.onClick.RemoveAllListeners();
//             btnExit.onClick.AddListener(OnClickExit);
//         }

//         private void OnClickAddBtn() // TODO: 
//         {
//             // Open panel
//             // wait for resolution of this panel
//         }

//         private void OnClickShowAll()
//         {
//             var val = BtnCS.Each.Any(a => !a.MarkerIsActive);
//             foreach (var item in BtnCS.Each)
//                 item.SetMarkerActive(val);
//         }

//         public void OnClickRefresh() => Refresh(true);

//         private void OnClickExit() => Close();

//         #endregion

//         #region Button Pipeline

//         public void Refresh(bool withButtonSearch)
//         {
//             ToggleBtnRootComponents(false);
//             StopAllCoroutines();

//             if (withButtonSearch)
//                 SetSpawnerButtons();
//             else
//                 UpdateButtons();
//             SortButtons();

//             StartCoroutine(ButtonRefresher());
//             ToggleBtnRootComponents(true);
//         }

//         private void ToggleBtnRootComponents(bool value)
//         {
//             btnRootVLG.enabled = value;
//             btnRootCSF.enabled = value;
//         }

//         private void SetSpawnerButtons()
//         {
//             Spawners.Clear();

//             // Search for CreatureSpawners
//             foreach (GameObject obj in SceneManager.GetActiveScene().GetRootGameObjects())
//             {
//                 CreatureSpawner cs = obj.GetComponent<CreatureSpawner>();
//                 if (cs is null || Vector3.Distance(cs.transform.position, Player.m_localPlayer.transform.position) < 100f) continue;
//                 Spawners.Add(cs);
//             }

//             // Add buttons if more are needed
//             int buttons = btnRoot.childCount, spawners = Spawners.Count;
//             if (buttons < spawners)
//                 for (int i = 0; i < spawners - buttons; i++)
//                     BtnCS.Each.Add(BtnCS.Instantiate(btnRoot));

//             // Assign cs to the buttons
//             for (int i = 0; i < BtnCS.Each.Count; i++)
//                 BtnCS.Each[i].SetCS((i < spawners) ? Spawners[i] : null);
//         }

//         private void UpdateButtons()
//         {
//             foreach (var btn in BtnCS.Each)
//                 if (btn.gameObject.activeSelf)
//                     btn.UpdateText();
//         }

//         private void SortButtons()
//         {
//             BtnCS.Each.Sort(BtnCS.CompareDesDistance);
//             foreach (var btn in BtnCS.Each)
//                 btn.transform.SetAsLastSibling();
//         }

//         private IEnumerator ButtonRefresher()
//         {
//             while (true)
//             {
//                 yield return new WaitForSecondsRealtime(AUTO_REFRESH);
//                 Refresh(false);
//             }
//         }

//         #endregion

//         public void Display(BtnCS csb) // TODO: call the csedition panel
//         {
//             displayHeader.text = csb.text.text;
//             var cs = csb.GetCS();

//             string str = "";
//             str += $"<b>m_respawnTimeMinuts:</b> {cs.m_respawnTimeMinuts:F0} \n";
//             str += $"<b>m_triggerDistance:</b> {cs.m_triggerDistance:F0} \n";
//             str += $"<b>m_triggerNoise:</b> {cs.m_triggerNoise:F0} \n";
//             str += $"<b>m_spawnAtDay:</b> {cs.m_spawnAtDay} \n";
//             str += $"<b>m_spawnAtNight:</b> {cs.m_spawnAtNight} \n";
//             str += $"<b>m_requireSpawnArea:</b> {cs.m_requireSpawnArea} \n";
//             str += $"<b>m_spawnInPlayerBase:</b> {cs.m_spawnInPlayerBase} \n";
//             str += $"<b>m_setPatrolSpawnPoint:</b> {cs.m_setPatrolSpawnPoint} \n";

//             displayText.text = str;
//         }

//     }

// }

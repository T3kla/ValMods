// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// namespace Areas
// {

//     public class BtnCS : MonoBehaviour
//     {
//         public static GameObject Prefab = null;
//         public static List<BtnCS> Each = new();
//         public static BtnCS Instantiate(Transform parent) => Instantiate(Prefab, Vector3.zero, Quaternion.identity, parent).GetComponent<BtnCS>();

//         public Button btnDisplay = null;
//         public Button btnShow = null;
//         public Button btnDelete = null;
//         public Text text = null;
//         public Color showColorOn = Color.white;
//         public Color showColorOff = Color.grey;

//         private CreatureSpawner cs = null;

//         private void Awake()
//         {
//             Each.Add(this);
//             marker = Marker.Create(PlayerPos);
//             Listeners();
//         }

//         public CreatureSpawner GetCS() => cs;
//         public void SetCS(CreatureSpawner newCS)
//         {
//             cs = newCS;

//             if (cs is null)
//             {
//                 gameObject.SetActive(false);
//                 marker.SetActive(false);
//             }
//             else
//             {
//                 gameObject.SetActive(true);
//                 marker.SetActive(true);
//                 custom = !string.IsNullOrEmpty(cs.GetCtName());
//                 UpdateText();
//             }
//         }

//         #region UI Events

//         private void Listeners()
//         {
//             btnDisplay.onClick.RemoveAllListeners();
//             btnDisplay.onClick.AddListener(OnClickDisplay);
//             btnShow.onClick.RemoveAllListeners();
//             btnShow.onClick.AddListener(OnClickShow);
//             btnDelete.onClick.RemoveAllListeners();
//             btnDelete.onClick.AddListener(OnClickRemoveCS);
//         }

//         private void OnClickDisplay() => Panel.Instance.Display(this);
//         private void OnClickShow() => SetMarkerActive(!isMarkerActive);

//         private void OnClickRemoveCS()
//         {
//             // TODO: Ask for confirmation
//             ZNetScene.instance.Destroy(cs.gameObject);
//             SetCS(null);
//             Panel.Instance.Refresh(true);
//         }

//         #endregion

//         #region Text & Distance

//         private bool custom = false;
//         private string critterName = "";
//         private float _distance = 999f;
//         private float Distance => (cs is not null) ? _distance : 999f;

//         private Vector3 PlayerPos => Player.m_localPlayer.transform.position;
//         private float CalculateDistance() => Vector3.Distance(cs.transform.position, PlayerPos);

//         public void UpdateText()
//         {
//             if (cs is null) return;
//             _distance = CalculateDistance();
//             critterName = cs.m_creaturePrefab.GetCleanName();
//             var v = cs.transform.position;
//             text.text = $"<b>{(custom ? "C" : "V")}</b> - {v.x:F0},{v.y:F0},{v.z:F0} - {_distance:F0}m | {critterName}";
//             if (marker is null) return;
//             marker.text.text = $"<b>{(custom ? "Custom" : "Vanilla")}</b>\n{v.x:F0},{v.y:F0},{v.z:F0} - {_distance:F0}m\n{critterName}";
//         }

//         public static int CompareDesDistance(BtnCS x, BtnCS y)
//         {
//             if (!x.gameObject.activeSelf) return !y.gameObject.activeSelf ? 0 : -1;
//             if (!y.gameObject.activeSelf) return 1;
//             return x.Distance > y.Distance ? 1 : -1;
//         }

//         #endregion

//         #region Markers

//         private Marker marker = null;
//         private bool isMarkerActive = false;
//         public bool MarkerIsActive => isMarkerActive;

//         public void SetMarkerActive(bool value)
//         {
//             isMarkerActive = value;
//             ShowMarker(value);
//         }

//         private void ShowMarker(bool value)
//         {
//             if (cs is null) return;
//             marker.transform.position = cs.transform.position;
//             marker.SetActive(value);
//             btnShow.targetGraphic.color = value ? showColorOn : showColorOff;
//         }

//         public void SetMarkerText(string str) => text.text = str;

//         #endregion

//     }

// }

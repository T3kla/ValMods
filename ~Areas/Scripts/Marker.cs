using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Areas
{

    public class Marker : MonoBehaviour
    {
        public static GameObject Prefab = null;
        public static List<Marker> Each = new();
        public static Marker Create(Vector3 position) => Instantiate(Prefab, position, Quaternion.identity).GetComponent<Marker>();

        public GameObject sphere = null;
        public Text text = null;

        private bool _isActive = false;
        public bool IsActive => _isActive;

        public void Awake()
        {
            Each.Add(this);
            _isActive = false;
        }

        public void SetActive(bool value)
        {
            sphere.SetActive(value);
            text.enabled = value;
            _isActive = value;
        }

        public void FixedUpdate()
        {
            if (!IsActive) return;
            // TODO: text points to player cam
        }

    }

}

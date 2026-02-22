using UnityEngine;

namespace Core.Scripts.Debugging
{
    public class DebugScreenToggle : MonoBehaviour
    {
        [SerializeField] private GameObject _debugPanel;
        [SerializeField] private KeyCode _toggleKey = KeyCode.Alpha0;

        private void Update()
        {
            if (Input.GetKeyDown(_toggleKey))
            {
                var target = _debugPanel != null ? _debugPanel : gameObject;
                target.SetActive(!target.activeSelf);
            }
        }
    }
}

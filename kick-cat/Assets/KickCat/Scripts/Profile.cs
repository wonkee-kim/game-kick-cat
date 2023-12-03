using UnityEngine;
using UnityEngine.UI;

public class Profile : MonoBehaviour
{
    [SerializeField] private Button _buttonProfile;
    [SerializeField] private GameObject _profilePanel;

    private void Awake()
    {
        _buttonProfile.onClick.AddListener(() => _profilePanel.SetActive(!_profilePanel.activeSelf));
    }

    private void Update()
    {
        if (_profilePanel.activeSelf &&
            (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            _profilePanel.SetActive(false);
        }
    }
}

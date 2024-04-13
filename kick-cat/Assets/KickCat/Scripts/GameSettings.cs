using SpatialSys.UnitySDK;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    private static GameSettings _instance;

    public static float gravity => _instance._gravity;
    public static float groundLevel => _instance._groundLevel;
    public static float drag => _instance._drag;
    public static float viewPortRange => _instance._viewPortRange;
    public static float scrollSpeed => _instance._scrollSpeed;

    [SerializeField] private float _groundLevel = 0f;
    [SerializeField] private float _gravity = 9.81f;
    [SerializeField] private float _drag = 0.7f;
    [SerializeField] private float _viewPortRange = 7f;
    [SerializeField] private float _scrollSpeed = 7f;

    private AvatarInputActionsListener _inputListener = new AvatarInputActionsListener();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        SpatialBridge.inputService.StartAvatarInputCapture(movement: true, jump: true, sprint: true, actionButton: true, _inputListener);
    }
    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}

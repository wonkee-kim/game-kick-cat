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

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        SpatialBridge.SetInputOverrides(movementOverride: true, jumpOverride: true, sprintOverride: true, actionButtonOverride: true, target: gameObject);
    }
    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}

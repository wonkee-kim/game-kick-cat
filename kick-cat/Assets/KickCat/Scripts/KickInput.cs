using System;
using UnityEngine;

public class KickInput : MonoBehaviour
{
    public static Action onKick;

    private const float TIME_INTERVAL_INPUT = 0.15f;
    private const float TIME_INTERVAL_KICK = 0.5f;

    private float _lastTimeKickPressed = 0f;
    private float _lastTimeKickPerformed = 0f;
    public bool _isKickPressed = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            _isKickPressed = true;
            _lastTimeKickPressed = Time.time;
        }

        if (_isKickPressed)
        {
            if (Time.time - _lastTimeKickPressed > TIME_INTERVAL_INPUT)
            {
                _isKickPressed = false;
            }
            else if (Time.time - _lastTimeKickPerformed > TIME_INTERVAL_KICK)
            {
                onKick?.Invoke();
                _isKickPressed = false;
                _lastTimeKickPerformed = Time.time;
            }
        }
    }
}

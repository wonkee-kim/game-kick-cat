using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Vector2 _generateKickableObjectIntervalRange = new Vector2(0.4f, 1.7f);

    private float _interval = 0f;
    private float _lastTimeKickableObjectGenerated = 0f;

    private void Update()
    {
        if (Time.time - _lastTimeKickableObjectGenerated > _interval)
        {
            _lastTimeKickableObjectGenerated = Time.time;
            _interval = Random.Range(_generateKickableObjectIntervalRange.x, _generateKickableObjectIntervalRange.y);
            KickablesManager.CreateKickableObject((KickableType)Random.Range(0, System.Enum.GetValues(typeof(KickableType)).Length));
        }
    }
}

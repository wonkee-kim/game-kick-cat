using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Vector2 _generateKickableObjectIntervalRange = new Vector2(1f, 7f);

    private float _lastTimeKickableObjectGenerated = 0f;

    private void Update()
    {
        if (Time.time - _lastTimeKickableObjectGenerated > Random.Range(_generateKickableObjectIntervalRange.x, _generateKickableObjectIntervalRange.y))
        {
            _lastTimeKickableObjectGenerated = Time.time;
            KickablesManager.CreateKickableObject((KickableType)Random.Range(0, System.Enum.GetValues(typeof(KickableType)).Length));
        }
    }
}

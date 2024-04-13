using UnityEngine;
using SpatialSys.UnitySDK;

public class KickManager : MonoBehaviour
{
    private static readonly Vector3 KICK_DIRECTION = new Vector3(0.7071f, 0.7071f, 0f);
    private const int SCORE_SCALE = 10;

    [SerializeField] private Transform _kickPoint;
    [SerializeField] private float _kickRadius = 1f;
    [SerializeField] private float _kickPower = 7f;

    [SerializeField] private CatView _catView;

    private void Awake()
    {
        KickInput.onKick += Kick;
    }

    private void OnDestroy()
    {
        KickInput.onKick -= Kick;
    }

    private void Kick()
    {
        bool isHit = false;
        Vector3 hitPosition = _kickPoint.position;
        foreach (KickableObject kickableObject in KickablesManager.activeKickableObjects)
        {
            // Hit
            float maxHitDistance = _kickRadius + kickableObject.state.halfSize;
            if (Vector3.Distance(kickableObject.transform.position, _kickPoint.position) < maxHitDistance)
            {
                // KickableObject
                Vector3 hitDirection = kickableObject.transform.position - _kickPoint.position;
                float hitDistance = hitDirection.magnitude;
                hitDirection /= hitDistance; // normalize

                // float attenuation = Mathf.Clamp01(1f - hitDistance / _kickRadius);
                float attenuation = 1f / Mathf.Max(hitDistance, 1f);

                Vector3 velocity = (KICK_DIRECTION + hitDirection) * _kickPower * attenuation;

                float dot = Vector3.Dot(hitDirection, KICK_DIRECTION);
                float sign = Mathf.Sign(Vector3.Cross(KICK_DIRECTION, hitDirection).z);
                float rotationVelocity = dot * sign * _kickPower * attenuation;

                kickableObject.Kick(velocity, rotationVelocity);
                isHit = true;
                hitPosition = (kickableObject.transform.position + _kickPoint.position) * 0.5f;

                // Score
                float distancePerScore = maxHitDistance / SCORE_SCALE;
                int score = Mathf.CeilToInt((maxHitDistance - hitDistance + distancePerScore) / distancePerScore);
                ScoreManager.AddScore(score);

                AudioManager.PlayHitSound();
            }
        }
        _catView.OnKick(isHit, hitPosition);
        AudioManager.PlayKickSound();
        SpatialBridge.inputService.PlayVibration(frequency: isHit ? 0.7f : 0.2f, amplitude: isHit ? 0.4f : 0.2f, duration: 0.1f);
    }
}

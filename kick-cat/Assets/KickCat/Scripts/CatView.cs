using UnityEngine;

public class CatView : MonoBehaviour
{
    private static int PROP_TEXTURE = Shader.PropertyToID("_BaseMap");

    public Texture2D textureCat;
    public Texture2D textureCatKick;

    [SerializeField] private MeshRenderer _catRenderer;
    [SerializeField] private Transform _kickEffectTransform;
    [SerializeField] private MeshRenderer _kickEffectRenderer;
    private float _effectZPosition = -0.01f;

    private const float TIME_KICK = 0.3f;
    private const float TIME_KICK_EFFECT = 0.15f;
    private float _lastTimeKick;
    private bool _isKicked = false;

    private void Awake()
    {
        _effectZPosition = _kickEffectTransform.position.z;
        _kickEffectRenderer.enabled = false;
        SetKick(false);
    }

    private void Update()
    {
        if (_isKicked)
        {
            float timeSinceLastKick = Time.time - _lastTimeKick;
            if (timeSinceLastKick > TIME_KICK_EFFECT)
            {
                _kickEffectRenderer.enabled = false;
            }
            if (timeSinceLastKick > TIME_KICK)
            {
                _isKicked = false;
                SetKick(false);
            }
        }
    }

    public void OnKick(bool isHit, Vector3 hitPosition)
    {
        _lastTimeKick = Time.time;
        _isKicked = true;
        SetKick(true);
        if (isHit)
        {
            _kickEffectTransform.position = new Vector3(hitPosition.x, hitPosition.y, _effectZPosition);
            _kickEffectRenderer.enabled = true;
        }
    }

    private void SetKick(bool isKick)
    {
        _catRenderer.sharedMaterial.SetTexture(PROP_TEXTURE, isKick ? textureCatKick : textureCat);
    }
}

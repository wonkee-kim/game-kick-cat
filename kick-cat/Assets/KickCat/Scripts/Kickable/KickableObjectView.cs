using UnityEngine;

public class KickableObjectView : MonoBehaviour
{
    private static int ANIM_BOOL_KICK = Animator.StringToHash("isKicked");
    private static int PROP_TEXTURE = Shader.PropertyToID("_BaseMap");

    [SerializeField] private Animator _animator;
    [SerializeField] private MeshRenderer _renderer;

    public void Initialize(Texture2D texture)
    {
        _renderer.material.SetTexture(PROP_TEXTURE, texture);
    }

    public void SetKick(bool isKick)
    {
        _animator.SetBool(ANIM_BOOL_KICK, isKick);
    }
}

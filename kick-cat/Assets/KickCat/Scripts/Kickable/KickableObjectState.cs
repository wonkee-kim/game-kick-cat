using UnityEngine;

public enum KickableType
{
    Dog = 0,
    Poo = 1,
    TrashCan = 2,
    Squirrel = 3,
    KitKat = 4,
}

[CreateAssetMenu(fileName = "KickableObjectState", menuName = "KickCat/KickableObjectState", order = 0)]
public class KickableObjectState : ScriptableObject
{
    public KickableType kickableType;
    public float speed = 1f;
    public float size = 1f;
    public float halfSize => size * 0.5f;
    public float mass = 1f;
    public Texture2D texture;
}
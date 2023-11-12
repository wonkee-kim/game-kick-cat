using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KickablesManager : MonoBehaviour
{
    private static KickablesManager _instance;

    [SerializeField] private KickableObjectState[] _kickableObjectStates;

    [SerializeField] private int _initialPoolCount = 10;
    [SerializeField] private GameObject _kickableObjectPrefab;

    private List<KickableObject> _kickableObjectsPool = new List<KickableObject>();
    private List<KickableObject> _activeKickableObjects = new List<KickableObject>();
    public static List<KickableObject> activeKickableObjects => _instance._activeKickableObjects;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        for (int i = 0; i < _initialPoolCount; i++)
        {
            GameObject kickableObjectGameObject = Instantiate(_kickableObjectPrefab, transform);
            KickableObject kickableObject = kickableObjectGameObject.GetComponent<KickableObject>();
            kickableObject.Initalize(GetKickableObjectState(KickableType.Dog));
            kickableObjectGameObject.SetActive(false);
            _kickableObjectsPool.Add(kickableObject);
        }
    }

    public static void CreateKickableObject(KickableType kickableType)
    {
        _instance.CreateKickableObjectsInternal(kickableType);
    }

    public static void RemoveKickableObject(KickableObject kickableObject)
    {
        _instance.RemoveKickableObjectInternal(kickableObject);
    }

    private void CreateKickableObjectsInternal(KickableType kickableType)
    {
        KickableObjectState state = GetKickableObjectState(kickableType);
        if (state == null)
        {
            Debug.LogError($"There's no state for kickable type: {kickableType}");
            return;
        }

        KickableObject kickableObject = null;
        // Object Pooling
        foreach (KickableObject kickable in _kickableObjectsPool)
        {
            if (!kickable.gameObject.activeSelf)
            {
                kickableObject = kickable;
                break;
            }
        }
        if (kickableObject == null)
        {
            GameObject kickableObjectGameObject = Instantiate(_kickableObjectPrefab, transform);
            kickableObject = kickableObjectGameObject.GetComponent<KickableObject>();
            _kickableObjectsPool.Add(kickableObject);
        }
        kickableObject.gameObject.SetActive(true);
        Vector3 spawnPosition = Vector3.right * (GameSettings.viewPortRange + state.size) + Vector3.up * state.halfSize;
        kickableObject.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);

        kickableObject.Initalize(state);
        _activeKickableObjects.Add(kickableObject);
    }

    private void RemoveKickableObjectInternal(KickableObject kickableObject)
    {
        kickableObject.gameObject.SetActive(false);
        _activeKickableObjects.Remove(kickableObject);
    }

    private KickableObjectState GetKickableObjectState(KickableType kickableType)
    {
        foreach (KickableObjectState kickableObjectState in _kickableObjectStates)
        {
            if (kickableObjectState.kickableType == kickableType)
            {
                return kickableObjectState;
            }
        }
        return null;
    }
}
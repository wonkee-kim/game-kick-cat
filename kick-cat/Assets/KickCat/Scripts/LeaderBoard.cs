using System;
using UnityEngine;
using Unity.VisualScripting;
using SpatialSys.UnitySDK;
using System.Collections.Generic;
using TMPro;

public class LeaderBoard : MonoBehaviour
{
    private static LeaderBoard _instance;

    private Dictionary<int, int> _scores = new Dictionary<int, int>(); // userID, score
    private List<KeyValuePair<int, int>> _sortedScores = new List<KeyValuePair<int, int>>();

    private const int MAX_LEADERBOARD_SCORES = 8;
    [SerializeField] private GameObject[] _textContainers;
    [SerializeField] private TextMeshProUGUI[] _textNames;
    [SerializeField] private TextMeshProUGUI[] _textScores;

    [SerializeField] private SpatialSyncedObject _leaderBoardSyncedObject;
    [SerializeField] private Variables _leaderBoardSyncedVariables;
    [SerializeField] private TextMeshProUGUI _textTopName;
    [SerializeField] private TextMeshProUGUI _textTopScore;
    private int _topScore;
    private string _topScorePlayerName;

    private Delegate _customEvHandler;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        _customEvHandler = VisualScriptingUtility.AddCustomEventListener(gameObject, HandleCustomEvent);
        UpdateLeaderBoardUI();
    }
    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
        VisualScriptingUtility.RemoveCustomEventListener(gameObject, _customEvHandler);
    }

    public static void UpdateScore(int score)
    {
        VisualScriptingUtility.TriggerCustomEvent(_instance.gameObject, "UpdateScoreEvent", new object[] { score });
    }

    // TODO: doesn't work
    public static void RemovePlayer()
    {
        VisualScriptingUtility.TriggerCustomEvent(_instance.gameObject, "RemovePlayerEvent");
    }

    private void HandleCustomEvent(string message, object[] args)
    {
        switch (message)
        {
            case nameof(OnScoreUpdated):
                OnScoreUpdated((int)args[0], (int)args[1]); // userID, score
                break;
            case nameof(OnPlayerRemoved):
                OnPlayerRemoved((int)args[0]); // userID
                break;
            default:
                Debug.LogWarning("received unknown message: " + message);
                break;
        }
    }

    private void OnScoreUpdated(int userID, int score)
    {
        if (_scores.ContainsKey(userID))
        {
            _scores[userID] = score;
        }
        else
        {
            _scores.Add(userID, score);
        }

        _topScore = (int)_instance._leaderBoardSyncedVariables.declarations.Get("topScore");
        _topScorePlayerName = (string)_instance._leaderBoardSyncedVariables.declarations.Get("topScorePlayerName");
        if (score > _topScore)
        {
            _topScore = score;
            _topScorePlayerName = SpatialBridge.actorService.actors[userID].displayName;
            if (SpatialBridge.spaceContentService.GetSyncedObjectIsLocallyOwned(_leaderBoardSyncedObject))
            {
                _instance._leaderBoardSyncedVariables.declarations.Set("topScore", _topScore);
                _instance._leaderBoardSyncedVariables.declarations.Set("topScorePlayerName", _topScorePlayerName);
            }
        }

        _instance.UpdateLeaderBoardUI();
    }

    private void OnPlayerRemoved(int userID)
    {
        if (_scores.ContainsKey(userID))
        {
            _scores.Remove(userID);
        }
        _instance.UpdateLeaderBoardUI();
    }

    private void UpdateLeaderBoardUI()
    {
        // Check any player has left
        foreach (int id in _scores.Keys)
        {
            if (!SpatialBridge.actorService.actors.ContainsKey(id))
            {
                _scores.Remove(id);
            }
        }

        _sortedScores.Clear();
        foreach (KeyValuePair<int, int> kvp in _scores)
        {
            _sortedScores.Add(kvp);
        }
        _sortedScores.Sort((x, y) => y.Value.CompareTo(x.Value));

        for (int i = 0; i < MAX_LEADERBOARD_SCORES; i++)
        {
            _textContainers[i].gameObject.SetActive(i < _sortedScores.Count);
            if (i < _sortedScores.Count)
            {
                _textNames[i].text = SpatialBridge.actorService.actors[_sortedScores[i].Key].displayName;
                _textScores[i].text = _sortedScores[i].Value.ToString("N0");
            }
        }

        _instance._textTopName.text = _topScorePlayerName;
        _instance._textTopScore.text = _topScore.ToString("N0");
    }
}
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

    private SpatialBridge.ActorUserData _localActorUserData;
    private static string _userName => _instance._localActorUserData.displayName;

    private const int MAX_LEADERBOARD_SCORES = 8;
    [SerializeField] private TextMeshProUGUI _textNames;
    [SerializeField] private TextMeshProUGUI _textScores;

    [SerializeField] private Variables _leaderBoardSyncedVariables;
    [SerializeField] private TextMeshProUGUI _textTopName;
    [SerializeField] private TextMeshProUGUI _textTopScore;

    private Delegate _customEvHandler;


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        _localActorUserData = SpatialBridge.GetLocalActorUserData();
        _customEvHandler = VisualScriptingUtility.AddCustomEventListener(gameObject, HandleCustomEvent);
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
        if (score > (int)_instance._leaderBoardSyncedVariables.declarations.Get("topScore"))
        {
            _instance._leaderBoardSyncedVariables.declarations.Set("topScore", score);
            _instance._leaderBoardSyncedVariables.declarations.Set("topScorePlayerName", _userName);
            _instance._textTopName.text = _userName;
            _instance._textTopScore.text = score.ToString("N0");
        }
    }

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
        _sortedScores.Clear();
        foreach (KeyValuePair<int, int> kvp in _scores)
        {
            _sortedScores.Add(kvp);
        }
        _sortedScores.Sort((x, y) => y.Value.CompareTo(x.Value));

        string names = "";
        string scores = "";
        int count = Mathf.Min(_sortedScores.Count, MAX_LEADERBOARD_SCORES);
        for (int i = 0; i < count; i++)
        {
            names += SpatialBridge.GetActorUserData(_sortedScores[i].Key).displayName + "\n";
            scores += _sortedScores[i].Value.ToString("N0") + "\n";
        }

        _textNames.text = names;
        _textScores.text = scores;
    }
}
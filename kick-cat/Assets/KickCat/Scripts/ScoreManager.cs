using System;
using UnityEngine;
using System.Collections;
using SpatialSys.UnitySDK;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _instance;

    private static readonly int ANIM_TRIGGER_POP = Animator.StringToHash("pop");

    [SerializeField] private TextMeshProUGUI _scoreText;

    [SerializeField] private TextMeshPro _scoreTextFloating;
    [SerializeField] private Animator _scoreTextFloatingAnimator;

    private int _currentScore = 0;

    private Delegate _customEvHandler;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        _customEvHandler = VisualScriptingUtility.AddCustomEventListener(gameObject, HandleCustomEvent);
    }
    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
        LeaderBoard.RemovePlayer();
        VisualScriptingUtility.RemoveCustomEventListener(gameObject, _customEvHandler);
    }

    private void HandleCustomEvent(string message, object[] args)
    {
        switch (message)
        {
            case "Initialized":
                UpdateCurrentScore(_currentScore + (int)args[0]); // current score + loaded scores
                break;
            default:
                Debug.LogWarning("received unknown message: " + message);
                break;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine(Initialize());
        }
    }

    private IEnumerator Initialize()
    {
        bool hasValue = false;
        bool completed = false;
        SpatialBridge.HasDataStoreVariable(
            ClientBridge.DataStoreScope.UserWorldData,
            "score",
            result =>
            {
                completed = true;
                hasValue = (bool)result.value;
            }
        );
        yield return new WaitUntil(() => completed);
    }

    public static void AddScore(int scoreAdd)
    {
        _instance.AddScoreInternal(scoreAdd);
    }
    private void AddScoreInternal(int scoreAdd)
    {
        _scoreTextFloating.text = scoreAdd.ToString();
        _scoreTextFloatingAnimator.SetTrigger(ANIM_TRIGGER_POP);

        UpdateCurrentScore(_currentScore + scoreAdd);

        // Save Data
        VisualScriptingUtility.TriggerCustomEvent(gameObject, "SaveScore", new object[] { _currentScore });
    }

    private void UpdateCurrentScore(int score)
    {
        _currentScore = score;
        _scoreText.text = _currentScore.ToString("N0");
        LeaderBoard.UpdateScore(_currentScore);
    }
}

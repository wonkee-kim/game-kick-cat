using UnityEngine;
using SpatialSys.UnitySDK;
using TMPro;
using System.Collections;
using System;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _instance;

    private static int ANIM_TRIGGER_POP = Animator.StringToHash("pop");
    private const string SCORE_DATA_STORE_KEU = "score";

    // [SerializeField] private Transform _scoreTextTransform;
    // [SerializeField] private AnimationCurve _floatingTextScaleCurve;
    // [SerializeField] private AnimationCurve _floatingTextAlphaCurve;

    [SerializeField] private TextMeshPro _scoreTextFloating;
    [SerializeField] private Animator _scoreTextFloatingAnimator;
    [SerializeField] private TextMeshProUGUI _scoreText;

    private IEnumerator _addScoreCoroutine;

    private int _currentScore = 0;
    private bool _initialized = false;

    private ClientBridge.DataStoreOperationResult _dataStoreOperationResult;
    private bool _isDataStoreOperationComplete = false;

    private Action<ClientBridge.DataStoreOperationResult> _hasDataStoreCallback;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        // _dataStoreOperationResult = new ClientBridge.DataStoreOperationResult();

        _hasDataStoreCallback += HasDataStoreCallbackA;
    }

    private void Update()
    {
        if (!_initialized && ClientBridge.GetIsSceneInitialized())
        {
            _initialized = true;
            _scoreText.text = _currentScore.ToString("N0");
            // StartCoroutine(InitializeScoreWithData());
            // InitializeScoreWithData();
        }
    }

    // private IEnumerator InitializeScoreWithData()
    private void InitializeScoreWithData()
    {
        // bool completed = false;
        // ClientBridge.GetDataStoreVariableValue?.Invoke(
        //     flow.GetValue<ClientBridge.DataStoreScope>(scope),
        //     flow.GetValue<string>(key),
        //     flow.GetValue<object>(defaultValue),
        //     result =>
        //     {
        //         completed = true;
        //         flow.SetValue(value, result.value);
        //         flow.SetValue(succeeded, result.succeeded);
        //         flow.SetValue(responseCode, result.responseCode);
        //     }
        // );
        // yield return new WaitUntil(() => completed);
        // yield return outputTrigger;

        Debug.LogError("!!! InitializeScoreWithData");
        // _isDataStoreOperationComplete = false;
        // ClientBridge.HasDataStoreVariable(ClientBridge.DataStoreScope.UserWorldData, SCORE_DATA_STORE_KEU, HasDataStoreCallback);

        bool completed = false;
        bool hasValue = false;
        // ClientBridge.HasDataStoreVariable(
        //     ClientBridge.DataStoreScope.UserWorldData,
        //     SCORE_DATA_STORE_KEU,
        //     result =>
        //     {
        //         completed = true;
        //         hasValue = (bool)result.value;
        //     }
        // );
        ClientBridge.HasDataStoreVariable(
            ClientBridge.DataStoreScope.UserWorldData,
            SCORE_DATA_STORE_KEU,
            null
        );
        Debug.LogError("!!! Runned");
        // yield return new WaitUntil(() => completed);

        // yield return new WaitUntil(() => _isDataStoreOperationComplete);
        // yield return new WaitForSeconds(0.2f);

        Debug.LogError("!!! HasDataStoreVariable Complete");

        // hasValue
        // if ((bool)_dataStoreOperationResult.value)
        // {
        //     _isDataStoreOperationComplete = false;
        //     ClientBridge.GetDataStoreVariableValue(ClientBridge.DataStoreScope.UserWorldData, SCORE_DATA_STORE_KEU, 0, GetDataStoreCallback);
        // }
        // Debug.LogError($"!!! hasValue: {(bool)_dataStoreOperationResult.value}");

        // yield return new WaitUntil(() => _isDataStoreOperationComplete);
        // yield return new WaitForSeconds(0.2f);

        // _currentScore = (int)_dataStoreOperationResult.value;
        // Debug.LogError($"!!! GetDataStoreVariable Complete, loaded Score: {_currentScore}");

        // _scoreText.text = _currentScore.ToString("N0");
    }

    private void HasDataStoreCallbackA(ClientBridge.DataStoreOperationResult result)
    {
        Debug.LogError("!!! HasDataStoreCallbackA");
    }



    public static void AddScore(int score)
    {
        _instance.AddScoreInternal(score);

    }

    public void AddScoreInternal(int score)
    {
        // VisualScriptingUtility.TriggerCustomEvent(gameObject, "AddScoreVS", new object[] { score });

        // Floating Text
        // string scoreText = $"<font-weight='900'>{score.ToString()}</font-weight>";
        // string scoreText = $"<size=300%>{score.ToString()}";
        // ClientBridge.CreateFloatingText(scoreText, FloatingTextAnimStyle.Bouncy, _scoreTextTransform.position, Vector3.up, Color.red, gravity: false, _floatingTextScaleCurve, _floatingTextAlphaCurve, lifetime: 0.2f);
        _scoreTextFloating.text = score.ToString();
        _scoreTextFloatingAnimator.SetTrigger(ANIM_TRIGGER_POP);

        // Score UI
        _currentScore += score;
        _scoreText.text = _currentScore.ToString("N0");

        // Save Score
        // if (_addScoreCoroutine != null)
        // {
        //     StopCoroutine(_addScoreCoroutine);
        // }
        // _addScoreCoroutine = SaveScoreCoroutine(_currentScore);
        // StartCoroutine(_addScoreCoroutine);
    }

    private IEnumerator SaveScoreCoroutine(int currentStore)
    {
        _isDataStoreOperationComplete = false;
        ClientBridge.SetDataStoreVariableValue(ClientBridge.DataStoreScope.UserWorldData, SCORE_DATA_STORE_KEU, currentStore, SetDataStoreCallback);

        Debug.LogError($"!!! Try Save Score: {currentStore}");

        yield return new WaitUntil(() => _isDataStoreOperationComplete);

        Debug.LogError("!!! SetDataStoreVariable Complete");
    }

    private void HasDataStoreCallback(ClientBridge.DataStoreOperationResult result)
    {
        _isDataStoreOperationComplete = true;
        _dataStoreOperationResult = result;
    }

    private void GetDataStoreCallback(ClientBridge.DataStoreOperationResult result)
    {
        _isDataStoreOperationComplete = true;
        _dataStoreOperationResult = result;
    }

    private void SetDataStoreCallback(ClientBridge.DataStoreOperationResult result)
    {
        _isDataStoreOperationComplete = true;
        _dataStoreOperationResult = result;
    }
}

using UnityEngine;
using SpatialSys.UnitySDK;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public static void AddScore(int score)
    {
        _instance.AddScoreInternal(score);
    }

    public void AddScoreInternal(int score)
    {
        VisualScriptingUtility.TriggerCustomEvent(gameObject, "AddScoreVS", new object[] { score });
    }
}

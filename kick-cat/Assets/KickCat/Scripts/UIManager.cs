using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RectTransform[] _uiAnchors;
    [SerializeField] private float[] widthOffsets;
    private Rect _safeArea;

    private void Update()
    {
        if (_safeArea != Screen.safeArea)
        {
            _safeArea = Screen.safeArea;
            UpdateUIAnchors();
        }
    }

    private void UpdateUIAnchors()
    {
        for (int i = 0; i < _uiAnchors.Length; i++)
        {
            _uiAnchors[i].anchoredPosition = new Vector2(_safeArea.xMin + widthOffsets[i], _uiAnchors[i].anchoredPosition.y);
        }
    }
}

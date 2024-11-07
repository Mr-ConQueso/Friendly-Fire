using UnityEngine;

public class ChangeCanvasMode : MonoBehaviour
{
    // ---- / Private Variables / ---- //
    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _canvas.renderMode = RenderMode.ScreenSpaceCamera;
        _canvas.worldCamera = Camera.main;
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AnimateImageSprites : MonoBehaviour
{
    // ---- / Serialized Variables / ---- //
    [SerializeField] private Sprite _usedLeftClick;
    [SerializeField] private Sprite _blinkLeftClick;
    [SerializeField] private float _timePeriod = 1f;
    
    // ---- / Private Variables / ---- //
    private Image _image;
    
    private Coroutine _pingPongCoroutine;

    private void Start()
    {
        _image = GetComponentInChildren<Image>();

        _pingPongCoroutine = StartCoroutine(PingPongSprite());
    }

    private IEnumerator PingPongSprite()
    {
        while (true)
        {
            _image.sprite = _usedLeftClick;
            yield return new WaitForSeconds(_timePeriod);

            _image.sprite = _blinkLeftClick;
            yield return new WaitForSeconds(_timePeriod);
        }
    }

    private void OnDisable()
    {
        if (_pingPongCoroutine != null)
        {
            StopCoroutine(_pingPongCoroutine);
        }
    }
}

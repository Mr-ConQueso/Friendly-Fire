using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class BlinkTransition : MonoBehaviour
{
    // ---- / Serialized Variables / ---- //
    [SerializeField] private float blinkDuration = 0.5f;
    [SerializeField] private Volume volume;
    
    // ---- / Private Variables / ---- //
    private BlurSettings _blurSettings;
    private Animator _animator;
    private bool _hasBlinked;
    
    public void StoppedBlinking()
    {
        _hasBlinked = true;
    }

    private void OnEnable()
    {
        GameController.OnChangeTurn += OnChangeTurn;
    }
    
    private void OnDisable()
    {
        GameController.OnChangeTurn -= OnChangeTurn;
    }
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
        if (!volume.profile.TryGet<BlurSettings>(out _blurSettings))
        {
            Debug.LogError("Missing blur settings");
        }
    }

    private void OnChangeTurn()
    {
        StartCoroutine(Blink(blinkDuration));
    }

    private void StartBlinking()
    {
        _animator.SetTrigger("startBlink");
        StartCoroutine(LerpBlur(0f, 15f));
    }
    
    private void EndBlinking()
    {
        _animator.SetTrigger("endBlink");
        _hasBlinked = false;
        StartCoroutine(LerpBlur(15f, 0f));
    }
    
    private IEnumerator Blink(float duration)
    {
        StartBlinking();

        while (_hasBlinked == false)
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(duration);
        
        switch (GameController.Instance.CurrentTurn)
        {
            case PlayerType.Player1:
            {
                GameController.Instance.InvokeStartTurn1();
                break;
            }
            case PlayerType.Player2:
            {
                GameController.Instance.InvokeStartTurn2();
                break;
            }
        }
        
        EndBlinking();
    }

    private IEnumerator LerpBlur(float startValue, float endValue)
    {
        float elapsedTime = 0f;
        float duration = _animator.GetCurrentAnimatorClipInfo(0).Length + 0.2f;
        
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            _blurSettings.strength.value = Mathf.Lerp(startValue, endValue, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _blurSettings.strength.value = endValue;
    }
}

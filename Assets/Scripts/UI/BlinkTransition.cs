using System.Collections;
using UnityEngine;

public class BlinkTransition : MonoBehaviour
{
    // ---- / Serialized Variables / ---- //
    [SerializeField] private float blinkDuration = 0.5f;
    
    // ---- / Private Variables / ---- //
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
    }

    private void OnChangeTurn()
    {
        StartCoroutine(Blink(blinkDuration));
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

    private void StartBlinking()
    {
        _animator.SetTrigger("startBlink");
    }
    
    private void EndBlinking()
    {
        _animator.SetTrigger("endBlink");
        _hasBlinked = false;
    }
}

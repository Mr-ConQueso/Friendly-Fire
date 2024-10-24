using UnityEngine;

public class TurnCard : MonoBehaviour
{
    // ---- / Static Variables / ---- //
    private static readonly int HoverUp = Animator.StringToHash("hoverUp");
    private static readonly int HoverDown = Animator.StringToHash("hoverDown");
    private static readonly int TurnDown = Animator.StringToHash("turnDown");
    private static readonly int TurnUp = Animator.StringToHash("turnUp");
    
    // ---- / Private Variables / ---- //
    private Animator _animator;
    private bool _isTurnedUp;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnMouseOver()
    {
        _animator.SetTrigger(HoverUp);
    }

    private void OnMouseExit()
    {
        _animator.SetTrigger(HoverDown);
    }

    private void OnMouseDown()
    {
        if (!_isTurnedUp)
        {
            _animator.SetTrigger(TurnUp);
            _isTurnedUp = true;
        }
        else
        {
            _animator.SetTrigger(TurnDown);
            _isTurnedUp = false;
        }
    }
}

using UnityEngine;

public class TurnCard : MonoBehaviour
{
    // ---- / Static Variables / ---- //
    private static readonly int TurnDown = Animator.StringToHash("turnDown");
    private static readonly int TurnUp = Animator.StringToHash("turnUp");
    
    // ---- / Serialized Variables / ---- //
    [SerializeField] private float lerpDuration = 0.5f;
    [SerializeField] private Vector3 endPosition;
    [SerializeField] private float endScale;
    
    // ---- / Private Variables / ---- //
    private Animator _animator;
    private bool _isTurnedUp;
    private Vector3 _startPosition;
    private Vector3 _startScale;
    private float _lerpTime = 0f;
    private bool _isLerping = false;
    private bool _isReversing = false; 
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _startPosition = transform.position;
        _startScale = transform.localScale;
    }

    private void Update()
    {
        if (_isLerping || _isReversing)
        {
            _lerpTime += Time.deltaTime / lerpDuration;

            if (_lerpTime <= 1f)
            {
                if (_isLerping)
                {
                    transform.position = Vector3.Lerp(_startPosition, endPosition, _lerpTime);
                    transform.localScale = Vector3.Lerp(_startScale, _startScale * endScale, _lerpTime);
                }
                else if (_isReversing)
                {
                    transform.position = Vector3.Lerp(endPosition, _startPosition, _lerpTime);
                    transform.localScale = Vector3.Lerp(_startScale * endScale, _startScale, _lerpTime);
                }
            }
            else
            {
                _isLerping = false;
                _isReversing = false;
                _lerpTime = 0f;
            }
        }
    }

    private void OnMouseEnter()
    {
        _isLerping = true;
        _isReversing = false;
        _lerpTime = 0f;
    }

    private void OnMouseExit()
    {
        _isReversing = true;
        _isLerping = false;
        _lerpTime = 0f;
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

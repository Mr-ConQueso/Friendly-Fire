using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectionHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    // ---- / Serialized Variables / ---- //
    [SerializeField] private float _horizontalMoveAmount = 30.0f;
    [SerializeField] private float _moveTime = 0.1f;
    [Range(0f, 2f), SerializeField] private float _scaleAmount = 1.1f;
    
    // ---- / Private Variables / ---- //
    private Vector3 _startPos;
    private Vector3 _startScale;

    private void Start()
    {
        _startPos = transform.position;
        _startScale = transform.localScale;
    }

    private IEnumerator MoveButton(bool isStartingAnimation)
    {
        float elapsedTime = 0f;
        while (elapsedTime < _moveTime)
        {
            elapsedTime += Time.deltaTime;

            Vector3 endPosition;
            Vector3 endScale;
            if (isStartingAnimation)
            {
                endPosition = _startPos + new Vector3(_horizontalMoveAmount, 0, 0);
                endScale = _startScale * _scaleAmount;
            }
            else
            {
                endPosition = _startPos;
                endScale = _startScale;
            }

            Vector3 lerpedPos = Vector3.Lerp(transform.position, endPosition, (elapsedTime / _moveTime));
            Vector3 lerpedScale = Vector3.Lerp(transform.localScale, endScale, (elapsedTime / _moveTime));

            transform.position = lerpedPos;
            transform.localScale = lerpedScale;

            yield return null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.selectedObject = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        eventData.selectedObject = null;
    }

    public void OnSelect(BaseEventData eventData)
    {
        StartCoroutine(MoveButton(true));
        MenuSelector.Instance.lastSelected = gameObject;

        for (int i = 0; i < MenuSelector.Instance.SelectableItems.Count; i++)
        {
            if (MenuSelector.Instance.SelectableItems[i] == gameObject)
            {
                MenuSelector.Instance.lastSelectedIndex = i;
                return;
            }
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        StartCoroutine(MoveButton(false));
    }
}

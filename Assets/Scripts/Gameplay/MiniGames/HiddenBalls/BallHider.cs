using Gameplay.Helper;

namespace Gameplay.MiniGames.HiddenBalls
{
    public class BallHider : HoverableObject
    {
        private void Awake()
        {
            HiddenBallsController.OnToggleBallsInteraction += OnToggleBallsInteraction;
            HiddenBallsController.OnSetBallsTransform += OnSetTransform;
        }

        private void OnDestroy()
        {
            HiddenBallsController.OnToggleBallsInteraction -= OnToggleBallsInteraction;
            HiddenBallsController.OnSetBallsTransform -= OnSetTransform;
        }

        private void OnToggleBallsInteraction(bool willInteract)
        {
            if (willInteract)
            {
                EnableInteractable();
            }
            else
            {
                DisableInteractable();
            }
        }
        
        private void OnSetTransform()
        {
            StartPosition = transform.position;
            StartScale = transform.localScale;
        }

        private void OnMouseDown()
        {
            if (!isInteractable) return;
            
            StartCoroutine(LerpTo(StartPosition + EndPosition, StartScale * EndScale));
            HiddenBallsController.Instance.CheckCorrectAnswer(this);
        }
    }
}
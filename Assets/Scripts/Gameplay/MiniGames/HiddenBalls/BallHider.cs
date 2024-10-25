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
                IsInteractable = true;
            }
            else
            {
                DisableInteractable();
            }
        }
        
        public override void OnMouseEnter() { return; }

        public override void OnMouseOver() { return; }

        public override void OnMouseExit() { return; }

        private void OnSetTransform()
        {
            StartPosition = transform.position;
            StartScale = transform.localScale;
        }

        private void OnMouseDown()
        {
            if (!IsInteractable) return;
            
            StartCoroutine(LerpTo(StartPosition + endPosition, StartScale * endScale));
            HiddenBallsController.Instance.CheckCorrectAnswer();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.MiniGames.HiddenBalls
{
    public class HiddenBallsController : MonoBehaviour
    {
        // ---- / Singleton / ---- //
        public static HiddenBallsController Instance;
    
        // ---- / Events / ---- //
        public delegate void ToggleBallsInteractionEventHandler(bool willInteract);
        public static event ToggleBallsInteractionEventHandler OnToggleBallsInteraction;
        
        public delegate void SetBallsTransformEventHandler();
        public static event SetBallsTransformEventHandler OnSetBallsTransform;
        
        // ---- / Public Variables / ---- //
        [SerializeField] public List<BallHider> BallHiders;
        
        // ---- / Serialized Variables / ---- //
        [SerializeField] private int _maxRounds = 5;
        [SerializeField] private float _rotationDuration = 0.75f;
        [SerializeField] private float _timeBetweenRounds = 0.5f;
        [SerializeField] private GameObject _ballPrefab;

        // ---- / Private Variables / ---- //
        private int _previousIndex1 = -1;
        private int _previousIndex2 = -1;
        private bool _canBallMove = true;
        private GameObject _ball;
        
        public void CheckCorrectAnswer(BallHider hider)
        {
            if (BallHiders.Count == 0) return;

            Invoke(BallHiders[0] == hider ? nameof(WinGame) : nameof(RestartGame), 1.0f);
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            DevConsole.RegisterConsoleCommand(this, "moverandomglass");
            StartCoroutine(SwitchMultipleTimes());
            
            _ball = Instantiate(_ballPrefab, transform.position, transform.rotation);
        }

        private void Update()
        {
            if (_canBallMove)
            {
                _ball.transform.position = BallHiders[0].transform.position;
            }
        }
        
        private void RestartGame()
        {
            Debug.Log("You lose");
            //StartCoroutine(SwitchMultipleTimes());
            OnToggleBallsInteraction?.Invoke(false);
        }

        private void WinGame()
        {
            Debug.Log("You win");
            OnToggleBallsInteraction?.Invoke(false);
        }

        private void MoveRandomBallHiders()
        {
            _canBallMove = true;

            int randomIndex;
            int randomIndex2;
            do
            {
                randomIndex = Random.Range(0, BallHiders.Count);
                randomIndex2 = Random.Range(0, BallHiders.Count);
                
            } while (randomIndex == randomIndex2 || 
                     (randomIndex == _previousIndex1 && randomIndex2 == _previousIndex2) || 
                     (randomIndex == _previousIndex2 && randomIndex2 == _previousIndex1));
            
            _previousIndex1 = randomIndex;
            _previousIndex2 = randomIndex2;

            float angle = Random.value > 0.5f ? 180f : -180f;

            Vector3 middlePosition = (BallHiders[randomIndex].transform.position + BallHiders[randomIndex2].transform.position) / 2;
            StartCoroutine(RotateAroundPivot(BallHiders[randomIndex].transform, BallHiders[randomIndex2].transform, middlePosition, angle, _rotationDuration));
        }

        private IEnumerator SwitchMultipleTimes()
        {
            OnToggleBallsInteraction?.Invoke(false);

            for (int i = 0; i < _maxRounds; i++)
            {
                MoveRandomBallHiders();
                yield return new WaitForSeconds(_rotationDuration + _timeBetweenRounds);
            }
            StartPlayerTurn();
        }

        private void StartPlayerTurn()
        {
            OnSetBallsTransform?.Invoke();
            OnToggleBallsInteraction?.Invoke(true);
            _canBallMove = false;
        }

        private static IEnumerator RotateAroundPivot(Transform obj1, Transform obj2, Vector3 pivot, float angle, float duration)
        {
            float elapsedTime = 0f;
            float totalRotation = 0f;

            while (elapsedTime < duration)
            {
                float step = (angle / duration) * Time.deltaTime;

                obj1.RotateAround(pivot, Vector3.up, step);
                obj2.RotateAround(pivot, Vector3.up, step);

                totalRotation += step;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            float correction = angle - totalRotation;
            obj1.RotateAround(pivot, Vector3.up, correction);
            obj2.RotateAround(pivot, Vector3.up, correction);
        }
        
        // ---- / Console Commands / ---- //
        private void OnConsoleCommand_moverandomglass(NotificationCenter.Notification n)
        {
            MoveRandomBallHiders();
        }
    }
}
using System;
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
        
        public void CheckCorrectAnswer()
        {
            if (BallHiders.Count == 0) return;
            
            // ToDo: Check if the balls are in the correct order
            int correctIndex = BallHiders.FindIndex(ballHider => ballHider.transform.position == BallHiders[0].transform.position);
            if (correctIndex != 0)
            {
                Invoke(nameof(WinGame), 1.0f);
            }
            else
            {
                Invoke(nameof(RestartGame), 1.0f);
            }
        }

        private void RestartGame()
        {
            Debug.Log("You lose");
            StartCoroutine(SwitchMultipleTimes());
        }

        private void WinGame()
        {
            Debug.Log("You win");
            //StartCoroutine(SwitchMultipleTimes());
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

        private void MoveRandomBallHiders()
        {
            _canBallMove = true;

            int randomIndex = 0;
            int randomIndex2 = 0;
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

        private IEnumerator RotateAroundPivot(Transform obj1, Transform obj2, Vector3 pivot, float angle, float duration)
        {
            float elapsedTime = 0f;
            Quaternion startRotation1 = obj1.rotation;
            Quaternion startRotation2 = obj2.rotation;

            while (elapsedTime < duration)
            {
                float step = (angle / duration) * Time.deltaTime;
                obj1.RotateAround(pivot, Vector3.up, step);
                obj2.RotateAround(pivot, Vector3.up, step);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            obj1.RotateAround(pivot, Vector3.up, angle - elapsedTime * (angle / duration));
            obj2.RotateAround(pivot, Vector3.up, angle - elapsedTime * (angle / duration));
        }
        
        private void OnConsoleCommand_moverandomglass(NotificationCenter.Notification n)
        {
            MoveRandomBallHiders();
        }
    }
}
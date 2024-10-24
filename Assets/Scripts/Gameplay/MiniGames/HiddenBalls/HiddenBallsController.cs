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
        
        // ---- / Public Variables / ---- //
        [SerializeField] public List<BallHider> BallHiders;
        
        // ---- / Serialized Variables / ---- //
        [SerializeField] private int maxRounds = 5;
        [SerializeField] private float rotationDuration = 0.75f;
        [SerializeField] private float timeBetweenRounds = 0.5f;
        
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
        }
                
        private void MoveRandomBallHiders()
        {
            int randomIndex = 0;
            int randomIndex2 = 0;
            do
            {
                randomIndex = Random.Range(0, BallHiders.Count);
                randomIndex2 = Random.Range(0, BallHiders.Count);
                
            } while (randomIndex == randomIndex2);
            
            float angle = Random.value > 0.5f ? 180f : -180f;

            Vector3 middlePosition = (BallHiders[randomIndex].transform.position + BallHiders[randomIndex2].transform.position) / 2;
            StartCoroutine(RotateAroundPivot(BallHiders[randomIndex].transform, BallHiders[randomIndex2].transform, middlePosition, angle, rotationDuration));
        }

        private IEnumerator SwitchMultipleTimes()
        {
            for (int i = 0; i < maxRounds; i++)
            {
                MoveRandomBallHiders();
                yield return new WaitForSeconds(rotationDuration + timeBetweenRounds);
            }
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

            (obj1.position, obj2.position) = (obj2.position, obj1.position);
        }
        
        private void OnConsoleCommand_moverandomglass(NotificationCenter.Notification n)
        {
            MoveRandomBallHiders();
        }
    }
}
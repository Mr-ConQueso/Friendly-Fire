using UnityEngine;

namespace Gameplay.MiniGames
{
    public class MiniGameController : MonoBehaviour
    {
        private void Awake()
        {
            GameController.OnStartMiniGame += OnStartMiniGame;
        }
    
        private void OnDestroy()
        {
            GameController.OnStartMiniGame -= OnStartMiniGame;
        }

        private void OnStartMiniGame()
        {
            
        }
    }
}
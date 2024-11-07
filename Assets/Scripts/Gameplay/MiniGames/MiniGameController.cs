using System;
using BaseGame;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.MiniGames
{
    public class MiniGameController : MonoBehaviour
    {
        private void Awake()
        {
            GameController.OnStartMiniGame += OnStartMiniGame;
        }

        private void Start()
        {
            DevConsole.RegisterConsoleCommand(this, "startminigame");
        }

        private void OnDestroy()
        {
            GameController.OnStartMiniGame -= OnStartMiniGame;
        }

        private void OnStartMiniGame()
        {
            MiniGameType randomMiniGame = GetRandomMiniGameType();
            Debug.Log("Starting MiniGame: " + randomMiniGame);
            SceneSwapManager.SwapScene(randomMiniGame.ToString());
        }

        private MiniGameType GetRandomMiniGameType()
        {
            return (MiniGameType)Random.Range(0, System.Enum.GetValues(typeof(MiniGameType)).Length);
        }
        
        // ---- / Console Commands / ---- //
        private void OnConsoleCommand_startminigame(NotificationCenter.Notification n)
        {
            OnStartMiniGame();
        }
    }
    
    public enum MiniGameType
    {
        Sequence,
        HiddenCards,
        HiddenBalls
    }
}
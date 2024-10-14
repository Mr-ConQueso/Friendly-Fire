using BaseGame;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // ---- / Singleton / ---- //
    public static GameController Instance;
    
    // ---- / Events / ---- //
    public delegate void GameStartEventHandler();
    public static event GameStartEventHandler OnGameStart;
    
    public delegate void GameEndEventHandler();
    public static event GameEndEventHandler OnGameEnd;
    
    public delegate void GamePausedEventHandler();
    public static event GamePausedEventHandler OnGamePaused;
    public delegate void GameResumedEventHandler();
    public static event GameResumedEventHandler OnGameResumed;
    
    // ---- / Public Variables / ---- //
    public bool DEBUG_MODE = true;
    
    // ---- / Hidden Public Variables / ---- //
    [HideInInspector] public bool CanPauseGame = false;
    [HideInInspector] public bool IsPlayerFrozen { get; private set; } = true;
    [HideInInspector] public bool IsGamePaused { get; private set; }

    // ---- / Private Variables / ---- //
    private bool _isGameEnded;

    public void InvokeOnGameResumed()
    {
        IsPlayerFrozen = false;
        IsGamePaused = false;
        OnGameResumed?.Invoke();
    }

    public void InvokeOnGameEnd()
    {
        EndGame();
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
        StartGame();
    }

    private void Update()
    {
        if (InputManager.WasEscapePressed && CanPauseGame)
        {
            IsGamePaused = !IsGamePaused;
            if (IsGamePaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    private void StartGame()
    {
        IsPlayerFrozen = false;
        IsGamePaused = false;
        CanPauseGame = true;
        OnGameStart?.Invoke();
    }

    private void PauseGame()
    {
        IsPlayerFrozen = true;
        OnGamePaused?.Invoke();
    }
    
    private void ResumeGame()
    {
        IsPlayerFrozen = false;
        OnGameResumed?.Invoke();
    }

    private void EndGame()
    {
        if (!_isGameEnded)
        {
            IsPlayerFrozen = true;
            OnGameEnd?.Invoke();
            SceneSwapManager.SwapScene("EndMenu");
            
            _isGameEnded = true;
        }
    }
}
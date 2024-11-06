using BaseGame;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // ---- / Singleton / ---- //
    public static GameController Instance;
    
    // ---- / Events / ---- //
    #region Game Loop Events
    public delegate void GameStartEventHandler();
    public static event GameStartEventHandler OnGameStart;
    
    public delegate void StartMiniGameEventHandler();
    public static event StartMiniGameEventHandler OnStartMiniGame;
    
    public delegate void GameEndEventHandler();
    public static event GameEndEventHandler OnGameEnd;
    
    public delegate void GamePausedEventHandler();
    public static event GamePausedEventHandler OnGamePaused;
    public delegate void GameResumedEventHandler();
    public static event GameResumedEventHandler OnGameResumed;
    
    #endregion
    
    #region Player Events
    
    public delegate void ChangeTurnEventHandler();
    public static event ChangeTurnEventHandler OnChangeTurn;
    
    public delegate void StartTurn1EventHandler();
    public static event StartTurn1EventHandler OnStartTurn1;
    
    public delegate void StartTurn2EventHandler();
    public static event StartTurn2EventHandler OnStartTurn2;
    
    #endregion
    
    // ---- / Public Variables / ---- //
    public bool DEBUG_MODE = true;
    
    // ---- / Hidden Public Variables / ---- //
    [HideInInspector] public bool CanPauseGame = false;
    [HideInInspector] public bool IsPlayerFrozen { get; private set; } = true;
    [HideInInspector] public bool IsGamePaused { get; private set; }
    [HideInInspector] public PlayerType CurrentTurn { get; private set; } = PlayerType.Player1;
    [HideInInspector] public int CurrentRound { get; private set; }
    
    // ---- / Serialized Variables / ---- //
    [SerializeField] private int _maxRounds = 5;
    [SerializeField] private int _shotsPerMiniGame = 3;

    // ---- / Private Variables / ---- //
    private bool _isGameEnded;
    private int _currentShotsInRound;

    public void InvokeOnGameResumed()
    {
        IsPlayerFrozen = false;
        IsGamePaused = false;
        OnGameResumed?.Invoke();
    }

    public void InvokeStartTurn1()
    {
        OnStartTurn1?.Invoke();
    }
    
    public void InvokeStartTurn2()
    {
        OnStartTurn2?.Invoke();
    }

    public void ChangeTurn()
    {
        if (CurrentRound + 1 > _maxRounds)
        {
           EndGame();
           return;
        }

        if (_currentShotsInRound < _shotsPerMiniGame)
        {
            _currentShotsInRound++;
        }
        else
        {
            OnStartMiniGame?.Invoke();
        }

        CurrentRound++;
        switch (CurrentTurn)
        {
            case PlayerType.Player1:
            {
                CurrentTurn = PlayerType.Player2;
                OnChangeTurn?.Invoke();
                break;
            }
            case PlayerType.Player2:
            {
                CurrentTurn = PlayerType.Player1;
                OnChangeTurn?.Invoke();
                break;
            }
        }
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
        DevConsole.RegisterConsoleCommand(this, "round");
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
        OnStartTurn1?.Invoke();
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

    private void OnConsoleCommand_round(NotificationCenter.Notification n)
    {
        string text = (string)n.Data[0];
        if (!string.IsNullOrEmpty(text) && int.TryParse(text, out var result))
        {
            Debug.Log("Current round set to: " + result);
            OnChangeTurn?.Invoke();
            CurrentRound = result;
        }
    }
}
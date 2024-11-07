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
    
    // ---- / Hidden Public Variables / ---- //
    [HideInInspector] public bool CanPauseGame = false;
    public bool isPlayerFrozen { get; private set; } = true;
    public bool isGamePaused { get; private set; }
    public PlayerType currentTurn { get; private set; } = PlayerType.Player1;
    public int currentRound { get; private set; }
    
    // ---- / Serialized Variables / ---- //
    [SerializeField] private int _maxRounds = 5;
    [SerializeField] private int _shotsPerMiniGame = 3;

    // ---- / Private Variables / ---- //
    private bool _isGameEnded;
    private int _currentShotsInRound;

    public void InvokeOnGameResumed()
    {
        isPlayerFrozen = false;
        isGamePaused = false;
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
        if (currentRound + 1 > _maxRounds)
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
            _currentShotsInRound = 0;
        }

        currentRound++;
        switch (currentTurn)
        {
            case PlayerType.Player1:
            {
                currentTurn = PlayerType.Player2;
                OnChangeTurn?.Invoke();
                break;
            }
            case PlayerType.Player2:
            {
                currentTurn = PlayerType.Player1;
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
            isGamePaused = !isGamePaused;
            if (isGamePaused)
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
        isPlayerFrozen = false;
        isGamePaused = false;
        CanPauseGame = true;
        OnGameStart?.Invoke();
        OnStartTurn1?.Invoke();
    }

    private void PauseGame()
    {
        isPlayerFrozen = true;
        OnGamePaused?.Invoke();
    }
    
    private void ResumeGame()
    {
        isPlayerFrozen = false;
        OnGameResumed?.Invoke();
    }

    private void EndGame()
    {
        if (!_isGameEnded)
        {
            isPlayerFrozen = true;
            OnGameEnd?.Invoke();
            SceneSwapManager.SwapScene("EndMenu");
            
            _isGameEnded = true;
        }
    }
    
    // ---- / Console Commands / ---- //
    private void OnConsoleCommand_round(NotificationCenter.Notification n)
    {
        string text = (string)n.Data[0];
        if (!string.IsNullOrEmpty(text) && int.TryParse(text, out var result))
        {
            Debug.Log("Current round set to: " + result);
            OnChangeTurn?.Invoke();
            currentRound = result;
        }
    }
}
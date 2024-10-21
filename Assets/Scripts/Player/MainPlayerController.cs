using UnityEngine;

public class MainPlayerController : MonoBehaviour
{
    // ---- / Serialized Variables / ---- //
    [SerializeField] private int maxPlayerHealth = 3;
    [SerializeField] private PlayerGUIController playerGUIController;
    
    // ---- / Private Variables / ---- //
    private int _currentPlayer1Health, _currentPlayer2Health;
    
    public void AddHealth(int amount)
    {
        switch (GameController.Instance.CurrentTurn)
        {
            case PlayerType.Player1:
            {
                TryAddHealth(_currentPlayer1Health, amount);
                break;
            }
            case PlayerType.Player2:
            {
                TryAddHealth(_currentPlayer2Health, amount);
                break;
            }
        }
    }
    
    public void RemoveHealth(int amount)
    {
        switch (GameController.Instance.CurrentTurn)
        {
            case PlayerType.Player1:
            {
                TryRemoveHealth(_currentPlayer1Health, amount);
                break;
            }
            case PlayerType.Player2:
            {
                TryRemoveHealth(_currentPlayer2Health, amount);
                break;
            }
        }
    }
    
    private void OnEnable()
    {
        GameController.OnStartTurn1 += OnChangeTurn;
        GameController.OnStartTurn2 += OnChangeTurn;
    }
    
    private void OnDisable()
    {
        GameController.OnStartTurn1 -= OnChangeTurn;
        GameController.OnStartTurn2 -= OnChangeTurn;
    }

    private void Start()
    {
        _currentPlayer1Health = maxPlayerHealth;
        _currentPlayer2Health = maxPlayerHealth;
        
        DevConsole.RegisterConsoleCommand(this, "addhealth");
        DevConsole.RegisterConsoleCommand(this, "removehealth");
        
        RefreshHealthBar();
    }

    private void OnChangeTurn()
    {
        RefreshHealthBar();
    }

    private void RefreshHealthBar()
    {
        switch (GameController.Instance.CurrentTurn)
        {
            case PlayerType.Player1:
                playerGUIController.UpdateHealthBar(_currentPlayer1Health, maxPlayerHealth);
                break;
            case PlayerType.Player2:
                playerGUIController.UpdateHealthBar(_currentPlayer2Health, maxPlayerHealth);
                break;
        }
    }

    private void TryAddHealth(int currentHealth, int amount)
    {
        if (currentHealth + amount > maxPlayerHealth)
        {
            Debug.LogWarning("Trying to add more health to max player health");
            return;
        }
        
        currentHealth += amount;
        switch (GameController.Instance.CurrentTurn)
        {
            case PlayerType.Player1:
                _currentPlayer1Health += amount;
                break;
            case PlayerType.Player2:
                _currentPlayer2Health += amount;
                break;
        }
        playerGUIController.UpdateHealthBar(currentHealth, maxPlayerHealth);
    }

    private void TryRemoveHealth(int currentHealth, int amount)
    {
        if (currentHealth - amount <= 0)
        {
            GameController.Instance.InvokeOnGameEnd();
            return;
        }
        
        currentHealth -= amount;
        switch (GameController.Instance.CurrentTurn)
        {
            case PlayerType.Player1:
                _currentPlayer1Health -= amount;
                break;
            case PlayerType.Player2:
                _currentPlayer2Health -= amount;
                break;
        }
        playerGUIController.UpdateHealthBar(currentHealth, maxPlayerHealth);
    }

    private void OnConsoleCommand_addhealth(NotificationCenter.Notification n)
    {
        string text = (string)n.data[0];
        if (!string.IsNullOrEmpty(text) && int.TryParse(text, out var result))
        {
            Debug.Log($"{result} health added to current player");
            AddHealth(result);
        }
    }
    
    private void OnConsoleCommand_removehealth(NotificationCenter.Notification n)
    {
        string text = (string)n.data[0];
        if (!string.IsNullOrEmpty(text) && int.TryParse(text, out var result))
        {
            Debug.Log($"{result} health removed from current player");
            RemoveHealth(result);
        }
    }
}

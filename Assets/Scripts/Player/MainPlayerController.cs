using System;
using UnityEngine;

public class MainPlayerController : MonoBehaviour
{
    // ---- / Serialized Variables / ---- //
    [SerializeField] private int _maxPlayerHealth = 3;
    [SerializeField] private PlayerGUIController _playerGUIController;
    
    // ---- / Private Variables / ---- //
    private int _currentPlayer1Health, _currentPlayer2Health;

    private void AddHealth(int amount)
    {
        switch (GameController.Instance.currentTurn)
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
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public void RemoveHealth(int amount)
    {
        switch (GameController.Instance.currentTurn)
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
            default:
                throw new ArgumentOutOfRangeException();
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
        _currentPlayer1Health = _maxPlayerHealth;
        _currentPlayer2Health = _maxPlayerHealth;
        
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
        switch (GameController.Instance.currentTurn)
        {
            case PlayerType.Player1:
                _playerGUIController.UpdateHealthBar(_currentPlayer1Health, _maxPlayerHealth);
                break;
            case PlayerType.Player2:
                _playerGUIController.UpdateHealthBar(_currentPlayer2Health, _maxPlayerHealth);
                break;
        }
    }

    private void TryAddHealth(int currentHealth, int amount)
    {
        if (currentHealth + amount > _maxPlayerHealth)
        {
            Debug.LogWarning("Trying to add more health to max player health");
            return;
        }
        
        currentHealth += amount;
        switch (GameController.Instance.currentTurn)
        {
            case PlayerType.Player1:
                _currentPlayer1Health += amount;
                break;
            case PlayerType.Player2:
                _currentPlayer2Health += amount;
                break;
        }
        _playerGUIController.UpdateHealthBar(currentHealth, _maxPlayerHealth);
    }

    private void TryRemoveHealth(int currentHealth, int amount)
    {
        if (currentHealth - amount <= 0)
        {
            GameController.Instance.InvokeOnGameEnd();
            return;
        }
        
        currentHealth -= amount;
        switch (GameController.Instance.currentTurn)
        {
            case PlayerType.Player1:
                _currentPlayer1Health -= amount;
                break;
            case PlayerType.Player2:
                _currentPlayer2Health -= amount;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        _playerGUIController.UpdateHealthBar(currentHealth, _maxPlayerHealth);
    }

    // ---- / Console Commands / ---- //
    private void OnConsoleCommand_addhealth(NotificationCenter.Notification n)
    {
        string text = (string)n.Data[0];
        if (string.IsNullOrEmpty(text) || !int.TryParse(text, out var result)) return;
        
        Debug.Log($"{result} health added to current player");
        AddHealth(result);
    }
    
    private void OnConsoleCommand_removehealth(NotificationCenter.Notification n)
    {
        string text = (string)n.Data[0];
        if (string.IsNullOrEmpty(text) || !int.TryParse(text, out var result)) return;
        
        Debug.Log($"{result} health removed from current player");
        RemoveHealth(result);
    }
}

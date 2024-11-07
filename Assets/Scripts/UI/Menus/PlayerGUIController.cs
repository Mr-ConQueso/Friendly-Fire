using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGUIController : MonoBehaviour
{
    // ---- / Serialized Variables / ---- //
    [Header("General")]
    [SerializeField] private TMP_Text _currentRoundText;
    
    [Header("Player 1")]
    [SerializeField] private GameObject _playerGUI1;
    [SerializeField] private Slider _healthBar1;

    [Header("Player 2")]
    [SerializeField] private GameObject _playerGUI2;
    [SerializeField] private Slider _healthBar2;
    
    // ---- / Private Variables / ---- //
    
    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        Debug.Log(GameController.Instance.currentTurn + " health changed : " + currentHealth + "/" + maxHealth);
        switch (GameController.Instance.currentTurn)
        {
            case PlayerType.Player1:
                _healthBar1.value = currentHealth / (float)maxHealth;
                break;
            case PlayerType.Player2:
                _healthBar2.value = currentHealth / (float)maxHealth;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void Awake()
    {
        GameController.OnStartTurn1 += OnStartTurn1;
        GameController.OnStartTurn2 += OnStartTurn2;
    }

    private void OnDestroy()
    {
        GameController.OnStartTurn1 -= OnStartTurn1;
        GameController.OnStartTurn2 -= OnStartTurn2;
    }

    private void OnStartTurn1()
    {
        _playerGUI1.SetActive(true);
        _playerGUI2.SetActive(false);
        UpdateCurrentRoundText();
    }
    
    private void OnStartTurn2()
    {
        _playerGUI1.SetActive(false);
        _playerGUI2.SetActive(true);
        UpdateCurrentRoundText();
    }
    
    private void UpdateCurrentRoundText()
    {
        _currentRoundText.text = GameController.Instance.currentRound.ToString();
    }
}

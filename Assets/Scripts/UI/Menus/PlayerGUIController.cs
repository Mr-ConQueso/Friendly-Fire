using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGUIController : MonoBehaviour
{
    // ---- / Serialized Variables / ---- //
    [Header("General")]
    [SerializeField] private TMP_Text currentRoundText;
    
    [Header("Player 1")]
    [SerializeField] private GameObject playerGUI1;
    [SerializeField] private Slider healthBar1;

    [Header("Player 2")]
    [SerializeField] private GameObject playerGUI2;
    [SerializeField] private Slider healthBar2;
    
    // ---- / Private Variables / ---- //
    
    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        Debug.Log(GameController.Instance.CurrentTurn + " health changed : " + currentHealth + "/" + maxHealth);
        switch (GameController.Instance.CurrentTurn)
        {
            case PlayerType.Player1:
                healthBar1.value = currentHealth / (float)maxHealth;
                break;
            case PlayerType.Player2:
                healthBar2.value = currentHealth / (float)maxHealth;
                break;
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
        playerGUI1.SetActive(true);
        playerGUI2.SetActive(false);
        UpdateCurrentRoundText();
    }
    
    private void OnStartTurn2()
    {
        playerGUI1.SetActive(false);
        playerGUI2.SetActive(true);
        UpdateCurrentRoundText();
    }
    
    private void UpdateCurrentRoundText()
    {
        currentRoundText.text = GameController.Instance.CurrentRound.ToString();
    }
}

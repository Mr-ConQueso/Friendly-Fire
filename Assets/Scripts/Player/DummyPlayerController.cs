using UnityEngine;

public class DummyPlayerController : MonoBehaviour
{
    // ---- / Serialized Variables / ---- //
    [SerializeField] private PlayerType playerType;
    
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
        switch (playerType)
        {
            case PlayerType.Player1:
                gameObject.SetActive(true);
                break;
            case PlayerType.Player2:
                gameObject.SetActive(false);
                break;
        }
    }
    
    private void OnStartTurn2()
    {
        switch (playerType)
        {
            case PlayerType.Player1:
                gameObject.SetActive(false);
                break;
            case PlayerType.Player2:
                gameObject.SetActive(true);
                break;
        }
    }
}

public enum PlayerType
{
    Player1,
    Player2
}

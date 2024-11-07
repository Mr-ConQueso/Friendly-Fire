using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardPairsController : MonoBehaviour
{
    // ---- / Singleton / ---- //
    public static CardPairsController Instance;
    
    // ---- / Events / ---- //
    public delegate void RandomizeCardsEventHandler();
    public static event RandomizeCardsEventHandler OnRandomizeCards;
    
    public delegate void TurnAllCardsEventHandler(bool willTurnUp, bool keepPreviousCards);
    public static event TurnAllCardsEventHandler OnTurnAllCards;
    
    public delegate void ResetAllCardsEventHandler();
    public static event ResetAllCardsEventHandler OnResetAllCards;
    
    public delegate void SetCardsInteractableEventHandler(bool isInteractable);
    public static event SetCardsInteractableEventHandler OnSetCardsInteractable;
    
    // ---- / Public Variables / ---- //
    [SerializeField] public int CardsPerType;
    
    [HideInInspector] public List<CardType> CurrentTurnedCards;
    [HideInInspector] public List<CardType> CardTypes;
    [HideInInspector] public List<GameObject> CompletedCards;
    
    // ---- / Serialized Variables / ---- //
    [SerializeField] private float doubleCardLookTime = 1.0f;
    
    // ---- / Private Variables / ---- //
    private bool _areAllCardsUp = false;
    private int _totalCardCount = 12;
    
    public int GetCardTypeCount(CardType type)
    {
        return CardTypes.Count(card => card == type);
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
        OnRandomizeCards?.Invoke();
        DevConsole.RegisterConsoleCommand(this, "turncards");
        DevConsole.RegisterConsoleCommand(this, "resetcards");
    }

    private void Update()
    {
        if (CurrentTurnedCards.Count >= 2)
        {
            if (CurrentTurnedCards[0] == CurrentTurnedCards[1])
            {
                OnTurnAllCards?.Invoke(false, true);
                CurrentTurnedCards.Clear();
            }
            else
            {
                Invoke(nameof(TurnAllCardsDown), doubleCardLookTime);
                OnSetCardsInteractable?.Invoke(false);
            }
        }

        if (CompletedCards.Count >= _totalCardCount)
        {
            StartCoroutine(ResetAllCards());
            CompletedCards.Clear();
            OnSetCardsInteractable?.Invoke(false);
        }
    }

    private void TurnAllCardsDown()
    {
        OnTurnAllCards?.Invoke(false, false);
        OnSetCardsInteractable?.Invoke(true);
        CurrentTurnedCards.Clear();
    }

    private IEnumerator ResetAllCards()
    {
        yield return new WaitForSeconds(doubleCardLookTime);
        OnResetAllCards?.Invoke();
        OnSetCardsInteractable?.Invoke(true);
        CardTypes.Clear();
        
        yield return new WaitForSeconds(1.0f);

        OnRandomizeCards?.Invoke();
    }
    
    // ---- / Console Commands / ---- //
    private void OnConsoleCommand_turncards(NotificationCenter.Notification n)
    {
        _areAllCardsUp = !_areAllCardsUp;
        if (_areAllCardsUp)
        {
            OnTurnAllCards?.Invoke(true, false);
        }
        else
        {
            OnTurnAllCards?.Invoke(false, false);
        }
    }
    
    private void OnConsoleCommand_resetcards(NotificationCenter.Notification n)
    {
        StartCoroutine(ResetAllCards());
    }
}

public enum CardType
{
    Red,
    Green,
    Blue,
    Yellow,
    Purple,
    Orange
}
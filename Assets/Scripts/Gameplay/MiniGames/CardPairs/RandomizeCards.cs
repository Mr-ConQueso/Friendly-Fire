using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomizeCards : MonoBehaviour
{
    // ---- / Singleton / ---- //
    public static RandomizeCards Instance;
    
    // ---- / Events / ---- //
    public delegate void RandomizeCardsEventHandler();
    public static event RandomizeCardsEventHandler OnRandomizeCards;
    
    // ---- / Public Variables / ---- //
    [SerializeField] public int CardsPerType;

    [HideInInspector] public List<CardType> CardTypes;
    
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
        DevConsole.RegisterConsoleCommand(this, "randomizecards");
    }
    
    private void OnConsoleCommand_randomizecards(NotificationCenter.Notification n)
    {
        CardTypes.Clear();
        OnRandomizeCards?.Invoke();
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
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomizeCards : MonoBehaviour
{
    // ---- / Singleton / ---- //
    public static RandomizeCards Instance;
    
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
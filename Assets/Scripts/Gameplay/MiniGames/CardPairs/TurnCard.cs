using Gameplay.Helper;
using Gameplay.MiniGames.CardPairs;
using UnityEngine;

public class TurnCard : HoverableObject
{
    // ---- / Static Variables / ---- //
    private static readonly int TurnDown = Animator.StringToHash("turnDown");
    private static readonly int TurnUp = Animator.StringToHash("turnUp");
    
    // ---- / Private Variables / ---- //
    private Animator _animator;
    private RandomizeCardType _randomizeCardType;
    private bool _isTurnedUp = false;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _randomizeCardType = GetComponent<RandomizeCardType>();
        CardPairsController.OnTurnAllCards += OnTurnAllCards;
        CardPairsController.OnResetAllCards += OnResetAllCards;
        CardPairsController.OnSetCardsInteractable += OnSetCardsInteractable;
    }

    private void OnDestroy()
    {
        CardPairsController.OnTurnAllCards -= OnTurnAllCards;
        CardPairsController.OnResetAllCards -= OnResetAllCards;
        CardPairsController.OnSetCardsInteractable -= OnSetCardsInteractable;
    }

    private void OnMouseDown()
    {
        if (isInteractable)
        {
            if (!_isTurnedUp)
            {
                TurnCardUp();
                CardPairsController.Instance.CurrentTurnedCards.Add(_randomizeCardType.CurrentCardType);
            }
            else
            {
                TurnCardDown();
                CardPairsController.Instance.CurrentTurnedCards.Remove(_randomizeCardType.CurrentCardType);
            }
        }
    }

    private void OnTurnAllCards(bool willTurnUp, bool keepPreviousCards)
    {
        if (keepPreviousCards && CardPairsController.Instance.CurrentTurnedCards.Contains(_randomizeCardType.CurrentCardType))
        {
            CardPairsController.Instance.CompletedCards.Add(gameObject);
            DisableInteractable();
        }
        else if (isInteractable)
        {
            if (willTurnUp && !_isTurnedUp)
            {
                TurnCardUp();
            }
            else if (!willTurnUp && _isTurnedUp)
            {
                TurnCardDown();
            }
        }
    }
    
    private void OnSetCardsInteractable(bool isinteractable)
    {
        if (isinteractable && !CardPairsController.Instance.CompletedCards.Contains(gameObject))
        {
            EnableInteractable();
        }
        else
        {
            DisableInteractable();
        } 
    }
    
    private void OnResetAllCards()
    {
        EnableInteractable();
        TurnCardDown();
    }
    
    private void TurnCardUp()
    {
        _animator.SetTrigger(TurnUp);
        _isTurnedUp = true;
    }
    
    private void TurnCardDown()
    {
        _animator.SetTrigger(TurnDown);
        _isTurnedUp = false;
    }
}

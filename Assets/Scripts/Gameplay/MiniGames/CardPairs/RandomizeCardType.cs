using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.MiniGames.CardPairs
{
    public class RandomizeCardType : MonoBehaviour
    {
        // ---- / Public Variables / ---- //
        [HideInInspector] public CardType CurrentCardType;
        
        // ---- / Serialized Variables / ---- //
        [SerializeField] private MeshRenderer cardFaceMeshRenderer;

        private void Awake()
        {
            CardPairsController.OnRandomizeCards += OnRandomizeCards;
        }

        private void OnDestroy()
        {
            CardPairsController.OnRandomizeCards -= OnRandomizeCards;
        }

        private void OnRandomizeCards()
        {
            do
            {
                CurrentCardType = GetRandomCardType();
                
            } while (CardPairsController.Instance.GetCardTypeCount(CurrentCardType) >= CardPairsController.Instance.CardsPerType);
            
            CardPairsController.Instance.CardTypes.Add(CurrentCardType);
            cardFaceMeshRenderer.material.color = GetRandomColor();
        }

        private Color GetRandomColor()
        {
            switch (CurrentCardType)
            {
                case CardType.Red:
                    return Color.red;
                case CardType.Green:
                    return Color.green;
                case CardType.Blue:
                    return Color.blue;
                case CardType.Yellow:
                    return Color.yellow;
                case CardType.Purple:
                    return Color.magenta;
                case CardType.Orange:
                    return Color.black;
                default:
                    return Color.white; 
            }
        }

        private CardType GetRandomCardType()
        {
            CardType[] values = (CardType[])Enum.GetValues(typeof(CardType));
            int randomIndex = Random.Range(0, values.Length);
            return values[randomIndex];
        }
    }
}
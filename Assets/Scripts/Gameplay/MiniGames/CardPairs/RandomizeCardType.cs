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
        [SerializeField] private Material[] cardFaceMaterial;

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
            cardFaceMeshRenderer.material = GetRandomMaterial();
        }

        private Material GetRandomMaterial()
        {
            switch (CurrentCardType)
            {
                case CardType.Red:
                    return cardFaceMaterial[0];
                case CardType.Green:
                    return cardFaceMaterial[1];
                case CardType.Blue:
                    return cardFaceMaterial[2];
                case CardType.Yellow:
                    return cardFaceMaterial[3];
                case CardType.Purple:
                    return cardFaceMaterial[4];
                case CardType.Orange:
                    return cardFaceMaterial[5];
                default:
                    return cardFaceMaterial[0]; 
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
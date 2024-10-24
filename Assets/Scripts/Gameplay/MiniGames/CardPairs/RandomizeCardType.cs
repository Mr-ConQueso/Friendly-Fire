using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.MiniGames.CardPairs
{
    public class RandomizeCardType : MonoBehaviour
    {
        // ---- / Private Variables / ---- //
        private CardType _cardType;
        private MeshRenderer _meshRenderer;

        private void Awake()
        {
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
        }

        private void Start()
        {
            do
            {
                _cardType = GetRandomCardType();
                
            } while (RandomizeCards.Instance.GetCardTypeCount(_cardType) > RandomizeCards.Instance.CardsPerType);
            
            RandomizeCards.Instance.CardTypes.Add(_cardType);
            _meshRenderer.material.color = GetRandomColor();
        }

        private Color GetRandomColor()
        {
            switch (_cardType)
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
                    return new Color(255, 129, 0, 1);
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
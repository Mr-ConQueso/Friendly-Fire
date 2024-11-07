using UnityEngine;

namespace Gameplay
{
    public class GunController : MonoBehaviour
    {
        // ---- / Serialized Variables / ---- //
        [SerializeField] private BulletType _currentBulletType;
        
        // ---- / Private Variables / ---- //
        private MainPlayerController _mainPlayerController;

        private void Start()
        {
            _mainPlayerController = GetComponent<MainPlayerController>();
            DevConsole.RegisterConsoleCommand(this, "changebullet");
        }

        private void Update()
        {
            if (InputManager.WasShootPressed)
            {
                if (_currentBulletType == BulletType.Bullet1 && GameController.Instance.currentTurn == PlayerType.Player2)
                {
                    _mainPlayerController.RemoveHealth(1);
                }
                else if (_currentBulletType == BulletType.Bullet2 && GameController.Instance.currentTurn == PlayerType.Player1)
                {
                    _mainPlayerController.RemoveHealth(1);
                }
                else
                {
                    Debug.Log("No Damage was dealt");
                }
                Invoke(nameof(ChangeTurnLater), 3.0f);
            }
        }

        private void ChangeTurnLater()
        {
            GameController.Instance.ChangeTurn();
        }
        
        // ---- / Console Commands / ---- //
        private void OnConsoleCommand_changebullet(NotificationCenter.Notification n)
        {
            string text = (string)n.Data[0];
            if (!string.IsNullOrEmpty(text))
            {
                if (text == "bullet1")
                {
                    _currentBulletType = BulletType.Bullet1;
                }
                else if (text == "bullet2")
                {
                    _currentBulletType = BulletType.Bullet2;
                }
            }
        }
    }

    public enum BulletType
    {
        Bullet1,
        Bullet2
    }
}
using UnityEngine;

namespace BaseGame
{
    public class PersistentObjects : MonoBehaviour
    {
        // ---- / Singleton / ---- //
        private static PersistentObjects _instance;
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
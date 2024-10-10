using BaseGame;
using UnityEngine;

public class EndGameMenu : MonoBehaviour
{
    public void OnClick_Exit()
    {
        SceneSwapManager.SwapScene("StartMenu");
    }
    
    public void OnClick_Credits()
    {
        SceneSwapManager.SwapScene("CreditsMenu");
    }
}

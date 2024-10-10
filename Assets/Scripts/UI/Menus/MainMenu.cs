using BaseGame;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void OnClick_StartGame()
    {
        SceneSwapManager.SwapScene("Level2");
    }

    public void OnClick_Exit()
    {
        Application.Quit();
    }
    
    public void OnClick_Credits()
    {
        SceneSwapManager.SwapScene("CreditsMenu");
    }
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; 
        Screen.fullScreen = true;
    }

    private void Update()
    {
        if (InputManager.WasEscapePressed)
        {
            OnClick_Exit();
        }
    }
}

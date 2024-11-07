using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // ---- / Singleton / ---- //
    public static InputManager Instance;
    
    // ---- / Public Variables / ---- //
    public Vector2 navigationInput { get; private set; }
    
    public static bool WasToggleDevConsolePressed;
    
    public static bool WasEscapePressed;
    
    public static bool WasShootPressed;
    
    // ---- / Private Variables / ---- //
    private static PlayerInput _playerInput;
    
    private InputAction _navigationAction;
    
    private InputAction _devConsoleAction;
    
    private InputAction _escapeAction;
    
    private InputAction _shootAction;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _playerInput = GetComponent<PlayerInput>();
        
        _navigationAction = _playerInput.actions["Navigate"];
        
        _devConsoleAction = _playerInput.actions["ToggleDevConsole"];
        
        _escapeAction = _playerInput.actions["Escape"];
        
        _shootAction = _playerInput.actions["Shoot"];
    }

    private void Update()
    {
        navigationInput = _navigationAction.ReadValue<Vector2>();

        WasToggleDevConsolePressed = _devConsoleAction.WasPerformedThisFrame();

        WasEscapePressed = _escapeAction.WasPressedThisFrame();
        
        WasShootPressed = _shootAction.WasPerformedThisFrame();
    }
}

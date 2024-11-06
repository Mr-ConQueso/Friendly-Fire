using System.Collections;
using System.Collections.Generic;
using Gameplay.MiniGames.HiddenBalls;
using UnityEngine;

public class SequenceController : MonoBehaviour
{
    // ---- / Singleton / ---- //
    public static SequenceController Instance;
    
    // ---- / Events / ---- //
    public delegate void ResetSequencedObjectsEventHandler(bool willActivate);
    public static event ResetSequencedObjectsEventHandler OnResetSequencedObjects;
    
    public delegate void SetSequencedObjectInteractableEventHandler(bool willInteractable);
    public static event SetSequencedObjectInteractableEventHandler OnSetSequencedObjectInteractable;
    
    public delegate void SetSequencedObjectColorEventHandler(Color color);
    public static event SetSequencedObjectColorEventHandler OnSetSequencedObjectColor;
        
    // ---- / Public Variables / ---- //
    [SerializeField] public List<SequencedObject> SequencedObjects;
    
    // ---- / Serialized Variables / ---- //
    [SerializeField] private int _maxRounds = 5;
    [SerializeField] private float _timeBetweenEvents = 0.5f;

    private List<int> _currentSequence = new List<int>();
    private int _currentSelectedIndex = 0;
    private int _lastSelectedIndex;
        
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    private void Start()
    {
        DevConsole.RegisterConsoleCommand(this, "restartsequence");
        
        StartNewGame();
    }

    private void StartNewGame()
    {
        OnResetSequencedObjects?.Invoke(false);
        _currentSequence.Clear();
        _currentSelectedIndex = 0;
        RandomizeSequence();
        CombinationStart();
    }
    
    private void RandomizeSequence()
    {
        _currentSequence.Clear();
        
        for (int i = 0; i < _maxRounds; i++)
        {
            int randomIndex = Random.Range(0, SequencedObjects.Count);
            _currentSequence.Add(randomIndex);
            
            SequencedObjects[randomIndex].ObjectIndex = randomIndex;
        }
    }
    
    public void ClickSequencedObject(int objectIndex)
    {
        if (objectIndex == _currentSequence[_currentSelectedIndex])
        {
            _lastSelectedIndex = _currentSelectedIndex;
            _currentSelectedIndex++;
            if (_currentSelectedIndex >= _currentSequence.Count)
            {
                Win();
            }
        }
        else
        {
            Error();
        }
    }
    
    public void ClearSequence()
    {
        _currentSequence.Clear();
        _currentSelectedIndex = 0;
    }

    private void CombinationStart()
    {
        OnSetSequencedObjectInteractable?.Invoke(false);
        StartCoroutine(ShowSequence());
    }

    IEnumerator ShowSequence()
    {
        foreach (int index in _currentSequence)
        {
            SequencedObject obj = SequencedObjects[index];
            obj.ActivateButton();
            yield return new WaitForSeconds(_timeBetweenEvents);
            obj.DeactivateButton();
            yield return new WaitForSeconds(_timeBetweenEvents);
        }
        OnSetSequencedObjectInteractable?.Invoke(true);
    }

    private void Win()
    {
        Debug.Log("You won!");
        Invoke(nameof(RestartGame), _timeBetweenEvents);
    }

    private void Error()
    {
        OnSetSequencedObjectColor?.Invoke(Color.black);
        Invoke(nameof(RestartGame), _timeBetweenEvents);
    }
    
    private void RestartGame()
    {
        StartNewGame();
    }
    
    private void OnConsoleCommand_restartsequence(NotificationCenter.Notification n)
    {
        StartNewGame();
    }
}

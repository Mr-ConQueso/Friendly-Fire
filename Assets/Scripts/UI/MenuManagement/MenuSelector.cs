using System;
using System.Collections;
using System.Collections.Generic;
using BaseGame;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuSelector : MonoBehaviour
{
    // ---- / Singleton / ---- //
    public static MenuSelector Instance;
    
    // ---- / Public Variables / ---- //
    public GameObject LastSelected { get; set; }
    public int LastSelectedIndex { get; set; }
    public List<GameObject> SelectableItems = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        List<Selectable> selectableChildren = CustomFunctions.GetComponentsInChildren<Selectable>(gameObject);
    
        if (SelectableItems.Count == 0 && selectableChildren.Count > 0)
        {
            SelectableItems.Clear();
            for (int i = 0; i < selectableChildren.Count; i++)
            {
                SelectableItems.Add(selectableChildren[i].gameObject);
            }
        }
        else if (selectableChildren.Count == 0)
        {
            Debug.LogWarning($"No selectable items within the menu: {gameObject.name}");
        }
    }
    
    private void OnEnable()
    {
        StartCoroutine(SelectAfterOneFrame());
    }
    
    private void Update()
    {
        if (InputManager.Instance.NavigationInput.x > 0)
        {
            HandleNextButtonSelection(1);
        }
        if (InputManager.Instance.NavigationInput.x < 0)
        {
            HandleNextButtonSelection(-1);
        }
    }

    private void HandleNextButtonSelection(int addition)
    {
        if (EventSystem.current.currentSelectedGameObject == null && LastSelected != null)
        {
            int newIndex = LastSelectedIndex + addition;
            newIndex = Mathf.Clamp(newIndex, 0, SelectableItems.Count - 1);
            EventSystem.current.SetSelectedGameObject(SelectableItems[newIndex]);
        }
    }

    private IEnumerator SelectAfterOneFrame()
    {
        if (SelectableItems == null || SelectableItems.Count == 0)
        {
            yield break;
        }
        EventSystem.current.SetSelectedGameObject(SelectableItems[0]);
        yield return null;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SaveLoad;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DevConsole : MonoBehaviour
{
	// ---- / Singleton / ---- //
	private static DevConsole _instance;
    
	// ---- / Serialized Variables / ---- //
	[SerializeField] private TMP_InputField inputField;
	
	// ---- / Private Variables / ---- //
    private class CommandData
    {
        public bool CaseSensitive;

        public bool CombineArgs;

        public Component Observer;
    }
    
    private const int CharacterLimit = 256;
    private const int MaxHistory = 10;
    private static readonly Dictionary<string, CommandData> Commands = new Dictionary<string, CommandData>(StringComparer.InvariantCultureIgnoreCase);
    private List<string> _history = new List<string>();
    private bool _isConsoleShown = false;
    private int _historyIndex = 0;
    
    private void Awake()
	{
		if (_instance != null)
		{
			Debug.LogError("Multiple DevConsole instances detected!");
			Destroy(this);
			return;
		}
		_instance = this;
		LoadHistory();
		SceneManager.sceneLoaded += OnSceneChange;
	}
    
	private void OnSceneChange(Scene scene, LoadSceneMode mode)
	{
		if (mode != 0)
		{
			return;
		}
		using ListPool<string> listPool = Pool<ListPool<string>>.Get();
		List<string> list = listPool.list;
		foreach (KeyValuePair<string, CommandData> command in Commands)
		{
			string key = command.Key;
			CommandData value = command.Value;
			if (value.Observer == null)
			{
				list.Add(key);
			}
			else
			{
				NotificationCenter.DefaultCenter.AddObserver(value.Observer, "OnConsoleCommand_" + key);
			}
		}
		foreach (string item in list)
		{
			Commands.Remove(item);
		}
	}

	private void Start()
	{
		RegisterConsoleCommand(this, "commands");
		RegisterConsoleCommand(this, "clearhistory");
		RegisterConsoleCommand(this, "developermode");
	}
	
	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnSceneChange;
	}
	
	private void Update()
	{
		if (InputManager.WasToggleDevConsolePressed)
		{
			_isConsoleShown = !_isConsoleShown;
			if (_isConsoleShown)
			{
				ShowDevConsole();
			}
			else if (!_isConsoleShown)
			{
				HideDevConsole();
			}
		}

		if (_isConsoleShown)
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				SetPreviousCommand();
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				SetNextCommand();
			}
		}
	}

	private void OnDisable()
	{
		SaveHistory();
	}

	private void ShowDevConsole()
	{
		inputField.gameObject.SetActive(true);
		EventSystem.current.SetSelectedGameObject(inputField.gameObject);
	}
	
	private void HideDevConsole()
	{
		inputField.text = null;
		inputField.gameObject.SetActive(false);
		EventSystem.current.SetSelectedGameObject(null);
		_isConsoleShown = false;
	}

	private void SetPreviousCommand()
	{
		if (_historyIndex == 0)
		{
			Debug.Log("previous command: " + _history[_historyIndex]);
			inputField.text = _history[_historyIndex];
		}
		if (_historyIndex > 0)
		{
			Debug.Log("previous command: " + _history[_historyIndex]);
			inputField.text = _history[_historyIndex];
			_historyIndex--;	
		}
	}
	
	private void SetNextCommand()
	{
		if (_historyIndex == _history.Count)
		{
			inputField.text = _history[_historyIndex];
		}
		if (_historyIndex < _history.Count)
		{
			_historyIndex++;
			inputField.text = _history[_historyIndex];
		}
	}

	public static void RegisterConsoleCommand(Component originator, string command, bool caseSensitiveArgs = false, bool combineArgs = false)
	{
		if (!Commands.ContainsKey(command))
		{
			CommandData commandData = new CommandData();
			commandData.CaseSensitive = caseSensitiveArgs;
			commandData.CombineArgs = combineArgs;
			commandData.Observer = originator;
			Commands.Add(command, commandData);
		}
		NotificationCenter.DefaultCenter.AddObserver(originator, "OnConsoleCommand_" + command);
	}

	public void SendConsoleCommand(string value)
	{
		if (_instance.Submit(value))
		{
			HideDevConsole();
		}
		else
		{
			Debug.LogWarning($"Command {value} not found!");
			inputField.text = string.Empty;
			EventSystem.current.SetSelectedGameObject(inputField.gameObject);
		}
	}
	
	private bool Submit(string value)
	{
		char[] separator = new char[2] { ' ', '\t' };
		string text = value.Trim();
		string[] array = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length == 0)
		{
			return false;
		}
		string text2 = array[0];
		if (Commands.TryGetValue(text2, out var value2))
		{
			bool caseSensitive = value2.CaseSensitive;
			bool combineArgs = value2.CombineArgs;
			Hashtable hashtable = null;
			if (combineArgs)
			{
				text = text.Substring(text2.Length).Trim();
				if (!caseSensitive)
				{
					text = text.ToLower();
				}
				if (text.Length > 0)
				{
					hashtable = new Hashtable();
					hashtable.Add(0, text);
				}
			}
			else if (array.Length > 1)
			{
				hashtable = new Hashtable();
				for (int i = 1; i < array.Length; i++)
				{
					hashtable[i - 1] = (caseSensitive ? array[i] : array[i].ToLower());
				}
			}
			if (hashtable != null)
			{
				NotificationCenter.DefaultCenter.PostNotification(this, "OnConsoleCommand_" + text2, hashtable);
			}
			else
			{
				NotificationCenter.DefaultCenter.PostNotification(this, "OnConsoleCommand_" + text2);
			}
			AddToHistory(value);
			
			return true;
		}
		return false;
	}

	private bool OnSubmit(string value)
	{
		bool result = false;
		if (!string.IsNullOrEmpty(value))
		{
			result = Submit(value);
		}
		return result;
	}

	private void AddToHistory(string command)
	{
		if (_history.Count < MaxHistory)
		{
			_history.Add(command);
			_historyIndex = _history.Count - 1;	
		}
	}

	private void OnConsoleCommand_developermode()
	{
		/*
		IngameMenu main = IngameMenu.main;
		if ((bool)main)
		{
			main.ActivateDeveloperMode();
		}
		*/
	}

	private void OnConsoleCommand_commands()
	{
		StringBuilder stringBuilder = new StringBuilder();
		Dictionary<string, CommandData>.KeyCollection.Enumerator enumerator = Commands.Keys.GetEnumerator();
		int num = 0;
		while (enumerator.MoveNext())
		{
			stringBuilder.Append(enumerator.Current);
			stringBuilder.Append(" ");
			if (num % 4 == 0)
			{
				stringBuilder.AppendLine();
			}

			num++;
		}
		
		Debug.Log("Console commands: " + stringBuilder.ToString());
	}
	
	private void OnConsoleCommand_clearhistory()
	{
		_history.Clear();
		//inputField.SetHistory(history);
		MiscSettings.consoleHistory = string.Empty;
	}
	
	private void SaveHistory()
	{
		if (_history == null)
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		int i = Mathf.Max(0, _history.Count - 10);
		for (int num = _history.Count - 1; i <= num; i++)
		{
			string value = _history[i];
			if (i == num)
			{
				stringBuilder.Append(value);
			}
			else
			{
				stringBuilder.AppendLine(value);
			}
		}
		string consoleHistory = stringBuilder.ToString();
		MiscSettings.consoleHistory = consoleHistory;
	}

	private void LoadHistory()
	{
		_history.Clear();
		string consoleHistory = MiscSettings.consoleHistory;
		if (!string.IsNullOrEmpty(consoleHistory))
		{
			string[] collection = consoleHistory.Split(new string[2] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
			_history.AddRange(collection);
		}
	}
}

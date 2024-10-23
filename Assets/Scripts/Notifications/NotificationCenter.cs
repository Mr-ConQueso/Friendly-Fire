using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationCenter : MonoBehaviour
{
	public class Notification
	{
		public readonly Component Sender;
		public string Name;
		public Hashtable Data;

		public C GetSenderComponent<C>() where C : Component
		{
			return Sender.gameObject.GetComponent<C>();
		}

		public Notification(Component aSender, string aName)
		{
			Sender = aSender;
			Name = aName;
			Data = null;
		}

		public Notification(Component aSender, string aName, Hashtable aData)
		{
			Sender = aSender;
			Name = aName;
			Data = aData;
		}
	}

	private static NotificationCenter _defaultCenter;
	private readonly Hashtable _notifications = new Hashtable();

	public static NotificationCenter DefaultCenter
	{
		get
		{
			if (!_defaultCenter)
			{
				_defaultCenter = new GameObject("Default Notification Center").AddComponent<NotificationCenter>();
			}
			return _defaultCenter;
		}
	}

	public void AddObserver(Component observer, string name)
	{
		AddObserver(observer, name, null);
	}

	public void AddObserver(Component observer, string name, Component sender)
	{
		if (string.IsNullOrEmpty(name))
		{
			Debug.Log("Null name specified for notification in AddObserver.");
			return;
		}
		if (_notifications[name] == null)
		{
			_notifications[name] = new HashSet<Component>();
		}
		(_notifications[name] as HashSet<Component>)?.Add(observer);
	}

	public void RemoveObserver(Component observer, string name)
	{
		HashSet<Component> hashSet = (HashSet<Component>)_notifications[name];
		if (hashSet != null)
		{
			hashSet.Remove(observer);
			if (hashSet.Count == 0)
			{
				_notifications.Remove(name);
			}
		}
	}

	public void PostNotification(Component aSender, string aName)
	{
		PostNotification(aSender, aName, null);
	}

	public void PostNotification(Component aSender, string aName, Hashtable aData)
	{
		PostNotification(new Notification(aSender, aName, aData));
	}

	public void PostNotification(Notification aNotification)
	{
		if (string.IsNullOrEmpty(aNotification.Name))
		{
			Debug.Log("Null name sent to PostNotification.");
			return;
		}
		HashSet<Component> hashSet = (HashSet<Component>)_notifications[aNotification.Name];
		if (hashSet == null)
		{
			Debug.Log("Notify list not found in PostNotification: " + aNotification.Name);
			return;
		}
		List<Component> list = new List<Component>();
		foreach (Component item in hashSet)
		{
			if (!item)
			{
				list.Add(item);
			}
			else
			{
				item.SendMessage(aNotification.Name, aNotification, SendMessageOptions.DontRequireReceiver);
			}
		}
		foreach (Component item2 in list)
		{
			hashSet.Remove(item2);
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationCenter : MonoBehaviour
{
	public class Notification
	{
		public Component sender;

		public string name;

		public Hashtable data;

		public C GetSenderComponent<C>() where C : Component
		{
			return sender.gameObject.GetComponent<C>();
		}

		public Notification(Component aSender, string aName)
		{
			sender = aSender;
			name = aName;
			data = null;
		}

		public Notification(Component aSender, string aName, Hashtable aData)
		{
			sender = aSender;
			name = aName;
			data = aData;
		}
	}

	private static NotificationCenter defaultCenter;

	private Hashtable notifications = new Hashtable();

	public static NotificationCenter DefaultCenter
	{
		get
		{
			if (!defaultCenter)
			{
				defaultCenter = new GameObject("Default Notification Center").AddComponent<NotificationCenter>();
			}
			return defaultCenter;
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
		if (notifications[name] == null)
		{
			notifications[name] = new HashSet<Component>();
		}
		(notifications[name] as HashSet<Component>).Add(observer);
	}

	public void RemoveObserver(Component observer, string name)
	{
		HashSet<Component> hashSet = (HashSet<Component>)notifications[name];
		if (hashSet != null)
		{
			hashSet.Remove(observer);
			if (hashSet.Count == 0)
			{
				notifications.Remove(name);
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
		if (string.IsNullOrEmpty(aNotification.name))
		{
			Debug.Log("Null name sent to PostNotification.");
			return;
		}
		HashSet<Component> hashSet = (HashSet<Component>)notifications[aNotification.name];
		if (hashSet == null)
		{
			Debug.Log("Notify list not found in PostNotification: " + aNotification.name);
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
				item.SendMessage(aNotification.name, aNotification, SendMessageOptions.DontRequireReceiver);
			}
		}
		foreach (Component item2 in list)
		{
			hashSet.Remove(item2);
		}
	}
}
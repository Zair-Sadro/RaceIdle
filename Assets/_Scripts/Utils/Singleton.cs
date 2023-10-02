using UnityEngine;

public class Singleton<T> : MonoBehaviour where T:Component
{
	private static T _instance;
	public static T Instance
	{
		get 
		{ 
			if(_instance == null)
			{
				Init();
			}
			return _instance; 
		}
		
		private set 
		{ 
			_instance = value; 
		}
	}
	
	private static void Init()
	{
		_instance = FindObjectOfType<T>();
		if(_instance == null)
		{
			Debug.LogError("NULL");
		}
	}
}

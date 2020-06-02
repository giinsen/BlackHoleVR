using UnityEngine;

public abstract class SGT_Singleton<T> : SGT_MonoBehaviour
	where T : SGT_Singleton<T>
{
	/*[SerializeField]*/
	private static T instance;
	
	public static T Instance
	{
		get
		{
			RequireInstance();
			
			return instance;
		}
	}
	
	public void Awake()
	{
		var instances = SGT_Helper.FindAll<T>();
		
		if (instances.Length > 1)
		{
			Debug.LogWarning("An instance of " + typeof(T).Name + " already exists!");
			Debug.Log("Removing other " + (instances.Length - 1) + " instance(s) of " + typeof(T).Name);
			
			// Destroy other instances
			foreach (var i in instances)
			{
				if (i != this)
				{
					SGT_Helper.DestroyObject(i);
				}
			}
		}
		
		instance = (T)this;
	}
	
	public static void RequireInstance()
	{
		if (instance == null)
		{
			instance = SGT_Helper.Find<T>();
			
			if (instance == null)
			{
				new GameObject(typeof(T).Name).AddComponent<T>();
				
				if (instance == null)
				{
					Debug.LogError("Something went wrong!");
				}
			}
		}
	}
}
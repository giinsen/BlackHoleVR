using UnityEngine;
using System.Reflection;

public abstract class SGT_MonoBehaviourUnique<T> : SGT_MonoBehaviour
	where T : SGT_MonoBehaviourUnique<T>
{
	public enum AwakeState
	{
		AwakeOriginal,
		AwakeDuplicate,
		AwakeAgain
	}
	
	/*[SerializeField]*/
	private static bool staticInit;
	
	/*[SerializeField]*/
	private static System.Collections.Generic.HashSet<T> instances;
	
	[SerializeField]
	private bool awoken;
	
	/*[SerializeField]*/
	protected delegate bool UniqueItemDuplicated(T other);
	
	protected bool ThisHasBeenDuplicated(UniqueItemDuplicated check)
	{
		if (staticInit == false || instances == null) FindAllInstances();
		
		// If it hasn't been awoken then it must be unique
		if (awoken == false)
		{
			awoken = true;
			
			return false;
		}
		// It may be a clone
		else
		{
			foreach (var instance in instances)
			{
				if (instance != null && instance != this)
				{
					if (check(instance) == true)
					{
						instances.Add((T)this);
						
						return true;
					}
				}
			}
		}
		
		instances.Add((T)this);
		
		return false;
	}
	
	protected bool ThisHasBeenDuplicated(params string[] objectNames)
	{
		return FindAwakeState(objectNames) == AwakeState.AwakeDuplicate;
	}
	
	protected AwakeState FindAwakeState(params string[] objectNames)
	{
		if (staticInit == false)
		{
			staticInit = true;
			
			FindAllInstances();
		}
		
		// If it hasn't been awoken then it must be unique
		if (awoken == false)
		{
			awoken = true;
			
			return AwakeState.AwakeOriginal;
		}
		// It may be a clone
		else
		{
			foreach (var objectName in objectNames)
			{
				var field = typeof(T).GetField(objectName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
				
				if (field == null) Debug.LogError("Failed to find a field with the name: " + objectName);
				
				var cur = field.GetValue(this);
				
				if (cur != null)
				{
					foreach (var instance in instances)
					{
						if (instance != null && instance != this)
						{
							var tgt = field.GetValue(instance);
							
							if (tgt != null && Equality(cur, tgt) == true)
							{
								instances.Add((T)this);
								
								return AwakeState.AwakeDuplicate;
							}
						}
					}
				}
			}
		}
		
		instances.Add((T)this);
		
		return AwakeState.AwakeAgain;
	}
	
	public bool Equality(object a, object b)
	{
		if (a.GetType().IsArray == true && b.GetType().IsArray == true)
		{
			var aArray = (System.Array)a;
			var bArray = (System.Array)b;
			
			if (aArray.Length == bArray.Length)
			{
				for (var i = 0; i < aArray.Length; i++)
				{
					var c = aArray.GetValue(i);
					var t = bArray.GetValue(i);
					
					if (c != null && t != null)
					{
						return Equality(c, t);
					}
				}
			}
		}
		else
		{
			if (a is Object && b is Object)
			{
				var a2 = (Object)a;
				var b2 = (Object)b;
				
				if (a2 != null && a2 == b2)
				{
					return true;
				}
			}
			else
			{
				if (a == b)
				{
					return true;
				}
			}
		}
		
		return false;
	}
	
	public void Start()
	{
		if (instances == null)
		{
			FindAllInstances();
		}
	}
	
	public void OnDestroy()
	{
		if (staticInit == true)
		{
			instances.Remove((T)this);
		}
	}
	
	private void FindAllInstances()
	{
		var currentInstances = SGT_Helper.FindAll<T>();
		
		staticInit = true;
		instances  = new System.Collections.Generic.HashSet<T>(currentInstances);
	}
}
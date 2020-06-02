using System.Collections.Generic;
using UnityEngine;

public static class SGT_ArrayHelper
{
	public static bool Remove<T>(ref T[] array, int index)
	{
		if (index >= 0 && array != null && index < array.Length)
		{
			var newArray = new T[array.Length - 1];
			
			System.Array.Copy(array, 0, newArray, 0, index);
			System.Array.Copy(array, index + 1, newArray, index, array.Length - index - 1);
			
			array = newArray;
			
			return true;
		}
		
		return false;
	}
	
	public static bool Remove<T>(List<T> array, int index)
	{
		if (index >= 0 && array != null && index < array.Count)
		{
			array.RemoveAt(index);
			
			return true;
		}
		
		return false;
	}
	
	public static bool Remove<T>(List<T> array, T element)
	{
		if (array != null)
		{
			return array.Remove(element);
		}
		
		return false;
	}
	
	public delegate bool RemoveIf<T>(T item);
	
	public static bool Remove<T>(List<T> array, RemoveIf<T> removeIf)
	{
		if (array != null && removeIf != null)
		{
			for (var i = 0; i < array.Count; i++)
			{
				if (removeIf(array[i]) == true)
				{
					array.RemoveAt(i);
					
					return true;
				}
			}
		}
		
		return false;
	}
	
	public static void Set<T>(List<T> array, T element, int index)
	{
		if (index >= 0 && array != null && index < array.Count)
		{
			array[index] = element;
		}
	}
	
	public static void Set<T>(T[] array, T element, int index)
	{
		if (index >= 0 && array != null && index < array.Length)
		{
			array[index] = element;
		}
	}
	
	public static bool CheckSet<T>(T[] array, T element, int index)
	{
		if (index >= 0 && array != null && index < array.Length)
		{
			var currentElement = array[index];
			
			if (EqualityComparer<T>.Default.Equals(element, currentElement) == false)
			{
				array[index] = element;
				
				return true;
			}
		}
		
		return false;
	}
	
	public static T Index<T>(T[] array, int index)
	{
		return index >= 0 && array != null && index < array.Length ? array[index] : default(T);
	}
	
	public static T Index<T>(List<T> array, int index)
	{
		return index >= 0 && array != null && index < array.Count ? array[index] : default(T);
	}
	
	public static T Index<T>(ref List<T> array, int index, bool createArrayIfNull)
	{
		Min(ref array, index + 1, createArrayIfNull);
		
		return array != null && index < array.Count ? array[index] : default(T);
	}
	
	public static T Index<T>(ref List<T> array, int index, bool createArrayIfNull, bool createElementIfNull)
		where T : class, new()
	{
		Min(ref array, index + 1, createArrayIfNull);
		
		if (array != null)
		{
			var element = array[index];
			
			if (element == null && createElementIfNull == true)
			{
				element = new T();
				
				array[index] = element;
			}
			
			return element;
		}
		
		return null;
	}
	
	public static void Min<T>(ref List<T> array, int size, bool createArrayIfNull)
	{
		if (size < 0) throw new System.IndexOutOfRangeException();
		
		if (array == null)
		{
			if (createArrayIfNull == true) array = new List<T>(size);
			
			return;
		}
		
		var arrayCount = array.Count;
		
		if (size > arrayCount)
		{
			array.Capacity = size;
			
			for (var i = arrayCount; i < size; i++)
			{
				array.Add(default(T));
			}
		}
	}
	
	public static void Min<T>(ref T[] array, int size, bool createArrayIfNull)
	{
		if (size < 0) throw new System.IndexOutOfRangeException();
		
		if (array == null)
		{
			if (createArrayIfNull == true) array = new T[size];
			
			return;
		}
		else if (array.Length < size)
		{
			System.Array.Resize(ref array, size);
		}
	}
	
	public static void Resize<T>(ref T[] array, int size, bool createArrayIfNull)
	{
		if (size < 0) throw new System.IndexOutOfRangeException();
		
		if (array == null)
		{
			if (createArrayIfNull == true) array = new T[size];
			
			return;
		}
		
		System.Array.Resize(ref array, size);
	}
	
	public static void Resize<T>(ref List<T> array, int size, bool createArrayIfNull)
	{
		if (size < 0) throw new System.IndexOutOfRangeException();
		
		if (array == null)
		{
			if (createArrayIfNull == true) array = new List<T>(size);
			
			return;
		}
		
		var arrayCount = array.Count;
		
		if (arrayCount != size)
		{
			// Add elements?
			if (size > arrayCount)
			{
				array.Capacity = size;
				
				for (var i = arrayCount; i < size; i++)
				{
					array.Add(default(T));
				}
			}
			// Remove elements?
			else if (arrayCount > size)
			{
				array.RemoveRange(size, arrayCount - size);
			}
		}
	}
	
	public static void Fill<T>(List<T> array)
		where T : class, new()
	{
		if (array != null)
		{
			for (var i = 0; i < array.Count; i++)
			{
				if (array[i] == null)
				{
					array[i] = new T();
				}
			}
		}
	}
	
	public static bool ContainsSomething<T>(List<T> array)
		where T : Object // UnityEngine.Object's != comparer differs from the default
	{
		if (array == null)
		{
			return false;
		}
		
		foreach (var element in array)
		{
			if (element != null)
			{
				return true;
			}
		}
		
		return false;
	}
	
	public static bool ContainsSomething<T>(T[] array)
		where T : Object // UnityEngine.Object's != comparer differs from the default
	{
		if (array == null)
		{
			return false;
		}
		
		foreach (var element in array)
		{
			if (element != null)
			{
				return true;
			}
		}
		
		return false;
	}
	
	public static bool Filled<T>(T[] os, int minSize = 1)
		where T : Object // UnityEngine.Object's != comparer differs from the default
	{
		if (os != null && os.Length >= minSize)
		{
			foreach (var o in os)
			{
				if (o == null)
				{
					return false;
				}
			}
			
			return true;
		}
		
		return false;
	}
}
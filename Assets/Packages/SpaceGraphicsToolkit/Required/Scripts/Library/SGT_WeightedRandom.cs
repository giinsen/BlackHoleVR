using IntList = System.Collections.Generic.List<int>;

using UnityEngine;
using System.Linq;

[System.Serializable]
public class SGT_WeightedRandom
{
	[SerializeField]
	private IntList ints;
	
	[SerializeField]
	private int resolution = 100;
	
	public int Count
	{
		get
		{
			return ints != null ? ints.Count : 0;
		}
	}
	
	public int Resolution
	{
		set
		{
			resolution = value;
		}
		
		get
		{
			return resolution;
		}
	}
	
	public int RandomIndex
	{
		get
		{
			if (ints != null && ints.Count > 0)
			{
				return ints[Random.Range(0, ints.Count)];
			}
			
			return -1;
		}
	}
	
	public SGT_WeightedRandom(int newResolution)
	{
		resolution = newResolution;
	}
	
	public void Add(int index, float weight)
	{
		if (ints == null) ints = new IntList();
		
		var c = Mathf.FloorToInt(weight * (float)resolution);
		
		for (var i = 0; i < c; i++)
		{
			ints.Add(index);
		}
	}
	
	
	public void Remove(int index)
	{
		if (ints != null)
		{
			ints = ints.Where(i => i != index).ToList();
		}
	}
}
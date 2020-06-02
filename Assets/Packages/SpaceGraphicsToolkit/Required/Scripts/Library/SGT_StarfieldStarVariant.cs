using UnityEngine;

[System.Serializable]
public class SGT_StarfieldStarVariant
{
	[SerializeField]
	private bool modified;
	
	[SerializeField]
	private float spawnProbability = 1.0f;
	
	[SerializeField]
	private bool custom;
	
	[SerializeField]
	private float customRadiusMin = 1.0f;
	
	[SerializeField]
	private float customRadiusMax = 3.0f;
	
	[SerializeField]
	private float customPulseRadiusMax = 1.0f;
	
	[SerializeField]
	private float customPulseRateMax = 1.0f;
	
	public bool Modified
	{
		set
		{
			modified = value;
		}
		
		get
		{
			return modified;
		}
	}
	
	public float SpawnProbability
	{
		set
		{
			value = Mathf.Clamp01(value);
			
			if (value != spawnProbability)
			{
				spawnProbability = value;
				modified         = true;
			}
		}
		
		get
		{
			return spawnProbability;
		}
	}
	
	public bool Custom
	{
		set
		{
			if (value != custom)
			{
				custom   = value;
				modified = true;
			}
		}
		
		get
		{
			return custom;
		}
	}
	
	public float CustomRadiusMin
	{
		set
		{
			if (value != customRadiusMin)
			{
				customRadiusMin = value;
				modified        = true;
			}
		}
		
		get
		{
			return customRadiusMin;
		}
	}
	
	public float CustomRadiusMax
	{
		set
		{
			if (value != customRadiusMax)
			{
				customRadiusMax = value;
				modified        = true;
			}
		}
		
		get
		{
			return customRadiusMax;
		}
	}
	
	public float CustomPulseRadiusMax
	{
		set
		{
			if (value != customPulseRadiusMax)
			{
				customPulseRadiusMax = value;
				modified             = true;
			}
		}
		
		get
		{
			return customPulseRadiusMax;
		}
	}
	
	public float CustomPulseRateMax
	{
		set
		{
			if (value != customPulseRateMax)
			{
				customPulseRateMax = value;
				modified           = true;
			}
		}
		
		get
		{
			return customPulseRateMax;
		}
	}
}
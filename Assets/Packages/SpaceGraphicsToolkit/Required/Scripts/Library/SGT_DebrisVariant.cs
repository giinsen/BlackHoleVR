using UnityEngine;

[System.Serializable]
public class SGT_DebrisVariant
{
	[SerializeField]
	private GameObject gameObject;
	
	[SerializeField]
	private float spawnProbability = 1.0f;
	
	public GameObject GameObject
	{
		set
		{
			gameObject = value;
		}
		
		get
		{
			return gameObject;
		}
	}
	
	public float SpawnProbability
	{
		set
		{
			spawnProbability = value;
		}
		
		get
		{
			return spawnProbability;
		}
	}
}
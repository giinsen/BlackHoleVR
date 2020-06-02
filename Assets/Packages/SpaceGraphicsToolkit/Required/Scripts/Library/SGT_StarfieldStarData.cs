using UnityEngine;

public class SGT_StarfieldStarData
{
	public Transform Transform;
	public Vector3   Position;
	public float     Angle;
	public float     RadiusMin;
	public float     RadiusMax;
	public float     RadiusPulseRate;
	public float     RadiusPulseOffset;
	public int       TextureIndex;
	
	public Vector3 WorldPosition
	{
		get
		{
			return Transform != null ? Transform.TransformPoint(Position) : Vector3.zero;
		}
	}
}
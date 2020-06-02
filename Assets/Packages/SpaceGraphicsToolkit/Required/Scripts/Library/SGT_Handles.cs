#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public static class SGT_Handles
{
	public static Color Colour
	{
		set
		{
			Handles.color = value;
		}
	}
	
	public static void DrawDisc(Vector3 position, Quaternion rotation, float radius)
	{
		DrawDisc(position, rotation, new Vector2(radius, radius), new Vector2(radius, radius));
	}
	
	public static void DrawDisc(Vector3 position, Quaternion rotation, float radius1, float radius2)
	{
		DrawDisc(position, rotation, new Vector2(radius1, radius1), new Vector2(radius2, radius2));
	}
	
	public static void DrawDisc(Vector3 position, Quaternion rotation, Vector2 radius)
	{
		DrawDisc(position, rotation, radius, radius);
	}
	
	public static void DrawDisc(Vector3 position, Quaternion rotation, Vector2 radius1, Vector2 radius2)
	{
		var positions = new Vector3[65];
		var step      = (Mathf.PI * 2.0f) / 64.0f;
		
		for (var i = 0; i < 65; i++)
		{
			var radius = i % 2 == 0 ? radius1 : radius2;
			var angle  = i * step; positions[i] = position + rotation * new Vector3(Mathf.Sin(angle) * radius.x, 0.0f, Mathf.Cos(angle) * radius.y);
		}
		
		Handles.DrawPolyLine(positions);
	}
	
	public static void DrawSphere(Vector3 position, float radius)
	{
		DrawDisc(position, Quaternion.FromToRotation(Vector3.up, Vector3.right  ), radius);
		DrawDisc(position, Quaternion.FromToRotation(Vector3.up, Vector3.up     ), radius);
		DrawDisc(position, Quaternion.FromToRotation(Vector3.up, Vector3.forward), radius);
	}
	
	public static void DrawSphere(Vector3 position, float radius1, float radius2)
	{
		DrawDisc(position, Quaternion.FromToRotation(Vector3.up, Vector3.right  ), radius1, radius2);
		DrawDisc(position, Quaternion.FromToRotation(Vector3.up, Vector3.up     ), radius1, radius2);
		DrawDisc(position, Quaternion.FromToRotation(Vector3.up, Vector3.forward), radius1, radius2);
	}
	
	public static void DrawSphere(Vector3 position, Quaternion rotation, Vector3 radius)
	{
		DrawDisc(position, rotation * Quaternion.FromToRotation(Vector3.up, Vector3.right  ), new Vector2(radius.y, radius.z));
		DrawDisc(position, rotation * Quaternion.FromToRotation(Vector3.up, Vector3.up     ), new Vector2(radius.x, radius.z));
		DrawDisc(position, rotation * Quaternion.FromToRotation(Vector3.up, Vector3.forward), new Vector2(radius.x, radius.y));
	}
	
	public static void DrawSphere(Vector3 position, Quaternion rotation, Vector3 radius1, Vector3 radius2)
	{
		DrawDisc(position, rotation * Quaternion.FromToRotation(Vector3.up, Vector3.right  ), new Vector2(radius1.y, radius1.z), new Vector2(radius2.y, radius2.z));
		DrawDisc(position, rotation * Quaternion.FromToRotation(Vector3.up, Vector3.up     ), new Vector2(radius1.x, radius1.z), new Vector2(radius2.x, radius2.z));
		DrawDisc(position, rotation * Quaternion.FromToRotation(Vector3.up, Vector3.forward), new Vector2(radius1.x, radius1.y), new Vector2(radius2.x, radius2.y));
	}
	
	public static void DrawLine(Vector3 position, Vector3 direction)
	{
		direction = direction.normalized * 5000.0f;
		
		Handles.DrawLine(position - direction, position + direction);
	}
	
	public static void DrawRay(Vector3 position, Vector3 direction)
	{
		direction = direction.normalized * 5000.0f;
		
		Handles.DrawLine(position, position + direction);
	}
	
	public static void DrawLineSegment(Vector3 position1, Vector3 position2)
	{
		Handles.DrawLine(position1, position2);
	}
}
#endif
using UnityEngine;

public static class SGT_MatrixHelper
{
	public static Matrix4x4 Rotation(Quaternion q)
	{
		var matrix = Matrix4x4.TRS(Vector3.zero, q, Vector3.one);
		
		return matrix;
	}
	
	public static Matrix4x4 Translation(Vector3 xyz)
	{
		var matrix = Matrix4x4.identity;
		
		matrix.m03 = xyz.x;
		matrix.m13 = xyz.y;
		matrix.m23 = xyz.z;
		
		return matrix;
	}
	
	public static Matrix4x4 Scaling(float xyz)
	{
		var matrix = Matrix4x4.identity;
		
		matrix.m00 = xyz;
		matrix.m11 = xyz;
		matrix.m22 = xyz;
		
		return matrix;
	}
	
	public static Matrix4x4 Scaling(Vector3 xyz)
	{
		var matrix = Matrix4x4.identity;
		
		matrix.m00 = xyz.x;
		matrix.m11 = xyz.y;
		matrix.m22 = xyz.z;
		
		return matrix;
	}
	
	public static Matrix4x4 ShearingZ(float x, float y)
	{
		var matrix = Matrix4x4.identity;
		
		matrix.m20 = x;
		matrix.m21 = y;
		
		return matrix;
	}
}
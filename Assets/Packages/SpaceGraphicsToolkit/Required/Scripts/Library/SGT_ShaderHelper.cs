using UnityEngine;

public static class SGT_ShaderHelper
{
	public static void SetColor(Material[] ma, string name, Color v)
	{
		foreach (var m in ma)
		{
			if (m != null)
			{
				m.SetColor(name, v);
			}
		}
	}
	
	public static void SetFloat(Material[] ma, string name, float v)
	{
		foreach (var m in ma)
		{
			if (m != null)
			{
				m.SetFloat(name, v);
			}
		}
	}
	
	public static void SetMatrix(Material[] ma, string name, Matrix4x4 v)
	{
		foreach (var m in ma)
		{
			if (m != null)
			{
				m.SetMatrix(name, v);
			}
		}
	}
	
	public static void SetTexture(Material[] ma, string name, Texture v)
	{
		foreach (var m in ma)
		{
			if (m != null)
			{
				m.SetTexture(name, v);
			}
		}
	}
	
	public static void SetVector(Material[] ma, string name, Vector4 v)
	{
		foreach (var m in ma)
		{
			if (m != null)
			{
				m.SetVector(name, v);
			}
		}
	}
}
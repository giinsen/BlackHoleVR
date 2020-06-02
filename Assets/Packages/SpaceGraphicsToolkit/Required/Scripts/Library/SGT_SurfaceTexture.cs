using UnityEngine;

[System.Serializable]
public class SGT_SurfaceTexture
{
	[SerializeField]
	private Texture[] textures;
	
	[SerializeField]
	private SGT_SurfaceConfiguration configuration = SGT_SurfaceConfiguration.Sphere;
	
	[SerializeField]
	private bool modified = true;
	
	public int Count
	{
		get
		{
			return SGT_SurfaceConfiguration_.SurfaceCount(configuration);
		}
	}
	
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
	
	public SGT_SurfaceConfiguration Configuration
	{
		set
		{
			if (value != configuration)
			{
				configuration = value;
				textures      = null;
				modified      = true;
			}
		}
		
		get
		{
			return configuration;
		}
	}
	
	public bool ContainsSomething
	{
		get
		{
			return SGT_ArrayHelper.ContainsSomething(textures);
		}
	}
	
	public Texture GetTexture(Vector3 direction)
	{
		return GetTexture(SGT_Helper.CubeFace(direction));
	}
	
	public Texture GetTexture(CubemapFace face)
	{
		return GetTexture((int)face);
	}
	
	public Texture GetTexture(int faceIndex)
	{
		return SGT_ArrayHelper.Index(textures, faceIndex);
	}
	
	public Texture2D GetTexture2D(Vector3 direction)
	{
		return GetTexture2D(SGT_Helper.CubeFace(direction));
	}
	
	public Texture2D GetTexture2D(CubemapFace face)
	{
		return GetTexture(face) as Texture2D;
	}
	
	public Texture2D GetTexture2D(int faceIndex)
	{
		return GetTexture(faceIndex) as Texture2D;
	}
	
	public void SetTexture(Texture newTexture, CubemapFace face)
	{
		SetTexture(newTexture, (int)face);
	}
	
	public void SetTexture(Texture newTexture, int faceIndex)
	{
		var surfaceCount = Count;
		
		if (textures == null || textures.Length != surfaceCount) textures = new Texture[surfaceCount];
		
		modified |= SGT_ArrayHelper.CheckSet(textures, newTexture, faceIndex);
	}
	
	
}
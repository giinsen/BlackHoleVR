using UnityEngine;

public enum SGT_SurfaceConfiguration
{
	Sphere,
	Cube
}

public static class SGT_SurfaceConfiguration_
{
	public static int SurfaceCount(SGT_SurfaceConfiguration surfaceConfiguration)
	{
		var surfaceCount = 0;
		
		switch (surfaceConfiguration)
		{
			case SGT_SurfaceConfiguration.Sphere: surfaceCount = 1; break;
			case SGT_SurfaceConfiguration.Cube:   surfaceCount = 6; break;
		}
		
		return surfaceCount;
	}
	
	public static Material[] CreateMaterials(SGT_SurfaceConfiguration surfaceConfiguration, string shaderName, int renderQueue)
	{
		var surfaceCount = SurfaceCount(surfaceConfiguration);
		var materials    = new Material[surfaceCount];
		
		materials[0] = SGT_Helper.CreateMaterial(shaderName, renderQueue);
		
		for (var i = 1; i < surfaceCount; i++)
		{
			materials[i] = SGT_Helper.CloneObject(materials[0]);
		}
		
		return materials;
	}
}
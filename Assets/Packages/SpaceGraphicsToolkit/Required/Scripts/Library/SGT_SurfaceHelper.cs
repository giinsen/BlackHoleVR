using UnityEngine;

public static class SGT_SurfaceHelper
{
	public static bool ContainsSurface(GameObject gameObject)
	{
		return gameObject != null && (gameObject.GetComponent<SGT_Planet>() != null || gameObject.GetComponent<SGT_Star>() != null);
	}
	
	public static float FindRadius(GameObject gameObject, Vector3 point)
	{
		if (gameObject != null)
		{
			var planet = gameObject.GetComponent<SGT_Planet>();
			
			if (planet != null)
			{
				return planet.SurfaceRadiusAtPoint(point) * FindDisplacement(gameObject, point);
			}
			else
			{
				var star = gameObject.GetComponent<SGT_Star>();
				
				if (star != null)
				{
					return star.SurfaceRadiusAtPoint(point) * FindDisplacement(gameObject, point);
				}
			}
		}
		
		return 0.0f;
	}
	
	public static float FindDisplacement(GameObject gameObject, Vector3 point)
	{
		if (gameObject != null)
		{
			var displacement = gameObject.GetComponent<SGT_SurfaceDisplacement>();
			
			if (displacement != null)
			{
				return displacement.RadiusScaleAtPoint(point);
			}
			else
			{
				var tessellator = gameObject.GetComponent<SGT_SurfaceTessellator>();
				
				if (tessellator != null)
				{
					return tessellator.RadiusScaleAtPoint(point);
				}
			}
		}
		
		return 1.0f;
	}
}
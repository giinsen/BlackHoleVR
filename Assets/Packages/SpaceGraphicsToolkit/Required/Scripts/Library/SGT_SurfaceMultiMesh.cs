using ObjectList = System.Collections.Generic.List<UnityEngine.Object>;

using UnityEngine;

[System.Serializable]
public class SGT_SurfaceMultiMesh
{
	[SerializeField]
	private GameObject gameObject;
	
	[SerializeField]
	private bool hasMeshColliders;
	
	[SerializeField]
	private bool hasMeshRenderers;
	
	[SerializeField]
	private bool meshRenderersEnabled;
	
	[SerializeField]
	private bool meshCollidersEnabled;
	
	[SerializeField]
	private PhysicMaterial sharedPhysicsMaterial;
	
	[SerializeField]
	private SGT_SurfaceConfiguration configuration = SGT_SurfaceConfiguration.Sphere;
	
	[SerializeField]
	private SGT_MultiMesh[] multiMeshes;
	
	[SerializeField]
	private GameObject[] multiMeshGameObjects;
	
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
	
	public SGT_SurfaceConfiguration Configuration
	{
		set
		{
			if (value != configuration)
			{
				configuration        = value;
				multiMeshes          = null;
				multiMeshGameObjects = SGT_Helper.DestroyGameObjects(multiMeshGameObjects);
				
				SGT_Helper.DestroyChildren(gameObject);
			}
		}
		
		get
		{
			return configuration;
		}
	}
	
	public int Count
	{
		get
		{
			return SGT_SurfaceConfiguration_.SurfaceCount(configuration);
		}
	}
	
	public bool ContainsSomething
	{
		get
		{
			if (multiMeshes != null)
			{
				foreach (var multiMesh in multiMeshes)
				{
					if (multiMesh != null)
					{
						if (multiMesh.ContainsSomething == true)
						{
							return true;
						}
					}
				}
			}
			
			return false;
		}
	}
	
	public bool HasMeshRenderers
	{
		set
		{
			hasMeshRenderers = value;
		}
		
		get
		{
			return hasMeshRenderers;
		}
	}
	
	public bool HasMeshColliders
	{
		set
		{
			hasMeshColliders = value;
		}
		
		get
		{
			return hasMeshColliders;
		}
	}
	
	public PhysicMaterial SharedPhysicsMaterial
	{
		set
		{
			sharedPhysicsMaterial = value;
		}
		
		get
		{
			return sharedPhysicsMaterial;
		}
	}
	
	public bool MeshRenderersEnabled
	{
		set
		{
			meshRenderersEnabled = value;
		}
		
		get
		{
			return meshRenderersEnabled;
		}
	}
	
	public bool MeshCollidersEnabled
	{
		set
		{
			meshCollidersEnabled = value;
		}
		
		get
		{
			return meshCollidersEnabled;
		}
	}
	
	public bool MeshesModified
	{
		set
		{
			if (multiMeshes != null)
			{
				foreach (var multiMesh in multiMeshes)
				{
					if (multiMesh != null)
					{
						multiMesh.MeshesModified = value;
					}
				}
			}
		}
		
		get
		{
			if (multiMeshes != null)
			{
				foreach (var multiMesh in multiMeshes)
				{
					if (multiMesh != null)
					{
						if (multiMesh.MeshesModified == true)
						{
							return true;
						}
					}
				}
			}
			
			return false;
		}
	}
	
	public void BuildUndoTargets(ObjectList list)
	{
		if (gameObject != null) list.Add(gameObject);
		
		if (multiMeshes != null)
		{
			foreach (var multiMesh in multiMeshes)
			{
				if (multiMesh != null)
				{
					multiMesh.BuildUndoTargets(list);
				}
			}
		}
	}
	
	public void OnEnable()
	{
		if (multiMeshes != null)
		{
			foreach (var multiMesh in multiMeshes)
			{
				if (multiMesh != null)
				{
					multiMesh.OnEnable();
				}
			}
		}
	}
	
	public void OnDisable()
	{
		if (multiMeshes != null)
		{
			foreach (var multiMesh in multiMeshes)
			{
				if (multiMesh != null)
				{
					multiMesh.OnDisable();
				}
			}
		}
	}
	
	public void Update(int layer, string tag)
	{
		var surfaceCount = Count;
		
		if (multiMeshes          == null || multiMeshes.Length          != surfaceCount) multiMeshes          = new SGT_MultiMesh[surfaceCount];
		if (multiMeshGameObjects == null || multiMeshGameObjects.Length != surfaceCount) multiMeshGameObjects = new GameObject[surfaceCount];
		
		// Update multi meshes
		for (var i = 0; i < multiMeshes.Length; i++)
		{
			var multiMesh           = multiMeshes[i];
			var multiMeshGameObject = multiMeshGameObjects[i];
			
			if (multiMesh == null)
			{
				multiMesh      = new SGT_MultiMesh();
				multiMeshes[i] = multiMesh;
			}
			
			if (multiMeshGameObject == null)
			{
				multiMeshGameObject     = SGT_Helper.CreateGameObject("Multi Mesh " + (i + 1), gameObject);
				multiMeshGameObjects[i] = multiMeshGameObject;
			}
			
			if (gameObject != null)
			{
				SGT_Helper.SetParent(multiMeshGameObject, gameObject);
				SGT_Helper.SetLayer(multiMeshGameObject, gameObject.layer);
				SGT_Helper.SetTag(multiMeshGameObject, gameObject.tag);
			}
			
			multiMesh.GameObject            = multiMeshGameObject;
			multiMesh.HasMeshRenderers      = hasMeshRenderers;
			multiMesh.MeshRenderersEnabled  = meshRenderersEnabled;
			multiMesh.HasMeshColliders      = hasMeshColliders;
			multiMesh.MeshCollidersEnabled  = meshCollidersEnabled;
			multiMesh.SharedPhysicsMaterial = sharedPhysicsMaterial;
			multiMesh.Update();
		}
		
		// Check integrity
		if (gameObject != null && gameObject.transform.childCount != surfaceCount)
		{
			SGT_Helper.DestroyGameObjects(multiMeshGameObjects);
			SGT_Helper.DestroyChildren(gameObject);
		}
	}
	
	public SGT_MultiMesh GetMultiMesh(CubemapFace face)
	{
		return GetMultiMesh((int)face);
	}
	
	public SGT_MultiMesh GetMultiMesh(int faceIndex)
	{
		var surfaceCount = Count;
		
		if (multiMeshes == null || multiMeshes.Length != surfaceCount) multiMeshes = new SGT_MultiMesh[surfaceCount];
		
		if (faceIndex >= 0 && faceIndex < surfaceCount)
		{
			var multiMesh = multiMeshes[faceIndex];
			
			if (multiMesh == null)
			{
				multiMesh              = new SGT_MultiMesh();
				multiMeshes[faceIndex] = multiMesh;
			}
			
			return multiMesh;
		}
		
		return null;
	}
	
	public void RemoveAll()
	{
		if (multiMeshes != null)
		{
			foreach (var multiMesh in multiMeshes)
			{
				if (multiMesh == null)
				{
					multiMesh.RemoveAll();
				}
			}
		}
	}
	
	public Mesh GetSharedMesh(CubemapFace face, int meshIndex)
	{
		return GetSharedMesh((int)face, meshIndex);
	}
	
	public Mesh GetSharedMesh(int faceIndex, int meshIndex)
	{
		var multiMesh = GetMultiMesh(faceIndex);
		
		return multiMesh != null ? multiMesh.GetSharedMesh(meshIndex) : null;
	}
	
	public void ReplaceAll(Mesh newSharedMesh, int faceIndex)
	{
		ReplaceAll(new Mesh[] { newSharedMesh }, faceIndex);
	}
	
	public void ReplaceAll(Mesh[] newSharedMeshes, int faceIndex)
	{
		var multiMesh = GetMultiMesh(faceIndex);
		
		if (multiMesh != null) multiMesh.ReplaceAll(newSharedMeshes);
	}
	
	public void SetSharedMaterials(Material newSharedMaterial)
	{
		var surfaceCount = SGT_SurfaceConfiguration_.SurfaceCount(configuration);
		
		for (var i = 0; i < surfaceCount; i++)
		{
			SetSharedMaterial(newSharedMaterial, i);
		}
	}
	
	public void SetSharedMaterials(Material[] newSharedMaterials)
	{
		if (newSharedMaterials != null)
		{
			for (var i = 0; i < newSharedMaterials.Length; i++)
			{
				SetSharedMaterial(newSharedMaterials[i], i);
			}
		}
	}
	
	public void SetSharedMaterial(Material newSharedMaterial, CubemapFace face)
	{
		SetSharedMaterial(newSharedMaterial, (int)face);
	}
	
	public void SetSharedMaterial(Material newSharedMaterial, int faceIndex)
	{
		var multiMesh = GetMultiMesh(faceIndex);
		
		if (multiMesh != null) multiMesh.SharedMaterial = newSharedMaterial;
	}
	
	public Material GetSharedMaterial(CubemapFace face)
	{
		return GetSharedMaterial((int)face);
	}
	
	public Material GetSharedMaterial(int faceIndex)
	{
		var multiMesh = GetMultiMesh(faceIndex);
		
		return multiMesh != null ? multiMesh.SharedMaterial : null;
	}
	
	public SGT_SurfaceMultiMesh Clear()
	{
		if (multiMeshes          != null) multiMeshes          = null;
		if (multiMeshGameObjects != null) multiMeshGameObjects = SGT_Helper.DestroyGameObjects(multiMeshGameObjects);
		
		return null;
	}
	
	public void DestroyAllMeshes()
	{
		if (multiMeshes != null)
		{
			foreach (var multiMesh in multiMeshes)
			{
				if (multiMesh != null)
				{
					multiMesh.DestroyAllMeshes();
				}
			}
			
			Clear();
		}
	}
	
	public void CopyMeshesInto(SGT_SurfaceMultiMesh target)
	{
		if (target != null && multiMeshes != null)
		{
			var surfaceCount = Count;
			
			target.Configuration = configuration;
			
			for (var i = 0; i < surfaceCount; i++)
			{
				var s = GetMultiMesh(i);
				var t = target.GetMultiMesh(i);
				
				if (s != null && t != null)
				{
					s.CopyMeshesInto(t);
				}
			}
		}
	}
	
	public static bool MeshesIdentical(SGT_SurfaceMultiMesh a, SGT_SurfaceMultiMesh b)
	{
		if (a == null && b != null) return false;
		if (a != null && b == null) return false;
		if (a != null && b != null)
		{
			if (a.multiMeshes == null && b.multiMeshes != null) return false;
			if (a.multiMeshes != null && b.multiMeshes == null) return false;
			if (a.multiMeshes != null && b.multiMeshes != null)
			{
				if (a.multiMeshes.Length != b.multiMeshes.Length) return false;
				
				for (var i = 0; i < a.multiMeshes.Length; i++)
				{
					if (SGT_MultiMesh.MeshesIdentical(a.multiMeshes[i], b.multiMeshes[i]) == false)
					{
						return false;
					}
				}
			}
		}
		
		return true;
	}
	
#if UNITY_EDITOR == true
	public void HideInEditor()
	{
		SGT_Helper.HideGameObject(gameObject);
		
		if (multiMeshes != null)
		{
			foreach (var multiMesh in multiMeshes)
			{
				if (multiMesh != null)
				{
					multiMesh.HideInEditor();
				}
			}
		}
	}
#endif
}
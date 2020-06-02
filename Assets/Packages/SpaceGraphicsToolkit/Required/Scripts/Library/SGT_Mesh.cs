using ObjectList = System.Collections.Generic.List<UnityEngine.Object>;

using UnityEngine;

[System.Serializable]
public class SGT_Mesh
{
	[SerializeField]
	private bool modified;
	
	[SerializeField]
	private bool meshModified;
	
	[SerializeField]
	private GameObject gameObject;
	
	[SerializeField]
	private Mesh sharedMesh;
	
	[SerializeField]
	private Material sharedMaterial;
	
	[SerializeField]
	private PhysicMaterial sharedPhysicsMaterial;
	
	[SerializeField]
	private MeshFilter meshFilter;
	
	[SerializeField]
	private MeshRenderer meshRenderer;
	
	[SerializeField]
	private MeshCollider meshCollider;
	
	[SerializeField]
	private bool hasMeshCollider;
	
	[SerializeField]
	private bool hasMeshRenderer;
	
	[SerializeField]
	private bool meshRendererEnabled;
	
	[SerializeField]
	private bool meshColliderEnabled;
	
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
	
	public Mesh SharedMesh
	{
		set
		{
			if (value != sharedMesh)
			{
				sharedMesh   = value;
				meshModified = true;
			}
		}
		
		get
		{
			return sharedMesh;
		}
	}
	
	public Material SharedMaterial
	{
		set
		{
			sharedMaterial = value;
		}
		
		get
		{
			return sharedMaterial;
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
	
	public bool HasMeshRenderer
	{
		set
		{
			hasMeshRenderer = value;
		}
		
		get
		{
			return hasMeshRenderer;
		}
	}
	
	public bool HasMeshCollider
	{
		set
		{
			hasMeshCollider = value;
		}
		
		get
		{
			return hasMeshCollider;
		}
	}
	
	public bool MeshRendererEnabled
	{
		set
		{
			meshRendererEnabled = value;
		}
		
		get
		{
			return meshRendererEnabled;
		}
	}
	
	public bool MeshColliderEnabled
	{
		set
		{
			meshColliderEnabled = value;
		}
		
		get
		{
			return meshColliderEnabled;
		}
	}
	
	public bool MeshModified
	{
		set
		{
			meshModified = value;
		}
		
		get
		{
			return meshModified;
		}
	}
	
	public void BuildUndoTargets(ObjectList list)
	{
		if (gameObject   != null) list.Add(gameObject  );
		if (meshFilter   != null) list.Add(meshFilter  );
		if (meshRenderer != null) list.Add(meshRenderer);
		if (meshCollider != null) list.Add(meshCollider);
	}
	
	public void OnEnable()
	{
		if (meshRenderer != null && meshRenderer.enabled != meshRendererEnabled) meshRenderer.enabled = meshRendererEnabled;
		if (meshCollider != null && meshCollider.enabled != meshColliderEnabled) meshCollider.enabled = meshColliderEnabled;
	}
	
	public void OnDisable()
	{
		if (meshRenderer != null && meshRenderer.enabled == true) meshRenderer.enabled = false;
		if (meshCollider != null && meshCollider.enabled == true) meshCollider.enabled = false;
	}
	
	public void Update()
	{
		if (gameObject != null)
		{
			if (hasMeshRenderer == true)
			{
				if (meshFilter   == null) meshFilter   = gameObject.AddComponent<MeshFilter>();
				if (meshRenderer == null) meshRenderer = gameObject.AddComponent<MeshRenderer>();
				
				if (meshFilter.sharedMesh       != sharedMesh         ) meshFilter.sharedMesh       = sharedMesh;
				if (meshRenderer.enabled        != meshRendererEnabled) meshRenderer.enabled        = meshRendererEnabled;
				if (meshRenderer.sharedMaterial != sharedMaterial     ) meshRenderer.sharedMaterial = sharedMaterial;
			}
			else
			{
				if (meshFilter   != null) meshFilter   = SGT_Helper.DestroyObject(meshFilter);
				if (meshRenderer != null) meshRenderer = SGT_Helper.DestroyObject(meshRenderer);
			}
			
			if (hasMeshCollider == true)
			{
				if (meshCollider == null) meshCollider = gameObject.AddComponent<MeshCollider>();
				
				if (meshCollider.enabled        != meshColliderEnabled  ) meshCollider.enabled        = meshColliderEnabled;
				if (meshCollider.sharedMesh     != sharedMesh           ) meshCollider.sharedMesh     = sharedMesh;
				if (meshCollider.sharedMaterial != sharedPhysicsMaterial) meshCollider.sharedMaterial = sharedPhysicsMaterial;
			}
			else
			{
				if (meshCollider != null) meshCollider = SGT_Helper.DestroyObject(meshCollider);
			}
		}
		else
		{
			if (meshFilter   != null) meshFilter   = SGT_Helper.DestroyObject(meshFilter  );
			if (meshRenderer != null) meshRenderer = SGT_Helper.DestroyObject(meshRenderer);
			if (meshCollider != null) meshCollider = SGT_Helper.DestroyObject(meshCollider);
		}
	}
	
	public SGT_Mesh Clear()
	{
		if (meshFilter   != null) meshFilter   = SGT_Helper.DestroyObject(meshFilter  );
		if (meshRenderer != null) meshRenderer = SGT_Helper.DestroyObject(meshRenderer);
		if (meshCollider != null) meshCollider = SGT_Helper.DestroyObject(meshCollider);
		
		return null;
	}
	
	public void DestroyMesh()
	{
		if (sharedMesh != null) sharedMesh = SGT_Helper.DestroyObject(sharedMesh);
	}
	
	public static bool MeshesIdentical(SGT_Mesh a, SGT_Mesh b)
	{
		if (a == null && b != null) return false;
		if (a != null && b == null) return false;
		if (a != null && b != null)
		{
			if (a.sharedMesh != b.sharedMesh) return false;
		}
		
		return true;
	}
	
#if UNITY_EDITOR == true
	public void HideInEditor()
	{
		SGT_Helper.HideGameObject(gameObject);
		SGT_Helper.HideWireframe(meshRenderer);
	}
#endif
}
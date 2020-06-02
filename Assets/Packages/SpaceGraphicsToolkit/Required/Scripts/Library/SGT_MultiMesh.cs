using ObjectList     = System.Collections.Generic.List<UnityEngine.Object>;
using MeshList       = System.Collections.Generic.List<SGT_Mesh>;
using GameObjectList = System.Collections.Generic.List<UnityEngine.GameObject>;

using UnityEngine;

[System.Serializable]
public class SGT_MultiMesh
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
	private Mesh sharedMesh;
	
	[SerializeField]
	private Material sharedMaterial;
	
	[SerializeField]
	private PhysicMaterial sharedPhysicsMaterial;
	
	[SerializeField]
	private GameObjectList meshGameObjects;
	
	[SerializeField]
	private MeshList meshes;
	
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
	
	public int Count
	{
		set
		{
			Resize(value);
		}
		
		get
		{
			return meshes != null ? meshes.Count : 0;
		}
	}
	
	public bool ContainsSomething
	{
		get
		{
			if (meshes != null)
			{
				foreach (var mesh in meshes)
				{
					if (mesh != null)
					{
						if (mesh.SharedMesh != null)
						{
							return true;
						}
					}
				}
			}
			
			return false;
		}
	}
	
	public Mesh SharedMesh
	{
		set
		{
			sharedMesh = value;
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
			if (meshes != null)
			{
				foreach (var mesh in meshes)
				{
					if (mesh != null)
					{
						mesh.MeshModified = value;
					}
				}
			}
		}
		
		get
		{
			if (meshes != null)
			{
				foreach (var mesh in meshes)
				{
					if (mesh != null)
					{
						if (mesh.MeshModified == true)
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
		
		if (meshes != null)
		{
			foreach (var mesh in meshes)
			{
				if (mesh != null)
				{
					mesh.BuildUndoTargets(list);
				}
			}
		}
	}
	
	public void OnEnable()
	{
		if (meshes != null)
		{
			foreach (var mesh in meshes)
			{
				if (mesh != null)
				{
					mesh.OnEnable();
				}
			}
		}
	}
	
	public void OnDisable()
	{
		if (meshes != null)
		{
			foreach (var mesh in meshes)
			{
				if (mesh != null)
				{
					mesh.OnDisable();
				}
			}
		}
	}
	
	public void Update()
	{
		if (meshes          == null) meshes          = new MeshList();
		if (meshGameObjects == null) meshGameObjects = new GameObjectList();
		
		if (meshGameObjects.Count != meshes.Count) SGT_ArrayHelper.Resize(ref meshGameObjects, meshes.Count, true);
		
		// Update meshes
		for (var i = meshes.Count - 1; i >= 0; i--)
		{
			var mesh           = meshes[i];
			var meshGameObject = meshGameObjects[i];
			
			if (mesh == null)
			{
				mesh      = new SGT_Mesh();
				meshes[i] = mesh;
			}
			
			if (meshGameObject == null)
			{
				meshGameObject     = SGT_Helper.CreateGameObject("Mesh " + (i + 1), gameObject);
				meshGameObjects[i] = meshGameObject;
			}
			
			if (gameObject != null)
			{
				SGT_Helper.SetParent(meshGameObject, gameObject);
				SGT_Helper.SetLayer(meshGameObject, gameObject.layer);
				SGT_Helper.SetTag(meshGameObject, gameObject.tag);
			}
			
			if (sharedMesh            != null) mesh.SharedMesh            = sharedMesh;
			if (sharedMaterial        != null) mesh.SharedMaterial        = sharedMaterial;
			if (sharedPhysicsMaterial != null) mesh.SharedPhysicsMaterial = sharedPhysicsMaterial;
			
			mesh.GameObject          = meshGameObject;
			mesh.HasMeshRenderer     = hasMeshRenderers;
			mesh.MeshRendererEnabled = meshRenderersEnabled;
			mesh.HasMeshCollider     = hasMeshColliders;
			mesh.MeshColliderEnabled = meshCollidersEnabled;
			mesh.Update();
		}
		
		// Check integrity
		if (gameObject != null && meshGameObjects.Count != gameObject.transform.childCount)
		{
			SGT_Helper.DestroyChildren(gameObject);
			SGT_Helper.DestroyGameObjects(meshGameObjects);
		}
	}
	
	public SGT_Mesh GetMesh(int index)
	{
		if (index >= 0 && meshes != null && index < meshes.Count)
		{
			var mesh = meshes[index];
			
			if (mesh == null)
			{
				mesh = new SGT_Mesh();
				meshes[index] = mesh;
			}
			
			return mesh;
		}
		
		return null;
	}
	
	public GameObject GetMeshGameObject(int index)
	{
		if (index >= 0 && meshGameObjects != null && index < meshGameObjects.Count)
		{
			var meshGameObject = meshGameObjects[index];
			
			if (meshGameObject == null)
			{
				meshGameObject         = SGT_Helper.CreateGameObject("Mesh " + (index + 1), gameObject);
				meshGameObjects[index] = meshGameObject;
			}
			
			return meshGameObject;
		}
		
		return null;
	}
	
	public void Add(Mesh newSharedMesh = null, Material newSharedMaterial = null, PhysicMaterial newSharedPhysicsMaterial = null)
	{
		if (meshes          == null) meshes          = new MeshList(1);
		if (meshGameObjects == null) meshGameObjects = new GameObjectList(1);
		
		var mesh = new SGT_Mesh();
		
		if (newSharedMesh            != null) { sharedMesh            = null; mesh.SharedMesh            = newSharedMesh;            }
		if (newSharedMaterial        != null) { sharedMaterial        = null; mesh.SharedMaterial        = newSharedMaterial;        }
		if (newSharedPhysicsMaterial != null) { sharedPhysicsMaterial = null; mesh.SharedPhysicsMaterial = newSharedPhysicsMaterial; }
		
		meshes.Add(mesh);
		meshGameObjects.Add(null);
	}
	
	public void Resize(int count)
	{
		count = count < 0 ? 0 : count;
		
		if (meshes == null) meshes = new MeshList(count);
		
		if (count != meshes.Count)
		{
			// Add meshes?
			if (count > meshes.Count)
			{
				meshes.Capacity = count;
				
				for (var i = meshes.Count; i < count; i++)
				{
					Add();
				}
			}
			// Remove meshes?
			else if (count < meshes.Count)
			{
				for (var i = meshes.Count - 1; i >= count; i--)
				{
					Remove(i);
				}
			}
		}
	}
	
	public void Remove(int index)
	{
		if (index >= 0)
		{
			if (meshes != null && index < meshes.Count)
			{
				meshes.RemoveAt(index);
			}
			
			if (meshGameObjects != null && index < meshGameObjects.Count)
			{
				SGT_Helper.DestroyGameObject(meshGameObjects[index]);
				
				meshGameObjects.RemoveAt(index);
			}
		}
	}
	
	public void RemoveAll()
	{
		if (meshes != null) meshes.Clear();
		
		if (meshGameObjects != null)
		{
			SGT_Helper.DestroyObjects(meshGameObjects);
			
			meshGameObjects.Clear();
		}
	}
	
	public void ReplaceAll(Mesh newSharedMesh, Material newSharedMaterial = null, PhysicMaterial newSharedPhysicsMaterial = null)
	{
		var newRendererMaterials = newSharedMaterial        != null ? new Material[]       { newSharedMaterial        } : null;
		var newColliderMaterials = newSharedPhysicsMaterial != null ? new PhysicMaterial[] { newSharedPhysicsMaterial } : null;
		
		ReplaceAll(new Mesh[] { newSharedMesh }, newRendererMaterials, newColliderMaterials);
	}
	
	public void ReplaceAll(Mesh[] newSharedMeshes, Material[] newSharedMaterials = null, PhysicMaterial[] newSharedPhysicsMaterials = null)
	{
		if (newSharedMeshes != null)
		{
			Resize(newSharedMeshes.Length);
			
			sharedMesh = null;
			if (newSharedMaterials        != null) sharedMaterial        = null;
			if (newSharedPhysicsMaterials != null) sharedPhysicsMaterial = null;
			
			// Set all meshes
			for (var i = 0; i < newSharedMeshes.Length; i++)
			{
				var mesh = meshes[i];
				
				if (mesh != null)
				{
					mesh.SharedMesh = newSharedMeshes[i];
					if (newSharedMaterials        != null) mesh.SharedMaterial        = SGT_ArrayHelper.Index(newSharedMaterials       , i);
					if (newSharedPhysicsMaterials != null) mesh.SharedPhysicsMaterial = SGT_ArrayHelper.Index(newSharedPhysicsMaterials, i);
				}
			}
		}
	}
	
	public void SetSharedMesh(Mesh sm, int index)
	{
		var mesh = GetMesh(index);
		
		if (mesh != null) mesh.SharedMesh = sm;
	}
	
	public Mesh GetSharedMesh(int index)
	{
		var mesh = GetMesh(index);
		
		return mesh != null ? mesh.SharedMesh : null;
	}
	
	public SGT_MultiMesh Clear()
	{
		if (meshes          != null) meshes          = null;
		if (meshGameObjects != null) meshGameObjects = SGT_Helper.DestroyGameObjects(meshGameObjects);
		
		return null;
	}
	
	public void DestroyAllMeshes()
	{
		if (meshes != null)
		{
			foreach (var mesh in meshes)
			{
				if (mesh != null)
				{
					mesh.DestroyMesh();
				}
			}
			
			Clear();
		}
	}
	
	public void CopyMeshesInto(SGT_MultiMesh target)
	{
		if (target != null && meshes != null)
		{
			target.Resize(meshes.Count);
			
			for (var i = 0; i < meshes.Count; i++)
			{
				var mesh = meshes[i];
				
				target.SetSharedMesh(mesh != null ? mesh.SharedMesh : null, i);
			}
		}
	}
	
	public static bool MeshesIdentical(SGT_MultiMesh a, SGT_MultiMesh b)
	{
		if (a == null && b != null) return false;
		if (a != null && b == null) return false;
		if (a != null && b != null)
		{
			if (a.meshes == null && b.meshes != null) return false;
			if (a.meshes != null && b.meshes == null) return false;
			if (a.meshes != null && b.meshes != null)
			{
				if (a.meshes.Count != b.meshes.Count) return false;
				
				for (var i = 0; i < a.meshes.Count; i++)
				{
					if (SGT_Mesh.MeshesIdentical(a.meshes[i], b.meshes[i]) == false)
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
		
		if (meshes != null)
		{
			foreach (var mesh in meshes)
			{
				if (mesh != null)
				{
					mesh.HideInEditor();
				}
			}
		}
	}
#endif
}
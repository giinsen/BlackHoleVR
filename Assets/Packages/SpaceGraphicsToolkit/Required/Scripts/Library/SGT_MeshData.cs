using ObjectList = System.Collections.Generic.List<UnityEngine.Object>;

using UnityEngine;

[System.Serializable]
public class SGT_MeshData
{
	[SerializeField]
	private bool modified;
	
	[SerializeField]
	private Mesh sharedMesh;
	
	/*[SerializeField]*/
	private Vector3[] positions;
	
	/*[SerializeField]*/
	private Vector3[] normals;
	
	/*[SerializeField]*/
	private Vector4[] tangents;
	
	/*[SerializeField]*/
	private Vector2[] uv0s;
	
	/*[SerializeField]*/
	private Vector2[] uv1s;
	
	/*[SerializeField]*/
	private Color[] colours;
	
	/*[SerializeField]*/
	private Color32[] colour32s;
	
	/*[SerializeField]*/
	private int[] indices;
	
	public Mesh SharedMesh
	{
		set
		{
			if (value != sharedMesh)
			{
				modified   = false;
				sharedMesh = value;
				positions  = null;
				normals    = null;
				tangents   = null;
				uv0s       = null;
				uv1s       = null;
				colours    = null;
				colour32s  = null;
				indices    = null;
			}
		}
		
		get
		{
			return sharedMesh;
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
	
	public Vector3[] Positions
	{
		get
		{
			if (positions == null || positions.Length == 0)
			{
				if (sharedMesh != null)
				{
					positions = sharedMesh.vertices;
				}
			}
			else if (sharedMesh == null)
			{
				positions = null;
			}
			
			return positions;
		}
	}
	
	public Vector3[] Normals
	{
		get
		{
			if (normals == null || normals.Length == 0)
			{
				if (sharedMesh != null)
				{
					normals = sharedMesh.normals;
				}
			}
			else if (sharedMesh == null)
			{
				normals = null;
			}
			
			return normals;
		}
	}
	
	public Vector4[] Tangents
	{
		get
		{
			if (tangents == null || tangents.Length == 0)
			{
				if (sharedMesh != null)
				{
					tangents = sharedMesh.tangents;
				}
			}
			else if (sharedMesh == null)
			{
				tangents = null;
			}
			
			return tangents;
		}
	}
	
	public Vector2[] Uv0s
	{
		get
		{
			if (uv0s == null || uv0s.Length == 0)
			{
				if (sharedMesh != null)
				{
					uv0s = sharedMesh.uv;
				}
			}
			else if (sharedMesh == null)
			{
				uv0s = null;
			}
			
			return uv0s;
		}
	}
	
	public Vector2[] Uv1s
	{
		get
		{
			if (uv1s == null || uv1s.Length == 0)
			{
				if (sharedMesh != null)
				{
					uv1s = sharedMesh.uv2;
				}
			}
			else if (sharedMesh == null)
			{
				uv1s = null;
			}
			
			return uv1s;
		}
	}
	
	public Color[] Colours
	{
		get
		{
			if (colours == null || colours.Length == 0)
			{
				if (sharedMesh != null)
				{
					colours = sharedMesh.colors;
				}
			}
			else if (sharedMesh == null)
			{
				colours = null;
			}
			
			return colours;
		}
	}
	
	public Color32[] Colour32s
	{
		get
		{
			if (colour32s == null || colour32s.Length == 0)
			{
				if (sharedMesh != null)
				{
					colour32s = sharedMesh.colors32;
				}
			}
			else if (sharedMesh == null)
			{
				colour32s = null;
			}
			
			return colour32s;
		}
	}
	
	public int[] Indices
	{
		get
		{
			if (indices == null)
			{
				if (sharedMesh != null)
				{
					indices = sharedMesh.triangles;
				}
			}
			else if (sharedMesh == null)
			{
				indices = null;
			}
			
			return indices;
		}
	}
	
	public void AddIndices(int[] a)
	{
		if (sharedMesh != null && a != null)
		{
			if (indices == null || indices.Length == 0)
			{
				indices = sharedMesh.triangles;
			}
			
			if (indices == null)
			{
				indices = (int[])a.Clone();
			}
			else
			{
				indices = SGT_Helper.ArrayConcat(indices, a);
			}
		}
	}
	
	public void AddPositions(Vector3[] a)
	{
		if (sharedMesh != null && a != null)
		{
			if (positions == null || positions.Length == 0)
			{
				positions = sharedMesh.vertices;
			}
			
			if (positions == null)
			{
				positions = (Vector3[])a.Clone();
			}
			else
			{
				positions = SGT_Helper.ArrayConcat(positions, a);
			}
		}
	}
	
	public void AddNormals(Vector3[] a)
	{
		if (sharedMesh != null && a != null)
		{
			if (normals == null || normals.Length == 0)
			{
				normals = sharedMesh.normals;
			}
			
			if (normals == null)
			{
				normals = (Vector3[])a.Clone();
			}
			else
			{
				normals = SGT_Helper.ArrayConcat(normals, a);
			}
		}
	}
	
	public void AddTangents(Vector4[] a)
	{
		if (sharedMesh != null && a != null)
		{
			if (tangents == null || tangents.Length == 0)
			{
				tangents = sharedMesh.tangents;
			}
			
			if (tangents == null)
			{
				tangents = (Vector4[])a.Clone();
			}
			else
			{
				tangents = SGT_Helper.ArrayConcat(tangents, a);
			}
		}
	}
	
	public void AddUv0s(Vector2[] a)
	{
		if (sharedMesh != null && a != null)
		{
			if (uv0s == null || uv0s.Length == 0)
			{
				uv0s = sharedMesh.uv;
			}
			
			if (uv0s == null)
			{
				uv0s = (Vector2[])a.Clone();
			}
			else
			{
				uv0s = SGT_Helper.ArrayConcat(uv0s, a);
			}
		}
	}
	
	public void AddUv1s(Vector2[] a)
	{
		if (sharedMesh != null && a != null)
		{
			if (uv1s == null || uv1s.Length == 0)
			{
				uv1s = sharedMesh.uv2;
			}
			
			if (uv1s == null)
			{
				uv1s = (Vector2[])a.Clone();
			}
			else
			{
				uv1s = SGT_Helper.ArrayConcat(uv1s, a);
			}
		}
	}
	
	public void AddColours(Color[] a)
	{
		if (sharedMesh != null && a != null)
		{
			if (colours == null || colours.Length == 0)
			{
				colours = sharedMesh.colors;
			}
			
			if (colours == null)
			{
				colours = (Color[])a.Clone();
			}
			else
			{
				colours = SGT_Helper.ArrayConcat(colours, a);
			}
		}
	}
	
	public void AddColour32s(Color32[] a)
	{
		if (sharedMesh != null && a != null)
		{
			if (colour32s == null || colour32s.Length == 0)
			{
				colour32s = sharedMesh.colors32;
			}
			
			if (colour32s == null)
			{
				colour32s = (Color32[])a.Clone();
			}
			else
			{
				colour32s = SGT_Helper.ArrayConcat(colour32s, a);
			}
		}
	}
	
	public void Apply()
	{
		if (sharedMesh != null)
		{
			if (positions != null && positions.Length > 0)
			{
				sharedMesh.vertices = positions;
			}
			
			if (normals != null && normals.Length > 0)
			{
				sharedMesh.normals = normals;
			}
			
			if (tangents != null && tangents.Length > 0)
			{
				sharedMesh.tangents = tangents;
			}
			
			if (uv0s != null && uv0s.Length > 0)
			{
				sharedMesh.uv = uv0s;
			}
			
			if (uv1s != null && uv1s.Length > 0)
			{
				sharedMesh.uv2 = uv1s;
			}
			
			if (colours != null && colours.Length > 0)
			{
				sharedMesh.colors = colours;
			}
			
			if (colour32s != null && colour32s.Length > 0)
			{
				sharedMesh.colors32 = colour32s;
			}
			
			if (indices != null && indices.Length > 0)
			{
				sharedMesh.triangles = indices;
			}
			
			modified = false;
		}
	}
	
	public SGT_MeshData(Mesh sm)
	{
		sharedMesh = sm;
	}
}
using IntArrayList     = System.Collections.Generic.List<int[]>;
using Vector2ArrayList = System.Collections.Generic.List<UnityEngine.Vector2[]>;
using Vector3ArrayList = System.Collections.Generic.List<UnityEngine.Vector3[]>;
using Vector4ArrayList = System.Collections.Generic.List<UnityEngine.Vector4[]>;

using UnityEngine;

public static class SGT_MeshHelper
{
	private static Vector3ArrayList verticesList  = new Vector3ArrayList();
	private static IntArrayList     trianglesList = new IntArrayList();
	private static Vector2ArrayList uvList        = new Vector2ArrayList();
	private static Vector2ArrayList uv1List       = new Vector2ArrayList();
	private static Vector3ArrayList normalsList   = new Vector3ArrayList();
	private static Vector4ArrayList tangentsList  = new Vector4ArrayList();
	
	public static Mesh CombineMeshes(CombineInstance[] instances, string newName)
	{
		var verticesTotal  = 0;
		var trianglesTotal = 0;
		var uvTotal        = 0;
		var uv1Total       = 0;
		var normalsTotal   = 0;
		var tangentsTotal  = 0;
		
		foreach (var instance in instances)
		{
			if (instance.mesh != null)
			{
				var vertices  = instance.mesh.vertices;
				var triangles = instance.mesh.triangles;
				var uv        = instance.mesh.uv;
				var uv1       = instance.mesh.uv2;
				var normals   = instance.mesh.normals;
				var tangents  = instance.mesh.tangents;
				
				verticesTotal  += vertices.Length;
				trianglesTotal += triangles.Length;
				uvTotal        += uv.Length;
				uv1Total       += uv1.Length;
				normalsTotal   += normals.Length;
				tangentsTotal  += tangents.Length;
				
				verticesList.Add(vertices);
				trianglesList.Add(triangles);
				uvList.Add(uv);
				uv1List.Add(uv1);
				normalsList.Add(normals);
				tangentsList.Add(tangents);
			}
		}
		
		var newVertices     = new Vector3[verticesTotal];
		var newTriangles    = new int[trianglesTotal];
		var newUv           = new Vector2[uvTotal];
		var newUv1          = new Vector2[uv1Total];
		var newNormals      = new Vector3[normalsTotal];
		var newTangents     = new Vector4[tangentsTotal];
		var verticesIndex   = 0;
		var trianglesIndex  = 0;
		var uvIndex         = 0;
		var uv1Index        = 0;
		var normalsIndex    = 0;
		var tangentsIndex   = 0;
		var trianglesOffset = 0;
		
		for (var i = 0; i < verticesList.Count; i++)
		{
			var vertices  = verticesList[i];
			var triangles = trianglesList[i];
			var uv        = uvList[i];
			var uv1       = uv1List[i];
			var normals   = normalsList[i];
			var tangents  = tangentsList[i];
			
			for (var j = 0; j < vertices.Length; j++)
			{
				newVertices[verticesIndex] = vertices[j];
				
				verticesIndex += 1;
			}
			
			for (var j = 0; j < triangles.Length; j++)
			{
				newTriangles[trianglesIndex] = triangles[j] + trianglesOffset;
				
				trianglesIndex += 1;
			}
			
			for (var j = 0; j < uv.Length; j++)
			{
				newUv[uvIndex] = uv[j];
				
				uvIndex += 1;
			}
			
			for (var j = 0; j < uv1.Length; j++)
			{
				newUv1[uv1Index] = uv1[j];
				
				uv1Index += 1;
			}
			
			for (var j = 0; j < normals.Length; j++)
			{
				newNormals[normalsIndex] = normals[j];
				
				normalsIndex += 1;
			}
			
			for (var j = 0; j < tangents.Length; j++)
			{
				newTangents[tangentsIndex] = tangents[j];
				
				tangentsIndex += 1;
			}
			
			trianglesOffset += vertices.Length;
		}
		
		verticesList.Clear();
		trianglesList.Clear();
		uvList.Clear();
		uv1List.Clear();
		normalsList.Clear();
		tangentsList.Clear();
		
		// Build mesh and return
		var newMesh = new Mesh();
		
		newMesh.vertices  = newVertices;
		newMesh.triangles = newTriangles;
		newMesh.uv        = newUv;
		newMesh.uv2       = newUv1;
		newMesh.normals   = newNormals;
		newMesh.tangents  = newTangents;
		newMesh.name      = newName;
		
		return newMesh;
	}
}
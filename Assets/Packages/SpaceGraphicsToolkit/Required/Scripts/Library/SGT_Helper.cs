// Comment out this line if you want to see Game Objects created by SGT
#define HIDE_HIERARCHY_IN_EDITOR

// Comment out this line if you want to see SGT object wireframe meshes in the scene view
#define HIDE_WIREFRAME_IN_EDITOR

using SeedStack      = System.Collections.Generic.Stack<int>;
using TimeScaleStack = System.Collections.Generic.Stack<float>;
using MeshList       = System.Collections.Generic.List<UnityEngine.Mesh>;
using GameObjectList = System.Collections.Generic.List<UnityEngine.GameObject>;
	
using UnityEngine;

public static class SGT_Helper
{
	private static SeedStack seedStack = new SeedStack();
	private static Texture2D checkerTexture;
	private static Texture2D asinTexture;
	private static Texture2D blackTexture;
	private static Texture2D redTexture;
	private static Texture2D greyTexture;
	private static int       asinTextureSize = 128;
	private static float     inscribedCube = 1.0f / Mathf.Sqrt(3.0f);
	private static float     inscribedBox  = 1.0f / Mathf.Sqrt(2.0f);
	
	public static readonly Color Black    = new Color(0.0f, 0.0f, 0.0f, 1.0f);
	public static readonly Color DarkGrey = new Color(0.25f, 0.25f, 0.25f, 1.0f);
	public static readonly Color Grey     = new Color(0.5f, 0.5f, 0.5f, 1.0f);
	
	public static int MeshVertexLimit
	{
		get
		{
			return 65000;
		}
	}
	
	public static Texture2D CheckerTexture
	{
		get
		{
			if (checkerTexture == null)
			{
				Color   white = Color.white;
				Color   color = new Color(0.7f, 0.7f, 0.7f);
				Color[] array = new Color[64];
				
				for (int i = 0; i < 8; i++)
				{
					for (int j = 0; j < 8; j++)
					{
						if ((i < 4 && j < 4) || (i >= 4 && j >= 4))
						{
							array[j + 8 * i] = color;
						}
						else
						{
							array[j + 8 * i] = white;
						}
					}
				}
				
				checkerTexture = new Texture2D(8, 8, TextureFormat.ARGB32, false);
				checkerTexture.hideFlags = HideFlags.HideAndDontSave;
				checkerTexture.SetPixels(array);
				checkerTexture.Apply();
			}
			
			return checkerTexture;
		}
	}
	
	public static Texture2D AsinTexture
	{
		get
		{
			if (asinTexture == null)
			{
				Color[] array = new Color[asinTextureSize];
				
				var t = -1.0f;
				var j = 2.0f / (float)asinTextureSize;
				t += j * 0.5f;
				for (int i = 0; i < asinTextureSize; i++)
				{
					var aSin = Mathf.Asin(t) / Mathf.PI + 0.5f;
					array[i] = new Color(aSin, aSin, aSin);
					
					t += j;
				}
				
				asinTexture = new Texture2D(asinTextureSize, 1, TextureFormat.ARGB32, false);
				asinTexture.wrapMode  = TextureWrapMode.Clamp;
				asinTexture.hideFlags = HideFlags.HideAndDontSave;
				asinTexture.SetPixels(array);
				asinTexture.Apply();
			}
			
			return asinTexture;
		}
	}
	
	public static Texture2D BlackTexture
	{
		get
		{
			if (blackTexture == null)
			{
				Color[] array = new Color[1] {Color.black};
				
				blackTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
				blackTexture.hideFlags = HideFlags.HideAndDontSave;
				blackTexture.SetPixels(array);
				blackTexture.Apply();
			}
			
			return blackTexture;
		}
	}
	
	public static Texture2D RedTexture
	{
		get
		{
			if (redTexture == null)
			{
				Color[] array = new Color[1] {Color.red};
				
				redTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
				redTexture.hideFlags = HideFlags.HideAndDontSave;
				redTexture.SetPixels(array);
				redTexture.Apply();
			}
			
			return redTexture;
		}
	}
	
	public static Texture2D GreyTexture
	{
		get
		{
			if (greyTexture == null)
			{
				Color[] array = new Color[1] {Color.grey};
				
				greyTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
				greyTexture.hideFlags = HideFlags.HideAndDontSave;
				greyTexture.SetPixels(array);
				greyTexture.Apply();
			}
			
			return greyTexture;
		}
	}
	
	public static float InscribedCube
	{
		get
		{
			return inscribedCube;
		}
	}
	
	public static float InscribedBox
	{
		get
		{
			return inscribedBox;
		}
	}
	
	public static float GlobalTime
	{
		get
		{
#if UNITY_EDITOR == true
			if (Application.isEditor == true)
			{
				return (float)UnityEditor.EditorApplication.timeSinceStartup;
			}
#endif
			return Time.time;
		}
	}
	
	public static float DeltaTime
	{
		get
		{
			if (Time.timeScale != 0.0f)
			{
				return Time.deltaTime / Time.timeScale;
			}
			return 1.0f / (float)Time.captureFramerate;
		}
	}
	
	public static float SmoothDeltaTime
	{
		get
		{
			if (Time.timeScale != 0.0f)
			{
				return Time.smoothDeltaTime / Time.timeScale;
			}
			return 1.0f / (float)Time.captureFramerate;
		}
	}
	
	public static Camera FindCamera()
	{
		if (Camera.main != null)
		{
			return Camera.main;
		}
		
		return Find<Camera>();
	}
	
	public static GameObject FindCameraGameObject()
	{
		if (Camera.main != null)
		{
			return Camera.main.gameObject;
		}
		
		var camera = Find<Camera>();
		
		return camera != null ? camera.gameObject : null;
	}
	
	public static T Find<T>()
		where T : Object
	{
		return (T)GameObject.FindObjectOfType(typeof(T));
	}
	
	public static T[] FindAll<T>()
		where T : Object
	{
		return (T[])GameObject.FindObjectsOfType(typeof(T));
	}
	
#if UNITY_EDITOR == true
	public static string FindAsset<T>(T asset, bool keepAssetDir = true, bool keepExtension = true)
		where T : Object
	{
		var path = string.Empty;
		
		if (Application.isEditor == true)
		{
			path = UnityEditor.AssetDatabase.GetAssetPath(asset);
			
			if (keepAssetDir == false && path.StartsWith("Assets/") == true)
			{
				path = path.Remove(0, "Assets/".Length);
			}
			
			if (keepExtension == false)
			{
				var d = path.LastIndexOf('.');
				var s = path.LastIndexOf('/');
				
				if (d != -1 && d > s)
				{
					path = path.Remove(d);
				}
			}
		}
		
		return path;
	}
#endif
	
#if UNITY_EDITOR == true
	public static T LoadAsset<T>(string path)
		where T : Object
	{
		var asset = default(T);
		
		if (Application.isEditor == true)
		{
			asset = UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(T)) as T;
		}
		
		return asset;
	}
#endif
	
	public static string GetAllCharactersInFont(Font f)
	{
		var characters = string.Empty;
		
		for (int i = 0; i < 10000; i++)
		{
			var c = (char)i;
			
			if (f.HasCharacter(c) == true)
			{
				characters += c;
			}
		}
		
		return characters;
	}
	
	public static void GizmoDrawCircle(Vector3 pos, Quaternion rot, float rad, float min, float max, bool sideLines = false, bool innerLines = false, int sides = 0)
	{
		var tot = max - min;
		
		if (sides == 0)
		{
			sides = Mathf.Max(2, (int)(tot / 10.0f));
		}
		
		var step = tot / sides;
		var a    = Vector3.zero;
		
		for (var i = 0; i <= sides; i++)
		{
			var b = pos + rot * (new Vector3(Mathf.Sin(min * Mathf.Deg2Rad), 0.0f, Mathf.Cos(min * Mathf.Deg2Rad)) * rad);
			
			if (i > 0)
			{
				Gizmos.DrawLine(a, b);
			}
			
			if (innerLines == true)
			{
				Gizmos.DrawLine(pos, b);
			}
			
			if (sideLines == true)
			{
				if (i == 0 || i == sides)
				{
					Gizmos.DrawLine(pos, b);
				}
			}
			
			a    = b;
			min += step;
		}
	}
	
	public static CubemapFace CubeFace(Vector3 direction)
	{
		var face = (CubemapFace)0;
		var x    = Mathf.Abs(direction.x);
		var y    = Mathf.Abs(direction.y);
		var z    = Mathf.Abs(direction.z);
		
		if ((x >= y) && (x >= z))
		{
			if (direction.x >= 0.0f)
			{
				face = CubemapFace.PositiveX;
			}
			else
			{
				face = CubemapFace.NegativeX;
			}
		}
		else if ((y >= x) && (y >= z))
		{
			if (direction.y >= 0.0f)
			{
				face = CubemapFace.PositiveY;
			}
			else
			{
				face = CubemapFace.NegativeY;
			}
		}
		else
		{
			if (direction.z >= 0.0f)
			{
				face = CubemapFace.PositiveZ;
			}
			else
			{
				face = CubemapFace.NegativeZ;
			}
		}
		
		return face;
	}
	
	public static float CubicInterpolate(float a, float b, float c, float d, float across)
	{
		float aSq = across * across;
		d = (d - c) - (a - b);
		return d * (aSq * across) + ((a - b) - d) * aSq + (c - a) * across + b;
	}
	
	public static float CosineInterpolate(float a, float b, float across)
	{
		across = (1.0f - Mathf.Cos(across * Mathf.PI)) * 0.5f;
		return a * (1.0f - across) + b * across;
	}
	
	public static float DampenFactor(float dampening, float elapsed)
	{
		return 1.0f - Mathf.Pow((float)System.Math.E, - dampening * elapsed);
	}
	
	public static float Frac(float v)
	{
		return v - (float)System.Math.Truncate(v);
	}
	
	public static float SampleDemCubic(Texture2D texture, Vector2 uv)
	{
		uv.x *= texture.width;
		uv.y *= texture.height;
		
		var x = (int)uv.x;
		var y = (int)uv.y;
		var z = Frac(uv.x);
		var w = Frac(uv.y);
		
		var v0 = texture.GetPixel(x - 1, y - 1).r;
		var v1 = texture.GetPixel(x    , y - 1).r;
		var v2 = texture.GetPixel(x + 1, y - 1).r;
		var v3 = texture.GetPixel(x + 2, y - 1).r;
		var x0 = texture.GetPixel(x - 1, y    ).r;
		var x1 = texture.GetPixel(x    , y    ).r;
		var x2 = texture.GetPixel(x + 1, y    ).r;
		var x3 = texture.GetPixel(x + 2, y    ).r;
		var y0 = texture.GetPixel(x - 1, y + 1).r;
		var y1 = texture.GetPixel(x    , y + 1).r;
		var y2 = texture.GetPixel(x + 1, y + 1).r;
		var y3 = texture.GetPixel(x + 2, y + 1).r;
		var z0 = texture.GetPixel(x - 1, y + 2).r;
		var z1 = texture.GetPixel(x    , y + 2).r;
		var z2 = texture.GetPixel(x + 1, y + 2).r;
		var z3 = texture.GetPixel(x + 2, y + 2).r;
		
		return CubicInterpolate(CubicInterpolate(v0, v1, v2, v3, z), CubicInterpolate(x0, x1, x2, x3, z), CubicInterpolate(y0, y1, y2, y3, z), CubicInterpolate(z0, z1, z2, z3, z), w);
	}
	
	public static float SampleDemCosine(Texture2D texture, Vector2 uv)
	{
		uv.x *= texture.width;
		uv.y *= texture.height;
		
		var x = (int)uv.x;
		var y = (int)uv.y;
		var z = Frac(uv.x);
		var w = Frac(uv.y);
		
		var x1 = texture.GetPixel(x    , y    ).r;
		var x2 = texture.GetPixel(x + 1, y    ).r;
		var y1 = texture.GetPixel(x    , y + 1).r;
		var y2 = texture.GetPixel(x + 1, y + 1).r;
		
		return CosineInterpolate(CosineInterpolate(x1, x2, z), CosineInterpolate(y1, y2, z), w);
	}
	
	public static Vector2 CubeUV(Vector3 direction, bool invert = false, bool fixY = false)
	{
		return CubeUV(CubeFace(direction), direction, invert, fixY);
	}
	
	public static Vector2 CubeUV(CubemapFace face, Vector3 direction, bool invert = false, bool fixY = false)
	{
		switch (face)
		{
			case CubemapFace.PositiveX: direction = Quaternion.Euler(0.0f, -90.0f, 0.0f) * direction; break; // X+
			case CubemapFace.NegativeX: direction = Quaternion.Euler(0.0f, 90.0f, 0.0f) * direction; break; // X-
			case CubemapFace.PositiveY: direction = Quaternion.Euler(90.0f, 0.0f, 0.0f) * Quaternion.Euler(0.0f, fixY ? -90.0f : 0.0f, 0.0f) * direction; break; // Y+
			case CubemapFace.NegativeY: direction = Quaternion.Euler(-90.0f, 0.0f, 0.0f) * Quaternion.Euler(0.0f, fixY ? -90.0f : 0.0f, 0.0f) * direction; break; // Y-
			case CubemapFace.PositiveZ: break; // Z+
			case CubemapFace.NegativeZ: direction = Quaternion.Euler(0.0f, 180.0f, 0.0f) * direction; break; // Z-
		}
		
		if (direction.z > 0.0f)
		{
			direction *= SGT_Helper.InscribedCube / direction.z;
			direction *= 1.0f / direction.z;
		}
		
		direction.x = 0.5f + direction.x * (invert ? -0.5f : 0.5f);
		direction.y = 0.5f + direction.y * (invert ? -0.5f : 0.5f);
		
		return new Vector2(direction.x, direction.y);
	}
	
	public static float ClampUV(float uv, float pixelUV)
	{
		return Mathf.Clamp(uv, pixelUV, 1.0f - pixelUV);
	}
	
	public static Vector2 ClampUV(Vector2 uv, Vector2 pixelUV)
	{
		uv.x = ClampUV(uv.x, pixelUV.x);
		uv.y = ClampUV(uv.y, pixelUV.y);
		
		return uv;
	}
	
	public static Vector2 ClampCollapseV(Vector2 uv, Vector2 pixelUV)
	{
		if (uv.y < pixelUV.y)
		{
			uv.x = 0.5f;
			uv.y = pixelUV.y;
		}
		
		if (uv.y > 1.0f - pixelUV.y)
		{
			uv.x = 0.5f;
			uv.y = 1.0f - pixelUV.y;
		}
		
		return uv;
	}
	
	public static Vector2 PixelUV(Texture texture)
	{
		if (texture != null)
		{
			return new Vector2(1.0f / (float)texture.width, 1.0f / (float)texture.height);
		}
		
		return Vector2.zero;
	}
	
	public static Vector3 ClosestPointToLineA(Vector3 lineA, Vector3 lineB, Vector3 point)
	{
		var pma = point - lineA;
		var bma = lineB - lineA;
		return lineA + Vector3.Dot(bma, pma) * bma / Vector3.Dot(bma, bma);
	}
	
	public static Vector3 ClosestPointToLineB(Vector3 line, Vector3 direction, Vector3 point)
	{
		return line + Vector3.Dot((point - line), direction) * direction;
	}
	
	public static bool PointInsideEllipse(Vector2 size, Vector3 point)
	{
		var pX = new Vector2(point.x, point.z).magnitude;
		return PointInsideEllipse(size, new Vector2(pX, point.y));
	}
	
	public static bool PointInsideEllipse(Vector2 size, Vector2 point)
	{
		var px = point.x * point.x;
		var py = point.y * point.y;
		var sx = size.x * size.x;
		var sy = size.y * size.y;
		
		return (px / sx) + (py / sy) < 1.0f;
	}
	
	// NOTE: This hack is only accurate when the distance is near 0
	public static float DistanceToEllipse(Vector2 size, Vector3 point3D)
	{
		/*
		Vector2 point2D;
		
		point2D.x = new Vector2(point3D.x, point3D.z).magnitude;
		point2D.y = point3D.y * (size.x / size.y);
		
		return point2D.magnitude - size.x;
		*/
		return EllipseDistance(size, new Vector2(new Vector2(point3D.x, point3D.z).magnitude ,point3D.y));
	}
	
	public static float EllipseDistance(Vector2 size, Vector2 point2D)
	{
		var latitude  = Mathf.Asin(point2D.y / point2D.magnitude);
		
		return Mathf.Abs(EllipseRadius(size, latitude) - point2D.magnitude);
	}
	
	public static float EllipseRadius(Vector2 size, Vector3 point3D)
	{
		return EllipseRadius(size, new Vector2(new Vector2(point3D.x, point3D.z).magnitude ,point3D.y));
	}
	
	public static float EllipseRadius(Vector2 size, Vector2 point2D)
	{
		return EllipseRadius(size, Mathf.Asin(point2D.y / point2D.magnitude));
	}
	
	public static float EllipseRadius(Vector2 size, float latitude)
	{
		var cos = Mathf.Cos(latitude);
		var sin = Mathf.Sin(latitude);
		
		return (size.x * size.y) / Mathf.Sqrt(Mathf.Pow(size.y * cos, 2.0f) + Mathf.Pow(size.x * sin, 2.0f));
	}
	
	public static float PolygonOuterBoundScale(float angle)
	{
		var point1 = new Vector2(Mathf.Sin(0.0f), Mathf.Cos(0.0f));
		var point2 = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
		var pointM = (point1 + point2) * 0.5f;
		
		return 1.0f / pointM.magnitude;
	}
	
	public static void SetLayer(GameObject root, int layer, bool recusrively = false)
	{
		if (root != null)
		{
			if (root.layer != layer)
			{
				root.layer = layer;
			}
			
			if (recusrively == true)
			{
				foreach (Transform t in root.transform)
				{
					SetLayer(t.gameObject, layer, true);
				}
			}
		}
	}
	
	public static void SetTag(GameObject root, string tag, bool recusrively = false)
	{
		if (root != null && tag != null)
		{
			if (root.tag != tag)
			{
				root.tag = tag;
			}
			
			if (recusrively == true)
			{
				foreach (Transform t in root.transform)
				{
					SetTag(t.gameObject, tag, true);
				}
			}
		}
	}
	
	// longLat.x = -PI   .. +PI
	// longLat.y = -PI/2 .. +PI/2
	public static Vector3 PolarToCartesian(Vector2 longLat)
	{
		var y = Mathf.Cos(longLat.y);
		
		return new Vector3(Mathf.Sin(longLat.x) * y, Mathf.Sin(longLat.y), Mathf.Cos(longLat.x) * y);
	}
	
	// return.x = -PI   .. +PI
	// return.y = -PI/2 .. +PI/2
	public static Vector2 CartesianToPolar(Vector3 xyz)
	{
		var longitude = Mathf.Atan2(xyz.x, xyz.z);
		var latitude  = Mathf.Asin(xyz.y / xyz.magnitude);
		
		return new Vector2(longitude, latitude);
	}
	
	// return.x = 0 .. 1
	// return.y = 0 .. 1
	public static Vector2 CartesianToPolarUV(Vector3 xyz)
	{
		var uv = CartesianToPolar(xyz);
		
		uv.x = Mathf.Repeat(0.5f - uv.x / (Mathf.PI * 2.0f), 1.0f);
		uv.y = 0.5f + uv.y /  Mathf.PI;
		
		return uv;
	}
	
	public static void BeginRandomSeed(int newSeed)
	{
		seedStack.Push(Random.seed);
		Random.seed = newSeed;
	}
	
	public static int EndRandomSeed()
	{
		var newSeed = Random.seed;
		
		Random.seed = seedStack.Pop();
		
		return newSeed;
	}
	
	public static Vector2 RandomOnUnitCircle
	{
		get
		{
			var t = Random.Range(-Mathf.PI, Mathf.PI);
			
			return new Vector2(Mathf.Sin(t), Mathf.Cos(t));
		}
	}
	
	public static T[] ArrayConcat<T>(T[] a, T[] b)
	{
		if (a != null && b != null)
		{
			var c = new T[a.Length + b.Length];
			
			a.CopyTo(c, 0);
			b.CopyTo(c, a.Length);
			
			return c;
		}
		
		return null;
	}
	
	public static bool ArrayPartiallyFilled<T>(T[] os)
		where T : Object // UnityEngine.Object's != comparer differs from the default
	{
		if (os != null)
		{
			foreach (var o in os)
			{
				if (o != null)
				{
					return true;
				}
			}
		}
		
		return false;
	}
	
	public static bool ArrayFilled<T>(T[] os, int minSize = 1)
		where T : Object // UnityEngine.Object's != comparer differs from the default
	{
		if (os != null && os.Length >= minSize)
		{
			foreach (var o in os)
			{
				if (o == null)
				{
					return false;
				}
			}
			
			return true;
		}
		
		return false;
	}
	
	public static T CloneObject<T>(T o)
		where T : Object
	{
		return o != null ? (T)Object.Instantiate(o) : null;
	}
	
	public static T[] CloneObjects<T>(T[] os, bool newArray = false)
		where T : Object
	{
		if (os != null)
		{
			var cs = newArray == true ? new T[os.Length] : os;
			
			for (var i = 0; i < os.Length; i++)
			{
				cs[i] = CloneObject(os[i]);
			}
			
			return cs;
		}
		
		return null;
	}
	
	public static System.Collections.Generic.List<T> CloneObjects<T>(System.Collections.Generic.List<T> os, bool newList = false)
		where T : Object
	{
		if (os != null)
		{
			if (newList == true)
			{
				var cs = new System.Collections.Generic.List<T>(os.Count);
				
				for (var i = 0; i < os.Count; i++)
				{
					cs.Add(CloneObject(os[i]));
				}
				
				return cs;
			}
			else
			{
				for (var i = 0; i < os.Count; i++)
				{
					os[i] = CloneObject(os[i]);
				}
				
				return os;
			}
		}
		
		return null;
	}
	
	public static Material CreateMaterial(string shaderName, int renderQueue = -1)
	{
		var shader = Shader.Find(shaderName);
		
		if (shader == null)
		{
			Debug.LogError("Failed to find the '" + shaderName + "' shader");
		}
		
		var material = new Material(shader);
		
		if (renderQueue != -1)
		{
			material.renderQueue = renderQueue;
		}
		
		return material;
	}
	
	public static GameObject CreateGameObject(GameObject source, Component parent, Vector3 xyz, Quaternion rot)
	{
		return CreateGameObject(source, parent != null ? parent.gameObject : null, xyz, rot);
	}
	
	public static void SetParent(GameObject source, GameObject newParent)
	{
		if (source != null)
		{
			SetParent(source.transform, newParent != null ? newParent.transform : null);
		}
	}
	
	public static void SetParent(Transform source, Transform newParent)
	{
		if (source != null && source.parent != newParent)
		{
			var oldLocalPosition = source.localPosition;
			var oldLocalRotation = source.localRotation;
			var oldLocalScale    = source.localScale;
			
			source.parent        = newParent;
			source.localPosition = oldLocalPosition;
			source.localRotation = oldLocalRotation;
			source.localScale    = oldLocalScale;
		}
	}
	
	public static GameObject CreateGameObject(GameObject gameObject, GameObject parent, Vector3 xyz, Quaternion rot)
	{
		if (gameObject != null)
		{
			gameObject = (GameObject)GameObject.Instantiate(gameObject, xyz, rot);
			
			if (parent != null)
			{
				SetParent(gameObject.transform, parent.transform);
			}
		}
		
		return gameObject;
	}
	
	public static GameObject CreateGameObject(string name, Component parent, HideFlags hideFlags = 0)
	{
		return CreateGameObject(name, parent != null ? parent.gameObject : null, hideFlags);
	}
	
	public static GameObject CreateGameObject(string name, GameObject parent, HideFlags hideFlags = 0)
	{
		var gameObject = new GameObject(name);
		
		gameObject.hideFlags = hideFlags;
		
		if (parent != null)
		{
			gameObject.transform.parent        = parent.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale    = Vector3.one;
		}
		
		return gameObject;
	}
	
	public static void DestroyGameObject(Component component)
	{
		if (component != null)
		{
			DestroyObject(component.gameObject);
		}
	}
	
	public static GameObject DestroyGameObject(GameObject gameObject)
	{
		return (GameObject)DestroyObject(gameObject);
	}
	
	public static GameObjectList DestroyGameObjects(GameObjectList gameObjects)
	{
		if (gameObjects != null)
		{
			for (var i = 0; i < gameObjects.Count; i++)
			{
				gameObjects[i] = DestroyGameObject(gameObjects[i]);
			}
		}
		
		return null;
	}
	
	public static GameObject[] DestroyGameObjects(GameObject[] gameObjects)
	{
		if (gameObjects != null)
		{
			for (var i = 0; i < gameObjects.Length; i++)
			{
				gameObjects[i] = DestroyGameObject(gameObjects[i]);
			}
		}
		
		return null;
	}
	
	public static void DestroyChildren(GameObject parent)
	{
		if (parent != null && parent.transform.childCount > 0)
		{
			for (var i = parent.transform.childCount - 1; i >= 0; i--)
			{
				DestroyGameObject(parent.transform.GetChild(i).gameObject);
			}
		}
	}
	
	public static System.Collections.Generic.List<T> DestroyObjects<T>(System.Collections.Generic.List<T> unityObjects)
		where T : Object
	{
		if (unityObjects != null)
		{
			for (var i = 0; i < unityObjects.Count; i++)
			{
				unityObjects[i] = DestroyObject(unityObjects[i]);
			}
		}
		
		return null;
	}
	
	public static T[] DestroyObjects<T>(T[] unityObjects)
		where T : Object
	{
		if (unityObjects != null)
		{
			for (var i = 0; i < unityObjects.Length; i++)
			{
				unityObjects[i] = DestroyObject(unityObjects[i]);
			}
		}
		
		return null;
	}
	
	public static T DestroyObject<T>(T unityObject)
		where T : Object
	{
		if (unityObject != null)
		{
			if (UnityEngine.Application.isPlaying == true)
			{
				Object.Destroy(unityObject);
			}
			else
			{
				Object.DestroyImmediate(unityObject);
			}
		}
		
		return null;
	}
	
	public static Vector3 NewVector3(float xyz)
	{
		return new Vector3(xyz, xyz, xyz);
	}
	
	public static Vector3 NewVector3(Vector2 xy, float z)
	{
		return new Vector3(xy.x, xy.y, z);
	}
	
	public static Vector4 NewVector4(Vector3 xyz, float w)
	{
		return new Vector4(xyz.x, xyz.y, xyz.z, w);
	}
	
	public static bool MaskInMask(int a, int b)
	{
		return (a & b) != 0;
	}
	
	public static bool MaskInMask(object a, object b)
	{
		if (a == null || a.GetType().IsEnum == false)
		{
			Debug.LogError("a isn't an enum.");
			
			return false;
		}
		
		if (b == null || b.GetType().IsEnum == false)
		{
			Debug.LogError("b isn't an enum.");
			
			return false;
		}
		
		return MaskInMask((int)a, (int)b);
	}
	
	public static T[] ResizeArray<T>(T[] a, int s, bool createArrayIfNull = true)
	{
		s = Mathf.Max(0, s);
		
		if (a == null && createArrayIfNull == true)
		{
			a = new T[s];
		}
		
		if (a != null)
		{
			System.Array.Resize(ref a, s);
		}
		
		return a;
	}
	
	public static T[] ResizeArray<T>(T[] a, int s, bool createArrayIfNull, bool createElementIfNull)
		where T : class, new()
	{
		a = ResizeArray(a, s, createArrayIfNull);
		
		if (a != null && createElementIfNull == true)
		{
			for (var i = 0; i < a.Length; i++)
			{
				if (a[i] == null)
				{
					a[i] = new T();
				}
			}
		}
		
		return a;
	}
	
	public static System.Collections.Generic.List<T> ResizeArray<T>(System.Collections.Generic.List<T> a, int s, bool createArrayIfNull = true)
	{
		s = Mathf.Max(0, s);
		
		if (a == null && createArrayIfNull == true)
		{
			a = new System.Collections.Generic.List<T>();
		}
		
		if (a != null)
		{
			// Add elements?
			if (s > a.Count)
			{
				for (var i = a.Count; i < s; i++)
				{
					a.Add(default(T));
				}
			}
			// Remove elements?
			else if (a.Count > s)
			{
				var c = a.Count - s;
				
				a.RemoveRange(a.Count - c, c);
			}
		}
		
		return a;
	}
	
	public static System.Collections.Generic.List<T> ResizeArray<T>(ref System.Collections.Generic.List<T> a, int s, bool createArrayIfNull, bool createElementIfNull)
		where T : class, new()
	{
		ResizeArray(a, s, createArrayIfNull);
		
		if (a != null && createElementIfNull == true)
		{
			for (var i = 0; i < a.Count; i++)
			{
				if (a[i] == null)
				{
					a[i] = new T();
				}
			}
		}
		
		return a;
	}
	
	public static T ArrayIndex<T>(ref System.Collections.Generic.List<T> array, int index, bool createElementIfNull)
		where T : class, new()
	{
		if (array == null)
		{
			array = new System.Collections.Generic.List<T>(index);
		}
		
		// Resize
		if (index >= array.Count)
		{
			array.Capacity = index;
			
			for (var i = array.Count; i < index; i++)
			{
				array.Add(default(T));
			}
		}
		
		var element = array[index];
		
		if (element == null)
		{
			element = new T();
			
			array[index] = element;
		}
		
		return element;
	}
	
	public static System.Collections.Generic.List<T> MinArray<T>(System.Collections.Generic.List<T> a, int s, bool createArrayIfNull = true)
	{
		if (a == null && createArrayIfNull == true)
		{
			a = new System.Collections.Generic.List<T>();
		}
		
		if (a != null && s >= 0)
		{
			if (s > a.Count)
			{
				for (var i = a.Count; i < s; i++)
				{
					a.Add(default(T));
				}
			}
		}
		
		return a;
	}
	
	public static T[] MinArray<T>(T[] a, int s, bool createArrayIfNull = true)
	{
		s = Mathf.Max(0, s);
		
		if (a == null && createArrayIfNull == true)
		{
			a = new T[s];
		}
		
		var b = a;
		
		if (b != null && b.Length < s)
		{
			b = new T[s];
			a.CopyTo(b, 0);
		}
		
		return b;
	}
	
	public static bool ArraysDiffer<T>(System.Collections.Generic.List<T> a, T[] b)
		where T : class
	{
		if (a == null || b == null)
		{
			return true;
		}
		
		if (a.Count != b.Length)
		{
			return true;
		}
		
		for (var i = 0; i < a.Count; i++)
		{
			if (a[i] != b[i])
			{
				return true;
			}
		}
		
		return false;
	}
	
	public static bool ArraysDiffer<T>(T[] a, T[] b)
		where T : class
	{
		if (a == null || b == null)
		{
			return true;
		}
		
		if (a.Length != b.Length)
		{
			return true;
		}
		
		for (var i = 0; i < a.Length; i++)
		{
			if (a[i] != b[i])
			{
				return true;
			}
		}
		
		return false;
	}
	
	public static T[] MergeArrays<T>(T[] a, T[] b)
	{
		var aLength = a != null ? a.Length : 0;
		var bLength = b != null ? b.Length : 0;
		var c       = new T[aLength + bLength];
		
		if (aLength > 0)
		{
			System.Array.Copy(a, 0, c, 0, aLength);
		}
		
		if (bLength > 0)
		{
			System.Array.Copy(b, aLength, c, 0, bLength);
		}
		
		return c;
	}
	
	public static T[] Merge2DArray<T>(T[][] array2D)
	{
		if (array2D != null)
		{
			var totalSize = 0;
			
			foreach (var array in array2D)
			{
				if (array != null)
				{
					totalSize += array.Length;
				}
			}
			
			var totalElements = new T[totalSize];
			var currentIndex  = 0;
			
			foreach (var array in array2D)
			{
				if (array != null)
				{
					for (var i = 0; i < array.Length; i++)
					{
						totalElements[currentIndex] = array[i];
						
						currentIndex++;
					}
				}
			}
			
			return totalElements;
		}
		
		return null;
	}
	
#if UNITY_EDITOR == true
	public static void HideWireframe(GameObject go)
	{
		if (go != null) HideWireframe(go.GetComponent<Renderer>());
	}
	
	public static void HideWireframe(Renderer r)
	{
 #if HIDE_WIREFRAME_IN_EDITOR == true
		UnityEditor.EditorUtility.SetSelectedWireframeHidden(r, true);
 #else
		UnityEditor.EditorUtility.SetSelectedWireframeHidden(r, false);
 #endif
	}
	
	public static void HideGameObject(Component c)
	{
		if (c != null) HideGameObject(c.gameObject);
	}
	
	public static void HideGameObject(GameObject go)
	{
		if (go != null)
		{
 #if HIDE_HIERARCHY_IN_EDITOR == true
			go.hideFlags = HideFlags.HideInHierarchy;
 #else
			go.hideFlags = 0;
 #endif
		}
	}
	
	public static void ShowGameObject(Component c)
	{
		if (c != null) ShowGameObject(c.gameObject);
	}
	
	public static void ShowGameObject(GameObject go)
	{
		if (go != null) go.hideFlags = 0;
	}
	
	public static void HideComponent(Component c)
	{
		if (c != null)
		{
 #if HIDE_HIERARCHY_IN_EDITOR == true
			c.hideFlags = HideFlags.HideInInspector;
 #else
			c.hideFlags = 0;
 #endif
		}
	}
	
	public static void ShowComponent(Component c)
	{
		if (c != null) c.hideFlags = 0;
	}
#endif
	
	public static void SetRenderQueues(Material[] ms, int rq)
	{
		if (ms != null)
		{
			foreach (var m in ms)
			{
				SetRenderQueue(m, rq);
			}
		}
	}
	
	public static void SetRenderQueue(Material m, int rq)
	{
		if (m != null)
		{
			if (m.renderQueue != rq)
			{
				m.renderQueue = rq;
			}
		}
	}
	
	public static int GetRenderQueue(Material m)
	{
		return m != null ? m.renderQueue : 0;
	}
	
	public static void SetSharedMesh(MeshFilter mf, Mesh m)
	{
		if (mf != null)
		{
			if (mf.sharedMesh != m)
			{
				mf.sharedMesh = m;
			}
		}
	}
	
	public static void SetSharedMaterial(GameObject go, Material m)
	{
		if (go != null) SetSharedMaterial(go.GetComponent<Renderer>(), m);
	}
	
	public static void SetSharedMaterial(Renderer r, Material m)
	{
		if (r != null)
		{
			if (r.sharedMaterial != m)
			{
				r.sharedMaterial = m;
			}
		}
	}
	
	public static void InsertSharedMaterial(Renderer r, Material m, bool recursive)
	{
		if (r != null)
		{
			if (recursive == true)
			{
				for (var i = r.transform.childCount - 1; i >= 0; i--)
				{
					InsertSharedMaterial(r.transform.GetChild(i).GetComponent<Renderer>(), m, recursive);
				}
			}
			
			var sms = r.sharedMaterials;
			
			foreach (var sm in sms)
			{
				if (sm == m) return;
			}
			
			SGT_ArrayHelper.Resize(ref sms, sms.Length + 1, true);
			
			sms[sms.Length - 1] = m;
			
			r.sharedMaterials = sms;
		}
	}
	
	public static void RemoveSharedMaterial(Renderer r, Material m, bool recursive)
	{
		if (r != null)
		{
			if (recursive == true)
			{
				for (var i = r.transform.childCount - 1; i >= 0; i--)
				{
					RemoveSharedMaterial(r.transform.GetChild(i).GetComponent<Renderer>(), m, recursive);
				}
			}
			
			var sms = r.sharedMaterials;
			
			for (var i = sms.Length - 1; i >= 0; i--)
			{
				if (sms[i] == m)
				{
					SGT_ArrayHelper.Remove(ref sms, i);
					
					r.sharedMaterials = sms;
					
					return;
				}
			}
		}
	}
	
	public static void SetPosition(Transform t, Vector3 v)
	{
		if (t != null)
		{
#if UNITY_EDITOR == true
			if (Application.isPlaying == false && t.position == v) return;
#endif
			t.position = v;
		}
	}
	
	public static void SetLocalPosition(Transform t, Vector3 v)
	{
		if (t != null)
		{
#if UNITY_EDITOR == true
			if (Application.isPlaying == false && t.localPosition == v) return;
#endif
			t.localPosition = v;
		}
	}
	
	public static void SetUp(Transform t, Vector3 v)
	{
		if (t != null)
		{
			var rot = Quaternion.FromToRotation(Vector3.up, v);
			
#if UNITY_EDITOR == true
			if (Application.isPlaying == false && t.localRotation == rot) return;
#endif
			t.localRotation = rot;
		}
	}
	
	public static void SetLocalScale(Transform t, float v)
	{
		SetLocalScale(t, new Vector3(v, v, v));
	}
	
	public static void SetLocalScale(Transform t, Vector3 v)
	{
		if (t != null)
		{
#if UNITY_EDITOR == true
			if (Application.isPlaying == false && t.localScale == v) return;
#endif
			if (t.localScale == v) return;
			
			t.localScale = v;
		}
	}
	
	public static void SetLocalRotation(Transform t, Quaternion q)
	{
		if (t != null)
		{
#if UNITY_EDITOR == true
			if (Application.isPlaying == false && t.localRotation == q) return;
#endif
			t.localRotation = q;
		}
	}
	
	public static void SetRotation(Transform t, Quaternion q)
	{
		if (t != null)
		{
#if UNITY_EDITOR == true
			if (Application.isPlaying == false && t.rotation == q) return;
#endif
			t.rotation = q;
		}
	}
	
	public static void EnableRenderer(Component c)
	{
		if (c != null)
		{
			EnableRenderer(c.GetComponent<Renderer>());
		}
	}
	
	public static void DisableRenderer(Component c)
	{
		if (c != null)
		{
			DisableRenderer(c.GetComponent<Renderer>());
		}
	}
	
	public static void EnableRenderer(GameObject go)
	{
		if (go != null)
		{
			EnableRenderer(go.GetComponent<Renderer>());
		}
	}
	
	public static void EnableRenderer(Renderer r)
	{
		if (r != null)
		{
			if (r.enabled == false)
			{
				r.enabled = true;
			}
		}
	}
	
	public static void EnableCollider(GameObject go)
	{
		if (go != null)
		{
			EnableCollider(go.GetComponent<Collider>());
		}
	}
	
	public static void EnableCollider(Collider c)
	{
		if (c != null)
		{
			if (c.enabled == false)
			{
				c.enabled = true;
			}
		}
	}
	
	public static void DisableCollider(GameObject go)
	{
		if (go != null)
		{
			DisableCollider(go.GetComponent<Collider>());
		}
	}
	
	public static void DisableCollider(Collider c)
	{
		if (c != null)
		{
			if (c.enabled == true)
			{
				c.enabled = false;
			}
		}
	}
	
	public static void DisableRenderer(GameObject go)
	{
		if (go != null)
		{
			DisableRenderer(go.GetComponent<Renderer>());
		}
	}
	
	public static void DisableRenderer(Renderer r)
	{
		if (r != null)
		{
			if (r.enabled == true)
			{
				r.enabled = false;
			}
		}
	}
	
	public static bool Approximately(Color a, Color b)
	{
		return Mathf.Approximately(a.r, b.r) &&
		       Mathf.Approximately(a.g, b.g) &&
		       Mathf.Approximately(a.b, b.b) &&
		       Mathf.Approximately(a.a, b.a);
	}
	
	public static Vector3 RotateZ(Vector3 v, float a)
	{
		Vector2 o = Rotate(new Vector2(v.x, v.y), a);
		
		return new Vector3(o.x, o.y, v.z);
	}
	
	public static Vector2 Rotate(Vector2 v, float a)
	{
		var sin = Mathf.Sin(a);
		var cos = Mathf.Cos(a);
		
		Vector2 o;
		
		o.x = cos * v.x - sin * v.y;
		o.y = sin * v.x + cos * v.y;
		
		return o;
	}
	
	public static float RemapClamped(float oldMin, float oldMax, float oldValue, float newMin, float newMax)
	{
		return Mathf.Clamp(Remap(oldMin, oldMax, oldValue, newMin, newMax), newMin, newMax);
	}
	
	public static float Remap(float oldMin, float oldMax, float oldValue, float newMin, float newMax)
	{
		if (oldMin != oldMax)
		{
			var value01 = (oldValue - oldMin) / (oldMax - oldMin);
			
			return newMin + value01 * (newMax - newMin);
		}
		
		return newMin;
	}
	
	public static bool RewritableTextureFormat(TextureFormat f)
	{
		return f == TextureFormat.ARGB32 || f == TextureFormat.RGB24 || f == TextureFormat.Alpha8;
	}
	
	public static Texture2D ResizeTexture(Texture2D oldTexture, int newWidth, int newHeight)
	{
		Texture2D newTexture = null;
		
		if (oldTexture != null && RewritableTextureFormat(oldTexture.format) == true)
		{
			newWidth   = Mathf.Max(0, newWidth);
			newHeight  = Mathf.Max(0, newHeight);
			newTexture = new Texture2D(newWidth, newHeight, oldTexture.format, false);
			
			for (var y = 0; y < newHeight; y++)
			{
				for (var x = 0; x < newWidth; x++)
				{
					var fX = (float)x / (float)newWidth;
					var fY = (float)y / (float)newHeight;
					
					newTexture.SetPixel(x, y, oldTexture.GetPixelBilinear(fX, fY));
				}
			}
			
			newTexture.Apply();
		}
		
		return newTexture;
	}
	
	public static float Reciprocal(float v)
	{
		if (v != 0.0f)
		{
			return 1.0f / v;
		}
		return 0.0f;
	}
	
	public static Vector3 Reciprocal(Vector3 v)
	{
		v.x = Reciprocal(v.x);
		v.y = Reciprocal(v.y);
		v.z = Reciprocal(v.z);
		return v;
	}
	
	public static float Clamp(float v, float min, float max)
	{
		if (min < max)
		{
			return Mathf.Clamp(v, min, max);
		}
		return Mathf.Clamp(v, max, min);
		
	}
	
	public static float RadiansPerSecond(float period)
	{
		if (Mathf.Approximately(period, 0.0f) == false)
		{
			return (Mathf.PI * 2.0f) / period;
		}
		return 0.0f;
	}
	
	public static float DegreesPerSecond(float period)
	{
		if (Mathf.Approximately(period, 0.0f) == false)
		{
			return 360.0f / period;
		}
		return 0.0f;
	}
	
	public static void UpdateNonOrthogonalTransform(Transform t)
	{
		// Fully updating the transform requires reparenting it
		if (t != null && t.parent != null)
		{
			var localPosition = t.localPosition;
			var localRotation = t.localRotation;
			var localScale    = t.localScale;
			
			t.parent = t.parent;
			
			t.localPosition = localPosition;
			t.localRotation = localRotation;
			t.localScale    = localScale;
		}
	}
	
	public static Vector3 GetPosition(Component component)
	{
		if (component != null)
		{
			return component.transform.position;
		}
		return Vector3.zero;
	}
	
	public static Vector3 GetPosition(GameObject gameObject)
	{
		if (gameObject != null)
		{
			return gameObject.transform.position;
		}
		return Vector3.zero;
	}
	
	public static float Expose(float v)
	{
		return 1.0f - Mathf.Exp(-v);
	}
	
	public static Vector3 IntersectSphere2Ray(Vector3 origin, Vector3 direction, Vector3 centre, float radius)
	{
		var R = origin - centre;
		var B = Vector3.Dot(R, direction);
		var C = Vector3.Dot(R, R) - radius * radius;
		var D = B * B - C;
		return origin + (-B - Mathf.Sqrt(D)) * direction;
	}
	
	public static Vector3 IntersectSphere2Ray(Vector3 origin, Vector3 direction)
	{
		var B = Vector3.Dot(origin, direction);
		var C = Vector3.Dot(origin, origin) - 1.0f;
		var D = B * B - C;
		return origin + (-B - Mathf.Sqrt(D)) * direction;
	}
	
	public static bool IntersectRayToSphereA(Vector3 origin, Vector3 direction, Vector3 centre, float radius, out float distance)
	{
		distance = new float();
		
		var b = Vector3.Dot(direction, origin - centre);
		var c = (origin - centre).sqrMagnitude - (radius * radius);
		var d = b * b - c;
		
		if (d >= 0.0f)
		{
			var dist = -b - Mathf.Sqrt(d);
			if (dist >= 0.0f)
			{
				distance = dist;
				
				return true;
			}
		}
		
		return false;
	}
	
	public static bool IntersectRayToSphereB(Vector3 origin, Vector3 direction, Vector3 centre, float radius, out float distance)
	{
		distance = new float();
		
		var b = Vector3.Dot(direction, origin - centre);
		var c = (origin - centre).sqrMagnitude - (radius * radius);
		var d = b * b - c;
		
		if (d >= 0.0f)
		{
			var dist = -b - Mathf.Sqrt(d);
			if (dist < 0.0f)
			{
				distance = Mathf.Abs(dist);
				
				return true;
			}
		}
		
		return false;
	}
	
	public static float DistanceToHorizon(float radius, float distanceToCentre)
	{
		if (distanceToCentre > radius)
		{
			return Mathf.Sqrt(distanceToCentre * distanceToCentre - radius * radius);
		}
		
		return 0.0f;
	}
	
	public static Matrix4x4 CalculateSphereShadowMatrix(float occluderRadius, Vector3 occluderPosition, Vector3 lightSourcePosition, Vector3 receiverPosition, float uniformScale)
	{
		var occluderToStarRot = Quaternion.LookRotation(lightSourcePosition - occluderPosition);
		var ellipseSize       = new Vector3(occluderRadius, occluderRadius, 1.0f);
		var shadowMatrix      = Matrix4x4.identity;
		
		shadowMatrix *= SGT_MatrixHelper.Translation(occluderPosition); // The rotation point must be this or the occluder
		shadowMatrix *= SGT_MatrixHelper.Rotation(occluderToStarRot);
		shadowMatrix *= SGT_MatrixHelper.Scaling(ellipseSize * uniformScale);
		
		shadowMatrix = shadowMatrix.inverse;
		
		return shadowMatrix;
	}
	
	public static Quaternion Quaternion_LookRotation(Vector3 dir)
	{
		if (dir.magnitude > 0.0f)
		{
			return Quaternion.LookRotation(dir);
		}
		
		return Quaternion.identity;
	}
	
	public static Matrix4x4 CalculateCircleShadowMatrix(float occluderRadius, Vector3 occluderPosition, Vector3 occluderUp, Vector3 lightSourcePosition, Vector3 receiverPosition, float uniformScale)
	{
		var starToPlanetRot = Quaternion_LookRotation(lightSourcePosition - occluderPosition);
		var pole            = Quaternion.Inverse(starToPlanetRot) * occluderUp;
		var squash          = Vector3.Dot(pole, -Vector3.forward);
		var ellipseSize     = new Vector3(occluderRadius, occluderRadius * squash, 1.0f);
		var shadowMatrix    = Matrix4x4.identity;
		
		// Roll rotation to the squash angle
		starToPlanetRot *= Quaternion.Euler(0.0f, 0.0f, -Mathf.Atan2(pole.x, pole.y) * Mathf.Rad2Deg);
		
		shadowMatrix *= SGT_MatrixHelper.Translation(receiverPosition);
		shadowMatrix *= SGT_MatrixHelper.Rotation(starToPlanetRot);
		shadowMatrix *= SGT_MatrixHelper.Scaling(ellipseSize * uniformScale);
		
		shadowMatrix = shadowMatrix.inverse;
		
		return shadowMatrix;
	}
	
	public static void CalculateHorizonAtmosphereDepth(float surfaceRadius, float atmosphereRadius, out float surfaceDepth, out float atmosphereDepth)
	{
		surfaceDepth    = Mathf.Sin(Mathf.Acos(surfaceRadius / atmosphereRadius)) * atmosphereRadius;
		atmosphereDepth = surfaceDepth * 2.0f;
	}
	
	public static void CalculateHorizonAtmosphereDepth(float surfaceRadius, float atmosphereRadius, float distance, out float surfaceDepth, out float atmosphereDepth)
	{
		var surfDist           = DistanceToHorizon(surfaceRadius, distance);
		var maxAtmosphereDepth = Mathf.Sin(Mathf.Acos(surfaceRadius / atmosphereRadius)) * atmosphereRadius;
		var maxSurfaceDepth    = Mathf.Min(surfDist, maxAtmosphereDepth);
		
		surfaceDepth    = Mathf.Max(maxSurfaceDepth, atmosphereRadius - surfaceRadius);
		atmosphereDepth = maxAtmosphereDepth + maxSurfaceDepth;
	}
	
	public delegate float ClosestComparer<T>(T item);
	
	public static T ClosestListItem<T>(System.Collections.Generic.List<T> list, ClosestComparer<T> del)
	{
		return list[ClosestListIndex(list, del)];
	}
	
	public static int ClosestListIndex<T>(System.Collections.Generic.List<T> list, ClosestComparer<T> del)
	{
		var bestIndex    = -1;
		var bestDistance = 0.0f;
		
		if (list.Count > 0)
		{
			for (var i = 0; i < list.Count; i++)
			{
				var distance = del(list[i]);
				if (distance >= 0.0f && (bestIndex == -1 || distance < bestDistance))
				{
					bestIndex    = i;
					bestDistance = distance;
				}
			}
			
			if (bestIndex == -1)
			{
				for (var i = 0; i < list.Count; i++)
				{
					var distance = Mathf.Abs(del(list[i]));
					if (distance >= 0.0f && (bestIndex == -1 || distance < bestDistance))
					{
						bestIndex    = i;
						bestDistance = distance;
					}
				}
			}
		}
		
		return bestIndex;
	}
	
	public delegate bool BestComparer<T>(T item, T best);
	
	public static T BestListItem<T>(System.Collections.Generic.List<T> list, BestComparer<T> del)
	{
		var output = default(T);
		
		var index = BestListIndex(list, del);
		if (index != -1)
		{
			output = list[index];
		}
		
		return output;
	}
	
	public static int BestListIndex<T>(System.Collections.Generic.List<T> list, BestComparer<T> del)
	{
		var bestIndex = -1;
		
		if (list.Count > 0)
		{
			bestIndex = 0;
			
			if (list.Count > 1)
			{
				for (var i = 1; i < list.Count; i++)
				{
					if (del(list[i], list[bestIndex]) == true)
					{
						bestIndex = i;
					}
				}
			}
		}
		
		return bestIndex;
	}
	
	public static Color Lerp(Color a, Color b, Color t)
	{
		t.r = a.r + (b.r - a.r) * t.r;
		t.g = a.g + (b.g - a.g) * t.g;
		t.b = a.b + (b.b - a.b) * t.b;
		t.a = a.a + (b.a - a.a) * t.a;
		return t;
	}
	
	public static Color SmoothStep(Color a, Color b, float across)
	{
		a.r = Mathf.SmoothStep(a.r, b.r, across);
		a.g = Mathf.SmoothStep(a.g, b.g, across);
		a.b = Mathf.SmoothStep(a.b, b.b, across);
		a.a = Mathf.SmoothStep(a.a, b.a, across);
		
		return a;
	}
	
	public static Color DivideAlpha(Color c)
	{
		if (c.a > 0.0f)
		{
			c.r /= c.a;
			c.g /= c.a;
			c.b /= c.a;
		}
		return c;
	}
	
	public static Color Premultiply(Color a)
	{
		a.r *= a.a;
		a.g *= a.a;
		a.b *= a.a;
		return a;
	}
	
	public static bool IsBlack(Color a)
	{
		return a.r <= 0.0f && a.g <= 0.0f && a.b <= 0.0f;
	}
	
	public static Color MultiplyRGB(Color a, Color b)
	{
		a.r *= b.r;
		a.g *= b.g;
		a.b *= b.b;
		return a;
	}
	
	public static Color MultiplyRGBWeightA(Color a, Color b)
	{
		a.r = Mathf.Lerp(a.r, a.r * b.r, b.a);
		a.g = Mathf.Lerp(a.g, a.g * b.g, b.a);
		a.b = Mathf.Lerp(a.b, a.b * b.b, b.a);
		return a;
	}
	
	public static Color AlphaBlend(Color a, Color b)
	{
		a.r = Mathf.Lerp(a.r, b.r, b.a);
		a.g = Mathf.Lerp(a.g, b.g, b.a);
		a.b = Mathf.Lerp(a.b, b.b, b.a);
		return a;
	}
	
	public enum ChannelBlendMode
	{
		Normal,
		Darken,
		Multiply,
		ColourBurn,
		LinearBurn,
		DarkerColour,
		Lighten,
		Screen,
		ColourDodge,
		LinearDodge,
		LighterColour,
		Overlay,
		SoftLight,
		HardLight,
		VividLight,
		LinearLight,
		PinLight,
		HardMix,
		Difference,
		Exclusion,
		Subtract,
		Divide
	}
	
	public static float BlendChannel(float a, float b, ChannelBlendMode blendMode)
	{
		float o = b;
		
		switch (blendMode)
		{
			case ChannelBlendMode.Normal:
				o = b;
			break;
			case ChannelBlendMode.Darken:
				o = b > a ? a : b;
			break;
			case ChannelBlendMode.Multiply:
				o = a * b;
			break;
			case ChannelBlendMode.ColourBurn:
				o = b;
			break;
			case ChannelBlendMode.LinearBurn:
				o = b;
			break;
			case ChannelBlendMode.DarkerColour:
				o = b;
			break;
			case ChannelBlendMode.Lighten:
				o = b > a ? b : a;
			break;
			case ChannelBlendMode.Screen:
				o = 1.0f - (1.0f - a) * (1.0f - b);
			break;
			case ChannelBlendMode.ColourDodge:
				o = b;
			break;
			case ChannelBlendMode.LinearDodge:
				o = Mathf.Clamp01(a + b);
			break;
			case ChannelBlendMode.LighterColour:
				o = b;
			break;
			case ChannelBlendMode.Overlay:
				o = b;
			break;
			case ChannelBlendMode.SoftLight:
				o = b;
			break;
			case ChannelBlendMode.HardLight:
				o = b;
			break;
			case ChannelBlendMode.VividLight:
				o = b;
			break;
			case ChannelBlendMode.LinearLight:
				o = b;
			break;
			case ChannelBlendMode.PinLight:
				o = b;
			break;
			case ChannelBlendMode.HardMix:
				o = b;
			break;
			case ChannelBlendMode.Difference:
				o = Mathf.Abs(a + b);
			break;
			case ChannelBlendMode.Exclusion:
				o = b;
			break;
			case ChannelBlendMode.Subtract:
				o = (a + b < 255) ? 0 : (a + b - 255);
			break;
			case ChannelBlendMode.Divide:
				o = b;
			break;
		}
		
		return o;
	}
	
	public static Color BlendRGB(Color a, Color b, ChannelBlendMode blendMode)
	{
		a.r = BlendChannel(a.r, b.r, blendMode);
		a.g = BlendChannel(a.g, b.g, blendMode);
		a.b = BlendChannel(a.b, b.b, blendMode);
		return a;
	}
	
	public static Color PreventZeroRGB(Color c)
	{
		/*
		if (c.r == 0.0f)
		{
			c.r = 3.0f / 255.0f;
		}
		
		if (c.g == 0.0f)
		{
			c.g = 3.0f / 255.0f;
		}
		
		if (c.b == 0.0f)
		{
			c.b = 3.0f / 255.0f;
		}
		*/
		
		c.r += 0.01f;
		c.g += 0.01f;
		c.b += 0.01f;
		
		return c;
	}
	
		public static bool FlagIsSet<T>(T flags, T flag)
			where T : struct
		{
			var flagsValue = (int)(object)flags;
			var flagValue  = (int)(object)flag;
			
			return (flagsValue & flagValue) != 0;
		}
	
	// The same as SendMessage, except it only sends the message to this MonoBehaviour
	public static void SendSingleMessage(Object target, string methodName, object methodArgument, SendMessageOptions methodOptions)
	{
		var method = target.GetType().GetMethod(methodName);
		if (method != null)
		{
			object[] methodArguments = null;
			
			if (methodArgument != null)
			{
				methodArguments = new object[]{ methodArgument };
			}
			
			method.Invoke(target, methodArguments);
		}
		else
		{
			if (methodOptions == SendMessageOptions.RequireReceiver)
			{
				Debug.LogError("SendSingleMessage " + methodName + " has no receiver!");
			}
		}
	}
	
	public static T GetOrAddComponent<T>(GameObject go)
		where T : Component
	{
		T component = null;
		
		if (go != null)
		{
			component = go.GetComponent<T>();
			
			if (component == null)
			{
				component = go.AddComponent<T>();
			}
		}
		
		return component;
	}
	
	public static T GetComponentUpwards<T>(GameObject current)
		where T : Component
	{
		if (current != null)
		{
			var component = current.GetComponent<T>();
			if (component != null)
			{
				return component;
			}
			
			var parent = current.transform.parent;
			if (parent != null)
			{
				return GetComponentUpwards<T>(parent.gameObject);
			}
		}
		
		return null;
	}
}
using UnityEngine;

[System.Serializable]
public class SGT_PackerOutput
{
	/*[SerializeField]*/
	private SGT_PackerInput input;
	
	[SerializeField]
	private Rect inputRect;
	
	[SerializeField]
	private Rect inputRectTrimmed;
	
	[SerializeField]
	private string outputName;
	
	[SerializeField]
	private int outputIndex;
	
	[SerializeField]
	private Texture2D outputTexture;
	
	[SerializeField]
	private int outputTextureId;
	
	[SerializeField]
	private bool outputRotated;
	
	[SerializeField]
	private Rect outputRect;
	
	[SerializeField]
	private int outputArea;
	
	[SerializeField]
	private Rect uv;
	
	[SerializeField]
	private Vector2 uvBottomLeft;
	
	[SerializeField]
	private Vector2 uvBottomRight;
	
	[SerializeField]
	private Vector2 uvTopLeft;
	
	[SerializeField]
	private Vector2 uvTopRight;
	
	[SerializeField]
	private Vector3 vertexBottomLeft;
	
	[SerializeField]
	private Vector3 vertexBottomRight;
	
	[SerializeField]
	private Vector3 vertexTopLeft;
	
	[SerializeField]
	private Vector3 vertexTopRight;
	
	[SerializeField]
	private object data;
	
	public SGT_PackerInput Input
	{
		set
		{
			input = value;
		}
		
		get
		{
			return input;
		}
	}
	
	public string OutputName
	{
		set
		{
			outputName = value;
		}
		
		get
		{
			return outputName;
		}
	}
	
	public int OutputIndex
	{
		set
		{
			outputIndex = value;
		}
		
		get
		{
			return outputIndex;
		}
	}
	
	public Texture2D OutputTexture
	{
		set
		{
			outputTexture = value;
		}
		
		get
		{
			return outputTexture;
		}
	}
	
	public int OutputTextureId
	{
		set
		{
			outputTextureId = value;
		}
		
		get
		{
			return outputTextureId;
		}
	}
	
	public bool OutputRotated
	{
		set
		{
			if (value != outputRotated)
			{
				var w = outputRect.width;
				var h = outputRect.height;
				
				outputRect.width  = h;
				outputRect.height = w;
				
				outputRotated = value;
			}
		}
		
		get
		{
			return outputRotated;
		}
	}
	
	public Rect InputRect
	{
		set
		{
			inputRect = value;
		}
		
		get
		{
			return inputRect;
		}
	}
	
	public Rect InputRectTrimmed
	{
		set
		{
			inputRectTrimmed = value;
		}
		
		get
		{
			return inputRectTrimmed;
		}
	}
	
	public Rect OutputRect
	{
		set
		{
			outputRect = value;
		}
		
		get
		{
			return outputRect;
		}
	}
	
	public int OutputArea
	{
		set
		{
			outputArea = value;
		}
		
		get
		{
			return outputArea;
		}
	}
	
	public Rect Uv
	{
		get
		{
			return uv;
		}
	}
	
	public Vector2 UvBottomLeft
	{
		get
		{
			return uvBottomLeft;
		}
	}
	
	public Vector2 UvBottomRight
	{
		get
		{
			return uvBottomRight;
		}
	}
	
	public Vector2 UvTopLeft
	{
		get
		{
			return uvTopLeft;
		}
	}
	
	public Vector2 UvTopRight
	{
		get
		{
			return uvTopRight;
		}
	}
	
	public Vector3 VertexBottomLeft
	{
		get
		{
			return vertexBottomLeft;
		}
	}
	
	public Vector3 VertexBottomRight
	{
		get
		{
			return vertexBottomRight;
		}
	}
	
	public Vector3 VertexTopLeft
	{
		get
		{
			return vertexTopLeft;
		}
	}
	
	public Vector3 VertexTopRight
	{
		get
		{
			return vertexTopRight;
		}
	}
	
	public object Data
	{
		set
		{
			data = value;
		}
		
		get
		{
			return data;
		}
	}
	
	public void PasteInto(Texture2D newOutputTexture)
	{
		outputTexture = newOutputTexture;
		
		// Write UV data
		var oX = outputRect.x      / (float)outputTexture.width;
		var oY = outputRect.y      / (float)outputTexture.height;
		var oW = outputRect.width  / (float)outputTexture.width;
		var oH = outputRect.height / (float)outputTexture.height;
		
		uv = new Rect(oX, oY, oW, oH);
		
		if (outputRotated == true)
		{
			uvBottomLeft  = new Vector2(oX     , oY + oH);
			uvBottomRight = new Vector2(oX     , oY     );
			uvTopLeft     = new Vector2(oX + oW, oY + oH);
			uvTopRight    = new Vector2(oX + oW, oY     );
		}
		else
		{
			uvBottomLeft  = new Vector2(oX     , oY     );
			uvBottomRight = new Vector2(oX + oW, oY     );
			uvTopLeft     = new Vector2(oX     , oY + oH);
			uvTopRight    = new Vector2(oX + oW, oY + oH);
		}
		
		var vX = (inputRectTrimmed.x - inputRect.x) / inputRect.width;
		var vY = (inputRectTrimmed.y - inputRect.y) / inputRect.height;
		var vW = inputRectTrimmed.width             / inputRect.width;
		var vH = inputRectTrimmed.height            / inputRect.height;
		
		vertexBottomLeft  = new Vector3(vX     , vY     , 0.0f);
		vertexBottomRight = new Vector3(vX + vW, vY     , 0.0f);
		vertexTopLeft     = new Vector3(vX     , vY + vH, 0.0f);
		vertexTopRight    = new Vector3(vX + vW, vY + vH, 0.0f);
		
		// Copy all pixels across
		var h  = (int)outputRect.height;
		var w  = (int)outputRect.width;
		var irX = (int)inputRectTrimmed.x;
		var irY = (int)inputRectTrimmed.y;
		var orX = (int)outputRect.x;
		var orY = (int)outputRect.y;
		
		for (var y = 0; y < h; y++)
		{
			for (var x = 0; x < w; x++)
			{
				Color sourceColour;
				
				if (outputRotated == true)
				{
					sourceColour = input.Texture.GetPixel(irX + y, irY + x);
				}
				else
				{
					sourceColour = input.Texture.GetPixel(irX + x, irY + y);
				}
				
				outputTexture.SetPixel(orX + x, orY + y, sourceColour);
			}
		}
	}
}
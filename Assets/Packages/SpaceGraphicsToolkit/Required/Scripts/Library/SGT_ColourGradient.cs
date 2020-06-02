using ColourNodeList = System.Collections.Generic.List<SGT_ColourGradient.ColourNode>;
using AlphaNodeList  = System.Collections.Generic.List<SGT_ColourGradient.AlphaNode>;

using UnityEngine;

public partial class SGT_ColourGradient
{
	[System.Serializable]
	public class AlphaNode
	{
		[SerializeField]
		private bool locked;
		
		[SerializeField]
		private float alpha;
		
		[SerializeField]
		private float position;
		
		public bool Locked
		{
			set
			{
				locked = value;
			}
			
			get
			{
				return locked;
			}
		}
		
		public float Alpha
		{
			get
			{
				return alpha;
			}
		}
		
		public float Position
		{
			get
			{
				return position;
			}
		}
		
		public void SetAlpha(SGT_ColourGradient parent, float newAlpha)
		{
			if (newAlpha != alpha)
			{
				alpha = newAlpha;
				
				parent.Modified = true;
			}
		}
		
		public void OffsetPosition(SGT_ColourGradient parent, float offset)
		{
			SetPosition(parent, position + offset);
		}
		
		public void SetPosition(SGT_ColourGradient parent, float newPosition)
		{
			newPosition = Mathf.Clamp01(newPosition);
			
			if (newPosition != position)
			{
				position = newPosition;
				
				parent.Modified = true;
			}
		}
	}
}

public partial class SGT_ColourGradient
{
	[System.Serializable]
	public class ColourNode
	{
		[SerializeField]
		private bool locked;
		
		[SerializeField]
		private Color colour;
		
		[SerializeField]
		private float position;
		
		public bool Locked
		{
			set
			{
				locked = value;
			}
			
			get
			{
				return locked;
			}
		}
		
		public Color Colour
		{
			get
			{
				return colour;
			}
		}
		
		public float Position
		{
			get
			{
				return position;
			}
		}
		
		public void SetColour(SGT_ColourGradient parent, Color newColour)
		{
			if (parent.HasAlpha == false)
			{
				newColour.a = 1.0f;
			}
			
			if (parent.IsGreyscale == true)
			{
				var g = colour.grayscale;
				colour = new Color(g, g, g, colour.a);
			}
			
			if (newColour != colour)
			{
				colour = newColour;
				
				parent.Modified = true;
			}
		}
		
		public void OffsetPosition(SGT_ColourGradient parent, float offset)
		{
			SetPosition(parent, position + offset);
		}
		
		public void SetPosition(SGT_ColourGradient parent, float newPosition)
		{
			newPosition = Mathf.Clamp01(newPosition);
			
			if (newPosition != position)
			{
				position = newPosition;
				
				parent.Modified = true;
			}
		}
	}
}

[System.Serializable]
public partial class SGT_ColourGradient
{
	[SerializeField]
	private bool modified = true;
	
	[SerializeField]
	private ColourNodeList colourNodes = new ColourNodeList();
	
	[SerializeField]
	private AlphaNodeList alphaNodes = new AlphaNodeList();
	
	[SerializeField]
	private bool isGreyscale;
	
	[SerializeField]
	private bool hasAlpha;
	
	[SerializeField]
	private bool sorted;
	
	public SGT_ColourGradient(bool newGreyscale, bool newAlpha)
	{
		isGreyscale = newGreyscale;
		hasAlpha     = newAlpha;
	}
	
	public ColourNode[] ColourNodes
	{
		get
		{
			return colourNodes.ToArray();
		}
	}
	
	public AlphaNode[] AlphaNodes
	{
		get
		{
			return alphaNodes.ToArray();
		}
	}
	
	public bool Modified
	{
		set
		{
			modified = value;
			sorted   = false;
		}
		
		get
		{
			return modified;
		}
	}
	
	public bool IsGreyscale
	{
		get
		{
			return isGreyscale;
		}
	}
	
	public bool HasAlpha
	{
		get
		{
			return hasAlpha;
		}
	}
	
	public ColourNode AddColourNode(Color colour, float position)
	{
		modified = true;
		
		var newNode = new ColourNode();
		newNode.SetColour(this, colour);
		newNode.SetPosition(this, position);
		
		colourNodes.Add(newNode);
		
		return newNode;
	}
	
	public AlphaNode AddAlphaNode(float alpha, float position)
	{
		modified = true;
		
		var newNode = new AlphaNode();
		newNode.SetAlpha(this, alpha);
		newNode.SetPosition(this, position);
		
		alphaNodes.Add(newNode);
		
		return newNode;
	}
	
	public void RemoveColourNode(ColourNode node)
	{
		modified = true;
		
		colourNodes.Remove(node);
	}
	
	public void RemoveAlphaNode(AlphaNode node)
	{
		modified = true;
		
		alphaNodes.Remove(node);
	}
	
	public ColourNode FindClosestColourNode(float position)
	{
		return SGT_Helper.BestListItem(colourNodes, (ColourNode item, ColourNode best) => Mathf.Abs(position - item.Position) < Mathf.Abs(position - best.Position));
	}
	
	public AlphaNode FindClosestAlphaNode(float position)
	{
		return SGT_Helper.BestListItem(alphaNodes, (AlphaNode item, AlphaNode best) => Mathf.Abs(position - item.Position) < Mathf.Abs(position - best.Position));
	}
	
	public void Sort()
	{
		if (sorted == false)
		{
			colourNodes.Sort((a, b) => a.Position.CompareTo(b.Position));
			alphaNodes.Sort((a, b) => a.Position.CompareTo(b.Position));
		}
	}
	
	private int FindSortedColourNodeBelow(float position)
	{
		var bestIndex = 0;
		
		for (var i = 1; i < colourNodes.Count; i++)
		{
			if (colourNodes[i].Position <= position)
			{
				bestIndex = i;
			}
			else
			{
				break;
			}
		}
		
		return bestIndex;
	}
	
	private int FindSortedAlphaNodeBelow(float position)
	{
		var bestIndex = 0;
		
		for (var i = 1; i < alphaNodes.Count; i++)
		{
			if (alphaNodes[i].Position <= position)
			{
				bestIndex = i;
			}
			else
			{
				break;
			}
		}
		
		return bestIndex;
	}
	
	public Color CalculateBlend(ColourNode a, float position, ColourNode b)
	{
		var colour = a.Colour;
		
		if (a != b)
		{
			var weight = (position - a.Position) / (b.Position - a.Position);
			colour = SGT_Helper.SmoothStep(a.Colour, b.Colour, weight);
		}
		
		return colour;
	}
	
	public float CalculateBlend(AlphaNode a, float position, AlphaNode b)
	{
		var alpha = a.Alpha;
		
		if (a != b)
		{
			var weight = (position - a.Position) / (b.Position - a.Position);
			alpha = Mathf.SmoothStep(a.Alpha, b.Alpha, weight);
		}
		
		return alpha;
	}
	
	public Color CalculateColour(float position)
	{
		Sort();
		
		position = Mathf.Clamp01(position);
		
		if (colourNodes.Count > 0)
		{
			var cNodeA = FindSortedColourNodeBelow(position);
			var cNodeB = Mathf.Min(cNodeA + 1, colourNodes.Count - 1);
			var colour = CalculateBlend(colourNodes[cNodeA], position, colourNodes[cNodeB]);
			
			if (hasAlpha == true)
			{
				if (alphaNodes.Count > 0)
				{
					var aNodeA = FindSortedAlphaNodeBelow(position);
					var aNodeB = Mathf.Min(aNodeA + 1, alphaNodes.Count - 1);
					var alpha  = CalculateBlend(alphaNodes[aNodeA], position, alphaNodes[aNodeB]);
					
					colour.a = alpha;
				}
			}
			
			return colour;
		}
		
		return default(Color);
	}
	
	public Color[] CalculateColours(float a, float b, int steps)
	{
		Sort();
		
		float step = (float)(b - a) / (float)steps;
		
		var colours = new Color[steps];
		
		if (colourNodes.Count > 0)
		{
			for (var i = 0; i < steps; i++)
			{
				a += step;
				
				colours[i] = CalculateColour(a);
			}
		}
		
		return colours;
	}
	
	public static Texture2D AllocateTexture(int x, int y = 1)
	{
		var texture = new Texture2D(x, y, TextureFormat.ARGB32, false);
		
		texture.wrapMode = TextureWrapMode.Clamp;
		
		return texture;
	}
}
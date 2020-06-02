using UnityEngine;

[System.Serializable]
public class SGT_PackerInput
{
	[SerializeField]
	private bool modified;
	
	[SerializeField]
	private string texturePath;
	
	/*[SerializeField]*/
	private Texture2D texture;
	
	[SerializeField]
	private string name;
	
	[SerializeField]
	private int index;
	
	[SerializeField]
	private SGT_PackerTrim trim;
	
	[SerializeField]
	private Color trimColour;
	
	[SerializeField]
	private SGT_PackerBorder border;
	
	[SerializeField]
	private SGT_PackerBorderSize borderSize;
	
	[SerializeField]
	private bool tilesheet;
	
	[SerializeField]
	private int tilesheetTilesX = 1;
	
	[SerializeField]
	private int tilesheetTilesY = 1;
	
	[SerializeField]
	private object data;
	
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
	
	public Texture2D Texture
	{
		set
		{
			if (value != texture)
			{
#if UNITY_EDITOR == true
				texturePath = SGT_Helper.FindAsset(value);
#endif
				texture  = value;
				modified = true;
			}
		}
		
		get
		{
#if UNITY_EDITOR == true
			if (texture == null && string.IsNullOrEmpty(texturePath) == false)
			{
				texture = SGT_Helper.LoadAsset<Texture2D>(texturePath);
			}
#endif
			return texture;
		}
	}
	
	public string Name
	{
		set
		{
			if (value != name)
			{
				name     = value;
				modified = true;
			}
		}
		
		get
		{
			return name;
		}
	}
	
	public SGT_PackerTrim Trim
	{
		set
		{
			if (value != trim)
			{
				trim     = value;
				modified = true;
			}
		}
		
		get
		{
			return trim;
		}
	}
	
	public Color TrimColour
	{
		set
		{
			if (value != trimColour)
			{
				trimColour = value;
				
				if (trim == SGT_PackerTrim.Colour)
				{
					modified = true;
				}
			}
		}
		
		get
		{
			return trimColour;
		}
	}
	
	public SGT_PackerBorder Border
	{
		set
		{
			if (value != border)
			{
				border   = value;
				modified = true;
			}
		}
		
		get
		{
			return border;
		}
	}
	
	public SGT_PackerBorderSize BorderSize
	{
		set
		{
			if (value != borderSize)
			{
				borderSize = value;
				modified   = true;
			}
		}
		
		get
		{
			return borderSize;
		}
	}
	
	public int Index
	{
		set
		{
			if (value != index)
			{
				index    = value;
				modified = true;
			}
		}
		
		get
		{
			return index;
		}
	}
	
	public bool Tilesheet
	{
		set
		{
			if (value != tilesheet)
			{
				tilesheet = value;
				modified  = true;
			}
		}
		
		get
		{
			return tilesheet;
		}
	}
	
	public int TilesheetTilesX
	{
		set
		{
			value = Mathf.Max(1, value);
			
			if (value != tilesheetTilesX)
			{
				tilesheetTilesX = value;
				modified        = true;
			}
		}
		
		get
		{
			return tilesheetTilesX;
		}
	}
	
	public int TilesheetTilesY
	{
		set
		{
			value = Mathf.Max(1, value);
			
			if (value != tilesheetTilesY)
			{
				tilesheetTilesY = value;
				modified        = true;
			}
		}
		
		get
		{
			return tilesheetTilesY;
		}
	}
	
	public object Data
	{
		set
		{
			if (value != data)
			{
				data     = value;
				modified = true;
			}
		}
		
		get
		{
			return data;
		}
	}
}
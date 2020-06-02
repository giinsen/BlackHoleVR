using InputList  = System.Collections.Generic.List<SGT_PackerInput>;
using BinList    = System.Collections.Generic.List<SGT_Internal.SGT_PackerBin>;
using OutputList = System.Collections.Generic.List<SGT_PackerOutput>;

using UnityEngine;
using SGT_Internal;

[System.Serializable]
public partial class SGT_Packer
{
	[SerializeField]
	private bool modified;
	
	[SerializeField]
	private InputList inputs;
	
	[SerializeField]
	private Texture2D[] atlases;
	
	[SerializeField]
	private TextureFormat atlasFormat = TextureFormat.ARGB32;
	
	[SerializeField]
	private bool atlasMipMaps = true;
	
	[SerializeField]
	private int atlasAnisoLevel = 1;
	
	[SerializeField]
	private FilterMode atlasFilterMode = FilterMode.Bilinear;
	
	[SerializeField]
	private int atlasCountMax;
	
	[SerializeField]
	private SGT_SquareSize atlasMaxSize = SGT_SquareSize.Square1024;
	
	[SerializeField]
	private OutputList outputs;
	
	public bool Modified
	{
		set
		{
			modified = value;
		}
		
		get
		{
			if (modified == false) CheckForModifications();
			
			return modified;
		}
	}
	
	public TextureFormat AtlasFormat
	{
		set
		{
			if (value != atlasFormat)
			{
				atlasFormat = value;
				modified    = true;
			}
		}
		
		get
		{
			return atlasFormat;
		}
	}
	
	public int AtlasAnisoLevel
	{
		set
		{
			value = Mathf.Clamp(value, 0, 9);
			
			if (value != atlasAnisoLevel)
			{
				atlasAnisoLevel = value;
				modified        = true;
			}
		}
		
		get
		{
			return atlasAnisoLevel;
		}
	}
	
	public bool AtlasMipMaps
	{
		set
		{
			if (value != atlasMipMaps)
			{
				atlasMipMaps = value;
				modified     = true;
			}
		}
		
		get
		{
			return atlasMipMaps;
		}
	}
	
	public FilterMode AtlasFilterMode
	{
		set
		{
			if (value != atlasFilterMode)
			{
				atlasFilterMode = value;
				modified        = true;
			}
		}
		
		get
		{
			return atlasFilterMode;
		}
	}
	
	public int InputCount
	{
		get
		{
			return inputs != null ? inputs.Count : 0;
		}
	}
	
	public int OutputCount
	{
		get
		{
			return outputs != null ? outputs.Count : 0;
		}
	}
	
	public int AtlasCount
	{
		get
		{
			return atlases != null ? atlases.Length : 0;
		}
	}
	
	public int AtlasCountMax
	{
		set
		{
			if (value != atlasCountMax)
			{
				atlasCountMax = value;
				modified      = true;
			}
		}
		
		get
		{
			return atlasCountMax;
		}
	}
	
	public SGT_SquareSize AtlasMaxSize
	{
		set
		{
			if (value != atlasMaxSize)
			{
				atlasMaxSize = value;
				modified     = true;
			}
		}
		
		get
		{
			return atlasMaxSize;
		}
	}
	
	public SGT_PackerResult Pack()
	{
		if (modified == false) CheckForModifications();
		
		if (modified == true)
		{
			atlases = SGT_Helper.DestroyObjects(atlases);
			outputs = Compile();
			
			// Can't pack nothing
			if (outputs.Count != 0)
			{
				// Sort into largest first
				outputs.Sort((a, b) => (int)b.OutputRect.width - (int)a.OutputRect.width);
				
				// Find base size
				var baseSize = Mathf.NextPowerOfTwo((int)outputs[0].OutputRect.width);
				
				// Largest texture is larger than maxTextureSize
				if (baseSize > (int)atlasMaxSize)
				{
					return SGT_PackerResult.AtlasTooSmall;
				}
				
				// Create first bin
				var bins = new BinList(1);
				
				bins.Add(new SGT_PackerBin(0, baseSize, (int)atlasMaxSize));
				
				// Pack all textures
				foreach (var output in outputs)
				{
					// Fits in existing bins?
					foreach (var bin in bins)
					{
						if (bin.Pack(output) == true)
						{
							goto FoundBin;
						}
					}
					
					// Create new bin?
					if (atlasCountMax <= 0 || bins.Count < atlasCountMax)
					{
						var newBin = new SGT_PackerBin(bins.Count, baseSize, (int)atlasMaxSize);
						
						if (newBin.Pack(output) == false)
						{
							return SGT_PackerResult.Unknown;
						}
						
						bins.Add(newBin);
					}
					// Too many bins?
					else
					{
						return SGT_PackerResult.NotEnoughAtlases;
					}
					
				FoundBin:
					continue;
				}
				
				// Build textures
				atlases = new Texture2D[bins.Count];
				
				for (var i = 0; i < bins.Count; i++)
				{
					var bin     = bins[i];
					var texture = new Texture2D(bin.Size, bin.Size, atlasFormat, atlasMipMaps);
					
					texture.anisoLevel = atlasAnisoLevel;
					texture.wrapMode   = TextureWrapMode.Clamp;
					texture.filterMode = atlasFilterMode;
					
					for (var y = 0; y < texture.width; y++)
					{
						for (var x = 0; x < texture.height; x++)
						{
							texture.SetPixel(x, y, Color.black);
						}
					}
					
					atlases[i] = texture;
				}
				
				// Paste textures
				foreach (var output in outputs)
				{
					var texture = atlases[output.OutputTextureId];
					
					output.PasteInto(texture);
				}
				
				// Apply pastes
				foreach (var texture in atlases)
				{
					texture.Apply();
				}
			}
			
			MarkAsUnmodified();
		}
		
		return SGT_PackerResult.Success;
	}
	
	public void AddInput(SGT_PackerInput input)
	{
		if (input != null)
		{
			if (inputs == null) inputs = new InputList();
			
			inputs.Add(input);
			
			modified = true;
		}
	}
	
	public void SetInput(SGT_PackerInput input, int index)
	{
		SGT_ArrayHelper.Set(inputs, input, index);
	}
	
	public SGT_PackerInput GetInput(int index)
	{
		return SGT_ArrayHelper.Index(inputs, index);
	}
	
	public void RemoveInput(int index)
	{
		if (SGT_ArrayHelper.Remove(inputs, index) == true)
		{
			modified = true;
		}
	}
	
	public void RemoveAllInputs()
	{
		if (inputs != null)
		{
			inputs.Clear();
			
			modified = true;
		}
	}
	
	public SGT_PackerOutput GetOutput(int index)
	{
		return SGT_ArrayHelper.Index(outputs, index);
	}
	
	public SGT_PackerOutput FindOutput(Vector2 uv)
	{
		var index = FindOutputIndex(uv);
		
		return index != -1 ? outputs[index] : null;
	}
	
	public int FindOutputIndex(Vector2 uv)
	{
		if (outputs != null)
		{
			for (var i = 0; i < outputs.Count; i++)
			{
				var output = outputs[i];
				
				if (output != null && output.Uv.Contains(uv) == true)
				{
					return i;
				}
			}
		}
		
		return -1;
	}
	
	public Texture2D GetAtlas(int index = 0)
	{
		return SGT_ArrayHelper.Index(atlases, index);
	}
	
	public void Duplicated()
	{
		outputs  = null;
		atlases  = null;
		modified = true;
	}
	
	public void OnDestroy()
	{
		SGT_Helper.DestroyObjects(atlases);
	}
	
	private void CheckForModifications()
	{
		if (modified == false && inputs != null)
		{
			foreach (var input in inputs)
			{
				if (input != null)
				{
					if (input.Modified == true)
					{
						modified = true;
						
						return;
					}
				}
			}
			
			if (atlases != null)
			{
				foreach (var atlas in atlases)
				{
					if (atlas == null)
					{
						modified = true;
						
						return;
					}
				}
			}
		}
	}
	
	private void MarkAsUnmodified()
	{
		modified = false;
		
		if (inputs != null)
		{
			foreach (var input in inputs)
			{
				if (input != null)
				{
					input.Modified = false;
				}
			}
		}
	}
	
	private OutputList Compile()
	{
		var outputs = new OutputList();
		
		if (inputs != null)
		{
			foreach (var input in inputs)
			{
				if (input != null)
				{
					if (input.Texture != null)
					{
						if (input.Tilesheet == true && input.TilesheetTilesX >= 0 && input.TilesheetTilesY >= 0)
						{
							var indexOffset = 0;
							var tileWidth   = input.Texture.width  / input.TilesheetTilesX;
							var tileHeight  = input.Texture.height / input.TilesheetTilesY;
							
							for (var y = 0; y < input.TilesheetTilesY; y++)
							{
								for (var x = 0; x < input.TilesheetTilesX; x++)
								{
									var output = new SGT_PackerOutput();
									
									output.InputRect   = new Rect(tileWidth * x, tileHeight * y, tileWidth, tileHeight);
									output.OutputIndex = input.Index + indexOffset;
									
									RotateAndTrim(output, input);
									
									outputs.Add(output);
									
									indexOffset += 1;
								}
							}
						}
						else
						{
							var output = new SGT_PackerOutput();
							
							output.InputRect   = new Rect(0, 0, input.Texture.width, input.Texture.height);
							output.OutputIndex = input.Index;
							
							RotateAndTrim(output, input);
							
							outputs.Add(output);
						}
					}
				}
			}
		}
		
		return outputs;
	}
	
	private void RotateAndTrim(SGT_PackerOutput o, SGT_PackerInput i)
	{
		o.Input      = i;
		o.Data       = i.Data;
		o.OutputName = i.Name;
		
		switch (i.Trim)
		{
			case SGT_PackerTrim.None:        o.InputRectTrimmed = o.InputRect; break;
			case SGT_PackerTrim.Transparent: o.InputRectTrimmed = TrimTransparent(i.Texture, o.InputRect); break;
			case SGT_PackerTrim.Colour:      o.InputRectTrimmed = TrimColour(i.Texture, o.InputRect, i.TrimColour); break;
			case SGT_PackerTrim.Auto:        o.InputRectTrimmed = TrimAuto(i.Texture, o.InputRect); break;
		}
		
		var borderSize = new Vector2(o.InputRectTrimmed.width, o.InputRectTrimmed.height);
		
		switch (i.BorderSize)
		{
			case SGT_PackerBorderSize.None:          borderSize = Vector2.zero;      break;
			case SGT_PackerBorderSize.OnePixel:      borderSize = Vector2.one;       break;
			case SGT_PackerBorderSize.TenPercent:    borderSize = borderSize * 0.1f; break;
			case SGT_PackerBorderSize.TwentyPercent: borderSize = borderSize * 0.2f; break;
			case SGT_PackerBorderSize.ThirtyPercent: borderSize = borderSize * 0.3f; break;
			case SGT_PackerBorderSize.FourtyPercent: borderSize = borderSize * 0.4f; break;
			case SGT_PackerBorderSize.FiftyPercent:  borderSize = borderSize * 0.5f; break;
		}
		
		borderSize.x = Mathf.Floor(borderSize.x);
		borderSize.y = Mathf.Floor(borderSize.y);
		
		o.OutputRect    = new Rect(borderSize.x, borderSize.y, o.InputRectTrimmed.width + borderSize.x * 2.0f, o.InputRectTrimmed.height + borderSize.y * 2.0f); // This will be overwritten when pasted
		o.OutputArea    = (int)o.InputRectTrimmed.width * (int)o.InputRectTrimmed.height;
		o.OutputRotated = o.InputRectTrimmed.height > o.InputRectTrimmed.width;
	}
	
	private Rect TrimTransparent(Texture2D t, Rect r)
	{
		for (var y = r.yMin    ; y <  r.yMax; y++) { if (RowTransparent(t, r, y) == true) { r.yMin += 1; } else { break; } }
		for (var y = r.yMax - 1; y >= r.yMin; y--) { if (RowTransparent(t, r, y) == true) { r.yMax -= 1; } else { break; } }
		
		for (var x = r.xMin    ; x <  r.xMax; x++) { if (ColumnTransparent(t, r, x) == true) { r.xMin += 1; } else { break; } }
		for (var x = r.xMax - 1; x >= r.xMin; x--) { if (ColumnTransparent(t, r, x) == true) { r.xMax -= 1; } else { break; } }
		
		return r;
	}
	
	private Rect TrimColour(Texture2D t, Rect r, Color c)
	{
		// TODO
		return r;
	}
	
	private Rect TrimAuto(Texture2D t, Rect r)
	{
		// TODO
		return r;
	}
	
	private bool RowTransparent(Texture2D t, Rect r, float y)
	{
		for (var x = r.xMin; x < r.xMax; x++) { if (t.GetPixel((int)x, (int)y).a > 0.0f) { return false; } }
		
		return true;
	}
	
	private bool ColumnTransparent(Texture2D t, Rect r, float x)
	{
		for (var y = r.yMin; y < r.yMax; y++) { if (t.GetPixel((int)x, (int)y).a > 0.0f) { return false; } }
		
		return true;
	}
}
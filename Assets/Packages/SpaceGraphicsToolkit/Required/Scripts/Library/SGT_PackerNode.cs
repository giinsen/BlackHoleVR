using UnityEngine;

namespace SGT_Internal
{
	public class SGT_PackerNode
	{
		private int              binId;
		private int              x;
		private int              y;
		private int              w;
		private int              h;
		private SGT_PackerNode[] nodes;
		
		public SGT_PackerNode(int newBinId, int newX, int newY, int newW, int newH)
		{
			binId = newBinId;
			x     = newX;
			y     = newY;
			w     = newW;
			h     = newH;
		}
		
		public bool Pack(SGT_PackerOutput po)
		{
			// Fits into a child?
			if (nodes != null)
			{
				foreach (var node in nodes)
				{
					if (node.Pack(po) == true)
					{
						return true;
					}
				}
			}
			// Fits into this node?
			else if (po.OutputRect.width <= w && po.OutputRect.height <= h)
			{
				if (po.OutputRect.width == w)
				{
					// Perfect fit?
					if (po.OutputRect.height == h)
					{
						nodes = new SGT_PackerNode[0];
					}
					// Just top?
					else
					{
						nodes = new SGT_PackerNode[1];
						nodes[0] = new SGT_PackerNode(binId, x, y + (int)po.OutputRect.height, w, h - (int)po.OutputRect.height);
					}
				}
				// Both?
				else
				{
					nodes = new SGT_PackerNode[2];
					nodes[0] = new SGT_PackerNode(binId, x, y + (int)po.OutputRect.height, w, h - (int)po.OutputRect.height);
					nodes[1] = new SGT_PackerNode(binId, x + (int)po.OutputRect.width, y, w - (int)po.OutputRect.width, (int)po.OutputRect.height);
				}
				
				po.OutputTextureId = binId;
				po.OutputRect      = new Rect(x + po.OutputRect.x, y + po.OutputRect.y, po.InputRectTrimmed.width, po.InputRectTrimmed.height);
				
				return true;
			}
			
			return false;
		}
	}
}
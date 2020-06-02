namespace SGT_Internal
{
	using NodeList = System.Collections.Generic.List<SGT_PackerNode>;
	
	using UnityEngine;
	
	public class SGT_PackerBin
	{
		private int      id;
		private int      size;
		private int      maxSize;
		private NodeList nodes = new NodeList();
		
		public int Size
		{
			get
			{
				return size;
			}
		}
		
		public SGT_PackerBin(int newId, int newSize, int newMaxSize)
		{
			id      = newId;
			size    = newSize;
			maxSize = newMaxSize;
			
			nodes.Add(new SGT_PackerNode(id, 0, 0, newSize, newSize));
		}
		
		public bool Pack(SGT_PackerOutput po)
		{
			// Fits into an existing node?
			foreach (var node in nodes)
			{
				if (node.Pack(po) == true)
				{
					return true;
				}
			}
			
			// Expand?
			if (size < maxSize)
			{
				var n0 = new SGT_PackerNode(id, 0, size, size * 2, size);
				var n1 = new SGT_PackerNode(id, size, 0, size, size);
				
				if (n1.Pack(po) == false)
				{
					return false;
				}
				
				nodes.Add(n0);
				nodes.Add(n1);
				
				size *= 2;
				
				return true;
			}
			// Can't expand
			else
			{
				return false;
			}
		}
	}
}
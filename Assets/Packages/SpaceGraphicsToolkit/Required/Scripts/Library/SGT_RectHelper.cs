using UnityEngine;

public static class SGT_RectHelper
{
	public static void HorizontalSeparatePx(ref Rect left, ref Rect right, float px)
	{
		var halfPx = px * 0.5f;
		
		left.xMax  -= halfPx;
		right.xMin += halfPx;
	}
	
	public static Rect[] HorizontalSlice(Rect source, int count, float space)
	{
		count = Mathf.Max(0, count);
		
		var o = new Rect[count];
		
		if (count > 0)
		{
			var width = source.width - space * (count - 1);
			var size  = width / count;
			var x     = 0.0f;
			
			for (var i = 0; i < count; i++)
			{
				var r = source;
				
				r.width = size;
				r.x    += x;
				
				o[i] = r;
				
				x += size + space;
			}
		}
		
		return o;
	}
	
	public static Rect HorizontalSlice(Rect source, float min, float max)
	{
		var output = new Rect(source);
		output.xMax = output.xMin + source.width * max;
		output.xMin = output.xMin + source.width * min;
		return output;
	}
	
	public static Rect GetLeftPx(Rect output, float px)
	{
		output.xMax = output.xMin + px;
		return output;
	}
	
	public static Rect GetRightPx(Rect output, float px)
	{
		output.xMin = output.xMax - px;
		return output;
	}
	
	public static Rect GetBottomPx(Rect output, float px)
	{
		output.yMax = output.yMin + px;
		return output;
	}
	
	public static Rect GetTopPx(Rect output, float px)
	{
		output.yMin = output.yMax - px;
		return output;
	}
	
	public static Rect GetLeftPx(ref Rect source, float px)
	{
		var output = new Rect(source);
		output.xMax = output.xMin + px;
		source.xMin = output.xMax;
		return output;
	}
	
	public static Rect GetRightPx(ref Rect source, float px)
	{
		var output = new Rect(source);
		output.xMin = output.xMax - px;
		source.xMax = output.xMin;
		return output;
	}
	
	public static Rect GetBottomPx(ref Rect source, float px)
	{
		var output = new Rect(source);
		output.yMax = output.yMin + px;
		source.yMin = output.yMax;
		return output;
	}
	
	public static Rect GetTopPx(ref Rect source, float px)
	{
		var output = new Rect(source);
		output.yMin = output.yMax - px;
		source.yMax = output.yMin;
		return output;
	}
	
	public static Rect GetPx(Rect output, float left, float right, float top, float bottom)
	{
		output = GetLeftPx(output, left);
		output = GetRightPx(output, right);
		output = GetBottomPx(output, bottom);
		output = GetTopPx(output, top);
		return output;
	}
	
	public static Rect RemoveLeftPx(Rect output, float px)
	{
		output.xMin += px;
		return output;
	}
	
	public static Rect RemoveRightPx(Rect output, float px)
	{
		output.xMax -= px;
		return output;
	}
	
	public static Rect RemoveBottomPx(Rect output, float px)
	{
		output.yMin += px;
		return output;
	}
	
	public static Rect RemoveTopPx(Rect output, float px)
	{
		output.yMax -= px;
		return output;
	}
	
	public static Rect RemoveLeftPx(ref Rect source, float px)
	{
		var output = new Rect(source);
		output.xMin += px;
		source.xMax = output.xMin;
		return output;
	}
	
	public static Rect RemoveRightPx(ref Rect source, float px)
	{
		var output = new Rect(source);
		output.xMax -= px;
		source.xMin = output.xMax;
		return output;
	}
	
	public static Rect RemoveBottomPx(ref Rect source, float px)
	{
		var output = new Rect(source);
		output.yMin += px;
		source.yMax = output.yMin;
		return output;
	}
	
	public static Rect RemoveTopPx(ref Rect source, float px)
	{
		var output = new Rect(source);
		output.yMax -= px;
		source.yMin = output.yMax;
		return output;
	}
	
	public static Rect RemovePx(Rect output, float left, float right, float top, float bottom)
	{
		output = RemoveLeftPx(output, left);
		output = RemoveRightPx(output, right);
		output = RemoveBottomPx(output, bottom);
		output = RemoveTopPx(output, top);
		return output;
	}
	
	public static Rect RemovePx(Rect output, float px)
	{
		output = RemoveLeftPx(output, px);
		output = RemoveRightPx(output, px);
		output = RemoveBottomPx(output, px);
		output = RemoveTopPx(output, px);
		return output;
	}
	
	public static Rect ExpandLeftPx(Rect output, float px)
	{
		output.xMin -= px;
		return output;
	}
	
	public static Rect ExpandRightPx(Rect output, float px)
	{
		output.xMax += px;
		return output;
	}
	
	public static Rect ExpandBottomPx(Rect output, float px)
	{
		output.yMin -= px;
		return output;
	}
	
	public static Rect ExpandTopPx(Rect output, float px)
	{
		output.yMax += px;
		return output;
	}
	
	public static Rect ExpandPx(Rect output, float left, float right, float top, float bottom)
	{
		output = ExpandLeftPx(output, left);
		output = ExpandRightPx(output, right);
		output = ExpandBottomPx(output, bottom);
		output = ExpandTopPx(output, top);
		return output;
	}
	
	public static Rect ExpandPx(Rect output, float px)
	{
		output = ExpandLeftPx(output, px);
		output = ExpandRightPx(output, px);
		output = ExpandBottomPx(output, px);
		output = ExpandTopPx(output, px);
		return output;
	}
}
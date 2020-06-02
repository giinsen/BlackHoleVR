using ObjectList = System.Collections.Generic.List<UnityEngine.Object>;

using UnityEngine;

public abstract class SGT_MonoBehaviour : MonoBehaviour
{
	public float UniformScale
	{
		get
		{
			var scale = transform.lossyScale;
			
			return (scale.x + scale.y + scale.z) / 3.0f;
		}
	}
	
	public new void SendMessage(string methodName, SendMessageOptions options)
	{
		base.SendMessage(methodName, options);
	}
	
	public new void SendMessage(string methodName, object value, SendMessageOptions options)
	{
		if (value != null)
		{
			base.SendMessage(methodName, value, options);
		}
	}
	
	public new void SendMessage(string methodName, object value)
	{
		if (value != null)
		{
			base.SendMessage(methodName, value);
		}
	}
	
	public virtual void BuildUndoTargets(ObjectList list)
	{
		list.Add(this);
	}
}
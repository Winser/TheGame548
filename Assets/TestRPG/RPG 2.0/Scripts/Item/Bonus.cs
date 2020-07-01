using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;

[System.Serializable]
public class Bonus:ISerializable {
	public int minValue;
	public int maxValue;
	public string attribute;
	public Color color=Color.white;
	public int bonusValue;
	
	public Bonus(){}
	
	public Bonus (SerializationInfo info, StreamingContext ctxt)
	{
		minValue=(int)info.GetValue("MinValue",typeof(int));
		maxValue=(int)info.GetValue("MaxValue",typeof(int));
		attribute=(string)info.GetValue("Attribute",typeof(string));
		color=new Color((float)info.GetValue("R",typeof(float)),(float)info.GetValue("G",typeof(float)),(float)info.GetValue("B",typeof(float)),(float)info.GetValue("A",typeof(float)));
		bonusValue=(int)info.GetValue("BonusValue",typeof(int));
	}
	
	public void GetObjectData (SerializationInfo info, StreamingContext ctxt)
	{
		info.AddValue ("BonusValue", bonusValue);
		info.AddValue("MinValue",minValue);
		info.AddValue("MaxValue",maxValue);
		info.AddValue("Attribute",attribute);
		info.AddValue("R",color.r);
		info.AddValue("G",color.g);
		info.AddValue("B",color.b);
		info.AddValue("A",color.a);
	}
}

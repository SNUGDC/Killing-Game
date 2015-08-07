using UnityEngine;
using System.Collections.Generic;
using System;

public class SerializableDictionary<TK, TV> : ISerializationCallbackReceiver
{
    public Dictionary<TK, TV> MyDictionary;
    [SerializeField] List<TK> _keys;
    [SerializeField] List<TV> _values;
 
	public void OnBeforeSerialize()
	{
		_keys.Clear();
		_values.Clear();
		foreach(var kvp in MyDictionary)
		{
			_keys.Add(kvp.Key);
			_values.Add(kvp.Value);
		}
	}
	
	public void OnAfterDeserialize()
	{
		MyDictionary = new Dictionary<TK, TV>();
		for (int i=0; i!= Math.Min(_keys.Count,_values.Count); i++)
			MyDictionary.Add(_keys[i],_values[i]);
	}
	
	public SerializableDictionary()
	{
		this.MyDictionary = new Dictionary<TK, TV>();
		this._keys = new List<TK>();
		this._values = new List<TV>();
	}
}
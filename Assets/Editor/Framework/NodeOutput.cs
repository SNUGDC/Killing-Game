using UnityEngine;
using System;
using System.Collections.Generic;

public class NodeOutput : ScriptableObject
{
	public Node body;
	public Rect rect = new Rect ();
	public List<NodeInput> connections = new List<NodeInput> ();
	public string type;
	public string nodeID;
	[NonSerialized]
	public object value = null;

	/// <summary>
	/// Creates a new NodeOutput in NodeBody of specified type
	/// </summary>
	public static NodeOutput Create (Node NodeBody, string OutputName, Type OutputType) 
	{
		NodeOutput output = NodeOutput.CreateInstance (typeof (NodeOutput)) as NodeOutput;
		output.body = NodeBody;
		output.type = OutputType.AssemblyQualifiedName;
		output.name = OutputName;
		NodeBody.Outputs.Add (output);
		return output;
	}

	/// <summary>
	/// Function to automatically draw and update the output with a label for it's name
	/// </summary>
	public void DisplayLayout () 
	{
		DisplayLayout (new GUIContent (name));
	}
	/// <summary>
	/// Function to automatically draw and update the output
	/// </summary>
	public void DisplayLayout (GUIContent content) 
	{
		GUIStyle style = new GUIStyle (UnityEditor.EditorStyles.label);
		GUILayout.Label (content, style);
		if (Event.current.type == EventType.Repaint) 
			SetRect (GUILayoutUtility.GetLastRect ());
	}
	
	/// <summary>
	/// Set the output rect as labelrect in global canvas space and extend it to the right node edge
	/// </summary>
	public void SetRect (Rect labelRect) 
	{
		rect = new Rect (body.rect.x, 
		                 body.rect.y + labelRect.y,
		                 labelRect.width + labelRect.x,
		                 labelRect.height);
	}
	
	/// <summary>
	/// Get the rect of the knob left to the input
	/// </summary>
	public Rect GetKnob () 
	{
		int knobSize = Node_Editor.editor.knobSize;
		return new Rect (rect.x - knobSize,
		                 rect.y + (rect.height - knobSize) / 2, 
		                 knobSize, knobSize);
	}
}
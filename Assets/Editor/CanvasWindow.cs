using UnityEngine;
using UnityEditor;
using System;
 
public class CanvasWindow : EditorWindow {
 
    [MenuItem("Canvas/Show")]
    public static void ShowWindow()
    {
        CanvasWindow window = (CanvasWindow)EditorWindow.GetWindow(typeof(CanvasWindow));
    }
 
    Texture2D mytexture;
    Color penColor;
    int penSize;
    readonly Rect texSize = new Rect(0, 0, 300, 400);
    Rect windowRect = new Rect(3, 3, 100, 400);
 
 
    void OnGUI()
    {
        if (mytexture == null)
            mytexture = new Texture2D((int)texSize.width, (int)texSize.height, TextureFormat.RGBA32, false);
 
        Rect canvasPos = new Rect((position.width - texSize.width) / 2, (position.height - texSize.height) / 2, texSize.width, texSize.height);
        
        EditorGUI.DrawPreviewTexture(canvasPos, mytexture);
        penColor = EditorGUILayout.ColorField("Pen Color", penColor);
        penSize = EditorGUILayout.IntSlider("Pen Size", penSize, 1, 10);
        
        BeginWindows();
        windowRect = GUILayout.Window(1, windowRect, DoWindow, "Palette");
        EndWindows();
        
        if (Event.current.type == EventType.MouseDrag)
        {
            int bx = (int)(Event.current.mousePosition.x - canvasPos.x);
            int by = (int)(texSize.height - (Event.current.mousePosition.y - canvasPos.y));
                
            for (int x = 0; x < penSize; ++x)
            {
                for (int y = 0; y < penSize; ++y )
                    mytexture.SetPixel(bx - penSize / 2 + x, by - penSize / 2 + y, penColor);
            }
 
            mytexture.Apply(false);
            Repaint();
        }
    }
 
    enum DrawMode
    {
        point,
        line,
        rectangle,
        ellipse,
    }
 
    DrawMode drawmode;
    void DoWindow(int id)
    {
        EditorGUILayout.BeginVertical();
 
        drawmode = (DrawMode)GUILayout.SelectionGrid((int)drawmode, Enum.GetNames(typeof(DrawMode)), 1);
 
        if (GUILayout.Button("clear"))
        {
            mytexture = new Texture2D((int)texSize.width, (int)texSize.height, TextureFormat.RGBA32, false);
        }
        EditorGUILayout.EndVertical();
 
        GUI.DragWindow();
    }
}
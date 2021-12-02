using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TemplateWindow : EditorWindow
{
    [MenuItem("Tools/TemplateWindow")]
    static void CreateWindow()
    {
        //utility: false:可以合并有遮挡的窗口 true:置顶的窗口
        TemplateWindow templateWindow = (TemplateWindow)GetWindow(typeof(TemplateWindow), true, "模板窗口", false);
        //templateWindow.titleContent = new GUIContent("模板窗口");
        templateWindow.position = new Rect(Screen.width / 2 - 300, Screen.height / 2 - 200, 600, 400);
        templateWindow.Show();
    }

    void OnGUI()
    {
        GUI.Button(new Rect(5, 5, 100, 30), "Button");
        GUI.Box(new Rect(5, 40, 100, 30), "Box", new GUIStyle("flow node 4"));
        GUI.Label(new Rect(5, 110, 100, 30), "Label");

    }
}

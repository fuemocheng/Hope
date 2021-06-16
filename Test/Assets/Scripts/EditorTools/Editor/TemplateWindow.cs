using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TemplateWindow : EditorWindow
{
    [MenuItem("Tools/TemplateWindow")]
    static void CreateWindow()
    {
        //utility: false:���Ժϲ����ڵ��Ĵ��� true:�ö��Ĵ���
        TemplateWindow templateWindow = (TemplateWindow)GetWindow(typeof(TemplateWindow), true, "ģ�崰��", false);
        //templateWindow.titleContent = new GUIContent("ģ�崰��");
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

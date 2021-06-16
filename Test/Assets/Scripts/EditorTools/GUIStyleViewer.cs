using UnityEditor;
using UnityEngine;

public class GUIStyleViewer : EditorWindow
{

    Vector2 _scrollPosition = new Vector2(0, 0);
    GUIStyle _textStyle;
    string _searchStr = "";
    
    static GUIStyleViewer window;

    [MenuItem("Tools/GUIStyleViewer")]
    static void Create()
    {
        window = GetWindow<GUIStyleViewer>("GUIStyleViewer");
        window.position = new Rect(Screen.width / 2 - 330, Screen.height / 2 - 400, 660, 800);
    }

    void OnGUI()
    {
        if(_textStyle == null)
        {
            _textStyle = new GUIStyle("HeaderLabel");
            _textStyle.fontSize = 20;
        }

        GUILayout.BeginHorizontal("HelpBox");
        GUILayout.Label("������£�", _textStyle);
        GUILayout.FlexibleSpace();                  //�������Ҷ���
        GUILayout.Label("Search");
        _searchStr = EditorGUILayout.TextField(_searchStr);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal("PopupCurveSwatchBackground");
        GUILayout.Label("��ʽչʾ", _textStyle, GUILayout.Width(300));
        GUILayout.FlexibleSpace();
        GUILayout.Label("����", _textStyle, GUILayout.Width(300));
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
        foreach(GUIStyle style in GUI.skin.customStyles)
        {
            if (style.name.ToLower().Contains(_searchStr.ToLower()))
            {
                GUILayout.Space(15);
                GUILayout.BeginHorizontal("PopupCurveSwatchBackground");
                if (GUILayout.Button(style.name, style, GUILayout.Width(300)))
                {
                    //���Ƶ����а�
                    EditorGUIUtility.systemCopyBuffer = style.name;
                    Debug.LogError(style.name);
                }
                GUILayout.FlexibleSpace();              //�������Ҷ���
                EditorGUILayout.SelectableLabel(style.name, GUILayout.Width(300));
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.EndScrollView();
    }
}

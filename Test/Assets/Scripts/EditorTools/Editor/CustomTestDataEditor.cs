using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.UI;

[CustomEditor(typeof(CustomTestData))]
public class CustomTestDataEditor : Editor
{
    /// <summary>
    /// ReorderableList built-in tool to visualize lists in IDE
    /// </summary>
    private ReorderableList _reorderableList;
    private SerializedProperty _seriListData;

    private void OnEnable()
    {
        _seriListData = serializedObject.FindProperty("listData");
        _reorderableList = new ReorderableList(serializedObject, _seriListData);
        _reorderableList.elementHeight = 100;
        _reorderableList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "界面属性");
        _reorderableList.drawElementCallback = (rect, index, isActive, isFocused) =>
        {
            var drawRect = rect;
            var element = _seriListData.GetArrayElementAtIndex(index);
            var customComType = element.FindPropertyRelative("customComType");
            var texture = element.FindPropertyRelative("texture");
            var button = element.FindPropertyRelative("button");
            var gameObj = element.FindPropertyRelative("gameObject");
            var intValue = element.FindPropertyRelative("intValue");
            var strValue = element.FindPropertyRelative("strValue");

            //默认lable宽度
            EditorGUIUtility.labelWidth = 60;

            //texture
            var textureRect = new Rect(rect)
            {
                width = 68,
                height = 68,
            };
            texture.objectReferenceValue = EditorGUI.ObjectField(textureRect, texture.objectReferenceValue, typeof(Texture), false);

            //customComType
            var customComTypeRect = new Rect(rect)
            {
                x = rect.x + textureRect.width + 10,
                width = rect.width - textureRect.width - 10,
                height = rect.height / 5f - 1,
            };
            EditorGUI.PropertyField(customComTypeRect, customComType, new GUIContent("ComType"));

            //Button
            var buttonLableRect = new Rect(rect)
            {
                x = rect.x + textureRect.width + 10,
                y = rect.y + rect.height * 1 / 5f,
                width = EditorGUIUtility.labelWidth,
                height = rect.height / 5f - 1,
            };
            EditorGUI.LabelField(buttonLableRect, new GUIContent("Button"));
            var buttonRect = new Rect(buttonLableRect)
            {
                x = buttonLableRect.x + buttonLableRect.width,
                width = rect.width - textureRect.width - buttonLableRect.width - 10,
            };
            button.objectReferenceValue = EditorGUI.ObjectField(buttonRect, button.objectReferenceValue, typeof(Button), false);

            //GameObject
            var gameObjectLableRect = new Rect(buttonLableRect)
            {
                y = rect.y + rect.height * 2 / 5f,
            };
            EditorGUI.LabelField(gameObjectLableRect, new GUIContent("Prefab"));
            var gameObjectRect = new Rect(gameObjectLableRect)
            {
                x = buttonRect.x,
                width = rect.width - textureRect.width - buttonLableRect.width - 10,
            };
            gameObj.objectReferenceValue = EditorGUI.ObjectField(gameObjectRect, gameObj.objectReferenceValue, typeof(GameObject), false);

            //intValue
            var intValueRect = new Rect(customComTypeRect)
            {
                y = rect.y + rect.height * 3 / 5f,
            };
            EditorGUI.IntSlider(intValueRect, intValue, 0, 100, intValue.displayName);

            //strValue
            var strValueRect = new Rect(intValueRect)
            {
                y = rect.y + rect.height * 4 / 5f,
            };
            EditorGUI.PropertyField(strValueRect, strValue, new GUIContent("string"));
        };

        _reorderableList.drawElementBackgroundCallback = (rect, index, isActive, isFocused) =>
        {
            GUI.backgroundColor = Color.yellow;
        };

        _reorderableList.onAddCallback = (list) =>
        {
            _seriListData.InsertArrayElementAtIndex(_seriListData.arraySize);
            var element = _seriListData.GetArrayElementAtIndex(_seriListData.arraySize - 1);
            var customComType = element.FindPropertyRelative("customComType");
            var texture = element.FindPropertyRelative("texture");
            var button = element.FindPropertyRelative("button");
            var strValue = element.FindPropertyRelative("strValue");

            customComType.enumValueIndex = (int)ECustomComType.GameObject;
            texture.objectReferenceValue = null;
            button.objectReferenceValue = null;
            strValue.stringValue = "";
        };

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        _reorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

}

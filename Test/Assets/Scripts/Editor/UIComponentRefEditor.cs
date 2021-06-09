using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(UIComponentRef))]
public class UIComponentRefEditor : Editor
{
    ReorderableList _reorderableList;
    SerializedProperty _itemInfoListProp;

    void OnEnable()
    {
        InitList();
    }

    void InitList()
    {
        _itemInfoListProp = serializedObject.FindProperty("itemInfoList");
        _reorderableList = new ReorderableList(serializedObject, _itemInfoListProp)
        {
            drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "字段列表"),
            drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                var drawRect = rect;
                var elementProp = _itemInfoListProp.GetArrayElementAtIndex(index);
                var fieldName = elementProp.FindPropertyRelative("fieldName");
                var typeName = elementProp.FindPropertyRelative("typeName");
                var gameObject = elementProp.FindPropertyRelative("gameObject");
                var typeIndex = 0;

                drawRect.width = rect.width * 0.32f;
                EditorGUI.PropertyField(drawRect, gameObject, new GUIContent());

                drawRect.y += 2;
                drawRect.x += drawRect.width + rect.width * 0.01f;
                drawRect.width = rect.width * 0.33f;
                if (gameObject.objectReferenceValue)
                {
                    var components = (gameObject.objectReferenceValue as GameObject).GetComponents<Component>();
                    if (typeName.stringValue == "GameObject")
                        typeIndex = 0;
                    else if (components.Where(x => x.GetType().Name == typeName.stringValue).Count() > 0)
                        typeIndex = components.ToList().FindIndex(x => x.GetType().Name == typeName.stringValue) + 1;
                    else
                        typeIndex = CheckPriorType(components);

                    var selection = components.Select(x => x.GetType().Name).ToList();
                    selection.Insert(0, "GameObject");

                    typeIndex = EditorGUI.Popup(drawRect, typeIndex, selection.ToArray());
                    typeName.stringValue = typeIndex > 0 ? components[typeIndex - 1].GetType().Name : "GameObject";
                }
                else
                    typeName.stringValue = string.Empty;

                drawRect.y -= 2;
                drawRect.x += drawRect.width + rect.width * 0.01f;
                drawRect.width = rect.width * 0.33f;

                if (string.IsNullOrEmpty(fieldName.stringValue))
                {
                    if (gameObject.objectReferenceValue)
                        fieldName.stringValue = gameObject.objectReferenceValue.name;
                }

                EditorGUI.PropertyField(drawRect, fieldName, new GUIContent(string.Empty, "字段名"));
            },
            onAddCallback = (list) =>
            {
                _itemInfoListProp.InsertArrayElementAtIndex(_itemInfoListProp.arraySize);
                var elementProp = _itemInfoListProp.GetArrayElementAtIndex(_itemInfoListProp.arraySize - 1);
                var fieldName = elementProp.FindPropertyRelative("fieldName");
                var typeName = elementProp.FindPropertyRelative("typeName");
                var gameObject = elementProp.FindPropertyRelative("gameObject");
                gameObject.objectReferenceValue = null;
                fieldName.stringValue = string.Empty;
                typeName.stringValue = "GameObject";
            },
        };
    }

    int CheckPriorType(Component[] components)
    {
        var componentList = components.ToList();
        var index = componentList.FindIndex(x => x is Button);
        if (index > -1) return index + 1;
        index = componentList.FindIndex(x => x is InputField);
        if (index > -1) return index + 1;
        index = componentList.FindIndex(x => x is Slider);
        if (index > -1) return index + 1;
        index = componentList.FindIndex(x => x is ScrollRect);
        if (index > -1) return index + 1;
        index = componentList.FindIndex(x => x is Image);
        if (index > -1) return index + 1;
        index = componentList.FindIndex(x => x is Text);
        if (index > -1) return index + 1;
        index = componentList.FindIndex(x => x is CanvasGroup);
        if (index > -1) return index + 1;
        index = componentList.FindIndex(x => x is GraphicRaycaster);
        if (index > -1) return index + 1;
        //index = componentList.FindIndex(x => x is MultiObject);
        //if (index > -1) return index + 1;
        //index = componentList.FindIndex(x => x is MultiImage);
        //if (index > -1) return index + 1;
        //index = componentList.FindIndex(x => x is MultiColor);
        //if (index > -1) return index + 1;

        return 0;
    }

    public override void OnInspectorGUI()
    {
        var root = serializedObject.FindProperty("root");
        var rootObject = root.FindPropertyRelative("gameObject");
        var rootTypeName = root.FindPropertyRelative("typeName");
        EditorGUILayout.PropertyField(root,
            new GUIContent("UIGameObject Unit",
            "如果填入了一个GameObject，则此GameObject成为UIGameObject，并且下面的字段均属于此UIGameObject，否则字段属于当前GameObject的UIGameObject"));

        if (rootObject.objectReferenceValue && string.IsNullOrEmpty(rootTypeName.stringValue))
            rootTypeName.stringValue = rootObject.objectReferenceValue.name;

        EditorGUILayout.Space();
        _reorderableList.DoLayoutList();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("设置字段名为默认值"))
        {
            for (int i = 0; i < _itemInfoListProp.arraySize; i++)
            {
                var elementProp = _itemInfoListProp.GetArrayElementAtIndex(i);
                var gameObject = elementProp.FindPropertyRelative("gameObject");
                var fieldName = elementProp.FindPropertyRelative("fieldName");
                if (gameObject.objectReferenceValue)
                    fieldName.stringValue = gameObject.objectReferenceValue.name;
            }
        }
        // C#版
        //if (GUILayout.Button("复制代码"))
        //{
        //    var str = string.Empty;
        //    if (rootObject.objectReferenceValue)
        //    {
        //        str += rootTypeName.stringValue + " " + root.FindPropertyRelative("fieldName").stringValue + ";\n";
        //        str += "class " + rootTypeName.stringValue + " : UIGameObject\n{\n";
        //    }
        //    for (int i = 0; i < _itemInfoListProp.arraySize; i++)
        //    {
        //        var elementProp = _itemInfoListProp.GetArrayElementAtIndex(i);
        //        var gameObject = elementProp.FindPropertyRelative("gameObject");
        //        if (!gameObject.objectReferenceValue)
        //            continue;

        //        var fieldName = elementProp.FindPropertyRelative("fieldName");
        //        var typeName = elementProp.FindPropertyRelative("typeName");
        //        str += typeName.stringValue + " " + fieldName.stringValue + ";\n";
        //    }
        //    if (rootObject.objectReferenceValue)
        //        str += "}";
        //    GUIUtility.systemCopyBuffer = str;
        //}
        //Lua 版
        if (GUILayout.Button("复制代码"))
        {
            var injectList = string.Empty;
            var injectField = string.Empty;

            //var prefix = ".root";
            var prefix = string.Empty;
            if (rootObject.objectReferenceValue)
            {
                var rootFieldName = root.FindPropertyRelative("fieldName").stringValue;
                prefix = "." + rootFieldName;
                injectField += "M." + rootFieldName + " = {}\r\n";
                injectField += "M." + rootFieldName + ".gameObject = {}\r\n";
            }

            injectList += "M.injectComponentList" + prefix + " = {\r\n";
            for (int i = 0; i < _itemInfoListProp.arraySize; i++)
            {
                var elementProp = _itemInfoListProp.GetArrayElementAtIndex(i);
                var gameObject = elementProp.FindPropertyRelative("gameObject");
                if (!gameObject.objectReferenceValue)
                    continue;

                var fieldName = elementProp.FindPropertyRelative("fieldName");
                var typeName = elementProp.FindPropertyRelative("typeName");
                injectList += "\"" + fieldName.stringValue + "\",\r\n";
                //if (prefix == ".root")
                //    prefix = string.Empty;
                injectField += $"---@type {typeName.stringValue}\r\n";
                injectField += "self" + prefix + "." + fieldName.stringValue + " = {}\r\n";
            }
            injectList += "}\r\n";
            GUIUtility.systemCopyBuffer = /*injectList + "\r\n" +*/ injectField;
        }

        if (GUILayout.Button("清理\"空\"元素"))
        {
            for (int i = 0; i < _itemInfoListProp.arraySize;)
            {
                var elementProp = _itemInfoListProp.GetArrayElementAtIndex(i);
                var gameObject = elementProp.FindPropertyRelative("gameObject");
                if (!gameObject.objectReferenceValue)
                    _itemInfoListProp.DeleteArrayElementAtIndex(i);
                else
                    i++;
            }

            InitList();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("生成Lua代码"))
        {
            GenLuaCode();
        }
        EditorGUILayout.EndHorizontal();
        serializedObject.ApplyModifiedProperties();
    }

    void CopyLuaCode()
    {
        var thisRef = target as UIComponentRef;
        UIComponentRef[] refs = thisRef.gameObject.GetComponents<UIComponentRef>();

        string str = "";
        //str += "local ui = {}\r\n";

        foreach (var uiRef in refs)
        {
            if (uiRef.root.gameObject == null)
            {
                foreach (var item in uiRef.itemInfoList)
                {
                    str += string.Format("---@type {0}\r\n", item.typeName);
                    str += string.Format("self.{0} = {{}}\r\n", item.fieldName);
                }
            }
            else
            {
                string prefix = string.Format("self.{0}", uiRef.root.fieldName);

                str += string.Format("{0} = {{}}\r\n", prefix);
                str += "---@type GameObject\r\n";
                str += string.Format("{0}.gameObject = {{}}\r\n", prefix);
                foreach (var item in uiRef.itemInfoList)
                {
                    str += string.Format("---@type {0}\r\n", item.typeName);
                    str += string.Format("{0}.{1} = {{}}\r\n", prefix, item.fieldName);
                }
            }
        }
        GUIUtility.systemCopyBuffer = str;
    }

    void GenLuaCode()
    {
        var thisRef = target as UIComponentRef;
        var root = thisRef.gameObject;
        string rootName = thisRef.gameObject.name;
        string str = "";
        str += "---------------\r\n";
        str += "--该文件为UIComponentRef工具自动生成，只是为了代码提示，请不要修改，不要直接使用\r\n";
        str += "--使用方式：\r\n";
        str += string.Format("--在需要提示的变量前增加---@type {0}\r\n", rootName);
        str += "---------------\r\n\r\n";
        UIComponentRef[] refs = root.gameObject.GetComponentsInChildren<UIComponentRef>();
        var groups = refs.GroupBy(x => x.gameObject).ToArray();

        foreach (var g in groups)
        {
            string className = g.Key.name;

            var groupRefs = g.ToArray<UIComponentRef>();
            // str += string.Format("---@class {0}\r\n", className);
            // str += string.Format("local {0} = {{}}\r\n", className);
            foreach (var uiRef in groupRefs)
            {
                if (uiRef.root.gameObject == null)
                {
                    str += $"{className} = {{}}\r\n";
                    foreach (var item in uiRef.itemInfoList)
                    {
                        str += string.Format("---@type {0}\r\n", item.typeName);
                        str += string.Format("{1}.{0} = nil\r\n", item.fieldName, className);
                    }

                }
                else
                {
                    str += "\r\n";
                    string fieldName = uiRef.root.fieldName;
                    if (string.IsNullOrEmpty(fieldName))
                    {
                        fieldName = uiRef.name;
                    }
                    string prefix = string.Format("{1}.{0}", fieldName, className);
                    str += string.Format("{0} = {{}}\r\n", prefix);
                    str += string.Format("---@type {0}\r\n", "GameObject");
                    str += string.Format("{0}.gameObject = nil\r\n", prefix);
                    foreach (var item in uiRef.itemInfoList)
                    {
                        str += string.Format("---@type {0}\r\n", item.typeName);
                        str += string.Format("{0}.{1} = nil\r\n", prefix, item.fieldName);
                    }
                }
            }
            str += "\r\n\r\n";
        }

        var LuaPath = "Assets/AssetBundles/Luas/EditorDefines/UI/" + rootName + ".lua";
        string dir = Path.GetDirectoryName(LuaPath);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        var asest = UnityEditor.VersionControl.Provider.GetAssetByPath(LuaPath);
        if (asest != null)
        {
            var task = UnityEditor.VersionControl.Provider.Checkout(LuaPath, UnityEditor.VersionControl.CheckoutMode.Asset);
            task.Wait();
        }

        File.WriteAllText(LuaPath, str);
        AssetDatabase.Refresh();
    }
}
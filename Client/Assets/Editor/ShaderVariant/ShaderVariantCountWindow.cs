using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

public class ShaderVariantCountWindow : EditorWindow
{
    public static string assetFolderPath = "Assets"; // Assets/Resources/Shaders";
    public static string dataSavePath;
    [@MenuItem("Tools/Shader/ͳ�Ʊ�������")]
    public static void ShowWindow()
    {
        EditorWindow thisWindow = EditorWindow.GetWindow(typeof(ShaderVariantCountWindow));
        thisWindow.titleContent = new GUIContent("ͳ��shader��������");
        thisWindow.position = new Rect(Screen.width / 2, Screen.height / 2, 600, 400);
    }

    public static void GetAllShaderVariantCount()
    {
        Assembly asm = Assembly.GetAssembly(typeof(UnityEditor.SceneView));
        // Assembly asm = Assembly.LoadFile(@"D:\Unity\Unity2018.4.7f1\Editor\Data\Managed\UnityEditor.dll");
        System.Type t2 = asm.GetType("UnityEditor.ShaderUtil");
        MethodInfo method = t2.GetMethod("GetVariantCount", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        Debug.Log(Application.dataPath);
        string projectPath = Application.dataPath.Replace("Assets", "");
        Debug.Log(projectPath);
        assetFolderPath = assetFolderPath.Replace(projectPath, "");
        var shaderList = AssetDatabase.FindAssets("t:Shader", new[] { assetFolderPath });

        // var output = System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
        string date = System.DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss");
        string pathF = string.Format("{0}/ShaderVariantCount{1}.txt", dataSavePath, date);
        FileStream fs = new FileStream(pathF, FileMode.Create, FileAccess.Write);
        StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);

        EditorUtility.DisplayProgressBar("Shaderͳ���ļ�", "����д��ͳ���ļ���...", 0f);
        int ix = 0;
        sw.WriteLine("Shader ������" + shaderList.Length);
        sw.WriteLine("ShaderFile, VariantCount");
        int totalCount = 0;
        foreach (var i in shaderList)
        {
            EditorUtility.DisplayProgressBar("Shaderͳ���ļ�", "����д��ͳ���ļ���...", ix / shaderList.Length);
            var path = AssetDatabase.GUIDToAssetPath(i);
            Shader s = AssetDatabase.LoadAssetAtPath(path, typeof(Shader)) as Shader;
            var variantCount = method.Invoke(null, new System.Object[] { s, true });
            if (int.Parse(variantCount.ToString()) > 20)
            {
                sw.WriteLine(path + "," + variantCount.ToString() + "!!!!!!!!!!!!!!!!!!!");
            }
            else
            {
                sw.WriteLine(path + "," + variantCount.ToString());
            }
            totalCount += int.Parse(variantCount.ToString());
            ++ix;
        }
        sw.WriteLine("Shader Variant Total Amount: " + totalCount);
        EditorUtility.ClearProgressBar();
        sw.Close();
        fs.Close();
        OpenFolder();
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("ѡ���ļ���");
        EditorGUILayout.TextField(assetFolderPath);
        if (GUILayout.Button("ѡ��"))
        {
            assetFolderPath = EditorUtility.OpenFolderPanel("ѡ���ļ���", assetFolderPath, "");
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("ѡ��洢λ��");
        EditorGUILayout.TextField(dataSavePath);
        if (GUILayout.Button("ѡ��"))
        {
            dataSavePath = EditorUtility.OpenFolderPanel("ѡ��洢λ��", dataSavePath, "");
        }
        EditorGUILayout.EndHorizontal();


        if (GUILayout.Button("��ʼ����") && assetFolderPath != null && dataSavePath != null)
        {
            GetAllShaderVariantCount();
        }
    }

    public static void OpenFolder()
    {
        string path = dataSavePath.Replace("/", "\\");
        System.Diagnostics.Process.Start("explorer.exe", path);
    }
}
/*
 * Created by jiadong chen
 * http://www.chenjd.me
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
public class AnimMapBakerWindow : EditorWindow
{


    #region 字段

    public static GameObject targetGo;
    private static AnimMapBaker baker;
    private static string path = "DefaultPath";
    private static string subPath = "SubPath";
    private static Shader animMapShader;

    #endregion


    #region  方法

    [MenuItem("Window/AnimMapBaker")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(AnimMapBakerWindow));
        baker = new AnimMapBaker();
        animMapShader = Shader.Find("chenjd/AnimMapShader");
    }

    void OnGUI()
    {
        targetGo = (GameObject)EditorGUILayout.ObjectField(targetGo, typeof(GameObject), true);
        subPath = targetGo == null ? subPath : targetGo.name;
        EditorGUILayout.LabelField(string.Format("保存路径output path:{0}", Path.Combine(path, subPath)));
        path = EditorGUILayout.TextField(path);
        subPath = EditorGUILayout.TextField(subPath);

        if (GUILayout.Button("Bake"))
        {
            if(targetGo == null)
            {
                EditorUtility.DisplayDialog("err", "targetGo is null！", "OK");
                return;
            }

            if(baker == null)
            {
                baker = new AnimMapBaker();
            }

            baker.SetAnimData(targetGo);

            List<BakedData> list = baker.Bake();

            if(list != null)
            {
                for(int i = 0; i < list.Count; i++)
                {
                    BakedData data = list[i];
                    Save(ref data);
                }
            }
        }
    }


    private void Save(ref BakedData data)
    {
        SaveAsAsset(ref data);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void SaveAsAsset(ref BakedData data)
    {
        string folderPath = CreateFolder();
        MemoryStream file = new MemoryStream(1024);
        StreamWriter writer = new StreamWriter(file);
        writer.Write(data.animMapWidth);
        writer.Write(data.animMapHeight);
        writer.Write(data.animLen);
        for (int i = 0; i < data.animData.Length; ++i)
        {
            var point = data.animData[i];
            writer.Write(point.x);
            writer.Write(point.y);
            writer.Write(point.z);
        }
        StringBuilder pathBuilder = new StringBuilder(1024);
        pathBuilder.Append(Application.dataPath).Append('/');
        pathBuilder.Append(subPath).Append('/');
        pathBuilder.Append(data.name).Append(".bytes");
        var fullPath = pathBuilder.ToString();
        File.WriteAllBytes(fullPath, file.GetBuffer());
    }

    

    private string CreateFolder()
    {
        string folderPath = Path.Combine("Assets/" + path,  subPath);
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets/" + path, subPath);
        }
        return folderPath;
    }

    #endregion


}

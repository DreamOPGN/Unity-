using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class ExportAB
{
    [MenuItem("AB������/Windows")]
    public static void ForWindows()
    {
        Export(BuildTarget.StandaloneWindows);
    }
    [MenuItem("AB������/Mac")]
    public static void ForMac()
    {
        Export(BuildTarget.StandaloneOSX);
    }
    [MenuItem("AB������/ios")]
    public static void Forios()
    {
        Export(BuildTarget.iOS);
    }
    [MenuItem("AB������/Android")]
    public static void ForAndroid()
    {
        Export(BuildTarget.Android);
    }

    private static void Export(BuildTarget platform)
    {
        string path = Config.ABPath;
        //path = path.Substring(0, path.Length - 6) + "ab";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        //Debug.Log(path);
        //����ABc�����Ĵ��룬����ABc���ļ�
        //����1��ab���ļ��洢·��
        //����2������ѡ��
        //����3��ƽ̨����ͬƽ̨AB����һ����
        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.ForceRebuildAssetBundle,
            platform);
        Debug.Log("�����ɹ�!");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class ExportAB
{
    [MenuItem("AB包导出/Windows")]
    public static void ForWindows()
    {
        Export(BuildTarget.StandaloneWindows);
    }
    [MenuItem("AB包导出/Mac")]
    public static void ForMac()
    {
        Export(BuildTarget.StandaloneOSX);
    }
    [MenuItem("AB包导出/ios")]
    public static void Forios()
    {
        Export(BuildTarget.iOS);
    }
    [MenuItem("AB包导出/Android")]
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
        //导出ABc包核心代码，生成ABc包文件
        //参数1：ab包文件存储路径
        //参数2：导出选项
        //参数3：平台（不同平台AB包不一样）
        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.ForceRebuildAssetBundle,
            platform);
        Debug.Log("导出成功!");
    }
}

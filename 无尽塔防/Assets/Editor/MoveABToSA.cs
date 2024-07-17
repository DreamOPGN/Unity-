using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;

public class MoveABToSA
{
    [MenuItem("AB包工具/移动选中资源到StreamingAssets中")]
    private static void MoveABToStreamingAssets()
    {
        //通过编辑器Selection类中的方法 获取再Project窗口中选中的资源 
        Object[] selectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        //如果一个资源都没有选择 就没有必要处理后面的逻辑了
        if (selectedAsset.Length == 0)
            return;



        //用于拼接本地默认AB包资源信息的字符串
        string abCompareInfo = "";
        //获得原本StreamingAssets文件夹里的AB包信息
        DirectoryInfo directory = Directory.CreateDirectory(Application.dataPath + "/StreamingAssets");
        FileInfo[] fileInfos = directory.GetFiles();

        for (int i = 0; i < fileInfos.Length; i++)
        {
            for (int j = 0; j < selectedAsset.Length; j++)
            {
                if (fileInfos[i].Extension == "" && selectedAsset[j].name != fileInfos[i].Name)  //如果后缀为空
                {
                    abCompareInfo += fileInfos[i].Name + " " + fileInfos[i].Length + " " + GetMD5(fileInfos[i].FullName);
                    //用一个符号隔开多个AB包信息

                    abCompareInfo += "|";
                }
            }
        }

        //遍历选中的资源对象
        foreach (Object asset in selectedAsset)
        {
            //通过Assetdatabase类 获取 资源的路径
            string assetPath = AssetDatabase.GetAssetPath(asset);
            //截取路径当中的文件名 用于作为 StreamingAssets中的文件名
            string fileName = assetPath.Substring(assetPath.LastIndexOf('/'));

            //判断是否有.符号 如果有 证明有后缀 不处理
            if (fileName.IndexOf('.') != -1)
                continue;
            //你还可以在拷贝之前 去获取全路径 然后通过FIleInfo去获取后缀来判断 这样更加的准确

            //利用AssetDatabase中的API 将选中文件 复制到目标路径
            AssetDatabase.CopyAsset(assetPath, "Assets/StreamingAssets" + fileName);

            //获取拷贝到StreamingAssets文件夹中的文件的全部信息
            FileInfo fileInfo = new FileInfo(Application.streamingAssetsPath + fileName);
            //拼接AB包信息到字符串中
            abCompareInfo += fileInfo.Name + " " + fileInfo.Length + " " + GetMD5(fileInfo.FullName);
            //用一个符号隔开多个AB包信息
            
            abCompareInfo += "|";
        }
        
        //去掉最后一个|符号 为了之后拆分字符串方便
        abCompareInfo = abCompareInfo.Substring(0, abCompareInfo.Length - 1);
        //将本地默认资源的对比信息 存入文件
        File.WriteAllText(Application.streamingAssetsPath + "/ABCompareInfo.txt", abCompareInfo);
        //刷新窗口
        AssetDatabase.Refresh();
    }

    public static string GetMD5(string filePath)
    {
        using (FileStream file = new FileStream(filePath, FileMode.Open))
        {
            //声明一个MD5对象 用于生成MD5码
            MD5 md5 = new MD5CryptoServiceProvider();
            //利用APi 得到数据的MD5码 16个字节 数组
            byte[] md5Info = md5.ComputeHash(file);

            //关闭文件流
            file.Close();

            //将16个字节转换为 16进制 拼接成字符串 减少md5码长度
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < md5Info.Length; i++)
            {
                stringBuilder.Append(md5Info[i].ToString("x2"));
            }
            return stringBuilder.ToString();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public  class CreateABCompare
{
    [MenuItem("AB包工具/生成AB包对比工具")]
    public static void Create()
    {
        DirectoryInfo directory = Directory.CreateDirectory(Config.UPLOADAB_PATH);
        FileInfo[] fileInfos = directory.GetFiles();

        string abCompareInfo = "";
        foreach (var info in fileInfos)
        {

            if(info.Extension == "" || info.Extension == ".manifest")  //如果无后缀才是AB包
            {
                
                //拼接一个AB包的信息
                abCompareInfo += info.Name + " " + info.Length + " " + GetMD5(info.FullName);
                //用一个分隔符分开不同文件之间的信息
                abCompareInfo += '|';
            }

        }

        abCompareInfo = abCompareInfo.Substring(0, abCompareInfo.Length - 1);
        

        File.WriteAllText(Application.dataPath + "/ABPackageRes/Data/ABCompareInfo.txt", abCompareInfo);
        AssetDatabase.Refresh();
        Debug.Log("AB包对比文件生成成功");
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

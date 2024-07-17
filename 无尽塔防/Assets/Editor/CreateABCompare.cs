using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public  class CreateABCompare
{
    [MenuItem("AB������/����AB���Աȹ���")]
    public static void Create()
    {
        DirectoryInfo directory = Directory.CreateDirectory(Config.UPLOADAB_PATH);
        FileInfo[] fileInfos = directory.GetFiles();

        string abCompareInfo = "";
        foreach (var info in fileInfos)
        {

            if(info.Extension == "" || info.Extension == ".manifest")  //����޺�׺����AB��
            {
                
                //ƴ��һ��AB������Ϣ
                abCompareInfo += info.Name + " " + info.Length + " " + GetMD5(info.FullName);
                //��һ���ָ����ֿ���ͬ�ļ�֮�����Ϣ
                abCompareInfo += '|';
            }

        }

        abCompareInfo = abCompareInfo.Substring(0, abCompareInfo.Length - 1);
        

        File.WriteAllText(Application.dataPath + "/ABPackageRes/Data/ABCompareInfo.txt", abCompareInfo);
        AssetDatabase.Refresh();
        Debug.Log("AB���Ա��ļ����ɳɹ�");
    }

    public static string GetMD5(string filePath)
    {
        using (FileStream file = new FileStream(filePath, FileMode.Open))
        {
            //����һ��MD5���� ��������MD5��
            MD5 md5 = new MD5CryptoServiceProvider();
            //����APi �õ����ݵ�MD5�� 16���ֽ� ����
            byte[] md5Info = md5.ComputeHash(file);

            //�ر��ļ���
            file.Close();

            //��16���ֽ�ת��Ϊ 16���� ƴ�ӳ��ַ��� ����md5�볤��
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < md5Info.Length; i++)
            {
                stringBuilder.Append(md5Info[i].ToString("x2"));
            }
            return stringBuilder.ToString();
        }
    }
}

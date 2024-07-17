using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;

public class MoveABToSA
{
    [MenuItem("AB������/�ƶ�ѡ����Դ��StreamingAssets��")]
    private static void MoveABToStreamingAssets()
    {
        //ͨ���༭��Selection���еķ��� ��ȡ��Project������ѡ�е���Դ 
        Object[] selectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        //���һ����Դ��û��ѡ�� ��û�б�Ҫ���������߼���
        if (selectedAsset.Length == 0)
            return;



        //����ƴ�ӱ���Ĭ��AB����Դ��Ϣ���ַ���
        string abCompareInfo = "";
        //���ԭ��StreamingAssets�ļ������AB����Ϣ
        DirectoryInfo directory = Directory.CreateDirectory(Application.dataPath + "/StreamingAssets");
        FileInfo[] fileInfos = directory.GetFiles();

        for (int i = 0; i < fileInfos.Length; i++)
        {
            for (int j = 0; j < selectedAsset.Length; j++)
            {
                if (fileInfos[i].Extension == "" && selectedAsset[j].name != fileInfos[i].Name)  //�����׺Ϊ��
                {
                    abCompareInfo += fileInfos[i].Name + " " + fileInfos[i].Length + " " + GetMD5(fileInfos[i].FullName);
                    //��һ�����Ÿ������AB����Ϣ

                    abCompareInfo += "|";
                }
            }
        }

        //����ѡ�е���Դ����
        foreach (Object asset in selectedAsset)
        {
            //ͨ��Assetdatabase�� ��ȡ ��Դ��·��
            string assetPath = AssetDatabase.GetAssetPath(asset);
            //��ȡ·�����е��ļ��� ������Ϊ StreamingAssets�е��ļ���
            string fileName = assetPath.Substring(assetPath.LastIndexOf('/'));

            //�ж��Ƿ���.���� ����� ֤���к�׺ ������
            if (fileName.IndexOf('.') != -1)
                continue;
            //�㻹�����ڿ���֮ǰ ȥ��ȡȫ·�� Ȼ��ͨ��FIleInfoȥ��ȡ��׺���ж� �������ӵ�׼ȷ

            //����AssetDatabase�е�API ��ѡ���ļ� ���Ƶ�Ŀ��·��
            AssetDatabase.CopyAsset(assetPath, "Assets/StreamingAssets" + fileName);

            //��ȡ������StreamingAssets�ļ����е��ļ���ȫ����Ϣ
            FileInfo fileInfo = new FileInfo(Application.streamingAssetsPath + fileName);
            //ƴ��AB����Ϣ���ַ�����
            abCompareInfo += fileInfo.Name + " " + fileInfo.Length + " " + GetMD5(fileInfo.FullName);
            //��һ�����Ÿ������AB����Ϣ
            
            abCompareInfo += "|";
        }
        
        //ȥ�����һ��|���� Ϊ��֮�����ַ�������
        abCompareInfo = abCompareInfo.Substring(0, abCompareInfo.Length - 1);
        //������Ĭ����Դ�ĶԱ���Ϣ �����ļ�
        File.WriteAllText(Application.streamingAssetsPath + "/ABCompareInfo.txt", abCompareInfo);
        //ˢ�´���
        AssetDatabase.Refresh();
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

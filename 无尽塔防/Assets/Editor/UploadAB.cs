using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class UploadAB
{
    [MenuItem("AB������/�ϴ�AB���ͶԱ��ļ�")]
    private static void UploadAllABFile()
    {
        //��ȡ�ļ�����Ϣ
        DirectoryInfo directory = Directory.CreateDirectory(Config.UPLOADAB_PATH);
        //��ȡ��Ŀ¼�µ������ļ���Ϣ
        FileInfo[] fileInfos = directory.GetFiles();

        foreach (FileInfo info in fileInfos)
        {
            //û�к�׺�� ����AB�� ����ֻ��ҪAB������Ϣ
            //������Ҫ��ȡ ��Դ�Ա��ļ� ��ʽ��txt�����ļ����� ֻ�жԱ��ļ��ĸ�ʽ����txt ���Կ��������жϣ�
            if (info.Extension == "" ||
                info.Extension == ".txt" ||
                info.Extension == ".manifest")
            {
                //�ϴ����ļ�
                FtpUploadFile(info.FullName, info.Name);
            }
        }
        //FtpUploadFile(Application.dataPath + "/ABPackageRes/Data.manifest", "Data.manifest");

    }

    private async static void FtpUploadFile(string filePath, string fileName)
    {
        await Task.Run(() =>
        {
            try
            {
                //1.����һ��FTP���� �����ϴ�
                FtpWebRequest req = FtpWebRequest.Create(new Uri("ftp://ABUpdate.io/" + fileName)) as FtpWebRequest;
                //FtpWebRequest req = FtpWebRequest.Create(new Uri("ftp://ABUpdate.io/AB/PC/" + fileName)) as FtpWebRequest;
                //FtpWebRequest req = FtpWebRequest.Create(new Uri("ftp://172.24.179.174/AB/PC/" + fileName)) as FtpWebRequest;
                //2.����һ��ͨ��ƾ֤ ���������ϴ�
                NetworkCredential n = new NetworkCredential("Admitor", "123456");
                req.Credentials = n;
                //3.��������
                //  ���ô���Ϊnull
                req.Proxy = null;
                //  ������Ϻ� �Ƿ�رտ�������
                req.KeepAlive = false;
                //  ��������-�ϴ�
                req.Method = WebRequestMethods.Ftp.UploadFile;
                //  ָ����������� 2����
                req.UseBinary = true;
                //4.�ϴ��ļ�
                //  ftp��������
                Stream upLoadStream = req.GetRequestStream();
                //  ��ȡ�ļ���Ϣ д���������
                using (FileStream file = File.OpenRead(filePath))
                {
                    //һ��һ����ϴ�����
                    byte[] bytes = new byte[2048];
                    //����ֵ �����ȡ�˶��ٸ��ֽ�
                    int contentLength = file.Read(bytes, 0, bytes.Length);

                    //ѭ���ϴ��ļ��е�����
                    while (contentLength != 0)
                    {
                        //д�뵽�ϴ�����
                        upLoadStream.Write(bytes, 0, contentLength);
                        //д���ٶ�
                        contentLength = file.Read(bytes, 0, bytes.Length);
                    }

                    //ѭ����Ϻ� ֤���ϴ�����
                    file.Close();
                    upLoadStream.Close();
                }

                Debug.Log(fileName + "�ϴ��ɹ�");
            }
            catch (Exception ex)
            {
                Debug.Log(fileName + "�ϴ�ʧ��" + ex.Message);
            }
        });
       
    }
}

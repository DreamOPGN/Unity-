using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class Lesson_MD5 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //dbd85bd6a0b667b82e1a14b922d7aed4
        //Debug.Log(GetMD5(Application.dataPath + "/ArtRes/AB/PC/lua"));
    }


    private string GetMD5(string filePath)
    {
        using(FileStream file = new FileStream(filePath, FileMode.Open))
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

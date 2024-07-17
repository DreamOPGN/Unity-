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

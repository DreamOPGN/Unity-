using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //����������ϵ��Ҫ������ab�������ļ�
        //������ab��
        //AssetBundle main = AssetBundle.LoadFromFile(Config.ABPath + "/ab");
        AssetBundle main = AssetBundle.LoadFromFile(Config.STEARINGASSET_PATH);
        //��ȡ��AB���������ļ�
        AssetBundleManifest mainfest = main.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        //����Ԥ�������ڵ�AB����������ЩAB��
        string[] deps = mainfest.GetAllDependencies("test2");
        //����������AB��
        for(int i = 0; i < deps.Length; i++)
        {
            AssetBundle.LoadFromFile(Config.STEARINGASSET_PATH + "/" + deps[i]);
        }
        //����Ԥ�������ڵ�AB��
        AssetBundle test2 = AssetBundle.LoadFromFile(Config.STEARINGASSET_PATH + "/test2");
        //����Ԥ����
        Object prefab = test2.LoadAsset("Image");

        GameObject img = Instantiate(prefab) as GameObject;
        img.transform.SetParent(GameObject.Find("/Canvas").transform);
        img.transform.localPosition = Vector3.zero;
        img.transform.localScale = Vector3.one;
    }

}

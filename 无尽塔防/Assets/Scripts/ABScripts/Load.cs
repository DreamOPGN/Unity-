using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //处理依赖关系需要加载主ab包配置文件
        //加载主ab包
        //AssetBundle main = AssetBundle.LoadFromFile(Config.ABPath + "/ab");
        AssetBundle main = AssetBundle.LoadFromFile(Config.STEARINGASSET_PATH);
        //获取主AB包的配置文件
        AssetBundleManifest mainfest = main.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        //分析预制体所在的AB包，依赖哪些AB包
        string[] deps = mainfest.GetAllDependencies("test2");
        //加载依赖的AB包
        for(int i = 0; i < deps.Length; i++)
        {
            AssetBundle.LoadFromFile(Config.STEARINGASSET_PATH + "/" + deps[i]);
        }
        //加载预制体所在的AB包
        AssetBundle test2 = AssetBundle.LoadFromFile(Config.STEARINGASSET_PATH + "/test2");
        //加载预制体
        Object prefab = test2.LoadAsset("Image");

        GameObject img = Instantiate(prefab) as GameObject;
        img.transform.SetParent(GameObject.Find("/Canvas").transform);
        img.transform.localPosition = Vector3.zero;
        img.transform.localScale = Vector3.one;
    }

}

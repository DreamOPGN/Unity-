using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotUpdate : MonoBehaviour
{
    public Image icon;
    void Start()
    {
        //第一步加载AB包文件
        AssetBundle ab = AssetBundle.LoadFromFile(Config.STEARINGASSET_PATH + "/one/ui");
        //第二部加载资源
        Sprite bg = ab.LoadAsset<Sprite>("Background_01");

        icon.GetComponent<Image>().sprite = bg;
        ab.Unload(false);
    }

    public void ChangeAB()
    {
        //第一步加载AB包文件
        AssetBundle ab = AssetBundle.LoadFromFile(Config.STEARINGASSET_PATH + "/two/ui");
        //第二部加载资源
        Sprite bg = ab.LoadAsset<Sprite>("Background_01");

        icon.GetComponent<Image>().sprite = bg;
        ab.Unload(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotUpdate : MonoBehaviour
{
    public Image icon;
    void Start()
    {
        //��һ������AB���ļ�
        AssetBundle ab = AssetBundle.LoadFromFile(Config.STEARINGASSET_PATH + "/one/ui");
        //�ڶ���������Դ
        Sprite bg = ab.LoadAsset<Sprite>("Background_01");

        icon.GetComponent<Image>().sprite = bg;
        ab.Unload(false);
    }

    public void ChangeAB()
    {
        //��һ������AB���ļ�
        AssetBundle ab = AssetBundle.LoadFromFile(Config.STEARINGASSET_PATH + "/two/ui");
        //�ڶ���������Դ
        Sprite bg = ab.LoadAsset<Sprite>("Background_01");

        icon.GetComponent<Image>().sprite = bg;
        ab.Unload(false);
    }
}

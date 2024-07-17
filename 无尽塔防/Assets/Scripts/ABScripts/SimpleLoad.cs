using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SimpleLoad : MonoBehaviour
{
    public Image image;
    void Start()
    {
        /*        //��һ������AB���ļ�
              AssetBundle ab = AssetBundle.LoadFromFile(Config.ABPath + "/ui");
                //�ڶ���������Դ
               Sprite bg = ab.LoadAsset<Sprite>("Background_01");

                GameObject.Find("/Canvas/Image").GetComponent<Image>().sprite = bg;*/
        CompleteABControl.Instance.LoadResourceAsync("ui", "Background_05", finishLoadObjectHandler);
    }
    private void finishLoadObjectHandler(Object obj)
    {
        //ͼ������
        /*        SpriteAtlas atlas = obj as SpriteAtlas;
                GameObject.Find("/Canvas/Image").GetComponent<Image>().sprite = atlas.GetSprite("SunFlower_04");*/
        Sprite sprite = obj as Sprite;
        GameObject.Find("/Canvas/Image").GetComponent<Image>().sprite = sprite;


        /*        GameObject img = Instantiate(obj) as GameObject;

                img.transform.SetParent(GameObject.Find("/Canvas").transform);
                img.transform.localPosition = Vector3.zero;
                img.transform.localScale = Vector3.one;*/
        //CompleteABControl.Instance.UnLoad("aaa/Plants");
    }

}

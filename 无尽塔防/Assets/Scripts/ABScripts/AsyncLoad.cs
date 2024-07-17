using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AsyncLoad : MonoBehaviour
{
    public CompleteABControl ca;
    void Start()
    {
        /*        GameObject.Find("/Canvas/Image").GetComponent<Image>().sprite =
                    Resources.Load<Sprite>("Label_BasicLabel01_White");*/

        // StartCoroutine(LoadImage());
        StartCoroutine(LoadAB());
     
    }
    IEnumerator LoadImage()
    {
        ResourceRequest rr = Resources.LoadAsync<Sprite>("Label_BasicLabel01_White");
        yield return rr;
        GameObject.Find("/Canvas/Image").GetComponent<Image>().sprite =
                    rr.asset as Sprite;
    }

    IEnumerator LoadAB()
    {
        AssetBundleCreateRequest abcr = AssetBundle.LoadFromFileAsync(Config.STEARINGASSET_PATH + "/table");
        yield return abcr;
       GameObject.Find("/Canvas/Image").GetComponent<Image>().sprite = abcr.assetBundle.LoadAsset<Sprite>("Label_TitleRibbon_Yellow");

    }
    // 委托调用处理逻辑
}
    //调用GC
    // Resources.UnloadUnusedAssets();



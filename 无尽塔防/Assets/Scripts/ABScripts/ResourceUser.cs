using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceUser : MonoBehaviour
{
    private AssetBundleCache assetBundleCache = new AssetBundleCache();

    void Start()
    {
        string assetBundlePath = "path/to/assetbundle";
        string resourceName = "AssetName";

        GameObject obj = (GameObject)assetBundleCache.GetResource(assetBundlePath, resourceName);
        // 使用 obj 完成相关操作

        // 当资源不再需要时释放
        assetBundleCache.ReleaseResource(resourceName);
    }
}

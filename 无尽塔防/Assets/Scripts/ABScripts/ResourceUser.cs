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
        // ʹ�� obj �����ز���

        // ����Դ������Ҫʱ�ͷ�
        assetBundleCache.ReleaseResource(resourceName);
    }
}

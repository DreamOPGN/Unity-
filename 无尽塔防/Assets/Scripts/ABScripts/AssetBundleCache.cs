using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleCache
{
    private Dictionary<string, AssetBundle> assetBundleCache = new Dictionary<string, AssetBundle>();
    private Dictionary<string, Object> resourceCache = new Dictionary<string, Object>();

    public Object GetResource(string assetBundlePath, string resourceName)
    {
        // �ȼ����Դ�Ƿ��Ѿ��ڻ�����
        if (resourceCache.ContainsKey(resourceName))
        {
            return resourceCache[resourceName];
        }
        else
        {
            // ��� AssetBundle �Ƿ��Ѿ��ڻ�����
            if (assetBundleCache.ContainsKey(assetBundlePath))
            {
                AssetBundle assetBundle = assetBundleCache[assetBundlePath];
                Object resource = assetBundle.LoadAsset<Object>(resourceName);
                if (resource != null)
                {
                    // ����Դ��ӵ�������
                    resourceCache.Add(resourceName, resource);
                    return resource;
                }
            }
            else
            {
                // ��� AssetBundle ���ڻ����У������ AssetBundle
                AssetBundle assetBundle = AssetBundle.LoadFromFile(assetBundlePath);
                if (assetBundle != null)
                {
                    assetBundleCache.Add(assetBundlePath, assetBundle);
                    Object resource = assetBundle.LoadAsset<Object>(resourceName);
                    if (resource != null)
                    {
                        // ����Դ��ӵ�������
                        resourceCache.Add(resourceName, resource);
                        return resource;
                    }
                }
            }
        }

        Debug.LogError("Resource not found: " + resourceName);
        return null;
    }

    public void ReleaseResource(string resourceName)
    {
        if (resourceCache.ContainsKey(resourceName))
        {
            Object resource = resourceCache[resourceName];
            resourceCache.Remove(resourceName);
            Resources.UnloadUnusedAssets(); // �ͷ���Դ
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleCache
{
    private Dictionary<string, AssetBundle> assetBundleCache = new Dictionary<string, AssetBundle>();
    private Dictionary<string, Object> resourceCache = new Dictionary<string, Object>();

    public Object GetResource(string assetBundlePath, string resourceName)
    {
        // 先检查资源是否已经在缓存中
        if (resourceCache.ContainsKey(resourceName))
        {
            return resourceCache[resourceName];
        }
        else
        {
            // 检查 AssetBundle 是否已经在缓存中
            if (assetBundleCache.ContainsKey(assetBundlePath))
            {
                AssetBundle assetBundle = assetBundleCache[assetBundlePath];
                Object resource = assetBundle.LoadAsset<Object>(resourceName);
                if (resource != null)
                {
                    // 将资源添加到缓存中
                    resourceCache.Add(resourceName, resource);
                    return resource;
                }
            }
            else
            {
                // 如果 AssetBundle 不在缓存中，则加载 AssetBundle
                AssetBundle assetBundle = AssetBundle.LoadFromFile(assetBundlePath);
                if (assetBundle != null)
                {
                    assetBundleCache.Add(assetBundlePath, assetBundle);
                    Object resource = assetBundle.LoadAsset<Object>(resourceName);
                    if (resource != null)
                    {
                        // 将资源添加到缓存中
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
            Resources.UnloadUnusedAssets(); // 释放资源
        }
    }
}

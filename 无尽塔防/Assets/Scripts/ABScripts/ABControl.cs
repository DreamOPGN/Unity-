using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABControl : MonoBehaviour
{
    private static ABControl instance;
    public static ABControl Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("GameManager").AddComponent<ABControl>();
            }
            return instance;
        }
    }


    private Dictionary<string, AssetBundle> abDictionary = new Dictionary<string, AssetBundle>();

    private AssetBundle mainAB = null;
    private AssetBundleManifest mainfest = null;
    private string mainABName
    {
        get
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            return "/StreamingAssets";
#elif UNITY_IPHONE
                return "IOS";
#elif UNITY_ANDROID
                return "Android";
#endif
        }
    }


    private string abPath
    {
        get
        {
            //使用StreamingAssets路径注意AB包打包时 勾选copy to streamingAssets
            return Config.STEARINGASSET_PATH + "/";
        }
    }
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //AB包同步加载
    /// <summary>
    /// LoadABPackage提供AB包是同步加载
    /// </summary>
    private AssetBundle LoadABPackage(string abName)
    {
        AssetBundle ab;
        if (mainAB == null)
        {
            mainAB = AssetBundle.LoadFromFile(abPath + mainABName);

            mainfest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        //根据manifest获取所有依赖包的名称 固定API
        string[] dependencies = mainfest.GetAllDependencies(abName);
        //循环加载所有依赖包
        for (int i = 0; i < dependencies.Length; i++)
        {
            //如果不在缓存则加入
            if (!abDictionary.ContainsKey(dependencies[i]))
            {
                //根据依赖包名称进行加载
                ab = AssetBundle.LoadFromFile(abPath + dependencies[i]);
                //注意添加进缓存 防止重复加载AB包
                abDictionary.Add(dependencies[i], ab);
            }
        }
        //加载目标包 -- 同理注意缓存问题
        if (abDictionary.ContainsKey(abName))
        {
            return abDictionary[abName];
        }
        else
        {

            ab = AssetBundle.LoadFromFile(abPath + abName);
            abDictionary.Add(abName, ab);
            return ab;

        }

    }

    /// <summary>
    /// LoadResourceAsync提供AB包是异步加载，资源也是异步加载（推荐使用）
    /// 只支持AB包文件在streamingAssets目录下的AB包
    /// abName是AB包的名字路径，resName是AB包里的资源名称，finishLoadObjectHandler是回调函数
    /// </summary>
    public void LoadResourceAsync<T>(string abName, string resName, System.Action<Object> finishLoadObjectHandler) where T : Object
    {
        StartCoroutine(LoadPackage<T>(abName, resName, finishLoadObjectHandler));
    }
    IEnumerator LoadPackage<T>(string abName, string resourceName, System.Action<Object> finishLoadObjectHandler) where T : Object
    {
        AssetBundle ab;
        if (mainAB == null)
        {
            mainAB = AssetBundle.LoadFromFile(abPath + mainABName);

            mainfest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        //根据manifest获取所有依赖包的名称 固定API
        string[] dependencies = mainfest.GetAllDependencies(abName);
        //循环加载所有依赖包
        for (int i = 0; i < dependencies.Length; i++)
        {
            //如果不在缓存则加入
            if (!abDictionary.ContainsKey(dependencies[i]))
            {
                //根据依赖包名称进行加载
                AssetBundleCreateRequest abcr = AssetBundle.LoadFromFileAsync(abPath + dependencies[i]);
                yield return abcr;
                //注意添加进缓存 防止重复加载AB包
                abDictionary.Add(dependencies[i], abcr.assetBundle);
            }
        }
        //加载目标包 -- 同理注意缓存问题
        if (abDictionary.ContainsKey(abName))
        {
            ab = abDictionary[abName];
        }
        else
        {

            AssetBundleCreateRequest abcr = AssetBundle.LoadFromFileAsync(abPath + abName);
            yield return abcr;
            ab = abcr.assetBundle;
            abDictionary.Add(abName, abcr.assetBundle);

        }

        CallBackPackage<T>(ab, resourceName, finishLoadObjectHandler);
    }
    private void CallBackPackage<T>(AssetBundle ab, string resName, System.Action<Object> finishLoadObjectHandler) where T : Object
    {
        StartCoroutine(LoadRes<T>(ab, resName, finishLoadObjectHandler));
    }
    private IEnumerator LoadRes<T>(AssetBundle ab, string resName, System.Action<Object> finishLoadObjectHandler) where T : Object
    {
        if (ab == null) yield break;
        AssetBundleRequest abr = ab.LoadAssetAsync<T>(resName);
        yield return abr;
        //委托调用处理逻辑
        finishLoadObjectHandler(abr.asset as T);
    }
    /// <summary>
    /// 这个方法是从persistentDataPath路径加载AB包获取资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="finishLoadObjectHandler"></param>
    public void LoadResourceFrompersistentDataPathAsync<T>(string abName, string resName, System.Action<Object> finishLoadObjectHandler) where T : Object
    {
        StartCoroutine(LoadFrompersistentDataPathPackage<T>(abName, resName, finishLoadObjectHandler));
    }
    IEnumerator LoadFrompersistentDataPathPackage<T>(string abName, string resourceName, System.Action<Object> finishLoadObjectHandler) where T : Object
    {
        AssetBundle ab;
        if (mainAB == null)
        {
            mainAB = AssetBundle.LoadFromFile(Application.persistentDataPath + "/ABPackageRes/Data/Data");

            mainfest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        //根据manifest获取所有依赖包的名称 固定API
        string[] dependencies = mainfest.GetAllDependencies(abName);
        //循环加载所有依赖包
        for (int i = 0; i < dependencies.Length; i++)
        {
            //如果不在缓存则加入
            if (!abDictionary.ContainsKey(dependencies[i]))
            {
                //根据依赖包名称进行加载
                AssetBundleCreateRequest abcr = AssetBundle.LoadFromFileAsync(Application.persistentDataPath + "/ABPackageRes/Data/" + dependencies[i]);
                yield return abcr;
                //注意添加进缓存 防止重复加载AB包
                abDictionary.Add(dependencies[i], abcr.assetBundle);
            }
        }
        //加载目标包 -- 同理注意缓存问题
        if (abDictionary.ContainsKey(abName))
        {
            ab = abDictionary[abName];
        }
        else
        {

            AssetBundleCreateRequest abcr = AssetBundle.LoadFromFileAsync(Application.persistentDataPath + "/ABPackageRes/Data/" + abName);
            yield return abcr;
            ab = abcr.assetBundle;
            abDictionary.Add(abName, abcr.assetBundle);

        }
        CallBackFrompersistentDataPathPackage<T>(ab, resourceName, finishLoadObjectHandler);
        
    }
    private void CallBackFrompersistentDataPathPackage<T>(AssetBundle ab, string resName, System.Action<Object> finishLoadObjectHandler) where T : Object
    {
        StartCoroutine(LoadResFrompersistentDataPath<T>(ab, resName, finishLoadObjectHandler));
    }
    private IEnumerator LoadResFrompersistentDataPath<T>(AssetBundle ab, string resName, System.Action<Object> finishLoadObjectHandler) where T : Object
    {
        if (ab == null) yield break;
        AssetBundleRequest abr = ab.LoadAssetAsync<T>(resName);
        yield return abr;
        //委托调用处理逻辑
        finishLoadObjectHandler(abr.asset as T);
    }


    /// <summary>
    /// 卸载单个AB包
    /// </summary>
    /// <param name="abName"></param>
    public void UnLoad(string abName)
    {
        if (mainfest == null || abDictionary.Count == 0)
            return;
        Debug.Log(mainfest.GetAllDependencies(abName).Length);
        if (mainfest.GetAllDependencies(abName).Length > 0)
        {

            UnloadAssetBundle(abName);

        }
        else
        {
            UnloadSingleAssetBundle(abName);
        }

    }

/// <summary>
/// 卸载所有AB包
/// </summary>
    public void UnLoadAll()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        //注意清空缓存
        abDictionary.Clear();
        mainAB = null;
        mainfest = null;
    }
    public void UnloadAssetBundle(string assetBundleName)
    {
        UnloadDependentAssetBundles(assetBundleName); // 卸载依赖包
        UnloadSingleAssetBundle(assetBundleName); // 卸载目标包
    }

    private void UnloadDependentAssetBundles(string assetBundleName)
    {
        string[] dependencies = mainfest.GetAllDependencies(assetBundleName);
        for (int i = 0; i < dependencies.Length; i++)
        {
            if (abDictionary.ContainsKey(dependencies[i]))
            {
                abDictionary[dependencies[i]].Unload(false); // 卸载依赖包
                abDictionary.Remove(dependencies[i]);
                UnloadDependentAssetBundles(dependencies[i]); // 递归卸载被依赖的包的依赖包
            }
        }
    }

    private void UnloadSingleAssetBundle(string assetBundleName)
    {
        if (abDictionary.ContainsKey(assetBundleName))
        {
            abDictionary[assetBundleName].Unload(false); // 卸载目标包
            abDictionary.Remove(assetBundleName);
        }
    }
}

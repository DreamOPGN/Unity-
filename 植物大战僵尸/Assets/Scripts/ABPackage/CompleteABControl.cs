using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteABControl : MonoBehaviour
{
    private static CompleteABControl instance;
    public static CompleteABControl Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("GameManager").AddComponent<CompleteABControl>();
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
            return Config.ABPath + "/";
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
    private void CallBackPackage<T>(AssetBundle ab , string resName, System.Action<Object> finishLoadObjectHandler) where T : Object
    {
        StartCoroutine(LoadRes<T>(ab, resName, finishLoadObjectHandler));
    }

/*    //==================三种资源同步加载方式==================
    //提供多种调用方式 便于其它语言的调用（Lua对泛型支持不好）
    #region 同步加载的三个重载

    /// <summary>
    /// 同步加载资源---泛型加载 简单直观 无需显示转换
    /// </summary>
    /// <param name="abName">ab包的名称</param>
    /// <param name="resName">资源名称</param>
    public T LoadResource<T>(string abName, string resName) where T : Object
    {
        //加载目标包
        AssetBundle ab = LoadABPackage(abName);

        //返回资源
        return ab.LoadAsset<T>(resName);
    }


    //不指定类型 有重名情况下不建议使用 使用时需显示转换类型
    public Object LoadResource(string abName, string resName)
    {
        //加载目标包
        AssetBundle ab = LoadABPackage(abName);

        //返回资源
        return ab.LoadAsset(resName);
    }


    //利用参数传递类型，适合对泛型不支持的语言调用，使用时需强转类型
    public Object LoadResource(string abName, string resName, System.Type type)
    {
        //加载目标包
        AssetBundle ab = LoadABPackage(abName);

        //返回资源
        return ab.LoadAsset(resName, type);
    }

    #endregion*/


    /// <summary>
    /// 提供异步加载----注意 这里加载AB包是同步加载，只是加载资源是异步
    /// </summary>
    /// <param name="abName">ab包名称</param>
    /// <param name="resourceName">资源名称</param>
    public void LoadResourceAsync(string abName, string resourceName, System.Action<Object> finishLoadObjectHandler)
    {
        AssetBundle ab = LoadABPackage(abName);
        //开启协程 提供资源加载成功后的委托
        StartCoroutine(LoadRes(ab, resourceName, finishLoadObjectHandler));
    }


    private IEnumerator LoadRes(AssetBundle ab, string resName, System.Action<Object> finishLoadObjectHandler)
    {
        if (ab == null) yield break;
        //异步加载资源API
        AssetBundleRequest abr = ab.LoadAssetAsync(resName);
        yield return abr;
        //委托调用处理逻辑
        finishLoadObjectHandler(abr.asset);
    }


    //根据Type异步加载资源
    /*    public void LoadResourceAsync(string abName, string resName, System.Type type, System.Action<Object> finishLoadObjectHandler)
        {
            AssetBundle ab = LoadABPackage(abName);
            StartCoroutine(LoadRes(ab, resName, type, finishLoadObjectHandler));
        }


        private IEnumerator LoadRes(AssetBundle ab, string resName, System.Type type, System.Action<Object> finishLoadObjectHandler)
        {
            if (ab == null) yield break;
            AssetBundleRequest abr = ab.LoadAssetAsync(resName, type);
            yield return abr;
            //委托调用处理逻辑
            finishLoadObjectHandler(abr.asset);
        }*/


    //泛型加载
    /// <summary>
    /// LoadResourceAsync1提供AB包是同步加载，只是加载资源是异步
    /// </summary>
    public void LoadResourceAsync1<T>(string abName, string resName, System.Action<Object> finishLoadObjectHandler) where T : Object
    {
        AssetBundle ab = LoadABPackage(abName);
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

    //单个包卸载
    public void UnLoad(string abName)
    {
        Debug.Log(abDictionary.Count);
        if (mainfest == null || abDictionary.Count == 0)
            return;
        Debug.Log(mainfest.GetAllDependencies(abName).Length);
        if(mainfest.GetAllDependencies(abName).Length > 0)
        {

            UnloadAssetBundle(abName);

        }
        else
        {
            UnloadSingleAssetBundle(abName);
        }
        Debug.Log(abDictionary.Count);
    }

    //所有包卸载
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

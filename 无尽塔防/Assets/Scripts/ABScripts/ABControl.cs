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
            //ʹ��StreamingAssets·��ע��AB�����ʱ ��ѡcopy to streamingAssets
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
    //AB��ͬ������
    /// <summary>
    /// LoadABPackage�ṩAB����ͬ������
    /// </summary>
    private AssetBundle LoadABPackage(string abName)
    {
        AssetBundle ab;
        if (mainAB == null)
        {
            mainAB = AssetBundle.LoadFromFile(abPath + mainABName);

            mainfest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        //����manifest��ȡ���������������� �̶�API
        string[] dependencies = mainfest.GetAllDependencies(abName);
        //ѭ����������������
        for (int i = 0; i < dependencies.Length; i++)
        {
            //������ڻ��������
            if (!abDictionary.ContainsKey(dependencies[i]))
            {
                //�������������ƽ��м���
                ab = AssetBundle.LoadFromFile(abPath + dependencies[i]);
                //ע����ӽ����� ��ֹ�ظ�����AB��
                abDictionary.Add(dependencies[i], ab);
            }
        }
        //����Ŀ��� -- ͬ��ע�⻺������
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
    /// LoadResourceAsync�ṩAB�����첽���أ���ԴҲ���첽���أ��Ƽ�ʹ�ã�
    /// ֻ֧��AB���ļ���streamingAssetsĿ¼�µ�AB��
    /// abName��AB��������·����resName��AB�������Դ���ƣ�finishLoadObjectHandler�ǻص�����
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
        //����manifest��ȡ���������������� �̶�API
        string[] dependencies = mainfest.GetAllDependencies(abName);
        //ѭ����������������
        for (int i = 0; i < dependencies.Length; i++)
        {
            //������ڻ��������
            if (!abDictionary.ContainsKey(dependencies[i]))
            {
                //�������������ƽ��м���
                AssetBundleCreateRequest abcr = AssetBundle.LoadFromFileAsync(abPath + dependencies[i]);
                yield return abcr;
                //ע����ӽ����� ��ֹ�ظ�����AB��
                abDictionary.Add(dependencies[i], abcr.assetBundle);
            }
        }
        //����Ŀ��� -- ͬ��ע�⻺������
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
        //ί�е��ô����߼�
        finishLoadObjectHandler(abr.asset as T);
    }
    /// <summary>
    /// ��������Ǵ�persistentDataPath·������AB����ȡ��Դ
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
        //����manifest��ȡ���������������� �̶�API
        string[] dependencies = mainfest.GetAllDependencies(abName);
        //ѭ����������������
        for (int i = 0; i < dependencies.Length; i++)
        {
            //������ڻ��������
            if (!abDictionary.ContainsKey(dependencies[i]))
            {
                //�������������ƽ��м���
                AssetBundleCreateRequest abcr = AssetBundle.LoadFromFileAsync(Application.persistentDataPath + "/ABPackageRes/Data/" + dependencies[i]);
                yield return abcr;
                //ע����ӽ����� ��ֹ�ظ�����AB��
                abDictionary.Add(dependencies[i], abcr.assetBundle);
            }
        }
        //����Ŀ��� -- ͬ��ע�⻺������
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
        //ί�е��ô����߼�
        finishLoadObjectHandler(abr.asset as T);
    }


    /// <summary>
    /// ж�ص���AB��
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
/// ж������AB��
/// </summary>
    public void UnLoadAll()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        //ע����ջ���
        abDictionary.Clear();
        mainAB = null;
        mainfest = null;
    }
    public void UnloadAssetBundle(string assetBundleName)
    {
        UnloadDependentAssetBundles(assetBundleName); // ж��������
        UnloadSingleAssetBundle(assetBundleName); // ж��Ŀ���
    }

    private void UnloadDependentAssetBundles(string assetBundleName)
    {
        string[] dependencies = mainfest.GetAllDependencies(assetBundleName);
        for (int i = 0; i < dependencies.Length; i++)
        {
            if (abDictionary.ContainsKey(dependencies[i]))
            {
                abDictionary[dependencies[i]].Unload(false); // ж��������
                abDictionary.Remove(dependencies[i]);
                UnloadDependentAssetBundles(dependencies[i]); // �ݹ�ж�ر������İ���������
            }
        }
    }

    private void UnloadSingleAssetBundle(string assetBundleName)
    {
        if (abDictionary.ContainsKey(assetBundleName))
        {
            abDictionary[assetBundleName].Unload(false); // ж��Ŀ���
            abDictionary.Remove(assetBundleName);
        }
    }
}

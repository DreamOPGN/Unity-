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
            //ʹ��StreamingAssets·��ע��AB�����ʱ ��ѡcopy to streamingAssets
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
    //AB��ͬ������
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
    private void CallBackPackage<T>(AssetBundle ab , string resName, System.Action<Object> finishLoadObjectHandler) where T : Object
    {
        StartCoroutine(LoadRes<T>(ab, resName, finishLoadObjectHandler));
    }

/*    //==================������Դͬ�����ط�ʽ==================
    //�ṩ���ֵ��÷�ʽ �����������Եĵ��ã�Lua�Է���֧�ֲ��ã�
    #region ͬ�����ص���������

    /// <summary>
    /// ͬ��������Դ---���ͼ��� ��ֱ�� ������ʾת��
    /// </summary>
    /// <param name="abName">ab��������</param>
    /// <param name="resName">��Դ����</param>
    public T LoadResource<T>(string abName, string resName) where T : Object
    {
        //����Ŀ���
        AssetBundle ab = LoadABPackage(abName);

        //������Դ
        return ab.LoadAsset<T>(resName);
    }


    //��ָ������ ����������²�����ʹ�� ʹ��ʱ����ʾת������
    public Object LoadResource(string abName, string resName)
    {
        //����Ŀ���
        AssetBundle ab = LoadABPackage(abName);

        //������Դ
        return ab.LoadAsset(resName);
    }


    //���ò����������ͣ��ʺ϶Է��Ͳ�֧�ֵ����Ե��ã�ʹ��ʱ��ǿת����
    public Object LoadResource(string abName, string resName, System.Type type)
    {
        //����Ŀ���
        AssetBundle ab = LoadABPackage(abName);

        //������Դ
        return ab.LoadAsset(resName, type);
    }

    #endregion*/


    /// <summary>
    /// �ṩ�첽����----ע�� �������AB����ͬ�����أ�ֻ�Ǽ�����Դ���첽
    /// </summary>
    /// <param name="abName">ab������</param>
    /// <param name="resourceName">��Դ����</param>
    public void LoadResourceAsync(string abName, string resourceName, System.Action<Object> finishLoadObjectHandler)
    {
        AssetBundle ab = LoadABPackage(abName);
        //����Э�� �ṩ��Դ���سɹ����ί��
        StartCoroutine(LoadRes(ab, resourceName, finishLoadObjectHandler));
    }


    private IEnumerator LoadRes(AssetBundle ab, string resName, System.Action<Object> finishLoadObjectHandler)
    {
        if (ab == null) yield break;
        //�첽������ԴAPI
        AssetBundleRequest abr = ab.LoadAssetAsync(resName);
        yield return abr;
        //ί�е��ô����߼�
        finishLoadObjectHandler(abr.asset);
    }


    //����Type�첽������Դ
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
            //ί�е��ô����߼�
            finishLoadObjectHandler(abr.asset);
        }*/


    //���ͼ���
    /// <summary>
    /// LoadResourceAsync1�ṩAB����ͬ�����أ�ֻ�Ǽ�����Դ���첽
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
        //ί�е��ô����߼�
        finishLoadObjectHandler(abr.asset as T);
    }

    //������ж��
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

    //���а�ж��
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

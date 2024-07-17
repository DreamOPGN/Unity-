using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResManager : MonoBehaviour
{
    public static ResManager instance
    {
        get
        {
            if(Instance == null)
            {
                new GameObject("ResManager").AddComponent<ResManager>();
            }
            return Instance;
        }
    }

    private static ResManager Instance;

    //预制体缓存
    private Dictionary<string, GameObject> PrefabsDic = new Dictionary<string, GameObject>();
    //声音缓存
    private Dictionary<string, AudioClip> AudioDic = new Dictionary<string, AudioClip>();
    private void Awake()
    {
        Instance = this;
        loadAudio();
    }

    void Start()
    {

    }

    /// <summary>
    /// 加载预制体,必须存放在Resource文件夹下面
    /// </summary>
    /// <param name="prefabName">预制体名字带完整路径</param>
    /// <returns>加载完成的预制体</returns>
    private  GameObject LoadPrefabFromName(string prefabName, bool cache = true)
    {
        if (prefabName == null)
        {
            Debug.Log("改预制体名字<" + prefabName + ">不存在!请重新检查ServerRes的配置!");
            return null;
        }

        GameObject prefab = null;
        prefabName = Config.PATH_PREFABS + prefabName;

        if (!PrefabsDic.TryGetValue(prefabName, out prefab))
        {
            prefab = Resources.Load<GameObject>(prefabName);
            if (cache) PrefabsDic.Add(prefabName, prefab);
        }
        return prefab;
    }

    /// <summary>
    /// 从预制体创建对象,必须存放在Resource文件夹下面
    /// </summary>
    /// <param name="prefabName">预制体名字带完整路径</param>
    /// <returns>加载完成的对象</returns>
    public GameObject LoadGameObjectFromPrefab(string prefabName)
    {
        GameObject goPrefab = LoadPrefabFromName(prefabName);
        if (goPrefab == null)
        {
            Debug.Log("没有该预制体<" + prefabName + ">!请重新检查ServerRes的配置!");
            return null;
        }
        return GameObject.Instantiate(goPrefab);
    }

    public AudioClip LoadMusicFromFile(string name, bool cache = true)
    {
        AudioClip ac = null;
        string musicName = Config.PATH_MUSIC + name;
        if (!AudioDic.TryGetValue(musicName, out ac))
        {
            ac = Resources.Load<AudioClip>(musicName);
            if (cache) AudioDic.Add(musicName, ac);
        }
        return ac;
    }
    public AudioClip LoadSoundFromFile(string name, bool cache = true)
    {
        AudioClip ac = null;
        string musicName = Config.PATH_SOUND + name;
        if (!AudioDic.TryGetValue(musicName, out ac))
        {
            ac = Resources.Load<AudioClip>(musicName);
            if (cache) AudioDic.Add(musicName, ac);
        }
        return ac;
    }

    public void LoadAllRes()
    {
        loadAudio();
        loadPrefabs();
    }

    private void loadAudio()
    {
        //背景音乐
        LoadMusicFromFile("bgm1");
        LoadMusicFromFile("bgm2");
        LoadMusicFromFile("bgm3");
        LoadMusicFromFile("bgm4");
        LoadMusicFromFile("bgm5");
        LoadMusicFromFile("ThemeSong");
        //音效
        LoadSoundFromFile("winmusic");
        LoadSoundFromFile("tap");
        LoadSoundFromFile("squash_hmm");
        LoadSoundFromFile("snow_pea_sparkles");
        LoadSoundFromFile("siren");
        LoadSoundFromFile("shovel");
        LoadSoundFromFile("shieldhit");
        LoadSoundFromFile("seedlift");
        LoadSoundFromFile("readysetplant");
        LoadSoundFromFile("potato_mine");
        LoadSoundFromFile("points");
        LoadSoundFromFile("plant");
        LoadSoundFromFile("pause");
        LoadSoundFromFile("newspaper_rip");
        LoadSoundFromFile("newspaper_rarrgh");
        LoadSoundFromFile("losemusic");
        LoadSoundFromFile("lawnmower");
        LoadSoundFromFile("kernelpult2");
        LoadSoundFromFile("jalapeno");
        LoadSoundFromFile("hugewave");
        LoadSoundFromFile("groan");
        LoadSoundFromFile("frozen");
        LoadSoundFromFile("finalwave");
        LoadSoundFromFile("doomshroom");
        LoadSoundFromFile("coffee");
        LoadSoundFromFile("chompsoft");
        LoadSoundFromFile("chomp");
        LoadSoundFromFile("cherrybomb");
        LoadSoundFromFile("buttonclick");
        LoadSoundFromFile("bungee_scream");
        LoadSoundFromFile("awooga");
        LoadSoundFromFile("shoot");
        LoadSoundFromFile("logo");
        LoadSoundFromFile("ShootBucketheadZombie");
        LoadSoundFromFile("EatPlant");
        LoadSoundFromFile("LogoSound");
    }

    private void loadPrefabs()
    {
        LoadPrefabFromName("sun"); 
        LoadPrefabFromName("sunFlower");
        LoadPrefabFromName("SunFlower_static");
        LoadPrefabFromName("LawnCleaner");
        

    }
    
}

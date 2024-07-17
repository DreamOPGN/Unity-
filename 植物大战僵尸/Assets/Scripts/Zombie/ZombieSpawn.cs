using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;
using UnityEngine.UI;

public class ZombieSpawn : MonoBehaviour
{
    private static ZombieSpawn instance;
    public static ZombieSpawn Instance
    {
        get
        {
            if (instance == null)
                new GameObject("ZombieSpawn").AddComponent<ZombieSpawn>();
            return instance;
        }
    }
    private void Awake()
    {
        instance = this;
    }
    [System.Serializable]
    public class CurrentLevelZombieInfo    //当前关卡的僵尸信息
    {
        public GameObject zombie;
        public string zombieName;
        public string weight;
        public string combatEffectiveness;

    }

    public List<CurrentLevelZombieInfo> currentLevelZombieInfo = new List<CurrentLevelZombieInfo>();

    public JsonData currentLevelData; //获得当前关卡的数据

    public List<GameObject> zombieSurvivalList;
    public List<GameObject> zombieCurrentWaveSurvivalList;
    public GameObject zombiePool;
    public GameObject levelProgress;   //游戏僵尸进程

    public float spawnTime;
    public float startSpawnTime;
    public float totalWave;  //总波数
    public float currentWave;
    public float currentWaveZombieHealth;

    private float time;  //用于做计数器,每0.5秒计数当前波所含的僵尸的总血量，如果小于一定值，隔2秒生成下一波
    private float spawnYPosition;  //
    private float currentWaveZombieTotalHealth;
   
    private float countSameYPosition;//记录生成僵尸时相同的y值，以防重合

    private float lastSpawnYPosition;
    private float nextZombieSpawnRandomTime;
    private float mustSpawnTime;
    public bool isStartGame = false;
    public bool canTimerSpawnZombie;
    private bool canWin = true;
    private Coroutine mustSpawnIE;

    public float maxCombatEffectiveness; //当前波最大的战斗力  计算方法  maxCombatEffectiveness = k * currentWave + b;
    public float currentCombatEffectiveness; //当前波最大的战斗力  计算方法  maxCombatEffectiveness = k * currentWave + b;

    private float totalWeight;
    public float k;                      //
    public float b;


    float a;  //随机总血量的倍数[0.5 - 0.67)
    void Start()
    {
        LevelDataModel.CreateNew();
        LevelDataModel.ReadCurrentLevel();
        StartCoroutine(LoadZombieInfo());
        zombiePool = GameObject.Find("ZombiePool");
        currentWave = 0;
        levelProgress = GameObject.Find("/Canvas/GameInterfacePanel/LevelProgressSlider");
        startSpawnTime = 20f;
        ZombieShowPool();
    }

    
    void Update()
    {
        if (isStartGame)
        {
            
           StartCoroutine(Timer(startSpawnTime));
            isStartGame = false;
        }
        if(zombieCurrentWaveSurvivalList != null)
        {
            
            time += Time.deltaTime;
            if(time >= 0.5f)
            {
                currentWaveZombieHealth = 0;
                if(currentWave != totalWave)
                {
                    for (int i = 0; i < zombieCurrentWaveSurvivalList.Count; i++)
                    {
                        currentWaveZombieHealth += zombieCurrentWaveSurvivalList[i].GetComponent<Zombie>().currentHealth;
                    }
                }


                if (currentWave == totalWave)
                {
                    if(zombiePool.transform.childCount == 0 && canWin)
                    {

                        canWin = false;
                        GameInterfaceControl.Instance.WinGame();
                        AudioSourceManager.Instance.PlayWinMusic();
                    }
                    else
                    {
                        for(int i = 0; i < zombiePool.transform.childCount; i++)
                        {
                            if(zombiePool.transform.GetChild(i).gameObject.activeSelf == false)
                            {
                                Destroy(zombiePool.transform.GetChild(i).gameObject);
                            }
                        }
                    }

                }
                else if (currentWave == totalWave - 1)
                {

                    if (currentWaveZombieHealth == 0 && canTimerSpawnZombie)
                    {

                        canTimerSpawnZombie = false;
                        StopCoroutine(mustSpawnIE);
                        StartCoroutine(Timer(6f));

                    }

                }
                else  if(currentWaveZombieHealth < currentWaveZombieTotalHealth * a && canTimerSpawnZombie)
                {
                    canTimerSpawnZombie = false;
                    StopCoroutine(mustSpawnIE);
                    StartCoroutine(Timer(2f));
                    time = 0;
                    
                }
                time = 0;
            }
        }
    }
    public void GetTotalWave(string wave)
    {
        totalWave = float.Parse(wave);
    }
    public void SpawnZombie()
    {
        currentWave++;
        if (currentWave > totalWave)
        {
            currentWave = totalWave;
            return;
        }
        if (currentWave == totalWave)
        {
            AudioSourceManager.Instance.PlayFinalWaveSound();
            maxCombatEffectiveness = (currentWave * k + b) * 2.5f;
            GameObject flag = Resources.Load("ZmobiePrefabs/FlagZombie") as GameObject;
            GameObject zom = Instantiate(flag);
            zombieCurrentWaveSurvivalList.Add(zom);
            zom.GetComponent<Zombie>().Init();
            zom.transform.SetParent(zombiePool.transform);
            zom.transform.localPosition = SelectSpawnPosition();
            zom.SetActive(true);
        }
        else
        {
            maxCombatEffectiveness = currentWave * k + b;

        }

        currentWaveZombieTotalHealth = 0;
        currentCombatEffectiveness = 0;
        if(currentWave == totalWave - 1)
        {
            mustSpawnTime = 45f;
        }
        else
        {
            mustSpawnTime = UnityEngine.Random.Range(25f, 31f);
        }

        a = UnityEngine.Random.Range(0.5f,0.67f);
        mustSpawnIE = StartCoroutine(MustSpawnZombie(mustSpawnTime));

        SelectZombie();
        for (int i = 0; i < zombieCurrentWaveSurvivalList.Count; i++)
        {
            currentWaveZombieTotalHealth += zombieCurrentWaveSurvivalList[i].GetComponent<Zombie>().currentHealth;
        }
        canTimerSpawnZombie = true;

    }
    public void SelectZombie()
    {
        float accumulatedProbability = 0f;
        float[] probabilityArray =  new float[currentLevelZombieInfo.Count];
        for (int i = 0; i < currentLevelZombieInfo.Count; i++)
        {
            probabilityArray[i] = float.Parse(currentLevelZombieInfo[i].weight) / totalWeight;
        }

        while (currentCombatEffectiveness <= maxCombatEffectiveness)
        {
            float v = UnityEngine.Random.value;
            if(maxCombatEffectiveness - currentCombatEffectiveness == 1)
            {
                string name = "CommonZombie";
                GameObject zom = PoolControl.Instance.GetFromPool<GameObject>(name, currentLevelZombieInfo[0].zombie);
                zombieCurrentWaveSurvivalList.Add(zom);
                zom.GetComponent<Zombie>().Init();
                zom.transform.SetParent(zombiePool.transform);
                zom.transform.localPosition = SelectSpawnPosition();
                zom.SetActive(true);
                break;
            }

            accumulatedProbability = 0;
            for (int i = 0; i < probabilityArray.Length; i++)
            {

                accumulatedProbability += probabilityArray[i];
                if (v <= accumulatedProbability)
                {

                    string name = currentLevelZombieInfo[i].zombieName;
                    GameObject zom = PoolControl.Instance.GetFromPool<GameObject>(name, currentLevelZombieInfo[i].zombie);
                    zombieCurrentWaveSurvivalList.Add(zom);
                    zom.GetComponent<Zombie>().Init();
                    zom.GetComponent<Zombie>().speed = zom.GetComponent<Zombie>().startSpeed;
                    zom.transform.SetParent(zombiePool.transform);
                    zom.transform.localPosition = SelectSpawnPosition();
                    zom.SetActive(true);
                    currentCombatEffectiveness += float.Parse(currentLevelZombieInfo[i].combatEffectiveness);
                    break;
                }
            }
        }

    }
    private IEnumerator Timer(float time)  //计时器
    {
        yield return new WaitForSeconds(time);
        Debug.Log("到时间生成僵尸");
            zombieCurrentWaveSurvivalList.Clear();
        if (currentWave != totalWave)
        {
            SpawnZombie();
        }
    }

    private IEnumerator MustSpawnZombie(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("必须生成Zombie");
            zombieCurrentWaveSurvivalList.Clear();
        if (currentWave != totalWave)
        {
            SpawnZombie();
        }

    }
    private IEnumerator LoadZombieInfo()
    {
/*        {
            "ZombieID": "0",
        "Name": "CommonZombie",
        "Weight": "4000",
        "CombatEffectiveness": "1"
    }*/
        currentLevelData = LevelDataModel.ReadCurrentLevel();
        for (int i = 0; i < currentLevelData.Count; i++)       //从一开始即是从第一波开始
        {
            CurrentLevelZombieInfo info = new CurrentLevelZombieInfo();
            string name = currentLevelData[i][1].ToString();
            info.zombieName = name;
            info.zombie = Resources.Load("ZmobiePrefabs/" + name) as GameObject;
            info.weight = currentLevelData[i][2].ToString();
            info.combatEffectiveness = currentLevelData[i][3].ToString();
            currentLevelZombieInfo.Add(info);
        }
        for(int i = 0; i < currentLevelZombieInfo.Count; i++)
        {
            totalWeight += float.Parse(currentLevelZombieInfo[i].weight);
        }



        yield return totalWeight;
    }
    private Vector3 SelectSpawnPosition()
    {
        Vector3 startPosition ;
        lastSpawnYPosition = spawnYPosition;
        switch ((int)UnityEngine.Random.Range(1, 6))
        {
            case 1:
                spawnYPosition = -2.5f;
                break;
            case 2:
                spawnYPosition = -0.8f;
                break;
            case 3:
                spawnYPosition = 0.75f;
                break;
            case 4:
                spawnYPosition = 2.5f;
                break;
            case 5:
                spawnYPosition = 4.2f;
                break;
        }
        if(lastSpawnYPosition == spawnYPosition)
        {
            countSameYPosition++;
            startPosition = new Vector3(10.3f + countSameYPosition, spawnYPosition, 0);
        }
        else
        {
            startPosition = new Vector3(9.3f, spawnYPosition, 0);
        }
        return startPosition;
    }
    private Vector2 minPosition = new Vector2(9f, -1f);
    private Vector2 maxPosition = new Vector2(13f, 4f);
    public void ZombieShowPool()
    {

        for (int i = 0; i < currentLevelZombieInfo.Count; i++)
        {
            int a = (int)UnityEngine.Random.Range(1f, 3f);
                for (int j = 0; j < a; j++)
                {
                    Vector3 ranDomPosition = new Vector3(UnityEngine.Random.Range(minPosition.x, maxPosition.x), UnityEngine.Random.Range(minPosition.y, maxPosition.y), 64);
                    GameObject zom = Instantiate(currentLevelZombieInfo[i].zombie);
                    zom.transform.SetParent(zombiePool.transform);
                    zom.transform.localPosition = ranDomPosition;
                    zom.GetComponent<Zombie>().speed = 0;
                    zom.GetComponent<Animator>().enabled = false;
                    zom.SetActive(true);
                }
            
        }
    }
}



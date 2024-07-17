using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

//敌人管理器
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    private JsonData data;
    
    private List<GameObject> enemyPrefabsList = new List<GameObject>();  //存储所有敌人的预制体
    [HideInInspector]
    public List<GameObject> enemyList = new List<GameObject>();
    private Transform[] pathArray = new Transform[17];
    private Transform path;  //路径节点
    private UIManager uIManager;
    //存储所有敌人的实例
    private bool isStop;  //是否停止生成敌人
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
        uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        Init();

    }
    //初始化
    void Init()
    {
        for(int i = 1; i < 11; i++)
        {
            GameObject go = Resources.Load<GameObject>("EnemyPrefabs/bullhound_" + i);
            enemyPrefabsList.Add(go);
            
        }
        //获取路径点
        path = GameObject.Find("Path").transform;
        for(int i = 0; i < 17; i++)
        {
            pathArray[i] = path.GetChild(i);
            
        }
        path.gameObject.SetActive(false);
    }
    public void GetData(JsonData d)
    {
        data = d;
        
    }

    
    void Update()
    {
        if (GameData.instance.allEnemyCount <= 0)
        {
            isStop = true;
            StopCoroutine(CreateEnemy());
        }
    }
    public void StartEnemy()
    {
        StartCoroutine(CreateEnemy());
    }
    IEnumerator CreateEnemy()
    {
        while (!isStop)
        {
            if(GameData.instance.enemyCount > 0)
            {
                //更新UI
                GameData.instance.enemyCount--;
                uIManager.UpdateBattleData();
                //创建敌人

                int index = UnityEngine.Random.Range(0, enemyPrefabsList.Count - 1);
                GameObject enemy = GameObject.Instantiate(enemyPrefabsList[index]);
                enemy.AddComponent<Enemy>().SetData(data[index]);
                enemy.GetComponent<Enemy>().SetPathData(pathArray);
                //设置敌人的位置以及旋转
                enemy.transform.position = pathArray[0].position;
                enemy.transform.eulerAngles = pathArray[0].eulerAngles;
                enemyList.Add(enemy);
            }

            yield return new WaitForSeconds(3f);
        }
    }

    //停止创建敌人，当水晶被推掉时，敌人数量还未生成完
    public void Stop()
    {
        isStop = true;
        //所有的敌人停止移动
        for(int i = 0; i < enemyList.Count; i++)
        {
            Enemy enemy = enemyList[i].GetComponent<Enemy>();
            enemy.Stop();
        }
        //清除怪物list数据
        enemyList.Clear();
    }
}

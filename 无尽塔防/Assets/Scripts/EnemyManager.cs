using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

//���˹�����
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    private JsonData data;
    
    private List<GameObject> enemyPrefabsList = new List<GameObject>();  //�洢���е��˵�Ԥ����
    [HideInInspector]
    public List<GameObject> enemyList = new List<GameObject>();
    private Transform[] pathArray = new Transform[17];
    private Transform path;  //·���ڵ�
    private UIManager uIManager;
    //�洢���е��˵�ʵ��
    private bool isStop;  //�Ƿ�ֹͣ���ɵ���
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
        uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        Init();

    }
    //��ʼ��
    void Init()
    {
        for(int i = 1; i < 11; i++)
        {
            GameObject go = Resources.Load<GameObject>("EnemyPrefabs/bullhound_" + i);
            enemyPrefabsList.Add(go);
            
        }
        //��ȡ·����
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
                //����UI
                GameData.instance.enemyCount--;
                uIManager.UpdateBattleData();
                //��������

                int index = UnityEngine.Random.Range(0, enemyPrefabsList.Count - 1);
                GameObject enemy = GameObject.Instantiate(enemyPrefabsList[index]);
                enemy.AddComponent<Enemy>().SetData(data[index]);
                enemy.GetComponent<Enemy>().SetPathData(pathArray);
                //���õ��˵�λ���Լ���ת
                enemy.transform.position = pathArray[0].position;
                enemy.transform.eulerAngles = pathArray[0].eulerAngles;
                enemyList.Add(enemy);
            }

            yield return new WaitForSeconds(3f);
        }
    }

    //ֹͣ�������ˣ���ˮ�����Ƶ�ʱ������������δ������
    public void Stop()
    {
        isStop = true;
        //���еĵ���ֹͣ�ƶ�
        for(int i = 0; i < enemyList.Count; i++)
        {
            Enemy enemy = enemyList[i].GetComponent<Enemy>();
            enemy.Stop();
        }
        //�������list����
        enemyList.Clear();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class GameData : MonoBehaviour
{

    private JsonData enemyData;
    private JsonData levelData = new JsonData();
    private JsonData towerData;
    public static GameData instance;
    public int levelID = 1;
    public float coins;
    public int allEnemyCount;
    public int enemyCount;   //ʣ��Ҫ���ɹ��������
    public int killCount;
    public float time;
    public float homeHP;

    private void Awake()
    {
        instance = this;


/*        if (!File.Exists(Config.CURRENTLEVELDATA_PATH))
        {
            JsonData newData = new JsonData();
            newData["CurrentLevel"] = 1.ToString();
            File.WriteAllText(Config.CURRENTLEVELDATA_PATH, newData.ToJson());
        }*/
        
        if (File.Exists(Config.DownLOADAB_PATH + "/Data.manifest"))   //���Ψһ�ɶ�д·��������Ϸ���ݵ�AB���������Ψһ�ɶ�д·����������
        {

            ABControl.Instance.LoadResourceFrompersistentDataPathAsync<TextAsset>("gamedata", "LevelData", GetLevelIDData);
            //ABControl.Instance.LoadResourceFrompersistentDataPathAsync<TextAsset>("gamedata", "EnemyData", GetEnemyData);
        }
        else    //����ʹ�StreamingAssets·����ȡAB��Ĭ������
        {

            ABControl.Instance.LoadResourceAsync<TextAsset>("ab/data", "LevelData", GetDefaultLevelData);
        }

    }
    private void Start()
    {

    }

    public void GetLevelIDData(Object obj)
    {
        levelData = JsonMapper.ToObject(obj.ToString());
        SetLevelData();
        ABControl.Instance.LoadResourceFrompersistentDataPathAsync<TextAsset>("gamedata", "EnemyData", GetEnemyData);
    }
    public void GetEnemyData(Object obj)
    {
        //Debug.Log(obj.ToString());

        enemyData = JsonMapper.ToObject(obj.ToString());
        EnemyManager.instance.GetData(enemyData);
        ABControl.Instance.LoadResourceFrompersistentDataPathAsync<TextAsset>("gamedata", "TowerData", GetTowerData);
        //File.WriteAllText(Config.LEVELDATA_PATH, obj.ToString());
    }
    public void GetTowerData(Object obj)
    {
        towerData = JsonMapper.ToJson(obj);
    }
    public void GetDefaultLevelData(Object obj)
    {
        levelData = JsonMapper.ToObject(obj.ToString());
        SetLevelData();
        ABControl.Instance.LoadResourceAsync<TextAsset>("ab/data", "EnemyData", GetDefaultEnemyData);
    }
    public void GetDefaultEnemyData(Object obj)
    {
        enemyData = JsonMapper.ToObject(obj.ToString());
        EnemyManager.instance.GetData(enemyData);
        ABControl.Instance.LoadResourceAsync<TextAsset>("ab/data", "TowerData", GetDefaultTowerData);
        
    }
    public void GetDefaultTowerData(Object obj)
    {
        towerData = JsonMapper.ToObject(obj.ToString());

    }

    public void SetCurrentLevelID()
    {
        if (!File.Exists(Config.CURRENTLEVELDATA_PATH))
        {
            levelID = 1;
            JsonData newData = new JsonData();
            newData["CurrentLevel"] = 1.ToString();
            File.WriteAllText(Config.CURRENTLEVELDATA_PATH, newData.ToJson());
        }
        else
        {
            JsonData d = new JsonData();
            d = GetCurrentLevelData();
            if(d.ToString() == "Uninitialized JsonData")
            {
                levelID = 1;
                JsonData newData = new JsonData();
                newData["CurrentLevel"] = 1.ToString();
                File.WriteAllText(Config.CURRENTLEVELDATA_PATH, newData.ToJson());
            }
            else
            {
                levelID = int.Parse(d[0].ToString());
            }

        }

    }
    //{"Level":"1","StartGold":"1000","HomeHP":"50","EnemyCount":"20"}
    public void SetLevelData()
    {
/*        if (!File.Exists(Config.LEVELDATA_PATH))
        {
            return;
        }*/
        killCount = 0;
        time = 0;
        SetCurrentLevelID();
        //JsonData levelData = new JsonData();
        //levelData = this.levelData;
        
        for (int i = 0; i < levelData.Count; i++)
        {
            if (int.Parse(levelData[i][0].ToString()) == levelID)
            {
                JsonData data = levelData[i];
                this.levelID = int.Parse(data[0].ToString());
                this.coins = int.Parse(data[1].ToString());

                this.allEnemyCount = int.Parse(data[3].ToString());
                this.enemyCount = allEnemyCount;
                homeHP = int.Parse(data[2].ToString());
            }
        }

    }
/*    public JsonData GetEnemyData()
    {

        string json = File.ReadAllText(Config.ENEMYDATA_PATH);
        
        return JsonMapper.ToObject(json);
    }*/
    public JsonData GetCurrentLevelData()
    {
        string json = File.ReadAllText(Config.CURRENTLEVELDATA_PATH);

        return JsonMapper.ToObject(json);
    }
/*    public JsonData GetLevelIDData()
    {
        string json = File.ReadAllText(Config.LEVELDATA_PATH);

        return JsonMapper.ToObject(json);
    }*/


    public void WriteLevelIDData()
    {
        JsonData oldata = GetCurrentLevelData();
        JsonData newData = new JsonData();
        newData["CurrentLevel"] = (levelID + 1).ToString();
        oldata = newData;
        File.WriteAllText(Config.CURRENTLEVELDATA_PATH, oldata.ToJson());

    }

}

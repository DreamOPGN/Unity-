using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public static class LevelDataModel
{
    public static void CreateNew()
    {
        //[{"Level":"1-1","Wave1":[{"Zombie":"CommonZombie","Count":1}]},{"Level":"1-2","Wave1":[{"Zombie":"FlagZombie","Count":2}]}]示例
        
        if (!File.Exists(Config.LevelDataPath))
        {
            JsonData data = new JsonData();
            JsonData d = new JsonData();
            d["Level"] = "1";
            d["TotalWave"] = "10";
            JsonData row = new JsonData();
            row["Zombie"] = "0";
            row["Zombie_1"] = "1";
            d["ZombiePool"] = row;

            data.Add(d);

            File.WriteAllText(Config.LevelDataPath, data.ToJson());
        }

        if (!File.Exists(Config.PlayerDataPath))
        {
            JsonData d = new JsonData();
            d["CurrentLevel"] = "1";
            File.WriteAllText(Config.PlayerDataPath, d.ToJson());
        }

        if (!File.Exists(Config.PlayerHavePlantsDataPath))
        {
            JsonData d = new JsonData();
            d["1"] = "SunFlower";
            d["2"] = "PeaShooter";
            d["3"] = "WallNut";
            d["4"] = "CherryBomb";
            d["5"] = "SnowPea";
            d["6"] = "Threepeater";
            File.WriteAllText(Config.PlayerHavePlantsDataPath, d.ToJson());
        }
    }
    public static JsonData ReadAllLevelData()
    {
        string json = File.ReadAllText(Config.LevelDataPath);
        return JsonMapper.ToObject(json);
    }

    public static JsonData ReadPlayerData()
    {
        string json = File.ReadAllText(Config.PlayerDataPath);
        return JsonMapper.ToObject(json);
    }
    public static JsonData ReadZombieInfo()
    {
        TextAsset info = Resources.Load<TextAsset>("Json/PVZZombieInfo");
        JsonData data = JsonMapper.ToObject(info.text);
        return data;
/*        string json = File.ReadAllText(Config.ZombieInfo);
        return JsonMapper.ToObject(json);*/
    }
    public static JsonData ReadCurrentLevel()
    {
        JsonData d = new JsonData();
        JsonData data = new JsonData();
        JsonData a = ReadAllLevelData();  //读取所有关卡信息
        JsonData b = ReadPlayerData();    //读取玩家当前关卡
        JsonData c = ReadZombieInfo();    //读取僵尸信息
        for (int i = 0; i < a.Count; i++)
        {
            if (a[i][0].ToJson() == b[0].ToJson())
            {

                d = a[i][2];
                ZombieSpawn.Instance.GetTotalWave(a[i][1].ToString());
            }
        }
        for (int i = 0; i < d.Count; i++)
        {
            for(int j = 0; j < c.Count; j++)
            {
                if (c[j][0].ToJson() == d[i].ToJson())
                {

                    data.Add(c[j]);

                }
            }
        }
        Debug.Log(data.ToJson());
        return data;
    }

    public static JsonData ReadPlayerHavePlants()
    {
        string json = File.ReadAllText(Config.PlayerHavePlantsDataPath);
        return JsonMapper.ToObject(json);
    }

    public static void AddPlamt(string name)
    {
       
        JsonData d = ReadPlayerHavePlants();
        string a = (d.Count + 1).ToString();
        d[a] = name;


        File.WriteAllText(Config.PlayerHavePlantsDataPath, d.ToJson());
    }
    public static void AddLevel()
    {
        JsonData d = ReadPlayerData();
        string a = d[0].ToString();
        int b = int.Parse(a);
        d["CurrentLevel"] = (b + 1).ToString();
        File.WriteAllText(Config.PlayerDataPath, d.ToJson());
    }
}

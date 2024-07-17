using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using LitJson;
using Mono.Data.Sqlite;

/*[System.Serializable]
public class ItemData
{
    public StudentDataItems studentDataItems;
    public int id;            //道具id
    public string name;       //道具名字
    public int type;          //道具类型(0:武器，1：材料)
    public int level;         //道具等级
    public string dec;        //道具描述
}

[System.Serializable]
public class StudentDataItems
{
    //public ItemData[] itemDataInfoList;
}*/

public class DataModle : MonoBehaviour
{
    public static DataModle instance;
    public JsonData itemInfo;   //记录本地道具信息
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        ReadItemInfo();

        //GetSqliteWeaponCreateData(1);
        //GetSqliteBagData(1);
    }
    public void ReadItemInfo()
    {
        string json = File.ReadAllText(Config.ITEMDATA_PATH);
        JsonData jsonData = JsonMapper.ToObject(json);
        itemInfo = jsonData;
    }
    public JsonData GetSqliteBagData(string id)     //
    {
        SqliteDataReader reader = SqliteManager.instance.SelectItem(id);
        //最终返回的数据
        JsonData d = new JsonData();

        //获取sqlite玩家所拥有的数据，结合本地道具配置文件
        while (reader.Read())
        {
            for (int i = 0; i < itemInfo.Count; i++)
            {
                if(int.Parse((string)itemInfo[i][0]) == (int)reader["ItemID"])
                {
                    JsonData row = itemInfo[i];
                    row["ItemCount"] = reader["ItemCount"].ToString();

                    d.Add(row);
                }
            }
        }
        return d;
    }
    //检查是否还装的下道具
    public bool CheckFullBag()
    {
        int nullBagCount = 30;
        JsonData d = GetSqliteBagData(Config.PLAYER_ID);

        for (int i = 0; i < d.Count; i++)  //生成有道具的格子
        {

            int maxCount = int.Parse(d[i][5].ToString());   //最大数量
            int haveCount = int.Parse(d[i][9].ToString());  // 拥有的数量
            if (haveCount > maxCount)
            {
                while (haveCount >= maxCount)
                {
                    haveCount -= maxCount;
                    nullBagCount--;
                }
                if (haveCount > 0)
                {
                    nullBagCount--;
                }

            }
            else
            {
                nullBagCount--;
            }

        }
        if(nullBagCount > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public JsonData GetSqliteWeaponCreateData(int id)   //c从Sqlite获得可以进行武器制作的数据id
    {

        //最终返回的数据
        JsonData d = new JsonData();

        int[] wc = new int[3] { 1, 2, 3 };


        for (int i = 0; i < itemInfo.Count; i++)
        {
            for (int j = 0; j < wc.Length; j++)
            {
                if (int.Parse((string)itemInfo[i][0]) == (int)wc[j])
                {


                   d.Add(itemInfo[i]);
                }
            }
            
        }
        
        return d;
    }
    public int GetItemCount(string id, int itemid)    //根据点击的武器制作传过来的Id获取该物品数量
    {
        SqliteDataReader reader = SqliteManager.instance.SelectItem(id);
        //最终返回的数据
        JsonData d = new JsonData();
        int count = 0;
        //获取sqlite玩家所拥有的数据，结合本地道具配置文件
        while (reader.Read())
        {

                if (itemid == (int)reader["ItemID"])
                {
                    count = (int)reader["ItemCount"];
                }
            
        }
        return count;
    }
    public JsonData GetSingleItemInfo(int itemid)    //根据传进来的单一物品id返回该jsondata数据
    {

        //最终返回的数据
        JsonData d = new JsonData();
        for (int i = 0; i < itemInfo.Count; i++)
        {
            if (int.Parse((string)itemInfo[i][0]) == itemid)
            {
                d = itemInfo[i];
            }
        }
        return d;
    }

}

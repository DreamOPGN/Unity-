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
    public int id;            //����id
    public string name;       //��������
    public int type;          //��������(0:������1������)
    public int level;         //���ߵȼ�
    public string dec;        //��������
}

[System.Serializable]
public class StudentDataItems
{
    //public ItemData[] itemDataInfoList;
}*/

public class DataModle : MonoBehaviour
{
    public static DataModle instance;
    public JsonData itemInfo;   //��¼���ص�����Ϣ
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
        //���շ��ص�����
        JsonData d = new JsonData();

        //��ȡsqlite�����ӵ�е����ݣ���ϱ��ص��������ļ�
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
    //����Ƿ�װ���µ���
    public bool CheckFullBag()
    {
        int nullBagCount = 30;
        JsonData d = GetSqliteBagData(Config.PLAYER_ID);

        for (int i = 0; i < d.Count; i++)  //�����е��ߵĸ���
        {

            int maxCount = int.Parse(d[i][5].ToString());   //�������
            int haveCount = int.Parse(d[i][9].ToString());  // ӵ�е�����
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
    public JsonData GetSqliteWeaponCreateData(int id)   //c��Sqlite��ÿ��Խ�����������������id
    {

        //���շ��ص�����
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
    public int GetItemCount(string id, int itemid)    //���ݵ��������������������Id��ȡ����Ʒ����
    {
        SqliteDataReader reader = SqliteManager.instance.SelectItem(id);
        //���շ��ص�����
        JsonData d = new JsonData();
        int count = 0;
        //��ȡsqlite�����ӵ�е����ݣ���ϱ��ص��������ļ�
        while (reader.Read())
        {

                if (itemid == (int)reader["ItemID"])
                {
                    count = (int)reader["ItemCount"];
                }
            
        }
        return count;
    }
    public JsonData GetSingleItemInfo(int itemid)    //���ݴ������ĵ�һ��Ʒid���ظ�jsondata����
    {

        //���շ��ص�����
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

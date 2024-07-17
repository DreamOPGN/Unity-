using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class BagControl : MonoBehaviour
{
    //背包界面组件
    private Button bag_close_btn;
    private int max_itemCount = 30;
    private int canSpawnNUllItemCount;   
    private Transform itemParent;
    void Start()
    {

        //SqliteManager.instance.InsertOrUpdateItem(1.ToString(), 4, 30);
        //Init();
    }

    // Update is called once per frame
    public void Init()
    {
        //将之前的物品删除

        canSpawnNUllItemCount = max_itemCount;
        itemParent = transform.Find("bagScroll/Viewport/Content");
        for (int i = 0; i < itemParent.childCount; i++)
        {
            Destroy(itemParent.GetChild(i).gameObject);
        }
        JsonData d = DataModle.instance.GetSqliteBagData(Config.PLAYER_ID);
        //SqliteManager.instance.InsertOrUpdateItem(1.ToString(), 4, 30);
        //[{"ID":"2",
        //"Name":"\u94C1\u5200",
        //"Type":"0",
        //"Level":"2",
        //"Icon":"icon_wuqi_dao",
        //"Max_ShowCount":"1",
        //"Describe":"\u4F7F\u7528\u751F\u94C1\u548C\u9EBB\u7EF3\u5236\u4F5C\u800C\u6210\uFF0C\u7CBE\u51C6\u5EA6\u4E00\u822C\uFF0C\u4F24\u5BB3\u9002\u4E2D",
        //"ItemCount":"50"}]

        for (int i = 0; i < d.Count; i++)  //生成有道具的格子
        {

            int maxCount = int.Parse(d[i][5].ToString());   //最大数量
            int haveCount = int.Parse(d[i][9].ToString());  // 拥有的数量
            if(haveCount > maxCount)
            {
                while (haveCount >= maxCount)
                {
                    haveCount -= maxCount;
                    SpawnBagItem(d[i], maxCount);
                }
                if(haveCount > 0)
                {
                    SpawnBagItem(d[i], haveCount);
                }

            }
            else
            {
                SpawnBagItem(d[i]);
            }
           
        }
        for (int i = 0; i < canSpawnNUllItemCount; i++)   //生成空道具的格子
        {
            SpawnBagItem();
        }
    }

    public void SpawnBagItem(JsonData d)
    {
        if (canSpawnNUllItemCount == 0)
        {

            SqliteManager.instance.canAddItem = false;
            return;
        }

        canSpawnNUllItemCount--;
        GameObject im = Resources.Load<GameObject>("Prefabs/Item");
        GameObject item = Instantiate(im, itemParent);
        item.name = "Item";
        
        item.GetComponent<BagItem>().BagInit(d);
    }
    public void SpawnBagItem(JsonData d, int count)
    {
        if (canSpawnNUllItemCount == 0)
        {
            SqliteManager.instance.canAddItem = false;
            return;
        }

        canSpawnNUllItemCount--;
        GameObject im = Resources.Load<GameObject>("Prefabs/Item");
        GameObject item = Instantiate(im, itemParent);
        item.name = "Item";

        item.GetComponent<BagItem>().BagInit(d, count);
    }
    public void SpawnBagItem()    //生成空格子
    {
        if (canSpawnNUllItemCount == 0)
            return;
        GameObject im = Resources.Load<GameObject>("Prefabs/Item");
        GameObject item = Instantiate(im, itemParent);
        item.name = "Item";
    }
}

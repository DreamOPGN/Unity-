using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopControl : MonoBehaviour
{
    private BagControl bagControl;
    private UIControl uiControl;
    private Transform shopParent;
    void Start()
    {





    }
    public void InitShopGroup()
    {
        shopParent = transform.Find("shopGroup");
        uiControl = transform.parent.GetComponent<UIControl>();
        for (int i = 0; i < shopParent.childCount; i++)
        {
            Destroy(shopParent.GetChild(i).gameObject);
        }
        SqliteDataReader read = SqliteManager.instance.SelectShopItem(Config.PLAYER_ID);
        while (read.Read())
        {
            SpawnShopItem(read["Name"].ToString(), (int)read["ItemID"], 1);
        }
        
    }
    public void SpawnShopItem(string name, int id , int count)
    {
        GameObject sg = Resources.Load<GameObject>("Prefabs/ShopItem");
        GameObject shopItem = Instantiate(sg, shopParent);
        shopItem.name = "shopItem";
        shopItem.transform.Find("name").GetComponent<Text>().text = name + " + " +count;
        shopItem.GetComponent<ShopItem>().Init(id, count, uiControl);
    }

}

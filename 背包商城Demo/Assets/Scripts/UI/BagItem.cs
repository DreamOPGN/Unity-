using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using UnityEngine.U2D;
using System;

public class BagItem : MonoBehaviour
{
    public JsonData data;
    //获取Item组件
    private int id;
    private int type;
    private int level;
    public Image icon;

    public Text itemName;
    public Text itemCount;
    private GameObject bg_kuang;
    void Start()
    {
        icon = transform.Find("icon").GetComponent<Image>();
        itemName = transform.Find("name").GetComponent<Text>();
        itemCount = transform.Find("num").GetComponent<Text>();
        bg_kuang = transform.Find("bg_kuang").gameObject;
    }

    public void BagInit(JsonData d)     //拥有数量<=最大可容纳数量
    {
        //[{ "ID":"2","Name":"\u94C1\u5200","Type":"0","Level":"2",
        //"Icon":"icon_wuqi_dao","Max_ShowCount":"1",
        //"Describe":"\u4F7F\u7528\u751F\u94C1\u548C\u9EBB\u7EF3\u5236\u4F5C\u800C\u6210\uFF0C\u7CBE\u51C6\u5EA6\u4E00\u822C\uFF0C\u4F24\u5BB3\u9002\u4E2D",
        //"ItemCount":"50"}]

        //获取组件
        icon = transform.Find("icon").GetComponent<Image>();
        itemName = transform.Find("name").GetComponent<Text>();
        itemCount = transform.Find("num").GetComponent<Text>();
        bg_kuang = transform.Find("bg_kuang").gameObject;

        data = d;
        id = int.Parse(d[0].ToString());
        type = int.Parse(d[2].ToString());
        level = int.Parse(d[3].ToString());
        itemName.text = d[1].ToString();
        itemCount.text = d[9].ToString();
        Color co = transform.GetComponent<Image>().color;
        transform.GetComponent<Image>().color = new Color(co.r, co.g, co.b, 1f);

        if (Resources.Load("Sprites/" + d[4].ToString()) != null)
        {
            icon.sprite = Resources.Load<Sprite>("Sprites/" + d[4].ToString());
        }

        Color c = icon.color;
        icon.color = new Color(c.r, c.g, c.b, 1f);


        //加载图集不知为何总崩溃解决不了
        /*SpriteAtlas icons = Resources.Load<SpriteAtlas>("Item");
        if(icons != null)
        {
            icon.sprite = icons.GetSprite("icon_wuqi_dao");
        }*/

    }
    public void BagInit(JsonData d, int count)         //拥有数量>最大可容纳数量
    {
        
        //获取组件
        icon = transform.Find("icon").GetComponent<Image>();
        itemName = transform.Find("name").GetComponent<Text>();
        itemCount = transform.Find("num").GetComponent<Text>();
        bg_kuang = transform.Find("bg_kuang").gameObject;

        data = d;
        id = int.Parse(d[0].ToString());
        type = int.Parse(d[2].ToString());
        level = int.Parse(d[3].ToString());
        itemName.text = d[1].ToString();
        itemCount.text = count.ToString();
        Color co = transform.GetComponent<Image>().color;
        transform.GetComponent<Image>().color = new Color(co.r, co.g, co.b, 1f);

        if (Resources.Load("Sprites/" + d[4].ToString()) != null)
        {
            icon.sprite = Resources.Load<Sprite>("Sprites/" + d[4].ToString());
        }

        Color c = icon.color;
        icon.color = new Color(c.r, c.g, c.b, 1f);
    }


}

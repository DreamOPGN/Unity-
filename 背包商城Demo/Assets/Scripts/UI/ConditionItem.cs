using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class ConditionItem : MonoBehaviour
{
    private JsonData data;
    private Image icon;
    private Text numCount;
    public bool isCanCreate = true;          //用来记录是否满足制作条件
    private string itemName;
    private int bagItemCount = 0;            //背包有这个材料的数量
    private int singleItemCount = 0;         //每个材料所需要的数量
    private Color _color;                    //记录满足条件字体的颜色
    public int _id;                          //记录这个条件道具的id
    public int _spendCount;                   //记录如果生成要消耗多少数量的道具

    public void Init(int id, int count)
    {
        _id = id;
        _color = transform.parent.parent.Find("name").GetComponent<Text>().color;
        icon = transform.Find("icon").GetComponent<Image>();
        numCount = transform.Find("needNum").GetComponent<Text>();
        isCanCreate = true;
        JsonData d = DataModle.instance.GetSingleItemInfo(id);
        if (Resources.Load("Sprites/" + d[4].ToString()) != null)
        {
           transform.Find("icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + d[4].ToString());
        }

        bagItemCount = DataModle.instance.GetItemCount(Config.PLAYER_ID, id);
        itemName = d[1].ToString();
        singleItemCount = count;
        _spendCount = count;
        numCount.text = "(" + bagItemCount + " / " + count + ") " + itemName;
        if (bagItemCount < count)
        {
            numCount.color = Color.red;
            isCanCreate = false;
        }
    }
    public void InitText(int createNum)
    {
        int total = singleItemCount * createNum;   //总共需要的材料数量
        _spendCount = total;
        bagItemCount = DataModle.instance.GetItemCount(Config.PLAYER_ID, _id);
        numCount.text = "(" + bagItemCount + " / " + total + ") " + itemName;
        if (bagItemCount < total)
        {
            numCount.color = Color.red;
            isCanCreate = false;
        }
        else
        {
            numCount.color = _color;
            isCanCreate = true;
        }
    }
}

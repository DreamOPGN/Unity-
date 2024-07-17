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
    public bool isCanCreate = true;          //������¼�Ƿ�������������
    private string itemName;
    private int bagItemCount = 0;            //������������ϵ�����
    private int singleItemCount = 0;         //ÿ����������Ҫ������
    private Color _color;                    //��¼���������������ɫ
    public int _id;                          //��¼����������ߵ�id
    public int _spendCount;                   //��¼�������Ҫ���Ķ��������ĵ���

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
        int total = singleItemCount * createNum;   //�ܹ���Ҫ�Ĳ�������
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

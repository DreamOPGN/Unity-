using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;

public class WeaponCreateControl : MonoBehaviour
{
    //武器制作界面组件
    private Button create_Btn;
    private Button add_Btn;
    private Button reduce_Btn;
    private Button weaponcreate_close_btn;
    private Image reduceImage;
    private Text reduceText;
    private Text createNumText;

    //背包数据
    private int max_itemCount = 5;
    private int canSpawnCount;
    private Transform itemParent;
    private int currentItemID;                 //记录当前点击制作武器的ID
    //细节制作
    private Transform details;
    private Image icon;
    private Text itemName;
    private Text itemNum;
    private Text dec;                         //武器的描述
    private Transform conditionParent;
    //武器制作数据
    private int createNum = 1;
    private bool isCanCreate;                 //用来监视是否满足制作条件
    public Transform LastHightLightItem;

    void Start()
    {
        canSpawnCount = max_itemCount;
        //获取武器界面制作组件

        create_Btn = transform.Find("details/create_btn").GetComponent<Button>();
        add_Btn = transform.Find("details/add_btn").GetComponent<Button>();
        reduce_Btn = transform.Find("details/reduce_btn").GetComponent<Button>();
        reduceImage = reduce_Btn.transform.Find("reduceImage").GetComponent<Image>();
        reduceText = reduce_Btn.transform.Find("reduceText").GetComponent<Text>();
        createNumText = create_Btn.transform.Find("createNum").GetComponent<Text>();
        itemParent =  transform.Find("Scroll View/Viewport/Content");
        icon = details.Find("icon").GetComponent<Image>();
        itemName = details.Find("name").GetComponent<Text>();
        itemNum = details.Find("num").GetComponent<Text>();
        dec = details.Find("dec").GetComponent<Text>();
        conditionParent = details.Find("conditions");
        //添加事件
        create_Btn.onClick.AddListener(Click_Create_Btn);
        add_Btn.onClick.AddListener(Click_Add_Btn);
        reduce_Btn.onClick.AddListener(Click_Reduce_Btn);

        //处理开始显示的界面
        //InitDetailCountChange();
        Init();
    }
    
    public void InitDetailCountChange()  //处理增加和减少那个制作数量和消耗材料的变化
    {
        Color color = reduceImage.color;
        if (createNum == 1)
        {
            
            color.a = 0.5f;
            reduceImage.color = color;
            reduceText.color = color;
        }
        else
        {
            color.a = 1f;
            reduceImage.color = color;
            reduceText.color = color;
        }
        createNumText.text = "制作x" + createNum;
        for (int i = 0; i < conditionParent.childCount; i++)
        {
            conditionParent.GetChild(i).GetComponent<ConditionItem>().InitText(createNum);
        }
    }
    public void Init()
    {
        details = transform.Find("details");
        details.gameObject.SetActive(false);
        itemParent = transform.Find("Scroll View/Viewport/Content");
        canSpawnCount = max_itemCount;
        //将之前的物品删除
        for (int i = 0; i < itemParent.childCount; i++)
        {
            Destroy(itemParent.GetChild(i).gameObject);
        }
        JsonData d = DataModle.instance.GetSqliteWeaponCreateData(1);
        for (int i = 0; i < d.Count; i++)         //生成有道具的格子
        {
            SpawnWeaponCreateItem(d[i]);
        }
        for (int i = 0; i < canSpawnCount; i++)   //生成空道具的格子
        {
            SpawnWeaponCreateItem();
        }
    }

    public void SpawnWeaponCreateItem(JsonData d)
    {
        if (canSpawnCount == 0)
            return;
        canSpawnCount--;
        GameObject im = Resources.Load<GameObject>("Prefabs/WeaponCreateItem");
        GameObject item = Instantiate(im, itemParent);
        item.name = "Item";

        item.GetComponent<WeaponCreateItem>().WeaponCreateInit(d);
    }
    public void SpawnWeaponCreateItem()
    {
        
        GameObject im = Resources.Load<GameObject>("Prefabs/WeaponCreateItem");
        GameObject item = Instantiate(im, itemParent);
        item.name = "Item";
    }
    public void Click_Create_Btn()
    {
        isCanCreate = true;
        for (int i = 0; i < conditionParent.childCount; i++)
        {
            if(conditionParent.GetChild(i).GetComponent<ConditionItem>().isCanCreate == false)
            {
                isCanCreate = false;
                return;
            }
        }
        if (isCanCreate)
        {
            Debug.Log("可以制作");
            Create();
        }
    }
    //执行制作，消耗道具并获得道具
    public void Create()
    {
        for (int i = 0; i < conditionParent.childCount; i++)
        {
            ConditionItem conditionItem = conditionParent.GetChild(i).GetComponent<ConditionItem>();
            SqliteManager.instance.InsertOrUpdateItem(1.ToString(), conditionItem._id, -conditionItem._spendCount);      //减少条件道具
            conditionItem.InitText(createNum); 
        }
        SqliteManager.instance.InsertOrUpdateItem(1.ToString(), currentItemID, createNum);                               //增加制作成功的道具
        itemNum.text = "拥有:" + DataModle.instance.GetItemCount(Config.PLAYER_ID, currentItemID).ToString();
    }
    public void Click_Add_Btn()
    {
        createNum++;
        InitDetailCountChange();
    }
    public void Click_Reduce_Btn()
    {
        if (createNum == 1)
            return;
        createNum--;
        InitDetailCountChange();
    }
    //改变显示哪个道具显示框
    public void ChangeHightLightItem(Transform Item)
    {


        if(LastHightLightItem == null)
        {
            LastHightLightItem = transform.Find("Scroll View/Viewport/Content").GetChild(0);
        }
        LastHightLightItem.Find("bg_kuang").gameObject.SetActive(false);
        LastHightLightItem = Item;
        Item.Find("bg_kuang").gameObject.SetActive(true);
    }
    //显示细节制作框
    public void DisplayDetails(JsonData d, Sprite sp)
    {
        //{"ID":"1",
        //"Name":"\u94C1\u5F13",
        //"Type":"0",
        //"Level":"2",
        //"Icon":"icon_wuqi_tiepigong",
        //"Max_ShowCount":"1",
        //"Describe":"\u4F7F\u7528\u751F\u94C1\u548C\u80F6\u5E26\u548C\u9EBB\u7EF3\u5236\u4F5C\u800C\u6210\uFF0C\u8FD1\u6218\u6B66\u5668\uFF0C\u4F24\u5BB3\u9002\u4E2D",
        //"SynthesisID":"4#5#7",
        //"SynthesisCount":"10#10#20"}
        int id = int.Parse(d[0].ToString());
        currentItemID = id;
        createNum = 1;
        InitDetailCountChange();
        icon.sprite = sp;
        itemName.text = d[1].ToString();
        dec.text = d[6].ToString();
        itemNum.text = "拥有:" + DataModle.instance.GetItemCount(Config.PLAYER_ID, id).ToString();
        string[] _sid = d[7].ToString().Split('#');
        string[] _scount = d[8].ToString().Split('#');
        SynthesisInit(_sid, _scount);
        //BagDataModle.instance.GetSqliteWeaponCreateSynthesisData(id);

        details.gameObject.SetActive(true);
    }
    //处理合成所需的道具
    public void SynthesisInit(string[] sid, string[] scount)
    {
        for (int i = 0; i < conditionParent.childCount; i++)
        {
            Destroy(conditionParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < sid.Length; i++)
        {
            SpawnCondition(int.Parse(sid[i]), int.Parse(scount[i]));
        }
    }
    public void SpawnCondition(int id, int count)   //生成合成条件的那个物品  
    {
        GameObject sc = Resources.Load<GameObject>("Prefabs/Condition");
        GameObject scGO = Instantiate(sc, conditionParent);

        scGO.GetComponent<ConditionItem>().Init(id, count);

    }

}

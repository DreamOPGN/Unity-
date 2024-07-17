using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    private Transform bagPanel;
    private Transform weaponCreatePanel;
    private Transform startPanel;
    private Transform shopPanel;

    private BagControl bagControl;
    private WeaponCreateControl weaponCreateControl;
    private ShopControl shopControl;
    private Text coins;

    //开始界面组件
    private Button bag_btn;
    private Button weaponCreate_btn;
    private Button shop_btn;

    //背包界面组件
    private Button bag_close_btn;

    //武器制作界面组件
    private Button create_Btn;
    private Button add_Btn;
    private Button reduce_Btn;
    private Button weaponcreate_close_btn;

    //商城界面组件
    private Button shop_Close;

    void Start()
    {
        if (transform.Find("BagPanel") == null || transform.Find("WeaponCreatePanel") == null || transform.Find("StartPanel") == null)
            return;

        bagPanel = transform.Find("BagPanel");
        weaponCreatePanel = transform.Find("WeaponCreatePanel");
        startPanel = transform.Find("StartPanel");
        shopPanel = transform.Find("ShopPanel");
        bagControl = bagPanel.GetComponent<BagControl>();
        weaponCreateControl = weaponCreatePanel.GetComponent<WeaponCreateControl>();
        coins = transform.Find("Coins").GetComponent<Text>();
        //获取开始界面组件
        bag_btn = startPanel.Find("Bag_btn").GetComponent<Button>();
        weaponCreate_btn = startPanel.Find("WeaponCreate_btn").GetComponent<Button>();
        shop_btn = startPanel.Find("Shop_btn").GetComponent<Button>();
        //添加事件
        bag_btn.onClick.AddListener(Click_bag_btn);
        weaponCreate_btn.onClick.AddListener(Click_weaponCreate_btn);
        shop_btn.onClick.AddListener(Click_shop_btn);


        //获取武器界面制作组件
        weaponcreate_close_btn = weaponCreatePanel.Find("right_close_table/close_btn").GetComponent<Button>();
        //添加事件
        weaponcreate_close_btn.onClick.AddListener(Close);


        //获取背包界面组件
        bag_close_btn = bagPanel.Find("right_close_table/close_btn").GetComponent<Button>();
        //添加事件
        bag_close_btn.onClick.AddListener(Close);

        //获取商城界面
        shop_Close = transform.Find("ShopPanel/right_close_table/close_btn").GetComponent<Button>();
        shopControl = shopPanel.GetComponent<ShopControl>();
        //添加事件
        shop_Close.onClick.AddListener(Close);

        //更新金币
        UpdateCoinsText();
    }

    public void Click_bag_btn()
    {
        bagControl.Init();
        startPanel.gameObject.SetActive(false);
        weaponCreatePanel.gameObject.SetActive(false);
        bagPanel.gameObject.SetActive(true);
    }
    public void Click_weaponCreate_btn()
    {
        weaponCreateControl.Init();
        startPanel.gameObject.SetActive(false);

        bagPanel.gameObject.SetActive(false);
        weaponCreatePanel.gameObject.SetActive(true);
    }
    public void Click_shop_btn()
    {
        shopControl.InitShopGroup();
        startPanel.gameObject.SetActive(false);
        bagPanel.gameObject.SetActive(false);
        weaponCreatePanel.gameObject.SetActive(false);
        shopPanel.gameObject.SetActive(true);
    }
    public void UpdateCoinsText()
    {
        coins.text = "金币：" + SqliteManager.instance.SelectPlayerCoins();
    }
    public void Close()
    {
        shopPanel.gameObject.SetActive(false);
        bagPanel.gameObject.SetActive(false);
        weaponCreatePanel.gameObject.SetActive(false);
        startPanel.gameObject.SetActive(true);
    }
}

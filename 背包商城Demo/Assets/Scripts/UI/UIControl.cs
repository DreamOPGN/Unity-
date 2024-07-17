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

    //��ʼ�������
    private Button bag_btn;
    private Button weaponCreate_btn;
    private Button shop_btn;

    //�����������
    private Button bag_close_btn;

    //���������������
    private Button create_Btn;
    private Button add_Btn;
    private Button reduce_Btn;
    private Button weaponcreate_close_btn;

    //�̳ǽ������
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
        //��ȡ��ʼ�������
        bag_btn = startPanel.Find("Bag_btn").GetComponent<Button>();
        weaponCreate_btn = startPanel.Find("WeaponCreate_btn").GetComponent<Button>();
        shop_btn = startPanel.Find("Shop_btn").GetComponent<Button>();
        //����¼�
        bag_btn.onClick.AddListener(Click_bag_btn);
        weaponCreate_btn.onClick.AddListener(Click_weaponCreate_btn);
        shop_btn.onClick.AddListener(Click_shop_btn);


        //��ȡ���������������
        weaponcreate_close_btn = weaponCreatePanel.Find("right_close_table/close_btn").GetComponent<Button>();
        //����¼�
        weaponcreate_close_btn.onClick.AddListener(Close);


        //��ȡ�����������
        bag_close_btn = bagPanel.Find("right_close_table/close_btn").GetComponent<Button>();
        //����¼�
        bag_close_btn.onClick.AddListener(Close);

        //��ȡ�̳ǽ���
        shop_Close = transform.Find("ShopPanel/right_close_table/close_btn").GetComponent<Button>();
        shopControl = shopPanel.GetComponent<ShopControl>();
        //����¼�
        shop_Close.onClick.AddListener(Close);

        //���½��
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
        coins.text = "��ң�" + SqliteManager.instance.SelectPlayerCoins();
    }
    public void Close()
    {
        shopPanel.gameObject.SetActive(false);
        bagPanel.gameObject.SetActive(false);
        weaponCreatePanel.gameObject.SetActive(false);
        startPanel.gameObject.SetActive(true);
    }
}

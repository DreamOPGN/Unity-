using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddItem : MonoBehaviour
{

    private BagControl bagControl;
    private UIControl uiControl;
    private Button tiedao;
    private Button tiepigong;
    private Button yumaojian;
    private Button shengtie;
    private Button mutou;
    private Button jiaodai;
    private Button masheng;
    private Button yumao;
    void Start()
    {
        uiControl = transform.parent.parent.GetComponent<UIControl>();

        tiedao = transform.Find("tiedao").GetComponent<Button>();
        tiepigong = transform.Find("tiepigong").GetComponent<Button>();
        yumaojian = transform.Find("yumaojian").GetComponent<Button>();
        shengtie = transform.Find("shengtie").GetComponent<Button>();
        mutou = transform.Find("mutou").GetComponent<Button>();
        jiaodai = transform.Find("jiaodai").GetComponent<Button>();
        masheng = transform.Find("masheng").GetComponent<Button>();
        yumao = transform.Find("yumao").GetComponent<Button>();

        tiedao.onClick.AddListener(() => SimpleShop(1, 1));
        tiepigong.onClick.AddListener(() => SimpleShop(2, 1));
        yumaojian.onClick.AddListener(() => SimpleShop(3, 1));
        shengtie.onClick.AddListener(() => SimpleShop(4, 10));
        mutou.onClick.AddListener(() => SimpleShop(5, 10));
        jiaodai.onClick.AddListener(() => SimpleShop(6, 10));
        masheng.onClick.AddListener(() => SimpleShop(7, 10));
        yumao.onClick.AddListener(() => SimpleShop(8, 10));

    }

    public void SimpleShop(int id, int count)
    {
        SqliteManager.instance.InsertOrUpdateItem("1", id, count);

        SqliteManager.instance.UpdateCoins(Config.PLAYER_ID, id, -count);
        uiControl.UpdateCoinsText();
    }

    
}

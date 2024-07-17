using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    private int _id;
    private int _count;
    private UIControl _uiControl;
    void Start()
    {
        transform.GetComponent<Button>().onClick.AddListener(SimpleShop);
    }
    public void Init(int id, int count, UIControl uIControl)
    {
        _id = id;
        _count = count;
        _uiControl = uIControl;
    }
    public void SimpleShop()
    {
        SqliteManager.instance.InsertOrUpdateItem("1", _id, _count);

        SqliteManager.instance.UpdateCoins(Config.PLAYER_ID, _id, -_count);
        _uiControl.UpdateCoinsText();
    }

}

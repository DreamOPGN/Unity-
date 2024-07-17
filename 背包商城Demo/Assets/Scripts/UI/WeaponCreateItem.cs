using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;

public class WeaponCreateItem : MonoBehaviour
{
    private string itemName;
    private Image icon;
    private Text Name;
    private Sprite sprite;
    private JsonData data;
    private WeaponCreateControl weaponCreateControl;

    void Start()
    {

        weaponCreateControl = transform.parent.parent.parent.parent.GetComponent<WeaponCreateControl>();
        transform.GetComponent<Button>().onClick.AddListener(OnClickItem);
    }
    public void Init()
    {

    }
    
    public void OnClickItem()
    {
        if (itemName == null)
            return;

        weaponCreateControl.ChangeHightLightItem(transform);
        weaponCreateControl.DisplayDetails(data, sprite);
    }
    public void WeaponCreateInit(JsonData d)
    {
        icon = transform.Find("icon").GetComponent<Image>();
        Name = transform.Find("name").GetComponent<Text>();
        data = d;
        itemName = d[1].ToString();
        Name.text = itemName;
        Color co = transform.GetComponent<Image>().color;
        transform.GetComponent<Image>().color = new Color(co.r, co.g, co.b, 1f);

        if (Resources.Load("Sprites/" + d[4].ToString()) != null)
        {
            icon.sprite = Resources.Load<Sprite>("Sprites/" + d[4].ToString());
            this.sprite = icon.sprite;
        }

        Color c = icon.color;
        icon.color = new Color(c.r, c.g, c.b, 1f);
    }
}

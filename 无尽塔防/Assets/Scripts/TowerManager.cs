using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 
 */
public class TowerManager : MonoBehaviour
{
    private List<Transform> towerBaseList = new List<Transform>();

    public List<GameObject> towerPrefabList = new List<GameObject>();
    private UIManager uIManager;
    public int selectIndex = 0;
    void Start()
    {
        uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        Init();
    }
    private void Init()
    {
        for(int i = 0; i < 25; i++)
        {
           Transform towerBase = GameObject.Find("Tower_Base1_1 (" + i + ")").transform;
            towerBase.gameObject.AddComponent<TowerBase>();
            towerBaseList.Add(towerBase);
        }

        for(int i = 1; i < 6; i++)
        {
            towerPrefabList.Add( Resources.Load<GameObject>("Prefabs_Turrets/Tow_"+ i));
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.instance.isGameStart == true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            //射线检测：检测所有带collider
            if (Physics.Raycast(ray, out hitInfo, 100f, LayerMask.GetMask("TowerBase")))
            {
                if(hitInfo.collider.gameObject.tag == "TowerBase" && hitInfo.transform.GetChild(0).childCount < 1)
                {
                    TowerBase towerBase = hitInfo.collider.gameObject.GetComponent<TowerBase>();
                    if(towerBase != null)
                    {
                        //判断金币是否不够

                        GameObject towerObject = GameObject.Instantiate(towerPrefabList[selectIndex], hitInfo.transform.GetChild(0));
                        Tower tower = towerObject.GetComponent<Tower>();
                        if (GameData.instance.coins >= tower.coin)
                        {
                            towerObject.transform.localPosition = Vector3.zero;
                            towerObject.transform.eulerAngles = hitInfo.collider.transform.eulerAngles;
                            towerBase.tower = towerObject;
                            GameData.instance.coins -= tower.coin;
                            uIManager.UpdateBattleData();
                        }
                        else
                        {
                            GameObject.Destroy(towerObject);
                            uIManager.CoinTips();
                        }

                    }

                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class PlantCardsControl : MonoBehaviour
{
    public List<PlantCardView> plantCards;
    public GameObject plantGrid;
    public GameObject plantSelectGrid;
    public int combom;
    private static PlantCardsControl instance;
    public static PlantCardsControl Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("GameManager").AddComponent<PlantCardsControl>();
            }
            return instance;
        }
    }
    private JsonData HavePlantsData;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        plantGrid = GameObject.Find("Canvas/GameInterfacePanel/PlantGrid");
        plantSelectGrid = GameObject.Find("Canvas/SelectPlantPanel/SelectPlant/Grid");
        GetHavePlantsData();
        LoadPlantCardsSelect();
    }
    public void StartGame()
    {
        for(int i = 0; i < plantCards.Count; i++)
        {
            plantCards[i].isStartGame = true;
        }

    }
    public void LoadPlantCards()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            plantCards.Add(transform.GetChild(i).GetChild(0).GetComponent<PlantCardView>());
        }
    }
    public void SunIsChange()
    {
        for(int i = 0; i < plantCards.Count; i++)
        {
            plantCards[i].SunIsChange();
        }
    }
    public void PlantCardsMaskOpen()
    {
        for(int i = 0; i < plantCards.Count; i++)
        {
            plantCards[i].OpenMask();
            plantCards[i].returnCardSelect = false;
        }
    }
    public void GetHavePlantsData()
    {
        HavePlantsData = LevelDataModel.ReadPlayerHavePlants();
    }
    public void LoadPlantCardsSelect()
    {
        for(int i = 0; i < HavePlantsData.Count; i++)
        {
            string name = HavePlantsData[i].ToString();
            for (int j = 0; j < plantSelectGrid.transform.childCount; j++)
            {
                if (plantSelectGrid.transform.GetChild(i).childCount == 0)
                {
                    GameObject cardi = Resources.Load("PlantCardi/" + name + "Cardi") as GameObject;
                    GameObject Cardi = Instantiate(cardi);
                    Cardi.transform.SetParent(plantSelectGrid.transform.GetChild(i));
                    Cardi.transform.localPosition = Vector3.zero;
                    Cardi.transform.localScale = Vector3.one;
                    Cardi.name = name + "Cardi";
                    break;
                }
                
            }
        }
    }
    public void PlantGridAdd(string name, GameObject cardi)
    {
        combom++;
        for (int i = 0; i < plantGrid.transform.childCount; i++)
        {
            if(plantGrid.transform.GetChild(i).childCount == 0)
            {
                GameObject card = Resources.Load("PlantCards/" + name) as GameObject;
                card.GetComponent<PlantCardView>().GetCardSelectInfo(cardi);
                GameObject ca = Instantiate(card);
                ca.transform.SetParent(plantGrid.transform.GetChild(i));
                ca.transform.localPosition = Vector2.zero;
                ca.transform.localScale = Vector3.one;
                ca.gameObject.SetActive(true);
                ca.name = name;

                break;
            }
        }
    }
}

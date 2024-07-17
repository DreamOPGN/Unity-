using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlantCardSelectView : MonoBehaviour, IPointerDownHandler
{
    public GameObject mask;
    private void Start()
    {
        mask = transform.Find("mask").gameObject;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(PlantCardsControl.Instance.combom < 6)
        {
            if (mask.activeSelf == false)
            {
                string name = gameObject.name.Substring(0, gameObject.name.Length - 1);
                PlantCardsControl.Instance.PlantGridAdd(name, gameObject);
            }
            mask.SetActive(true);
        }

    }
}

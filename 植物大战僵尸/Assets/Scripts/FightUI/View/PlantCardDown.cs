using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.EventSystems;

public class PlantCardDown : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        LevelDataModel.AddPlamt(gameObject.name);

        Destroy(gameObject);
    }
}

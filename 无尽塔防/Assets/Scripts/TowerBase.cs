using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    private Transform towerPostion;
    public GameObject tower;
    void Start()
    {
        towerPostion = transform.Find("pos");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

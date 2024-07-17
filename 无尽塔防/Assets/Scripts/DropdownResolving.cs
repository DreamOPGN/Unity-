using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownResolving : MonoBehaviour
{
    private Dropdown dropdown;
    void Start()
    {
        dropdown = transform.Find("Dropdown").GetComponent<Dropdown>();
        dropdown.onValueChanged.AddListener(DropItemChange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DropItemChange(int value)
    {
        Debug.Log(value);
    }
}

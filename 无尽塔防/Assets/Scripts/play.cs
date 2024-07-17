using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class play : MonoBehaviour
{

    private Ray ra;//声明射线
    private RaycastHit hit;//声明碰撞点
    private GameObject Element;//控件

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("按下鼠标左键");
            ra = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ra, out hit) && hit.collider.tag == "TowerBase")
            {

                Element = hit.collider.gameObject;
                Debug.Log(Element.transform.position);
                Debug.Log(hit.collider.gameObject.name);

            }

        }
    }
}

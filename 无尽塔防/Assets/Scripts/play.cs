using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class play : MonoBehaviour
{

    private Ray ra;//��������
    private RaycastHit hit;//������ײ��
    private GameObject Element;//�ؼ�

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("����������");
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

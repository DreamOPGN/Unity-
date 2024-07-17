using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShovelControl : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IPointerClickHandler
{
    private GameObject shovel;
    private GameObject shovelMove;
    private Transform shovelMoveOriginal;
    public bool IsMove;

    public float combom;    //**用于计数点击铲子的数量如果是双数就可以返回

    public void OnPointerClick(PointerEventData eventData)
    {
      if(combom % 2 == 0)
      {
            shovelMove.SetActive(false);
            shovelMove.transform.position = shovelMoveOriginal.position;
            shovel.SetActive(true);
            IsMove = false;
      }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AudioSourceManager.Instance.PlayShovelSound();
        shovel.SetActive(false); 
        shovelMove.SetActive(true);
        IsMove = true;
       
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        combom++;
    }

    // Start is called before the first frame update
    void Start()
    {
        shovel = transform.Find("Shovel").gameObject;
        shovelMove = GameObject.Find("ShovelMove");
        shovelMoveOriginal = shovelMove.transform;
        shovelMove.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsMove)
        {
            
            Vector3 mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            shovelMove.transform.position = new Vector3(mousePos.x, mousePos.y, 64);
            RayCheckControl.Instance.isCheckSun = false;
            if (Input.GetMouseButton(0))
            {


                RaycastHit2D[] hits = RayCheckControl.Instance.StartCheck();
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider != null)
                    {
                        if (hit.collider.gameObject.CompareTag("GrassGrid") && hit.collider.transform.childCount > 0)
                        {

                            //PoolControl.Instance.PlaceInPool(hit.collider.transform.GetChild(0).gameObject);
                            Destroy(hit.collider.transform.GetChild(0).gameObject);
                            shovelMove.SetActive(false);
                            shovelMove.transform.position = shovelMoveOriginal.position;
                            shovel.SetActive(true);
                            IsMove = false;

                            AudioSourceManager.Instance.PlayShovelSound();
                            combom++;
                        }
                        RayCheckControl.Instance.isCheckSun = true;
                    }
                }

            }

        }
    }
}

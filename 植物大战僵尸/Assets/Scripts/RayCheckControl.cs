using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCheckControl : MonoBehaviour
{
    private static RayCheckControl instance;
    public static RayCheckControl Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("GameManager").AddComponent<RayCheckControl>();
            }
            return instance;
        }
    }

    public bool isCheckSun = true;
    private void Awake()
    {
        instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        RayCheck();
    }
    public void RayCheck()
    {
        if (Input.GetMouseButtonDown(0) && isCheckSun) // ¼à²âÊó±ê×ó¼üµã»÷
        {

            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(clickPosition, Vector2.zero);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null)
                {

                    if (hit.collider.CompareTag("Sun"))
                    {
                        
                        hit.collider.gameObject.GetComponent<SunView>().Detected();
                    }
                }
            }
        }
    }
    public RaycastHit2D[] StartCheck()
    {
        Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hit = Physics2D.RaycastAll(clickPosition, Vector2.zero);
        return hit;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    private Camera mainCamera;
    private Vector3 startPos;

    public float moveSpeed = 250;  //摄像机移动速度
    public float smoothSpeed = 5;  //摄像机平滑程度
    public float scrollWheelSpeed = 5;

    public Vector2 limitXPos = new Vector2(48, 153);
    public Vector2 limitZPos = new Vector2(90, 120);
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        mainCamera = this.GetComponent<Camera>();
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isGameStart) return;
        Move();
        ScaleView();
    }

    //
    private void Move()
    {
        if (Input.GetMouseButton(1))
        {
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");

                if (((mouseX > 0 && transform.position.x <= limitXPos.x) || (transform.position.x > limitXPos.y && mouseX < 0)) ||
                   ((mouseY < 0 && transform.position.z >= limitZPos.x) || (transform.position.z < limitZPos.y && mouseY > 0))
                    )   //限制摄像机移动范围
                    return;
                //插值运算(a, b, t)
                Vector3 targetPos = transform.position + new Vector3(-mouseX, 0, -mouseY) * moveSpeed * Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothSpeed);
                //transform.position = new Vector3(Mathf.Clamp(transform.position.x, limitXPos.x, limitXPos.y), transform.position.y, Mathf.Clamp(transform.position.z, limitZPos.x, limitZPos.y));
            }
        }

    }
    void ScaleView()
    {
        float mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");

        if(mouseScrollWheel != 0)
        {
            mainCamera.fieldOfView += mouseScrollWheel * -scrollWheelSpeed;
            mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, 35, 95);
        }
    }
}

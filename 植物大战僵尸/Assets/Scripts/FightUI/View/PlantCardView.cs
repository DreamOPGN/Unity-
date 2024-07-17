using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using PlantSpace;

public class PlantCardView : MonoBehaviour,IPointerDownHandler, IPointerUpHandler
{
    public enum Stats
    {
        Normal,
        IsLoding,
        IsSelect,
        NoEnough
    }
    public Stats currentStats;
    [SerializeField]
    private float cdTime;
    private float price      //价格
    {
        get
        {
            return float.Parse(transform.Find("sunNum").GetComponent<Text>().text);
        }
    }
    private float currentSunNum
    {
        get
        {
            return SunModle.Instance.GetSunNum();
        }
    }
    public GameObject mask;//阴影遮罩
    private GameObject plantMove;//用于拖动时跟着鼠标显示的植物图片
    private Slider timeSlider;                 
    private GameObject timeSliderGO;           
    private GameObject PlantPrefabs;           //生成植物的预制体
    private string PlantPrefabName;
    public GameObject plants;                 //生成的植物
    private Transform originalParent;         //植物卡原本在的格子
    private SpriteRenderer sr;
    private bool canReturn;
    public Camera mainCamera;
    public Canvas canvas;


    public int clickCount;
    public bool isStartLoad;
    public bool isStartGame = false;

    public bool returnCardSelect = false;
    private GameObject cardi;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (returnCardSelect)
        {
            Debug.Log("1");
            ReturnSelectPlant();
            returnCardSelect = false;
            return;
        }
            





        clickCount++;
        if (currentStats == Stats.Normal && currentSunNum >= price)
        {
            currentStats = Stats.IsSelect;
            plantMove.transform.SetParent(GameObject.Find("/Canvas/GameInterfacePanel").transform);
            plantMove.SetActive(true);
            mask.SetActive(true);
            GameObject plant = PoolControl.Instance.GetFromPool<GameObject>(PlantPrefabName, PlantPrefabs) ;
            // 获取当前精灵的颜色
            Color currentColor = plant.GetComponent<SpriteRenderer>().color;
            // 创建一个新的颜色对象，并设置新的alpha值
            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0.5f); // 设置为半透明                                                                                            // 应用修改后的颜色
            plant.GetComponent<SpriteRenderer>().color = newColor;
            plants = plant;
            sr = plants.GetComponent<SpriteRenderer>();
            plants.SetActive(false);
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (returnCardSelect) return;
        canReturn = true;
        clickCount++;
    }
    public void ReturnSelectPlant()
    {
        cardi.GetComponent<PlantCardSelectView>().mask.SetActive(true);
        Destroy(gameObject);
    }
    public void GetCardSelectInfo(GameObject cardi)
    {
        returnCardSelect = true;
        this.cardi = cardi;
    }
    public void OpenMask()
    {
        mask.SetActive(true);
    }
    private void Init()
    {

    }
    void Start()
    {
        mainCamera = Camera.main;
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        
        PlantPrefabName = gameObject.name.Substring(0,gameObject.name.Length - 4);

        mask = gameObject.transform.Find("mask").gameObject;
        plantMove = gameObject.transform.Find("plantMove").gameObject;
        originalParent = gameObject.transform;
        timeSlider = gameObject.transform.Find("timeSlider").gameObject.GetComponent<Slider>();
        timeSliderGO = gameObject.transform.Find("timeSlider").gameObject;
        clickCount = 0;
        //加载植物预制体
        if (Resources.Load("PlantPrefabs/" + PlantPrefabName) != null)
        {
            PlantPrefabs = Resources.Load("PlantPrefabs/" + PlantPrefabName) as GameObject;
        }
        else
        {
            Debug.Log("找不到预制体！");
        }
        currentStats = Stats.NoEnough;
        mask.SetActive(false);
    }

    void Update()
    {
        if (isStartGame)
        {
            if (currentSunNum < price)
            {

                currentStats = Stats.NoEnough;
                mask.SetActive(true);
            }
            else
            {

                currentStats = Stats.Normal;
                mask.SetActive(false);
            }
            if (isStartLoad)
            {

                //将状态设置为加载
                currentStats = Stats.IsLoding;
                timeSlider.value = 1;
                mask.SetActive(true);
                timeSliderGO.SetActive(true);
                StartCoroutine(startLoad(cdTime));
            }
            isStartGame = false;
        }

        if (currentStats == Stats.IsSelect)
        {

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -mainCamera.transform.position.z; // 使 z 坐标与摄像机位置相匹配
            Vector3 uiPosition;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.GetComponent<RectTransform>(), mousePos, mainCamera, out uiPosition);
            plantMove.GetComponent<RectTransform>().position = uiPosition;

            Vector2 rayOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 将屏幕坐标转换为世界坐标
            RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, Vector2.zero); // 在2D平面上进行射线检测
            foreach(RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.CompareTag("GrassGrid"))
                {
                    if (hit.collider.transform.childCount < 1)
                    {
                        plants.transform.SetParent(hit.collider.transform);
                        plants.transform.localPosition = Vector3.zero;
                        plants.GetComponent<BoxCollider2D>().enabled = false; 
                        plants.SetActive(true);
                        if (sr.color.a == 0)
                        {
                            Color currentColor = sr.color;
                            // 创建一个新的颜色对象，并设置新的alpha值
                            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0.5f); // 设置为半透明                                                                                            // 应用修改后的颜色
                            plants.GetComponent<SpriteRenderer>().color = newColor;
                        }
                    }

                }
                if (hit.collider.gameObject.CompareTag("PlantGrid"))
                {
                    plants.transform.SetParent(plantMove.transform);
                    //plants.SetActive(false);
                    if(sr.color.a != 0)
                    {

                        Color currentColor = sr.color;
                        // 创建一个新的颜色对象，并设置新的alpha值
                        Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0f); // 设置为透明                                                                                            // 应用修改后的颜色
                        plants.GetComponent<SpriteRenderer>().color = newColor;
                    }

                }
            }

            
            if(plants.activeSelf == true || canReturn)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    canReturn = false;
                    foreach (RaycastHit2D hit in hits)
                    {

                        if (hit.collider.gameObject.CompareTag("PlantGrid")  && clickCount % 2 == 0)
                        {

                            currentStats = Stats.Normal;
                            //将拖在手中的植物返回原本的地方
                            plantMove.SetActive(false);
                            plantMove.transform.SetParent(originalParent);
                            plantMove.transform.localPosition = Vector3.zero;

                            mask.SetActive(false);
                            Destroy(plants);

                            return;
                        }

                        if (hit.collider.gameObject.CompareTag("GrassGrid"))
                        {
                            //生成植物在检测到的格子里
                            Color currentColor = plants.GetComponent<SpriteRenderer>().color;
                            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, 1f);                                                                                        // 应用修改后的颜色
                            plants.GetComponent<SpriteRenderer>().color = newColor;
                            plants.GetComponent<Plants>().TakeNormal();
                            plants.GetComponent<Plants>().Init();
                            plants.GetComponent<BoxCollider2D>().enabled = true;
                            //减少太阳数量
                            SunModle.Instance.ChangeSunNum(-price);
                            //将状态设置为加载
                            currentStats = Stats.IsLoding;
                            timeSlider.value = 1;
                            timeSliderGO.SetActive(true);
                            //开启协程让slider逐渐降为0
                            StartCoroutine(startLoad(cdTime));
                            //将拖在手中的植物返回原本的地方
                            plantMove.SetActive(false);
                            plantMove.transform.SetParent(originalParent);
                            plantMove.transform.localPosition = Vector3.zero;
                            plants = null;
                        }
                    }


                }
            }
            
           
        }    
    }
    IEnumerator startLoad(float time)
    {
        float startValue = timeSlider.value;
        float startTime = Time.time;
        while (Time.time - startTime < time)
        {
            float timeFraction = (Time.time - startTime) / time;
            timeSlider.value = Mathf.Lerp(startValue, 0, timeFraction);
            yield return null;
        }

        timeSlider.value = 0; // 确保最终值为0
        timeSliderGO.SetActive(false);
        //加载完判断
        if (currentSunNum < price)
        {
            currentStats = Stats.NoEnough;
            mask.SetActive(true);
        }
        else
        {
            currentStats = Stats.Normal;
            mask.SetActive(false);
        }

    }
    public void SunIsChange()
    {
        if (currentStats == Stats.IsLoding || currentStats == Stats.IsSelect)
            return;
        if(currentSunNum < price)
        {
            currentStats = Stats.NoEnough;
            mask.SetActive(true);
        }
        else
        {
            currentStats = Stats.Normal;
            mask.SetActive(false);
        }
    }


}

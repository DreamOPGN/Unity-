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
    private float price      //�۸�
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
    public GameObject mask;//��Ӱ����
    private GameObject plantMove;//�����϶�ʱ���������ʾ��ֲ��ͼƬ
    private Slider timeSlider;                 
    private GameObject timeSliderGO;           
    private GameObject PlantPrefabs;           //����ֲ���Ԥ����
    private string PlantPrefabName;
    public GameObject plants;                 //���ɵ�ֲ��
    private Transform originalParent;         //ֲ�￨ԭ���ڵĸ���
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
            // ��ȡ��ǰ�������ɫ
            Color currentColor = plant.GetComponent<SpriteRenderer>().color;
            // ����һ���µ���ɫ���󣬲������µ�alphaֵ
            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0.5f); // ����Ϊ��͸��                                                                                            // Ӧ���޸ĺ����ɫ
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
        //����ֲ��Ԥ����
        if (Resources.Load("PlantPrefabs/" + PlantPrefabName) != null)
        {
            PlantPrefabs = Resources.Load("PlantPrefabs/" + PlantPrefabName) as GameObject;
        }
        else
        {
            Debug.Log("�Ҳ���Ԥ���壡");
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

                //��״̬����Ϊ����
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
            mousePos.z = -mainCamera.transform.position.z; // ʹ z �����������λ����ƥ��
            Vector3 uiPosition;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.GetComponent<RectTransform>(), mousePos, mainCamera, out uiPosition);
            plantMove.GetComponent<RectTransform>().position = uiPosition;

            Vector2 rayOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition); // ����Ļ����ת��Ϊ��������
            RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, Vector2.zero); // ��2Dƽ���Ͻ������߼��
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
                            // ����һ���µ���ɫ���󣬲������µ�alphaֵ
                            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0.5f); // ����Ϊ��͸��                                                                                            // Ӧ���޸ĺ����ɫ
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
                        // ����һ���µ���ɫ���󣬲������µ�alphaֵ
                        Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0f); // ����Ϊ͸��                                                                                            // Ӧ���޸ĺ����ɫ
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
                            //���������е�ֲ�ﷵ��ԭ���ĵط�
                            plantMove.SetActive(false);
                            plantMove.transform.SetParent(originalParent);
                            plantMove.transform.localPosition = Vector3.zero;

                            mask.SetActive(false);
                            Destroy(plants);

                            return;
                        }

                        if (hit.collider.gameObject.CompareTag("GrassGrid"))
                        {
                            //����ֲ���ڼ�⵽�ĸ�����
                            Color currentColor = plants.GetComponent<SpriteRenderer>().color;
                            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, 1f);                                                                                        // Ӧ���޸ĺ����ɫ
                            plants.GetComponent<SpriteRenderer>().color = newColor;
                            plants.GetComponent<Plants>().TakeNormal();
                            plants.GetComponent<Plants>().Init();
                            plants.GetComponent<BoxCollider2D>().enabled = true;
                            //����̫������
                            SunModle.Instance.ChangeSunNum(-price);
                            //��״̬����Ϊ����
                            currentStats = Stats.IsLoding;
                            timeSlider.value = 1;
                            timeSliderGO.SetActive(true);
                            //����Э����slider�𽥽�Ϊ0
                            StartCoroutine(startLoad(cdTime));
                            //���������е�ֲ�ﷵ��ԭ���ĵط�
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

        timeSlider.value = 0; // ȷ������ֵΪ0
        timeSliderGO.SetActive(false);
        //�������ж�
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

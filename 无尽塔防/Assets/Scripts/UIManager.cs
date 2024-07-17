using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Transform startPanel;
    private Transform fightPanel;
    private Transform pausePanel;
    private Transform gameOverPanel;
    private Transform settingPanel;

    //������
    private Button startBtn;
    private Button settingBtn;
    private Button quitBtn;

    private AudioSource audioSource;
    //��ͣ����
    private Button pauseBtn;
    private Button pause_BackGameBtn;
    private Button pause_ReStartBtn;
    private Button pause_BackMainMenuBtn;
    private Image icon1;
    //ʧ�ܽ���
    private Button gameOver_restart;
    private Button gameOver_quit;

    //���ý���
    private Dropdown dropdown;
    private Slider slider;
    private Slider mouseRollerSpeedSlider;
    private Slider mouseSliderSpeedSlider;
    private Button setting_backgame;


    //ս������
    
    private int secondTime;
    private int minuteTime;
    public Transform Tips;
    private Text timeText;
    private Text homeHPText;
    private Text enemyCountText;
    private Text enemyKillText;
    private Text coinText;
    public Text tipsText;
    private Sprite pauseSprite;
    private Sprite resumeSprite;
    private Button[] towerBtn = new Button[5];
    private Button lastClick;

    private TowerManager towerManager;


    private GameObject tipsBgPrefab;
    //����ʱ


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        //��ȡ��Ӧ�Ľ���
        startPanel = transform.Find("StartPanel");
        fightPanel = transform.Find("FightPanel");
        pausePanel = transform.Find("PausePanel");
        gameOverPanel = transform.Find("GameOverPanel");
        settingPanel = transform.Find("SettingPanel");

        //��ʼ���������ȡ
        startBtn = startPanel.Find("start_btn").GetComponent<Button>();
        settingBtn = startPanel.Find("setting_btn").GetComponent<Button>();
        quitBtn = startPanel.Find("quit_btn").GetComponent<Button>();
        pauseBtn = fightPanel.Find("TopContainers/pauseBtn").GetComponent<Button>();

        //��ͣ���������ȡ
        pause_BackGameBtn = pausePanel.Find("BackGameBtn").GetComponent<Button>();
        pause_ReStartBtn = pausePanel.Find("ReStartBtn").GetComponent<Button>();
        pause_BackMainMenuBtn = pausePanel.Find("BackMainMenuBtn").GetComponent<Button>();

        //���ý��������ȡ
        dropdown = settingPanel.Find("resolving power").GetComponent<Dropdown>();
        slider = settingPanel.Find("Music/Slider").GetComponent<Slider>();
        setting_backgame = settingPanel.Find("BackGameBtn").GetComponent<Button>();
        mouseRollerSpeedSlider = settingPanel.Find("mouseRollerSpeed/Slider").GetComponent<Slider>();
        mouseSliderSpeedSlider = settingPanel.Find("mouseSliderSpeed/Slider").GetComponent<Slider>();


        //��Ϸʧ�ܽ��������ȡ
        gameOver_restart = gameOverPanel.Find("restart").GetComponent<Button>();
        gameOver_quit = gameOverPanel.Find("quit").GetComponent<Button>();
        towerManager = GameObject.Find("TowerManager").GetComponent<TowerManager>();
        //icon1 = startPanel.Find("title/Image (1)").GetComponent<Image>();

        //����ʼ������������¼�
        startBtn.onClick.AddListener(OnClickStartBtn);
        settingBtn.onClick.AddListener(OnClickSettingBtn);
        quitBtn.onClick.AddListener(OnClickQuitBtn);
        pauseBtn.onClick.AddListener(OnClickPauseBtn);

        //������ͣ������������¼�
        pause_BackGameBtn.onClick.AddListener(OnClickPause_BackGameBtn);
        pause_ReStartBtn.onClick.AddListener(OnClickPause_RestartGameBtn);
        pause_BackMainMenuBtn.onClick.AddListener(OnClickPause_BackMainMenuBtn);

        //����ʧ�ܽ�����ۼ����¼�
        gameOver_restart.onClick.AddListener(OnClickPause_RestartGameBtn);
        gameOver_quit.onClick.AddListener(OnClickQuitBtn);

        //�������ý�������¼�
        dropdown.onValueChanged.AddListener(DropItemChange);
        
        slider.onValueChanged.AddListener(VoiceSlider);
        mouseRollerSpeedSlider.onValueChanged.AddListener(MouseRollerSpeedSlider) ;
        mouseSliderSpeedSlider.onValueChanged.AddListener(MouseSliderSpeedSlider);
        setting_backgame.onClick.AddListener(OnClickSetting_BackGameBtn);
        if (PlayerPrefs.HasKey("DropDownValue"))
        {
            dropdown.value = PlayerPrefs.GetInt("DropDownValue");

        }
        else
        {
            dropdown.value = 1;
        }
        if (PlayerPrefs.HasKey("mouseSliderSpeedSliderValue"))
        {
            mouseSliderSpeedSlider.value = PlayerPrefs.GetFloat("mouseSliderSpeedSliderValue");
            CameraManager.instance.moveSpeed = mouseSliderSpeedSlider.value * 2 * 250;
        }
        else
        {
            mouseSliderSpeedSlider.value = 0.5f;
            CameraManager.instance.moveSpeed = mouseSliderSpeedSlider.value * 2 * 250;
        }
        if (PlayerPrefs.HasKey("mouseRollerSpeedSliderValue"))
        {
            mouseRollerSpeedSlider.value = PlayerPrefs.GetFloat("mouseRollerSpeedSliderValue");
            CameraManager.instance.scrollWheelSpeed = mouseRollerSpeedSlider.value * 2 * 5;
        }
        else
        {
            mouseRollerSpeedSlider.value = 0.5f;
            CameraManager.instance.scrollWheelSpeed = mouseRollerSpeedSlider.value * 2 * 5;
        }
        //����ս�������ı�
        Tips = fightPanel.Find("Tips");
        tipsText = Tips.Find("Text").GetComponent<Text>();
        timeText = fightPanel.Find("TopContainers/timeText").GetComponent<Text>();
        homeHPText = fightPanel.Find("TopContainers/homeHPBg/Text").GetComponent<Text>();
        enemyCountText = fightPanel.Find("TopContainers/enemyCountBg/Text").GetComponent<Text>();
        coinText = fightPanel.Find("TopContainers/coinsBg/Text").GetComponent<Text>();
        enemyKillText = fightPanel.Find("TopContainers/enemyKillBg/Text").GetComponent<Text>();

        pauseSprite = Resources.Load<Sprite>("Icon_pause");
        resumeSprite = Resources.Load<Sprite>("Icon_Resume");
        tipsBgPrefab = Resources.Load<GameObject>("Tips");

        for(int i = 0; i < 5; i++)
        {

            towerBtn[i] = fightPanel.transform.Find("TowerContainers/Item" + (i + 1)).GetComponent<Button>();
        }

        for (int i = 0; i < towerBtn.Length; i++)
        {
            int index = i;
            Button btn = towerBtn[i];
            btn.onClick.AddListener(delegate() { OnCLickTowerBtn(index, btn); });

        }
        towerManager.selectIndex = 0;
        towerBtn[0].gameObject.GetComponent<Image>().color = Color.yellow;
        lastClick = towerBtn[0];

        for (int i = 0; i < towerManager.towerPrefabList.Count; i++)
        {
            float coin = towerManager.towerPrefabList[i].GetComponent<Tower>().coin;
            towerBtn[i].transform.Find("Text (Legacy)").GetComponent<Text>().text = coin.ToString();
        }
        //GameData.instance.SetLevelData();
        UpdateBattleData();

        fightPanel.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(false);
        settingPanel.gameObject.SetActive(false);
        startPanel.gameObject.SetActive(true);
        audioSource = transform.gameObject.GetComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>("audio/bg");
        if(PlayerPrefs.HasKey("voiceValue"))
        {
            float v = PlayerPrefs.GetFloat("voiceValue");
            slider.value = v;
            audioSource.volume = v;

        }
        else
        {
            slider.value = 1;
            audioSource.volume = 1;

        }
        audioSource.Play();

        
    }
    private void Update()
    {

        //GMָ��
        if (Input.GetKeyDown(KeyCode.K))
        {
            // GameData.instance. WriteLevelIDData();
            ShowGameResult(true);
        }

    }
    //��ʼ����
    void OnClickStartBtn()
    {
       
        startPanel.gameObject.SetActive(false);
        fightPanel.gameObject.SetActive(true);
        StartCoroutine(CountDown());
    }
    void OnClickSettingBtn()
    {
        startPanel.gameObject.SetActive(false);
        fightPanel.gameObject.SetActive(false);
        settingPanel.gameObject.SetActive(true);
        //Application.targetFrameRate = 30;   //����֡��
    }
    void OnClickQuitBtn()
    {
        Application.Quit();
    }

    //��������¼�
    void OnCLickTowerBtn(int index, Button btn)
    {
        towerManager.selectIndex = index;
        if(lastClick != null)
        {
            lastClick.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
        }
        btn.gameObject.GetComponent<Image>().color = Color.yellow;
        lastClick = btn;
    }

    //������ͣ�����߼�
    void OnClickPauseBtn()
    {
        Time.timeScale = 0;
        GameManager.instance.isGameStart = false;
        pauseBtn.gameObject.GetComponent<Image>().sprite = resumeSprite;
        fightPanel.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(true);
    }
    void OnClickPause_BackGameBtn()
    {
        Time.timeScale = 1;
        GameManager.instance.isGameStart = true;
        pauseBtn.gameObject.GetComponent<Image>().sprite = pauseSprite;
        pausePanel.gameObject.SetActive(false);
        fightPanel.gameObject.SetActive(true);
    }
    void OnClickPause_RestartGameBtn()
    {
        Debug.Log("���¿�ʼ��Ϸ");
        Time.timeScale = 1;
        Scene scene = SceneManager.GetActiveScene();    //��õ�ǰ���еĳ���
        SceneManager.LoadScene(scene.name);
    }
    void OnClickPause_BackMainMenuBtn()
    {
        GameManager.instance.isGameStart = false;
        SceneManager.LoadScene(0);
    }

    //����ս����������
    public void UpdateBattleData()
    {

        homeHPText.text = GameData.instance.homeHP.ToString();
        enemyCountText.text = GameData.instance.enemyCount.ToString();
        coinText.text = GameData.instance.coins.ToString();
        enemyKillText.text = GameData.instance.killCount.ToString();
    }
    
    public void UpdateTips(string str)
    {
        tipsText.text = str;
        Tips.gameObject.SetActive(true);

    }

    //���ý���
    public void DropItemChange(int value)   //�ֱ��������˵�
    {

        switch (value)
        {
            case 0:
                Screen.SetResolution(1920, 1080, true);
                break;
            case 1:
                Screen.SetResolution(1920, 1080, false);
                break;
            case 2:
                Screen.SetResolution(1280, 720, false);
                break;
        }

        PlayerPrefs.SetInt("DropDownValue", value);

    }
    public void OnClickSetting_BackGameBtn()
    {

        settingPanel.gameObject.SetActive(false);
        startPanel.gameObject.SetActive(true);
    }
    public void VoiceSlider(float value)
    {
        audioSource.volume = value;
        PlayerPrefs.SetFloat("voiceValue", value);
    }
    public void MouseRollerSpeedSlider(float value)
    {
        CameraManager.instance.scrollWheelSpeed = value * 2 * 5;
        PlayerPrefs.SetFloat("mouseRollerSpeedSliderValue", value);
    }
    public void MouseSliderSpeedSlider(float value)
    {
        CameraManager.instance.moveSpeed = value * 2 * 250;
        PlayerPrefs.SetFloat("mouseSliderSpeedSliderValue", value);
    }
    //Э�̵���ʱ
    IEnumerator CountDown()
    {
        Tips.gameObject.SetActive(true);
        for(int i = 0; i < 5; i++)
        {
            tipsText.text = $"���Ｔ������ {5 - i}";
            yield return new WaitForSeconds(1f);  //�ж�ָ��
        }
        Tips.gameObject.SetActive(false);
        GameManager.instance.isGameStart = true;
        EnemyManager.instance.StartEnemy();
        //��ˮ��Ѫ������0��ʱ����ʾ��ǰ��Ϸʱ��
        while(GameData.instance.homeHP > 0)
        {
            yield return new WaitForSeconds(1f);
            GameData.instance.time += 1;
            float second = GameData.instance.time % 60;
            timeText.text = $"{(GameData.instance.time / 60).ToString("00")}:{second.ToString("00")}";
        }
    }

    //������Ϸ�ɹ���ʧ�ܽ���
    public void ShowGameResult(bool isWin)
    {
        if (!isWin)
        {
            //��ʾʧ�ܽ���
            fightPanel.gameObject.SetActive(false);
            gameOverPanel.gameObject.SetActive(true);
            GameManager.instance.isGameStart = false;
        }
        else
        {
            UpdateTips("��Ϸʤ����������һ��");
            NextLevel();
        }
    }
    public void NextLevel()
    {
        GameData.instance.WriteLevelIDData();
        SceneManager.LoadScene(1);
    }
    //��������ʱ����Ҳ������ʾ
    public void CoinTips()
    {
        GameObject tips = GameObject.Instantiate(tipsBgPrefab);
        tips.transform.SetParent(fightPanel);  //���õ��򸸽ڵ�

        tips.transform.position = Tips.transform.position;
        tips.AddComponent<MoveTween>();
        tips.gameObject.SetActive(true);
        if(GameManager.instance.tipsSpawnCount == 5)
        {
            tips.transform.GetComponentInChildren<Text>().text = "��˵��Ҳ����ˣ�С�ϵ�";
        }
        else
        {
            tips.transform.GetComponentInChildren<Text>().text = "��Ҳ���!!!";
        }
    }
}

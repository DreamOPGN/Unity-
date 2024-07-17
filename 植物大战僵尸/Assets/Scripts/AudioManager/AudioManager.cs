using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //����ģʽ,����������
    public static AudioManager Instance
    {
        get
        {
            if(instance == null)
            {
                new GameObject("AudioManger").AddComponent<AudioManager>();
                
            }
            return instance;
        }
    }
    private static AudioManager instance;
    public bool IsMusic; 
    public bool isSound;
    private AudioSource audioSource;

    private float currentSoundVolume;
    private void Awake()
    {
        instance = this;
        if(GetComponent<AudioSource>() == null)
        {
            gameObject.AddComponent<AudioSource>();
        }
        audioSource = GetComponent<AudioSource>();
        //Ĭ����0��Ҫ��������,��������ֵ����Ҫ����
        IsMusic = (PlayerPrefs.GetInt("music") == 0);
        isSound = (PlayerPrefs.GetInt("sound") == 0);

        if (IsMusic)
        {
            audioSource.volume = PlayerPrefs.GetFloat("musicVolume");
        }
        if (isSound)
        {
            
            currentSoundVolume = PlayerPrefs.GetFloat("soundVolume");
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    /// <summary>
    /// ���ű�������
    /// </summary>
    /// <param name="ads">�������</param>
    /// <param name="musicFileName">�����ļ���(��Ҫ·��)</param>
    /// <param name="isLoop">�Ƿ�Ϊѭ������(Ĭ��ֵtrue)</param>
    public void PlayBGM(string musicFileName,bool isLoop = true)
    {
        if (audioSource == null) return;

        AudioClip clip = ResManager.instance.LoadMusicFromFile(musicFileName);
        if (clip == null)
        {
            Debug.Log("�����ļ�:"+ musicFileName + "����ʧ��!");
            return;
        }

        audioSource.clip = clip;
        audioSource.loop = isLoop;
        if(IsMusic)
            audioSource.Play();
    }

    public void stopBGM()
    {
        if (audioSource == null) return;

        audioSource.Stop();
        audioSource.clip = null;

    }

    public void pauseBGM()
    {
        if (audioSource == null) return;
        Debug.Log("AudioManager:��ͣ");
        audioSource.Pause();
    }

    public void resumBGM()
    {
        if (audioSource == null) return;
        if (audioSource.clip == null) return;
        Debug.Log("AudioManager:�ָ�");
        audioSource.Play();
    }
    public void PlaySound(AudioSource ads, string soundFileName, bool isLoop = false)
    {
        if (ads == null) return;
        AudioClip clip = null;
        clip = ResManager.instance.LoadSoundFromFile(soundFileName);
        if (clip == null)
        {
            Debug.Log("��Ч�ļ�:" + soundFileName + "����ʧ��!");
            return;
        }
        if (isSound)
        {
            ads.volume = currentSoundVolume;
        }
        else
        {
            ads.volume = 0;
            //return;
        }
        ads.Stop();
        ads.clip = clip;
        ads.loop = isLoop;
        ads.Play();


    }
    /// <summary>
    /// ֹͣ������Ч
    /// </summary>
    /// <param name="ads"></param>
    public void stop(AudioSource ads)
    {
        if (ads == null) return;

        ads.Stop();
        ads.clip = null;

    }
    /// <summary>
    /// ���ű������ִ�С
    /// </summary>
    public void MusicChange(float value)
    {
        audioSource.volume = value;
        PlayerPrefs.SetFloat("musicVolume", value);
    }
    public void SoundChange(float value)
    {
        currentSoundVolume = value;

        PlayerPrefs.SetFloat("soundVolume", value);
    }
}

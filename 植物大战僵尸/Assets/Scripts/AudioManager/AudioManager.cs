using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //单例模式,声音管理者
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
        //默认是0需要播放音乐,假如其他值不需要音乐
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
    /// 播放背景音乐
    /// </summary>
    /// <param name="ads">声音组件</param>
    /// <param name="musicFileName">音乐文件名(需要路径)</param>
    /// <param name="isLoop">是否为循环播放(默认值true)</param>
    public void PlayBGM(string musicFileName,bool isLoop = true)
    {
        if (audioSource == null) return;

        AudioClip clip = ResManager.instance.LoadMusicFromFile(musicFileName);
        if (clip == null)
        {
            Debug.Log("音乐文件:"+ musicFileName + "加载失败!");
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
        Debug.Log("AudioManager:暂停");
        audioSource.Pause();
    }

    public void resumBGM()
    {
        if (audioSource == null) return;
        if (audioSource.clip == null) return;
        Debug.Log("AudioManager:恢复");
        audioSource.Play();
    }
    public void PlaySound(AudioSource ads, string soundFileName, bool isLoop = false)
    {
        if (ads == null) return;
        AudioClip clip = null;
        clip = ResManager.instance.LoadSoundFromFile(soundFileName);
        if (clip == null)
        {
            Debug.Log("音效文件:" + soundFileName + "加载失败!");
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
    /// 停止播放音效
    /// </summary>
    /// <param name="ads"></param>
    public void stop(AudioSource ads)
    {
        if (ads == null) return;

        ads.Stop();
        ads.clip = null;

    }
    /// <summary>
    /// 播放背景音乐大小
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

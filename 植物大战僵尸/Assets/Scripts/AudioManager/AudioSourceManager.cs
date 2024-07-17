using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManager : MonoBehaviour
{
    public static AudioSourceManager Instance;

    public GameObject zombieEatPlant;
    public GameObject plantShoot;
    public AudioSource[] zombieEatPlantSource;
    public AudioSource[] plantShootSource;
    public AudioSource UISource;
    public AudioSource singleSource;
    public AudioSource getSunSource;
    public AudioSource waveSource;
    public AudioSource carSource;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        zombieEatPlant = transform.Find("ZombieEatPlantSource").gameObject;
        zombieEatPlantSource = zombieEatPlant.GetComponents<AudioSource>();
        plantShoot = transform.Find("PlantShootSource").gameObject;
        plantShootSource = plantShoot.GetComponents<AudioSource>();
        UISource = transform.Find("UISource").GetComponent<AudioSource>();
        singleSource = transform.Find("SingleSource").GetComponent<AudioSource>();
        getSunSource = GameObject.Find("GetSun").GetComponent<AudioSource>();
        waveSource = transform.Find("WaveSource").GetComponent<AudioSource>();
        carSource = transform.Find("CarSource").GetComponent<AudioSource>();
    }


    public void PlayPreBattleBGM()
    {
        AudioManager.Instance.PlayBGM("bgm3");
    }
    public void PlayBeginPlayBGM()
    {
        // AudioManager.Instance.PlayBGM();
    }
    public void PlayBattleBGM()
    {
        AudioManager.Instance.PlayBGM("DaytimeCombatMusic");
    }
    public void PlayWinMusic()
    {
        AudioManager.Instance.PlayBGM("winmusic", false);
    }
    public void PlayLoseMusic()
    {
        AudioManager.Instance.PlayBGM("losemusic", false);

    }
    public void PlayReadlySetPlantSound()
    {
        AudioManager.Instance.PlaySound(waveSource, "readysetplant");
    }
    public void PlayZombieCommingSound()
    {
        AudioManager.Instance.PlaySound(waveSource, "FirstWaveSound");
    }
    public void HugeWave()
    {
        AudioManager.Instance.PlaySound(waveSource, "hugewave");
    }
    public void PlayFinalWaveSound()
    {
        AudioManager.Instance.PlaySound(waveSource, "finalwave");
    }
    public void PlayCarSound(GameObject gameObject)
    {
        if(gameObject.GetComponent<AudioSource>() == null)
        {
           AudioManager.Instance.PlaySound(gameObject.AddComponent<AudioSource>(), "lawnmower");
        }
        else
        {
            AudioManager.Instance.PlaySound(gameObject.GetComponent<AudioSource>(), "lawnmower");
        }

    }
    public void PlayEatPlantSound()
    {
        bool isFind = false;
        for (int i = 0; i < zombieEatPlantSource.Length; i++)
        {

            if (zombieEatPlantSource[i].clip == null)
            {

                AudioManager.Instance.PlaySound(zombieEatPlantSource[i], "EatPlant");
                isFind = true;
                break;
            }
            else if (zombieEatPlantSource[i].clip.name == "EatPlant" && !zombieEatPlantSource[i].isPlaying)
            {

                zombieEatPlantSource[i].Play();
                isFind = true;
                break;
            }
        }
        if (!isFind)
        {
            AudioManager.Instance.PlaySound(zombieEatPlant.AddComponent<AudioSource>(), "EatPlant");
        }
        zombieEatPlantSource = zombieEatPlant.GetComponents<AudioSource>();
    }
    public void PlantShootSound()
    {
        bool isFind = false;
        for (int i = 0; i < plantShootSource.Length; i++)
        {
           
            if (plantShootSource[i].clip == null)
            {
                
                AudioManager.Instance.PlaySound(plantShootSource[i], "shoot");
                isFind = true;
                break;
            }
            else if (plantShootSource[i].clip.name == "shoot")
            {
               
                plantShootSource[i].Play();
                isFind = true;
                break;
            }
        }
        if (!isFind)
        {
            AudioManager.Instance.PlaySound(plantShoot.AddComponent<AudioSource>(), "shoot");
        }
        plantShootSource = plantShoot.GetComponents<AudioSource>();
    }
    
    public void PlayGetSunSound()
    {
        AudioManager.Instance.PlaySound(UISource, "GetSun");
    }
    public void PlayShovelSound()
    {
        AudioManager.Instance.PlaySound(UISource, "shovel");
    }
    public void OnClickBtnSound()
    {
        AudioManager.Instance.PlaySound(UISource, "buttonclick");
    }
    public void CherryBomb()
    {
        AudioManager.Instance.PlaySound(waveSource, "cherrybomb");
    }
    public void PotatoBomb()
    {
        AudioManager.Instance.PlaySound(waveSource, "potato_mine");
    }
    public void ShootBucketheadZombie()
    {
        bool isFind = false;
        for(int i = 0; i < plantShootSource.Length; i++)
        {
            if(plantShootSource[i].clip == null)
            {
                AudioManager.Instance.PlaySound(plantShootSource[i], "ShootBucketheadZombie");
                isFind = true;
                break;
            }
            else if(plantShootSource[i].clip.name == "ShootBucketheadZombie")
            {
                plantShootSource[i].Play();
                isFind = true;
                break;
            }
        }
        if (!isFind)
        {
                AudioManager.Instance.PlaySound(plantShoot.AddComponent<AudioSource>(), "ShootBucketheadZombie");
        }
        plantShootSource = plantShoot.GetComponents<AudioSource>();
    }
}

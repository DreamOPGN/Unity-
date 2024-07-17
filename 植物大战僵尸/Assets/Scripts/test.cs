using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public AudioSource[] sound;
    void Start()
    {
        sound = GameObject.Find("GameObject").GetComponents<AudioSource>();

        AudioManager.Instance.PlaySound(sound[0], "cherrybomb");
        AudioManager.Instance.PlaySound(sound[1], "logo");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

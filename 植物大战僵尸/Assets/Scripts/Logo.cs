using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviour
{
    private void Start()
    {
        Screen.SetResolution(800, 600, false);
        StartCoroutine(NextScene());
        AudioManager.Instance.PlaySound(gameObject.GetComponent<AudioSource>(),"LogoSound");
    }
    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(Config.SCENE_LOADING);
    }
}

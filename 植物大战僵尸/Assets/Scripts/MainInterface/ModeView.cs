using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModeView : MonoBehaviour
{
    public Sprite bright;
    public Sprite dark;
    public void ChangeSprite()
    {
        gameObject.GetComponent<Image>().sprite = bright;

    }
    public void ChangeSpriteDark()
    {
        gameObject.GetComponent<Image>().sprite = dark;

    }
    public void ChangeScene()
    {
        SceneManager.LoadScene(1);
        
    }
    public void StartBlinkSprite()
    {
        StartCoroutine(startDark());
    }
    IEnumerator startLight()
    {
        yield return new WaitForSeconds(0.2f);
        ChangeSprite();
        StartCoroutine(startDark());
    }
    IEnumerator startDark()
    {
        yield return new WaitForSeconds(0.2f);
        ChangeSpriteDark();
        StartCoroutine(startLight());
    }
    public void StopBlink()
    {
        StopAllCoroutines();
    }
}

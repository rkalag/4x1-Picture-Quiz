using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LevelInfo : MonoBehaviour
{
    [SerializeField]
    private GameObject bg = null;
    private RectTransform rect;
    Vector2 targetPos;
    private void Start()
    {
        rect = bg.GetComponent<RectTransform>();
        //StartCoroutine(AnimateLevelInfo());
        // bg.GetComponent<RectTransform>().offsetMin = new Vector2(0, 725);
        // targetPos = bg.GetComponent<RectTransform>().offsetMin;
        StartCoroutine(AnimateLevelInfo());
    }


    IEnumerator AnimateLevelInfo()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Canvas Height: " + Display.main.systemHeight);
        rect.DOSizeDelta(new Vector2(0, 960), 2);
       
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Game : MonoBehaviour
{
    [SerializeField]
    private GameObject bg;
    private RectTransform rt;
    [SerializeField]
    GameObject levelInfo = null;
    [SerializeField]
    private CanvasGroup levelNum;

    private JSONReader jsonReader;
    void Start()
    {
        if(levelInfo)
            levelInfo.SetActive(true);
        jsonReader = GameObject.FindGameObjectWithTag("JSONReader").GetComponent<JSONReader>();
        rt = bg.GetComponent<RectTransform>();
        StartCoroutine(jsonReader.DownloadImage());
        
    }

    public IEnumerator AnimateLevelInfo()
    {
        yield return new WaitForSeconds(2f);
        rt.DOAnchorPosY(Display.main.systemHeight, 0.5f).OnComplete(AnimationFinished);
        levelNum.DOFade(0, 0.5f);

    }
    private void AnimationFinished()
    {
        Debug.Log("____Animation Finished");
        levelInfo.SetActive(false);
    }

}

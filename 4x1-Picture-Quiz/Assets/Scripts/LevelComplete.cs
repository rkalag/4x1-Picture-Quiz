using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelComplete : MonoBehaviour
{
    [SerializeField]
    public Image bg;

    [SerializeField]
    public GameObject container;

    private void Start()
    {
        // gameObject.SetActive(false);


    }


    public void CallLevelComplete()
    {
        var tempColor = bg.color;
        tempColor.a = 0f;
        bg.color = tempColor;
        container.transform.localScale = new Vector3(0f, 0f);

        gameObject.SetActive(true);
        bg.DOFade(0.8f, 0.5f).SetEase(Ease.OutQuad);
        container.transform.DOScale(1, DataManager.POPUP_DURATION).SetEase(Ease.OutBack).OnComplete(LevelCompleteAppeared);
    }
    void LevelCompleteAppeared()
    {

    }
    public void CloseLevelComplete()
    {
        //FindObjectOfType<SoundManager>().Play("Click");
        bg.DOFade(0f, 0.5f).SetEase(Ease.InQuad);
        container.transform.DOScale(0, DataManager.POPUP_DURATION).SetEase(Ease.InBack).OnComplete(RemoveLevelComplete);
    }
    void RemoveLevelComplete()
    {
        var tempColor = bg.color;
        tempColor.a = 0f;
        bg.color = tempColor;

        container.transform.localScale = new Vector3(0f, 0f);
        gameObject.SetActive(false);
    }
}

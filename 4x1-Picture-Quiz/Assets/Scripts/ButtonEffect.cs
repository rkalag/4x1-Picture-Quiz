using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonEffect : MonoBehaviour
{
    private void Start()
    {
       // GetComponent<Button>().onClick.AddListener(() => ClickEffect());
    }
    void ClickEffect()
    {
        Debug.Log("____Clicked!");
        this.transform.DOScale(0.8f, 0.2f).OnComplete(Completed);
    }
    void Completed()
    {
        this.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack).OnComplete(Completed);
    }
}

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

    [SerializeField] Text levelNumber;

    private JSONReader jsonReader;

    [SerializeField] private GameObject levelCompleted = null;
    [SerializeField] private GameObject levelCompletedBG = null;
    [SerializeField] private GameObject levelCompletedText = null;
    [SerializeField] private Text levelCompletedTxt = null;
    [SerializeField] private string []textArray;

    [SerializeField] GameObject jokerBtn = null;
    void Start()
    {
        if(levelInfo)
            levelInfo.SetActive(true);
        if (levelCompleted)
            levelCompleted.SetActive(false);
        levelNumber.text = DataManager.CURRENT_LEVEL.ToString();
        jsonReader = GameObject.FindGameObjectWithTag("JSONReader").GetComponent<JSONReader>();
        rt = bg.GetComponent<RectTransform>();
        StartCoroutine(jsonReader.DownloadImage());


    }

    public IEnumerator AnimateLevelInfo()
    {
        yield return new WaitForSeconds(2f);
        rt.DOAnchorPosY(Display.main.systemHeight, 0.5f).OnComplete(LevelInfoDone);
        levelNum.DOFade(0, 0.5f);

    }
    private void LevelInfoDone()
    {
        Debug.Log("____LevelInfoDone");
        levelInfo.SetActive(false);
        Invoke("Shake", Random.Range(4f, 8f));
    }

    //Level Completed
    public void CallLevelCompleted()
    {
        Debug.Log("____ CallLevelCompleted");
        levelCompleted.SetActive(true);
        var tmp = Random.Range(0, textArray.Length);
        levelCompletedTxt.text = textArray[tmp];
        levelCompletedBG.GetComponent<RectTransform>().anchoredPosition = new Vector2(-(Display.main.systemWidth), 0);
        levelCompletedText.GetComponent<RectTransform>().anchoredPosition = new Vector2(Display.main.systemWidth, 0);
        StartCoroutine(AnimateLevelCompleted());
    }
    private IEnumerator AnimateLevelCompleted()
    {
        yield return new WaitForSeconds(0.5f);
        FindObjectOfType<SoundManager>().Play("LevelPassed");
        levelCompletedBG.GetComponent<RectTransform>().DOAnchorPosX(0, 0.3f);
        levelCompletedText.GetComponent<RectTransform>().DOAnchorPosX(0, 0.5f).OnComplete(LevelCompletedDone);
    }
    private void LevelCompletedDone()
    {
        Debug.Log("____LevelCompletedDone");
        StartCoroutine(ShowLevelInfo());
    }
    private IEnumerator ShowLevelInfo()
    {
        yield return new WaitForSeconds(2f);
        //levelCompleted.SetActive(false);
        // levelNumber.text = DataManager.CURRENT_LEVEL.ToString();
        // levelInfo.SetActive(true);
        // StartCoroutine(jsonReader.DownloadImage());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Shake()
    {
        Debug.Log("____Shaking");
        const float duration = 0.8f;
        const float strength = 0.1f;

        jokerBtn.GetComponent<Transform>().DOShakePosition(duration, strength);
        jokerBtn.GetComponent<Transform>().DOShakeRotation(duration, strength);
        jokerBtn.GetComponent<Transform>().DOShakeScale(duration, strength).OnComplete(ShakeComplete);
    }
    void ShakeComplete()
    {
        Invoke("Shake", Random.Range(4f, 8f));
    }

}

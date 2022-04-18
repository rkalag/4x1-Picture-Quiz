using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class Game : MonoBehaviour
{
    [SerializeField]
    private GameObject bg;
    private RectTransform rt;
    [SerializeField]
    GameObject levelInfo = null;
    [SerializeField]
    private CanvasGroup levelNum;

    [SerializeField] TextMeshProUGUI levelNumber;
    [SerializeField] TextMeshProUGUI levelNumberTxt_Top;

    private JSONReader jsonReader;

    [SerializeField] private GameObject levelCompleted = null;
    [SerializeField] private GameObject levelCompletedBG = null;
    [SerializeField] private GameObject levelCompletedText = null;
    [SerializeField] private TextMeshProUGUI levelCompletedTxt = null;
    [SerializeField] private string []textArray;

    public GameObject jokerBtn = null;
    public GameObject newJokerBtn = null;
    public GameObject backBtn = null;
    public TextMeshProUGUI jokerBtnTxt = null;
    [SerializeField] GameObject adLoading = null;
    [SerializeField] GameObject tutorial = null;
    public Image levelImg = null;

    private QuizManager quizManager;
    void Start()
    {
        levelNumberTxt_Top.text = DataManager.CURRENT_LEVEL.ToString();
        if (DataManager.TOTAL_JOKER > 0)
        {
            jokerBtn.SetActive(true);
            newJokerBtn.SetActive(false);
            levelImg.sprite = Resources.Load("orange-circle", typeof(Sprite)) as Sprite;
        }
        else
        {
            jokerBtn.SetActive(false);
            newJokerBtn.SetActive(true);
            levelImg.sprite = Resources.Load("blue-circle", typeof(Sprite)) as Sprite;
        }
        if(DataManager.IS_TUTORIAL)
        {
            jokerBtn.SetActive(false);
            newJokerBtn.SetActive(false);
            backBtn.SetActive(false);
        }
        if(tutorial)
        {
            tutorial.SetActive(false);
        }
        if(tutorial)
        {
            tutorial.SetActive(false);
        }
        if(levelInfo)
            levelInfo.SetActive(true);
        if (levelCompleted)
            levelCompleted.SetActive(false);
        if(DataManager.CURRENT_LEVEL >= 100)
            levelNumber.fontSize = 130;
        else
            levelNumber.fontSize = 180;
        levelNumber.text = DataManager.CURRENT_LEVEL.ToString();
        jsonReader = GameObject.FindGameObjectWithTag("JSONReader").GetComponent<JSONReader>();
        rt = bg.GetComponent<RectTransform>();
        jokerBtnTxt.text = DataManager.TOTAL_JOKER + " Joker";
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
        if(DataManager.IS_TUTORIAL && DataManager.CURRENT_LEVEL == 1)
        {
            StartCoroutine(ShowTutorial());
        }
    }
    private IEnumerator ShowTutorial()
    {
        yield return new WaitForSeconds(0.2f);
        tutorial.SetActive(true);
        GameObject.FindGameObjectWithTag("QuizManager").GetComponent<QuizManager>().SetPosOfTut();
        
        DataManager.IS_TUTORIAL = true;
    }
    public void RemoveTutorial()
    {
        if(tutorial.activeSelf)
        {
            Destroy(tutorial);
            tutorial = null;
            DataManager.IS_TUTORIAL = false;
            PlayerData.SavePlayerData();
        }
    }
   

    //Level Completed
    public void CallLevelCompleted()
    {
        Debug.Log("____ CallLevelCompleted");
        PlayerData.SavePlayerData();
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
        Debug.Log("____LevelCompletedDone "+ DataManager.REMOVE_ADS);
        if (DataManager.REMOVE_ADS || DataManager.BUILD_TYPE == "Unity")
        {
            ContinueGameAfterInterstitial();
        }
        else
        {
            Debug.Log("____DataManager.CAN_SHOW_INTERSTITIAL " + DataManager.CAN_SHOW_INTERSTITIAL);
            if ((DataManager.CAN_SHOW_INTERSTITIAL || DataManager.FIRST_TIME_AD) && (DataManager.CURRENT_LEVEL == 3 || DataManager.CURRENT_LEVEL % 5 == 0))
            {

                adLoading.SetActive(true);
                StartCoroutine(ShowInterstitialAd());
                //Application.ExternalCall("ShowAd_Interstitial", "LevelClear_AD");
            }
            else
                ContinueGameAfterInterstitial();
        }
        
    }
    public void ContinueGameAfterInterstitial()
    {
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
        const float strength = 0.5f;
        if(jokerBtn.activeSelf)
        {
            jokerBtn.GetComponent<Transform>().DOShakePosition(duration, strength);
            jokerBtn.GetComponent<Transform>().DOShakeRotation(duration, strength);
            jokerBtn.GetComponent<Transform>().DOShakeScale(duration, strength).OnComplete(ShakeComplete);
        }
        else
        {
            newJokerBtn.GetComponent<Transform>().DOShakePosition(duration, strength);
            newJokerBtn.GetComponent<Transform>().DOShakeRotation(duration, strength);
            newJokerBtn.GetComponent<Transform>().DOShakeScale(duration, strength).OnComplete(ShakeComplete);
        }
        
    }
    void ShakeComplete()
    {
        Invoke("Shake", Random.Range(4f, 8f));
    }
    IEnumerator ShowInterstitialAd()
    {
        yield return new WaitForSeconds(2f);
        if (adLoading.activeSelf)
        {
            adLoading.SetActive(false);
        }
        Application.ExternalCall("ShowAd_Interstitial", "LevelClear_AD");
    }
    public void GotJokerAfterReward()
    {
        DataManager.TOTAL_JOKER = DataManager.TOTAL_JOKER + 3;
        jokerBtn.SetActive(true);
        jokerBtnTxt.text = DataManager.TOTAL_JOKER.ToString() + " Joker";
        newJokerBtn.SetActive(false);
        PlayerData.SavePlayerData();
        levelImg.sprite = Resources.Load("orange-circle", typeof(Sprite)) as Sprite;
    }
    public void LoadMenu()
    {
        if (DataManager.IS_TUTORIAL) return;
        Initiate.Fade("Menu", Color.black, 2f);
    }



}

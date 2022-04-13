
using UnityEngine;
public class AdTimer : MonoBehaviour
{

    public static float interstitialCntr = 100;
    private bool timerIsActive = false;

    void Awake()
    {
        int n = FindObjectsOfType<AdTimer>().Length;
        if (n != 1)
        {
            Destroy(this.gameObject);
        }
        // if more then once is in the scene
        //destroy ourselves
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Update()
    {
        if (DataManager.BUILD_TYPE == "Unity" || DataManager.FIRST_TIME_AD) return;
        if (DataManager.REMOVE_ADS)
        {
            timerIsActive = false;
            DataManager.CAN_SHOW_INTERSTITIAL = false;
            return;
        }
        if (timerIsActive)
        {
            interstitialCntr -= Time.deltaTime;
            Debug.Log("123 : " + interstitialCntr + " " + DataManager.CAN_SHOW_INTERSTITIAL);

            if (interstitialCntr <= 0)
            {
                interstitialCntr = 0;
                timerIsActive = false;
                DataManager.CAN_SHOW_INTERSTITIAL = true;
            }
        }

    }
    public void ResetTimerAfterInterstitial()
    {
        DataManager.FIRST_TIME_AD = false;
        interstitialCntr = 100;
        timerIsActive = true;
        DataManager.CAN_SHOW_INTERSTITIAL = false;
    }
}


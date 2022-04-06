using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.IO;
using SimpleJSON;

public class PlayerData : MonoBehaviour
{
   

    Menu menu;
    private void Awake()
    {
       

//#if UNITY_EDITOR
//        Debug.unityLogger.logEnabled = true;
//#else
//  Debug.unityLogger.logEnabled = false;
//#endif


        Debug.Log("______Player Data Awake");

        int n = FindObjectsOfType<PlayerData>().Length;
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

    public static void SavePlayerData()
    {
        string data = DataManager.CURRENT_LEVEL + ":"  + DataManager.IS_SOUND + ":" + DataManager.REMOVE_ADS + ":" + DataManager.IS_TUTORIAL+":"+DataManager.TOTAL_JOKER;

        Application.ExternalCall("SavePlayerData", data);
    }
    public void GetPlayerData(string data)
    {
        Debug.Log("_______________ RecievePlayerData" + data);
        JSONNode playerData;
        playerData = JSON.Parse(data);


<<<<<<< HEAD
        DataManager.CURRENT_LEVEL = playerData["currentLevel_3"];
        DataManager.IS_SOUND = playerData["isSound_3"];
        DataManager.REMOVE_ADS = playerData["removeAds_3"];
        DataManager.IS_TUTORIAL = playerData["isTutorial_3"];
        DataManager.TOTAL_JOKER = playerData["totalJoker_3"];
=======
        DataManager.CURRENT_LEVEL = playerData["currentLevel_1"];
        DataManager.IS_SOUND = playerData["isSound_1"];
        DataManager.REMOVE_ADS = playerData["removeAds_1"];
        DataManager.IS_TUTORIAL = playerData["isTutorial_1"];
        DataManager.TOTAL_JOKER = playerData["totalJoker_1"];
>>>>>>> 339f603c6eef32e958d5974ff71c31f79bc42da5

        Debug.Log(DataManager.CURRENT_LEVEL + "__________ RecievePlayerData22");


        if (DataManager.CURRENT_LEVEL > DataManager.TOTAL_LEVELS)
        {
            DataManager.CURRENT_LEVEL = 1;
        }
        DataManager.DATA_LOADED = true;
        StartCoroutine(LoadMenu());
        //Initiate.Fade("Menu", Color.black, 2f);
        //menu = GameObject.FindGameObjectWithTag("Menu").GetComponent<Menu>();
        //menu.Menu_Awake();
        //menu.Menu_Start();
        // GameManager.DATA_LOADED = true;
        //menu = GameObject.FindGameObjectWithTag("Menu").GetComponent<MenuManager>();
        //menu.MenuSetup_Awake();
        //menu.MenuSetup();
    }
    IEnumerator LoadMenu()
    {
        yield return new WaitForSeconds(2f);
        Initiate.Fade("Menu", Color.black, 2f);
        //menu = GameObject.FindGameObjectWithTag("Menu").GetComponent<Menu>();
        //menu.Menu_Awake();
        //menu.Menu_Start();

    }
    public static void ResetPlayerData()
    {
        //PlayerPrefs.DeleteAll();
    }

    public void GetMyName(string name)
    {
        DataManager.PLAYER_NAME = name;

    }

   
    public void GetMobileOS(string osType)
    {
        Debug.Log("OS: " + osType);
        DataManager.OS_TYPE = osType;
    }
    public void MobileOrDesktop(string deviceType)
    {
        Debug.Log("Device: " + deviceType);
        DataManager.DEVICE_TYPE = deviceType;

    }
}

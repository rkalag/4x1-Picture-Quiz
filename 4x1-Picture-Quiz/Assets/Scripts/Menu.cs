using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] Text titleTxt;
    [SerializeField] Text btnTxt;

    [SerializeField] GameObject onImg;
    [SerializeField] Text onTxt;
    [SerializeField] GameObject offImg;
    [SerializeField] Text offTxt;
    // Start is called before the first frame update
    private void Awake()
    {
        
        Debug.Log("DataManager.DATA_LOADED___ " + DataManager.DATA_LOADED);
        if (DataManager.DATA_LOADED)
        {
            Menu_Awake();
        }
    }
    public void Menu_Awake()
    {
        SetSound();
        //Application.ExternalCall("LoadBanner");
        Debug.Log("DataManager.CURRENT_LEVEL: " + DataManager.CURRENT_LEVEL);
       if(DataManager.CURRENT_LEVEL > 1)
        {
            titleTxt.text = "Welcome back! " + DataManager.PLAYER_NAME;
            titleTxt.fontSize = 33;
            btnTxt.text = "Proceed with Level " + DataManager.CURRENT_LEVEL + "!";
            btnTxt.fontSize = 38;
        }
    }
    public void SetSoundOn()
    {
        AudioListener.volume = 1f;
        onImg.SetActive(true);
        onTxt.color = Color.white;
        offImg.SetActive(false);
        offTxt.color = Color.black;
        DataManager.IS_SOUND = true;
        PlayerData.SavePlayerData();
    }
    public void SetSoundOff()
    {
        AudioListener.volume = 0f;
        onImg.SetActive(false);
        onTxt.color = Color.black;
        offImg.SetActive(true);
        offTxt.color = Color.white;
        DataManager.IS_SOUND = false;
        PlayerData.SavePlayerData();
    }
    private void SetSound()
    {
        DataManager.IS_SOUND = false;
        Debug.Log("___________Sound: " + DataManager.IS_SOUND);
        if (DataManager.IS_SOUND)
        {
            AudioListener.volume = 1f;
            onImg.SetActive(true);
            onTxt.color = Color.white;
            offImg.SetActive(false);
            offTxt.color = Color.black;
        }
        else
        {
            AudioListener.volume = 0f;
            onImg.SetActive(false);
            onTxt.color = Color.black;
            offImg.SetActive(true);
            offTxt.color = Color.white;
        }
    }
    public void Menu_Start()
    {

    }
    void Start()
    {
        
    }
    public void LoadGame()
    {
        Initiate.Fade("Game", Color.black, 2f);
    }
}

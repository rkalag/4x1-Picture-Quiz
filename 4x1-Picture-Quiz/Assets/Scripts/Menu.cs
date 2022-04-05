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

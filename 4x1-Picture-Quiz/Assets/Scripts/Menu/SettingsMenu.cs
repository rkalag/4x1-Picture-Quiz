using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingsMenu : MonoBehaviour
{
    [Header("space between menu items")]
    [SerializeField] Vector2 spacing;

    [Space]
    [Header("Setting button rotation")]
    [SerializeField] float rotationDuration;
    [SerializeField] Ease rotationEase;

    [Space]
    [Header("Animation")]
    [SerializeField] float expandDuration;
    [SerializeField] float collapseDuration;
    [SerializeField] Ease expandEase;
    [SerializeField] Ease collapseEase;

    [Space]
    [Header("Fading")]
    [SerializeField] float expandFadeDuration;
    [SerializeField] float collapseFadeDuration;


    Button settingButton;
    SettingsMenuItem[] menuItems;
    bool isExpanded = false;

    Vector2 settingButtonPosition;
    int itemsCount;

    RectTransform rectTrans;
    [Space]
    [Header("Music and Sound toggle sprites")]
    public Sprite mscOnSprite;
    public Sprite mscOffSprite;

    SettingsMenuItem settingsMenuItem;

    void Start()
    {
        rectTrans = transform.GetChild(0).GetComponent<RectTransform>();
        itemsCount = transform.childCount - 1;
        menuItems = new SettingsMenuItem[itemsCount];
        //settingsMenuItem = GameObject.FindGameObjectWithTag("SoundBtn").GetComponent<SettingsMenuItem>();
        //settingsMenuItem.SetSoundButton();

        for (int i = 0; i < itemsCount; i++)
        {
            menuItems[i] = transform.GetChild(i + 1).GetComponent<SettingsMenuItem>();
        }
        settingButton = transform.GetChild(0).GetComponent<Button>();
        settingButton.onClick.AddListener(ToggleMenu);
        settingButton.transform.SetAsLastSibling();
        Debug.Log("_________Sound");
        settingButtonPosition = rectTrans.anchoredPosition;

        //Reset all menu items position to setting button 
        ResetPositions();

        //settingMenuItem = new SettingMenuItem();
    }
    void ResetPositions()
    {

        for (int i = 0; i < itemsCount; i++)
        {
            menuItems[i].trans.anchoredPosition = settingButtonPosition;
            print(menuItems[i].trans.anchoredPosition);
        }
    }
    public void ToggleMenu()
    {
        //FindObjectOfType<SoundManager>().Play("Click");
        isExpanded = !isExpanded;
        print("toggle" + isExpanded);
        if (isExpanded)
        {
            // menu items opened
            for (int i = 0; i < itemsCount; i++)
            {
                // menuItems[i].trans.anchoredPosition = settingButtonPosition + spacing*(i+1) ;
                menuItems[i].trans.DOAnchorPos(settingButtonPosition + spacing * (i + 1), expandDuration).SetEase(expandEase);
                menuItems[i].img.DOFade(1f, expandFadeDuration).From(0f);
            }
            //Rotate setting button
            //settingButton.transform
            //    .DORotate(Vector3.forward * 180f, rotationDuration)
            //    .From(Vector3.zero)
            //    .SetEase(rotationEase);
        }
        else
        {
            //menu items closed
            for (int i = 0; i < itemsCount; i++)
            {
                //menuItems[i].trans.anchoredPosition = settingButtonPosition;
                //print(menuItems[i].trans.anchoredPosition);
                menuItems[i].trans.DOAnchorPos(settingButtonPosition, collapseDuration).SetEase(collapseEase);
                menuItems[i].img.DOFade(0f, collapseFadeDuration);
            }
            //Rotate setting button
            //settingButton.transform
            //    .DORotate(Vector3.forward * -180f, rotationDuration)
            //    .From(Vector3.zero)
            //    .SetEase(rotationEase);

        }

    }
    public void OnItemClick(int index)
    {
       // print("item" + index + "clicked");
        switch (index)
        {
            case 0:
                print("_____Sound Button");
               // FindObjectOfType<SoundManager>().Play("Click");
                // settingMenuItem.button.image.sprite = mscBtnSprite;
                break;
            case 1:
                print("_____Sound Button");
               // FindObjectOfType<SoundManager>().Play("Click");
                // settingMenuItem.button.image.sprite = sndBtnSprite;
                break;
            case 2:
                //print("_____Vibration Button");
                break;
        }
    }
    void OnDestroy()
    {
        // settingButton.onClick.RemoveListener(ToggleMenu);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuItem : MonoBehaviour
{
    [HideInInspector] public Image img;
    [HideInInspector] public RectTransform trans;
    SettingsMenu settingMenu;
    int index;
    [HideInInspector] public Button button;

    void Awake()
    {
        img = GetComponent<Image>();
        trans = GetComponent<RectTransform>();

        settingMenu = transform.parent.GetComponent<SettingsMenu>();
        index = trans.GetSiblingIndex() - 1;
        button = GetComponent<Button>();
        button.onClick.AddListener(OnItemClicked);
    }
    void OnItemClicked()
    {
        print("+++++ DataManager.IS_SOUND: " + DataManager.IS_SOUND);
        DataManager.IS_SOUND = !DataManager.IS_SOUND;
        if (button.image.sprite == settingMenu.mscOnSprite)
        {
            button.image.sprite = settingMenu.mscOffSprite;
            AudioListener.volume = 0f;
        }
        else if (button.image.sprite == settingMenu.mscOffSprite)
        {
            button.image.sprite = settingMenu.mscOnSprite;
            AudioListener.volume = 0.7f;
        }
        settingMenu.OnItemClick(index);
    }
    void OnDestroy()
    {
        // button.onClick.RemoveListener(OnItemClicked);
    }
    public void SetSoundButton()
    {
        Debug.Log("DataManager.IS_SOUND " + DataManager.IS_SOUND);
        if (DataManager.IS_SOUND)
        {
            if (button.image.sprite == settingMenu.mscOnSprite)
            {
                button.image.sprite = settingMenu.mscOnSprite;
            }
            else if (button.image.sprite == settingMenu.mscOffSprite)
            {
                button.image.sprite = settingMenu.mscOnSprite;
            }
            AudioListener.volume = 0.7f;
        }
        else
        {
            if (button.image.sprite == settingMenu.mscOnSprite)
            {
                button.image.sprite = settingMenu.mscOffSprite;
            }
            else if (button.image.sprite == settingMenu.mscOffSprite)
            {
                button.image.sprite = settingMenu.mscOffSprite;
            }
            AudioListener.volume = 0f;
        }
    }
}

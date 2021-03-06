using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class WordData : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI charText;
    [HideInInspector]
    public char charValue;
    public GameObject gTab = null;
    public GameObject emptyBox = null;
    public GameObject deleteBtn = null;
    private Button buttonObj;

    public CanvasGroup emptyBoxCG;
    public CanvasGroup gTabCG;

    [HideInInspector]
    public int ind;

    private void Awake()
    {
        if(gTab)
        {
            gTab.SetActive(false);
        }
        if(deleteBtn)
        {
            deleteBtn.SetActive(false);
        }
        buttonObj = GetComponent<Button>();
        if(buttonObj)
        {
            buttonObj.onClick.AddListener(() => CharSelected());
            
        }
    }
    
    public void GetIndex(int index)
    {
        ind = index;
    }
    public void SetChar(char value)
    {
        charText.text = value + "";
        charValue = value;
    }
    public char GetChar()
    {
        string str = charText.text;
        char chr = char.Parse(str);
        return chr;
    }
    private void CharSelected()
    {
        QuizManager.instance.SelectedOption(this);
    }
}

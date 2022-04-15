using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QuizManager : MonoBehaviour
{
    public static QuizManager instance;

    [SerializeField] GameObject answerWordArrayObj, answerWordArray2Obj, optionWordArrayObj, optionWordArray2Obj;

    [SerializeField]
    private WordData[] answerWordArray;

    [SerializeField]
    private WordData[] answerWordArray2;



    [SerializeField]
    private WordData[] optionWordArray;

    [SerializeField]
    private WordData[] optionWordArray2;

    [SerializeField]
    public Image questionImg;

    [SerializeField] GameObject removeAllBtn = null;
    [SerializeField] GameObject removeAllBtn2 = null;

    private int index;


    private char[] charArray = new char[8];
    private int currentAnswerIndex = 0;
    private bool correctAnswer = true;
    //private int wordNum = true;

    public List<int> selectedCharIndex;

    public int currentQuestionIndex = 0;
    private GameStatus gameStatus = GameStatus.Playing;

    [SerializeField]
    private Sprite sp = null;

    private string answerWord;
    // Start is called before the first frame update

   



  

    private Game game;
    public JSONReader jsonReader;
    private int questionCount;

    public float timeTaken = 0;

    Texture2D texture;
    private int tutorialCntr = 1;

    private char[] tutorialCharArray = {'A', 'V', 'D', 'F', 'G', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'U', 'V', 'W', 'X', 'Y', 'Z' };


   

    private void Awake()
    {
        jsonReader = GameObject.FindGameObjectWithTag("JSONReader").GetComponent<JSONReader>();
        //currentQuestionIndex = DataManager.CURRENT_LEVEL - 1;
        game = GameObject.FindGameObjectWithTag("Game").GetComponent<Game>();
        Debug.Log(DataManager.CURRENT_LEVEL + " -------------");
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        selectedCharIndex = new List<int>();
    }
    private void Start()
    {
        texture = Resources.Load<Texture2D>("Sprites/GreenTab");
        //questionCount = jsonReader.jsonData.Count - 1;
        // SetQuestion();
    }
    public void SetQuestion()
    {
        Debug.Log("SetQuestion: ");
        timeTaken = 0;
        currentAnswerIndex = 0;
        selectedCharIndex.Clear();
        questionImg.sprite = Sprite.Create(jsonReader.questionList[0], new Rect(0, 0, 475, 475), Vector2.zero);


        answerWord = jsonReader.jsonData[DataManager.CURRENT_LEVEL]["answer"];
        DataManager.ANSWER = answerWord;
        Debug.Log("answerWord.Length: " +answerWord +"_____"+ answerWord.Length);
        if(answerWord.Length <= 8)
        {
            answerWordArrayObj.SetActive(true);
            optionWordArrayObj.SetActive(true);
            answerWordArray2Obj.SetActive(false);
            optionWordArray2Obj.SetActive(false);
            charArray = new char[8];
        }
        else
        {
            answerWordArrayObj.SetActive(false);
            optionWordArrayObj.SetActive(false);
            answerWordArray2Obj.SetActive(true);
            optionWordArray2Obj.SetActive(true);
            charArray = new char[12];
            if (answerWord.Length == 12)
            {
                for (int i = 0; i < answerWordArray2.Length; i++)
                {
                    answerWordArray2[i].gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(34f, 35.35f);
                }
            }
            else
            {
                for (int i = 0; i < answerWordArray2.Length; i++)
                {
                    answerWordArray2[i].gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(38.1f, 39.65f);
                }
            }

        }
        ResetQuestion();
        for (int i = 0; i < answerWord.Length; i++)
        {
            charArray[i] = char.ToUpper(answerWord[i]);
        }
        if(answerWord.Length <= 8)
        {
            if(/*DataManager.CURRENT_LEVEL == 1 &&*/ DataManager.IS_TUTORIAL)
            {
                for (int i = answerWord.Length; i < optionWordArray.Length; i++)
                {
                    charArray[i] = (char)UnityEngine.Random.Range(85, 91);

                }
                charArray = ShuffleList.ShuffleListItems<char>(charArray.ToList()).ToArray();

                for (int i = 0; i < optionWordArray.Length; i++)
                {
                    optionWordArray[i].SetChar(charArray[i]);
                }
            }
            else
            {
                for (int i = answerWord.Length; i < optionWordArray.Length; i++)
                {
                    charArray[i] = (char)UnityEngine.Random.Range(65, 91);

                }
                charArray = ShuffleList.ShuffleListItems<char>(charArray.ToList()).ToArray();

                for (int i = 0; i < optionWordArray.Length; i++)
                {
                    optionWordArray[i].SetChar(charArray[i]);
                }
            
            }
            
        }
        else
        {
            for (int i = answerWord.Length; i < optionWordArray2.Length; i++)
            {
                charArray[i] = (char)UnityEngine.Random.Range(65, 91);

            }
            charArray = ShuffleList.ShuffleListItems<char>(charArray.ToList()).ToArray();

            for (int i = 0; i < optionWordArray2.Length; i++)
            {
                optionWordArray2[i].SetChar(charArray[i]);
            }
        }
        
        currentQuestionIndex++;
        gameStatus = GameStatus.Playing;
    }
    
    public void SelectedOption(WordData wordData)
    {
        if(DataManager.IS_TUTORIAL)
        {
            Debug.Log("wordData.charValue: " + wordData.charValue+"____"+answerWordArray);
            if (wordData.charValue == 'C' && tutorialCntr == 1)
                tutorialCntr++;
            else if (wordData.charValue == 'H' && tutorialCntr == 2)
                tutorialCntr++;
            else if (wordData.charValue == 'E' && tutorialCntr == 3)
                tutorialCntr++;
            else if (wordData.charValue == 'S' && tutorialCntr == 4)
                tutorialCntr++;
            else if (wordData.charValue == 'T' && tutorialCntr == 5)
            {
                tutorialCntr++;
                game.RemoveTutorial();
            }
            else
            {
                return;
            }
        }
        if (wordData.gTab != null)
            if (wordData.gTab.activeSelf) return;
        if (wordData.emptyBox != null)
            if (wordData.emptyBox.activeSelf) return;
        Debug.Log(currentAnswerIndex + "answerWord.Length____" + answerWord.Length);
        if (currentAnswerIndex >= answerWord.Length) return;
        if(wordData.CompareTag("ans"))
        {
            Debug.Log("ans");
            return;
        }
        else
        {
            Debug.Log("opt");
        }
        
        //Debug.Log(currentAnswerIndex + "____" + answerWordArray.Length);
        if (gameStatus == GameStatus.Next) return;
        selectedCharIndex.Add(wordData.transform.GetSiblingIndex());
        //answerWordArray[currentAnswerIndex].SetChar(wordData.charValue);
        //answerWordArray[currentAnswerIndex].GetComponent<Image>().sprite = sp;
        // selectedChar.Add(wordData.gameObject);
        
        wordData.emptyBox.SetActive(true);
        wordData.emptyBoxCG.alpha = 0;
        wordData.emptyBoxCG.DOFade(1, 0.3f);
        if (DataManager.IS_TUTORIAL)
            SetPosOfTut();
        currentAnswerIndex++;
        if(answerWord.Length <= 8)
        {
            for (int i = 0; i < answerWordArray.Length; i++)
            {
                if(!DataManager.IS_TUTORIAL /*&& DataManager.CURRENT_LEVEL != 1*/)
                {
                    if (answerWordArray[i].deleteBtn.activeSelf)
                        answerWordArray[i].deleteBtn.SetActive(false);
                }
                
                if (!answerWordArray[i].gTab.activeSelf)
                {
                    answerWordArray[i].GetComponent<Image>().enabled = false;
                    answerWordArray[i].gTab.SetActive(true);
                    answerWordArray[i].gTabCG.alpha = 0;
                    answerWordArray[i].gTabCG.DOFade(1, 0.3f);
                    if (!DataManager.IS_TUTORIAL /*&& DataManager.CURRENT_LEVEL != 1*/)
                        answerWordArray[i].deleteBtn.SetActive(true);
                    answerWordArray[i].SetChar(wordData.charValue);
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < answerWordArray2.Length; i++)
            {
                if (answerWordArray2[i].deleteBtn.activeSelf)
                    answerWordArray2[i].deleteBtn.SetActive(false);
                if (!answerWordArray2[i].gTab.activeSelf)
                {
                    answerWordArray2[i].GetComponent<Image>().enabled = false;
                    answerWordArray2[i].gTab.SetActive(true);
                    answerWordArray2[i].gTabCG.alpha = 0;
                    answerWordArray2[i].gTabCG.DOFade(1, 0.3f);
                    answerWordArray2[i].deleteBtn.SetActive(true);
                    answerWordArray2[i].SetChar(wordData.charValue);
                    break;
                }
            }
        }
        
        FindObjectOfType<SoundManager>().Play("Set");
        CheckAnswer();
    }
    public void ResetLastCharacter(Button btn)
    {
       // Debug.Log("btn: " + btn);
       // Debug.Log("btn: " + btn.transform.GetChild(0).gameObject.transform.GetChild(1));
        if(!btn.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.activeSelf)
        {
            Debug.Log("qqqqqqqq");
            return;
        }
        if (answerWord.Length <= 8)
        {
            for (int i = 0; i < answerWordArray.Length; i++)
            {
                answerWordArray[i].gTab.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                answerWordArray[i].gTab.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, 200, 204), Vector2.zero);
            }
        }
        else
        {
            for (int i = 0; i < answerWordArray2.Length; i++)
            {
                answerWordArray2[i].gTab.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                answerWordArray2[i].gTab.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, 200, 204), Vector2.zero);
            }
        }


        
        if (removeAllBtn.activeSelf)
            removeAllBtn.SetActive(false);
        if (removeAllBtn2.activeSelf)
            removeAllBtn2.SetActive(false);

        //Debug.Log("selectedCharIndex.Count: " + selectedCharIndex.Count);
        //Debug.Log("aaaaa: " + currentAnswerIndex +"___"+ answerWordArray[currentAnswerIndex - 1]);
        if (answerWord.Length <= 8)
        {
            answerWordArray[currentAnswerIndex - 1].deleteBtn.SetActive(false);
        }
        else
        {
            answerWordArray2[currentAnswerIndex - 1].deleteBtn.SetActive(false);
        }
        
        if (selectedCharIndex.Count > 0)
        {
            index = selectedCharIndex[selectedCharIndex.Count - 1];
            Debug.Log("_______________iiindeX::: " + index);
            //optionWordArray[index].emptyBox.SetActive(false);
            if (answerWord.Length <= 8)
            {
                
                optionWordArray[index].emptyBoxCG.alpha = 1;
                optionWordArray[index].emptyBoxCG.DOFade(0, 0.3f);
            }
            else
            {
                optionWordArray2[index].emptyBoxCG.alpha = 1;
                optionWordArray2[index].emptyBoxCG.DOFade(0, 0.3f);
            }
            
            selectedCharIndex.RemoveAt(selectedCharIndex.Count - 1);
            if(answerWord.Length <= 8)
            {
                answerWordArray[currentAnswerIndex - 1].GetComponent<Image>().enabled = true;
                answerWordArray[currentAnswerIndex - 1].gTabCG.alpha = 1;
                answerWordArray[currentAnswerIndex - 1].gTabCG.DOFade(0, 0.3f).OnComplete(gTabAlphaGone);
                answerWordArray[currentAnswerIndex - 1].deleteBtn.SetActive(false);
                if (currentAnswerIndex - 2 >= 0)
                    answerWordArray[currentAnswerIndex - 2].deleteBtn.SetActive(true);
            }
            else
            {
                answerWordArray2[currentAnswerIndex - 1].GetComponent<Image>().enabled = true;
                answerWordArray2[currentAnswerIndex - 1].gTabCG.alpha = 1;
                answerWordArray2[currentAnswerIndex - 1].gTabCG.DOFade(0, 0.3f).OnComplete(gTabAlphaGone);
                answerWordArray2[currentAnswerIndex - 1].deleteBtn.SetActive(false);
                if (currentAnswerIndex - 2 >= 0)
                    answerWordArray2[currentAnswerIndex - 2].deleteBtn.SetActive(true);
            }

            
        }
        FindObjectOfType<SoundManager>().Play("Delete");
        Debug.Log("currentAnswerIndex:: " + currentAnswerIndex);

    }
    void gTabAlphaGone()
    {
        //int index = selectedCharIndex[selectedCharIndex.Count - 1];
        //Debug.Log("index: " + index + "____currentAnswerIndex: " + currentAnswerIndex);
        if(answerWord.Length <= 8)
        {
            optionWordArray[index].emptyBoxCG.alpha = 0;
            optionWordArray[index].emptyBox.SetActive(false);
        }
        else
        {
            optionWordArray2[index].emptyBoxCG.alpha = 0;
            optionWordArray2[index].emptyBox.SetActive(false);
        }
        

        if(answerWord.Length <= 8)
        {
            answerWordArray[currentAnswerIndex - 1].GetComponent<Image>().enabled = true;
            answerWordArray[currentAnswerIndex - 1].gTab.SetActive(false);
            answerWordArray[currentAnswerIndex - 1].gTabCG.alpha = 0;
            answerWordArray[currentAnswerIndex - 1].deleteBtn.SetActive(false);
        }
        else
        {
            answerWordArray2[currentAnswerIndex - 1].GetComponent<Image>().enabled = true;
            answerWordArray2[currentAnswerIndex - 1].gTab.SetActive(false);
            answerWordArray2[currentAnswerIndex - 1].gTabCG.alpha = 0;
            answerWordArray2[currentAnswerIndex - 1].deleteBtn.SetActive(false);
        }

        
        
        currentAnswerIndex--;
    }
    void CheckAnswer()
    {
        if (currentAnswerIndex >= answerWord.Length)
        {
            correctAnswer = true;
            for (int i = 0; i < answerWord.Length; i++)
            {
                if(answerWord.Length <= 8)
                {
                    if (char.ToUpper(answerWord[i]) != char.ToUpper(answerWordArray[i].charValue))
                    {
                        correctAnswer = false;
                        break;
                    }
                }
                else
                {
                    if (char.ToUpper(answerWord[i]) != char.ToUpper(answerWordArray2[i].charValue))
                    {
                        correctAnswer = false;
                        break;
                    }
                }
                
            }
            if (correctAnswer)
            {
                Debug.Log("_____CORRECT!!___"+ currentQuestionIndex+"_____"+ questionCount);
                gameStatus = GameStatus.Next;
                DataManager.CURRENT_LEVEL++;
                jsonReader.questionList.Clear();
                game.CallLevelCompleted();
                if (currentQuestionIndex <= questionCount)
                {
                    // questionImg.sprite = Sprite.Create(jsonReader.solutionList[DataManager.CURRENT_SUB_LEVEL - 1], new Rect(0, 0, 662, 545), Vector2.zero);
                    // questionImg.sprite = questionData.questions[currentQuestionIndex-1].solutionImage;
                    // Invoke("ShowDescription", 1.5f);
                   // game.CallLevelCompleted();
                   // DataManager.CURRENT_SUB_LEVEL++;
                }
            }
            else
            {
                Debug.Log("_____WRONG!");
                if(answerWord.Length <= 8)
                {
                    for (int i = 0; i < answerWordArray.Length; i++)
                    {
                        answerWordArray[i].gTab.GetComponent<Image>().color = new Color(0.5764706f, 0, 0, 1f);
                        answerWordArray[i].gTab.GetComponent<Image>().sprite = null;
                    }
                    removeAllBtn.SetActive(true);
                }
                else
                {
                    for (int i = 0; i < answerWordArray2.Length; i++)
                    {
                        answerWordArray2[i].gTab.GetComponent<Image>().color = new Color(0.5764706f, 0, 0, 1f);
                        answerWordArray2[i].gTab.GetComponent<Image>().sprite = null;
                    }
                    removeAllBtn2.SetActive(true);
                }

                Application.ExternalCall("VibrateDevice", "100");
                FindObjectOfType<SoundManager>().Play("Fail");
            }
        }
    }
    public void ResetQuestion()
    {
       // Debug.Log("answerWordArray.Length____" + answerWordArray.Length);
       // Debug.Log("optionWordArray.Length____" + optionWordArray.Length);
        if(answerWord.Length <= 8)
        {
            for (int i = 0; i < answerWordArray.Length; i++)
            {
                answerWordArray[i].gameObject.SetActive(true);
                //answerWordArray[i].SetChar('_');
            }
            for (int i = answerWord.Length; i < answerWordArray.Length; i++)
            {
                answerWordArray[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < optionWordArray.Length; i++)
            {
                optionWordArray[i].gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < answerWordArray2.Length; i++)
            {
                answerWordArray2[i].gameObject.SetActive(true);
            }
            for (int i = answerWord.Length; i < answerWordArray2.Length; i++)
            {
                answerWordArray2[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < optionWordArray2.Length; i++)
            {
                optionWordArray2[i].gameObject.SetActive(true);
            }
        }
        
    }
    
    public void ShowCorrectLetter()
    {
        if (DataManager.TOTAL_JOKER < 1)
        {
            if(DataManager.BUILD_TYPE == "Facebook")
            {
                Application.ExternalCall("ShowAd_Reward", "GetJoker_AD");
            }
            else
            {
                game.GotJokerAfterReward();
            }
            
            return;
        }
        char revealChar;
        if (gameStatus == GameStatus.Next || currentAnswerIndex >= answerWord.Length) return;
        //if(answerWord.Length <= 8)
        //    answerWordArray[currentAnswerIndex].SetChar(char.ToUpper(answerWord[currentAnswerIndex]));
        //else
        //    answerWordArray2[currentAnswerIndex].SetChar(char.ToUpper(answerWord[currentAnswerIndex]));
        //selectedCharIndex.Add(wordData.transform.GetSiblingIndex());
        
        revealChar = char.ToUpper(answerWord[currentAnswerIndex]);
        
        if(answerWord.Length <= 8)
        {
            for (int i = 0; i < optionWordArray.Length; i++)
            {
                Debug.Log(optionWordArray[i].GetChar() + "_____" + revealChar);
                if (optionWordArray[i].gameObject.activeSelf && optionWordArray[i].GetChar() == revealChar)
                {
                    selectedCharIndex.Add(optionWordArray[i].transform.GetSiblingIndex());
                    // optionWordArray[i].gameObject.SetActive(false);
                    optionWordArray[i].emptyBox.SetActive(true);
                    optionWordArray[i].emptyBoxCG.alpha = 0;
                    optionWordArray[i].emptyBoxCG.DOFade(1, 0.3f);
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < optionWordArray2.Length; i++)
            {
                Debug.Log(optionWordArray2[i].GetChar() + "_____" + revealChar);
                if (optionWordArray2[i].gameObject.activeSelf && optionWordArray2[i].GetChar() == revealChar)
                {
                    //optionWordArray2[i].gameObject.SetActive(false);
                    optionWordArray2[i].emptyBox.SetActive(true);
                    optionWordArray2[i].emptyBoxCG.alpha = 0;
                    optionWordArray2[i].emptyBoxCG.DOFade(1, 0.3f);
                    break;
                }
            }
        }
        currentAnswerIndex++;
        if (answerWord.Length <= 8)
        {
            for (int i = 0; i < answerWordArray.Length; i++)
            {
                if (answerWordArray[i].deleteBtn.activeSelf)
                    answerWordArray[i].deleteBtn.SetActive(false);
                if (!answerWordArray[i].gTab.activeSelf)
                {
                    answerWordArray[i].GetComponent<Image>().enabled = false;
                    answerWordArray[i].gTab.SetActive(true);
                    answerWordArray[i].gTabCG.alpha = 0;
                    answerWordArray[i].gTabCG.DOFade(1, 0.3f);
                    answerWordArray[i].deleteBtn.SetActive(true);
                    answerWordArray[i].SetChar(revealChar);
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < answerWordArray2.Length; i++)
            {
                if (answerWordArray2[i].deleteBtn.activeSelf)
                    answerWordArray2[i].deleteBtn.SetActive(false);
                if (!answerWordArray2[i].gTab.activeSelf)
                {
                    answerWordArray2[i].GetComponent<Image>().enabled = false;
                    answerWordArray2[i].gTab.SetActive(true);
                    answerWordArray2[i].gTabCG.alpha = 0;
                    answerWordArray2[i].gTabCG.DOFade(1, 0.3f);
                    answerWordArray2[i].deleteBtn.SetActive(true);
                    answerWordArray2[i].SetChar(revealChar);
                    break;
                }
            }
        }
        DataManager.TOTAL_JOKER--;
        game.jokerBtnTxt.text = DataManager.TOTAL_JOKER.ToString()+" Joker";
        if(DataManager.TOTAL_JOKER <= 0)
        {
            game.jokerBtn.SetActive(false);
            game.newJokerBtn.SetActive(true);
            game.levelImg.sprite = Resources.Load("blue-circle", typeof(Sprite)) as Sprite;

        }
        else
        {
            game.jokerBtn.SetActive(true);
            game.newJokerBtn.SetActive(false);
            game.levelImg.sprite = Resources.Load("orange-circle", typeof(Sprite)) as Sprite;
        }
        PlayerData.SavePlayerData();
        FindObjectOfType<SoundManager>().Play("Set");

        CheckAnswer();
    }
    public void ShowCorrectWord()
    {
        currentAnswerIndex = 0;
        if(answerWord.Length <= 8)
        {
            for (int i = 0; i < answerWordArray.Length; i++)
            {
                answerWordArray[i].SetChar('_');
            }
            for (int i = 0; i < answerWordArray.Length; i++)
            {
                ShowCorrectLetter();
            }
        }
        else
        {
            for (int i = 0; i < answerWordArray2.Length; i++)
            {
                answerWordArray2[i].SetChar('_');
            }
            for (int i = 0; i < answerWordArray2.Length; i++)
            {
                ShowCorrectLetter();
            }
        }
        
    }
    public void RemoveUnnecessaryLetters()
    {
        if(answerWord.Length <= 8)
        {
            for (int i = 0; i < optionWordArray.Length; i++)
            {
                if (answerWord.ToUpper().Contains(optionWordArray[i].GetChar()))
                {
                    Debug.Log("Match!");
                }
                else
                {
                    Debug.Log("___UnMatch!");
                    optionWordArray[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < optionWordArray2.Length; i++)
            {
                if (answerWord.ToUpper().Contains(optionWordArray2[i].GetChar()))
                {
                    Debug.Log("Match!");
                }
                else
                {
                    Debug.Log("___UnMatch!");
                    optionWordArray2[i].gameObject.SetActive(false);
                }
            }
        }
        
        
    }
    public void RemoveAllLetters()
    {
        if (removeAllBtn.activeSelf)
            removeAllBtn.SetActive(false);
        if (removeAllBtn2.activeSelf)
            removeAllBtn2.SetActive(false);


        if (answerWord.Length <= 8)
        {
            for (int i = 0; i < optionWordArray.Length; i++)
            {
                optionWordArray[i].emptyBoxCG.alpha = 1;
                optionWordArray[i].emptyBoxCG.DOFade(0, 0.3f).OnComplete(AllLettersRemoved);
            }
            for (int i = 0; i < answerWordArray.Length; i++)
            {
                answerWordArray[i].gTab.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                answerWordArray[i].gTab.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, 200, 204), Vector2.zero);
                answerWordArray[i].GetComponent<Image>().enabled = true;
                //answerWordArray[i].gTab.SetActive(false);
                answerWordArray[i].gTabCG.alpha = 1;
                answerWordArray[i].gTabCG.DOFade(0, 0.3f);
                //answerWordArray[i].deleteBtn.SetActive(false);
                answerWordArray[i].deleteBtn.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < optionWordArray2.Length; i++)
            {
                optionWordArray2[i].emptyBoxCG.alpha = 1;
                optionWordArray2[i].emptyBoxCG.DOFade(0, 0.3f).OnComplete(AllLettersRemoved);
            }
            for (int i = 0; i < answerWordArray2.Length; i++)
            {
                answerWordArray2[i].gTab.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                answerWordArray2[i].gTab.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, 200, 204), Vector2.zero);
                answerWordArray2[i].GetComponent<Image>().enabled = true;
                //answerWordArray2[i].gTab.SetActive(false);
                answerWordArray2[i].gTabCG.alpha = 1;
                answerWordArray2[i].gTabCG.DOFade(0, 0.3f);
                //answerWordArray2[i].deleteBtn.SetActive(false);
                answerWordArray2[i].deleteBtn.SetActive(false);
            }
        }
        
        currentAnswerIndex = 0;
        FindObjectOfType<SoundManager>().Play("Delete");
    }
    void AllLettersRemoved()
    {
        
        if(answerWord.Length <= 8)
        {
            for (int i = 0; i < optionWordArray.Length; i++)
            {
                optionWordArray[i].emptyBoxCG.alpha = 0;
                optionWordArray[i].emptyBox.SetActive(false);
            }
            for (int i = 0; i < answerWordArray.Length; i++)
            {
                answerWordArray[i].gTab.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                answerWordArray[i].gTab.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, 200, 204), Vector2.zero);
                answerWordArray[i].GetComponent<Image>().enabled = true;
                answerWordArray[i].gTab.SetActive(false);
                answerWordArray[i].deleteBtn.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < optionWordArray2.Length; i++)
            {
                optionWordArray2[i].emptyBoxCG.alpha = 0;
                optionWordArray2[i].emptyBox.SetActive(false);
            }
            for (int i = 0; i < answerWordArray2.Length; i++)
            {
                answerWordArray2[i].gTab.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                answerWordArray2[i].gTab.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, 200, 204), Vector2.zero);
                answerWordArray2[i].GetComponent<Image>().enabled = true;
                answerWordArray2[i].gTab.SetActive(false);
                answerWordArray2[i].deleteBtn.SetActive(false);
            }
        }
        
    }
    public void SendBackLetter(int index)
    {
        if(answerWord.Length <= 8)
        {
            char pressedLtr = answerWordArray[index].charValue;
            Debug.Log(pressedLtr + "  ++++++");
            answerWordArray[index].SetChar('_');
            currentAnswerIndex--;
            for (int i = 0; i < optionWordArray.Length; i++)
            {
                //Debug.Log(optionWordArray[i].GetChar() + "_____" + revealChar);
                if (!optionWordArray[i].gameObject.activeSelf && optionWordArray[i].GetChar() == pressedLtr)
                {
                    optionWordArray[i].gameObject.SetActive(true);
                    break;
                }

            }
        }
        else
        {
            char pressedLtr = answerWordArray2[index].charValue;
            Debug.Log(pressedLtr + "  ++++++");
            answerWordArray2[index].SetChar('_');
            currentAnswerIndex--;
            for (int i = 0; i < optionWordArray2.Length; i++)
            {
                //Debug.Log(optionWordArray2[i].GetChar() + "_____" + revealChar);
                if (!optionWordArray2[i].gameObject.activeSelf && optionWordArray2[i].GetChar() == pressedLtr)
                {
                    optionWordArray2[i].gameObject.SetActive(true);
                    break;
                }

            }
        }
        
    }    
    public void LoadNextLevel()
    {
        Debug.Log("__________________________> " + currentQuestionIndex + "____________________________" + questionCount);
        if (currentQuestionIndex >= questionCount)
        {
           // game.ShowLevelComplete();
           // descriptionObj.SetActive(false);
        }
        else
        {
            SetQuestion();
           // descriptionObj.SetActive(false);
        }
            
    }

    public void SetPosOfTut()
    {
        int j = 0;
        for (int i = 0; i < optionWordArray.Length; i++)
        {
            if(optionWordArray[i].GetChar() == 'C' && tutorialCntr == 1)
                j = i;
            else if (optionWordArray[i].GetChar() == 'H' && tutorialCntr == 2)
                j = i;
            else if (optionWordArray[i].GetChar() == 'E' && tutorialCntr == 3)
                j = i;
            else if (optionWordArray[i].GetChar() == 'S' && tutorialCntr == 4)
                j = i;
            else if (optionWordArray[i].GetChar() == 'T' && tutorialCntr == 5)
                j = i;
        }
        Debug.Log("_________JJJJJJJJJ: " + j);
        SetPos(j);
       
    }
    void SetPos(int index)
    {
        switch (index)
        {
            case 0:
                GameObject.FindGameObjectWithTag("OptMask").GetComponent<RectTransform>().anchoredPosition = new Vector2(-196f, 359.7f);
                break;
            case 1:
                GameObject.FindGameObjectWithTag("OptMask").GetComponent<RectTransform>().anchoredPosition = new Vector2(-98.2f, 359.7f);
                break;
            case 2:
                GameObject.FindGameObjectWithTag("OptMask").GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 359.7f);
                break;
            case 3:
                GameObject.FindGameObjectWithTag("OptMask").GetComponent<RectTransform>().anchoredPosition = new Vector2(98.2f, 359.7f);
                break;
            case 4:
                GameObject.FindGameObjectWithTag("OptMask").GetComponent<RectTransform>().anchoredPosition = new Vector2(196f, 359.7f);
                break;
            case 5:
                GameObject.FindGameObjectWithTag("OptMask").GetComponent<RectTransform>().anchoredPosition = new Vector2(-98.2f, 261f);
                break;
            case 6:
                GameObject.FindGameObjectWithTag("OptMask").GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 261f);
                break;
            case 7:
                GameObject.FindGameObjectWithTag("OptMask").GetComponent<RectTransform>().anchoredPosition = new Vector2(98.2f, 261f);
                break;
        }
    }
}

public enum GameStatus
{
    Playing,
    Next
}

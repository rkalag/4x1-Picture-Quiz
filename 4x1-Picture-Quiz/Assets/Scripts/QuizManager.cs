using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QuizManager : MonoBehaviour
{
    public static QuizManager instance;

    [SerializeField]
    private WordData[] answerWordArray;

    

    [SerializeField]
    private WordData[] optionWordArray;
    [SerializeField]
    public Image questionImg;

    [SerializeField] GameObject removeAllBtn = null;

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
        ResetQuestion();
        for (int i = 0; i < answerWord.Length; i++)
        {
            charArray[i] = char.ToUpper(answerWord[i]);
        }
        for (int i = answerWord.Length; i < optionWordArray.Length; i++)
        {
            charArray[i] = (char)UnityEngine.Random.Range(65, 91);

        }
        charArray = ShuffleList.ShuffleListItems<char>(charArray.ToList()).ToArray();
        
        for (int i = 0; i < optionWordArray.Length; i++)
        {
            optionWordArray[i].SetChar(charArray[i]);
        }
        currentQuestionIndex++;
        gameStatus = GameStatus.Playing;
    }
    
    public void SelectedOption(WordData wordData)
    {
        if (wordData.gTab != null)
            if (wordData.gTab.activeSelf) return;
        if (wordData.emptyBox != null)
            if (wordData.emptyBox.activeSelf) return;
        Debug.Log(currentAnswerIndex + "____" + answerWord.Length);
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
        
        Debug.Log(currentAnswerIndex + "____" + answerWordArray.Length);
        if (gameStatus == GameStatus.Next) return;
        selectedCharIndex.Add(wordData.transform.GetSiblingIndex());
        //answerWordArray[currentAnswerIndex].SetChar(wordData.charValue);
        //answerWordArray[currentAnswerIndex].GetComponent<Image>().sprite = sp;
        // selectedChar.Add(wordData.gameObject);
        
        wordData.emptyBox.SetActive(true);
        wordData.emptyBoxCG.alpha = 0;
        wordData.emptyBoxCG.DOFade(1, 0.3f);
        // wordData.gameObject.em

        //wordData.gameObject.SetActive(false);
        currentAnswerIndex++;

        for (int i = 0; i < answerWordArray.Length; i++)
        {
            if(answerWordArray[i].deleteBtn.activeSelf)
                answerWordArray[i].deleteBtn.SetActive(false);
            if (!answerWordArray[i].gTab.activeSelf)
            {
                answerWordArray[i].GetComponent<Image>().enabled = false;
                answerWordArray[i].gTab.SetActive(true);
                answerWordArray[i].gTabCG.alpha = 0;
                answerWordArray[i].gTabCG.DOFade(1, 0.3f);
                answerWordArray[i].deleteBtn.SetActive(true);
                answerWordArray[i].SetChar(wordData.charValue);
                break;
            }
        }
        FindObjectOfType<SoundManager>().Play("Set");
        CheckAnswer();
    }
    public void ResetLastCharacter(Button btn)
    {
        Debug.Log("btn: " + btn);
        Debug.Log("btn: " + btn.transform.GetChild(0).gameObject.transform.GetChild(1));
        if(!btn.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.activeSelf)
        {
            Debug.Log("qqqqqqqq");
            return;
        }
        for (int i = 0; i < answerWordArray.Length; i++)
        {
            answerWordArray[i].gTab.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            answerWordArray[i].gTab.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, 200, 204), Vector2.zero);
            

        }
        if (removeAllBtn.activeSelf)
        {
            removeAllBtn.SetActive(false);
        }

        //Debug.Log("selectedCharIndex.Count: " + selectedCharIndex.Count);
        //Debug.Log("aaaaa: " + currentAnswerIndex +"___"+ answerWordArray[currentAnswerIndex - 1]);
        answerWordArray[currentAnswerIndex - 1].deleteBtn.SetActive(false);
        if (selectedCharIndex.Count > 0)
        {
            index = selectedCharIndex[selectedCharIndex.Count - 1];
            Debug.Log("index: " + index + "____currentAnswerIndex: "+ currentAnswerIndex);
            //optionWordArray[index].emptyBox.SetActive(false);
            optionWordArray[index].emptyBoxCG.alpha = 1;
            optionWordArray[index].emptyBoxCG.DOFade(0, 0.3f);
            selectedCharIndex.RemoveAt(selectedCharIndex.Count - 1);

            answerWordArray[currentAnswerIndex - 1].GetComponent<Image>().enabled = true;
           // answerWordArray[currentAnswerIndex - 1].gTab.SetActive(false);
            answerWordArray[currentAnswerIndex - 1].gTabCG.alpha = 1;
            answerWordArray[currentAnswerIndex - 1].gTabCG.DOFade(0, 0.3f).OnComplete(gTabAlphaGone);
             answerWordArray[currentAnswerIndex - 1].deleteBtn.SetActive(false);
            if (currentAnswerIndex - 2 >= 0)
                answerWordArray[currentAnswerIndex - 2].deleteBtn.SetActive(true);


            // answerWordArray[currentAnswerIndex].SetChar('_');
        }
        FindObjectOfType<SoundManager>().Play("Delete");
        Debug.Log("currentAnswerIndex:: " + currentAnswerIndex);

    }
    void gTabAlphaGone()
    {
        //int index = selectedCharIndex[selectedCharIndex.Count - 1];
        Debug.Log("index: " + index + "____currentAnswerIndex: " + currentAnswerIndex);
        optionWordArray[index].emptyBoxCG.alpha = 0;
        optionWordArray[index].emptyBox.SetActive(false);

        answerWordArray[currentAnswerIndex - 1].GetComponent<Image>().enabled = true;
        answerWordArray[currentAnswerIndex - 1].gTab.SetActive(false);
        answerWordArray[currentAnswerIndex - 1].gTabCG.alpha = 0;
        answerWordArray[currentAnswerIndex - 1].deleteBtn.SetActive(false);
        
        currentAnswerIndex--;
    }
    void CheckAnswer()
    {
        if (currentAnswerIndex >= answerWord.Length)
        {
            correctAnswer = true;
            for (int i = 0; i < answerWord.Length; i++)
            {
                if (char.ToUpper(answerWord[i]) != char.ToUpper(answerWordArray[i].charValue))
                {
                    correctAnswer = false;
                    break;
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
                for (int i = 0; i < answerWordArray.Length; i++)
                {
                    answerWordArray[i].gTab.GetComponent<Image>().color = new Color(0.5764706f, 0, 0, 1f);
                    answerWordArray[i].gTab.GetComponent<Image>().sprite = null;
                }
                removeAllBtn.SetActive(true);
                FindObjectOfType<SoundManager>().Play("Fail");
            }
        }
    }
    public void ResetQuestion()
    {
        Debug.Log("answerWordArray.Length____" + answerWordArray.Length);
        Debug.Log("optionWordArray.Length____" + optionWordArray.Length);
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
    
    public void ShowCorrectLetter()
    {
        char revealChar;
        if (gameStatus == GameStatus.Next || currentAnswerIndex >= answerWord.Length) return;
        answerWordArray[currentAnswerIndex].SetChar(char.ToUpper(answerWord[currentAnswerIndex]));
        revealChar = char.ToUpper(answerWord[currentAnswerIndex]);
        currentAnswerIndex++;
        for (int i = 0; i < optionWordArray.Length; i++)
        {
            Debug.Log(optionWordArray[i].GetChar()+"_____"+ revealChar);
            if(optionWordArray[i].gameObject.activeSelf && optionWordArray[i].GetChar() == revealChar)
            {
                optionWordArray[i].gameObject.SetActive(false);
                break;
            }
                
        }
        CheckAnswer();
    }
    public void ShowCorrectWord()
    {
        currentAnswerIndex = 0;
        for (int i = 0; i < answerWordArray.Length; i++)
        {
            answerWordArray[i].SetChar('_');
        }
        for (int i = 0; i < answerWordArray.Length; i++)
        {
            ShowCorrectLetter();
        }
    }
    public void RemoveUnnecessaryLetters()
    {

        //Debug.Log(answerWordArray.Length);
        for (int i = 0; i < optionWordArray.Length; i++)
        { 
            if(answerWord.ToUpper().Contains(optionWordArray[i].GetChar()))
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
    public void RemoveAllLetters()
    {
        if (removeAllBtn.activeSelf)
        {
            removeAllBtn.SetActive(false);
        }

        for (int i = 0; i < optionWordArray.Length; i++)
        {
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
        currentAnswerIndex = 0;
        FindObjectOfType<SoundManager>().Play("Delete");
    }
    public void SendBackLetter(int index)
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
}

public enum GameStatus
{
    Playing,
    Next
}

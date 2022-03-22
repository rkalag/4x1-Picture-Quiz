using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public static QuizManager instance;

    [SerializeField]
    private WordData[] answerWordArray;

    

    [SerializeField]
    private WordData[] optionWordArray;
    [SerializeField]
    public Image questionImg;


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
        //questionCount = jsonReader.jsonData.Count - 1;
       // SetQuestion();
    }
    public void SetQuestion()
    {
        timeTaken = 0;
        currentAnswerIndex = 0;
        selectedCharIndex.Clear();
        questionImg.sprite = Sprite.Create(jsonReader.questionList[0], new Rect(0, 0, 475, 475), Vector2.zero);


        answerWord = jsonReader.jsonData[DataManager.CURRENT_SUB_LEVEL]["answer"];
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
                answerWordArray[i].deleteBtn.SetActive(true);
                answerWordArray[i].SetChar(wordData.charValue);
                break;
            }
        }

        CheckAnswer();
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
                //DataManager.CURRENT_LEVEL++;
                if (currentQuestionIndex <= questionCount)
                {
                    // questionImg.sprite = Sprite.Create(jsonReader.solutionList[DataManager.CURRENT_SUB_LEVEL - 1], new Rect(0, 0, 662, 545), Vector2.zero);
                    // questionImg.sprite = questionData.questions[currentQuestionIndex-1].solutionImage;
                    Invoke("ShowDescription", 1.5f);
                    DataManager.CURRENT_SUB_LEVEL++;
                }
            }
            else
            {
                Debug.Log("_____WRONG!");
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
            answerWordArray[i].SetChar('_');
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
    public void ResetLastCharacter()
    {
        if(selectedCharIndex.Count > 0)
        {
            int index = selectedCharIndex[selectedCharIndex.Count - 1];
            optionWordArray[index].gameObject.SetActive(true);
            selectedCharIndex.RemoveAt(selectedCharIndex.Count - 1);

            currentAnswerIndex--;
            answerWordArray[currentAnswerIndex].SetChar('_');
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

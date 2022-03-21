using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Selection : MonoBehaviour
{
    JSONReader jsonReader;
    private void Start()
    {
        //jsonReader = GameObject.FindGameObjectWithTag("JSONReader").GetComponent<JSONReader>();
    }
    public void LoadLevel(int levelNum)
    {
        //DataManager.CURRENT_LEVEL = levelNum;
        //SceneManager.LoadScene(3);
        Initiate.Fade("Game", DataManager.SCENE_TRANSITION_COLOR, DataManager.SCENE_TRANSITION_DURATION);
    }
    public void LoadCategory()
    {
        //jsonReader.questionList.Clear();
        //jsonReader.solutionList.Clear();
        Initiate.Fade("Category", DataManager.SCENE_TRANSITION_COLOR, DataManager.SCENE_TRANSITION_DURATION);
    }
}

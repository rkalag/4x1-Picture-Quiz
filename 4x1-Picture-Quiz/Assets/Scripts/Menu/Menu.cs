using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
       
    }
    void Start()
    {
        
    }
    public void LoadGame()
    {
        Initiate.Fade("Game", DataManager.SCENE_TRANSITION_COLOR, DataManager.SCENE_TRANSITION_DURATION);
    }
}

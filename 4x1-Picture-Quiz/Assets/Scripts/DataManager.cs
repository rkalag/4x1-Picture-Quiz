using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static int CURRENT_TOPIC = 1; 
    //public static int CURRENT_LEVEL = 1;
    public static int CURRENT_LEVEL = 1;
    public static int CURRENT_SUB_LEVEL = 1;
    public static int TOTAL_SUB_LEVEL = 10;

    public static bool IS_MUSIC = true;
    public static bool IS_SOUND = true;
    public static bool SHOW_DAILY_REWARD = true;
    public static int TOTAL_HINT = 0;
    public static float POPUP_DURATION = 0.5f;
    public static float SCENE_TRANSITION_DURATION = 1.2f;
    public static Color SCENE_TRANSITION_COLOR = Color.black;

    public static string ANSWER = "";
    public static string CLUE_1 = "";
    public static string CLUE_2 = "";
    public static string DESCRIPTION = "";
    public static string SLOGAN = "";
    public static string OWNER = "";
}

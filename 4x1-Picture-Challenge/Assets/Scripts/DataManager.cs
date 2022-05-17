using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static string BUILD_TYPE = "Facebook";//Facebook, Unity
    public static bool IS_TESTING = false;

    public static int CURRENT_LEVEL = 2;
    public static int TOTAL_LEVELS = 500;
    public static int TOTAL_JOKER = 5;

    public static string ANSWER = "";

    public static bool IS_SOUND = true;
    public static bool REMOVE_ADS = false;

    public static bool DATA_LOADED = true;
    public static string PLAYER_NAME = "Player 1";
    public static bool CAN_SHOW_INTERSTITIAL = false;
    public static bool IS_TUTORIAL = false;
    public static string OS_TYPE = "Android";
    public static string DEVICE_TYPE;
    public static bool FIRST_TIME_AD = true;
}

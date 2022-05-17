using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScreenshotHandler : MonoBehaviour
{
    private static ScreenshotHandler instance;
    private Camera myCamera;
    private bool takeScreenshotOnNextFrame;

    private void Awake()
    {
        instance = this;
        myCamera = gameObject.GetComponent<Camera>();
    }
    private void OnPostRender()
    {
        if(takeScreenshotOnNextFrame)
        {
            takeScreenshotOnNextFrame = false;
            RenderTexture renderTexture = myCamera.targetTexture;

            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height-100, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height-100);
            renderResult.ReadPixels(rect, 0, 0);

            byte[] byteArray = renderResult.EncodeToPNG();
            //System.IO.File.WriteAllBytes(Application.dataPath + "/Screenshot.png", byteArray);
            Debug.Log("________ Saved Screenshot");

            Texture2DToBase64(byteArray);
            //Debug.Log("base64: " + Texture2DToBase64(byteArray));
            Application.ExternalCall("AskFriends", Texture2DToBase64(byteArray));
            RenderTexture.ReleaseTemporary(renderTexture);
            myCamera.targetTexture = null;

            Game.instance.backBtn.SetActive(true);
            Game.instance.askYourFriend.SetActive(true);
            if (DataManager.TOTAL_JOKER > 0)
                Game.instance.jokerBtn.SetActive(true);
            else
                Game.instance.newJokerBtn.SetActive(true);
            Game.instance.topLvl.SetActive(true);
            if (QuizManager.instance.answerWord.Length <= 8)
                QuizManager.instance.answerWordArrayObj.SetActive(true);
            else
                QuizManager.instance.answerWordArray2Obj.SetActive(true);
            Game.instance.guessTheWordObj.SetActive(false);
        }
    }

    private void TakeScreenshot(int width, int height)
    {

        Game.instance.backBtn.SetActive(false);
        Game.instance.askYourFriend.SetActive(false);
        Game.instance.jokerBtn.SetActive(false);
        Game.instance.newJokerBtn.SetActive(false);
        Game.instance.topLvl.SetActive(false);
        QuizManager.instance.answerWordArrayObj.SetActive(false);
        QuizManager.instance.answerWordArray2Obj.SetActive(false);
        Game.instance.guessTheWordObj.SetActive(true);
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        takeScreenshotOnNextFrame = true;
    }
    public static void TakeScreenshot_Static(int width, int height)
    {
        instance.TakeScreenshot(width, height);
    }

    public string Texture2DToBase64(byte[] bytesArr)
    {
        string strbaser64 = Convert.ToBase64String(bytesArr);
        return strbaser64;
    }
}

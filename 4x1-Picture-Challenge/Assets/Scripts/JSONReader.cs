using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JSONReader : MonoBehaviour
{
    private string jsonURL = "https://rkalag.github.io/fb-instant/4x1-picture-quiz/Data.json";
    private string imgURL = "https://rkalag.github.io/fb-instant/4x1-picture-quiz/";
    public JSONNode jsonData;
    [HideInInspector]
    public Texture2D iTexture;
    public List<Texture2D> questionList;

    private Game game;

    private void Awake()
    {
        int n = FindObjectsOfType<JSONReader>().Length;
        if (n != 1)
        {
            Destroy(this.gameObject);
        }
        // if more then once is in the scene
        //destroy ourselves
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        //game = GameObject.FindGameObjectWithTag("Game").GetComponent<Game>();
        StartCoroutine(RequestWebService());
    }
    public IEnumerator RequestWebService()
    {
        Debug.Log("jsonURL: " + jsonURL);
        using (UnityWebRequest webData = UnityWebRequest.Get(jsonURL))
        {

            yield return webData.SendWebRequest();
            if (webData.isNetworkError)
            {
                Debug.Log("Oops something went wrong." + webData.error);
            }
            else
            {
                if (webData.isDone)
                {
                    jsonData = JSON.Parse(System.Text.Encoding.UTF8.GetString(webData.downloadHandler.data));
                    if (jsonData == null)
                    {
                        Debug.Log("_________________No Data______________");
                    }
                    else
                    {
                        Debug.Log("JSON is loaded!_________________"+ jsonData.Count);
                       // Debug.Log("jsonData_______________________________> " + jsonData[1]["options"][0].Value);
                       // StartCoroutine(DownloadImage());
                    }
                }
            }
        }
    }
    public IEnumerator DownloadImage()
    {
        for (int i = DataManager.CURRENT_LEVEL; i <= DataManager.CURRENT_LEVEL; i++)
        {
            var tempURL = imgURL + DataManager.CURRENT_LEVEL + ".png";
             Debug.Log("tempURL: " + tempURL);
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(tempURL);
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("Image Loaded______ " + i+"____ "+ (jsonData.Count-1));
                iTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                questionList.Add(iTexture);
                GameObject.FindGameObjectWithTag("QuizManager").GetComponent<QuizManager>().SetQuestion();
                StartCoroutine(GameObject.FindGameObjectWithTag("Game").GetComponent<Game>().AnimateLevelInfo());
            }
        }
    }
    IEnumerator RemoveLoading()
    {
        yield return new WaitForSeconds(1f);
        Initiate.Fade("Selection", Color.black, 1.2f);
    }
}

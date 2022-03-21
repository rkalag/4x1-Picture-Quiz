using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    private Slider slider;
    public float fillSpeed = 0.5f;
    private float targetProgress = 0;
    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
    }
    private void Start()
    {
        IncrementProgress(0.75f);
    }
    private void Update()
    {
        if(slider.value < targetProgress)
        {
            slider.value += fillSpeed * Time.deltaTime;
        }
    }
    public void IncrementProgress(float newProgress)
    {
        targetProgress = slider.value + newProgress;
    }
}

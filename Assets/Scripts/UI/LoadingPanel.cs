using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{
    public float timeToLoad;
    public float timeWaitBeforeShow;
    private float currentTimeWaitBeforeShow;

    private Color targetColor;
    private Color defaultColor;
    private Image panel;
    public Color blackColor;

    private bool isLoading;

    // Start is called before the first frame update
    void Start()
    {
        panel = GetComponent<Image>();
        targetColor = panel.color;
        defaultColor = panel.color;
    }

    // Update is called once per frame
    void Update()
    {
        Color newColor = panel.color;
        newColor.a = Mathf.Lerp(newColor.a, targetColor.a, Time.deltaTime * timeToLoad);

        panel.color = newColor;

        if(panel.color == blackColor)
        {
            currentTimeWaitBeforeShow += Time.deltaTime;

            if(currentTimeWaitBeforeShow > timeWaitBeforeShow)
            {
                currentTimeWaitBeforeShow = 0;
                targetColor = defaultColor;
                isLoading = false;
            }
        }
    }

    public void Run()
    {
        targetColor = blackColor;
        isLoading = true;
    }

    public bool IsLoading()
    {
        return isLoading;
    }
}

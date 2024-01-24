using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleReport : MonoBehaviour
{
    public TMP_Text battleReport;
    public float timeChangeTextReport;
    public float timeUpdateChangeTextReport;
    private float currentTimeCHangeTextReport;
    private float currentTimeUpdateChangeTextReport;

    private string textToReport;
    private int indexTextReport;
    private List<string> messagesToReport = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(textToReport != battleReport.text)
        {
            currentTimeCHangeTextReport += Time.deltaTime;
            if(currentTimeCHangeTextReport > timeChangeTextReport)
            {
                currentTimeCHangeTextReport = 0;
                string[] subs = textToReport.Split(" ");
                battleReport.text = "";

                for(int i = 0; i < subs.Length; i++)
                {
                    if(i < indexTextReport)
                    {
                        battleReport.text += (i > 0 ? " " : "") + subs[i];
                    }
                    
                }

                indexTextReport++;

            }
        } else
        {
            if(messagesToReport.Count > 0)
            {
                currentTimeUpdateChangeTextReport += Time.deltaTime;

                if(currentTimeUpdateChangeTextReport > timeUpdateChangeTextReport)
                {
                    currentTimeUpdateChangeTextReport = 0;
                    textToReport = messagesToReport[0];
                    messagesToReport.RemoveAt(0);
                    indexTextReport = 0;
                }
              
            }
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            battleReport.text = textToReport;
        }
    }

    public void Report(string text)
    {
        textToReport = text;
        indexTextReport = 0;
    } 

    public void Report(List<string> texts)
    {
        messagesToReport = texts;

    }

    public bool IsReportFinished()
    {
        return textToReport == battleReport.text && messagesToReport.Count == 0;
    }
}

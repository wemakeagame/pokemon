using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ReportMessage
{
    public string message;
    public bool dismiss;

    public ReportMessage(string message, bool dismiss)
    {
        this.message = message;
        this.dismiss = dismiss;
    }
}

public class ReportMessageUI : MonoBehaviour
{
    public TMP_Text reportText;
    public float timeChangeTextReport;
    public float timeUpdateChangeTextReport;
    private float currentTimeCHangeTextReport;
    private float currentTimeUpdateChangeTextReport;
    public Image continueButton;

    protected string textToReport = "";
    private int indexTextReport;
    protected List<ReportMessage> messagesToReport = new List<ReportMessage>();
    private bool dismissed = true;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(textToReport != reportText.text)
        {
            currentTimeCHangeTextReport += Time.deltaTime;
            if(currentTimeCHangeTextReport > timeChangeTextReport)
            {
                currentTimeCHangeTextReport = 0;
                char[] subs = textToReport.ToCharArray();
                reportText.text = "";

                for(int i = 0; i < subs.Length; i++)
                {
                    if(i < indexTextReport)
                    {
                        reportText.text += subs[i];
                    }
                    
                }

                indexTextReport++;

            }
        } else
        {
            if(messagesToReport.Count > 0)
            {
                currentTimeUpdateChangeTextReport += Time.deltaTime;

                if(currentTimeUpdateChangeTextReport > timeUpdateChangeTextReport && dismissed)
                {
                    currentTimeUpdateChangeTextReport = 0;
                    textToReport = messagesToReport[0].message;
                    dismissed = messagesToReport[0].dismiss;
                    messagesToReport.RemoveAt(0);
                    indexTextReport = 0;
                }
              
            }
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            if(reportText.text != textToReport)
            {
                reportText.text = textToReport;
            } else
            {
                dismissed = true;
            }
            
        }

        continueButton.gameObject.SetActive(!dismissed && reportText.text == textToReport);
    }

    public void Report(string text, bool autoDismiss = true)
    {

        messagesToReport.Add(new ReportMessage(text, autoDismiss));
        indexTextReport = 0;
        
    } 


    public void ClearReports()
    {
        indexTextReport = 0;
        messagesToReport.Clear();
        textToReport = "";
        reportText.text = "";
        dismissed = true;
    }

    public void Report(List<string> texts, bool autoDismiss = true)
    {
        foreach(string text in texts)
        {
            messagesToReport.Add(new ReportMessage(text, autoDismiss));
        }
    }

    public void Report(List<ReportMessage> texts)
    {
        foreach (ReportMessage text in texts)
        {
            messagesToReport.Add(text);
        }
    }


    public bool IsReportFinished()
    {
        if(messagesToReport.Count > 0)
        {
            return false;
        } else
        {
            return textToReport == reportText.text && dismissed;
        }
    }
}

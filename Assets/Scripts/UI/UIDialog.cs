using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDialog : ReportMessageUI
{
    public GameObject panel;

    protected override void Start()
    {
        base.Start();
        
    }

    protected override void Update()
    {
        base.Update();
        panel.SetActive(!IsReportFinished());
    }
}

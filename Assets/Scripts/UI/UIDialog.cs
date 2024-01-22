using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDialog : MonoBehaviour
{

    public TMP_Text dialogText;
    public Image dialogPanel;

    private GameController gameController;

    public string currentText;
    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentText.Length > 0 && gameController != null && gameController.GetCurrentState() == GameState.DIALOG)
        {
            dialogPanel.gameObject.SetActive(true);
            dialogText.text = currentText;
        } else
        {
            dialogPanel.gameObject.SetActive(false);
        }
    }

}

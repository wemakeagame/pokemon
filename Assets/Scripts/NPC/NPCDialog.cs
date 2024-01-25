using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

[RequireComponent(typeof(NPCController))]
[RequireComponent(typeof(Interactible))]
[RequireComponent(typeof(Human))]
public class NPCDialog : MonoBehaviour
{

    public List<string> dialogsBeforeEvent = new List<string>();
    public List<string> dialogsAfterEvent = new List<string>();
    public EVENTS_KEYS eventName;
    public TriggerEvent afterDialogEvent;
    private int dialogIndex;
    private bool dialogInProgress;
    private UIDialog dialog;

    private NPCController controller;
    private GameController gameController;
    private PlayerStatsController playerStatsController;
    private Human human;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<NPCController>();
        dialog = FindObjectOfType<UIDialog>();
        gameController = FindObjectOfType<GameController>();
        playerStatsController = FindObjectOfType<PlayerStatsController>();
        human = GetComponent<Human>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteract()
    {
        if(!human.isMoving)
        {
            Talk();
        }
    }

    public void OnStopInteract()
    {
        Talk(true);
    }


    void Talk(bool cancel = false)
    {
        if(cancel)
        {
            CancelDialog();
        } else
        {
            if (!dialogInProgress)
            {
                StartDialog();
            } else
            {
                ContinueDialog();
            }

        }
        
    }

    private void StartDialog()
    {
        gameController.ChangeState(GameState.DIALOG);
        dialogInProgress = true;
        dialogIndex = 0;
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            Human playerHuman = playerController.GetComponent<Human>();
            if(playerHuman != null)
            {
                if(playerHuman.direction == Direction.DOWN)
                {
                    human.ChangeDirection(Direction.UP);
                } else if (playerHuman.direction == Direction.UP)
                {
                    human.ChangeDirection(Direction.DOWN);
                }
                else if (playerHuman.direction == Direction.LEFT)
                {
                    human.ChangeDirection(Direction.RIGHT);
                }
                else if (playerHuman.direction == Direction.RIGHT)
                {
                    human.ChangeDirection(Direction.LEFT);
                }
            }
        }
        UpdateCurrentDialog();
    }

    private void ContinueDialog()
    {
        dialogIndex++;

        List<string> dialogs = !playerStatsController.IsEventDone(eventName) ? dialogsBeforeEvent : dialogsAfterEvent;

        if(dialogIndex == dialogs.Count)
        {
            CancelDialog();

            if(!playerStatsController.IsEventDone(eventName))
            {
                afterDialogEvent.Trigger();
            }
        }
        UpdateCurrentDialog();
    }

    private void CancelDialog()
    {
        dialogIndex = 0;
        dialogInProgress = false;
        UpdateCurrentDialog();
        gameController.ChangeState(GameState.EXPLORATION);
    }

    private void UpdateCurrentDialog()
    {
        List<string> dialogs = !playerStatsController.IsEventDone(eventName) ? dialogsBeforeEvent : dialogsAfterEvent;
        dialog.currentText = dialogs[dialogIndex];
        if(!dialogInProgress)
        {
            dialog.currentText = "";
        }
    }
}

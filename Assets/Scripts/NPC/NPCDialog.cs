using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

[RequireComponent(typeof(NPCController))]
[RequireComponent(typeof(Interactible))]
[RequireComponent(typeof(Human))]
public class NPCDialog : MonoBehaviour
{

    public List<ReportMessage> dialogsBeforeEvent = new List<ReportMessage>();
    public List<ReportMessage> dialogsAfterEvent = new List<ReportMessage>();
    public EVENTS_KEYS eventName;
    public TriggerEvent afterDialogEvent;
    private ReportMessageUI dialog;
    private bool dialogStarted = false;

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
        if (dialog.IsReportFinished() && !playerStatsController.IsEventDone(eventName) && dialogStarted)
        {
            dialogStarted = false;
            if(afterDialogEvent != null && !playerStatsController.IsEventDone(eventName))
            {
                afterDialogEvent.Trigger();

            }
            CancelDialog();
        } else if(dialog.IsReportFinished() && dialogStarted)
        {
            dialogStarted = false;
            gameController.ChangeState(GameState.EXPLORATION);
        }
    }

    public void OnInteract()
    {
        if (!human.isMoving)
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
        if (cancel)
        {
            CancelDialog();
            gameController.ChangeState(GameState.EXPLORATION);
        }
        else
        {
            if (dialog.IsReportFinished() && !dialogStarted)
            {
                StartDialog();
            }

        }

    }

    private void StartDialog()
    {
        gameController.ChangeState(GameState.DIALOG);
        dialogStarted = true;
        List<ReportMessage> dialogs = !playerStatsController.IsEventDone(eventName) ? dialogsBeforeEvent : dialogsAfterEvent;
        dialog.Report(dialogs);
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            Human playerHuman = playerController.GetComponent<Human>();
            if (playerHuman != null)
            {
                if (playerHuman.direction == Direction.DOWN)
                {
                    human.ChangeDirection(Direction.UP);
                }
                else if (playerHuman.direction == Direction.UP)
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
    }

    private void CancelDialog()
    {
        dialog.ClearReports();
        dialogStarted = false;
    }

}

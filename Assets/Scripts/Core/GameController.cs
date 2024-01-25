using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    PAUSED,
    EXPLORATION,
    DIALOG,
    LOADING,
    BATTLE,
    MENU,
}

public class GameController : MonoBehaviour
{

    private GameState state = GameState.EXPLORATION;
    private GameState nextState = GameState.EXPLORATION;

    private BattleController battleController;
    private LoadingPanel loadingPanel;

    // Start is called before the first frame update
    void Start()
    {
        battleController = FindObjectOfType<BattleController>();
        loadingPanel = battleController.loadingPanel;
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine();
    }

    public void ChangeState(GameState newState)
    {
        nextState = newState;
        switch(nextState)
        {
            case GameState.PAUSED:
                break;
            case GameState.EXPLORATION:
                break;
            case GameState.DIALOG:
                break;
            case GameState.BATTLE:
                battleController.ChangeState(BattaleState.START_LOAD);
                break;
        }
    }

    void StateMachine()
    {
        state = nextState;
        switch (state)
        {
            case GameState.PAUSED:
                break;
            case GameState.EXPLORATION:
                break;
            case GameState.LOADING:
                if(!loadingPanel.IsLoading())
                {
                    ChangeState(GameState.EXPLORATION);
                }
                break;
            case GameState.DIALOG:
                break;
            case GameState.BATTLE:
                break;
        }
    }

    public GameState GetCurrentState()
    {
        return state;
    }
}

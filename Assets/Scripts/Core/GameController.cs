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

    private GameState state = GameState.LOADING;
    private GameState nextState = GameState.LOADING;
    private float minLoadingTime = 2;

    private BattleController battleController;
    private LoadingPanel loadingPanel;
    private float currentLoadingTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        battleController = FindObjectOfType<BattleController>();
        loadingPanel = battleController.loadingPanel;
        loadingPanel.Run(minLoadingTime);
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
                currentLoadingTime += Time.deltaTime;
                if(!loadingPanel.IsLoading() && currentLoadingTime > minLoadingTime)
                {
                    currentLoadingTime = 0;
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

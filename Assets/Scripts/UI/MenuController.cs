using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuState
{
    NO_MENU,
    POKEMON_FIRST_CHOOSE
}


public class MenuController : MonoBehaviour
{

    //Menus
    public FirstPokemonMenu firstPokemonMenu;

    private MenuState state = MenuState.NO_MENU;
    private MenuState nextState = MenuState.NO_MENU;

    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(MenuState.NO_MENU);
        gameController = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine();
    }

    public void ChangeState(MenuState newState)
    {
        nextState = newState;

        if(nextState != MenuState.NO_MENU)
        {
            gameController.ChangeState(GameState.MENU);
        }

        switch (nextState)
        {
            case MenuState.NO_MENU:
                firstPokemonMenu.gameObject.SetActive(false);
                if(gameController != null)
                {
                    gameController.ChangeState(GameState.EXPLORATION);
                }
                break;
            case MenuState.POKEMON_FIRST_CHOOSE:
                firstPokemonMenu.gameObject.SetActive(true);
                break;
        }
    }

    void StateMachine()
    {
        state = nextState;
        switch (state)
        {
            case MenuState.NO_MENU:
                break;
            case MenuState.POKEMON_FIRST_CHOOSE:
                break;
        }
    }

    public MenuState GetCurrentState()
    {
        return state;
    }
}

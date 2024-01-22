using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class BattleMenu : GenericMenu
{

    private BattleController battleController;


    override protected void Start()
    {
        base.Start();
        battleController = FindObjectOfType<BattleController>();
    }

    protected override void OnMenuPressed(MenuButton button) {
        if(button.name == "Run")
        {
            battleController.ChangeState(BattaleState.RUN);
        }

        if(button.name == "Fight")
        {
            battleController.ChangeState(BattaleState.CHOOSE_ATTACK);
        }
    }

    private void OnEnable()
    {
        ResetSelection();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using TMPro;

public class AttackBattleMenu : GenericMenu
{
    private BattleController battleController;

    private List<PokemonSkillBase> currentSkills;

    override protected void Start()
    {
        base.Start();
        battleController = FindObjectOfType<BattleController>();
    }


    protected override void OnMenuPressed(MenuButton button)
    {
        if (button.name == "Attack1")
        {
            battleController.SetPlayerAttack(currentSkills[0]);
            battleController.ChangeState(BattaleState.SELECT_FASTEST_ATTACK);
        } else if(button.name == "Attack2")
        {
            battleController.SetPlayerAttack(currentSkills[1]);
            battleController.ChangeState(BattaleState.SELECT_FASTEST_ATTACK);
        }
        else if (button.name == "Attack3")
        {
            battleController.SetPlayerAttack(currentSkills[2]);
            battleController.ChangeState(BattaleState.SELECT_FASTEST_ATTACK);
        }
        else if (button.name == "Attack4")
        {
            battleController.SetPlayerAttack(currentSkills[3]);
            battleController.ChangeState(BattaleState.SELECT_FASTEST_ATTACK);
        }
    }

    protected override void OnMenuCanceled()
    {
        battleController.ChangeState(BattaleState.CHOOSE_ACTION);
    }

    public void SetSkills(List<PokemonSkillBase> skills)
    {
        for (int i=0; i<skills.Count; i++)
        {
            if (skills[i] != null)
            {
                buttons[i].button.GetComponentInChildren<TMP_Text>().text = skills[i].skillName;
                buttons[i].isAvailable = true;
            } else
            {
                buttons[i].isAvailable = false;
            }
        }

        currentSkills = skills;
    }
}

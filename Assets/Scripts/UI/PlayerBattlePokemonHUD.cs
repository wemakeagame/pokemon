using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBattlePokemonHUD : BattlePokemonHUD
{

    public Slider xp;
    public float speedLevelUp;


    private int targetXp;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if(xp.value > targetXp)
        {
            xp.value = targetXp;
        } else
        {
            xp.value += Time.deltaTime * speedLevelUp;

        }
    }

    public void SetupXP(PokemonBase pokemon)
    {
        xp.maxValue = pokemon.GetTargetXP();
        targetXp = pokemon.GetCurrentXP();
    }


    public bool IsChangeDone ()
    {
        return targetXp == xp.value;
    }


}

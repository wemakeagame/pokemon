using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosePokemonEvent : TriggerEvent
{

    private MenuController menuController;

    // Start is called before the first frame update
    void Start()
    {
        menuController = FindObjectOfType<MenuController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override public void Trigger()
    {
        menuController.ChangeState(MenuState.POKEMON_FIRST_CHOOSE);
    }
}

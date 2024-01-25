using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAllPokemons : TriggerEvent
{

    private Trainer playerTrainer;
    private LoadingPanel loadingPanel;
    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        playerTrainer = FindObjectOfType<PlayerController>().GetComponent<Trainer>();
        loadingPanel = FindObjectOfType<LoadingPanel>(); 
        gameController = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override public void Trigger()
    {
        gameController.ChangeState(GameState.LOADING);

        foreach(PokemonBase pokemon in playerTrainer.GetPokemons())
        {
            pokemon.Ressurect();
            loadingPanel.Run();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAllPokemons : TriggerEvent
{

    private Trainer playerTrainer;
    private LoadingPanel loadingPanel;

    // Start is called before the first frame update
    void Start()
    {
        playerTrainer = FindObjectOfType<PlayerController>().GetComponent<Trainer>();
        loadingPanel = FindObjectOfType<LoadingPanel>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override public void Trigger()
    {
        foreach(PokemonBase pokemon in playerTrainer.GetPokemons())
        {
            pokemon.Ressurect();
            loadingPanel.Run();
        }
    }
}

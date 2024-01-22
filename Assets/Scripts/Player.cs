using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private GameController gameController;
    private BattleController battleController;
    private Interactible curentInteractable;

    private Human human;
    private Trainer trainer;
    private bool canCheckPokemon = true;


    private PokemonArea area;
    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        battleController = FindObjectOfType<BattleController>();
        human = GetComponent<Human>();
        trainer = GetComponent<Trainer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(area != null && !human.isMoving && canCheckPokemon)
        {
            CheckPokemonFound(area);
            canCheckPokemon = false;
        }

        canCheckPokemon = human.isMoving;
    }


    void CheckPokemonFound(PokemonArea pokemonArea)
    {
        if (pokemonArea != null && trainer.GetPokemons().Count > 0)
        {
            PokemonData pokemonFound = pokemonArea.FindPokemon();

            if (pokemonFound != null)
            {
                battleController.SetOpenentPokemon(pokemonFound);
                gameController.ChangeState(GameState.BATTLE);
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Interactible interactible = collision.collider.GetComponent<Interactible>();

        if (interactible != null)
        {
            curentInteractable = interactible;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
       area = collider.GetComponent<PokemonArea>();
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        area = null;
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        Interactible interactible = collision.collider.GetComponent<Interactible>();

        if (interactible != null)
        {
            curentInteractable = null;
        }
    }

    public Interactible GetCurrentInteractible()
    {
        return curentInteractable;
    }
}

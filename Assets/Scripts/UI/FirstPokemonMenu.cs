using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FirstPokemonMenu : MonoBehaviour
{

    public Image firstPokemon;
    public Image secondPokemon;
    public Image thirdPokemon;

    public PokemonData firstPokemonRef;
    public PokemonData secondPokemonRef;
    public PokemonData thirdPokemonRef;

    public Color selectedColor;
    public string selectText;
    public string confirmText;
    public TMP_Text info;

    private Image selectedPokemon;
    private Color defaultColor;
    private float currentTimeSelect;
    private MenuController menuController;

    private PlayerStatsController playerStatsController;
    private Trainer trainerPlayer;

    // Start is called before the first frame update
    void Start()
    {
        selectedPokemon = firstPokemon;
        defaultColor = firstPokemon.color;

        info.text = selectText;
        menuController = FindObjectOfType<MenuController>();
        playerStatsController = FindObjectOfType<PlayerStatsController>();
        trainerPlayer = FindObjectOfType<PlayerController>().GetComponent<Trainer>();
    }

    // Update is called once per frame
    void Update()
    {
        selectedPokemon.color = selectedColor;

        currentTimeSelect += Time.deltaTime;

        if(currentTimeSelect > 0.1f)
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                SelectNext();
                currentTimeSelect = 0;
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                SelectPrevious();
                currentTimeSelect = 0;
            }
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            if(info.text == selectText)
            {
                info.text = confirmText;

            } else
            {
                // fazer a selecao do primeiro pokemom
                if(selectedPokemon == firstPokemon)
                {
                    trainerPlayer.AddPokemon(firstPokemonRef);
                } else if (selectedPokemon == secondPokemon)
                {
                    trainerPlayer.AddPokemon(secondPokemonRef);
                }
                else if (selectedPokemon == thirdPokemon)
                {
                    trainerPlayer.AddPokemon(thirdPokemonRef);
                }
                menuController.ChangeState(MenuState.NO_MENU);
                playerStatsController.CompleteEvent(EVENTS_KEYS.CHOOSE_FIRST_POKEMON);
                Debug.Log("Voce escolheu seu pokemon");
            }
           
        }

        if(Input.GetKeyDown(KeyCode.X))
        {
            if(info.text == confirmText)
            {
                info.text = selectText;
            } else
            {
                menuController.ChangeState(MenuState.NO_MENU);
            }

        }
       
    }

    public void SelectNext()
    {
        Unselect();

        if(selectedPokemon == firstPokemon)
        {
            selectedPokemon = secondPokemon;
        }
        else if(selectedPokemon == secondPokemon)
        {
            selectedPokemon = thirdPokemon;
        }
    }

    public void SelectPrevious()
    {
        Unselect();

        if (selectedPokemon == secondPokemon)
        {
            selectedPokemon = firstPokemon;
        }
        else if (selectedPokemon == thirdPokemon)
        {
            selectedPokemon = secondPokemon;
        }
    }

    void Unselect()
    {
        firstPokemon.color = defaultColor;
        secondPokemon.color = defaultColor;
        thirdPokemon.color = defaultColor;
    }
}
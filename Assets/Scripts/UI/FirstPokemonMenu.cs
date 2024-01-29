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
    private GameController gameController;

    private PlayerStatsController playerStatsController;
    private Trainer trainerPlayer;

    // Start is called before the first frame update
    void Start()
    {
        selectedPokemon = firstPokemon;
        defaultColor = firstPokemon.color;

        info.text = selectText;
        menuController = FindObjectOfType<MenuController>();
        gameController = FindObjectOfType<GameController>();
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

        if(Input.GetButtonDown("Fire1"))
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

                // This makes sure that your first pokemon is the strong you can get
                trainerPlayer.GetFirstPokemon().ChangeLuck(5);

                menuController.ChangeState(MenuState.NO_MENU);
                playerStatsController.CompleteEvent(EVENTS_KEYS.CHOOSE_FIRST_POKEMON);
            }
           
        }

        if(Input.GetButtonDown("Fire2"))
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

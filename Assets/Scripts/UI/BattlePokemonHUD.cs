using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattlePokemonHUD : MonoBehaviour
{
    public TMP_Text pokemonName;
    public TMP_Text level;
    public Slider life;
    public Image lifeBar;
    public Image image;
    public float blinkRate;


    private float targetLife;
    private Color targetColor;
    private float currentblinkRate;
    private bool shouldBlink = false;


    // Start is called before the first frame update
    void Start()
    {
        life.value = life.maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        if(shouldBlink)
        {
            currentblinkRate += Time.deltaTime;

            if(currentblinkRate > blinkRate)
            {
                currentblinkRate = 0;
                targetColor.a = targetColor.a == 1 ? 0.5f : 1;
            }
        }

        life.value = Mathf.Lerp(life.value, targetLife, 5f * Time.deltaTime);
        lifeBar.color = Color.Lerp(lifeBar.color, targetColor, 5f * Time.deltaTime);
    }

    public void SetupPokemon(PokemonBase pokemon )
    {
        pokemonName.text = pokemon.pokemonName;
        level.text = pokemon.level.ToString();
        life.value = pokemon.GetCurrentLife();
        targetLife = life.value;
        UpdateLife(pokemon);
    }

    public void UpdateLife(PokemonBase pokemon)
    {
        life.maxValue = pokemon.totalLife;
        targetLife = pokemon.GetCurrentLife();

        float percentLife = targetLife / life.maxValue * 100;

        if(percentLife > 60)
        {
            targetColor = Color.green;
            targetColor.g = 0.8f;
        } else if(percentLife > 30)
        {
            targetColor = Color.yellow;
        } else 
        {
            targetColor = Color.red;
        }

        shouldBlink = percentLife <= 20;
    }


    public void SetupImage(PokemonBase pokemon, bool isOpenent = true)
    {
        image.sprite = isOpenent ? pokemon.frontImage : pokemon.backImage;
    }
}

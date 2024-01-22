using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public enum POKEMON_TYPE
{
    NORMAL,
    GRASS,
    FIRE,
    WATER,
}

public abstract class PokemonBase : MonoBehaviour
{
    public int level = 1;
    public int initPower;
    public int speed;
    public Sprite frontImage;
    public Sprite backImage;
    public string pokemonName;
    public List<POKEMON_TYPE> pokemonType = new List<POKEMON_TYPE>();
    public List<PokemonSkillBase> skills = new List<PokemonSkillBase>() { null, null, null, null};
    public float totalLife;
    private float currentLife;

    private bool defeated;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentLife = totalLife;
        if(pokemonType.Count == 0)
        {
            pokemonType.Add(POKEMON_TYPE.NORMAL);
        }
    }


    public void ApplyDamage(float damage)
    {
        currentLife -= damage;

        if (currentLife < 0)
        {
            currentLife = 0;
            defeated = true;
        }
    }

    public bool IsDefeated()
    {
        return defeated;
    }

    public float GetCurrentLife()
    {
        return currentLife;
    }

    public void SetupPokemon(PokemonData pokemonData, List<PokemonSkillBase> skillsToSet)
    {
        totalLife = pokemonData.totalLife;
        level = pokemonData.level;
        frontImage = pokemonData.frontImage;
        backImage = pokemonData?.backImage;
        pokemonType = pokemonData.pokemonType;
        skills = skillsToSet;
        pokemonName = pokemonData.pokemonName;
        initPower = pokemonData.initPower;
        speed = pokemonData.speed;
    }

    public int GetSkillBasePower()
    {
        return level * initPower / 2;
    }
}

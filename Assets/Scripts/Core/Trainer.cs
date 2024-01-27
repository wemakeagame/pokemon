using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Trainer : MonoBehaviour
{
    private List<PokemonData> pokemons = new List<PokemonData>();
    private List<PokemonBase> pokemonsInstantiated = new List<PokemonBase>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPokemon(PokemonData pokemonData)
    {
        if (pokemons.Count < 6)
        {
            pokemons.Add(pokemonData);

            GameObject pokemonGO = Instantiate(pokemonData.prefab.gameObject);
            pokemonGO.name = pokemonData.pokemonName;
            PokemonBase pokemon = pokemonGO.GetComponent<PokemonBase>();
            pokemonGO.transform.parent = transform;
            if (pokemon != null)
            {
                pokemon.SetupPokemon(pokemonData);
                pokemonsInstantiated.Add(pokemon);

            }

        }
    }

    public List<PokemonBase> GetPokemons()
    {
        return pokemonsInstantiated;
    }

    public PokemonBase GetFirstPokemon ()
    {
        if(pokemonsInstantiated.Count > 0)
        {
            return pokemonsInstantiated[0];
        }

        return null;
    }

    public PokemonSkillBase GetFirstPokemonSkill()
    {
        PokemonSkillBase skill = null;

        PokemonBase pokemon = GetFirstPokemon();
       

        while(skill == null)
        {
            int randSkillIndex = Random.Range(0, 3);

            if (pokemon.skills[randSkillIndex] != null)
            {
                skill = pokemon.skills[randSkillIndex];
            }
        }

        return skill;

    }

    public bool HasPokemonToBattle()
    {
        bool returnValue = false;

        foreach(PokemonBase pokemon in pokemonsInstantiated)
        {
            if(!pokemon.IsDefeated() && !returnValue)
            {
                returnValue = true;
            }
        }

        return returnValue;
    }
}

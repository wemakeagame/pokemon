using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PokemonAreaChance
{
    public PokemonData pokemon;
    public float chance;
}

public class PokemonArea : MonoBehaviour
{

    public List<PokemonAreaChance> pokemons = new List<PokemonAreaChance>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public PokemonData FindPokemon()
    {
        PokemonAreaChance chance = pokemons.ElementAt(Random.Range(0, pokemons.Count));

        int randomChance = Random.Range(0, 100);

        if(randomChance < chance.chance)
        {
            return chance.pokemon;
        }

        return null;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "pokemons", menuName = "Pokemons/CreatePokemon", order = 1)]
public class PokemonData : ScriptableObject
{

    public int level = 1;
    public Sprite frontImage;
    public Sprite backImage;
    public string pokemonName;
    public List<POKEMON_TYPE> pokemonType = new List<POKEMON_TYPE>();
    public List<PokemonSkillBase> attacks = new List<PokemonSkillBase>() { null, null, null, null };
    public List<EvolutionTrack> evolutionTrack = new List<EvolutionTrack>();
    public List<SkillData> initialSkills = new List<SkillData>();
    public int totalLife;
    public int initPower;
    public int speed;

    public PokemonBase prefab;
}

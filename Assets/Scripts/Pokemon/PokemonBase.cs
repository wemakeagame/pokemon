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
    private int currentXP;
    private int luck;
    private int targetXp;

    private bool defeated;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentLife = totalLife;
        if (pokemonType.Count == 0)
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

        luck = Random.Range(1, 5);
        targetXp = GetTargetXP();
    }

    public int GetSkillBasePower()
    {
        return level * initPower / 2;
    }

    public void Heal(float points)
    {
        currentLife += points;

        if(currentLife > totalLife)
        {
            currentLife = totalLife;
        }
    }

    public void Ressurect()
    {
        defeated = false;
        Heal(totalLife);
    }

    public void AddXp(int xp)
    {
        targetXp = GetTargetXP();
        currentXP += xp;

        if(currentXP >= targetXp)
        {
            int diffXp = currentXP - targetXp;
            currentXP = 0;
            level++;
            totalLife += totalLife / (level * luck);
            speed += speed / (level * luck);
            initPower += initPower / (level * luck);
            currentLife = totalLife;
            AddXp(diffXp);
        }
    }

    public int GetXpBattle()
    {
        return (int)((level + luck) * Config.xpMultiplier);
    }

    public int GetTargetXP()
    {
        return level * (initPower + speed);
    }

    public int GetCurrentXP()
    {
        return currentXP;
    }

    public int NextLevelXP()
    {
        return targetXp - currentXP;
    }

     
}

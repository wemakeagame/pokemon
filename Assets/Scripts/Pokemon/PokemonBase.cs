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

[System.Serializable]
public class EvolutionTrack
{
    public PokemonData evolution;
    public int levelToEvolve;
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
    public int currentEvolution = 0;
    public List<EvolutionTrack> evolutionTrack = new List<EvolutionTrack>();
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

    public void SetupPokemon(PokemonData pokemonData)
    {
        totalLife = pokemonData.totalLife;
        level = pokemonData.level;
        frontImage = pokemonData.frontImage;
        backImage = pokemonData?.backImage;
        pokemonType = pokemonData.pokemonType;
        skills = InstantiateSkills(pokemonData);
        pokemonName = pokemonData.pokemonName;
        initPower = pokemonData.initPower;
        speed = pokemonData.speed;
        evolutionTrack = pokemonData.evolutionTrack;

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

    public List<PokemonSkillBase> InstantiateSkills(PokemonData pokemonData)
    {
        List<PokemonSkillBase> skills = new List<PokemonSkillBase>();

        // 4  is the limit of attacks
        for (int i = 0; i < 4; i++)
        {
            SkillData skill = null;
            if (i < pokemonData.initialSkills.Count)
            {
                skill = pokemonData.initialSkills[i];
            }

            if (skill != null && skills.Find(s => s.attackName == skill.attackName) == null)
            {
                GameObject newSkillGO = new GameObject(skill.name + " Instance");
                switch (skill.skillType)
                {
                    case SkillType.ATTACK:
                        newSkillGO.AddComponent<AttackSkill>();
                        break;
                }

                newSkillGO.transform.parent = transform;

                PokemonSkillBase newSkill = newSkillGO.GetComponent<PokemonSkillBase>();
                newSkill.SetSkillData(skill);
                skills.Add(newSkill);
            }
            else
            {
                skills.Add(null);
            }
        }

        return skills;
    }

    public void SetUpSkills(PokemonData pokemonData)
    {
        List<PokemonSkillBase> skills = InstantiateSkills(pokemonData);
        foreach (PokemonSkillBase skill in skills)
        {
            if (skill != null)
            {
                skill.transform.parent = transform;
            }
        }
    }


    public bool CanEvolve()
    {
        return evolutionTrack.Count > 0 && currentEvolution < evolutionTrack.Count && level >= evolutionTrack[currentEvolution].levelToEvolve;
    }


    public void Evolve()
    {
        EvolutionTrack evolution = evolutionTrack[currentEvolution];
        currentEvolution++;
        int previousLevel = level;
        SetupPokemon(evolution.evolution);
        level = previousLevel;
    }

    public Sprite GetNextEvolutionImage()
    {
        if(CanEvolve())
        {
            return evolutionTrack[currentEvolution].evolution.frontImage;
        }
        return null;
    }
     
}

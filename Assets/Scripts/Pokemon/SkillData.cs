using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    ATTACK,
    HEAL,
    BUFF,
    DEBUFF,
}

[CreateAssetMenu(fileName = "pokemon_attacks", menuName = "Pokemons/CreateAttack", order = 2)]
public class SkillData : ScriptableObject
{
    // Start is called before the first frame update
    public string attackName;
    public int power;
    public int chance;
    public float attackSpeed;
    public POKEMON_TYPE attackType;
    public SkillType skillType;
    public int amount;
    public string animationName;
    public Sprite projectilImage;
   
}

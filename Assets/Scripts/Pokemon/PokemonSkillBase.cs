using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PokemonSkillBase : MonoBehaviour
{
    public string skillName;
    public int power;
    public int chance;
    public float attackSpeed;
    public POKEMON_TYPE attackType;
    public SkillType skillType;
    public int amount;
    public SkillData skillData;
    public string animationName;
    public Sprite projectilImage;

    private int currentAmount;

    // Start is called before the first frame update
    void Start()
    {
        currentAmount = amount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSkillData(SkillData skillData)
    {
        skillName = skillData.skillName;
        power = skillData.power;
        chance = skillData.chance;
        attackSpeed = skillData.attackSpeed;
        attackType = skillData.attackType;
        skillType = skillData.skillType;
        amount = skillData.amount;
        animationName = skillData.animationName;
        projectilImage = skillData.projectilImage;

        this.skillData = skillData;
    }

    public bool GetSkillHitConfirmation()
    {
        return Random.Range(0, 100) < chance;
    }

}

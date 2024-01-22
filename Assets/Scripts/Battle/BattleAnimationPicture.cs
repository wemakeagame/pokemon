using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BattleAnimationPicture : MonoBehaviour
{

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySkill(PokemonSkillBase skill )
    {
        animator.SetTrigger("trigger_" + skill.animationName);
    }

    public void PlayDamage()
    {
        animator.SetTrigger("trigger_damage");
    }
}

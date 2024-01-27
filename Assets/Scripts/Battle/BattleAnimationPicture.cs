using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class BattleAnimationPicture : MonoBehaviour
{

    private Animator animator;
    public Image projectil;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        projectil.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySkill(PokemonSkillBase skill )
    {
        animator.SetTrigger("trigger_" + skill.animationName);
        if(skill.projectilImage != null)
        {
            projectil.enabled = true;
            projectil.sprite = skill.projectilImage;
        }
    }

    public void PlayDamage()
    {
        animator.SetTrigger("trigger_damage");
    }

    public void PlayEnterBattle()
    {
        animator.SetTrigger("trigger_enter");
    }

    public void PlayLeaveBattle()
    {
        animator.SetTrigger("trigger_leave");
    }

}

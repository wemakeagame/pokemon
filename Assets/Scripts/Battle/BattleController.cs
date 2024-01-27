using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattaleState
{
    NO_BATTALE,
    START_LOAD,
    LOADING,
    FINISH_LOAD,
    PRESENT_OPONENT,
    PRESENT_PLAYER_POKEMON,
    CHOOSE_ACTION,
    CHOOSE_ATTACK,
    SELECT_FASTEST_ATTACK,
    ATTACK_PLAYER,
    ATTACK_OPONENT,
    RESOLVE_ATTACK_PLAYER,
    RESOLVE_ATTACK_OPONENT,
    RUN,
    GET_XP_AFTER_BATTLE,
    ADD_XP,
    ANOUNCE_END_BATTLE,
    CHECK_EVOLUTION,
    SHOW_EVOLUTION,
    END,
}

public class BattleController : MonoBehaviour
{

    private BattaleState state = BattaleState.NO_BATTALE;
    private BattaleState nextState = BattaleState.NO_BATTALE;

    public LoadingPanel loadingPanel;
    public GameObject battleScene;
    public BattleMenu battleMenu;
    public AttackBattleMenu attackBattleMenu;
    public BattleAnimationPicture pictureOponent;
    public BattleAnimationPicture picturePlayer;
    public EvolutionReport evolutionReport;
    public BattleReport battleReport;
    public float transitionTime;
    private PokemonBase playerPokemon;
    private PokemonBase oponentPokemon;

    public BattlePokemonHUD hudOponent;
    public PlayerBattlePokemonHUD hudPlayer;

    private Trainer playerTrainer;
    private Trainer opponentTrainer;
    private float currentTransitionTime;
    private GameController gameController;
    private PokemonSkillBase playerSelectedSkill;
    private PokemonSkillBase oponentSelectedSkill;
    private float playerDamageResult = 0;
    private float oponentDamageResult = 0;
    private int roundCount = 0;
    private int remainingXpToAdd = 0;
    private List<PokemonBase> pokemonsToCheckEvolution = new List<PokemonBase>();
    private PokemonBase evolvingPokemon;

    private void Start()
    {
        battleScene.SetActive(false);
        evolutionReport.gameObject.SetActive(false);
        battleMenu.gameObject.SetActive(false);
        attackBattleMenu.gameObject.SetActive(false);
        playerTrainer = FindObjectOfType<PlayerController>().GetComponent<Trainer>();
        gameController = FindObjectOfType<GameController>();
        battleReport.Report("...");
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine();
        currentTransitionTime += Time.deltaTime;
    }

    public void ChangeState(BattaleState newState)
    {
        nextState = newState;

        switch (nextState)
        {
            case BattaleState.NO_BATTALE:
                battleScene.SetActive(false);
                attackBattleMenu.gameObject.SetActive(false);
                battleMenu.isBlocked = false;
                battleMenu.gameObject.SetActive(false);
                hudPlayer.ShowTrainer();
                evolutionReport.gameObject.SetActive(false);
                pokemonsToCheckEvolution = new List<PokemonBase>();
                break;
            case BattaleState.START_LOAD:
                loadingPanel.Run();
                ChangeState(BattaleState.LOADING);
                break;
            case BattaleState.LOADING:
                break;
            case BattaleState.FINISH_LOAD:
                battleScene.SetActive(true);
                picturePlayer.PlayEnterBattle();
                pictureOponent.PlayEnterBattle();
                ChangeState(BattaleState.PRESENT_OPONENT);
                break;
            case BattaleState.PRESENT_OPONENT:
                LoadPlayerPokemon();
                LoadOponentPokemon();
                pictureOponent.PlayEnterBattle();
                battleReport.Report("Your battle started against " + opponentTrainer.GetFirstPokemon().pokemonName);
                WaitTransition();
                break;
            case BattaleState.PRESENT_PLAYER_POKEMON:
                battleReport.Report("You release " + playerPokemon.pokemonName);
                WaitTransition();
                break;
            case BattaleState.CHOOSE_ACTION:
                battleMenu.gameObject.SetActive(true);
                attackBattleMenu.gameObject.SetActive(false);
                break;
            case BattaleState.CHOOSE_ATTACK:
                battleMenu.gameObject.SetActive(false);
                attackBattleMenu.gameObject.SetActive(true);
                attackBattleMenu.SetSkills(playerPokemon.skills);
                roundCount = 0;
                break;
            case BattaleState.SELECT_FASTEST_ATTACK:
                oponentSelectedSkill = opponentTrainer.GetFirstPokemonSkill();
                attackBattleMenu.gameObject.SetActive(false);
                float speedPlayer = playerSelectedSkill.attackSpeed + playerPokemon.speed;
                float speedOponent = oponentSelectedSkill.attackSpeed + oponentPokemon.speed;

                if (speedPlayer > speedOponent)
                {
                    ChangeState(BattaleState.ATTACK_PLAYER);
                } else if(speedPlayer < speedOponent)
                {
                    ChangeState(BattaleState.ATTACK_OPONENT);
                } else
                {
                    int rand = Random.Range(0, 100);

                    ChangeState(rand > 50 ? BattaleState.ATTACK_PLAYER : BattaleState.ATTACK_OPONENT);
                }
                break;
            case BattaleState.ATTACK_PLAYER:
                playerDamageResult = ResolveAttackPlayer();
                roundCount++;
                break;
            case BattaleState.ATTACK_OPONENT:
                oponentDamageResult = ResolveAttackOponent();
                roundCount++;
                break;
            case BattaleState.RESOLVE_ATTACK_PLAYER:
                WaitTransition();
                break;
            case BattaleState.RESOLVE_ATTACK_OPONENT:
                WaitTransition();
                break;
            case BattaleState.ADD_XP:
                AddXpPosBattle();
                break;
            case BattaleState.RUN:
                battleReport.Report("You Ran away...");
                battleMenu.isBlocked = true;
                WaitTransition();
                break;
            case BattaleState.CHECK_EVOLUTION:
                break;
            case BattaleState.SHOW_EVOLUTION:
                evolutionReport.gameObject.SetActive(true);
                evolutionReport.SetPokemonPicture(pokemonsToCheckEvolution[0].frontImage, pokemonsToCheckEvolution[0].GetNextEvolutionImage());
                break;
            case BattaleState.END:
                loadingPanel.Run();
                Destroy(oponentPokemon.gameObject);
                Destroy(opponentTrainer.gameObject);
                gameController.ChangeState(GameState.EXPLORATION);
                break;
        }
    }

    void StateMachine()
    {
        state = nextState;
        switch (state)
        {
            case BattaleState.NO_BATTALE:
                break;
            case BattaleState.LOADING:
                if (!loadingPanel.IsLoading())
                {
                    ChangeState(BattaleState.FINISH_LOAD);
                }
                break;
            case BattaleState.PRESENT_OPONENT:
                if (battleReport.IsReportFinished())
                {  

                    if (IsTransitionOver())
                    {
                        picturePlayer.PlayLeaveBattle();
                        ChangeState(BattaleState.PRESENT_PLAYER_POKEMON);
                    }

                }
                break;
            case BattaleState.PRESENT_PLAYER_POKEMON:
                if (battleReport.IsReportFinished())
                {
                    if (IsTransitionOver())
                    {
                        hudPlayer.SetupImage(playerPokemon, false);
                        picturePlayer.PlayEnterBattle();
                        ChangeState(BattaleState.CHOOSE_ACTION);
                    }
                }
                break;
            case BattaleState.CHOOSE_ACTION:
                break;
            case BattaleState.CHOOSE_ATTACK:
                break;
            case BattaleState.ATTACK_PLAYER:
                if (battleReport.IsReportFinished())
                {
                    if(playerDamageResult > 0)
                    {
                        picturePlayer.PlaySkill(playerSelectedSkill);
                       
                    }
                    ChangeState(BattaleState.RESOLVE_ATTACK_PLAYER);
                }
                break;
            case  BattaleState.RESOLVE_ATTACK_PLAYER:
                if (IsTransitionOver())
                {
                    if (playerDamageResult > 0)
                    {
                        pictureOponent.PlayDamage();
                        battleReport.Report(playerPokemon.pokemonName + " dealed " + playerDamageResult + " of damage");
                        oponentPokemon.ApplyDamage(playerDamageResult);
                        hudOponent.UpdateLife(oponentPokemon);
                    }
                    SelectNextRound(BattaleState.ATTACK_OPONENT);
                }
                break;
            case BattaleState.ATTACK_OPONENT:
                if (battleReport.IsReportFinished())
                {
                    if (oponentDamageResult > 0)
                    {
                        pictureOponent.PlaySkill(oponentSelectedSkill);
                    }
                    ChangeState(BattaleState.RESOLVE_ATTACK_OPONENT);
                }
                break;
            case BattaleState.RESOLVE_ATTACK_OPONENT:
                if (IsTransitionOver())
                {
                    if (oponentDamageResult > 0)
                    {
                        picturePlayer.PlayDamage();
                        battleReport.Report(oponentPokemon.pokemonName + " dealed " + oponentDamageResult + " of damage");
                        playerPokemon.ApplyDamage(oponentDamageResult);
                        hudPlayer.UpdateLife(playerPokemon);

                    }
                    SelectNextRound(BattaleState.ATTACK_PLAYER);
                }
                break;
            case BattaleState.RUN:
                if (IsTransitionOver())
                {
                    ChangeState(BattaleState.END);
                    
                }
                break;
            case BattaleState.GET_XP_AFTER_BATTLE:
                    if (battleReport.IsReportFinished())
                    {
                        ChangeState(BattaleState.ADD_XP);
                    }
                    break;
            case BattaleState.ANOUNCE_END_BATTLE:
                if (IsTransitionOver())
                {
                    WaitTransition();
                    if (battleReport.IsReportFinished())
                    {
                        WaitTransition();
                        ChangeState(BattaleState.END);
                    }
                }
                break;
            case BattaleState.ADD_XP:
                if(battleReport.IsReportFinished())
                {
                    if (hudPlayer.IsChangeDone() && remainingXpToAdd > 0)
                    {
                        battleReport.Report(playerPokemon.pokemonName + " leveled up to " + (playerPokemon.level + 1));
                        hudPlayer.SetupPokemon(playerPokemon);
                        evolutionReport.StartEvolutionAnimation();
                        if (!pokemonsToCheckEvolution.Contains(playerPokemon))
                        {
                            pokemonsToCheckEvolution.Add(playerPokemon);
                        }
                        AddXpPosBattle();
                    }
                    else
                    {
                        if(hudPlayer.IsChangeDone())
                        {
                            AnounceEndBattle(playerPokemon, oponentPokemon);
                        }
                    }
                }
                break;
            case BattaleState.CHECK_EVOLUTION:
                if(IsTransitionOver())
                {
                    if(battleReport.IsReportFinished())
                    {
                        if (pokemonsToCheckEvolution.Count > 0 && pokemonsToCheckEvolution[0].CanEvolve())
                        {
                            WaitTransition();
                            ChangeState(BattaleState.SHOW_EVOLUTION);
                        }
                        else
                        {
                            WaitTransition();
                            ChangeState(BattaleState.ANOUNCE_END_BATTLE);
                        }
                    }
                }
                break;
            case BattaleState.SHOW_EVOLUTION:
                if(evolutionReport.IsEvolutionOver() && IsTransitionOver())
                {
                    string previousName = pokemonsToCheckEvolution[0].pokemonName;
                    pokemonsToCheckEvolution[0].Evolve();

                    battleReport.Report(previousName + " evolved to " + pokemonsToCheckEvolution[0].pokemonName);
                    WaitTransition();
                    pokemonsToCheckEvolution.RemoveAt(0);

                    ChangeState(BattaleState.CHECK_EVOLUTION);
                }
                break;
            case BattaleState.END:
                if(IsTransitionOver()) {
                    ChangeState(BattaleState.NO_BATTALE);
                }
                break;
        }
    }

    private void SelectNextRound(BattaleState nextState)
    {
        if(oponentPokemon.IsDefeated())
        {
            GetXpAfterBattle();
        } else if(playerPokemon.IsDefeated())
        {
            AnounceEndBattle(oponentPokemon, playerPokemon);
        }
        else
        {
            if (roundCount == 2)
            {
                ChangeState(BattaleState.CHOOSE_ACTION);
            }
            else
            {
                ChangeState(nextState);
            }
        }
    }

    private void AnounceEndBattle(PokemonBase winner, PokemonBase loser)
    {
        List<string> messages = new List<string>();
        messages.Add(loser.pokemonName + " was defeated...");
        battleReport.Report(messages);
        WaitTransition();

        if(pokemonsToCheckEvolution.Count > 0)
        {
            ChangeState(BattaleState.CHECK_EVOLUTION);
            battleReport.Report("...");
        } else
        {
            ChangeState(BattaleState.ANOUNCE_END_BATTLE);
        }

        if (winner == playerPokemon)
        {
            pictureOponent.PlayLeaveBattle();
        } else
        {
            picturePlayer.PlayLeaveBattle();
        }
    }

    private void GetXpAfterBattle()
    {
        int xp = oponentPokemon.GetXpBattle();
        remainingXpToAdd = xp;
        battleReport.Report(playerPokemon.pokemonName + " received " + xp + " of experience...");
        ChangeState(BattaleState.GET_XP_AFTER_BATTLE);
    }


    private void AddXpPosBattle()
    {
        int nextXp = playerPokemon.NextLevelXP();

        if(oponentPokemon.GetXpBattle() > nextXp)
        {
            remainingXpToAdd -= nextXp;
            playerPokemon.AddXp(nextXp);
            hudPlayer.SetupXP(playerPokemon);
        }
        else
        {
            playerPokemon.AddXp(remainingXpToAdd);
            hudPlayer.SetupXP(playerPokemon);
            AnounceEndBattle(playerPokemon, oponentPokemon);
        }
    }

    private bool IsTransitionOver()
    {
        return currentTransitionTime > transitionTime;
    }

    public void WaitTransition()
    {
        currentTransitionTime = 0;
    }

    public BattaleState GetCurrentState()
    {
        return state;
    }


    public void LoadPlayerPokemon()
    {
        playerPokemon = playerTrainer.GetPokemons()[0];
        hudPlayer.SetupPokemon(playerPokemon);
        hudPlayer.SetupXP(playerPokemon);
    }

    public void LoadOponentPokemon()
    {
        hudOponent.SetupPokemon(opponentTrainer.GetFirstPokemon());
        hudOponent.SetupImage(opponentTrainer.GetFirstPokemon());
    }


    public void SetOpenentPokemon(PokemonData pokemonData)
    {
        opponentTrainer = new GameObject("OponentTrainer").AddComponent<Trainer>();
        opponentTrainer.AddPokemon(pokemonData);
        oponentPokemon = opponentTrainer.GetFirstPokemon();
    }

    public void SetPlayerAttack(PokemonSkillBase skill)
    {
        playerSelectedSkill = skill;
        ChangeState(BattaleState.RESOLVE_ATTACK_PLAYER);
    }

    public float ResolveAttackPlayer()
    {
        return ResolveAttack(playerSelectedSkill, playerPokemon, oponentPokemon);
    }

    public float ResolveAttackOponent()
    {
        return ResolveAttack(oponentSelectedSkill, oponentPokemon, playerPokemon);
    }

    private float ResolveAttack(PokemonSkillBase skill, PokemonBase pokemon, PokemonBase target)
    {
        if (skill != null)
        {
            bool didAttackHit = skill.GetSkillHitConfirmation();

            if (didAttackHit)
            {
                battleReport.Report(pokemon.pokemonName + " used: " + skill.attackName);
                switch (skill.skillType)
                {
                    case SkillType.ATTACK:
                        int pokemonBasePower = pokemon.GetSkillBasePower();
                        return (pokemonBasePower + skill.power) * CalculateAdvantageAttack(skill, target);

                }
            }
            else
            {
                battleReport.Report(pokemon.name + " tried : " + skill.attackName + " but missed...");
            }
        }

        return -1;
    }

    public float CalculateAdvantageAttack(PokemonSkillBase skill, PokemonBase target)
    {
        float advantage = 1;

        switch (skill.attackType)
        {
            case POKEMON_TYPE.NORMAL:
                return 1;
            case POKEMON_TYPE.FIRE:
                if(target.pokemonType.Contains(POKEMON_TYPE.FIRE) || target.pokemonType.Contains(POKEMON_TYPE.WATER))
                {
                    advantage -= 0.5f;
                }

                if(target.pokemonType.Contains(POKEMON_TYPE.GRASS))
                {
                    advantage += 2;
                }
                break;
            case POKEMON_TYPE.WATER:
                if (target.pokemonType.Contains(POKEMON_TYPE.WATER) || target.pokemonType.Contains(POKEMON_TYPE.GRASS))
                {
                    advantage -= 0.5f;
                }

                if (target.pokemonType.Contains(POKEMON_TYPE.FIRE))
                {
                    advantage += 2;
                }
                break;
            case POKEMON_TYPE.GRASS:
                if (target.pokemonType.Contains(POKEMON_TYPE.GRASS) || target.pokemonType.Contains(POKEMON_TYPE.FIRE))
                {
                    advantage -= 0.5f;
                }

                if (target.pokemonType.Contains(POKEMON_TYPE.WATER))
                {
                    advantage += 2;
                }
                break;
            default:
                return 1;

        }

        return advantage;


    }
}

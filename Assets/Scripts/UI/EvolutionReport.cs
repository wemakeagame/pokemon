using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionReport : MonoBehaviour
{
    public Image pokemonBeforeEvolve;
    public Image pokemonAfterEvolve;
    public Image overlayEvolution;

    public float timeToBlink;
    public float blinkFactor;
    public int blinkTimes = 0;

    private float currentTimeToBlink;
    private int currentBlinkTime;
    private bool isEvolving;
    private Color targetColorBefore;
    private Color targetColorAfter;
    private Color targetOverlay;

    // Start is called before the first frame update
    void Start()
    {
        targetColorBefore = pokemonBeforeEvolve.color;
        targetColorAfter = pokemonAfterEvolve.color;
        targetOverlay = overlayEvolution.color;
    }

    // Update is called once per frame
    void Update()
    {

        if(currentBlinkTime > blinkTimes)
        {
            isEvolving = false;
            targetColorBefore.a = 0;
            pokemonBeforeEvolve.color = targetColorBefore;
            targetColorAfter.a = 1;
            pokemonAfterEvolve.color = targetColorAfter;
            targetOverlay.a = 0;

        }


        if(isEvolving)
        {
            currentTimeToBlink += Time.deltaTime;

            if(currentTimeToBlink > timeToBlink)
            {
                currentTimeToBlink = 0;
                currentBlinkTime++;
                targetColorBefore.a = targetColorBefore.a == 1 ? 0 : 1;
                targetColorAfter.a = targetColorAfter.a == 1 ? 0 : 1;
            }

            pokemonBeforeEvolve.color = Color.Lerp(pokemonBeforeEvolve.color, targetColorBefore, Time.deltaTime * blinkFactor * 2);
            pokemonAfterEvolve.color = Color.Lerp(pokemonAfterEvolve.color, targetColorAfter, Time.deltaTime * blinkFactor);
        }
        else
        {
            overlayEvolution.color = Color.Lerp(overlayEvolution.color, targetOverlay, Time.deltaTime * blinkFactor);
        }
    }

    public void StartEvolutionAnimation()
    {
        currentBlinkTime = 0;
        currentTimeToBlink = 0;
        isEvolving = true;
        targetColorAfter.a = 0;
        targetColorBefore.a = 1;
        targetOverlay.a = 1;
    }

    public bool IsEvolutionOver()
    {
        return !isEvolving;
    }

    public void SetPokemonPicture(Sprite before, Sprite after)
    {
        pokemonAfterEvolve.sprite = after;
        pokemonBeforeEvolve.sprite = before;
    }
}

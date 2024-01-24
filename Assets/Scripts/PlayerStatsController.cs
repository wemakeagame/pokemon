using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EVENTS_KEYS
{
    NO_EVENT,
    CHOOSE_FIRST_POKEMON,
    HEAL_ALL_POKEMONS,
}

public class PlayerStatsController : MonoBehaviour
{

    public List<EVENTS_KEYS> eventsDone;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CompleteEvent(EVENTS_KEYS eventName)
    {
        eventsDone.Add(eventName);
    }

    public bool IsEventDone(EVENTS_KEYS eventName)
    {
        return eventsDone.Contains(eventName);
    }


}

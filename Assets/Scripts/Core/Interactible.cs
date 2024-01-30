using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    public void Interact()
    {
       OnInteract();
    }

    public void StopInteract()
    {
        StopInteract();
    }

    protected virtual void OnInteract()
    {

    }

    protected virtual void OnStopInteract()
    {

    }
}

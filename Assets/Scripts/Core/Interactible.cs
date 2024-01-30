using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    public void Interact()
    {
        gameObject.SendMessage("OnInteract", SendMessageOptions.DontRequireReceiver);
    }

    public void StopInteract()
    {
        gameObject.SendMessage("OnStopInteract", SendMessageOptions.DontRequireReceiver);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        gameObject.SendMessage("OnInteract", SendMessageOptions.DontRequireReceiver);
    }

    public void StopInteract()
    {
        gameObject.SendMessage("OnStopInteract", SendMessageOptions.DontRequireReceiver);
    }
}

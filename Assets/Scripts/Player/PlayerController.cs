using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Human))]
public class PlayerController : MonoBehaviour
{
    private Human human;
    private Player player;

    private GameController gameController;
    private float currentTimeToGetDirection;
    // Start is called before the first frame update
    void Start()
    {
        human = GetComponent<Human>();
        player = GetComponent<Player>();
        gameController = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameController != null && (gameController.GetCurrentState() != GameState.EXPLORATION && gameController.GetCurrentState() != GameState.DIALOG))
        {
            return;
        }

        int x = (int)Input.GetAxisRaw("Horizontal");
        int y = (int)Input.GetAxisRaw("Vertical");
        currentTimeToGetDirection += Time.deltaTime;
        if (gameController.GetCurrentState() == GameState.EXPLORATION && currentTimeToGetDirection > 0.1f)
        {
            if (x != 0 && human.canMove)
            {
                human.Walk(x > 0 ? Direction.RIGHT : Direction.LEFT);
                currentTimeToGetDirection = 0;
            }

            if (y != 0 && human.canMove)
            {
                human.Walk(y < 0 ? Direction.DOWN : Direction.UP);
                currentTimeToGetDirection = 0;
            }
        }

        

        if(Input.GetButtonDown("Fire1"))
        {
            if(!human.isMoving)
            {
                ActionEnter();
            }
        }

        if(Input.GetButtonDown("Fire2"))
        {
            ActionExit();
            
        }
    }

    void ActionEnter()
    {
        Interactible interactible = player.GetCurrentInteractible();
        if(interactible != null)
        {
            interactible.Interact();
        }
    }

    void ActionExit()
    {
        Interactible interactible = player.GetCurrentInteractible();
        if (interactible != null)
        {
            interactible.StopInteract();
        }

    }
}

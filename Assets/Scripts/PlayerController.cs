using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Human))]
public class PlayerController : MonoBehaviour
{
    private Human human;
    private Player player;

    private GameController gameController;
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

        if (x != 0 && human.canMove && gameController.GetCurrentState() == GameState.EXPLORATION)
        {
            human.Walk(x > 0 ? Direction.RIGHT : Direction.LEFT);
        }

        if(y != 0 && human.canMove && gameController.GetCurrentState() == GameState.EXPLORATION)
        {
            human.Walk(y > 0 ? Direction.DOWN : Direction.UP);
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            if(!human.isMoving)
            {
                ActionEnter();
            }
        }

        if(Input.GetKeyDown(KeyCode.X))
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

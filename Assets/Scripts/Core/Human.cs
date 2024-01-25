using System.Collections;
using System.Collections.Generic;
using UnityEditor.TextCore.Text;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class HumanSprites
{
    public Sprite[] walkingUp;
    public Sprite[] walkingDown;
    public Sprite[] walkingLeft;
    public Sprite[] walkingRight;

    public float transitionTime;
    public int index = 0;

    public void ChangeIndex()
    {
        index++;
        if(index >= walkingUp.Length)
        {
            index = 0;
        }
    }
}

[System.Serializable]
public class Movement
{
    public float speed;
    public float restingTime;
}

public enum Direction
{
    UP,
    DOWN,
    LEFT,
    RIGHT,
}

public class Human : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public HumanSprites sprites;
    public Direction direction = Direction.DOWN;
    public Movement movement;
    public Vector3 newPosition;
    public Vector3 currentPosition;
    public Vector3 previousPosition;
    public CollisionManager collisionManager;
    public GameController gameController;

    private Sprite[] selectedSprites;
    private float currentTransitionTime;
    private float currentRestingTime;
    public bool isMoving = false;
    public bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        currentPosition = transform.position;
        collisionManager = FindObjectOfType<CollisionManager>();
        gameController = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {

        if(gameController.GetCurrentState() != GameState.EXPLORATION)
        {
            return;
        }

        // movement
        if(transform.position != newPosition && isMoving)
        {
            

            //animation
            currentTransitionTime += Time.deltaTime;

            if(currentTransitionTime > sprites.transitionTime)
            {
                currentTransitionTime = 0;
                sprites.ChangeIndex();
                spriteRenderer.sprite = selectedSprites[sprites.index];
            }

            //transform.position = Vector3.Slerp(transform.position, newPosition, Time.deltaTime * movement.speed);
            switch (direction)
            {
                case Direction.DOWN:
                    transform.Translate(Vector3.up * Time.deltaTime * movement.speed);
                    break;
                case Direction.UP:
                    transform.Translate(Vector3.down * Time.deltaTime * movement.speed);
                    break;
                case Direction.LEFT:
                    transform.Translate(Vector3.left * Time.deltaTime * movement.speed);
                    break;
                case Direction.RIGHT:
                    transform.Translate(Vector3.right * Time.deltaTime * movement.speed);
                    break;
            }
            if (Mathf.Abs(Vector3.Distance(transform.position, newPosition)) < 0.05f)
            {
                isMoving = false;
                transform.position = currentPosition;
                collisionManager.ReleasePosition(previousPosition);
                canMove = true;
            }
        }

        
        if (!isMoving && selectedSprites != null && selectedSprites.Length > 0)
        {
            currentRestingTime += Time.deltaTime;
            if(currentRestingTime > movement.restingTime)
            {
                currentRestingTime = 0;
                spriteRenderer.sprite = selectedSprites[0];
                sprites.index = 0;
            }
        }

        if(gameController != null)
        {
            GameState state = gameController.GetCurrentState();
            canMove = state == GameState.EXPLORATION;
        }

    }

    public void Walk(Direction newDirection)
    {
        if(isMoving || !canMove)
        {
            return;
        }


        if (direction != newDirection)
        {
            ChangeDirection(newDirection);
            canMove=false;
            return;
        }
        canMove = true;


        isMoving = true;
        direction = newDirection;
        newPosition = transform.position;
        previousPosition = transform.position;
        SelectSpritesAnimation();


        if (direction == Direction.UP){
            newPosition.y -= Config.WorldUnity;
            currentPosition.y -= Config.WorldUnity;
        }
        else if (direction == Direction.DOWN) {
            newPosition.y += Config.WorldUnity;
            currentPosition.y += Config.WorldUnity;
        } else if (direction == Direction.LEFT) {
            newPosition.x -= Config.WorldUnity;
            currentPosition.x -= Config.WorldUnity;
        } else if (direction == Direction.RIGHT){
            newPosition.x += Config.WorldUnity;
            currentPosition.x += Config.WorldUnity;
        }

        if(collisionManager.IsPositionAvailable(newPosition))
        {
            collisionManager.RegisterPosition(newPosition);
        } else
        {
            newPosition = previousPosition;
            currentPosition = previousPosition;
            isMoving = false;
        }
    }

    private void SelectSpritesAnimation()
    {
        switch(direction)
        {
            case Direction.DOWN:
                selectedSprites = sprites.walkingUp;
                break;
            case Direction.UP:
                selectedSprites = sprites.walkingDown;
                break;
            case Direction.LEFT:
                selectedSprites = sprites.walkingLeft;
                break;
            case Direction.RIGHT:
                selectedSprites = sprites.walkingRight;
                break;
        }
    }

    public void ChangeDirection(Direction newDirection)
    {
        direction = newDirection;
        SelectSpritesAnimation();
        spriteRenderer.sprite = selectedSprites[0];
    }

   


}

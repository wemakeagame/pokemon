using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Human))]
public class NPCController : MonoBehaviour
{
    public List<Vector3> positions;
    public float timeToChangePosition;
    public GameController gameController;

    private float currentTimeChangePosition;
    private int currentPosition = 0;
    private Human human;
    private Vector3 targetPosition;

    public bool canWalk = true;

    // Start is called before the first frame update
    void Start()
    {
        human = GetComponent<Human>();
        gameController = FindObjectOfType<GameController>();
        for (int i=0; i< positions.Count; i++) 
        {
            Vector3 newPos = positions[i] + transform.position;
            positions[i] = newPos;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(gameController.GetCurrentState() != GameState.EXPLORATION)
        {
            return;
        }

        currentTimeChangePosition += Time.deltaTime;

        if(currentTimeChangePosition > timeToChangePosition)
        {
            currentTimeChangePosition = 0;
            currentPosition = Random.Range(0, positions.Count);
        }

        targetPosition = positions[currentPosition];


        if (transform.position.y < targetPosition.y)
        {
            human.Walk(Direction.DOWN);
        } else if (transform.position.y > targetPosition.y)
        {
            human.Walk(Direction.UP);
        } else if (transform.position.x > targetPosition.x)
        {
            human.Walk(Direction.LEFT);
        } else if (transform.position.x < targetPosition.x)
        {
            human.Walk(Direction.RIGHT);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        foreach (var position in positions)
        {
            Gizmos.DrawWireCube(position + transform.position, new Vector3( 0.1f, 0.1f, 0));
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{

    public List<Vector3> positions = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RegisterPosition(Vector3 newPosition) {
        if(!positions.Contains(newPosition))
        {
            positions.Add(newPosition);
        }
    }

    public void ReleasePosition(Vector3 position)
    {
        positions.Remove(position);
    }

    public bool IsPositionAvailable(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
        return !positions.Contains(position) && (hit.collider == null || hit.collider.isTrigger);
    }


}

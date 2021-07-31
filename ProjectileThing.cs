using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileThing : MonoBehaviour
{
    protected Rigidbody thisRigidbody = null;
    public Vector3 thisDirection = Vector3.zero; // Moving direction. Inherited from Player.
    [SerializeField] protected float thisSpeed = 0f;

    // Start is called before the first frame update
    protected void StartThing()
    {
        thisRigidbody = GetComponent<Rigidbody>();

        if (thisDirection != Vector3.right)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    // Update is called once per frame
    protected void UpdateThing()
    {
        thisRigidbody.velocity = thisDirection * thisSpeed;
    }
}

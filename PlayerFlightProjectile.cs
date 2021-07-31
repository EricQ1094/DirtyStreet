using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlightProjectile : MonoBehaviour
{
    private Rigidbody thisRigidbody = null;
    // Start is called before the first frame update
    void Start()
    {
        thisRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        thisRigidbody.velocity = Vector3.up * 10f;
    }

    protected void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}

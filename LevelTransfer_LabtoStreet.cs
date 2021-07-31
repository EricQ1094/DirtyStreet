using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransfer_LabtoStreet : MonoBehaviour
{
    [SerializeField] private GameObject thisDoor = null;
    [SerializeField] private GameObject thisLab = null;
    [SerializeField] private GameObject thisStreetWall = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void OnTriggerEnter(Collider other)
    {
        Player aPlayer = other.gameObject.GetComponent<Player>();

        if (aPlayer != null)
        {
            thisLab.SetActive(false);

            thisStreetWall.SetActive(true);
        }
    }
}

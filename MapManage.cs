using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManage : MonoBehaviour
{
    [SerializeField] private GameObject MapToHide = null;
    [SerializeField] private GameObject MapToShow = null;
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
            MainMethod();
        }
    }

    protected void MainMethod()
    {
        MapToHide.SetActive(false);
        MapToShow.SetActive(true);
    }
}

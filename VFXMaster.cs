using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXMaster : MonoBehaviour
{
    private static VFXMaster thisInstance = null;

    public static VFXMaster ThisVFXMaster
    {
        get
        {
            return thisInstance;
        }
    }

    [SerializeField] private Explode thisExplodePrefab = null;
    // Start is called before the first frame update
    void Start()
    {
        thisInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnExplode(Vector3 aPos)
    {
        Explode aExplode = Instantiate(thisExplodePrefab, aPos, Quaternion.identity);
    }
}

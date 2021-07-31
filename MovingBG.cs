using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBG : MonoBehaviour
{
    [SerializeField] private Player thisPlayer = null;
    private bool startMoving = false;
    private bool isMoving = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (thisPlayer.inFlightMode)
        {
            if (startMoving == false)
            {
                StartCoroutine(WaitToMove());
                startMoving = true;
            }
        }

        if (isMoving)
        {
            transform.Translate(Vector3.down * 5f * Time.deltaTime);
        }
    }

    protected IEnumerator WaitToMove()
    {
        yield return new WaitForSeconds(5f);

        isMoving = true;
    }
}

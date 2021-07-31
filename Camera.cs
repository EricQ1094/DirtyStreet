using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private static Camera thisInstance;

    public static Camera ThisPlayerCamera
    {
        get
        {
            return thisInstance;
        }
    }
    // For shake Camera
    private Vector3 thisOriginPos = Vector3.zero;
    private float thisShakeTimer = 0f;
    protected void Awake()
    {
        thisInstance = this;
    }

    protected void Start()
    {
        thisOriginPos = transform.localPosition;
    }

    protected void Update()
    {
        if (thisShakeTimer > 0f)
        {
            thisShakeTimer -= Time.deltaTime;
            ShakeCamera();
        }
    }
    public void CallShakeCamera()
    {
        thisShakeTimer = 0.1f;
    }

    protected void ShakeCamera()
    {
        for (int iShake = 0; iShake < 5; iShake++)
        {
            Vector3 aRandomShakePos = thisOriginPos;
            aRandomShakePos.x += Random.Range(-0.1f, 0.1f);
            aRandomShakePos.y += Random.Range(-0.1f, 0.1f);

            transform.localPosition = aRandomShakePos;
        }
    }
}

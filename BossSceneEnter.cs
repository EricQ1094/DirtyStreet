using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSceneEnter : MonoBehaviour
{
    // Transfer Player to the Boss Scene.
    private Camera thisPlayerCamera = null;
    [SerializeField] private GameObject thisBreakFloor = null;
    [SerializeField] private GameObject[] thisHideScene = null;
    private AudioSource thisAudioSource = null;
    [SerializeField] private AudioClip thisShakeSound = null;
    [SerializeField] private AudioClip thisBreakSound = null;
    // To Start the BossFight
    [SerializeField] private GameObject[] thisBossFightObjs = null;
    // Start is called before the first frame update
    void Start()
    {
        thisPlayerCamera = Camera.ThisPlayerCamera;
        thisAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected IEnumerator BossSceneEnterBehavior()
    {
        thisAudioSource.PlayOneShot(thisShakeSound);

        thisPlayerCamera.CallShakeCamera();

        yield return new WaitForSeconds(2f);

        thisPlayerCamera.CallShakeCamera();

        yield return new WaitForSeconds(2f);

        thisPlayerCamera.CallShakeCamera();

        yield return new WaitForSeconds(2f);

        Destroy(thisBreakFloor);

        thisAudioSource.PlayOneShot(thisBreakSound);

        yield return new WaitForSeconds(0.5f);

        foreach (GameObject aSceneObj in thisHideScene)
        {
            aSceneObj.SetActive(false);
        }

        yield return new WaitForSeconds(0.5f);

        foreach (GameObject aBossEssentialObj in thisBossFightObjs)
        {
            aBossEssentialObj.SetActive(true);
        }

        Destroy(gameObject);
    }

    protected void OnTriggerEnter(Collider other)
    {
        Player aPlayer = other.gameObject.GetComponent<Player>();

        if (aPlayer != null)
        {
            StartCoroutine(BossSceneEnterBehavior());
        }
    }
}

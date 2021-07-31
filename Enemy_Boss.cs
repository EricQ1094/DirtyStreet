using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss : MonoBehaviour
{
    private AudioSource thisAudioSource = null;
    private SpriteRenderer thisSpriteRenderer = null;
    private Rigidbody thisRigidbody = null;
    private Animator thisAnimator = null;
    private Player thisPlayer = null;
    private float thisDistancetoPlayer = 0f;
    [Header("Boss Attributes")]
    [SerializeField] private float thisHP = 0f;
    [SerializeField] private float thisDamage = 0f;
    private Color thisOriginColor = Color.white;
    private Color thisBeenHitColor = Color.red;
    [Header("Boss Status")]
    public bool canBeAttack = false;
    public bool canUpdateVelocity = false;
    public float thisBouceForce = 0f;
    public float thisUpForce = 0f;
    // For Boss Fight Phase 0
    public bool inPhase1 = true;
    private bool isNormalAttacking = false;
    private bool isFacingLeft = true;
    [SerializeField] private GameObject thisNormalLeftCollider = null;
    [SerializeField] private GameObject thisNormalRightCollider = null;
    // For Boss Fight Phase 1
    private bool startedSpinAttack = false;
    private bool spinAttack = false;
    private float thisSpinAttackTimer = 0f;
    [Header("Boss Fight Phase 1")]
    [SerializeField] private float thisSpinAttackCD = 0f;
    private Vector3 thisAttackDir = Vector3.zero;
    [SerializeField] private int thisHitWallTimes = 0;
    [SerializeField] private GameObject thisSpinCollider = null;
    // For Boss Fight Phase 2
    private bool startedTurretAttack = false;
    private bool turretAttack = false;
    [SerializeField] private GameObject thisPhase2Pos = null;
    [SerializeField] private GameObject thisTurret = null;
    private GameObject thisTurretIns = null;
    [SerializeField] private GameObject[] thisSupplies = null;
    [Header("Boss Sound Effect")]
    [SerializeField] private AudioClip thisOnDamageSound = null;
    [SerializeField] private AudioClip thisBlockSound = null;
    [SerializeField] private AudioClip thisHitGround = null;
    // Start is called before the first frame update
    void Start()
    {
        thisAudioSource = GetComponent<AudioSource>();
        thisAnimator = GetComponent<Animator>();
        thisRigidbody = GetComponent<Rigidbody>();
        thisSpriteRenderer = GetComponent<SpriteRenderer>();
        thisPlayer = GameObject.Find("Player").GetComponent<Player>();
        thisSpinAttackTimer = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpinAttack();
        if (canUpdateVelocity)
        {
            UpdateVelocity();
        }
        UpdateUI();

        if (thisHP >= 70f)
        {
            UpdateDistancetoPlayer();

            if (!isNormalAttacking)
            {
                UpdateFacing();

                if (thisDistancetoPlayer <= 2.5f)
                {
                    StartCoroutine(NormalAttackBehavior());
                }
            }
        }
    }

    protected void UpdateDistancetoPlayer()
    {
        thisDistancetoPlayer = (transform.position - thisPlayer.transform.position).magnitude;

        print(thisDistancetoPlayer);
    }

    protected void UpdateFacing()
    {
        float aXDifference = thisPlayer.transform.position.x - transform.position.x;

        if (aXDifference > 0f)
        {
            isFacingLeft = false;
        }

        else
        {
            isFacingLeft = true;
        }

        if (isFacingLeft)
        {
            thisSpriteRenderer.flipX = false;
        }

        else
        {
            thisSpriteRenderer.flipX = true;
        }
    }

    protected IEnumerator NormalAttackBehavior()
    {
        isNormalAttacking = true;

        thisAnimator.SetBool("isNormalAttack", true);

        yield return new WaitForSeconds(1.333f);

        if (isFacingLeft)
        {
            thisNormalLeftCollider.SetActive(true);
        }

        else
        {
            thisNormalRightCollider.SetActive(true);
        }

        yield return new WaitForSeconds(0.5f);

        thisNormalLeftCollider.SetActive(false);
        thisNormalRightCollider.SetActive(false);

        thisAnimator.SetBool("isNormalAttack", false);

        if (thisHP < 70f)
        {
            StartAttackInUpdate();
        }

        isNormalAttacking = false;
    }
    protected void StartAttackInUpdate()
    {
        if (!startedSpinAttack)
        {
            StartCoroutine(SpinAttackStartBehavior());

            startedSpinAttack = true;
        }

    }
    protected IEnumerator SpinAttackStartBehavior()
    {
        canBeAttack = false;

        thisAnimator.SetBool("isSpinAttackStart", true);

        yield return new WaitForSeconds(1.125f);

        thisAnimator.SetBool("isSpinAttackStart", false);

        thisAnimator.SetBool("isSpinAttackLoop", true);

        spinAttack = true;
    }

    protected IEnumerator SpinAttackBodyBehavior()
    {
        canUpdateVelocity = true;

        Vector3 aHitDir = thisPlayer.transform.position - transform.position;

        yield return new WaitForSeconds(1.0f);

        thisAttackDir = aHitDir.normalized;

        thisAttackDir.y = 0f;
    }

    protected void UpdateSpinAttack()
    {
        if (spinAttack)
        {
            thisSpinCollider.SetActive(true);

            if (thisSpinAttackTimer >= 0f)
            {
                thisSpinAttackTimer -= Time.deltaTime;
            }

            else
            {
                thisSpinAttackTimer = thisSpinAttackCD;

                StartCoroutine(SpinAttackBodyBehavior());
            }
        }

        else
        {
            thisSpinCollider.SetActive(false);
        }
    }
    protected IEnumerator TransformTurretAttackStartBehavior()
    {
        turretAttack = true;

        canBeAttack = false;

        thisAnimator.SetBool("isTurretAttackStart", true);

        thisRigidbody.AddForce(Vector3.up * 30000f);

        yield return new WaitForSeconds(0.5f);

        transform.position = thisPhase2Pos.transform.position;
    }
    protected IEnumerator TurretAttackStartBehavior()
    {
        turretAttack = true;

        canBeAttack = false;

        thisAnimator.SetBool("isTurretAttackStart", true);

        thisRigidbody.AddForce(Vector3.up * 5000f);

        yield return new WaitForSeconds(0.5f);
    }

    protected IEnumerator TurretAttackBodyBehavior()
    {
        yield return new WaitForSeconds(1.0f);

        thisRigidbody.AddForce(Vector3.up * 2000f);
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BossWall")
        {
            thisAttackDir = Vector3.zero;

            canUpdateVelocity = false;

            Vector3 aBounceDir = thisPlayer.transform.position - transform.position;

            thisRigidbody.AddForce(aBounceDir * thisBouceForce);

            thisRigidbody.AddForce(Vector3.up * thisUpForce);

            thisHitWallTimes++;

            thisAudioSource.PlayOneShot(thisHitGround);

            Camera.ThisPlayerCamera.CallShakeCamera();

            if (thisHitWallTimes >= 3)
            {
                StartCoroutine(BossStunBehavior());

                thisHitWallTimes = 0;
            }
        }

        if (turretAttack)
        {
            if (collision.gameObject.tag == "BossFloor")
            {
                Vector3 aTurretPos = thisPhase2Pos.transform.position;

                aTurretPos.x += Random.Range(-5f, 5f);
                aTurretPos.z += Random.Range(-5f, 5f);

                thisTurretIns = Instantiate(thisTurret, aTurretPos, Quaternion.identity);

                for (int iSupply = 0; iSupply < Random.Range(1, 4); iSupply++)
                {
                    Vector3 aSupplyPos = thisPhase2Pos.transform.position;

                    aSupplyPos.x += Random.Range(-10f, 10f);
                    aSupplyPos.z += Random.Range(-10f, 10f);

                    Instantiate(thisSupplies[Random.Range(0, thisSupplies.Length)], aSupplyPos, Quaternion.identity);
                }

                thisAudioSource.PlayOneShot(thisHitGround);

                Camera.ThisPlayerCamera.CallShakeCamera();
            }
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        Projectile_Enemy aProjectile = other.gameObject.GetComponent<Projectile_Enemy>();

        if (aProjectile != null)
        {
            Destroy(aProjectile.gameObject);

            Destroy(thisTurretIns);

            thisTurretIns = null;

            StartCoroutine(BossStunBehavior());
        }
    }

    protected IEnumerator BossStunBehavior()
    {
        canBeAttack = true;

        spinAttack = false;

        thisAnimator.SetBool("isSpinAttackLoop", false);

        thisAnimator.SetBool("isTurretAttackStart", false);

        yield return new WaitForSeconds(3.0f);

        if (thisHP > 50f)
        {
            StartCoroutine(SpinAttackStartBehavior());
        }

        else
        {
            if (startedTurretAttack)
            {
                StartCoroutine(TurretAttackStartBehavior());
            }

            else
            {
                StartCoroutine(TransformTurretAttackStartBehavior());

                startedTurretAttack = true;
            }
        }
    }

    protected void UpdateVelocity()
    {
        print(thisAttackDir);

        //thisAttackDir.y = thisRigidbody.velocity.y;

        thisRigidbody.velocity = thisAttackDir * 10f;
    }

    public void CallOnDamage(float aDamage)
    {
        if (canBeAttack)
        {
            StartCoroutine(OnDamageBehavior(aDamage));
        }

        else
        {
            thisAudioSource.PlayOneShot(thisBlockSound);
        }
    }

    protected IEnumerator OnDamageBehavior(float aDamage)
    {
        thisHP -= aDamage * 2f;

        thisAudioSource.PlayOneShot(thisOnDamageSound);

        thisSpriteRenderer.color = thisBeenHitColor;

        yield return new WaitForSeconds(0.05f);

        thisSpriteRenderer.color = thisOriginColor;

        if (thisHP <= 0f)
        {
            GlobalController.ThisGlobalController.LoadWinScene();
        }
    }
    protected void UpdateUI()
    {
        UIController.ThisUIController.SetBossHPBarScale(thisHP);
    }
}

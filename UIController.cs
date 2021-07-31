using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private static UIController thisInstance;

    public static UIController ThisUIController
    {
        get
        {
            return thisInstance;
        }
    }

    [SerializeField] private Text thisHintText = null;
    private float thisHintTextTimer = 0f;
    [Header("Player HUD")]
    [SerializeField] private Image thisPlayerHPBar = null;
    [Header("Boss HUD")]
    [SerializeField] private Image thisBossHPBar = null;
    // Start is called before the first frame update
    void Start()
    {
        thisInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHintTextTimer();
    }

    protected void UpdateHintTextTimer()
    {
        if (thisHintTextTimer > 0f)
        {
            thisHintTextTimer -= Time.deltaTime;
        }

        else
        {
            thisHintText.text = " ";
        }
    }

    public void SetHintText(string aString)
    {
        thisHintText.text = aString;

        thisHintTextTimer = 2f;
    }

    public void SetHPBarScale(float aHP)
    {
        float aScale = aHP / 20f;

        thisPlayerHPBar.rectTransform.localScale= new Vector3(aScale, 1f, 1f);
    }
    public void SetBossHPBarScale(float aHP)
    {
        float aScale = aHP / 100f;

        thisBossHPBar.rectTransform.localScale = new Vector3(aScale, 1f, 1f);
    }
}

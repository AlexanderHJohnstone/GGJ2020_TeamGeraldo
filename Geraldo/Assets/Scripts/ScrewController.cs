using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrewController : MonoBehaviour
{
    [SerializeField]
    private float rotationsUntilTightened = 2.0f;
    [SerializeField]
    private float rotationsUntilPopOut = 2.0f;

    //Color change Vars
    [SerializeField]
    private string matPropertyToChange = "_Color";
    [SerializeField]
    private Color tightenedColor = Color.red;
    [SerializeField]
    private Color loosenedColor = Color.blue;
    [SerializeField]
    private Color defaultColor = Color.white;

    //Debug Vars
    [SerializeField]
    private bool hasPlayer = false;

    [SerializeField]
    private float rotationCounter = 0;

    //Private Vars
    private Material screwMat;

    private Transform myTransform;

    private Rigidbody screwRB;

    private Animator anim;

    private bool turningEnabled = true;

    private bool fullyTightened = false;

    private float playerRot;

    private float animationPercentage = 0.5f;

    private CapsuleCollider myCol;

    #region Temp Vars
    [SerializeField]
    private Slider rotationSlider;
    #endregion


    private void Start()
    {
        //convert the rotation min and max vars to rotation values
        rotationsUntilTightened *= 360.0f;
        rotationsUntilPopOut *= -360.0f;

        //setup component connections
        myTransform = GetComponent<Transform>();
        myCol = GetComponent<CapsuleCollider>();
        anim = GetComponentInChildren<Animator>();
        screwRB = GetComponentInChildren<Rigidbody>();
        screwMat = GetComponentInChildren<Renderer>().material;
    }

    private void Update()
    {
        //Temp Code
        playerRot = rotationSlider.value * 4;
        rotationCounter = 360 * playerRot;
        //end temp


        //rotationCounter will be pulled from player script
        if (turningEnabled && hasPlayer)
        {
            if (!fullyTightened)
            {
                myTransform.eulerAngles = new Vector3(0, 0, rotationCounter);
                UpdateAnimation();

                if (rotationCounter >= rotationsUntilTightened)
                    SetTightended();
                else if (rotationCounter <= rotationsUntilPopOut)
                    ScrewFallOut();
            }
            else
            {
                if (rotationCounter < rotationsUntilTightened)
                {
                    myTransform.eulerAngles = new Vector3(0, 0, rotationCounter);
                    UpdateAnimation();

                    SetBackToNorm();
                }

            }
        }
    }


    private void UpdateAnimation()
    {
        animationPercentage = Mathf.InverseLerp(rotationsUntilTightened, rotationsUntilPopOut, rotationCounter);
        anim.SetFloat("AnimationTime", animationPercentage);
    }


    /// <summary>
    /// Undoes the fully tightened thing and allows the screw to move as normal again
    /// </summary>
    private void SetBackToNorm()
    {
        fullyTightened = false;
        screwMat.SetColor(matPropertyToChange, defaultColor);
    }

    /// <summary>
    /// Flips turning bool,
    /// stops player's momentum?
    /// </summary>
    private void SetTightended()
    {
        fullyTightened = true;
        screwMat.SetColor(matPropertyToChange, tightenedColor);
        //stop player's momentum?
    }


    /// <summary>
    ///turns on physics and dissable animator for screw object, 
    ///turns off this thing's grapple collision, 
    ///makes player ungrapple if grappled
    ////// <summary>
    private void ScrewFallOut()
    {
        turningEnabled = false;
        screwRB.useGravity = true;
        screwRB.isKinematic = false;
        anim.enabled = false;
        myCol.enabled = false;
        screwMat.SetColor(matPropertyToChange, loosenedColor);
        //player let go of screw
    }
}

﻿using System.Collections;
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

    [SerializeField]
    GameObject nut;

    public bool hasPlayer = false;

    public float rotationOnLatch = 0f;


    //Private Vars
    private PlayerMovementController pController;

    private Material screwMat;

    private Transform myTransform;

    private Rigidbody screwRB;

    private Animator anim;

    private bool turningEnabled = true;

    private bool fullyTightened = false;

    //private float playerRot;

    private float animationPercentage = 0.5f;

    private CapsuleCollider myCol;

    [SerializeField]
    private float playerRotationDir = -1;

    private void Start()
    {
        //convert the rotation min and max vars to rotation values

        pController = GameObject.FindObjectOfType<PlayerMovementController>();

        //setup component connections
        myTransform = GetComponent<Transform>();
        myCol = GetComponent<CapsuleCollider>();
        anim = nut.GetComponent<Animator>();
        screwRB = nut.GetComponent<Rigidbody>();
        screwMat = nut.GetComponent<Renderer>().material;
    }

    private float _angleLastFrame = 0;
    private float _angleEslaped = 0f;

    [SerializeField]
    private int counter = 0;
    private void Update()
    {
        //rotationCounter will be pulled from player script
        if (turningEnabled && hasPlayer)
        {
            float angleDif = Mathf.Min(
                Mathf.Abs(pController._angle - _angleLastFrame),
                Mathf.Abs(Mathf.Abs(pController._angle - _angleLastFrame) - 360));

            _angleEslaped += angleDif;

            if ((int)(_angleEslaped / 360) > 0)
            {
                _angleEslaped -= 360f;
                counter++;
            }

            _angleLastFrame = pController._angle;

            float screwAngle = counter * 360 + _angleEslaped * playerRotationDir;
            if (!fullyTightened)
            {
                myTransform.eulerAngles = new Vector3(0, 0, screwAngle);
                UpdateAnimation();

                if (counter >= rotationsUntilTightened)
                    SetTightended();
                else if (counter >= rotationsUntilPopOut)
                    ScrewFallOut();
            }
            else
            {
                if (counter < rotationsUntilTightened)
                {
                    myTransform.eulerAngles = new Vector3(0, 0, screwAngle);
                    UpdateAnimation();
                    SetBackToNorm();
                }
            }
        }
    }

    public void OnPlayerLatch()
    {
        hasPlayer = true;

        playerRotationDir = pController._rotationDirection;
        _angleEslaped = 0f;
        _angleLastFrame = pController._angle;
        _angleEslaped = 0f;
        counter = 0;
        rotationOnLatch = pController._angle;
    }

    public void OnPlayerDetach()
    {
        hasPlayer = false;
    }

    private void UpdateAnimation()
    {
        float percent = Mathf.InverseLerp(-rotationsUntilTightened, rotationsUntilPopOut, _angleEslaped / 360 * -playerRotationDir);
        anim.SetFloat("AnimationTime", percent);
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
        pController.ReleaseGrapple();
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
        pController.ReleaseGrapple();

        //player let go of screw
    }
}

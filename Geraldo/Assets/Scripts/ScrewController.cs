using UnityEngine;

public class ScrewController : MonoBehaviour
{
    [SerializeField]
    private float rotationsUntilTightened = 2;
    private float _directedRotationUntilTightened => -rotationsUntilTightened;

    [SerializeField]
    private float rotationsUntilPopOut = 2;
    private float _directedRotationsUntilPopOut => rotationsUntilPopOut;

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

    private CapsuleCollider myCol;

    [SerializeField]
    private float playerRotationDir = -1;

    private void Start()
    {
        //convert the rotation min and max vars to rotation values

        pController = FindObjectOfType<PlayerMovementController>();

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
    private float counter = 0;

    private void Update()
    {
        //rotationCounter will be pulled from player script
        if (turningEnabled && hasPlayer)
        {
            float angleDif = Mathf.Min(
                Mathf.Abs(pController._angle - _angleLastFrame),
                Mathf.Abs(Mathf.Abs(pController._angle - _angleLastFrame) - 360));

            _angleEslaped += angleDif;

            counter = _angleEslaped / 360f * playerRotationDir;
            _angleLastFrame = pController._angle;

            float screwAngle = counter * 360;

            if (!fullyTightened)
            {
                myTransform.eulerAngles = new Vector3(0, 0, screwAngle);
                UpdateAnimation();

                if (counter <= _directedRotationUntilTightened)
                    SetTightended();
                else if (counter >= _directedRotationsUntilPopOut)
                    ScrewFallOut();
            }
            else
            {
                if (counter > _directedRotationUntilTightened)
                {
                    myTransform.eulerAngles = new Vector3(0, 0, screwAngle);
                    UpdateAnimation();
                    SetBackToNorm();
                }
            }
        }
    }

    private void OnValidate()
    {
        rotationsUntilTightened = Mathf.Max(0.1f, rotationsUntilTightened);
        rotationsUntilPopOut = Mathf.Max(0.1f, rotationsUntilPopOut);
    }

    public void OnPlayerLatch()
    {
        hasPlayer = true;

        playerRotationDir = pController._rotationDirection;
        _angleEslaped = 0f;
        _angleLastFrame = pController._angle;
        counter = 0;
        rotationOnLatch = pController._angle;
    }

    public void OnPlayerDetach()
    {
        hasPlayer = false;
    }

    private void UpdateAnimation()
    {
        float percent = Mathf.InverseLerp(_directedRotationUntilTightened, _directedRotationsUntilPopOut, -1 * counter);
        percent = Mathf.Min(percent, 0.97f); //1 reset clip to 0:00
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMover : MonoBehaviour
{
    public Transform playerCam;
    public bool isGrounded;

    public float life = 1000;
    public float maxLife = 1000;
    [SerializeReference] private Slider sliderHealth;
    [SerializeReference] private GameObject canvasLoosing;
    [SerializeReference] private GameObject canvasWin;
    public bool hasLost = false;

    public int numberBeforeWin = 18;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        canvasLoosing.SetActive(false);


        life = maxLife;
        sliderHealth.value = calculationHealth();

        if (playerCam == null)
        {
            Camera cam = transform.GetComponentInChildren<Camera>();
            playerCam = cam.transform;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
        }

        sliderHealth.value = calculationHealth();

        if (numberBeforeWin <= 0)
        {
            canvasWin.SetActive(true);
            hasLost = true;
            Time.timeScale = 0;
        }

        if (life <= 0)
        {
            canvasLoosing.SetActive(true);
            hasLost = true;
            Time.timeScale = 0;
        }

        if (!hasLost)
        {
            //Sauve la rotation
            Quaternion lastRotation = playerCam.rotation;

            //Baisse / leve la tete
            float rot = Input.GetAxis("Mouse Y") * -10;
            Quaternion q = Quaternion.AngleAxis(rot, playerCam.right);
            playerCam.rotation = q * playerCam.rotation;

            //Est ce qu'on a la tete Ã  l'envers ?
            Vector3 forwardCam = playerCam.forward;
            Vector3 forwardPlayer = transform.forward;
            float regardeDevant = Vector3.Dot(forwardCam, forwardPlayer);
            if (regardeDevant < 0.0f)
                playerCam.rotation = lastRotation;

            //Tourner gauche droite
            rot = Input.GetAxis("Mouse X") * 10;
            q = Quaternion.AngleAxis(rot, transform.up);
            transform.rotation = q * transform.rotation;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!hasLost)
        {
            Rigidbody rb;
            rb = GetComponent<Rigidbody>();

            float vert = Input.GetAxis("Vertical");
            float hori = Input.GetAxis("Horizontal");

            Vector3 horizontalVelocity = Vector3.zero;
            horizontalVelocity += vert * transform.forward * 10;
            horizontalVelocity += hori * transform.right * 10;
            rb.velocity = new Vector3(horizontalVelocity.x,
                rb.velocity.y,
                horizontalVelocity.z);

            //Est ce qu'on touche le sol ?
            isGrounded = false;
            RaycastHit infos;
            bool trouve = Physics.SphereCast(transform.position + transform.up * 0.1f,
                0.05f, -transform.up, out infos, 2);
            if (trouve && infos.distance < 0.15)
                isGrounded = true;

            if (Input.GetButton("Jump"))
            {
                if (isGrounded)
                {
                    rb.AddForce(transform.up * 10, ForceMode.Impulse);
                    isGrounded = false;
                }
                else
                {
                    if (rb.velocity.y < 3)
                    {
                        rb.AddForce(transform.up * 50);
                    }
                }
            }
        }
    }

    private float calculationHealth()
    {
        return life / maxLife;
    }
}
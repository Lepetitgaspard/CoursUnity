using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIMover : MonoBehaviour
{
    [Tooltip("Vitesse de déplacement"), Range(1, 15)]
    public float linearSpeed = 6;
    [Tooltip("Vitesse de rotation"), Range(1, 5)]
    public float angularSpeed = 1;

    private Transform player;

    public Vector3 dirPlayer;

    public float life = 100;
    public float maxLife = 1000;
    [SerializeReference] private GameObject healthUI;
    [SerializeReference] private Slider sliderHealth;
    [SerializeReference] private int damage = 15;
    private GameObject goPlayer;

    public void Start()
    {
        life = maxLife;
        sliderHealth.value = calculationHealth();
        goPlayer = GameObject.FindGameObjectWithTag("Player");
        player = goPlayer.transform;
    }

    void FixedUpdate()
    {
        sliderHealth.value = calculationHealth();
        if (life < maxLife)
        {
            healthUI.SetActive(true);
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            dirPlayer = player.position - transform.position;
            //dirPlayer = new Vector3(dirPlayer.x, 0, dirPlayer.z);
            dirPlayer = dirPlayer.normalized;

            float angle = Vector3.SignedAngle(dirPlayer,
                transform.forward,
                transform.up);

            if (angle > 10)
            {
                rb.AddTorque(transform.up * -100);
            }
            else if (angle < -10)
            {
                rb.AddTorque(transform.up * 100);
            }

            if (Mathf.Abs(angle) < 10 && rb.velocity.magnitude < 3)
            {
                rb.AddForce(transform.forward * 40);
            }

            Animator anim = GetComponent<Animator>();
            if (anim != null)
            {
                //anim.SetFloat("Speed", rb.velocity.magnitude);
            }
        }

        if (life <= 0)
        {
            Destroy(gameObject);
            goPlayer.GetComponent<PlayerMover>().numberBeforeWin--;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + dirPlayer);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            var script = collision.transform.GetComponent<PlayerMover>();
            script.life = script.life - damage;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            var script = collision.transform.GetComponent<PlayerMover>();
            script.life = script.life - damage;
        }
    }

    private float calculationHealth()
    {
        return life / maxLife;
    }
}
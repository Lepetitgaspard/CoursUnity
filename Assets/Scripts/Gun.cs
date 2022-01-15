using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 25f;
    public float range = 1000f;

    [SerializeReference] private Camera fpsCam;
    [SerializeReference] private GameObject particle;
    [SerializeReference] private Material hitMaterial;
    [SerializeReference] private Material defaultMaterial;
    private Transform selected;
    [SerializeReference] private GameObject player;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1") && !player.GetComponent<PlayerMover>().hasLost)
        {
            particle.SetActive(true);
        }

        if (Input.GetButton("Fire1") && !player.GetComponent<PlayerMover>().hasLost)
        {
            Shoot();
        }
        else if (Input.GetButtonUp("Fire1") && !player.GetComponent<PlayerMover>().hasLost)
        {
            particle.SetActive(false);
            deSelection();
        }
    }

    void Shoot()
    {
        deSelection();

        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            var selection = hit.transform;
            AIMover target = hit.transform.GetComponent<AIMover>();
            if(target != null)
            {
                target.life = target.life - damage;
                var selectionRenderer = selection.GetComponentInChildren<Renderer>();
                if (selectionRenderer != null)
                {
                    selectionRenderer.material = hitMaterial;
                }
                selected = selection;
            }
        }
    }

    void deSelection()
    {
        if (selected != null)
        {
            var selectionRenderer = selected.GetComponentInChildren<Renderer>();
            selectionRenderer.material = defaultMaterial;
            selected = null;
        }
    }
}

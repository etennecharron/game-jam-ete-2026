using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class TornadoV2 : MonoBehaviour
{
    public float pullForce = 15f;
    public float rotateForce = 50f;
    public float upwardForce = 5f;

    public BoxCollider map;
    Vector3 min;
    Vector3 max;
    public float speed = 5f;
    Vector3 direction = new Vector3(1, 0, 1);

    private void Start()
    {
        min = map.bounds.min;
        max = map.bounds.max;
    }

    private List<Rigidbody> affectedBodies = new List<Rigidbody>();

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null && !affectedBodies.Contains(rb))
        {
            rb.constraints = RigidbodyConstraints.None;
            affectedBodies.Add(rb);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null)
        {
            affectedBodies.Remove(rb);
            if(rb.transform.CompareTag("Player"))
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation;
            }
        }
    }

    private void FixedUpdate()
    {
        MoveTornado();
        WindEffect();
        CleanList();
    }

    private void CleanList()
    {
        affectedBodies.RemoveAll(rb => rb == null);
    }
    private void MoveTornado()
    {
        transform.position = new Vector3(transform.position.x + (direction.x *speed * Time.fixedDeltaTime), transform.position.y, transform.position.z + (direction.z * speed * Time.fixedDeltaTime));
        ClampToMap();
    }
    private void WindEffect()
    {
        foreach (Rigidbody rb in affectedBodies)
        {
            if (rb == null) continue;

            Vector3 dirToCenter = (transform.position - rb.position).normalized;


            rb.AddForce(dirToCenter * pullForce, ForceMode.Force);


            Vector3 swirl = Vector3.Cross(dirToCenter, Vector3.up);
            rb.AddForce(swirl * rotateForce, ForceMode.Force);


            rb.AddForce(Vector3.up * upwardForce, ForceMode.Force);
        }
    }

    private void ClampToMap()
    {
        Vector3 pos = transform.position;

        if (pos.x <= map.bounds.min.x || pos.x >= map.bounds.max.x)
        {
            direction.x *= -1;
            pos.x = Mathf.Clamp(pos.x, map.bounds.min.x, map.bounds.max.x);
        }

        if (pos.z <= map.bounds.min.z || pos.z >= map.bounds.max.z)
        {
            direction.z *= -1;
            pos.z = Mathf.Clamp(pos.z, map.bounds.min.z, map.bounds.max.z);
        }

        transform.position = pos;
    }
}
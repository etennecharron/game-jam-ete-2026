using UnityEngine;

public class ProjectileActions : MonoBehaviour
{
    private Rigidbody rb;

    private bool targetHit;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Sticks to the tar
        if (targetHit)
            return;
        else
            targetHit = true;
    }
}

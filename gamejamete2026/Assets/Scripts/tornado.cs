using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Tornado : MonoBehaviour
{
    [Header("Déplacement")]
    public string targetTag = "TornadoZone";
    public float speed = 5f;
    public float checkDistance = 2f;
    public LayerMask whatIsGround;
    Rigidbody rb;

    [Header("Zone tornade")]
    public float radius = 15f;

    [Header("Forces tornade")]
    public float pullForce = 600f;
    public float spinForce = 900f;
    public float liftForce = 500f;

    private void FixedUpdate()
    {
        MoveTornado();

        Collider[] hits = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider col in hits)
        {
            Rigidbody rb = col.attachedRigidbody;
            if (rb == null) continue;

            Vector3 offset = rb.position - transform.position;
            float distance = Mathf.Max(offset.magnitude, 0.1f);

            float strength = 1f - Mathf.Clamp01(distance / radius);

            Vector3 pull = -offset.normalized * pullForce;
            Vector3 spin = Vector3.Cross(Vector3.up, offset).normalized * spinForce;
            Vector3 lift = Vector3.up * liftForce;

            rb.AddForce((pull + spin + lift) * strength, ForceMode.Force);
        }

        
    }

    public void MoveTornado()
    {
        Vector3 move = transform.forward * speed * Time.deltaTime;

        Vector3 checkPos = transform.position + move + Vector3.up;

        if (Physics.Raycast(checkPos, Vector3.down, checkDistance, whatIsGround))
        {
            transform.position += move;
        }
        else
        {
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            transform.position,
            transform.position + Vector3.up * 5f
        );
    }


}
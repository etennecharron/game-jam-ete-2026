using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
    private Rigidbody objectRB;
    private Transform objectGrabPointTransform;

    private void Awake()
    {
        objectRB = GetComponent<Rigidbody>();
    }
    public void Grab(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRB.useGravity = false;
    }
    public void Release()
    {
        objectGrabPointTransform = null;
        objectRB.useGravity = true;
    }
    public void Throw(float throwForce, float throwUpwardForce, Transform cam)
    {
        Vector3 forceDirection = cam.transform.forward;
        RaycastHit hit;
        if (Physics.Raycast(cam.position, -cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - objectGrabPointTransform.position).normalized;
        }
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;
        objectRB.AddForce(forceToAdd, ForceMode.Impulse);
        objectGrabPointTransform = null;
        objectRB.useGravity = true;
    }
    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            float lerpSpeed = 20f;
            Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            objectRB.MovePosition(objectGrabPointTransform.position);
        }
    }
}

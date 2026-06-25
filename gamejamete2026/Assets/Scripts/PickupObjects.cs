using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PickupObjects : MonoBehaviour
{
    [SerializeField] private Transform playerCamTransform;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private float pickupDistance = 20f;
    [SerializeField] private LayerMask pickUpLayerMask;
    private ObjectGrabbable objectGrabbable;

    [SerializeField] float throwForce = 600f;
    [SerializeField] float throwUpwardForce;

    private void Update()
    {
        //Grabs the object when mouse button is held down and the object is in range and is grabbable
        if (Input.GetMouseButtonDown(0))
        {
            if (objectGrabbable == null)
            {
                if (Physics.Raycast(playerCamTransform.position, playerCamTransform.forward, out RaycastHit raycastHit, pickupDistance))
                {
                    //Debug.Log(raycastHit.transform);
                    if (raycastHit.transform.TryGetComponent(out ObjectGrabbable objectGrabbable))
                    {
                        this.objectGrabbable = objectGrabbable;
                        objectGrabbable.Grab(objectGrabPointTransform);
                        //Debug.Log(objectGrabbable);
                    }
                }
            }
        }
        //Drop the object when mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            if (objectGrabbable != null)
            {
                objectGrabbable.Release();
                objectGrabbable = null;
            }
        }
        //Throw the object when right mouse button is pressed
        if (Input.GetMouseButton(1))
        {
            if(objectGrabbable != null)
            {
                objectGrabbable.Throw(throwForce, throwUpwardForce, playerCamTransform);
                objectGrabbable = null;
            }
        }
    }
}
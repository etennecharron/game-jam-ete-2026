using UnityEngine;

public class PickupObjects : MonoBehaviour
{
    bool isHoldingObject = false;
    
    TempParent tempParent;
    Rigidbody rb;

    Vector3 objectPos;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tempParent = TempParent.Instance;
    }

    void Update()
    {
        if(isHoldingObject)
        {
            Hold();
        }
    }

    private void OnMouseDown()
    {
        //pickup
        if (tempParent != null)
        {
            isHoldingObject = true;
            rb.useGravity = false;
            rb.detectCollisions = true;

            this.transform.SetParent(tempParent.transform);
        }
        else
        {
            Debug.Log("Temp Parent not in scene");
        }
    }
    private void OnMouseUp()
    {
        //drop
        isHoldingObject = false;
        rb.useGravity = true;
        rb.detectCollisions = true;
        this.transform.SetParent(null);
    }
    private void OnMouseExit()
    {
        //drop
        isHoldingObject = false;
        rb.useGravity = true;
        rb.detectCollisions = true;
        this.transform.SetParent(null);
    }
    private void Hold()
    {
        objectPos = tempParent.transform.position;
        this.transform.position = objectPos;
    }
}

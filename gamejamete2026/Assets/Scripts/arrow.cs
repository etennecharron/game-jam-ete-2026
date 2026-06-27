using UnityEngine;

public class arrow : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        if (target == null)
            return;

        Vector3 direction = target.position - transform.position;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}

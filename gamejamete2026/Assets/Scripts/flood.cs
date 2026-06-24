using System.Collections;
using UnityEngine;

public class Flood : MonoBehaviour
{

    public GameObject ocean;
    public float maxHeight;
    private bool isFlooding;
    public float time;

    private void Start()
    {
        StartFlood();
    }

    public void StartFlood()
    {
        isFlooding = true;
        StartCoroutine(AnimateFlood());
    }


    private IEnumerator AnimateFlood()
    {
        
        float startPos = ocean.transform.position.y;
        float targetPos = startPos + maxHeight;

        float elapsed = 0f;
        Vector3 pos = ocean.transform.position;

        while (isFlooding)
        {
            while (elapsed < time)
            {
                elapsed += Time.deltaTime;

                float y = Mathf.Lerp(startPos, targetPos, elapsed / time);
                ocean.transform.position = new Vector3(pos.x, y, pos.z);

                yield return null;
            }

            ocean.transform.position = new Vector3(pos.x, targetPos, pos.z);

            yield return new WaitForSeconds(1f);

            elapsed = 0f;

            while (elapsed < time)
            {
                elapsed += Time.deltaTime;

                float y = Mathf.Lerp(targetPos, startPos, elapsed / time);
                ocean.transform.position = new Vector3(pos.x, y, pos.z);

                yield return null;
            }
        }



        ocean.transform.position = new Vector3(pos.x, startPos, pos.z);
    }

}

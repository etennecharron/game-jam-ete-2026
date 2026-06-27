using UnityEngine;

public class FloodDisaster : MonoBehaviour
{
    public GameObject ocean;
    public float maxHeight = 5f;
    public float time = 3f;

    private bool isFlooding;

    private float startHeight;
    private Vector3 startPos;
    private float timer;

    private void Start()
    {
        startPos = ocean.transform.position;
        startHeight = startPos.y;
    }

    private void Update()
    {
        if (!isFlooding)
            return;

        timer += Time.deltaTime;

        // Va de 0 à 1 puis revient à 0
        float t = Mathf.PingPong(timer / time, 1f);

        float y = Mathf.Lerp(startHeight, startHeight + maxHeight, t);

        ocean.transform.position = new Vector3(startPos.x, y, startPos.z);
    }

    public void StartFlood()
    {
        timer = 0f;
        isFlooding = true;
    }

    public void StopFlood()
    {
        isFlooding = false;
        ocean.transform.position = startPos;
    }
}
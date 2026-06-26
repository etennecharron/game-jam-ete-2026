using System.Collections.Generic;
using UnityEngine;

public class EventWheel : MonoBehaviour
{
    [Header("Keybinds")]
    public KeyCode eventKey = KeyCode.E;

    private bool inRange = false;
    private bool eventActivated = false;

    public GameObject[] spawns;
    public GameObject wheel;

    [Header("Wheel Visual")]
    public RectTransform wheelUI;
    public WheelSection sectionPrefab;
    public float iconRadius = 150f;

    public List<Disaster> eventList = new List<Disaster>();

    private List<WheelSection> sections = new List<WheelSection>();

    private Disaster currentResult;


    public abstract class Disaster
    {
        public string Name { get; private set; }
        public float Luck { get; private set; }

        public Disaster(string name, float luck)
        {
            Name = name;
            Luck = luck;
        }

        public abstract void Action();
    }

    private class Flood : Disaster
    {
        public Flood() : base("Flood", 30) { }

        public override void Action()
        {
            Debug.Log("FLOOOOOODS AND STUFF");
        }
    }

    private class BuffMonkeys : Disaster
    {
        public BuffMonkeys() : base("Stronger monkeys", 30) { }

        public override void Action()
        {
            Debug.Log("Monkeys are stronger!");
        }
    }

    private class Tornados : Disaster
    {
        public Tornados() : base("Tornado alert!", 30) { }

        public override void Action()
        {
            Debug.Log("Careful for the wind!");
        }
    }

    private class Win : Disaster
    {
        public Win() : base("Victory", 1) { }

        public override void Action()
        {
            Debug.Log("VICTORY!");
        }
    }


    void Start()
    {
        eventList.Add(new Flood());
        eventList.Add(new BuffMonkeys());
        eventList.Add(new Win());

        CreateWheel();
        UpdateWheel();
    }

    void Update()
    {
        myInput();
    }

    private void myInput()
    {
        if (Input.GetKey(eventKey) && inRange && !eventActivated)
        {
            eventActivated = true;

            currentResult = GetRdmEvent(); 

            MoveWheel();

            SpinToEvent(currentResult);
        }
    }

    private void MoveWheel()
    {
        int random = Random.Range(0, spawns.Length);
        wheel.transform.position = spawns[random].transform.position;
    }


    public Disaster GetRdmEvent()
    {
        float total = 0f;

        foreach (var e in eventList)
            total += e.Luck;

        float random = Random.Range(0, total);

        foreach (var e in eventList)
        {
            random -= e.Luck;

            if (random <= 0)
                return e;
        }

        return eventList[0];
    }

    public void CreateWheel()
    {
        foreach (Transform child in wheelUI)
            Destroy(child.gameObject);

        sections.Clear();

        for (int i = 0; i < eventList.Count; i++)
        {
            WheelSection s = Instantiate(sectionPrefab, wheelUI);
            s.gameObject.SetActive(true);

            sections.Add(s);
        }
    }


    public void UpdateWheel()
    {
        float total = 0f;

        foreach (var e in eventList)
            total += e.Luck;

        float angle = 0f;

        for (int i = 0; i < eventList.Count; i++)
        {
            Disaster d = eventList[i];
            WheelSection s = sections[i];

            float percent = d.Luck / total;
            float size = percent * 360f;

            s.fillImage.fillAmount = percent;
            s.fillImage.rectTransform.localRotation =
                Quaternion.Euler(0, 0, -angle);

            if (s.text != null)
                s.text.text = d.Name;

            float mid = angle + size * 0.5f;

            Vector2 pos = new Vector2(
                Mathf.Cos(mid * Mathf.Deg2Rad),
                Mathf.Sin(mid * Mathf.Deg2Rad)
            ) * iconRadius;

            s.icon.rectTransform.anchoredPosition = pos;

            angle += size;
        }
    }

    private void SpinToEvent(Disaster target)
    {
        float total = 0f;

        foreach (var e in eventList)
            total += e.Luck;

        float startAngle = 0f;

        int index = eventList.IndexOf(target);

        for (int i = 0; i < index; i++)
            startAngle += (eventList[i].Luck / total) * 360f;

        float size = (target.Luck / total) * 360f;

        float middle = startAngle + size * 0.5f;

        float finalAngle = 360f * Random.Range(5, 8) + middle;

        StartCoroutine(SpinAnimation(finalAngle));
    }

    private System.Collections.IEnumerator SpinAnimation(float targetAngle)
    {
        float start = wheelUI.eulerAngles.z;
        float t = 0f;
        float duration = 4f;

        while (t < duration)
        {
            t += Time.deltaTime;

            float lerp = t / duration;
            lerp = 1 - Mathf.Pow(1 - lerp, 3);

            float angle = Mathf.Lerp(start, targetAngle, lerp);

            wheelUI.rotation = Quaternion.Euler(0, 0, angle);

            yield return null;
        }

        wheelUI.rotation = Quaternion.Euler(0, 0, targetAngle);

        currentResult.Action();

        eventActivated = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        inRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        inRange = false;
    }
}
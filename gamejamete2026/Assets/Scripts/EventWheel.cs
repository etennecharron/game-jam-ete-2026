using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using static Unity.VisualScripting.FlowStateWidget;

public class EventWheel : MonoBehaviour
{

    public List<Disaster> eventList = new List<Disaster>();


    public abstract class Disaster
    {
        public string Name { get; private set; }
        public float Luck { get; private set; }
        public Disaster(string name, float luck)
        {
            Name = name;
            Luck = luck;
        }
        abstract public void Action();
    }

    private class Flood : Disaster
    {
        public Flood() : base("Flood", 30)
        { }

        override
        public void Action()
        {
            Debug.Log("FLOOOOOODS AND STUFF");
        }

    }
    private class BuffMonkeys : Disaster
    {
        public BuffMonkeys() : base("Stronger monkeys", 30)
        { }

        override
        public void Action()
        {
            Debug.Log("Monkeys are stronger!");
        }
    }
    private class Win : Disaster
    {
        public Win() : base("Victory", 1)
        { }

        override
        public void Action()
        {
            Debug.Log("VICTORY!");
        }
    }

    public Disaster GetRdmEvent()
    {
        float total = 0;
        foreach (Disaster e in eventList)
            total += e.Luck;

        float random = Random.Range(0, total);

        foreach (Disaster e in eventList)
        {

            random -= e.Luck;

            if (random <= 0)
                return e;
        }

        return eventList[0];
    }

    public void Start()
    {
        eventList.Add(new Flood());
        eventList.Add(new BuffMonkeys());
        eventList.Add(new Win());

    }


}

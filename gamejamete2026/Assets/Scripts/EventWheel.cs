using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.VFX;
using static Unity.VisualScripting.FlowStateWidget;

public class EventWheel : MonoBehaviour
{

    [Header("Keybinds")]
    public KeyCode eventKey = KeyCode.E;

    private bool inRange = false;
    private bool eventActivated = false;

    public GameObject[] spawns;
    public GameObject wheel;

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

    private class Tornados : Disaster
    {
        public Tornados() : base("Tornado alert!", 30)
        { }

        override
        public void Action()
        {
            Debug.Log("Carefull for the wind!");
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

    private void MoveWheel()
    {
        int random = UnityEngine.Random.Range(0, spawns.Length);
        wheel.transform.position = new Vector3(spawns[random].transform.position.x, spawns[random].transform.position.y, spawns[random].transform.position.z);
    }

    public Disaster GetRdmEvent()
    {
        float total = 0;
        foreach (Disaster e in eventList)
            total += e.Luck;

        float random = UnityEngine.Random.Range(0, total);

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

    private void Update()
    {
        myInput();
    }

    private void myInput()
    {
        if (Input.GetKey(eventKey) && inRange && !eventActivated)
        {
            /**
             * 
             * add eventActivated for the spinning wheel animation B)
                eventActivated = true;
            
             */
            GetRdmEvent();
            MoveWheel();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enters");
        inRange = true;
    }
    private void OnTriggerExit(Collider other)
    {
        inRange = false;
    }

}

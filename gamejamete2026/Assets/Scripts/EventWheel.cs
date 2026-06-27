using EasyUI.PickerWheelUI;
using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class EventWheel : MonoBehaviour
{

    [Header("Keybinds")]
    public KeyCode eventKey = KeyCode.E;

    private bool inRange = false;
    private bool spinning = false;

    public GameObject[] spawns;
    public GameObject wheel;

    public List<Disaster> eventList = new List<Disaster>();

    public GameObject tornado;
    public FloodDisaster flood;

    [SerializeField] private PickerWheel pickerWheel;

    public TextMeshProUGUI wheelMessage;
    private Coroutine currentMessage;

    public gameManager gameManager;

    public abstract class Disaster
    {
        public bool activated { get; protected set; }
        public string Name { get; private set; }
        public float Luck { get; private set; }
        public string Message { get; protected set; }
        public Disaster(string name, float luck)
        {
            Name = name;
            Luck = luck;
        }
        abstract public void Action();
    }

    private class Flood : Disaster
    {

        public FloodDisaster FloodDisaster { get; protected set; }

        public Flood(FloodDisaster floodDisaster) : base("Flood", 30)
        {
            FloodDisaster = floodDisaster;
        }

        override
        public void Action()
        {
            if (!activated)
            {
                activated = true;
                FloodDisaster.StartFlood();
            }
            else
            {
                FloodDisaster.time *= 0.9f;
                FloodDisaster.maxHeight *= 1.1f;
                    
            }
        }

    }

    private class Tornados : Disaster
    {
        private GameObject Tornado;
        public Tornados(GameObject tornado) : base("Tornado", 30)
        { 
            Tornado = tornado;
            Message = "The wind is getting stronger!";
        }

        override
        public void Action()
        {
            if (!activated)
            {
                this.activated = true;
                Tornado.SetActive(true);
            }
            else
            {
                TornadoV2 script = Tornado.GetComponent<TornadoV2>();
                Tornado.transform.localScale *= 1.5f;
                script.pullForce *= 1.5f;
                script.rotateForce *= 1.5f;
                script.upwardForce *= 1.5f;
                script.speed *= 1.5f;
            }
        }
    }
    private class Win : Disaster
    {
        private gameManager GameManager;
        public Win(gameManager gameManager) : base("Victory", 1)
        {
            Message = "You won!";
        }

        override
        public void Action()
        {
            GameManager.wonGame();
        }
    }

    private void MoveWheel()
    {
        int random = UnityEngine.Random.Range(0, spawns.Length);
        wheel.transform.position = new Vector3(spawns[random].transform.position.x, spawns[random].transform.position.y, spawns[random].transform.position.z);
    }

    public void Start()
    {
        tornado.SetActive(false);
        eventList.Add(new Tornados(tornado));
        eventList.Add(new Win(gameManager));
        eventList.Add(new Flood(flood));

    }

    private void Update()
    {
        myInput();
    }

    private void myInput()
    {
        if (Input.GetKey(eventKey) && inRange && !spinning)
        {
            pickerWheel.OnSpinStart(() =>
            {
                spinning = true;
            });


            pickerWheel.OnSpinEnd(wheelPiece =>
            {
                GetEvent(wheelPiece.Label);
                spinning = false;
                MoveWheel();
            });

            pickerWheel.Spin();


        }
    }

    public void GetEvent(String label)
    {
        for(int i = 0; i < eventList.Count; i++)
        {
            if (label.Equals(eventList[i].Name))
            {
                eventList[i].Action();
                ShowMessage(eventList[i].Message);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        inRange = true;
    }
    private void OnTriggerExit(Collider other)
    {
        inRange = false;
    }


    public void ShowMessage(string message, float duration = 3f)
    {
        if (currentMessage != null)
            StopCoroutine(currentMessage);

        currentMessage = StartCoroutine(ShowMessageRoutine(message, duration));
    }

    private IEnumerator ShowMessageRoutine(string message, float duration)
    {
        wheelMessage.gameObject.SetActive(true);
        wheelMessage.text = message;

        yield return new WaitForSeconds(duration);

        wheelMessage.text = "";
        wheelMessage.gameObject.SetActive(false);

        currentMessage = null;
    }

}

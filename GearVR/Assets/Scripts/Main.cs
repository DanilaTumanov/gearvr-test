using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Main : MonoBehaviour
{

    private GameObject _controllerGO;
        
    public static Main Instance { get; private set; }
    public GameObject Player { get; private set; }



    void Start()
    {
        Instance = this;

        Player = GameObject.FindGameObjectWithTag("Player");

        _controllerGO = new GameObject(name = "Controllers");


        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(_controllerGO);
    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isInputEnabled = true;
    RocketCollisionHandler _rocketCollisionHandler;
    [SerializeField] GameObject _rocket;

    void Start()
    {
        _rocketCollisionHandler = (RocketCollisionHandler) _rocket.GetComponent(typeof(RocketCollisionHandler));
    }

    void Update()
    {
        DebugListener();
    }

    void DebugListener()
    {
        // Load Next Level
        if (Input.GetKeyDown(KeyCode.L))
        {
            _rocketCollisionHandler.StartLevelCompleteSequence();
        }

        // Toggle Collisions
        if (Input.GetKeyDown(KeyCode.C))
        {
            _rocketCollisionHandler.ToggleDamageCollisions();
        }
    }
}

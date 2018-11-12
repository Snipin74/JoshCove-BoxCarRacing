﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour {

    public Rigidbody[] cars;
    public float respawnDelay = 5f;
    public float distanceToCover = 1f;
    private CarController1[] scripts;
    private float[] respawnTimes;
    private float[] distanceLeftToTravel;
    private Transform[] waypoint;


	// Use this for initialization
	void Start () {
        respawnTimes = new float[cars.Length];
        distanceLeftToTravel = new float[cars.Length];
        scripts = new CarController1[cars.Length];
        waypoint = new Transform[cars.Length];

        //initialize the array with starting values 
        for(int i=0; i < respawnTimes.Length; ++i)
        {
            scripts[i] = cars[i].gameObject.GetComponent<CarController1>();
            respawnTimes[i] = respawnDelay;
            distanceLeftToTravel[i] = respawnDelay;
            distanceLeftToTravel[i] = float.MaxValue;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        //check if cars need a respawn
        for(int i = 0; i < cars.Length; i++)
        {
            Transform nextWaypoint = scripts[i].GetCurrentWaypoint();
            float distanceCovered = (nextWaypoint.position - cars[i].position).magnitude;
            //if the car has moved far enough or is moving to a new waypoint reset its values.
            if(distanceLeftToTravel[i] - distanceToCover > distanceCovered || waypoint[i] != nextWaypoint)
            {
                waypoint[i] = nextWaypoint;
                respawnTimes[i] = respawnDelay;
                distanceLeftToTravel[i] = distanceCovered;
            }

            //otherwise tick down time before we respawn it.
            else
            {
                respawnTimes[i] -= Time.deltaTime;
            }
            if(respawnTimes[i] <= 0)
            {
                //reset its respawn tracking variables
                respawnTimes[i] = respawnDelay;
                distanceLeftToTravel[i] = float.MaxValue;
                cars[i].velocity = Vector3.zero;

                //and spawn it at its last waypoint facing the next waypoint.
                Transform lastWaypoint = scripts[i].GetLastWaypoint();
                cars[i].position = lastWaypoint.position;
                cars[i].rotation = Quaternion.LookRotation(nextWaypoint.position - lastWaypoint.position);
            }
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SmokePoint : MonoBehaviour //Class that destroy the gameobject with the particle system after a certain time
{
    float time;

    void Start() //Method that initialize the time variable
    {
        time = 0;
    }
    
    void Update() //Method that destroy the gameobject with the particle system after the time passed
    {
        time += Time.deltaTime;

        if(time >= 1.5)
        {
            Destroy(this.gameObject);
        }
    }
}

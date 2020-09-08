using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour //Class that constantly chooses cubes in the backwall to spin
{
    List<CubeSpin> backCubes; 
    public float coolDown;
    float time;

    void Start() //Method that initialize some variables and then calls GetCubeList
    {
        time = 0;
        backCubes = new List<CubeSpin>();
        GetCubeList();
    }

    void Update() //Method that checks if the cooldown is over to call SpinCube
    {
        time += Time.deltaTime;

        if (time >= coolDown)
        {
            SpinCube();
        }
    }

    void GetCubeList() //Method that finds all objects with the BackCube tag and adds them in the backCubes list
    {
        GameObject[] cubes;

        cubes = GameObject.FindGameObjectsWithTag("BackCube");

        foreach (GameObject cube in cubes)
        {
            backCubes.Add(cube.GetComponent<CubeSpin>());
        }
    }

    void SpinCube() //Method that chooses randomically the next cube to spin and then tell it to spin if it isn't spinning yet
    {
        int cubeIndex = 0;

        time = 0;
        cubeIndex = Random.Range(0, backCubes.Count);

        if (backCubes[cubeIndex].CanSpin())
        {
            backCubes[cubeIndex].StartSpin();
        }
    }
}

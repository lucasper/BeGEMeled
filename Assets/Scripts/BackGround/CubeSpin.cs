using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpin : MonoBehaviour //Class that spin the cubes in the backwall
{
    Quaternion originalPosit;
    Quaternion target;
    bool isSpinning;
    public float speed;
    float time;

    void Start() //Method that initialize the isSpinning variable
    {
        isSpinning = false;
    }
    
    void Update() //Method that calls the method Rotate if the cube is spinning
    {
        if (isSpinning)
        {
            Spin();
        }

    }

    public bool CanSpin() //Method that returns true if the cube is not spinning and false if it is
    {
        return !isSpinning;
    }

    public void StartSpin() //Method that define the spinning direction of the spin and reset the time for the lerp
    {
        if (!isSpinning)
        {
            int direction;

            time = 0;
            originalPosit = this.transform.rotation;
            isSpinning = true;
            direction = Random.Range(0, 4);

            switch (direction)
            {
                case 0:
                    target.eulerAngles = originalPosit.eulerAngles + new Vector3(0, 90, 0);
                    break;
                case 1:
                    target.eulerAngles = originalPosit.eulerAngles + new Vector3(90, 0, 0);
                    break;
                case 2:
                    target.eulerAngles = originalPosit.eulerAngles + new Vector3(0, -90, 0);
                    break;
                case 3:
                    target.eulerAngles = originalPosit.eulerAngles + new Vector3(-90, 0, 0);
                    break;
            }
        }
    }

    void Spin() //Method that actually Spin the cube usin a Lerp to uniformely spin to the desired rotation defined at StartSpin
    {
        time += Time.deltaTime;
        transform.rotation = Quaternion.Lerp(originalPosit, target, time * speed);

        if((transform.rotation.eulerAngles - target.eulerAngles).magnitude  <= 0.1)
        {
            transform.rotation = target;
            isSpinning = false;
        }
    }
}

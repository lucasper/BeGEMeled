using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour //Class that store values and that move the gems in the game
{
    Vector2 target;
    Vector3 original;
    public int[] matrixPosit; //Array that stores the present position of the gem in the matrix of the board
    public int id; //Variable that stores the type of the gem (example: 0 -> yellowGem and so forth)
    float speed;
    float time;
    bool isReady;

    private void Awake() //Method that initialize some variables
    {
        matrixPosit = new int[2];
        isReady = false;
        time = 0;
    }
    
    void Update() //Method that check every frame if the gem should move and if it is calls the Move method
    {
        if (!isReady)
        {
            Move();
        }   
    }

    public void StartMove(Vector2 targetPos, float moveSpeed) //Method that defines the necessarie values for the lerp movement of the gems
    {                                                         //it recieves as parameters targetPos, which contains the targeted position for the lerp movement,
        original = this.transform.position;                   //and the float moveSpeed, which contains the speed of the lerp
        time = 0;
        target = targetPos;
        speed = moveSpeed;
        isReady = false;
    }

    public bool IsReady() //Method that returns if the gem is ready for the next move
    {
        return isReady;
    }

    public void Crush() //Method that destroys the gem
    {
        Destroy(this.gameObject);
    }

    private void Move() //Method that move the gem by lerping it using deltaTime to maintain the same speed overtime and teleport to it's destiny if it gets close enough
    {
        Vector3 moveTo;

        time += Time.deltaTime;
        moveTo = Vector3.Lerp(original, new Vector3(target.x, target.y, 5), time * speed);

        if((this.transform.position - new Vector3(target.x, target.y, 5)).magnitude <= 0.01)
        {
            this.transform.position = new Vector3(target.x,target.y,5);
            isReady = true;

            return;
        }
        else
        {
            this.transform.position = moveTo;
        }
    }
}

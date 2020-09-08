using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTrigger : MonoBehaviour //Class that credits the coin score
{
    public int score; //variable that stores how much the player will score if the coin falls in this trigger
    public Board board; //variable that stores a reference to tha board of the game
    public GameObject smokePoint; //Variable that stores a reference to the prefab of the smoke particle system

    void Start() //Method that gets the reference to the instance of the board
    {
        board = GameObject.FindObjectOfType<Board>();
    }

    private void OnTriggerEnter2D(Collider2D col) //Method that credits the coin score, destroy the coin and spawn the gameobject with the smoke particle system
    {
        if(col.gameObject.tag == "Coin")
        {
            GameObject smoke = Instantiate(smokePoint, col.transform.position, Quaternion.identity);

            Destroy(col.gameObject);
            board.AddPoints(score);
        }
    }
}

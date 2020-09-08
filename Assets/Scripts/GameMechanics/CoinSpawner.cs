using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour //Class that spawns the coin that grants extra points and that moves the spawner so that the coin will constantly change its spawnpoint
{
    int crushed; //Variable that acumulates the number of gems destroyed
    float time;
    Vector2 target;
    Vector3 original;
    public GameObject coin;

    void Start() //Method that initialize some variables
    {
        crushed = 0;
        time = 0;
        target = new Vector2(3,7);
        original = this.transform.position;
    }
    
    void Update() //Method that check every frame if a coin should be spawned, a single coin spawns at every 3 gems that were destroyed
    {             //This Method also calls the Move method so that every frame the spanwer has a new position
        if (crushed > 2)
        {
            crushed -= 3;
            SpawnCoin();
        }
        Move();
    }
    private void Move() //Method that moves the coinSpawner by lerping back and forth between 2 points
    {
        Vector3 moveTo;

        time += Time.deltaTime;
        moveTo = Vector3.Lerp(original, new Vector3(target.x, target.y, 5), time * 1);

        if ((this.transform.position - new Vector3(target.x, target.y, 5)).magnitude <= 0.01)
        {
            this.transform.position = new Vector3(target.x, target.y, 5);
            original = this.transform.position;
            time = 0;

            if(target.x > 0)
            {
                target.x = -3;
            }
            else
            {
                target.x = 3;
            }

            return;
        }
        else
        {
            this.transform.position = moveTo;
        }
    }

    public void AddCrushed() //Method that adds one to the variable that keeps track of the destroyed gems
    {
        crushed++;
    }

    void SpawnCoin() //Method that spawns a coin in the present position of the coinSpawner
    {
        Instantiate(coin, new Vector3(transform.position.x, transform.position.y, 4f), this.transform.rotation);
    }
}

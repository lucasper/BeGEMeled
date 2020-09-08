using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour //Class that controls the board of gems
{
    public GameObject[] gems; //Array containing references to the prefabs of the gems
    public CoinSpawner coinSpawner;
    public GameObject smokePoint;
    public GameObject gameOver; //Variable that contains a reference to the game over screen
    public Text score; //Variable that contains a reference to the UI Text that represents the score of the player
    public bool aiPlay;
    int points;
    Gem[,] matrix; //Array that contains the matrix of gems of the board
    Collider gemTarget; //Variable that contains the Collider of the gem the player is touching
    Vector2 touchOrigin; //Variable that contains the position of the touch when the player began touching the screen
    bool touchHolding;
    float coolDown;
    float time;
    
    void Start() //Method that initializes some variables and calls the SpawnMatrix method
    {
        coolDown = 1;
        coinSpawner = GameObject.FindObjectOfType<CoinSpawner>();
        points = 0;
        time = 0;
        SpawnMatrix();
    }
    
    void Update() //Method that checks and updates the board and the score frame by frame, calls the CheckDefeat method with a cooldown to check the defeat condition and calls CheckTouch if the player is playing
    {            //Also calls the AutoPlay method when the AI is playing the game instead of the player
        score.text = "Score:" + points.ToString();

        if (CheckBoard())
        {
            time += Time.deltaTime;
            CheckCrush();

            if (time >= coolDown)
            {
                time = 0;

                if (CheckDefeat())
                {
                    GameOver();
                }
                if (aiPlay)
                {
                    AutoPlay();
                }
            }
            if(!aiPlay)
            {
                CheckTouch();
            }
        }
    }

    void GameOver() //Method that activates the game over
    {
        gameOver.SetActive(true);
    }

    void AutoPlay() //Method that implements a simple AI to play the game instead of the player
    {               //It goes through the matrix checking if there is a possible move and if there is it calls the method Swap to execute the move
        for (int ite1 = 0; ite1 < 7; ite1++)
        {
            for (int ite2 = 0; ite2 < 7; ite2++)
            {
                if (ite1 < 6)
                {
                    if (CheckSwap(matrix[ite1, ite2], 1) || CheckSwap(matrix[ite1 + 1, ite2], 3))
                    {
                        Swap(matrix[ite1, ite2], matrix[ite1 + 1, ite2]);

                        return;
                    }
                }
                if (ite2 < 6)
                {
                    if (CheckSwap(matrix[ite1, ite2], 0) || CheckSwap(matrix[ite1, ite2 + 1], 2))
                    {
                        Swap(matrix[ite1, ite2], matrix[ite1, ite2 + 1]);

                        return;
                    }
                }
            }
        }
    }

    bool CheckDefeat() //Method that checks for the defeat condition and return true if the game is over
    {                  //It works similarily to the AutoPlay method goin through the matrix to check if there is a possible move
        for (int ite1 = 0; ite1 < 7; ite1++)
        {
            for (int ite2 = 0; ite2 < 7; ite2++)
            {
                if(ite1 < 6)
                {
                    if (CheckSwap(matrix[ite1, ite2], 1) || CheckSwap(matrix[ite1 + 1, ite2], 3))
                    {
                        return false;
                    }
                }
                if(ite2 < 6)
                {
                    if (CheckSwap(matrix[ite1, ite2], 0) || CheckSwap(matrix[ite1, ite2 + 1], 2))
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    void CheckTouch() //Method that checks and executes the touch commands inputed by the player
    {                 //It checks if the player is touching the screen, then it cast a ray and check if it hits any of the gems
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began)
            {
                Ray rayCast = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(rayCast, out hit))
                {
                    if (hit.collider != null)
                    {
                        if (hit.collider.gameObject.tag == "Gem")
                        {
                            touchHolding = true;
                            gemTarget = hit.collider;
                            touchOrigin = touch.position;
                        }
                    }
                }
            }
            if (touch.phase == TouchPhase.Ended)
            {
                touchHolding = false;
            }
            if (touch.phase == TouchPhase.Moved && touchHolding) //If a gem has been hit when the player first touched the screen then it will check if the player is "dragging" the gem and to which direction
            {
                if( touchOrigin.x - touch.position.x > 4) //Left
                {
                    Gem gem = gemTarget.gameObject.GetComponent<Gem>();

                    if (matrix[gem.matrixPosit[0] - 1, gem.matrixPosit[1]] != null)
                    {
                        if (CheckSwap(gem, 3) || CheckSwap(matrix[gem.matrixPosit[0] - 1, gem.matrixPosit[1]], 1)) //If the move is possible then it will then execute the move
                        {
                            Swap(gem, matrix[gem.matrixPosit[0] - 1, gem.matrixPosit[1]]);
                        }
                    }

                    touchHolding = false;
                }
                else
                {
                    if (touchOrigin.x - touch.position.x < -4) //Right
                    {
                        Gem gem = gemTarget.gameObject.GetComponent<Gem>();

                        if (matrix[gem.matrixPosit[0] + 1, gem.matrixPosit[1]] != null)
                        {
                            if (CheckSwap(gem, 1) || CheckSwap(matrix[gem.matrixPosit[0] + 1, gem.matrixPosit[1]], 3))
                            {
                                Swap(gem, matrix[gem.matrixPosit[0] + 1, gem.matrixPosit[1]]);
                            }
                        }

                        touchHolding = false;
                    }
                    else
                    {
                        if (touchOrigin.y - touch.position.y > 4) //Down
                        {
                            Gem gem = gemTarget.gameObject.GetComponent<Gem>();

                            if(matrix[gem.matrixPosit[0], gem.matrixPosit[1] - 1] != null)
                            {
                                if (CheckSwap(gem, 2) || CheckSwap(matrix[gem.matrixPosit[0], gem.matrixPosit[1] - 1], 0))
                                {
                                    Swap(gem, matrix[gem.matrixPosit[0], gem.matrixPosit[1] - 1]);
                                }
                            }

                            touchHolding = false;
                        }
                        else
                        {
                            if (touchOrigin.y - touch.position.y < -4) //Up
                            {
                                Gem gem = gemTarget.gameObject.GetComponent<Gem>();

                                if (matrix[gem.matrixPosit[0], gem.matrixPosit[1] + 1] != null)
                                {
                                    if (CheckSwap(gem, 0) || CheckSwap(matrix[gem.matrixPosit[0], gem.matrixPosit[1] + 1], 2))
                                    {
                                        Swap(gem, matrix[gem.matrixPosit[0], gem.matrixPosit[1] + 1]);
                                    }
                                }

                                touchHolding = false;
                            }
                        }
                    }
                }
            }
        }
    }

    void SpawnMatrix() //Method that spawns the matrix of gems
    {
        matrix = new Gem[7,7];

        for(int ite1 = 0;ite1 < 7 ;ite1 ++ )
        {
            for (int ite2 = 0; ite2 < 7; ite2++)
            {
                ChooseGem(ite1, ite2,true);
            }
        }
    }

    void ChooseGem(int nodeX, int nodeY, bool isFirst) //Method that spawns each gem separatedly to prevent automatic matches in the spawn
    {                                                  //It creates a list of permitted types of gems and then checks which ones would create instant matches in that position and then removes them from the list
        List<int> permitted = new List<int>() { 0, 1, 2, 3, 4, 5, 6 };

        if (nodeX > 1)
        {
            if (matrix[nodeX - 1, nodeY].id == matrix[nodeX - 2, nodeY].id)
            {
                permitted.Remove(matrix[nodeX - 1, nodeY].id);
            }
        }
        if (nodeY > 1)
        {
            if (matrix[nodeX, nodeY - 1].id == matrix[nodeX, nodeY - 2].id)
            {
                permitted.Remove(matrix[nodeX, nodeY - 1].id);
            }
        }
        if (!isFirst)
        {
            if (nodeX < 5 && matrix[nodeX + 1, nodeY].IsReady() && matrix[nodeX + 2, nodeY].IsReady())
            {
                if (matrix[nodeX + 1, nodeY].id == matrix[nodeX + 2, nodeY].id)
                {
                    permitted.Remove(matrix[nodeX + 1, nodeY].id);
                }
            }
            if (nodeX < 6 && nodeX > 0 && matrix[nodeX + 1, nodeY].IsReady() && matrix[nodeX - 1, nodeY].IsReady())
            {
                if (matrix[nodeX + 1, nodeY].id == matrix[nodeX - 1, nodeY].id)
                {
                    permitted.Remove(matrix[nodeX - 1, nodeY].id);
                }
            }
        }

        int gemId = permitted[Random.Range(0, permitted.Count)]; //And then it chooses randomically between the permitted gem types

        SpawnGem(nodeX, nodeY, gemId);

        return;
    }

    void SpawnGem(int nodeX, int nodeY, int id) //Method that instanciate the prefab of a gem type "id" and insert it into the gem matrix into the position defined by the parameters of nodeX and nodeY
    {
        Gem gem = Instantiate(gems[id], new Vector3(Random.Range(0, 21) - 10, 10, 5), Quaternion.Euler(-90,0,0)).GetComponent<Gem>(); //it spawns the gem in a position above the vision of the camera

        matrix[nodeX, nodeY] = gem;
        gem.StartMove(new Vector2(nodeX - 3, nodeY - 7), 0.5f); //And then sends it lerping to its position in the board
        gem.matrixPosit[0] = nodeX;
        gem.matrixPosit[1] = nodeY;
    }

    void Swap(Gem gem1, Gem gem2) //Mehod that swap 2 gems by using auxiliar variables to switch the value of some variables
    {
        Gem auxGem;
        int aux;

        gem1.StartMove(gem2.transform.position, 3f); //And making them lerp to their new places in the board
        gem2.StartMove(gem1.transform.position, 3f);

        auxGem = matrix[gem1.matrixPosit[0], gem1.matrixPosit[1]];
        matrix[gem1.matrixPosit[0], gem1.matrixPosit[1]] = matrix[gem2.matrixPosit[0], gem2.matrixPosit[1]];
        matrix[gem2.matrixPosit[0], gem2.matrixPosit[1]] = auxGem;

        aux = gem1.matrixPosit[0];
        gem1.matrixPosit[0] = gem2.matrixPosit[0];
        gem2.matrixPosit[0] = aux;

        aux = gem1.matrixPosit[1];
        gem1.matrixPosit[1] = gem2.matrixPosit[1];
        gem2.matrixPosit[1] = aux;
    }

    public void RequestGem(int nodeX, int nodeY) //Method that calls a gem to a gemless spot in the board
    {
        if(nodeY == 6) //If the spot is in the top row it spawns a new gem
        {
            ChooseGem(nodeX, nodeY, false);
        }
        else //If the spot is not in the top row then it will first check if there is any gem above the spot to fill the spot
        {
            for(int ite = nodeY + 1;ite < 7; ite++)
            {
                if(matrix[nodeX,ite] != null)
                {
                    matrix[nodeX, nodeY] = matrix[nodeX, ite];
                    matrix[nodeX, ite].StartMove(new Vector2(nodeX - 3, nodeY - 7), 2f);
                    matrix[nodeX, ite] = null;
                    matrix[nodeX, nodeY].matrixPosit[0] = nodeX;
                    matrix[nodeX, nodeY].matrixPosit[1] = nodeY;

                    return;
                }
            }

            ChooseGem(nodeX, nodeY, false); //If there isn't any gem above the spot then it spawns a new gem
        }
    }

    bool CheckSwap(Gem gem, int dir) //Method that recieve a gem and a direction and check if it's possible to swap this gem to the gem beside it in the direction
    {                                //It checks each possibility to see if the swap is possible and then return a boolean indicating if it is
        int x = gem.matrixPosit[0];
        int y = gem.matrixPosit[1];

        if (dir == 0 && y + 1 < 7) 
        {
            if (y + 3 < 7)          
            {
                if (matrix[x, y + 2].id == matrix[x, y + 3].id && matrix[x, y + 2].id == gem.id)
                {
                    return true;
                }
            }
            if (x + 2 < 7)
            {
                if (matrix[x + 1, y + 1].id == matrix[x + 2, y + 1].id && matrix[x + 1, y + 1].id == gem.id)
                {
                    return true;
                }
            }
            if (x - 2 >= 0)
            {
                if (matrix[x - 1, y + 1].id == matrix[x - 2, y + 1].id && matrix[x - 1, y + 1].id == gem.id)
                {
                    return true;
                }
            }
            if (x + 1 < 7 && x - 1 >= 0)
            {
                if (matrix[x + 1, y + 1].id == matrix[x - 1, y + 1].id && matrix[x + 1, y + 1].id == gem.id)
                {
                    return true;
                }
            }
        }
        if (dir == 1 && x + 1 < 7)
        {
            if (x + 3 < 7)
            {
                if (matrix[x + 2, y].id == matrix[x + 3, y].id && matrix[x + 2, y].id == gem.id)
                {
                    return true;
                }
            }
            if (y + 2 < 7)
            {
                if (matrix[x + 1, y + 1].id == matrix[x + 1, y + 2].id && matrix[x + 1, y + 1].id == gem.id)
                {
                    return true;
                }
            }
            if (y - 2 >= 0)
            {
                if (matrix[x + 1, y - 1].id == matrix[x + 1, y - 2].id && matrix[x + 1, y - 1].id == gem.id)
                {
                    return true;
                }
            }
            if (y + 1 < 7 && y - 1 >= 0)
            {
                if (matrix[x + 1, y + 1].id == matrix[x + 1, y - 1].id && matrix[x + 1, y + 1].id == gem.id)
                {
                    return true;
                }
            }
        }
        if (dir == 2)
        {
            if (y - 3 >= 0)
            {
                if (matrix[x, y - 2].id == matrix[x, y - 3].id && matrix[x, y - 2].id == gem.id)
                {
                    return true;
                }
            }
            if (x - 2 >= 0)
            {
                if (matrix[x - 1, y - 1].id == matrix[x - 2, y - 1].id && matrix[x - 1, y - 1].id == gem.id)
                {
                    return true;
                }
            }
            if (x + 2 < 7)
            {
                if (matrix[x + 1, y - 1].id == matrix[x + 2, y - 1].id && matrix[x + 1, y - 1].id == gem.id)
                {
                    return true;
                }
            }
            if (x + 1 < 7 && x - 1 >= 0)
            {
                if (matrix[x + 1, y - 1].id == matrix[x - 1, y - 1].id && matrix[x + 1, y - 1].id == gem.id)
                {
                    return true;
                }
            }
        }
        if (dir == 3)
        {
            if (x - 3 >= 0)
            {
                if (matrix[x - 2, y].id == matrix[x - 3, y].id && matrix[x - 2, y].id == gem.id)
                {
                    return true;
                }
            }
            if (y - 2 >= 0)
            {
                if (matrix[x - 1, y - 1].id == matrix[x - 1, y - 2].id && matrix[x - 1, y - 1].id == gem.id)
                {
                    return true;
                }
            }
            if (y + 2 < 7)
            {
                if (matrix[x - 1, y + 1].id == matrix[x - 1, y + 2].id && matrix[x - 1, y + 1].id == gem.id)
                {
                    return true;
                }
            }
            if (y + 1 < 7 && y - 1 >= 0)
            {
                if (matrix[x - 1, y + 1].id == matrix[x - 1, y - 1].id && matrix[x - 1, y + 1].id == gem.id)
                {
                    return true;
                }
            }
        }

        return false;
    }

    bool CheckBoard() //Method that checks if the board is ready to another move
    {                 //if goes through the matrix to check if there is any gemless spot or if there is any gem still moving
        bool isValid = true;

        for (int ite1 = 0; ite1 < 7; ite1++)
        {
            for (int ite2 = 0; ite2 < 7; ite2++)
            {
                if (matrix[ite1, ite2] != null)
                {
                    if (!matrix[ite1, ite2].IsReady())
                    {
                        isValid = false;
                    }
                }
                else
                {
                    RequestGem(ite1, ite2);
                    isValid = false;
                }
            }
        }

        return isValid;
    }

    void CheckCrush() //Method that checks if there is any match to be made
    {                 //it goes through the rows and columns of the matrix to check if there is any match
        int repeated = 0;
        int id = 0;
        List<Gem> crushedGems = new List<Gem>(); //If there is a match all gems of the match are added to the crushedGems list

        for (int ite1 = 0; ite1 < 7; ite1++)
        {
            repeated = 0;

            for (int ite2 = 0; ite2 < 7; ite2++)
            {
                if (ite2 > 0)
                {
                    if(id == matrix[ite1, ite2].id)
                    {
                        repeated++;

                        if(ite2 == 6 && repeated > 1)
                        {
                            while (repeated >= 0)
                            {
                                if (!crushedGems.Contains(matrix[ite1, ite2 - repeated]))
                                {
                                    crushedGems.Add(matrix[ite1, ite2 - repeated]);
                                }

                                repeated--;
                            }
                        }
                    }
                    else
                    {
                        id = matrix[ite1, ite2].id;

                        if (repeated > 1)
                        {
                            while (repeated >= 0)
                            {
                                if (!crushedGems.Contains(matrix[ite1, ite2 - repeated - 1]))
                                {
                                    crushedGems.Add(matrix[ite1, ite2 - repeated - 1]);
                                }

                                repeated--;
                            }
                        }

                        repeated = 0;
                    }
                }
                else
                {
                    repeated = 0;
                    id = matrix[ite1, ite2].id;
                }
            }
        }
        for (int ite2 = 0; ite2 < 7; ite2++)
        {
            repeated = 0;

            for (int ite1 = 0; ite1 < 7; ite1++)
            {
                if (ite1 > 0)
                {
                    if (id == matrix[ite1, ite2].id)
                    {
                        repeated++;

                        if (ite1 == 6 && repeated > 1)
                        {
                            while (repeated >= 0)
                            {
                                if (!crushedGems.Contains(matrix[ite1 - repeated, ite2]))
                                {
                                    crushedGems.Add(matrix[ite1 - repeated, ite2]);
                                }

                                repeated--;
                            }
                        }
                    }
                    else
                    {
                        id = matrix[ite1, ite2].id;

                        if (repeated > 1)
                        {
                            while (repeated >= 0)
                            {
                                if (!crushedGems.Contains(matrix[ite1 - repeated - 1, ite2]))
                                {
                                    crushedGems.Add(matrix[ite1 - repeated - 1, ite2]);
                                }

                                repeated--;
                            }
                        }

                        repeated = 0;
                    }
                }
                else
                {
                    repeated = 0;
                    id = matrix[ite1, ite2].id;
                }
            }
        }
        foreach(Gem gem in crushedGems) //It destroy all the gems in the matchs and add 3 points to the score for each gem destroyed
        {
            GameObject smoke = Instantiate(smokePoint, gem.transform.position, Quaternion.identity);

            gem.Crush();
            coinSpawner.AddCrushed();
            AddPoints(3);
        }
    }

    public void AddPoints(int point) //Method that adds a certain amount of points to the score
    {
        points += point;
    }
}

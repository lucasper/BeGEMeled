using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasButtons : MonoBehaviour //Class that implements the commands that the buttons on the canvas execute
{
    public void Restart() //Method that restart the scene
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu() //Method that change the present scene to the MainMenu scene
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Play() //Method that change the present scene to the Ingame scene
    {
        SceneManager.LoadScene("Ingame");
    }
    public void PlayAI() //Method that change the present scene to the IngameAI scene
    {
        SceneManager.LoadScene("IngameAI");
    }
    public void QuitApp() //Method that quits the application
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public string pilot;
    public string diff;


    public void newGame()
    {
        SceneManager.LoadScene("NewGameOptions");
    }

    public void startGame()
    {
        GameObject charGroup = GameObject.Find("/Canvas/CharSelect");
        Toggle character = charGroup.GetComponent<ToggleGroup>().ActiveToggles().FirstOrDefault();
        GameObject diffGroup = GameObject.Find("/Canvas/Difficulty");
        Toggle difficulity = diffGroup.GetComponent<ToggleGroup>().ActiveToggles().FirstOrDefault();
        PlayerPrefs.SetString("pilot", character.name);
        PlayerPrefs.SetString("diff", difficulity.name);
        SceneManager.LoadScene("MainGame");
    }

    public void backOptions()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void appQuit()
    {
        Application.Quit(); 
    }
}

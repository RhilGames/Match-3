using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectMenu : MonoBehaviour
{
    public string mainMenu = "Main Menu";


    // Start is called before the first frame update
   public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }
}

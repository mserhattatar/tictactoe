using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;
    public GameObject multiplayerPanel;

    private void Awake()
    {
        instance = this;
    }

    public void OnePlayer()
    {
        GameManager.instance.twoPlayer = false;
        SceneManager.LoadScene(1);
    }

    public void TwoPlayer()
    {
        GameManager.instance.twoPlayer = true;
        SceneManager.LoadScene(2);
    }
}
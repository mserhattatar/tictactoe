using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool twoPlayer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this);

        twoPlayer = false;
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        var y = SceneManager.GetActiveScene().buildIndex;
        if (y == 1 || y == 2)
            SceneManager.LoadScene(0);
        else
            Application.Quit();
    }
}
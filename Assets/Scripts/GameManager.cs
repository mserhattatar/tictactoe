using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool twoPlayer;
    //public int aiChallenge;

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
        //aiChallenge = 3;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            int y = SceneManager.GetActiveScene().buildIndex;
            if (y==1 || y== 2)
                SceneManager.LoadScene(0);
            else
                Application.Quit();
        }            
    }
}

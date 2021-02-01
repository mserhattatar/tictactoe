using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool twoPlayer;
    public int aiChallenge;

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
        aiChallenge = 3;

    }
}

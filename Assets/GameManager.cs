using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool ikiKisilik;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this);

        ikiKisilik = false;
    }
}

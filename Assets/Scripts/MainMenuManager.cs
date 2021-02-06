using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;
    public GameObject multiplayerPanel;
    //public int _aiChallenge;

    //private void Start()
    //{
    //    _aiChallenge = 3;
    //}

    private void Awake()
    {
        instance = this;
    }      
    public void TekKisilik()
    {
        //GameManager.instance.aiChallenge = _aiChallenge;
        GameManager.instance.twoPlayer = false;
        SceneManager.LoadScene(1);
    }
    public void IkiKisilik()
    {
        GameManager.instance.twoPlayer = true;
        SceneManager.LoadScene(2);
    }
   
}

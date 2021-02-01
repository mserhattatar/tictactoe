using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
   public void TekKisilik()
    {
        GameManager.instance.ikiKisilik = false;
        SceneManager.LoadScene(1);
    }
    public void IkiKisilik()
    {
        GameManager.instance.ikiKisilik = true;
        SceneManager.LoadScene(2);
    }
}

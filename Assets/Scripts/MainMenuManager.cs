using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public int _aiChallenge;
    public Dropdown langDropdown;

    private void Start()
    {
        _aiChallenge = 3;
    }
    public void TekKisilik()
    {
        GameManager.instance.aiChallenge = _aiChallenge;
        GameManager.instance.twoPlayer = false;
        SceneManager.LoadScene(1);
    }
    public void IkiKisilik()
    {
        GameManager.instance.twoPlayer = true;
        SceneManager.LoadScene(2);
    }
    public void DropDownAiChallenge()
    {
        var _LangDropdown = langDropdown.GetComponent<Dropdown>();
        _aiChallenge = 3 +_LangDropdown.value;
        Debug.Log(_aiChallenge);

    }
}

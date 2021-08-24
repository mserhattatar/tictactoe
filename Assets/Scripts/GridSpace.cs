using UnityEngine;
using UnityEngine.UI;


public class GridSpace : MonoBehaviour
{
    public Button button;
    public Text buttonText;

    private GameController _gameController;

    public void SetSpace()
    {
        if (_gameController.playerMove == true)
        {
            buttonText.text = _gameController.GetPlayerSide();
            button.interactable = false;
            _gameController.EndTurn();
        }
        else if (GameManager.instance.twoPlayer)
        {
            buttonText.text = _gameController.GetComputerSide();
            button.interactable = false;
            _gameController.EndTurn();
        }
    }

    public void SetSpaceMultiPlayer()
    {
        if (_gameController.playerMove == true)
        {
            buttonText.text = _gameController.GetPlayerSide();
            button.interactable = false;
            _gameController.EndTurn();
        }
        else if (GameManager.instance.twoPlayer)
        {
            buttonText.text = _gameController.GetComputerSide();
            button.interactable = false;
            _gameController.EndTurn();
        }
    }

    public void SetGameControllerReference(GameController controller)
    {
        _gameController = controller;
    }
}
using UnityEngine;
using UnityEngine.UI;


public class GridSpace : MonoBehaviour
{
    public Button button;
    public Text buttonText;

    private GameController gameController;

    public void SetSpace()
    {
        if(gameController.playerMove == true)
        {
            buttonText.text = gameController.GetPlayerSide();
            button.interactable = false;
            gameController.EndTrun();
        } 
        else if(GameManager.instance.twoPlayer)
        {
            buttonText.text = gameController.GetComputerSide();
            button.interactable = false;
            gameController.EndTrun();
        }
    }
    public void SetGameControllerReference(GameController controller)
    {
        gameController = controller;
    }

}

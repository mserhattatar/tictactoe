using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Player
{
    public Image panel;
    public Text text;
    public Button button;
}

[System.Serializable]
public class PlayerColor
{
    public Color panelColor;
    public Color textColor;
}

public class GameController : MonoBehaviour
{

    private int moveCount;

    public Text[] buttonList;
    public GameObject gameOverPanel;
    public Text gameOverPanelText;  
    public GameObject restartGameButton;
    public Player playerX;
    public Player playerO;
    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;
    public GameObject startInfo;

    private string playerSide;
    private string computerSide;
    private int value;
    public bool playerMove;
    public float delay;


    private void Awake()
    {        
        SetGameControllerReferenceOnButtons();
        gameOverPanel.SetActive(false);
        moveCount = 0;
        restartGameButton.SetActive(false);
        SetPlayerButtons(true);
        playerMove = true;
        delay = 10;
    }
    private void Update()
    {
        if(playerMove == false)
        {
            delay += delay * Time.deltaTime;
            if (delay >= 20)
            {
                // value = Random.Range(0, 8);
                value = FindBestMove();
                if(buttonList[value].GetComponentInParent<Button>().interactable == true)
                {
                    buttonList[value].text = GetComputerSide();
                    buttonList[value].GetComponentInParent<Button>().interactable = false;
                    EndTrun();
                }
            }
        }
    }

    void SetGameControllerReferenceOnButtons()
    {
        for(int i=0; i<buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    public void SetStartingSide(string startingSide)
    {
        playerSide = startingSide;
        if(playerSide == "X")
        {
            computerSide = "O";
            SetPlayerColors(playerX, playerO);
        }
        else
        {
            computerSide = "X";
            SetPlayerColors(playerO, playerX);
        }
        StartGame();
    }

    void StartGame()
    {
        SetBoardInteractable(true);
        SetPlayerButtons(false);
        startInfo.SetActive(false);
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }
    public string GetComputerSide()
    {
        return computerSide;
    }

    public string FindWinner()
    {
        //check horizontal equality
        for (var i = 0; i < 7; i = i + 3)
        {
            if (buttonList[i].text != "" && buttonList[i].text == buttonList[i + 1].text && buttonList[i + 1].text == buttonList[i + 2].text)
            {
                return buttonList[i].text;
            }
        }
        //check vertical equality
        for (var i = 0; i < 3; i++)
        {
            if (buttonList[i].text != "" && buttonList[i].text == buttonList[i + 3].text && buttonList[i + 3].text == buttonList[i + 6].text)
            {
                return buttonList[i].text;
            }
        }       
        //check cross equality 
        if (buttonList[0].text != "" && buttonList[0].text == buttonList[4].text && buttonList[4].text == buttonList[8].text)
        {
            return buttonList[4].text;
        }
        if (buttonList[2].text != "" && buttonList[2].text == buttonList[4].text && buttonList[4].text == buttonList[6].text)
        {
            return buttonList[4].text;           
        }
        return null;
    }

    public void EndTrun()
    {
        moveCount++;
        var _winner = FindWinner();

        if ((_winner == null && moveCount >= 9) || _winner != null)
            GameEnd(_winner);       
        else
        {
            ChangeSides();
            delay = 10;
        }       
    }

    void SetPlayerColors(Player newPlayer, Player oldPlayer)
    {
        newPlayer.panel.color = activePlayerColor.panelColor;
        newPlayer.text.color = activePlayerColor.textColor;
        oldPlayer.panel.color = inactivePlayerColor.panelColor;
        oldPlayer.text.color = inactivePlayerColor.textColor;
    }

    void GameEnd(string winningPlayer)
    {
        SetBoardInteractable(false);

        if (winningPlayer == null)
        {
            SetGameOverText("No Winner!");
            SetPlayerColorsInactive();
        }
        else
            SetGameOverText(winningPlayer + " Wins!");

        restartGameButton.SetActive(true);
    }

    void ChangeSides()
    {
        //playerSide = (playerSide == "X") ? "O" : "X";
        playerMove = (playerMove == true) ? false : true;
        //if (playerSide == "X")
        if (playerMove == true)
        {
            SetPlayerColors(playerX, playerO);
        }
        else
        {
            SetPlayerColors(playerO, playerX);
        }
    }

    void SetGameOverText(string value)
    {
        gameOverPanel.SetActive(true);
        gameOverPanelText.text = value;
    }

    public void RestartGame()
    {
        moveCount = 0;
        gameOverPanel.SetActive(false);
        restartGameButton.SetActive(false);
        SetPlayerButtons(true);
        SetPlayerColorsInactive();
        startInfo.SetActive(true);
        playerMove = true;
        delay = 10;

        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].text = "";
        }        
    }
    public void SetBoardInteractable(bool toggle)
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }
    void SetPlayerButtons(bool toggle)
    {
        playerX.button.interactable = toggle;
        playerO.button.interactable = toggle;
    }
    void SetPlayerColorsInactive()
    {
        playerX.panel.color = inactivePlayerColor.panelColor;
        playerX.text.color = inactivePlayerColor.textColor;
        playerO.panel.color = inactivePlayerColor.panelColor;
        playerO.text.color = inactivePlayerColor.textColor;
    }

    private bool FindEmptyText(Text[] _buttonList)
    {
        for (var i = 0; i < _buttonList.Length; i++)
        {
            if (_buttonList[i].text == "")
            {
                return true;
            }
        }
        return false;
    }

    int minimax(Text[] _buttonList,int depth, bool isMax)
    {
        string _winner = FindWinner();
        if (depth == 3)
            return 0;
        // If Maximizer has won the game return his/her
        // evaluated score
        if (_winner != null && computerSide == _winner)
            return 10;

        // If Minimizer has won the game return his/her
        // evaluated score
        if (_winner != null && playerSide == _winner)
            return -10;
        // If there are no more moves and no winner then
        // it is a tie
        if (!FindEmptyText(_buttonList))
            return 0;

        Debug.Log("emptytext = s" + FindEmptyText(_buttonList));
        // If this maximizer's move
        if (isMax)
        {
            int best = -1000;
            // Traverse all cells
            for (int i = 0; i < _buttonList.Length; i++)
            {   
                // Check if cell is empty
                if (_buttonList[i].text == "")
                {
                    Debug.Log("isMax!!! = ");

                    // Make the move
                    _buttonList[i].text = computerSide;

                    // Call minimax recursively and choose
                    // the maximum value
                    best = System.Math.Max(best, minimax(_buttonList, depth +1, !isMax));
                    Debug.Log("isMax best = " + best);

                    // Undo the move
                    _buttonList[i].text = "";
                }
                
            }
            return best;
        }

        // If this minimizer's move
        else
        {
            int best = 1000;
            for (int i = 0; i < _buttonList.Length; i++)
            {
                // Check if cell is empty
                if (_buttonList[i].text == "")
                {
                    // Make the move
                    _buttonList[i].text = playerSide;

                    // Call minimax recursively and choose
                    // the maximum value
                    best = System.Math.Min(best, minimax(_buttonList,depth +1, !isMax));
                    Debug.Log("isMin best = " + best);


                    // Undo the move
                    _buttonList[i].text = "";
                }
            }          
            return best;
        }
    }

    // This will return the best possible move for the player
    public int FindBestMove()
    {
        int bestVal = -1000;
        int bestMove = -1;


        // Traverse all cells, evaluate minimax function for
        // all empty cells. And return the cell with optimal
        // value.

        for (int i = 0; i < buttonList.Length; i++)
        {
           
            // Check if cell is empty
            if (buttonList[i].text == "")
            {
                Debug.Log(buttonList[i].text);
                // Make the move
                buttonList[i].text = computerSide;

                // Call minimax recursively and choose
                // the maximum value
                int moveVal = minimax(buttonList, 0, true);

                // Undo the move
                buttonList[i].text = "";

                // If the value of the current move is
                // more than the best value, then update
                // best/
               
                if (moveVal > bestVal)
                {
                    bestVal = moveVal;
                    bestMove = i;
                }

            }
        }
        Debug.Log("The value of the best Move is =  " + bestMove.ToString() + "// best val" + bestVal.ToString());

        return bestMove;
    }
}

using System;
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
    public GameObject restartGameButton2;
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
    public bool delayBool;
    public bool ikiKisi;


    private void Awake()
    {        
        SetGameControllerReferenceOnButtons();
        gameOverPanel.SetActive(false);
        moveCount = 0;
        restartGameButton.SetActive(false);
        if (restartGameButton2 != null)
            restartGameButton2.SetActive(false);
        SetPlayerButtons(true);
        playerMove = true;
        delay = 10;
        delayBool = true;
        ikiKisi = GameManager.instance.twoPlayer;
    }
    private void Update()
    {
        if(!ikiKisi && playerMove == false)
        {
            delay += delay * Time.deltaTime;
            if (delay >= 20 && delayBool)
            {
                delayBool = false;
                value = FindBestMove(buttonList);
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
            delayBool = true;
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
        if (restartGameButton2 != null)
            restartGameButton2.SetActive(true);
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
        if (restartGameButton2 != null)
            restartGameButton2.SetActive(false);
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

    int[] minimax(Text[] _buttonList,int depth, bool isMax)
    {
        string _winner = FindWinner();
        int[] _returSearch;
        // If Maximizer has won the game return his/her
        // evaluated score
        if (computerSide == _winner)
        {
            _returSearch = new int[] {10, depth};
            return _returSearch;
        }          

        // If Minimizer has won the game return his/her
        // evaluated score
        if (playerSide == _winner)
        {
            _returSearch = new int[] { -10, depth };
            return _returSearch;
        }
        // If there are no more moves and no winner then
        // it is a tie
        if (!FindEmptyText(_buttonList))
        {
            _returSearch = new int[] { 0, depth };
            return _returSearch;
        }

        //if (depth == 2)
        //{
        //    _returSearch = new int[] { 0, depth };
        //    return _returSearch;
        //}

        // If this maximizer's move
        if (isMax)
        {
            int best = -1000;
            int[] _returnBest= new int[] {best, depth};
            // Traverse all cells
            for (int i = 0; i < _buttonList.Length; i++)
            {   
                // Check if cell is empty
                if (_buttonList[i].text == "")
                {

                    // Make the move
                    _buttonList[i].text = computerSide;

                    // Call minimax recursively and choose
                    // the maximum value
                    _returnBest[0] = Math.Max(_returnBest[0], minimax(_buttonList, depth +1, !isMax)[0]);
                    _returnBest[1] = minimax(_buttonList, depth + 1, !isMax)[1];
                    // Undo the move
                    _buttonList[i].text = "";
                }                
            }
            return _returnBest;
        }

        // If this minimizer's move
        else
        {
            int best = 1000;
            int[] _returnBest = new int[] { best, depth };
            for (int i = 0; i < _buttonList.Length; i++)
            {
                // Check if cell is empty
                if (_buttonList[i].text == "")
                {
                    // Make the move
                    _buttonList[i].text = playerSide;

                    // Call minimax recursively and choose
                    // the maximum value
                    _returnBest[0] = Math.Min(_returnBest[0], minimax(_buttonList, depth + 1, !isMax)[0]);
                    _returnBest[1] = minimax(_buttonList, depth + 1, !isMax)[1];


                    // Undo the move
                    _buttonList[i].text = "";
                }
            }          
            return _returnBest;
        }
    }

    // This will return the best possible move for the player
    public int FindBestMove(Text[] _buttonlist)
    {
        //int bestVal = -1000;
        int[] bestVal = new int[] { -1000, +1000 };
        int bestMove = -1;

        // Traverse all cells, evaluate minimax function for
        // all empty cells. And return the cell with optimal
        // value.

        for (int i = 0; i < _buttonlist.Length; i++)
        {
           
            // Check if cell is empty
            if (_buttonlist[i].text == "")
            {
                // Make the move
                _buttonlist[i].text = computerSide;

                // Call minimax recursively and choose
                // the maximum value
                int[] moveVal = minimax(_buttonlist, 0, false);

                // Undo the move
                _buttonlist[i].text = "";

                // If the value of the current move is
                // more than the best value, then update
                // best/
                Debug.Log("i= "+ i +"  moveVal = " + moveVal[0].ToString()+ " " +  moveVal[1].ToString());
                if (moveVal[0] > bestVal[0] /*&& moveVal[1] <= bestVal[1]*/)
                {
                    bestVal[0] = moveVal[0];
                    //bestVal[1] = moveVal[1];
                    bestMove = i;
                }
            }
        }
        Debug.Log( "The value of the best Move is =  " + bestMove.ToString() + "// best val" + bestVal[0].ToString());
        return bestMove;
    }
}

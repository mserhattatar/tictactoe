using System.Collections.Generic;
using System.Linq;
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
    private string _playerSide;
    private string _computerSide;
    private int _value, _moveCount;

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

    public bool playerMove, delayBool, twoPlayer;
    public float delay;

    private void Awake()
    {
        SetGameControllerReferenceOnButtons();
        gameOverPanel.SetActive(false);
        _moveCount = 0;
        restartGameButton.SetActive(false);
        if (restartGameButton2 != null)
            restartGameButton2.SetActive(false);
        SetPlayerButtons(true);
        playerMove = true;
        delay = 10;
        delayBool = true;
        twoPlayer = GameManager.instance.twoPlayer;
    }

    private void Update()
    {
        if (!twoPlayer && playerMove == false)
        {
            delay += delay * Time.deltaTime;
            if (delay >= 20 && delayBool)
            {
                delayBool = false;
                _value = FindBestMove(buttonList);
                if (buttonList[_value].GetComponentInParent<Button>().interactable == true)
                {
                    buttonList[_value].text = GetComputerSide();
                    buttonList[_value].GetComponentInParent<Button>().interactable = false;
                    EndTurn();
                }
            }
        }
    }

    private void SetGameControllerReferenceOnButtons()
    {
        foreach (var t in buttonList)
        {
            t.GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    public void SetStartingSide(string startingSide)
    {
        _playerSide = startingSide;
        if (_playerSide == "X")
        {
            _computerSide = "O";
            SetPlayerColors(playerX, playerO);
        }
        else
        {
            _computerSide = "X";
            SetPlayerColors(playerO, playerX);
        }

        StartGame();
    }

    private void StartGame()
    {
        SetBoardInteractable(true);
        SetPlayerButtons(false);
        startInfo.SetActive(false);
    }

    public string GetPlayerSide()
    {
        return _playerSide;
    }

    public string GetComputerSide()
    {
        return _computerSide;
    }

    private string FindWinner()
    {
        //check horizontal equality
        for (var i = 0; i < 7; i = i + 3)
        {
            if (buttonList[i].text != "" && buttonList[i].text == buttonList[i + 1].text &&
                buttonList[i + 1].text == buttonList[i + 2].text)
            {
                return buttonList[i].text;
            }
        }

        //check vertical equality
        for (var i = 0; i < 3; i++)
        {
            if (buttonList[i].text != "" && buttonList[i].text == buttonList[i + 3].text &&
                buttonList[i + 3].text == buttonList[i + 6].text)
            {
                return buttonList[i].text;
            }
        }

        //check cross equality 
        if (buttonList[0].text != "" && buttonList[0].text == buttonList[4].text &&
            buttonList[4].text == buttonList[8].text)
        {
            return buttonList[4].text;
        }

        if (buttonList[2].text != "" && buttonList[2].text == buttonList[4].text &&
            buttonList[4].text == buttonList[6].text)
        {
            return buttonList[4].text;
        }

        return null;
    }

    public void EndTurn()
    {
        _moveCount++;
        var winner = FindWinner();

        if ((winner == null && _moveCount >= 9) || winner != null)
            GameEnd(winner);
        else
        {
            ChangeSides();
            delay = 10;
            delayBool = true;
        }
    }

    private void SetPlayerColors(Player newPlayer, Player oldPlayer)
    {
        newPlayer.panel.color = activePlayerColor.panelColor;
        newPlayer.text.color = activePlayerColor.textColor;
        oldPlayer.panel.color = inactivePlayerColor.panelColor;
        oldPlayer.text.color = inactivePlayerColor.textColor;
    }

    private void GameEnd(string winningPlayer)
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

    private void ChangeSides()
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

    private void SetGameOverText(string value)
    {
        gameOverPanel.SetActive(true);
        gameOverPanelText.text = value;
    }

    public void RestartGame()
    {
        _moveCount = 0;
        gameOverPanel.SetActive(false);
        restartGameButton.SetActive(false);
        if (restartGameButton2 != null)
            restartGameButton2.SetActive(false);
        SetPlayerButtons(true);
        SetPlayerColorsInactive();
        startInfo.SetActive(true);
        playerMove = true;
        delay = 10;

        foreach (var t in buttonList)
        {
            t.text = "";
        }
    }

    private void SetBoardInteractable(bool toggle)
    {
        foreach (var t in buttonList)
        {
            t.GetComponentInParent<Button>().interactable = toggle;
        }
    }

    private void SetPlayerButtons(bool toggle)
    {
        playerX.button.interactable = toggle;
        playerO.button.interactable = toggle;
    }

    private void SetPlayerColorsInactive()
    {
        playerX.panel.color = inactivePlayerColor.panelColor;
        playerX.text.color = inactivePlayerColor.textColor;
        playerO.panel.color = inactivePlayerColor.panelColor;
        playerO.text.color = inactivePlayerColor.textColor;
    }

    private static bool FindEmptyText(IEnumerable<Text> buttonTextList)
    {
        return buttonTextList.Any(t => t.text == "");
    }

    private int[] Minimax(IReadOnlyList<Text> buttonList, int depth, bool isMax)
    {
        string winner = FindWinner();
        // If Maximizer has won the game return his/her
        // evaluated score
        if (_computerSide == winner)
        {
            return new int[] { 10, depth };
        }

        // If Minimizer has won the game return his/her
        // evaluated score
        if (_playerSide == winner)
        {
            return new int[] { -10, depth };
        }

        // If there are no more moves and no winner then
        // it is a tie
        if (!FindEmptyText(buttonList))
        {
            return new int[] { 0, depth };
        }

        // If this maximizer's move
        if (isMax)
        {
            int best = -1000;
            int[] returnBest = new int[] { best, depth };
            // Traverse all cells
            for (int i = 0; i < buttonList.Count; i++)
            {
                // Check if cell is empty
                if (buttonList[i].text == "")
                {
                    // Make the move
                    buttonList[i].text = _computerSide;


                    // Call minimax recursively and choose
                    // the maximum value
                    var returnMiniMax = Minimax(buttonList, depth + 1, !isMax);
                    if (returnMiniMax[0] > returnBest[0])
                        returnBest = returnMiniMax;

                    // Undo the move
                    buttonList[i].text = "";
                }
            }

            return returnBest;
        }

        // If this minimizer's move
        else
        {
            int best = 1000;
            int[] returnBest = new int[] { best, depth };
            foreach (var t in buttonList)
            {
                // Check if cell is empty
                if (t.text == "")
                {
                    // Make the move
                    t.text = _playerSide;

                    // Call minimax recursively and choose
                    // the maximum value
                    var returnMiniMax = Minimax(buttonList, depth + 1, !isMax);
                    if (returnMiniMax[0] < returnBest[0])
                        returnBest = returnMiniMax;
                    // Undo the move
                    t.text = "";
                }
            }

            return returnBest;
        }
    }

    // This will return the best possible move for the player
    private int FindBestMove(Text[] buttonTextList)
    {
        int[] bestVal = new int[] { -1000, +100 };
        int bestMove = -1;

        // Traverse all cells, evaluate minimax function for
        // all empty cells. And return the cell with optimal
        // value.

        for (var i = 0; i < buttonTextList.Length; i++)
        {
            // Check if cell is empty
            if (buttonTextList[i].text == "")
            {
                // Make the move
                buttonTextList[i].text = _computerSide;

                // Call minimax recursively and choose
                // the maximum value
                int[] newVal = Minimax(buttonTextList, 0, false);

                // Undo the move
                buttonTextList[i].text = "";

                // If the value of the current move is
                // more than the best value, then update
                // best/

                if (newVal[0] > bestVal[0] || newVal[0] == bestVal[0] && newVal[1] < bestVal[1])
                {
                    bestVal = newVal;
                    bestMove = i;
                }
            }
        }

        return bestMove;
    }
}
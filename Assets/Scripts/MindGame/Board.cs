using System.Collections.Generic;
using UnityEngine;

public enum Owners//кому принадлежит гекс
{
    Player,
    AI,
    Neutral
}

public class Board : MonoBehaviour
{
    public AllCells container;
    public int playerScore = 0;
    public int aiScore = 0;
    public List<Cell> playerCells = new List<Cell>();
    public List<Cell> aiCells = new List<Cell>();
    public char[,] gameBoard = new char[9, 9];
    private int minimaxDepth = 6;

    public bool CanSelect(Cell curCell, int turn)//можем ли мы выбрать данный гекс
    {
        return false;
    }
    public int Move(Cell fromCell, Cell toCell)
    {
        int distance = -1;
        //Debug.Log("Diff: " + (toCell.pos.First - fromCell.pos.First));
        switch (toCell.pos.First - fromCell.pos.First)
        {
            case 0:
                distance = Mathf.Abs(toCell.pos.Second - fromCell.pos.Second);
                break;
            case 1:
                //Debug.Log(fromCell.pos.Second + " " + toCell.pos.Second);
                if ((toCell.pos.Second >= fromCell.pos.Second - 1) && (toCell.pos.Second <= fromCell.pos.Second + 2))
                {
                    if (fromCell.pos.Second == toCell.pos.Second || toCell.pos.Second == fromCell.pos.Second + 1)
                    {
                        //Debug.Log(fromCell.pos.Second + " " + toCell.pos.Second);
                        distance = 1;
                    }
                    else
                        distance = 2;
                }
                break;
            case -1:
                if ((toCell.pos.Second >= fromCell.pos.Second - 2) && (toCell.pos.Second <= fromCell.pos.Second + 1))
                {
                    if (fromCell.pos.Second == toCell.pos.Second || toCell.pos.Second == fromCell.pos.Second - 1)
                        distance = 1;
                    else
                        distance = 2;
                }
                break;
            case 2:
                if ((toCell.pos.Second >= fromCell.pos.Second) && (toCell.pos.Second <= fromCell.pos.Second + 2))
                {
                    distance = 2;
                }
                break;
            case -2:
                if ((toCell.pos.Second <= fromCell.pos.Second) && (toCell.pos.Second >= fromCell.pos.Second - 2))
                {
                    distance = 2;
                }
                break;
            default:
                distance = -1;
                break;
        }

        return distance;
    }
    public void UpdateScore()
    {
        
    }
    public List<Cell> AfterMove(Cell cell)
    {
        List<Cell> changedCells = new List<Cell>();
        int i = cell.pos.First;
        int j = cell.pos.Second;

        changedCells.Add(new Cell(i, j - 1/*, cell.id*/, Owners.AI));
        changedCells.Add(new Cell(i, j + 1/*, cell.id*/, Owners.AI));
        changedCells.Add(new Cell(i - 1, j - 1/*, cell.id*/, Owners.AI));
        changedCells.Add(new Cell(i - 1, j/*, cell.id*/, Owners.AI));
        changedCells.Add(new Cell(i + 1, j/*, cell.id*/, Owners.AI));
        changedCells.Add(new Cell(i + 1, j + 1/*, cell.id*/, Owners.AI));

        return changedCells;
    }

    bool first = true;

    public int Minimax(Pair<int, int> curPos, bool maximizingPlayer, char[,] board, int depth, int alpha, int beta)//ai - maximize, player - minimize
    {
        //add optimization; more depth; add 2 distance moves; check not all moves of all hexes, just all possible hexes to go without duplicates;
        //fix bugs: more depths - incorrect hexes;

        //check pos
        if (depth > 0)
        {
            if (curPos.First < 0 || curPos.First > 8 || curPos.Second < 0 || curPos.Second > 8)
                return maximizingPlayer ? System.Int32.MaxValue : -999999;

            if (board[curPos.First, curPos.Second] != 'n')
                return maximizingPlayer ? System.Int32.MaxValue : -999998;
        }

        if (maximizingPlayer)
        {
            if (depth >= this.minimaxDepth)
            {
                int p = 0;
                int a = 0;
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (board[i, j] == 'p')
                        {
                            //if (first)
                            //Debug.Log("P: " + i.ToString() + " " + j.ToString());
                            p++;
                        }
                        else if (board[i, j] == 'a')
                        {
                            //if (first)
                            //Debug.Log("A: " + i.ToString() + " " + j.ToString());
                            a++;
                        }
                    }
                }
                //Debug.Log('\n');
                //Debug.Log("ANSWER " + a.ToString() + " " + p.ToString());
                first = true;
                return a - p;
            }
            //move & aftermove
            if (depth > 0)
            {
                board[curPos.First, curPos.Second] = 'p';
                if (curPos.Second - 1 >= 0)
                {
                    if (board[curPos.First, curPos.Second - 1] == 'a')
                    {
                        board[curPos.First, curPos.Second - 1] = 'p';
                    }
                }

                if (curPos.Second + 1 < 9)
                {
                    if (board[curPos.First, curPos.Second + 1] == 'a')
                    {
                        board[curPos.First, curPos.Second + 1] = 'p';
                    }
                }

                if (curPos.First - 1 >= 0 && curPos.Second - 1 >= 0)
                {
                    if (board[curPos.First - 1, curPos.Second - 1] == 'a')
                    {
                        board[curPos.First - 1, curPos.Second - 1] = 'p';
                    }
                }

                if (curPos.First - 1 >= 0 && curPos.Second >= 0)
                {
                    if (board[curPos.First - 1, curPos.Second] == 'a')
                    {
                        board[curPos.First - 1, curPos.Second] = 'p';
                    }
                }

                if (curPos.First + 1 < 9 && curPos.Second + 1 < 9)
                {
                    if (board[curPos.First + 1, curPos.Second + 1] == 'a')
                    {
                        board[curPos.First + 1, curPos.Second + 1] = 'p';
                    }
                }

                if (curPos.First + 1 < 9)
                {
                    if (board[curPos.First + 1, curPos.Second] == 'a')
                    {
                        board[curPos.First + 1, curPos.Second] = 'p';
                    }
                }
            }

            /*
            for (int k = 0; k < 9; k++)
            {
                for (int z = 0; z < 9; z++)
                {
                    if (board[k, z] == 'a' || board[k, z] == 'p')
                        Debug.Log(board[k, z]);
                }
                Debug.Log('\n');
            }
            */

            //check board what hexes we already visited
            char[,] checkBoard = new char[9, 9];
            for (int k = 0; k < 9; k++)
            {
                for (int z = 0; z < 9; z++)
                {
                    checkBoard[k, z] = 'n';
                }
            }

            //recursion
            int maxScore = System.Int32.MinValue;
            int tmp;
            Pair<int, int> best = new Pair<int, int>(-1, -1);
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (board[i, j] == 'a')
                    {
                        //Debug.Log("Depth: " + depth.ToString() + " " + i.ToString() + " " + j.ToString() + '\n');
                        int y = i;
                        int x = j;
                        Pair<int, int> newPos = new Pair<int, int>(y, x + 1);

                        /*
                        char[,] tmpBoard = new char[9, 9];                
                        for (int k = 0; k < 9; k++)
                        {
                            for (int z = 0; z < 9; z++)
                            {
                                tmpBoard[k, z] = board[k, z];
                            }
                        }
                        */
                        if (!(newPos.First < 0 || newPos.First > 8 || newPos.Second < 0 || newPos.Second > 8)) 
                        {
                            if (checkBoard[newPos.First, newPos.Second] != 'c' && board[newPos.First, newPos.Second] == 'n') {
                                char[,] tmpBoard = new char[9, 9];
                                for (int k = 0; k < 9; k++)
                                {
                                    for (int z = 0; z < 9; z++)
                                    {
                                        char temp = new char();
                                        temp = board[k, z];
                                        tmpBoard[k, z] = temp;
                                    }
                                }
                                tmp = Mathf.Max(maxScore, Minimax(newPos, !maximizingPlayer, tmpBoard, depth + 1, alpha, beta));
                                if (tmp > maxScore)
                                {
                                    maxScore = tmp;
                                    best.First = newPos.First;
                                    best.Second = newPos.Second;
                                }
                                alpha = Mathf.Max(alpha, maxScore);
                                if (beta <= alpha)
                                {
                                    return maxScore;
                                }

                                checkBoard[newPos.First, newPos.Second] = 'c';
                            }
                        }

                        newPos = new Pair<int, int>();
                        newPos.First = y;
                        newPos.Second = x - 1;
                        if (!(newPos.First < 0 || newPos.First > 8 || newPos.Second < 0 || newPos.Second > 8))
                        {
                            if (checkBoard[newPos.First, newPos.Second] != 'c' && board[newPos.First, newPos.Second] == 'n')
                            {
                                char[,] tmpBoard = new char[9, 9];
                                for (int k = 0; k < 9; k++)
                                {
                                    for (int z = 0; z < 9; z++)
                                    {
                                        char temp = new char();
                                        temp = board[k, z];
                                        tmpBoard[k, z] = temp;
                                    }
                                }
                                tmp = Mathf.Max(maxScore, Minimax(newPos, !maximizingPlayer, tmpBoard, depth + 1, alpha, beta));
                                if (tmp > maxScore)
                                {
                                    maxScore = tmp;
                                    best.First = newPos.First;
                                    best.Second = newPos.Second;
                                }
                                alpha = Mathf.Max(alpha, maxScore);
                                if (beta <= alpha)
                                {
                                    return maxScore;
                                }

                                checkBoard[newPos.First, newPos.Second] = 'c';
                            }
                        }

                        newPos = new Pair<int, int>();
                        newPos.First = y - 1;
                        newPos.Second = x - 1;
                        if (!(newPos.First < 0 || newPos.First > 8 || newPos.Second < 0 || newPos.Second > 8))
                        {
                            if (checkBoard[newPos.First, newPos.Second] != 'c' && board[newPos.First, newPos.Second] == 'n')
                            {
                                char[,] tmpBoard = new char[9, 9];
                                for (int k = 0; k < 9; k++)
                                {
                                    for (int z = 0; z < 9; z++)
                                    {
                                        char temp = new char();
                                        temp = board[k, z];
                                        tmpBoard[k, z] = temp;
                                    }
                                }
                                tmp = Mathf.Max(maxScore, Minimax(newPos, !maximizingPlayer, tmpBoard, depth + 1, alpha, beta));
                                if (tmp > maxScore)
                                {
                                    maxScore = tmp;
                                    best.First = newPos.First;
                                    best.Second = newPos.Second;
                                }
                                alpha = Mathf.Max(alpha, maxScore);
                                if (beta <= alpha)
                                {
                                    return maxScore;
                                }

                                checkBoard[newPos.First, newPos.Second] = 'c';
                            }
                        }

                        newPos = new Pair<int, int>();
                        newPos.First = y - 1;
                        newPos.Second = x;
                        if (!(newPos.First < 0 || newPos.First > 8 || newPos.Second < 0 || newPos.Second > 8))
                        {
                            if (checkBoard[newPos.First, newPos.Second] != 'c' && board[newPos.First, newPos.Second] == 'n')
                            {
                                char[,] tmpBoard = new char[9, 9];
                                for (int k = 0; k < 9; k++)
                                {
                                    for (int z = 0; z < 9; z++)
                                    {
                                        char temp = new char();
                                        temp = board[k, z];
                                        tmpBoard[k, z] = temp;
                                    }
                                }
                                tmp = Mathf.Max(maxScore, Minimax(newPos, !maximizingPlayer, tmpBoard, depth + 1, alpha, beta));
                                if (tmp > maxScore)
                                {
                                    maxScore = tmp;
                                    best.First = newPos.First;
                                    best.Second = newPos.Second;
                                }
                                alpha = Mathf.Max(alpha, maxScore);
                                if (beta <= alpha)
                                {
                                    return maxScore;
                                }

                                checkBoard[newPos.First, newPos.Second] = 'c';
                            }
                        }

                        newPos = new Pair<int, int>();
                        newPos.First = y + 1;
                        newPos.Second = x;
                        if (!(newPos.First < 0 || newPos.First > 8 || newPos.Second < 0 || newPos.Second > 8))
                        {
                            if (checkBoard[newPos.First, newPos.Second] != 'c' && board[newPos.First, newPos.Second] == 'n')
                            {
                                char[,] tmpBoard = new char[9, 9];
                                for (int k = 0; k < 9; k++)
                                {
                                    for (int z = 0; z < 9; z++)
                                    {
                                        char temp = new char();
                                        temp = board[k, z];
                                        tmpBoard[k, z] = temp;
                                    }
                                }
                                tmp = Mathf.Max(maxScore, Minimax(newPos, !maximizingPlayer, tmpBoard, depth + 1, alpha, beta));
                                if (tmp > maxScore)
                                {
                                    maxScore = tmp;
                                    best.First = newPos.First;
                                    best.Second = newPos.Second;
                                }
                                alpha = Mathf.Max(alpha, maxScore);
                                if (beta <= alpha)
                                {
                                    return maxScore;
                                }

                                checkBoard[newPos.First, newPos.Second] = 'c';
                            }
                        }

                        newPos = new Pair<int, int>();
                        newPos.First = y + 1;
                        newPos.Second = x + 1;
                        if (!(newPos.First < 0 || newPos.First > 8 || newPos.Second < 0 || newPos.Second > 8))
                        {
                            if (checkBoard[newPos.First, newPos.Second] != 'c' && board[newPos.First, newPos.Second] == 'n')
                            {
                                char[,] tmpBoard = new char[9, 9];
                                for (int k = 0; k < 9; k++)
                                {
                                    for (int z = 0; z < 9; z++)
                                    {
                                        char temp = new char();
                                        temp = board[k, z];
                                        tmpBoard[k, z] = temp;
                                    }
                                }
                                tmp = Mathf.Max(maxScore, Minimax(newPos, !maximizingPlayer, tmpBoard, depth + 1, alpha, beta));
                                if (tmp > maxScore)
                                {
                                    maxScore = tmp;
                                    best.First = newPos.First;
                                    best.Second = newPos.Second;
                                }
                                alpha = Mathf.Max(alpha, maxScore);
                                if (beta <= alpha)
                                {
                                    return maxScore;
                                }

                                checkBoard[newPos.First, newPos.Second] = 'c';
                            }
                        }
                    }
                }
            }

            if (depth == 0)
            {
                Debug.Log("Best position: " + best.First.ToString() + best.Second.ToString() + " Max score: " + maxScore.ToString());
                if (maxScore == int.MinValue) return int.MinValue;
                return System.Int32.Parse((best.First.ToString() + best.Second.ToString()));
            }
            else
                return maxScore;
        }
        else
        {          
            //move & aftermove
            board[curPos.First, curPos.Second] = 'a';
            if (curPos.Second - 1 >= 0)
            {
                if (board[curPos.First, curPos.Second - 1] == 'p')
                {
                    board[curPos.First, curPos.Second - 1] = 'a';
                }
            }

            if (curPos.Second + 1 < 9)
            {
                if (board[curPos.First, curPos.Second + 1] == 'p')
                {
                    board[curPos.First, curPos.Second + 1] = 'a';
                }
            }

            if (curPos.First - 1 >= 0 && curPos.Second - 1 >= 0)
            {
                if (board[curPos.First - 1, curPos.Second - 1] == 'p')
                {
                    board[curPos.First - 1, curPos.Second - 1] = 'a';
                }
            }

            if (curPos.First - 1 >= 0 && curPos.Second >= 0)
            {
                if (board[curPos.First - 1, curPos.Second] == 'p')
                {
                    board[curPos.First - 1, curPos.Second] = 'a';
                }
            }

            if (curPos.First + 1 < 9 && curPos.Second + 1 < 9)
            {
                if (board[curPos.First + 1, curPos.Second + 1] == 'p')
                {
                    board[curPos.First + 1, curPos.Second + 1] = 'a';
                }
            }

            if (curPos.First + 1 < 9)
            {
                if (board[curPos.First + 1, curPos.Second] == 'p')
                {
                    board[curPos.First + 1, curPos.Second] = 'a';
                }
            }
            /*
            if (depth >= this.minimaxDepth)
            {
                int p = 0;
                int a = 0;
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (board[i, j] == 'p')
                        {
                            //if (first)
                                //Debug.Log("P: " + i.ToString() + " " + j.ToString());
                            p++;
                        }
                        else if (board[i, j] == 'a')
                        {
                            //if (first)
                                //Debug.Log("A: " + i.ToString() + " " + j.ToString());
                            a++;
                        }
                    }
                }
                //Debug.Log('\n');
                //Debug.Log("ANSWER " + a.ToString() + " " + p.ToString());
                first = true;
                return a - p;
            }*/

            //check board what hexes we already visited
            char[,] checkBoard = new char[9, 9];
            for (int k = 0; k < 9; k++)
            {
                for (int z = 0; z < 9; z++)
                {
                    checkBoard[k, z] = 'n';
                }
            }

            //recursion
            int minScore = System.Int32.MaxValue;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (board[i, j] == 'p')
                    {
                        //Debug.Log("Depth: " + depth.ToString() + " " + i.ToString() + " " + j.ToString() + '\n');
                        int y = i;
                        int x = j;
                        Pair<int, int> newPos = new Pair<int, int>(y, x + 1);

                        /*
                        char[,] tmpBoard = new char[9,9];
                        for (int k = 0; k < 9; k++)
                        {
                            for (int z = 0; z < 9; z++)
                            {
                                char temp = new char();
                                temp = board[k, z];
                                tmpBoard[k, z] = temp;
                            }
                        }
                        */
                        if (!(newPos.First < 0 || newPos.First > 8 || newPos.Second < 0 || newPos.Second > 8))
                        {
                            if (checkBoard[newPos.First, newPos.Second] != 'c' && board[newPos.First, newPos.Second] == 'n')
                            {
                                char[,] tmpBoard = new char[9, 9];
                                for (int k = 0; k < 9; k++)
                                {
                                    for (int z = 0; z < 9; z++)
                                    {
                                        char temp = new char();
                                        temp = board[k, z];
                                        tmpBoard[k, z] = temp;
                                    }
                                }
                                minScore = Mathf.Min(minScore, Minimax(newPos, !maximizingPlayer, tmpBoard, depth + 1, alpha, beta));
                                beta = Mathf.Min(beta, minScore);
                                if (beta <= alpha)
                                {
                                    return minScore;
                                }

                                checkBoard[newPos.First, newPos.Second] = 'c';
                            }
                        }

                        newPos = new Pair<int, int>();
                        newPos.First = y;
                        newPos.Second = x - 1;
                        if (!(newPos.First < 0 || newPos.First > 8 || newPos.Second < 0 || newPos.Second > 8))
                        {
                            if (checkBoard[newPos.First, newPos.Second] != 'c' && board[newPos.First, newPos.Second] == 'n')
                            {
                                char[,] tmpBoard = new char[9, 9];
                                for (int k = 0; k < 9; k++)
                                {
                                    for (int z = 0; z < 9; z++)
                                    {
                                        char temp = new char();
                                        temp = board[k, z];
                                        tmpBoard[k, z] = temp;
                                    }
                                }
                                minScore = Mathf.Min(minScore, Minimax(newPos, !maximizingPlayer, tmpBoard, depth + 1, alpha, beta));
                                beta = Mathf.Min(beta, minScore);
                                if (beta <= alpha)
                                {
                                    return minScore;
                                }

                                checkBoard[newPos.First, newPos.Second] = 'c';
                            }
                        }

                        newPos = new Pair<int, int>();
                        newPos.First = y - 1;
                        newPos.Second = x - 1;
                        if (!(newPos.First < 0 || newPos.First > 8 || newPos.Second < 0 || newPos.Second > 8))
                        {
                            if (checkBoard[newPos.First, newPos.Second] != 'c' && board[newPos.First, newPos.Second] == 'n')
                            {
                                char[,] tmpBoard = new char[9, 9];
                                for (int k = 0; k < 9; k++)
                                {
                                    for (int z = 0; z < 9; z++)
                                    {
                                        char temp = new char();
                                        temp = board[k, z];
                                        tmpBoard[k, z] = temp;
                                    }
                                }
                                minScore = Mathf.Min(minScore, Minimax(newPos, !maximizingPlayer, tmpBoard, depth + 1, alpha, beta));
                                beta = Mathf.Min(beta, minScore);
                                if (beta <= alpha)
                                {
                                    return minScore;
                                }

                                checkBoard[newPos.First, newPos.Second] = 'c';
                            }
                        }

                        newPos = new Pair<int, int>();
                        newPos.First = y - 1;
                        newPos.Second = x;
                        if (!(newPos.First < 0 || newPos.First > 8 || newPos.Second < 0 || newPos.Second > 8))
                        {
                            if (checkBoard[newPos.First, newPos.Second] != 'c' && board[newPos.First, newPos.Second] == 'n')
                            {
                                char[,] tmpBoard = new char[9, 9];
                                for (int k = 0; k < 9; k++)
                                {
                                    for (int z = 0; z < 9; z++)
                                    {
                                        char temp = new char();
                                        temp = board[k, z];
                                        tmpBoard[k, z] = temp;
                                    }
                                }
                                minScore = Mathf.Min(minScore, Minimax(newPos, !maximizingPlayer, tmpBoard, depth + 1, alpha, beta));
                                beta = Mathf.Min(beta, minScore);
                                if (beta <= alpha)
                                {
                                    return minScore;
                                }

                                checkBoard[newPos.First, newPos.Second] = 'c';
                            }
                        }

                        newPos = new Pair<int, int>();
                        newPos.First = y + 1;
                        newPos.Second = x;
                        if (!(newPos.First < 0 || newPos.First > 8 || newPos.Second < 0 || newPos.Second > 8))
                        {
                            if (checkBoard[newPos.First, newPos.Second] != 'c' && board[newPos.First, newPos.Second] == 'n')
                            {
                                char[,] tmpBoard = new char[9, 9];
                                for (int k = 0; k < 9; k++)
                                {
                                    for (int z = 0; z < 9; z++)
                                    {
                                        char temp = new char();
                                        temp = board[k, z];
                                        tmpBoard[k, z] = temp;
                                    }
                                }
                                minScore = Mathf.Min(minScore, Minimax(newPos, !maximizingPlayer, tmpBoard, depth + 1, alpha, beta));
                                beta = Mathf.Min(beta, minScore);
                                if (beta <= alpha)
                                {
                                    return minScore;
                                }

                                checkBoard[newPos.First, newPos.Second] = 'c';
                            }
                        }

                        newPos = new Pair<int, int>();
                        newPos.First = y + 1;
                        newPos.Second = x + 1;
                        if (!(newPos.First < 0 || newPos.First > 8 || newPos.Second < 0 || newPos.Second > 8))
                        {
                            if (checkBoard[newPos.First, newPos.Second] != 'c' && board[newPos.First, newPos.Second] == 'n')
                            {
                                char[,] tmpBoard = new char[9, 9];
                                for (int k = 0; k < 9; k++)
                                {
                                    for (int z = 0; z < 9; z++)
                                    {
                                        char temp = new char();
                                        temp = board[k, z];
                                        tmpBoard[k, z] = temp;
                                    }
                                }
                                minScore = Mathf.Min(minScore, Minimax(newPos, !maximizingPlayer, tmpBoard, depth + 1, alpha, beta));
                                beta = Mathf.Min(beta, minScore);
                                if (beta <= alpha)
                                {
                                    return minScore;
                                }

                                checkBoard[newPos.First, newPos.Second] = 'c';
                            }
                        }
                    }
                }
            }
            return minScore;
        }
    }

    private void Awake()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                gameBoard[i, j] = '-';
            }
        }
        container = new AllCells();
    }
}

//[System.Serializable]
public class AllCells//массив всех гексов доски
{
    public Cell[] cells;//зависит от уровня сложности пациента
    public AllCells()
    {
        cells = new Cell[61];
    }
}
//[System.Serializable]
public class Cell //: System.IComparable<Cell>//отдельный гекс
{
    public Pair<int, int> pos = new Pair<int, int>();
    public Owners owner;
    //public int id;

    public Cell()
    {
        owner = Owners.Neutral;
        pos.First = -1;
        pos.Second = -1;
        //id = -1;
    }

    public Cell(int _row, int _col, /*int _id,*/ Owners _owner = Owners.Neutral)
    {
        owner = _owner;
        //id = _id;
        pos.First = _row;
        pos.Second = _col;
    }

    public void ChangeOwner(Owners newOwner)
    {
        this.owner = newOwner;
    }

    public void SetPosition(int row, int col)
    {
        pos.First = row;
        pos.Second = col;
    }

    public void SetId(int id)
    {
        //this.id = id;
    }
    
    public static bool operator ==(Cell cell1, Cell cell2)
    {
        return (cell1.pos.First == cell2.pos.First) && (cell1.pos.Second == cell2.pos.Second);
    }

    public static bool operator !=(Cell cell1, Cell cell2)
    {
        return !(cell1.pos.First == cell2.pos.First) && (cell1.pos.Second == cell2.pos.Second);
    }
    
    public override string ToString()
    {
        return this.pos.ToString() + " " /*+ this.id.ToString()*/ + " " + this.owner.ToString();
    }

    //public int CompareTo(Cell obj)
    //{
    //    Cell c = obj as Cell;
    //    if (c != null)
    //    {
    //        if ((this.pos.First == c.pos.First) && (this.pos.Second == c.pos.Second))
    //            return 0;
    //        else
    //            return this.id >= c.id ? 1 : -1;
    //    }
    //    else
    //        throw new System.NotImplementedException();
    //}


    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        Cell c = obj as Cell;
        if ((this.pos.First == c.pos.First) && (this.pos.Second == c.pos.Second)/* && (this.owner == c.owner)*/)
            return true;
        else
            return false;
    }

    public bool Equals(Cell obj)
    {
        return obj != null && (this.pos.First == obj.pos.First) && (this.pos.Second == obj.pos.Second) /*&& (this.owner == obj.owner)*/;
    }

    public override int GetHashCode()
    {
        return this.pos.First + this.pos.Second;
    }
}

public class Pair<T, U>
{
    public Pair() {}

    public Pair(T first, U second)
    {
        this.First = first;
        this.Second = second;
    }

    public T First { get; set; }
    public U Second { get; set; }

    public override string ToString()
    {
        return this.First.ToString() + "  " + this.Second.ToString();
    }
};
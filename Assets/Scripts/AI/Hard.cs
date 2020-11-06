using System;
using System.Collections.Generic;
using UnityEngine;

public class Hard : AI
{
    Stack<(int, int, Pieces)> prev = new Stack<(int, int, Pieces)>();
    bool isMaxStart = Game._currTeam == Game.Team.Light; //(Game._currTeam == Game.Team.Light ? true : false);
    private (int, int) cLoc;
    private (int, int) nLoc;
    private (int, int) bmove;
    private Pieces bP;



    public void bruh()
    {

        bP.Move(board._tiles[bmove.Item1, bmove.Item2]);



    }
    public override void selectMove()
    {


        playing = true;
        var bTemp = -9999;

        foreach (var p in board.p2)
        {

            foreach (var m in p.GetMoves(board._tiles))
            {

                forward(p, m);
                var value = minimax(1, -100000, 100000, !isMaxStart);
                reverse(p);
                if (value >= bTemp)
                {
                    bP = p;
                    bTemp = value;
                    bmove = m;

                }
            }
        }

        bruh();

    }

    private int minimax(int depth, int alpha, int beta, bool isMax)
    {
        if (depth == 0)
        {
            return -EvalBoard();
        }
        if (isMax)
        {
            var bestM = -9999;
            foreach (var p in board.p1)
            {
                foreach (var m in p.GetMoves(board._tiles))
                {

                    forward(p, m);
                    bestM = Math.Max(bestM, minimax(depth - 1, alpha, beta, !isMax));
                    reverse(p);
                    alpha = Math.Max(alpha, bestM);
                    if (beta <= alpha)
                    {
                        return bestM;
                    }
                }
            }
            return bestM;
        }
        else
        {
            var bestM = 9999;
            foreach (var p in board.p2)
            {
                foreach (var m in p.GetMoves(board._tiles))
                {
                    forward(p, m);
                    bestM = Math.Min(bestM, minimax(depth - 1, alpha, beta, !isMax));
                    reverse(p);
                    beta = Math.Min(alpha, bestM);
                    if (beta <= alpha)
                    {
                        return bestM;
                    }
                }
            }
            return bestM;
        }


    }

    private int EvalBoard()
    {
        var tEval = 0;
        var x = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (board._tiles[i, j].piece != null)
                {
                    x++;
                    tEval += board._tiles[i, j].piece.value;

                }

            }
        }
        return tEval;
    }

    private void forward(Pieces p, (int, int) n)
    {

        if (board._tiles[n.Item1, n.Item2].piece == null) //if the tile has no piece
        {
            prev.Push((p.Loc.Item1, p.Loc.Item2, null));
        }
        else
        {
            prev.Push((p.Loc.Item1, p.Loc.Item2, board._tiles[n.Item1, n.Item2].piece));
        }

        board._tiles[p.Loc.Item1, p.Loc.Item2].piece = null;


        try
        {
            p.GhostMove(board._tiles[n.Item1, n.Item2]);//move piece to tile
        }
        catch (Exception e)
        {
            foreach (var x in p.GetMoves(board._tiles))
            {
                Debug.LogError(x);
            }
            Debug.LogError(p.GetType());
        }


    }

    private void reverse(Pieces p)
    {

        var x = prev.Pop();
        if (x.Item3 == null)
        {
            board._tiles[p.Loc.Item1, p.Loc.Item2].piece = null;


        }
        else
        {
            board._tiles[p.Loc.Item1, p.Loc.Item2].piece = x.Item3;
        }

        p.GhostMove(board._tiles[x.Item1, x.Item2]);
    }





}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Hard : AI
{
    private readonly Stack<(int, int, Pieces)> _prev;

    public Hard(Game game, Board board) : base(game, board)
    {
        _prev = new Stack<(int, int, Pieces)>();
    }
    
    public override IEnumerator SelectMove(Action<(Pieces, int, int)> action)
    {
        var bestScore = int.MinValue;
        Pieces bestPiece = null;
        var bestMove = (0, 0);
        foreach (var tile in Board.Tiles)
        {
            if (tile.piece == null || tile.piece.team != Game.currTeam) continue;
            foreach (var move in tile.piece.GetMoves(Board.Tiles))
            {
                var piece = tile.piece;
                Forward(piece, move);
                var value = Minimax(5, -800, 800, false);
                Reverse(piece, move);
                if (value >= bestScore)
                {
                    bestPiece = piece;
                    bestScore = value;
                    bestMove = move;
                }
                yield return null;
            }
        }
        var (x, y) = bestMove;
        action((bestPiece, x, y));
    }

    private int Minimax(int depth, int alpha, int beta, bool isMax)
    {
        if (depth == 0)
        {
            return -EvalBoard();
        }
        if (isMax)
        {
            var bestScore = int.MinValue;
            foreach (var tile in Board.Tiles)
            {
                if (tile.piece == null || tile.piece.team == Game.currTeam) continue;
                foreach (var move in tile.piece.GetMoves(Board.Tiles))
                {
                    var piece = tile.piece;
                    Forward(piece, move);
                    bestScore = Math.Max(bestScore, Minimax(depth - 1, alpha, beta, false));
                    Reverse(piece, move);
                    alpha = Math.Max(alpha, bestScore);
                    if (beta <= alpha)
                    {
                        return bestScore;
                    }
                }
            }
            return bestScore;
        }
        else
        {
            var bestScore = int.MaxValue;
            foreach (var tile in Board.Tiles)
            {
                if (tile.piece == null || tile.piece.team != Game.currTeam) continue;
                foreach (var move in tile.piece.GetMoves(Board.Tiles))
                {
                    var piece = tile.piece;
                    Forward(piece, move);
                    bestScore = Math.Min(bestScore, Minimax(depth - 1, alpha, beta, true));
                    Reverse(piece, move);
                    beta = Math.Min(alpha, bestScore);
                    if (beta <= alpha)
                    {
                        return bestScore;
                    }
                }
            }
            return bestScore;
        }

    }

    private int EvalBoard()
    {
        return (from Tile tile in Board.Tiles where tile.piece != null select tile.piece.Value).Sum();
    }

    private void Forward(Pieces p, (int x, int y) m)
    {
        var (x, y) = m;
        var (px, py) = p.Loc;
        _prev.Push((px, py, Board.Tiles[x, y].piece));

        Board.Tiles[x, y].piece = p;
        p.Loc = (x, y);

    }

    private void Reverse(Pieces p, (int x, int y) m)
    {
        var (px, py, piece) = _prev.Pop();
        var (x, y) = m;
        Board.Tiles[px, py].piece = p;
        if (p != null)
            p.Loc = (px, py);
        Board.Tiles[x, y].piece = piece;
    }

}

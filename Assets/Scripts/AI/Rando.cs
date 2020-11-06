using System.Collections.Generic;

public class Rando : AI
{

    public override void selectMove()
    {

        do
        {
            current = pieces[UnityEngine.Random.Range(0, pieces.Count - 1)];

            moves = (List<(int, int)>)current.GetMoves(board._tiles);

        } while (moves.Count <= 0);
        selectm = moves[UnityEngine.Random.Range(0, moves.Count - 1)];
        board._tiles[current.Loc.Item1, current.Loc.Item2].piece = null;
        current.Move(board._tiles[selectm.Item1, selectm.Item2]);


    }



}

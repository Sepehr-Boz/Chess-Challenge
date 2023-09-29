using ChessChallenge.API;
using ChessChallenge.Application;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class MyBot : IChessBot
{
	private Random rnd = new Random();

    private Dictionary<PieceType, List<Move>> pieceMoves = new Dictionary<PieceType, List<Move>>();
    //to access board across all methods
    private Board storedBoard;

    public Move Think(Board board, Timer timer)
    {
        Move[] moves = board.GetLegalMoves();
        //find any moves where a pawn can take a significant piece
        List<Move> pawnTakes = new List<Move>();
        //empty the piecemoves dictionary for new moves
        pieceMoves = new Dictionary<PieceType, List<Move>>();
        //populate each type with at least a key and an empty list
        pieceMoves[PieceType.Pawn] = new List<Move>();
        pieceMoves[PieceType.Knight] = new List<Move>();
        pieceMoves[PieceType.Bishop] = new List<Move>();
        pieceMoves[PieceType.Rook] = new List<Move>();
        pieceMoves[PieceType.Queen] = new List<Move>();
        pieceMoves[PieceType.King] = new List<Move>();
        storedBoard = board;

        foreach (Move move in moves)
        {
            PieceType type = move.MovePieceType;
            PieceType capture = move.CapturePieceType;
            pieceMoves[type].Add(move);
 

            if (type == PieceType.Pawn && (capture==PieceType.Knight || capture==PieceType.Bishop || capture==PieceType.Rook || capture == PieceType.Queen))
            {
                pawnTakes.Add(move);
            }
        }
        
        if (pawnTakes.Count > 0)
        {
            return pawnTakes[rnd.Next(pawnTakes.Count)];
        }

        //filter out any safe pieces
        CheckForAnyPiecesInDanger(pieceMoves);

        return GoToAnotherPiece();
    }

    private Move GoToAnotherPiece()
    {
        //if no valid moves then go to a random move
        int val = rnd.Next(0, 12);
        switch (val)
        {
            case 10: case 11:
                return TryPawnMove();
            case 8: case 9:
                return TryKnightMove();
            case 6: case 7:
                return TryBishopMove();
            case 4: case 5:
                return TryRookMove();
            case 2: case 3:
                return TryQueenMove();
            case 0: case 1:
                return TryKingMove();
            default:
                return Move.NullMove;
        }
    }



    private Move TryPawnMove()
    {
        if (pieceMoves[PieceType.Pawn].Count == 0)
        {
            //if no valid moves then go to a random move
            return GoToAnotherPiece();
        }

        List<Move> moves = pieceMoves[PieceType.Pawn];
        return moves[rnd.Next(moves.Count)];
    }

    private Move TryKnightMove()
    {
        if (pieceMoves[PieceType.Knight].Count == 0)
        {
            return GoToAnotherPiece();
        }

        List<Move> moves = pieceMoves[PieceType.Knight];
        return moves[rnd.Next(moves.Count)];
    }

    private Move TryBishopMove()
    {
        if (pieceMoves[PieceType.Bishop].Count == 0)
        {
            return GoToAnotherPiece();
        }

        List<Move> moves = pieceMoves[PieceType.Bishop];
        return moves[rnd.Next(moves.Count)];
    }

    private Move TryRookMove()
    {
        if (pieceMoves[PieceType.Rook].Count == 0)
        {
            return GoToAnotherPiece();
        }

        List<Move> moves = pieceMoves[PieceType.Rook];
        return moves[rnd.Next(moves.Count)];
    }

    private Move TryQueenMove()
    {
        if (pieceMoves[PieceType.Queen].Count == 0)
        {
            return GoToAnotherPiece();
        }

        List<Move> moves = pieceMoves[PieceType.Queen];
        return moves[rnd.Next(moves.Count)];
    }

    private Move TryKingMove()
    {
        if (pieceMoves[PieceType.King].Count == 0)
        {
            return GoToAnotherPiece();
        }


        List<Move> moves = pieceMoves[PieceType.King];
        return moves[rnd.Next(moves.Count)];
    }

    private void CheckForAnyPiecesInDanger(Dictionary<PieceType, List<Move>> movesList)
    {
        //loop through the whole dictionary and filter out any pieces not in danger
        foreach (PieceType type in movesList.Keys)
        {
            List<Move> moves = movesList[type];
            foreach (Move move in moves)
            {
                //get the piece from the move
                //get the start square then find the piece from the board
                Piece piece = storedBoard.GetPiece(move.StartSquare);

                if (IsPieceInDanger(piece))
                {
                    moves.Remove(move);
                }
            }
        }

    }

    private bool IsPieceInDanger(Piece piece)
    {
        //check if its in danger from other pieces;
        return false;
    }

}
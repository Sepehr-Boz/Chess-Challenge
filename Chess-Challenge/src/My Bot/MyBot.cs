using ChessChallenge.API;
using ChessChallenge.Application;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
        PieceType typeToCheck;
        //if no valid moves then go to a random move
        int val = rnd.Next(0, 12);
        switch (val)
        {
            case 10: case 11:
                typeToCheck = PieceType.Pawn;
                break;
            case 8: case 9:
                typeToCheck = PieceType.Knight;
                break;
            case 6: case 7:
                typeToCheck = PieceType.Bishop;
                break;
            case 4: case 5:
                typeToCheck = PieceType.Rook;
                break;
            case 2: case 3:
                typeToCheck = PieceType.Queen;
                break;
            case 0: case 1:
                typeToCheck = PieceType.King;
                break;
            default:
                typeToCheck = PieceType.None;
                break;
        }

        return TryMove(typeToCheck);
    }

    private bool NeedToRedirect(PieceType type)
    {
        return pieceMoves[type].Count == 0;
    }

    private Move TryMove(PieceType type)
    {
        List<Move> moves = pieceMoves[type];
        return NeedToRedirect(type)? GoToAnotherPiece(): moves[rnd.Next(moves.Count)];
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
using System;
using System.Collections.Generic;

namespace GameModel
{
	
	// Checks if a given move is legal, depending on the piece's capabilities
	// Return boolean value depending on PieceType, departure square, arrival square and power
	static class MoveValidator
	{
		static public bool MoveIsValid(Piece piece, Square fromSqr, Square toSqr, bool power)
		{
			switch(piece.Type)
			{
				case PieceType.Globule:
					if(toSqr.X == (fromSqr.X + 1) || toSqr.Y == (fromSqr.Y + 1)
					    || toSqr.X == (fromSqr.X - 1) || toSqr.Y == (fromSqr.Y - 1)
					    || toSqr.X == (fromSqr.X - 1) && toSqr.Y == (fromSqr.Y - 1)
					    || toSqr.X == (fromSqr.X + 1) && toSqr.Y == (fromSqr.Y + 1)
					    || toSqr.X == (fromSqr.X - 1) && toSqr.Y == (fromSqr.Y + 1)
					    || toSqr.X == (fromSqr.X + 1) && toSqr.Y == (fromSqr.Y - 1))
						return true;
					return false;
				case PieceType.Triglobe:
					if(power)
					{
						if(toSqr.X <= (fromSqr.X + 7) || toSqr.Y <= (fromSqr.Y + 7)
						   || toSqr.X >= (fromSqr.X - 7) || toSqr.Y >= (fromSqr.Y - 7))
							return true;
					}
					else if(toSqr.X == (fromSqr.X + 1) || toSqr.Y == (fromSqr.Y + 1)
					    || toSqr.X == (fromSqr.X - 1) || toSqr.Y == (fromSqr.Y - 1))
						return true;
					return false;
				case PieceType.Triastre:
					if(toSqr.Y == (fromSqr.Y + 1) || toSqr.Y == (fromSqr.Y - 1)
					   || toSqr.Y == (fromSqr.Y + 2) || toSqr.Y == (fromSqr.Y - 2))
						return true;
					return false;
				case PieceType.Tetraglobe:
					if(toSqr.X == (fromSqr.X + 1) || toSqr.Y == (fromSqr.Y + 1)
					   || toSqr.X == (fromSqr.X - 1) || toSqr.Y == (fromSqr.Y - 1))
						return true;
					return false;;
				case PieceType.Tetrastre:
					if(power)
					{
						if(toSqr.X >= (fromSqr.X + 7) || toSqr.X >= (fromSqr.X - 7)
						   || toSqr.Y <= (fromSqr.Y + 7) || toSqr.Y >= (fromSqr.Y - 7)
						   || toSqr.X <= (fromSqr.X + 7) && toSqr.Y <= (fromSqr.Y + 7)
						   || toSqr.X >= (fromSqr.X - 7) && toSqr.Y >= (fromSqr.Y - 7)
						   || toSqr.X <= (fromSqr.X + 7) && toSqr.Y >= (fromSqr.Y - 7)
						   || toSqr.X >= (fromSqr.X - 7) && toSqr.Y <= (fromSqr.Y + 7))
							return true;
					}
					else
						for(int i = 2; i < 8;)
						{
							if(toSqr.X == (fromSqr.X + i) || toSqr.X == (fromSqr.X - i)
							   || toSqr.Y == (fromSqr.Y + i) || toSqr.Y == (fromSqr.Y - i)
							   || toSqr.X == (fromSqr.X + i) && toSqr.Y == (fromSqr.Y + i)
							   || toSqr.X == (fromSqr.X - i) && toSqr.Y == (fromSqr.Y - i)
							   || toSqr.X == (fromSqr.X + i) && toSqr.Y == (fromSqr.Y - i)
							   || toSqr.X == (fromSqr.X - i) && toSqr.Y == (fromSqr.Y + i))
								return true;
							i += 2;
						}
					return false;
				case PieceType.Pentaglobe:
					return true;
				case PieceType.Pentastre:
					if(power)
					{
						if(toSqr.X >= (fromSqr.X + 7) || toSqr.X >= (fromSqr.X - 7)
						   || toSqr.Y <= (fromSqr.Y + 7) || toSqr.Y >= (fromSqr.Y - 7)
						   || toSqr.X <= (fromSqr.X + 7) && toSqr.Y <= (fromSqr.Y + 7)
						   || toSqr.X >= (fromSqr.X - 7) && toSqr.Y >= (fromSqr.Y - 7)
						   || toSqr.X <= (fromSqr.X + 7) && toSqr.Y >= (fromSqr.Y - 7)
						   || toSqr.X >= (fromSqr.X - 7) && toSqr.Y <= (fromSqr.Y + 7))
							return true;
					}
					else if(toSqr.X <= (fromSqr.X + 2) || toSqr.Y <= (fromSqr.Y + 2)
					    || toSqr.X >= (fromSqr.X - 2) || toSqr.Y >= (fromSqr.Y - 2)
					    || toSqr.X == (fromSqr.X + 1) && toSqr.Y == (fromSqr.Y + 1)
					    || toSqr.X == (fromSqr.X - 1) && toSqr.Y == (fromSqr.Y - 1)
					    || toSqr.X == (fromSqr.X - 1) && toSqr.Y == (fromSqr.Y + 1)
					    || toSqr.X == (fromSqr.X + 1) && toSqr.Y == (fromSqr.Y - 1))
						return true;
					return false;
				case PieceType.Rosace:
					if(toSqr.X <= (fromSqr.X + 7) && toSqr.Y <= (fromSqr.Y + 7)
					   || toSqr.X >= (fromSqr.X - 7) && toSqr.Y >= (fromSqr.Y - 7)
					   || toSqr.X <= (fromSqr.X + 7) && toSqr.Y >= (fromSqr.Y - 7)
					   || toSqr.X >= (fromSqr.X - 7) && toSqr.Y <= (fromSqr.Y + 7))
						return true;
					return false;
				case PieceType.Astree:
					if(toSqr.X == (fromSqr.X + 2) || toSqr.Y == (fromSqr.Y + 2)
					   || toSqr.X == (fromSqr.X - 2) || toSqr.Y == (fromSqr.Y - 2)
					   || toSqr.X == (fromSqr.X + 2) && toSqr.Y == (fromSqr.Y + 2)
					   || toSqr.X == (fromSqr.X - 2) && toSqr.Y == (fromSqr.Y - 2)
					   || toSqr.X == (fromSqr.X - 2) && toSqr.Y == (fromSqr.Y + 2)
					   || toSqr.X == (fromSqr.X + 2) && toSqr.Y == (fromSqr.Y - 2))
						return true;
					return false;
				default:
					return false;
			} //endof switch()
		} //endof MoveIsValid()

	} //endof class MoveValidator
} // endof namespace GameModel

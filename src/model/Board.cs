using System;
using System.Linq;
using System.Collections.Generic;

#if DEBUG
using System.Diagnostics; // Only to be used in DEBUG mode
#endif


namespace GameModel
{
	// Contains current state of the pieces in game
	// Also provides all Pieces' interactions
	public class Board
	{
		List<PieceState> m_pieces;
		List<Square> m_triggeredSquares;

		public Board()
		{
			m_pieces = new List<PieceState>();
			m_triggeredSquares = new List<Square>();
		}

		// Return a List of pieceless Squares around a given Square
		public List<Square> GetFreeSquares(Square sqr)
		{
			List<Intersection> intrListBuffer = Intersection.GetBySquare(sqr);
			List<Square> sqrListBuffer = new List<Square>();
			List<Square> freeSqrList = new List<Square>();

			if(intrListBuffer == null)
				Debug.WriteLine("Error in Board.cs, l:41 : decl. l:39. Intersection.GetBySquare(sqr) has returned a null value");

			for(int i = 0; i < intrListBuffer.Count; i++)
			{
				sqrListBuffer = GetFreeSquares(intrListBuffer[i]);

				if(!sqrListBuffer.Any()) // If the list is NOT empty
					for(int j = 0; j < sqrListBuffer.Count; j++)
					{
							freeSqrList.Add(sqrListBuffer[j]);
					}
			}

			if(freeSqrList.Count != 0)
				return freeSqrList;
			else
				return null;
		}

		// Return a List of pieceless Squares around a given Intersection
		public List<Square> GetFreeSquares(Intersection intr)
		{
			List<Square> sqrListBuffer = intr.ToSquares; // Returns all four squares around 'intr'
			List<Square> freeSqrList = new List<Square>();

			PieceState pBuffer; // Will be used to temporarily store a given PieceState, needed to access Squares

			if(sqrListBuffer == null)
				Debug.WriteLine("Error in Board.cs, l:65 : decl. l:63.Intersection.GetBySquare(sqr) has returned a null value");

			for(int i = 0; i < sqrListBuffer.Count; i++)
			{	
				pBuffer = m_pieces.Find(item => item.Square == sqrListBuffer[i]); // Trying to find the given Square in the PieceState list

				if(pBuffer == null) // If the given Square wasn't found in m_pieces (meaning, no piece is on this Square)
				{
					freeSqrList.Add(sqrListBuffer[i]); // Add it to the free Squares list
				}
			}

			if(freeSqrList.Count != 0)
				return freeSqrList;
			else
				return null;
		}

		// Return a List of pieceless Intersections around a given Square
		public List<Intersection> GetFreeIntersections(Square sqr)
		{

			List<Intersection> intrListBuffer = Intersection.GetBySquare(sqr); // Getting all four intersection around 'sqr'
			List<List<Square>> sqrListBuffer = new List<List<Square>>();
			List<Intersection> freeIntrList = new List<Intersection>();
			
			PieceState pBuffer;

			if(intrListBuffer == null)
				Debug.WriteLine("Error in Board.cs, l:91 : decl. l:88. Intersection.GetBySquare(sqr) has returned a null value");

			for(int i = 0; i < intrListBuffer.Count; i++) // Getting four Lists of four Squares
			{
				// Getting the four Squares around the first Intersection, stocking in our Square List buffer
				sqrListBuffer[i] = intrListBuffer[i].ToSquares;
			}

			for(int i = 0; i < sqrListBuffer.Count; i++)
				for(int j = 0; j < sqrListBuffer[i].Count; j++)
				{
					// Trying to find the Square <j> of our Intersection's Squares List <i>
					pBuffer = m_pieces.Find(item => item.Square == sqrListBuffer[i][j]);

					if(pBuffer == null ) // If the given Square wasn't found in m_pieces (meaning, no piece is on this Square)
					{
						freeIntrList.Add(intrListBuffer[i]); // Add it to the free Squares list
					}
				}

			if(freeIntrList.Count != 0)
				return freeIntrList;
			else
				return null;
		}

		// Returns a List of free intersection around a given one, movingPiece is in case we're moving a Tetraglobe
		public List<Intersection> GetFreeIntersections(Intersection intr, bool movingPiece = false)
		{
			List<Intersection> intrListBuffer = new List<Intersection>();
			List<Square> sqrListBuffer = new List<Square>();
			List<Intersection> freeIntrList = new List<Intersection>();
			
			PieceState pBuffer;
			bool TetraglobeMove = false;

			if(intr.A+1 <= 7 && intr.A+1 >= 0)
				intrListBuffer.Add( new Intersection(intr.A+1, intr.B) );
			if(intr.B+1 <= 7 && intr.B+1 >= 0)
				intrListBuffer.Add( new Intersection(intr.A, intr.B+1) );
			if(intr.A-1 <= 7 && intr.A-1 >= 0)	
				intrListBuffer.Add( new Intersection(intr.A-1, intr.B) );
			if(intr.B-1 <= 7 && intr.B-1 >= 0)	
				intrListBuffer.Add( new Intersection(intr.A, intr.B-1) );

			foreach(Intersection intrItem in intrListBuffer)
			{
				foreach(Square sqrItem in intrItem.ToSquares)
				{
					pBuffer = GetPieceState(sqrItem); // Returns null if sqrItem wasn't found in the pieces list

					if(pBuffer == null) // null pBuffer means empty square
						sqrListBuffer.Add(sqrItem);
				}

				// If we're moving a Tetraglobe and the two squares not occupied by movingPiece are free
				if(TetraglobeMove && sqrListBuffer.Count == 2)
					freeIntrList.Add(intrItem);

				// If all four squares are free
				if(sqrListBuffer.Count == 4 || TetraglobeMove && sqrListBuffer.Count == 4)
					freeIntrList.Add(intrItem);
			}

			if(freeIntrList.Count != 0)
				return freeIntrList;
			else
				return null;
		}

		// Place given Piece on given Square
		public void PlacePiece(Piece piece, Square sqr)
		{
			m_pieces.Add(new PieceState(piece, sqr));
		}

		// Place given Piece on given Intersection
		public void PlacePiece(Piece piece, Intersection intr)
		{
				m_pieces.Add(new PieceState(piece, intr));
		}

		// Swap piece1 with piece2 on the board
		public void SwapPiece(Piece piece1, Piece piece2)
		{
			PieceState pieceState1 = GetPieceState(piece1);
			PieceState pieceState2 = GetPieceState(piece2);
			PieceState bufferPiece = pieceState2;
		
			// pieceState2 = pieceState1
			pieceState2.Square = pieceState1.Square;
			pieceState2.Piece = pieceState1.Piece;

			// pieceState1 = pieceState2
			pieceState1.Square = bufferPiece.Square;
			pieceState1.Piece = bufferPiece.Piece;

		}
			
		// Swap piece1 with piece2, in case square is choosen by player (Tetraglobe swap)
		public void SwapPiece(Piece piece1, Piece piece2, Square toSqr, Intersection toIntr)
		{
			if(piece2.Type != PieceType.Tetraglobe)
			{
				SwapPiece(piece1, piece2);
			}
			else
			{
				PieceState pieceState1 = GetPieceState(piece1);
				PieceState pieceState2 = GetPieceState(piece2);
				PieceState bufferPiece = pieceState2;

				// pieceState2 = pieceState1
				pieceState2.Square = toSqr;
				pieceState2.Piece = pieceState1.Piece;

				// pieceState1 = pieceState2
				pieceState1.Intersection = toIntr;
				pieceState1.Piece = bufferPiece.Piece;

			}
		}
			
		// DEBUG: Need to double check, not sure for the Square/Intersection choice
		public void TransformPiece(PieceType type = PieceType.Globule, Intersection toIntr = null, Square toSqr = null, params Piece[] piece)
		{
			PieceState pBuffer = GetPieceState(piece[m_pieces.Count-1]);

			// If we're not transforming Globules to some other piece, we do the contrary
			if(piece[0].Type != PieceType.Globule && type == PieceType.Globule && piece.Length == 1)
			{
				m_pieces[m_pieces.IndexOf(pBuffer)] = new PieceState(new Piece(type, piece[m_pieces.Count-1].Color), pBuffer.Square);
				return;
			}

			if(piece.Length == 1 && toSqr != null)
			{
				if(toSqr.X <= 7 && toSqr.X >= 0 && toSqr.Y <= 7 && toSqr.Y >= 0)
					m_pieces[m_pieces.IndexOf(pBuffer)] = new PieceState(new Piece(type, piece[m_pieces.Count-1].Color), toSqr);
				return;
			}

			if(piece.Length != 1 && toIntr != null)
			{
				List<PieceState> pieceList = new List<PieceState>();

				foreach(Piece item in piece)
				{
					pieceList.Add(GetPieceState(item));
				}

				for(int i = 0; i < pieceList.Count-1; i++)
				{
					m_pieces.Remove(pieceList[i]); // 0, 1, 2
				}

				if(type != PieceType.Tetraglobe)
					m_pieces[m_pieces.IndexOf(pBuffer)] = new PieceState(new Piece(type, piece[m_pieces.Count-1].Color), toSqr);

				if(type == PieceType.Tetraglobe && toIntr.A <= 7 && toIntr.A >= 0 && toIntr.B <= 7 && toIntr.B >= 0)
					m_pieces[m_pieces.IndexOf(pBuffer)] = new PieceState(new Piece(type, piece[m_pieces.Count-1].Color), toIntr);
			}

			return;

		}

		// Move given Piece from Intersection to Intersection
		public void MovePiece(Piece piece, Intersection fromIntr, Intersection toIntr)
		{
			PieceState movedPiece = new PieceState(piece, fromIntr);
			if(m_pieces.Contains(movedPiece))
			{
				m_pieces.Find(item => item.Equals(movedPiece)).Intersection = toIntr;
			}
		}

		// Move given Piece from Square to Square
		public void MovePiece(Piece piece, Square fromSquare, Square toSquare)
		{
			PieceState movedPiece = new PieceState(piece, fromSquare);

			if(m_pieces.Contains(movedPiece))
			{
				m_pieces.Find(item => item.Equals(movedPiece)).Square = toSquare;
			}

		}

		// Remove piece on Square
		public void RemovePiece(Square square)
		{
			PieceState removedPiece;

			for(int i=0; i<m_pieces.Count; ++i)
			{
				removedPiece = m_pieces[i];

				if (removedPiece.Square == square) 
				{
					removedPiece.IsActive = false;
					m_pieces.Remove(removedPiece);
				}
			}
		}

		// Remove piece on Intersection
		public void RemovePiece(Intersection intersection)
		{
			PieceState removedPiece;

			removedPiece = m_pieces.Find(item => item.Intersection == intersection);

			if (m_pieces.Contains(removedPiece)) 
			{
				removedPiece.IsActive = false;
				m_pieces.Remove(removedPiece);
			}
		}

		// Remove piece by Piece
		public void RemovePiece(Piece piece)
		{
			PieceState removedPiece;

			for(int i=0; i<m_pieces.Count; ++i)
			{
				removedPiece = m_pieces[i];

				if (removedPiece.Piece == piece) 
				{
					removedPiece.IsActive = false;
					m_pieces.Remove(removedPiece);
				}
			}
		}

		// Get PieceState by PieceState
		public PieceState GetPieceState(PieceState piece)
		{
			foreach(PieceState item in m_pieces)
			{
				if(item == piece)
					return item;
			}

#if DEBUG
			Debug.WriteLine("GetPieceState(PieceState) : PieceState {0} does not exist in List<PieceState> m_pieces.", piece.ToString());
#endif
			return null;
		}

		// Get PieceState by Piece
		public PieceState GetPieceState(Piece piece)
		{
			foreach(PieceState item in m_pieces)
			{
				if(item.Piece == piece)
				{
#if DEBUG
					Debug.WriteLine("Encore un !");
#endif
					return item;
				}
			}

#if DEBUG
			Debug.WriteLine("GetPieceState(Piece) : No PieceState containing {0} exists in List<PieceState> m_pieces.", piece.ToString());
#endif
			return null;
		}

		// Get PieceState by Square
		public PieceState GetPieceState(Square sqr)
		{
			foreach(PieceState item in m_pieces)
			{
				if(item.Square == sqr)
					return item;
			}

#if DEBUG
			Debug.WriteLine("GetPieceState(Square) : No PieceState containing {0} exists in List<PieceState> m_pieces.", sqr.ToString());
#endif
			return null;
		}

		// Get PieceState by Intersection
		public PieceState GetPieceState(Intersection intr)
		{
			foreach(PieceState item in m_pieces)
			{
				if(item.Intersection == intr)
					return item;		
			}

#if DEBUG
			Debug.WriteLine("GetPieceState(Intersection) : No PieceState containing {0} exists in List<PieceState> m_pieces.", intr.ToString());
#endif
			return null;
		}

		// Get PieceState by X,Y coord
		public PieceState GetPieceState(int x, int y, bool isTetraglobe = false)
		{
			foreach(PieceState item in m_pieces)
			{
				if(isTetraglobe == true)
				{
					if(item.Intersection.A == x && item.Intersection.B == y)
						return item;
				}

				if(item.Square.X == x && item.Square.Y == y)
					return item;
			}

#if DEBUG
			Debug.WriteLine("GetPieceState(int, int, bool) : No PieceState containing Square({0}, {1}) exists in List<PieceState> m_pieces.", x.ToString(), y.ToString());
#endif
			return null;
		}

		// Get triggered square by Square
		public Square GetTriggeredSquare(Square sqr)
		{
			foreach(Square item in m_triggeredSquares)
			{
				if(item.TriggerType != Square.Trigger.None && item.TriggerType == sqr.TriggerType)
					return item;
			}

			return null;
		}

		// Get triggered square by X,Y coordinates
		public Square GetTriggeredSquare(int x, int y)
		{
			foreach(Square item in m_triggeredSquares)
			{
				if(item.X == x && item.Y == y)
					return item;
			}

			return null;
		}
			
		// Get all triggered squares around an intersection
		public List<Square> GetTriggeredSquare(Intersection intr)
		{
			List<Square> triggeredSquares = new List<Square>();

			foreach(Square item in m_triggeredSquares)
			{
				for(int i = 0; i < 4; ++i)
				{
					if(item == intr.ToSquares[i])
						triggeredSquares.Add(intr.ToSquares[i]);
				}
			}

			if(triggeredSquares.Count >= 1)
				return triggeredSquares;
			else
				return null;
		}

		// Create a Globule on given Square
		public void CreateGlobule(Piece piece, Square toSqr)
		{
			PlacePiece(piece, toSqr);
		}

		// Add a triggered Square to the board's list
		public void AddTriggeredSquare(Square newSqr)
		{
			if(newSqr.X <= 7 && newSqr.X >= 0 && newSqr.Y <= 7 && newSqr.Y >= 0)
				m_triggeredSquares.Add(newSqr);
		}

		// Remove a triggered Square from the board's list
		public void RemoveTriggeredSquare(Square removeSqr)
		{
			if (m_triggeredSquares.Contains(removeSqr))
				m_triggeredSquares.Remove(removeSqr);
		}

		/* ACCESSORS */

		public List<PieceState> Pieces
		{
			get
			{
				return m_pieces;
			}
		}

		public List<Square> Triggers
		{
			get
			{
				return m_triggeredSquares;
			}
		}

	} // endof class Board
} // endof namespace GameModel
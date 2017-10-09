using System;
using System.Collections.Generic;

namespace GameModel
{

	//Contains vital informations about a given piece
	// The piece itself, its state (dead or alive) and its current position
	public class PieceState
	{
		Piece m_piece;
		bool m_isActive;
		Square m_square;
		List<Square> m_squareList; //if Tetraglobe
		Intersection m_intersection;

		bool m_hasFreeMove;

		public PieceState()
		{
			m_square = new Square(-1,-1);
			m_intersection = new Intersection(-1,-1);
		}

		public PieceState(Piece piece, Square square)
		{
			m_isActive = true;
			m_hasFreeMove = false;


//			if(piece.Type != PieceType.Empty)
//				m_isActive = true;

			m_piece = piece;
			m_square = square;
		}

		public PieceState(Piece piece, Intersection intr)
		{
			m_isActive = true;
			m_hasFreeMove = false;


//			if(piece.Type != PieceType.Empty)
//				m_isActive = true;

			m_piece = piece;
			m_intersection = intr;
			m_squareList = new List<Square>();

			// HOLY CODE : Defines which squares are actually occupied by the Tetraglobe on intersection 'A,B'
			m_squareList.Add(new Square(intr.A, intr.B) );
			m_squareList.Add( new Square(intr.A, (intr.B+1)) );
			m_squareList.Add( new Square((intr.A+1), intr.B) );
			m_squareList.Add( new Square((intr.A+1), (intr.B+1)) );
		}

		public Piece Piece
		{
			get
			{
				return m_piece;
			}
			set
			{
				m_piece = value;
			}
		}

		public bool IsActive
		{
			get
			{
				return m_isActive;
			}
			set
			{
				m_isActive = value;
			}
		}

		public Square Square
		{
			get
			{
				return m_square;
			}
			set
			{
				m_square = value;
			}
		}

		public Intersection Intersection
		{
			get
			{
				return m_intersection;
			}
			set
			{
				m_intersection = value;
				m_squareList = value.ToSquares;
			}
		}

		public List<Square> SquareArray
		{
			get
			{
				return m_squareList;
			}
			set
			{
				m_squareList = value;
			}
		}

		public bool HasFreeMove
		{
			get {return m_hasFreeMove;}
			set {m_hasFreeMove = value;}
		}

	} //endof class PieceState
} // endof namepsace GameModel
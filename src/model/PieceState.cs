using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;

namespace GameModel
{

	//Contains vital informations about a given piece
	// The piece itself, its state (dead or alive) and its current position
	public class PieceState
	{
		private Square m_square;
		private Intersection m_intersection;
		private List<Square> m_squareArray;
		
		private Piece m_piece;
		
		private bool m_isActive;
		private bool m_hasFreeMove;
		
		
		public PieceState()
		{
			m_square = new Square(-1,-1);
			m_intersection = new Intersection(-1,-1);
		}

		public PieceState(Piece piece, Square square)
		{
			m_isActive = true;
			m_hasFreeMove = false;

			m_piece = piece;
			m_square = square;
		}

		public PieceState(Piece piece, Intersection intr)
		{
			m_isActive = true;
			m_hasFreeMove = false;

			m_piece = piece;
			m_intersection = intr;
			m_squareArray = new List<Square>();

			// HOLY CODE : Defines which squares are actually occupied by the Tetraglobe on intersection 'A,B'
			m_squareArray.Add(new Square(intr.A, intr.B) );
			m_squareArray.Add( new Square(intr.A, intr.B+1) );
			m_squareArray.Add( new Square(intr.A+1, intr.B) );
			m_squareArray.Add( new Square(intr.A+1, intr.B+1) );
		}

		public Piece Piece { get; set; }

		public bool IsActive { get; set; }

		public Square Square { get; set; }

		public Intersection Intersection
		{
			get
			{
				return m_intersection;
			}
			set
			{
				m_intersection = value;
				SquareArray = value.ToSquares;
			}
		}

		public List<Square> SquareArray { get; set; }

		public bool HasFreeMove { get; set; }
	} //endof class PieceState
} // endof namepsace GameModel
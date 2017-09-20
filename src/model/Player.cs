using System;
using System.Collections.Generic;

namespace GameModel
{
	
	// Represents a player, contains the player's color, name and his pieces
	public class Player
	{
		Color m_color;
		List<Piece> m_pieces;
		String m_playerName;

		public Player()
		{
			m_pieces = new List<Piece>();
		}

		public Player(Color color)
		{
			m_color = color;
			m_pieces = new List<Piece>() {
				new Piece(PieceType.Astree, m_color),
				new Piece(PieceType.Astree, m_color),
				new Piece(PieceType.Rosace, m_color),
				new Piece(PieceType.Rosace, m_color),
				new Piece(PieceType.Pentaglobe, m_color),
				new Piece(PieceType.Pentaglobe, m_color),
				new Piece(PieceType.Tetraglobe, m_color),
				new Piece(PieceType.Pentastre, m_color),
				new Piece(PieceType.Pentastre, m_color),
				new Piece(PieceType.Tetrastre, m_color),
				new Piece(PieceType.Tetrastre, m_color)
			};

				for(int i=0;i<=10;++i)
					m_pieces.Add(new Piece(PieceType.Globule, m_color));
		}

		public void RemovePiece(Piece piece)
		{
			m_pieces.Remove(piece);
		}

		public List<Piece> Pieces
		{
			get
			{
				return m_pieces;
			}
			set
			{
				m_pieces = value;
			}
		}

		public Color Color
		{
			get {return m_color;}
			set {m_color = value;}
		}

		public String Name
		{
			get {return m_playerName;}
			set {m_playerName = value;}
		}
	} // endof class Player
} // endof namespace GameModel
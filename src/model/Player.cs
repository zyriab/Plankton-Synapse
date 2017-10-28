using System.Collections.Generic;

// Represents a player, contains the player's color, name and his pieces
namespace GameModel
{	
	public class Player
	{
		public Player()
		{
			Pieces = new List<Piece>();
		}

		public Player(Color color)
		{
			Color = color;
			Pieces = new List<Piece>
			{
				new Piece(PieceType.Astree, Color),
				new Piece(PieceType.Astree, Color),
				new Piece(PieceType.Rosace, Color),
				new Piece(PieceType.Rosace, Color),
				new Piece(PieceType.Pentaglobe, Color),
				new Piece(PieceType.Pentaglobe, Color),
				new Piece(PieceType.Tetraglobe, Color),
				new Piece(PieceType.Pentastre, Color),
				new Piece(PieceType.Pentastre, Color),
				new Piece(PieceType.Tetrastre, Color),
				new Piece(PieceType.Tetrastre, Color)
			};

				for(int i=0;i<=10;++i)
					Pieces.Add(new Piece(PieceType.Globule, Color));
		}

		public void RemovePiece(Piece piece)
		{
			Pieces.Remove(piece);
		}

		public List<Piece> Pieces { get; set; }
		public Color Color { get; set; }
		public string Name { get; set; }
		
	} // endof class Player
} // endof namespace GameModel
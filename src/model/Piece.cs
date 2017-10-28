namespace GameModel
{
	// Represents a piece, contains its type and color
	public class Piece
	{
		private PieceType m_type;
		private Color m_color;

		// Will be used to count the number of moves a given piece has performed
		// This will be reseted every turn
		private int m_moveCounter = 0;

		public Piece(PieceType type, Color color)
		{
			m_type = type;
			m_color = color;
		}

		// Example string: "White Globule"
		public override string ToString()
		{
			return string.Format("{0} {1}", GetString.GetStr(m_color), GetString.GetStr(m_type));
		}

		public PieceType Type
		{
			get
			{
				return m_type;
			}
			set
			{
				m_type = value;
			}
		}

		public Color Color
		{
			get
			{
				return m_color;
			}
			set
			{
				m_color = value;
			}
		}

	} // endof class Piece
} // endof namespace GameModel
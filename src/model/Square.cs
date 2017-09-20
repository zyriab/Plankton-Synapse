using System;
using System.Collections.Generic;

namespace GameModel
{

	// Represents a square on the board
	// It only contains its own coordinates and can be triggered
	public class Square
	{
		int m_x;
		int m_y;
		Trigger m_trigger;
		Color m_triggerColor;

		public enum Trigger {None, Stop, Shield};

		public Square()
		{
		}

		public Square(int x, int y, Trigger trigger = Trigger.None)
		{
			m_x = x;
			m_y = y;
			m_trigger = trigger;
		}

		// Gets all four Squares around a given Intersection
		public static List<Square> GetByIntersection(Intersection intr)
		{
			List<Square> sqrList = new List<Square>();

				sqrList.Add( new Square(intr.A, intr.B) );
			if(intr.B+1 <= 7 && intr.B+1 >= 0)
				sqrList.Add( new Square(intr.A, intr.B+1) );
			if(intr.A+1 <= 7 && intr.A+1 >= 0)
				sqrList.Add( new Square(intr.A+1, intr.B) );
			if(intr.A+1 <= 7 && intr.A+1 >= 0 && intr.B+1 <= 7 && intr.B+1 >= 0)
				sqrList.Add( new Square(intr.A+1, intr.B+1) );

			if(sqrList.Count != 0)
				return sqrList;
			else
				return null;
		}

		// Returns the Squares on top, bottom, left and right of 'sqr'
		// If 'diagonal' is true, returns as well top-left, bottom-left, top-right, bottom-right
		public static List<Square> GetSquaresAround(Square sqr, bool diagonal = false)
		{
			List<Square> sqrAround = new List<Square>();

			if(sqr.Y+1 <= 7 && sqr.Y+1 >= 0) // top
				sqrAround.Add( new Square(sqr.X, sqr.Y+1) );
			if(sqr.Y-1 <= 7 && sqr.Y-1 >= 0) // bottom
				sqrAround.Add( new Square(sqr.X, sqr.Y-1) );
			if(sqr.X-1 <= 7 && sqr.X-1 >= 0) // left
				sqrAround.Add( new Square(sqr.X-1, sqr.Y) );
			if(sqr.X+1 <= 7 && sqr.X+1 >= 0) // right
				sqrAround.Add( new Square(sqr.X-1, sqr.Y) );

			if(diagonal)
			{
				if(sqr.X-1 <= 7 && sqr.X-1 >= 0 && sqr.Y+1 <= 7 && sqr.Y+1 >= 0) // top-left
					sqrAround.Add( new Square(sqr.X-1, sqr.Y+1) );
				if(sqr.X-1 <= 7 && sqr.X-1 >= 0 && sqr.Y-1 <= 7 && sqr.Y-1 >= 0) // bottom-left
					sqrAround.Add( new Square(sqr.X-1, sqr.Y-1) );
				if(sqr.X+1 <= 7 && sqr.X+1 >= 0 && sqr.Y+1 <= 7 && sqr.Y+1 >= 0) // top-right
					sqrAround.Add( new Square(sqr.X+1, sqr.Y+1) );
				if(sqr.X+1 <= 7 && sqr.X+1 >= 0 && sqr.Y-1 <= 7 && sqr.Y-1 >= 0) // bottom-right
					sqrAround.Add( new Square(sqr.X+1, sqr.Y+1-1) );
			}

			if(sqrAround.Count != 0)
				return sqrAround;
			else
				return null;

		}

		// Example string: "A,G"
		// if triggered: "A,G [Square effect]: Shield"
		public override string ToString()
		{
			// Convert int to char litteral, usic ASCII decimal value (0+65=A, 1+65=B, etc)
			string strX = Convert.ToString(Convert.ToChar(m_x + 65));
			string strY = Convert.ToString(Convert.ToChar(m_y + 65));

			if(this.TriggerType != Square.Trigger.None)
				return string.Format("{0},{1} [Square effect]: {2}", strX, strY, GetString.GetStr(m_trigger));
			else
				return string.Format("{0},{1}", strX, strY);
		}

		public bool isTriggered()
		{
			if(m_trigger != Trigger.None)
				return true;
			else
				return false;
		}

		public bool isDangerous()
		{
			if(m_trigger == Trigger.Stop)
				return true;
			else
				return false;
		}

		public bool isProtected()
		{
			if(m_trigger == Trigger.Shield)
				return true;
			else
				return false;
		}

		public int X
		{
			get
			{
				return m_x;
			}
			set
			{
				m_x = value;
			}
		}

		public int Y
		{
			get
			{
				return m_y;
			}
			set
			{
				m_y = value;
			}
		}

		public Trigger TriggerType
		{
			get
			{
				return m_trigger;
			}
			set
			{
				m_trigger = value;
			}
		}

		public Color TriggerColor
		{
			get { return m_triggerColor; }
			set { m_triggerColor = value; }
		}

		// Returns all four intersections sharing 'this' Square -- like a frame around 'this' Square
		public List<Intersection> IntersectionsAround
		{
			get
			{ 
				List<Intersection> intersectionsCoord = new List<Intersection>();

				intersectionsCoord.Add( new Intersection(X, Y) );
				if(Y-1 <= 7 && Y-1 >= 0)
					intersectionsCoord.Add( new Intersection(X, (Y-1) );
				if(X-1 <= 7 && X-1 >= 0)
					intersectionsCoord.Add( new Intersection((X-1), Y) );
				if(X-1 <= 7 && X-1 >= 0 && Y-1 <= 7 && Y-1 >= 0)
					intersectionsCoord.Add( new Intersection((X-1), (Y-1)) );

				if(intersectionsCoord.Count != 0)
					return intersectionsCoord;
				else
					return null;
			}
		}

		public static Square operator+(Square sqr1, Square sqr2)
		{
			Square sqr3 = new Square();

			sqr3.X += sqr1.X;
			sqr3.X += sqr2.X;
			sqr3.Y += sqr1.Y;
			sqr3.Y += sqr2.Y;

			return sqr3;
		}

		public override bool Equals(System.Object obj)
		{
			if(obj == null)
				return false;

			Square sqr = obj as Square;

			if((Object)sqr == null)
				return false;

			return (this.X == sqr.X) && (this.Y == sqr.Y);
		}

		public override int GetHashCode()
		{
			throw new System.NotImplementedException ();
		}

		public static bool operator==(Square sqr1, Square sqr2)
		{
			if(sqr1.Equals(sqr2))
				return true;
			else
				return false;
		}

		public static bool operator!=(Square sqr1, Square sqr2)
		{
			return !(sqr1 == sqr2);
		}

	} // endof class Square
} //endof namespace GameModel
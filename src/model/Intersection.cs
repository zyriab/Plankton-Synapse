using System;
using System.Collections.Generic;

namespace GameModel
{

// Represents an intersection between 4 squares
	// This class will be used to position the Tetraglobe
	// Coordinates are not represented as in an array (0,1;1,1;1,2;...)
	// But using alphabet (a = between 1 & 2, b = 2 & 3, ...)
	public class Intersection
	{
		int m_a;
		int m_b;

		public Intersection()
		{
		}

		public Intersection(int a, int b)
		{
			m_a = a;
			m_b = b;
		}

		// Gets all intersections around a given Square
		public static List<Intersection> GetBySquare(Square sqr)
		{
			List<Intersection> intrList = new List<Intersection>();

			intrList.Add( new Intersection(sqr.X, sqr.Y) );
			if((sqr.Y-1) <= 7 && (sqr.Y-1) >= 0)
				intrList.Add( new Intersection(sqr.X, (sqr.Y-1)) );
			if((sqr.X-1) <= 7 && (sqr.X-1) >= 0)
				intrList.Add( new Intersection((sqr.X-1), sqr.Y) );
			if((sqr.X-1) <= 7 && (sqr.X-1) >= 0 && (sqr.Y-1) <= 7 && (sqr.Y-1) >= 0)
				intrList.Add( new Intersection((sqr.X-1), (sqr.Y-1)) );

			if(intrList.Count != 0)
				return intrList;
			else
				return null;
		}

		public static List<Intersection> GetIntersectionsAround(Intersection intr, bool diagonal = false)
		{
			List<Intersection> intrAround = new List<Intersection>();

			if(intr.B+1 <= 7 && intr.B+1 >= 0) // top
				intrAround.Add( new Intersection(intr.A, intr.B+1) );
			if(intr.B-1 <= 7 && intr.B-1 >= 0) // bottom
				intrAround.Add( new Intersection(intr.A, intr.B-1) );
			if(intr.A-1 <= 7 && intr.A-1 >= 0) // left
				intrAround.Add( new Intersection(intr.A-1, intr.B) );
			if(intr.A+1 <= 7 && intr.A+1 >= 0) // right
				intrAround.Add( new Intersection(intr.A+1, intr.B) );

			if(diagonal)
			{
				if(intr.A-1 <= 7 && intr.A-1 >= 0 && intr.B+1 <= 7 && intr.B+1 >= 0) // top-left
					intrAround.Add( new Intersection(intr.A-1, intr.B+1) );
				if(intr.A-1 <= 7 && intr.A-1 >= 0 && intr.B-1 <= 7 && intr.B-1 >= 0) // bottom-left
					intrAround.Add( new Intersection(intr.A-1, intr.B-1) );
				if(intr.A+1 <= 7 && intr.A+1 >= 0 && intr.B+1 <= 7 && intr.B+1 >= 0) // top-right
					intrAround.Add( new Intersection(intr.A+1, intr.B+1) );
				if(intr.A+1 <= 7 && intr.A+1 >= 0 && intr.B-1 <= 7 && intr.B-1 >= 0) // bottom-right
					intrAround.Add( new Intersection(intr.A+1, intr.B+1-1) );
			}

			if(intrAround.Count != 0)
				return intrAround;
			else
				return null;

		}

		// Example string: "1,4"
		public override string ToString()
		{
			return string.Format("{0},{1}", A, B);
		}

		public int A
		{
			get
			{
				return m_a;
			}
			set
			{
				m_a = value;
			}
		}

		public int B
		{
			get
			{
				return m_b;
			}
			set
			{
				m_b = value;
			}
		}

		public List<Square> ToSquares
		{
			get
			{
				List<Square> squareCoord = new List<Square>();

				squareCoord.Add( new Square(A, B) );
				if(B+1 <= 7 && B+1 >= 0)
					squareCoord.Add( new Square(A, (B + 1)) );
				if(A+1 <= 7 && A+1 >= 0)
					squareCoord.Add( new Square((A+1), B) );
				if(A+1 <= 7 && A+1 >= 0 && B+1 <= 7 && B+1 >= 0)
					squareCoord.Add( new Square((A+1), (B + 1)) );

				if(squareCoord.Count == 4) // Intersection cannot have less than four Squares
					return squareCoord;
				else
					return null;
			}
		}

		public List<Square> this[int index]
		{
			get
			{
				List<Square> squareCoord = this.ToSquares;

				if(squareCoord != null)
					return squareCoord;
				else
					return null;
			}
		}

	} // endof class Intersection
} // endof namespace GameModel
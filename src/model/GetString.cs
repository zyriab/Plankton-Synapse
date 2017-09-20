using System;


namespace GameModel
{

	// Utility class, returns string for given enum !
	public static class GetString
	{
		public static string GetStr(PieceType type)
		{
			switch(type)
			{
				case PieceType.Astree:
					return "Astrée";
				case PieceType.Globule:
					return "Globule";
				case PieceType.Pentaglobe:
					return "Pentaglobe";
				case PieceType.Pentastre:
					return "Pentastre";
				case PieceType.Rosace:
					return "Rosace";
				case PieceType.Tetraglobe:
					return "Tetraglobe";
				case PieceType.Tetrastre:
					return "Tetrastre";
				case PieceType.Triastre:
					return "Triastre";
				case PieceType.Triglobe:
					return "Triglobe";
				
				default :
					return " ";	
			}
		}

		public static string GetStr(Color color)
		{
			switch(color)
			{
				case Color.Black:
					return "Teal";
				case Color.White:
					return "White";

				default:
					return " ";	
			}
		}

		public static string GetStr(Square.Trigger trigger)
		{
			switch(trigger)
			{
				case Square.Trigger.Shield:
					return "Shield";
				case Square.Trigger.Stop:
					return "Stop";

				default:
					return " ";
			}
		}

	} // endof class GetString
} // endof namespace GameModel

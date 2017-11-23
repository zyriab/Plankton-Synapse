using UnityEngine;

/* SINGLETON */
// This class interface between the view and presenter
// The purpose of this class is to give all necesarry accesses to the view from the presenter
namespace GameView
{
	public class GameDrawer : MonoBehaviour
	{
		[SerializeField] private Board m_board;
		// ReSharper disable once InconsistentNaming
		[SerializeField] private GameObject m_squares;
		
		public void SetupBoard()
		{
			//Black side
			m_board.PlacePiece(new GameModel.Piece(PieceType.Astree, Color.Black), new GameModel.Square(0,0));
			m_board.PlacePiece(new GameModel.Piece(PieceType.Astree, Color.Black), new GameModel.Square(0,7));
			m_board.PlacePiece(new GameModel.Piece(PieceType.Rosace, Color.Black), new GameModel.Square(0,1));
			m_board.PlacePiece(new GameModel.Piece(PieceType.Rosace, Color.Black), new GameModel.Square(0,6));
			m_board.PlacePiece(new GameModel.Piece(PieceType.Pentaglobe, Color.Black), new GameModel.Square(0,2));
			m_board.PlacePiece(new GameModel.Piece(PieceType.Pentaglobe, Color.Black), new GameModel.Square(0,5));
			m_board.PlacePiece(new GameModel.Piece(PieceType.Tetraglobe, Color.Black), new GameModel.Intersection(0,3)); //Will occupy  GameModel.Squares : 0,3 - 0,4 - 1,3 - 1,4
			m_board.PlacePiece(new GameModel.Piece(PieceType.Globule, Color.Black), new GameModel.Square(1,0));
			m_board.PlacePiece(new GameModel.Piece(PieceType.Globule, Color.Black), new GameModel.Square(1,7));
			m_board.PlacePiece(new GameModel.Piece(PieceType.Pentastre, Color.Black), new GameModel.Square(1,1));
			m_board.PlacePiece(new GameModel.Piece(PieceType.Pentastre, Color.Black), new GameModel.Square(1,6));
			m_board.PlacePiece(new GameModel.Piece(PieceType.Tetrastre, Color.Black), new GameModel.Square(1,2));
			m_board.PlacePiece(new GameModel.Piece(PieceType.Tetrastre, Color.Black), new GameModel.Square(1,5));
			for(int i=0;i<8;i++)
				m_board.PlacePiece(new GameModel.Piece(PieceType.Globule, Color.Black), new GameModel.Square(2,i));

			//White Side
			m_board.PlacePiece(new GameModel.Piece(PieceType.Astree, Color.White), new GameModel.Square(7,0));
			m_board.PlacePiece(new GameModel.Piece(PieceType.Astree, Color.White), new GameModel.Square(7,7));
			m_board.PlacePiece(new GameModel.Piece(PieceType.Rosace, Color.White), new GameModel.Square(7,1));
			m_board.PlacePiece(new GameModel.Piece(PieceType.Rosace, Color.White), new GameModel.Square(7,6));
			m_board.PlacePiece(new GameModel.Piece(PieceType.Pentaglobe, Color.White), new GameModel.Square(7,2));
			m_board.PlacePiece(new GameModel.Piece(PieceType.Pentaglobe, Color.White), new GameModel.Square(7,5));
			m_board.PlacePiece(new GameModel.Piece(PieceType.Tetraglobe, Color.White), new GameModel.Intersection(6,3)); //Will occupy  GameModel.Squares : 6,3 - 6,4 - 7,3 - 7,4
			m_board.PlacePiece(new GameModel.Piece(PieceType.Globule, Color.White), new GameModel.Square(6,0));
			m_board.PlacePiece(new GameModel.Piece(PieceType.Globule, Color.White), new GameModel.Square(6,7));
			m_board.PlacePiece(new GameModel.Piece(PieceType.Pentastre, Color.White), new GameModel.Square(6,1));
			m_board.PlacePiece(new GameModel.Piece(PieceType.Pentastre, Color.White), new GameModel.Square(6,6));
			m_board.PlacePiece(new GameModel.Piece(PieceType.Tetrastre, Color.White), new GameModel.Square(6,2));
			m_board.PlacePiece(new GameModel.Piece(PieceType.Tetrastre, Color.White), new GameModel.Square(6,5));
			for(int i=0;i<8;i++)
				m_board.PlacePiece(new GameModel.Piece(PieceType.Globule, Color.White), new GameModel.Square(5,i));
		}
	
		/* UNITY METHODS */
		
		// Use this for initialization
		private void Start()
		{
			m_board = GetComponent<Board>();
			SetupBoard();
		}
		
		/* ACCESSORS */
		
		public Board Board { get; set; }
		// ReSharper disable once InconsistentNaming
		public UIManager UIManager { get; set; }
		public GameObject Squares { get; set; }
		
	} // endof class GameManager
} // endof namespace GameView
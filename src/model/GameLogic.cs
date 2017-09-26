using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public enum PieceType {Globule, Triglobe, Triastre, Tetraglobe, Tetrastre, Pentaglobe, Pentastre, Rosace, Astree, None};
public enum ActionType {Move, Create, Transform, Swap, DeleteThis, DeleteLastSwapped, FreeMove, SkipTurn};
public enum Color {Black, White};
public enum GameState {InPlay, CheckMate, Pat};

	
	// TODO:
	// (Prsntr?)- Implement 'JumpPiece()' and automatic jump managing
	// (Prsntr?)- Implement Game Over, etc
	// (Prsntr) - Implement 'StopPiece()' for trigger // Maybe StopPiece() and ProtectPiece() will be live managed by the board ?
	// (Prsntr) - Implement 'ProtectPiece()' for trigger
	// (Prsntr)	- Implement a move counter? How to know if that unique move has already been done? (i.e. Jumping when not powerful)
	//          - Implement Apoptose
	//          - Implement Rosace's second move
	// (Prsntr?)- Implement ErrorLog & GameLog (GameLog = Current moves summary)
	// 			- Implement free moves and pass free moves !
	// 		    - Implement 'CanDoAnotherMove()' (Globule, TTG, PTG) 
	// (Prsntr) - Implement "if Triastre is eaten, change eater to Globule"
	//			- Implement proper action system
	// (CANCEL) - Implement undo/redo capabilities

namespace GameModel
{
	
	// Game Logic engine, used to ensure a move stays within the board boundaries
	// Then calling class MoveValidator to get the legal moves depending on PieceType
	// At last, checking the destination
	// Get the current gameState and also sets up the board
	// This class and NO OTHER makes use of class MoveValidator

	public class GameLogic
	{
		Board m_board; // Reference to actual board
		GameState m_gameState; // Reference to actual game state (check, pat, etc)

		Stack<Action> m_ActionStack; // Used to undo actions and to access them
		Action m_redoAction; // Used to redo last undoed action

		List<List<PieceState>> m_compareStateList; // Used to compare with actual board. For PAT
		int m_lastIndex, m_samePositions; // Used for PAT purpose

		public GameLogic(Board board)
		{
			m_board = board;
			m_ActionStack = new Stack<Action>();
			m_compareStateList = new List<List<PieceState>>();
			m_lastIndex = 0;
			m_samePositions = 0;
		}

		public void SetupBoard()
		{
			//Black side
			m_board.PlacePiece(new Piece(PieceType.Astree, Color.Black), new Square(0,0));
			m_board.PlacePiece(new Piece(PieceType.Astree, Color.Black), new Square(0,7));
			m_board.PlacePiece(new Piece(PieceType.Rosace, Color.Black), new Square(0,1));
			m_board.PlacePiece(new Piece(PieceType.Rosace, Color.Black), new Square(0,6));
			m_board.PlacePiece(new Piece(PieceType.Pentaglobe, Color.Black), new Square(0,2));
			m_board.PlacePiece(new Piece(PieceType.Pentaglobe, Color.Black), new Square(0,5));
			m_board.PlacePiece(new Piece(PieceType.Tetraglobe, Color.Black), new Intersection(0,3)); //Will occupy Squares : 0,3 - 0,4 - 1,3 - 1,4
			m_board.PlacePiece(new Piece(PieceType.Globule, Color.Black), new Square(1,0));
			m_board.PlacePiece(new Piece(PieceType.Globule, Color.Black), new Square(1,7));
			m_board.PlacePiece(new Piece(PieceType.Pentastre, Color.Black), new Square(1,1));
			m_board.PlacePiece(new Piece(PieceType.Pentastre, Color.Black), new Square(1,6));
			m_board.PlacePiece(new Piece(PieceType.Tetrastre, Color.Black), new Square(1,2));
			m_board.PlacePiece(new Piece(PieceType.Tetrastre, Color.Black), new Square(1,5));
			for(int i=0;i<8;i++)
				m_board.PlacePiece(new Piece(PieceType.Globule, Color.Black), new Square(2,i));

			//White Side
			m_board.PlacePiece(new Piece(PieceType.Astree, Color.White), new Square(7,0));
			m_board.PlacePiece(new Piece(PieceType.Astree, Color.White), new Square(7,7));
			m_board.PlacePiece(new Piece(PieceType.Rosace, Color.White), new Square(7,1));
			m_board.PlacePiece(new Piece(PieceType.Rosace, Color.White), new Square(7,6));
			m_board.PlacePiece(new Piece(PieceType.Pentaglobe, Color.White), new Square(7,2));
			m_board.PlacePiece(new Piece(PieceType.Pentaglobe, Color.White), new Square(7,5));
			m_board.PlacePiece(new Piece(PieceType.Tetraglobe, Color.White), new Intersection(6,3)); //Will occupy Squares : 6,3 - 6,4 - 7,3 - 7,4
			m_board.PlacePiece(new Piece(PieceType.Globule, Color.White), new Square(6,0));
			m_board.PlacePiece(new Piece(PieceType.Globule, Color.White), new Square(6,7));
			m_board.PlacePiece(new Piece(PieceType.Pentastre, Color.White), new Square(6,1));
			m_board.PlacePiece(new Piece(PieceType.Pentastre, Color.White), new Square(6,6));
			m_board.PlacePiece(new Piece(PieceType.Tetrastre, Color.White), new Square(6,2));
			m_board.PlacePiece(new Piece(PieceType.Tetrastre, Color.White), new Square(6,5));
			for(int i=0;i<8;i++)
				m_board.PlacePiece(new Piece(PieceType.Globule, Color.White), new Square(5,i));

			m_gameState = GameState.InPlay;

		}

		// FIXME: GameOver (check/check-mate) not implemented? Prsntr?
		public GameState GetGameState()
		{
			List<PieceState> actualPiecesStates = new List<PieceState>(m_board.Pieces); //Copying actual pieces states

			// Checking for Pat
			for(int i=m_lastIndex;i<m_board.Pieces.Count;i++)
			{
				//if the actual pieces positions are the same, increment (int)samePositions
				if(actualPiecesStates.Equals(m_compareStateList[i]))
					++m_samePositions;

				m_lastIndex = i;
			}

			if(m_samePositions >= 3)
				m_gameState = GameState.Pat;
			

			return m_gameState;
		}

		// Returns a boolean value based on what is whithin the range of the desired transformation
		// 'wantedPieceType' is mostly used to get a valid return value for a transformation to Tetraglobe
		public bool CanTransform(Piece piece, PieceType wantedPieceType = PieceType.None)
		{
			if(piece.Type != PieceType.Globule)
				return false;

			PieceState pieceState = m_board.GetPieceState(piece);

			List<PieceState> pieceList = new List<PieceState>();
			List<Square> sqrAround = new List<Square>(); 
			Square pieceSquare = pieceState.Square;

			PieceState pBuffer = m_board.GetPieceState(piece);

			if(wantedPieceType == PieceType.Globule)
				return false;

			if(wantedPieceType == PieceType.None || wantedPieceType == PieceType.Triglobe
			|| wantedPieceType == PieceType.Triastre)
			{
				sqrAround = Square.GetSquaresAround(pieceSquare); // Get squares 

				for(int i = 0; i < pieceList.Count; i++)
				{
					pieceList.Add(m_board.GetPieceState(sqrAround[i]));

					// If there's at least one globule next to 'piece'
					if(pieceList[i].Piece.Type == PieceType.Globule && wantedPieceType == PieceType.None
					&& pieceList[i].Piece.Color == pBuffer.Piece.Color
					|| pieceList[i].Piece.Type == PieceType.Globule && wantedPieceType == PieceType.Triglobe
					&& pieceList[i].Piece.Color == pBuffer.Piece.Color
					|| pieceList[i].Piece.Type == PieceType.Globule && wantedPieceType == PieceType.Triastre
					&& pieceList[i].Piece.Color == pBuffer.Piece.Color)
						return true;
				}

			}
			
			else if(wantedPieceType != PieceType.None || wantedPieceType != PieceType.Triglobe
			|| wantedPieceType != PieceType.Triastre)
			{
				List<Intersection> intrAround = pieceSquare.IntersectionsAround;
				
				foreach(Intersection intrItem in intrAround)
				{
					sqrAround = intrItem.ToSquares;
					int globulesOnIntr = 0;

					for(int i = 0; i < sqrAround.Count; i++)
					{
						pieceList.Add(m_board.GetPieceState(sqrAround[i]));

						// We need minimum 4 Globules, somewhere, making a 2x2 square with 'piece'
						if(pBuffer.Piece.Type == PieceType.Globule && pieceList[i].Piece.Color == pBuffer.Piece.Color && pieceList.Count == 4)
							globulesOnIntr++;
					}

					if(globulesOnIntr == 4)
						return true;
				}

			}

			return false;
		}
		
		// Returns a list of possible actions for a given piece
		public List<ActionType> GetPossibleActions(PieceState pieceState, bool power = false)
		{
			List<ActionType> ActionList = new List<ActionType>();
			List<Square> sqrAround = new List<Square>();
			List<Intersection> intrAround = new List<Intersection>();

			bool nextToEnemy = false;

			if(pieceState.Piece.Type != PieceType.Astree)
				ActionList.Add(ActionType.DeleteThis);

			// If the move is a free move, possibility to skip it
			if(pieceState.HasFreeMove)
				ActionList.Add(ActionType.SkipTurn);

			// Return possible actions for a given Globule
			if(pieceState.Piece.Type == PieceType.Globule)
			{
				sqrAround.Clear();

				// First checking if any Square around is free
				sqrAround = m_board.GetFreeSquares(pieceState.Square);
				if(!sqrAround.Any()) // If sqrAround is NOT empty - Meaning there's free Square(s) around
					ActionList.Add(ActionType.Create); // Add the creating action to the possible actions list
				if(this.CanTransform(pieceState.Piece))
					ActionList.Add(ActionType.Transform);
				// If the piece is powerful and last action has been made by an enemy
				if(power && m_ActionStack.Peek().PieceState.Piece.Color != pieceState.Piece.Color)
				{
					// Giving all allied (and this one) Globules a free move, letting the player chose which one he'll play 
					foreach(PieceState pState in m_board.Pieces)
					{
						if(pState.Piece.Type == PieceType.Globule)
							pState.HasFreeMove = true;
					}
				}
			} // endof Globule

			else if(pieceState.Piece.Type == PieceType.Triglobe || pieceState.Piece.Type == PieceType.Triastre)
			{
				sqrAround.Clear();

				sqrAround = Square.GetSquaresAround(pieceState.Square);

				foreach(Square sqr in sqrAround)
					if(m_board.GetPieceState(sqr).Piece.Color != pieceState.Piece.Color)
						nextToEnemy = true;

				if(sqrAround.Count != 0) // If there's free Squares around
					ActionList.Add(ActionType.Move);
				if(nextToEnemy)
					ActionList.Add(ActionType.Move);
			} // endof Triglobe || Triastre

			else if(pieceState.Piece.Type == PieceType.Tetraglobe)
			{
				intrAround.Clear();

				intrAround = Intersection.GetIntersectionsAround(pieceState.Intersection);

				List<Square> _buffPossibleSqrList = new List<Square>();

				List<Square> _buffSqrList;
				PieceState _bufferPiece; 
				PieceState _buffNextPiece;

				int currentIndex;

				foreach(Intersection item in intrAround)
				{
					_buffSqrList = Square.GetByIntersection(item);
					_bufferPiece = m_board.GetPieceState(item);

					if(_bufferPiece.Piece.Type == PieceType.Tetraglobe && _bufferPiece.Piece.Color != pieceState.Piece.Color)
						ActionList.Add(ActionType.Move);

					foreach(Square sqrItem in _buffSqrList)
					{
						_bufferPiece = m_board.GetPieceState(sqrItem);

						if(_bufferPiece != pieceState && _bufferPiece.Piece.Color != pieceState.Piece.Color
						|| _bufferPiece == null)
						_buffPossibleSqrList.Add(sqrItem);
					}

					foreach(Square sqrItem in _buffPossibleSqrList)
					{
						_bufferPiece = m_board.GetPieceState(sqrItem);
						currentIndex = m_board.Pieces.IndexOf(_bufferPiece);

						for(int i = 0; i < _buffPossibleSqrList.Count; i++)
						{
							if(_buffPossibleSqrList[i] == sqrItem)
								break;

							_buffNextPiece = m_board.Pieces[currentIndex+i];

							if(_buffNextPiece.Square.X == _bufferPiece.Square.X+1)
								ActionList.Add(ActionType.Move);								

							if(_buffNextPiece.Square.X == _bufferPiece.Square.X-1)
								ActionList.Add(ActionType.Move);

							if(_buffNextPiece.Square.Y == _bufferPiece.Square.Y+1)
								ActionList.Add(ActionType.Move);

							if(_buffNextPiece.Square.Y == _bufferPiece.Square.Y-1)
								ActionList.Add(ActionType.Move);

							
						}

						if(_bufferPiece == null) // If the Square is empty
							ActionList.Add(ActionType.Move);
						else if(_bufferPiece.Piece.Color != pieceState.Piece.Color)
							ActionList.Add(ActionType.Move);
					}
					
				}
			} // endof Tetraglobe

			else if(pieceState.Piece.Type == PieceType.Tetrastre)
			{
				sqrAround.Clear();

				sqrAround = Square.GetSquaresAround(pieceState.Square, true);

				PieceState _bufferPiece;

				foreach(Square item in sqrAround)
				{
					// First, we change the range, since the Tetrastre moves by EVEN numbers of squares ;)
					item.X+=1;
					item.Y+=1;

					_bufferPiece = m_board.GetPieceState(item);

					if(!m_board.Pieces.Contains(_bufferPiece)) // If that Square IS EMPTY
					{
						ActionList.Add(ActionType.Move);
						break;
					}
				}
			} // endof Tetrastre

			else if(pieceState.Piece.Type == PieceType.Pentaglobe)
			{
				Action _actionBuff;


				if(power)
				{
					_actionBuff = m_ActionStack.Peek();

					// If the Pentaglobe is powerful and has just swapped position with an allied Globule
					if(_actionBuff.PieceState == pieceState && _actionBuff.TargetState.Piece.Type == PieceType.Globule
					&& _actionBuff.TargetState.Piece.Color == pieceState.Piece.Color)
						ActionList.Add(ActionType.DeleteLastSwapped);

				}

				foreach(PieceState item in m_board.Pieces)
				{
					// If there's any ally other than Pentaglobe on the board
					if(item.Piece.Type != PieceType.Pentaglobe && item.Piece.Color == pieceState.Piece.Color)
					{
						ActionList.Add(ActionType.Swap);
						break;
					}
				}
			} // endof Pentaglobe

			else if(pieceState.Piece.Type == PieceType.Pentastre)
			{
				sqrAround.Clear();

				sqrAround = Square.GetSquaresAround(pieceState.Square, true);

				PieceState _bufferPiece;

				foreach(Square item in sqrAround)
				{
					_bufferPiece = m_board.GetPieceState(item);

					// If a Square next to us is EMPTY - OR - if we're powerful and that Square is occupied by an enemy Pentastre
					if(_bufferPiece == null
					|| power && _bufferPiece.Piece.Type == PieceType.Pentastre && _bufferPiece.Piece.Color != pieceState.Piece.Color)
					{
						ActionList.Add(ActionType.Move);
					}
				}
			} // endof Pentastre

			else if(pieceState.Piece.Type == PieceType.Rosace)
			{
				sqrAround.Clear();

				sqrAround = Square.GetSquaresAround(pieceState.Square, true);
			
				List<Square> _sqrToRemove = new List<Square>();

				PieceState _bufferPiece;
				Square actualSqr = pieceState.Square;

				// First, removing all "illegal-to-move-on" Squares
				foreach(Square item in sqrAround)
				{
					// Since the Rosace can only move diagonally, we're removing the ortogonal Squares
					if(item.X == actualSqr.X+1 && item.Y == actualSqr.Y // right
					|| item.X == actualSqr.X-1 && item.Y == actualSqr.Y // left
					|| item.X == actualSqr.X && item.Y == actualSqr.Y+1 // top
					|| item.X == actualSqr.X && item.Y == actualSqr.Y-1) // down
						_sqrToRemove.Add(item);

				}

				// Now, checking the "legal-to-move-on" Squares
				foreach(Square item in sqrAround)
				{
					_bufferPiece = m_board.GetPieceState(item);

					if(_bufferPiece == null)
					{
						ActionList.Add(ActionType.Move);
						break;
					}
				}
			} // endof Rosace

			else if(pieceState.Piece.Type == PieceType.Astree)
			{
				ActionList.Add(ActionType.Create);

				sqrAround.Clear();

				sqrAround = Square.GetSquaresAround(pieceState.Square, true);
				List<Square> sqrAroundAround = sqrAround; // SqrAround with +1 range !

				PieceState _bufferPiece;
				Square _bufferSquare = new Square();

				// Setting the correct range for SqrAroundAround
				foreach(Square sqrItem in sqrAroundAround)
				{
					sqrItem.X+=1;
					sqrItem.Y+=1;

					_bufferPiece = m_board.GetPieceState(sqrItem);

					if(_bufferPiece != null && _bufferPiece.Piece.Color == pieceState.Piece.Color) // If the Square CONTAINS an ALLY, we CANNOT GO there,
					{
						sqrAroundAround.Remove(_bufferPiece.Square);
					}
				}

				// Checking Squares directly next to 'pieceState'
				foreach(Square nxtSqrItem in sqrAround)
				{

					_bufferPiece = m_board.GetPieceState(nxtSqrItem);

					if(m_board.Pieces.Contains(_bufferPiece)) // If there's a piece next to us
					{
						// Left
						if(nxtSqrItem.X == pieceState.Square.X-1)		
						{
							_bufferSquare.X = nxtSqrItem.X-1; // Going left
							_bufferSquare.Y = nxtSqrItem.Y;

							// If the Square is empty or contains an enemy, it is a legal move
							if(sqrAroundAround.Contains(_bufferSquare))
							{
								ActionList.Add(ActionType.Move);
								break;
							}
						}
						
						// Right
						if(nxtSqrItem.X == pieceState.Square.X+1)		
						{
							_bufferSquare.X = nxtSqrItem.X+1; // Going right
							_bufferSquare.Y = nxtSqrItem.Y;

							// If the Square is empty or contains an enemy, it is a legal move
							if(sqrAroundAround.Contains(_bufferSquare))
							{
								ActionList.Add(ActionType.Move);
								break;
							}
						}

						// Top
						if(nxtSqrItem.Y == pieceState.Square.Y+1)		
						{
							_bufferSquare.X = nxtSqrItem.X;
							_bufferSquare.Y = nxtSqrItem.Y+1; // going top

							// If the Square is empty or contains an enemy, it is a legal move
							if(sqrAroundAround.Contains(_bufferSquare))
							{
								ActionList.Add(ActionType.Move);
								break;
							}
						}

						// Down
						if(nxtSqrItem.Y == pieceState.Square.Y-1)		
						{
							_bufferSquare.X = nxtSqrItem.X;
							_bufferSquare.Y = nxtSqrItem.Y-1; // Going down

							// If the Square is empty or contains an enemy, it is a legal move
							if(sqrAroundAround.Contains(_bufferSquare))
							{
								ActionList.Add(ActionType.Move);
								break;
							}
						}

						// Top-Left
						if(nxtSqrItem.X == pieceState.Square.X-1 && nxtSqrItem.Y == pieceState.Square.Y+1)		
						{
							_bufferSquare.X = nxtSqrItem.X-1; // Going left
							_bufferSquare.Y = nxtSqrItem.Y+1; // Goin top

							// If the Square is empty or contains an enemy, it is a legal move
							if(sqrAroundAround.Contains(_bufferSquare))
							{
								ActionList.Add(ActionType.Move);
								break;
							}
						}

						// Top-Right
						if(nxtSqrItem.X == pieceState.Square.X+1 && nxtSqrItem.Y == pieceState.Square.Y+1)
						{
							_bufferSquare.X = nxtSqrItem.X+11; // Going right
							_bufferSquare.Y = nxtSqrItem.Y; // Going top

							// If the Square is empty or contains an enemy, it is a legal move
							if(sqrAroundAround.Contains(_bufferSquare))
							{
								ActionList.Add(ActionType.Move);
								break;
							}
						}
						
						// Down-Left
						if(nxtSqrItem.X == pieceState.Square.X-1 && nxtSqrItem.Y == pieceState.Square.Y-1)		
						{
							_bufferSquare.X = nxtSqrItem.X-1; // Going left
							_bufferSquare.Y = nxtSqrItem.Y; // Going down

							// If the Square is empty or contains an enemy, it is a legal move
							if(sqrAroundAround.Contains(_bufferSquare))
							{
								ActionList.Add(ActionType.Move);
								break;
							}
						}

						// Down-Right
						if(nxtSqrItem.X == pieceState.Square.X+1)		
						{
							_bufferSquare.X = nxtSqrItem.X+1; // Going right
							_bufferSquare.Y = nxtSqrItem.Y; // Going down

							// If the Square is empty or contains an enemy, it is a legal move
							if(sqrAroundAround.Contains(_bufferSquare))
							{
								ActionList.Add(ActionType.Move);
								break;
							}
						}
					}
				}	

			} // endof Astree

			return ActionList;
		} // endof GetPossibleActions()


		public bool CheckMoveIsValid(Piece piece, Square fromSqr, Square toSqr, bool power)
		{
			// First checking if user input is correct. (i.e. within the board boundaries)
			if(toSqr.X > 7 || toSqr.X < 0 || toSqr.Y > 7 || toSqr.Y < 0)
				return false;

			// Checking if 'piece' can theoretically go to 'toSqr'
			if(MoveValidator.MoveIsValid(piece, fromSqr, toSqr, power) == false)
				return false;

			// Checking what is on 'toSqr' and whether or not 'piece' can pass
			if(CheckObstacles(piece, fromSqr, toSqr, power) == false)
				return false;

			if(CheckTriggers(piece, toSqr) == false)
				return false;

			//if no piece between fromSqr and toSqr AND no piece on toSqr AND no trigger problems
			else
				return true;

		} // endof CheckMoveIsValid(Piece, Square, Square, bool)

		// Used to move Tetraglobes
		public bool CheckMoveIsValid(Piece piece, Intersection fromIntr, Intersection toIntr, bool power)
		{
			// Checking if toIntr is within the board boundaries
			if(toIntr.A >= 6 || toIntr.A <= 0 || toIntr.B >= 6 || toIntr.B <= 0)
				return false;

			Square[] fromSqrs = new Square[4];
			Square[] toSqrs = new Square[4];

			fromSqrs[0].X = fromIntr.A;
			fromSqrs[0].Y = fromIntr.B;
			fromSqrs[1].X = fromIntr.A;
			fromSqrs[1].Y = (fromIntr.B+1);
			fromSqrs[2].X = (fromIntr.A+1);
			fromSqrs[2].Y = fromIntr.B;
			fromSqrs[3].X = (fromIntr.A+1);
			fromSqrs[3].Y = (fromIntr.B+1);

			toSqrs[0].X = toIntr.A;
			toSqrs[0].Y = toIntr.B;
			toSqrs[1].X = toIntr.A;
			toSqrs[1].Y = (toIntr.B+1);
			toSqrs[2].X = (toIntr.A+1);
			toSqrs[2].Y = toIntr.B;
			toSqrs[3].X = (toIntr.A+1);
			toSqrs[3].Y = (toIntr.B+1);

			for(int i = 0; i < 4; i++)
			{
				if(CheckMoveIsValid(piece, fromSqrs[i], toSqrs[i], power) == false)
					return false;
			}

			return true;
		} // endof CheckMoveIsValid(Piece, Intersection, Intersection, bool)

		// Checking if there's a piece between fromSqr and toSqr or if a piece is on toSqr
		// Return value depends on parameter piece type and power, as well as obstacle nature (type, color, etc)
		private bool CheckObstacles(Piece piece, Square fromSqr, Square toSqr, bool power)
		{
			foreach(PieceState otherState in m_board.Pieces)
			{

				// If there's an otherState between starting square and destination
				if(fromSqr.X == toSqr.X && fromSqr.X < toSqr.X && otherState.Square.X == fromSqr.X
					&& otherState.Square.X < toSqr.X && otherState.Square.X > fromSqr.X
					|| fromSqr.X == toSqr.X && fromSqr.X > toSqr.X && otherState.Square.X == fromSqr.X
					&& otherState.Square.X > toSqr.X && otherState.Square.X < fromSqr.X
					|| fromSqr.Y == toSqr.Y && fromSqr.Y < toSqr.Y && otherState.Square.Y == fromSqr.Y
					&& otherState.Square.Y < toSqr.Y && otherState.Square.Y > fromSqr.Y
					|| fromSqr.Y == toSqr.Y && fromSqr.Y > toSqr.Y && otherState.Square.Y == fromSqr.Y
					&& otherState.Square.Y > toSqr.Y && otherState.Square.Y < toSqr.Y)
				{

					switch(piece.Type)
					{
						case PieceType.Globule:
							break;
						case PieceType.Triglobe:
							break;
						case PieceType.Tetraglobe:
							break;
						case PieceType.Pentaglobe:
							break;
						case PieceType.Tetrastre:
							if((fromSqr.X % 2) == 0) //if fromSqr is even
							{
								if(otherState.Square.X == fromSqr.X && (otherState.Square.Y % 2) != 0) //if on the same row and has odd column
									return true;
								else if(otherState.Square.Y == fromSqr.Y && (otherState.Square.X % 2) != 0) //if on the same column and has odd row
									return true;
							}
							else //if fromSqr is odd
							{
								if(otherState.Square.X == fromSqr.X && (otherState.Square.Y % 2) == 0) //if on the same row and has even column
									return true;
								else if(otherState.Square.Y == fromSqr.X && (otherState.Square.Y % 2) == 0) //if on the same colum and has even row
									return true;
							}
							return false;
						case PieceType.Triastre:
							return false;
						case PieceType.Pentastre:
							return false;
						case PieceType.Rosace:
							return false;
						case PieceType.Astree:
							if(toSqr.X > (otherState.Square.X + 1) || toSqr.X < (otherState.Square.X - 1)
								|| toSqr.Y > (otherState.Square.Y + 1) || toSqr.Y < (otherState.Square.Y - 1))
								return true;
							return false;
						default:
							return false;
					}
					break;
				}

				//Checking if there's an ENEMY piece on destination
				if(toSqr == otherState.Square && otherState.Piece.Color != piece.Color)
				{
					switch(piece.Type)
					{
						case PieceType.Globule:
							return false;
						case PieceType.Triastre:
							return true;
						case PieceType.Triglobe:
							if(power)
							{
								if(toSqr.X > (fromSqr.X + 1) || toSqr.X < (fromSqr.X - 1)
									|| toSqr.Y > (fromSqr.Y + 1) || toSqr.Y < (fromSqr.Y - 1))
									return false;
							}
							if(toSqr.X == (fromSqr.X + 1) || toSqr.X == (fromSqr.X - 1)
								|| toSqr.Y == (fromSqr.Y + 1) || toSqr.Y == (fromSqr.Y - 1))
								return true;
							return false;
						case PieceType.Tetraglobe:
							return true;
						case PieceType.Tetrastre:
							return false;
						case PieceType.Pentastre: // NB : IF Powerful : Can only eat same PieceType
							if(power)
							if(otherState.Piece.Type == PieceType.Pentastre)
								return true;
							return false;
						case PieceType.Pentaglobe:
							return false;
						case PieceType.Rosace:
							return false;
						case PieceType.Astree:
							return true;
						default:
							return false;
					}
				}

				//Checking if there's an ALLY piece on destination
				if(toSqr == otherState.Square && otherState.Piece.Color == piece.Color)
				{
					if(piece.Type == PieceType.Pentaglobe && otherState.Piece.Type != PieceType.Pentaglobe)
						return true;
					else
						return false;
				}
			} // endof 'foreach(PieceState otherState in m_board.Pieces)'

			return false;
		} // endof CheckObstacles(Piece, Square, Square, bool)

		// Checking if toSqr is triggered and, if so, checks toSqr's TriggerType
		// Return value depends on parameter piece and TriggerType
		private bool CheckTriggers(Piece piece, Square toSqr)
		{
			// Checking if destination is triggered
			if(toSqr.TriggerType != Square.Trigger.None)
			{
				if(toSqr.TriggerType == Square.Trigger.Shield)
				{
					// if player wants to create a Globule on shielded 'toSqr' -> nope
					if(piece.Type == PieceType.Globule)
						return false;
				}
			}

			return true;
		} // endof CheckTriggers(Piece, Square)



		// FIXME: Stack action is part of the Action managing system (undo/redo)


		// StackAction is called once an action as been performed by the user
		// It is used to store the user's actions so he's able to keep track and undo those actions
		// 'pieceState' reprensents the piece PERFORMING the action
		public void StackAction(PieceState pieceState, Square fromSqr, Square toSqr, ActionType actionType)
		{
			m_ActionStack.Push(new Action(pieceState, fromSqr, toSqr, actionType));
		}

		public void StackAction(PieceState pieceState, Intersection fromIntr, Intersection toIntr, ActionType actionType)
		{
			m_ActionStack.Push(new Action(pieceState, fromIntr, toIntr, actionType));
		}

		public void StackAction(Piece piece, Square fromSqr, Square toSqr, ActionType actionType)
		{
			PieceState _bufferPiece = m_board.GetPieceState(piece);
			m_ActionStack.Push(new Action(_bufferPiece, fromSqr, toSqr, actionType));
		}

		public void StackAction(Piece piece, Intersection fromIntr, Intersection toIntr, ActionType actionType)
		{
			PieceState _bufferPiece = m_board.GetPieceState(piece);
			m_ActionStack.Push(new Action(_bufferPiece, fromIntr, toIntr, actionType));
		}

		// DEBUG: Err, need to fix this or abandon the idea -- STILL NEED THE STACK ACTION THO !
		// public void UndoAction()
		// {
		// 	m_redoAction = m_ActionStack.Pop();
		// }

		// DEBUG: This need a dedicated file

		// This class will be used to store previously done actions, permitting to undo and redo moves or just access them
		protected class Action
		{
			PieceState m_pieceState; // Piece PERFORMING the action
			PieceState m_targetState;  // Possible Piece RECEIVING the action
			Square m_fromSqr;
			Square m_toSqr;
			Intersection m_fromIntr;
			Intersection m_toIntr;
			ActionType m_actionType;

			public Action()
			{
			}

			public Action(PieceState pieceState, Square fromSqr, Square toSqr, ActionType actionType)
			{
				m_pieceState = pieceState;
				m_fromSqr = fromSqr;
				m_toSqr = toSqr;
				m_actionType = actionType;
			}

			public Action(PieceState pieceState, PieceState targetState, Square fromSqr, Square toSqr, ActionType actionType)
			{
				m_pieceState = pieceState;
				m_targetState = targetState;
				m_fromSqr = fromSqr;
				m_toSqr = toSqr;
				m_actionType = actionType;
			}

			public Action(PieceState pieceState, Intersection fromIntr, Intersection toIntr, ActionType actionType)
			{
				m_pieceState = pieceState;
				m_fromIntr = fromIntr;
				m_toIntr = toIntr;
				m_actionType = actionType;
			}

			public Action(PieceState pieceState, PieceState targetState, Intersection fromIntr, Intersection toIntr, ActionType actionType)
			{
				m_pieceState = pieceState;
				m_targetState = targetState;
				m_fromIntr = fromIntr;
				m_toIntr = toIntr;
				m_actionType = actionType;
			}

			public PieceState PieceState
			{
				get {return m_pieceState;}
				set {m_pieceState = value;}
			}

			public PieceState TargetState
			{
				get {return m_targetState;}
				set {m_targetState = value;}
			}

			public Square FromSquare
			{
				get {return m_fromSqr;}
				set {m_fromSqr = value;}
			}

			public Square ToSquare
			{
				get {return m_toSqr;}
				set {m_toSqr = value;}
			}

			public Intersection FromIntersection
			{
				get {return m_fromIntr;}
				set {m_fromIntr = value;}
			}

			public Intersection ToIntersection
			{
				get {return m_toIntr;}
				set {m_toIntr = value;}
			}

			public ActionType ActionType
			{
				get {return m_actionType;}
				set {m_actionType = value;}
			}
		} // endof class Action

	} // endof class GameLogic
} // endof namespace GameModel
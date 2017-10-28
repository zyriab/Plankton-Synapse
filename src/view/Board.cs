using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

/* SINGLETON */
// Reprensents the board, gives the methods to move, remove, swap, etc pieces GameObject
// Also stores all GameObjects and give methods to find them
namespace GameView
{	
	public class Board : MonoBehaviour
	{
		// Contains all pieces on the board
		private List<GameObject> m_pieces;

		private static Board m_instance;
		
		public Board()
		{
			m_pieces = new List<GameObject>();
		}

		// Although the name may be confusing, this returns a GameObject in-game, based on a given Piece
		public GameObject GetPieceState(GameModel.Piece piece)
		{
			foreach(GameObject item in m_pieces)
			{
				if(item.GetComponent<PieceScript>().Piece == piece)
					return item;
			}

			return null;		
		}

		public GameObject GetPieceState(GameModel.Square square)
		{
			foreach(GameObject item in m_pieces)
			{
				if(item.GetComponent<PieceScript>().Square == square)
					return item;
			}

			return null;
		}

		public GameObject GetPieceState(GameModel.Intersection intersection)
		{
			foreach(GameObject item in m_pieces)
			{
				if(item.GetComponent<PieceScript>().Intersection == intersection)
					return item;
			}

			return null;
		}

		public GameObject GetPieceState(Vector3 position)
		{
			foreach(GameObject item in m_pieces)
			{
				if(item.transform.position == position)
					return item;
			}

			return null;
		}

		[SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
		public GameObject GetPieceState(float x, float y)
		{
			foreach(GameObject item in m_pieces)
			{
				if(item.transform.position.x == x && item.transform.position.y == y)
					return item;
			}

			return null;
		}

		// For the translation of ingame coordinates to Unity and vice-versa :
		// We inverse X and Y coordinates because Unity's coordinates system doesn't work the same way
		// In Square(5, 6) we would be 5 squares UP and 6 squares RIGHT
		// In Vector3(5,6) we could be 5 "meter" RIGHT and 6 "meters" UP
		// So we need to invert that and then make the math to have distances relevant to the actual "physical" squares !
		public static Vector3 ToUnityCoord(GameModel.Square sqr)
		{
			Vector3 position = new Vector3();
			
			position.x = (float)(sqr.Y*0.6); // the distance between two squares center is of 0.6 "meters"
			
			// since Unity starts 0,0 on TOP-LEFT and the GameModel starts 0,0 on BOTTOM-LEFT
			// we need to lower our starting position (7*0.6) times to be on BOTTOM-LEFT, hence the -4.2
			position.y = (float)(-4.2 + sqr.X*0.6);
			

			return position;
		}

		public static Vector3 ToUnityCoord(GameModel.Intersection intr)
		{
			Vector3 position = new Vector3();

			// same as for the squares here, adding a 0.3 since it is where the INTR(0,0) is, "physically"
			position.x = (float) (0.3 + intr.B * 0.6);

			// same as for the squares, the Y INTR starts just a bit higher
			position.y = (float) (-3.9 + intr.A * 0.6);

			return position;
		}

		public static GameModel.Square ToModelSqr(Vector3 pos)
		{
			GameModel.Square square = new GameModel.Square();

			square.X = (int)(pos.y / 0.6);
			square.Y = (int)((pos.x + 4.2) / 0.6);

			return square;
		}

		public static GameModel.Intersection ToModelIntr(Vector3 pos)
		{
			GameModel.Intersection intersection = new GameModel.Intersection();

			intersection.A = (int)((pos.y - 0.3) / 0.6);
			intersection.B = (int)((pos.x + 3.9) / 0.6);
			
			return intersection;
		}

		/* ENDOF COORDINATES TRANSLATORS */

		// Adds a new GameObject containing a SpriteRenderer, BoxCollider2D, PieceScript, fully initiated; on the board
		public void PlacePiece(GameModel.Piece piece, GameModel.Square square)
		{
			GameObject newPiece = new GameObject(piece.Type.ToString());
			
			newPiece.AddComponent<SpriteRenderer>();
			newPiece.AddComponent<BoxCollider2D>();
			newPiece.AddComponent<PieceScript>();

			newPiece.GetComponent<BoxCollider2D>().isTrigger = true;
			newPiece.GetComponent<PieceScript>().Collider = newPiece.GetComponent<BoxCollider2D>();
			newPiece.GetComponent<PieceScript>().Type = piece.Type;
			newPiece.GetComponent<PieceScript>().Color = piece.Color;
			newPiece.GetComponent<PieceScript>().Square = square;
			newPiece.GetComponent<PieceScript>().Piece = piece;
			newPiece.GetComponent<PieceScript>().InitSprites();
			newPiece.GetComponent<PieceScript>().CheckSprite();
			
			
			m_pieces.Add(newPiece);

			Instantiate(newPiece, newPiece.transform.position, Quaternion.identity);
		}

		public void PlacePiece(GameModel.Piece piece, GameModel.Intersection intersection)
		{
			if(piece.Type != PieceType.Tetraglobe)
				return;

			GameObject newPiece = new GameObject(GameModel.GetString.GetStr(piece.Type));

			newPiece.AddComponent<SpriteRenderer>();
			newPiece.AddComponent<BoxCollider2D>();
			newPiece.AddComponent<PieceScript>();

			newPiece.GetComponent<BoxCollider2D>().isTrigger = true;
			newPiece.GetComponent<PieceScript>().Collider = newPiece.GetComponent<BoxCollider2D>();
			newPiece.GetComponent<PieceScript>().Type = piece.Type;
			newPiece.GetComponent<PieceScript>().Color = piece.Color;
			newPiece.GetComponent<PieceScript>().Intersection = intersection;
			newPiece.GetComponent<PieceScript>().Piece = piece;
			newPiece.GetComponent<PieceScript>().InitSprites();
			newPiece.GetComponent<PieceScript>().CheckSprite();
			
			m_pieces.Add(newPiece);

			Instantiate(newPiece, newPiece.transform.position, Quaternion.identity);
		}

		// Swaps two pieces position
		public void SwapPiece(GameModel.Piece piece1, GameModel.Piece piece2)
		{
			Vector3 pos1 = GetPieceState(piece1).transform.position;
			Vector3 pos2 = GetPieceState(piece2).transform.position;

			MovePiece(piece1, pos2);
			MovePiece(piece2, pos1);
		}

		public void SwapPiece(GameModel.Piece piece1, GameModel.Piece piece2, GameModel.Square toSqr, GameModel.Intersection toIntr)
		{	
			MovePiece(piece1, toSqr);
			MovePiece(piece2, toIntr);
			
			GetPieceState(piece1).GetComponent<PieceScript>().CheckSprite();
			GetPieceState(piece2).GetComponent<PieceScript>().CheckSprite();
		}

		// Transforms one or more given pieces to a certain type, on a certain Square or Intersection
		public void TransformPiece(PieceType type, GameModel.Intersection toIntr, GameModel.Square toSqr, params GameModel.Piece[] piece)
		{
			// Transforming one piece
			if(piece.Length == 1 && toSqr != null)
			{
				int index = m_pieces.IndexOf(GetPieceState(piece[0]));
				
				m_pieces[index].GetComponent<PieceScript>().Type = type;
				m_pieces[index].GetComponent<PieceScript>().Square = toSqr;
				m_pieces[index].GetComponent<PieceScript>().Piece = new GameModel.Piece(type, piece[0].Color);
				m_pieces[index].GetComponent<PieceScript>().InitSprites();
				m_pieces[index].GetComponent<PieceScript>().CheckSprite();
			}

			// Transforming Globules
			else if(piece.Length > 1 && toIntr != null)
			{
				int index = m_pieces.IndexOf(GetPieceState(piece[0]));
				
				foreach (GameModel.Piece item in piece)
				{
					if(item != piece[0])
						m_pieces.Remove(GetPieceState(item));
				}

				m_pieces[index].GetComponent<PieceScript>().Type = type;
				m_pieces[index].transform.position = type != PieceType.Tetraglobe ? ToUnityCoord(toSqr) : ToUnityCoord(toIntr);
				m_pieces[index].GetComponent<PieceScript>().Piece = new GameModel.Piece(type, piece[0].Color);
				m_pieces[index].GetComponent<PieceScript>().InitSprites();
				m_pieces[index].GetComponent<PieceScript>().CheckSprite();
				
			}
		}

		private void MovePiece(GameModel.Piece piece, Vector3 position)
		{
			GetPieceState(piece).transform.position = position;
		}
		
		public void MovePiece(GameModel.Piece piece, GameModel.Intersection toIntr)
		{
			GetPieceState(piece).transform.position = ToUnityCoord(toIntr);
		}

		public void MovePiece(GameModel.Piece piece, GameModel.Square toSqr)
		{
			GetPieceState(piece).transform.position = ToUnityCoord(toSqr);
		}

		// Removes a piece on a given square
		public void RemovePiece(GameModel.Square sqr)
		{
			GameObject removedPiece = GetPieceState(sqr);

			if (!m_pieces.Contains(removedPiece))
				return;
			
			removedPiece.SetActive(false);
			m_pieces.Remove(removedPiece);
			Destroy(removedPiece);
		}

		// Removes a piece on a given intersection
		public void RemovePiece(GameModel.Intersection intr)
		{
			GameObject removedPiece = GetPieceState(intr);

			if (!m_pieces.Contains(removedPiece))
				return;
			
			removedPiece.SetActive(false);
			m_pieces.Remove(removedPiece);
			Destroy(removedPiece);
		}

		// Removes a piece by passing its reference piece directly
		public void RemovePiece(GameModel.Piece piece)
		{
			GameObject removedPiece = GetPieceState(piece);

			if (!m_pieces.Contains(removedPiece))
				return;
			
			removedPiece.SetActive(false);
			m_pieces.Remove(removedPiece);
			Destroy(removedPiece);
		}

		// Create a Globule on a given square
		public void CreateGlobule(GameModel.Piece piece, GameModel.Square toSqr)
		{
			PlacePiece(piece, toSqr);
		}

		/* ACCESSORS */

		public static Board Instance()
		{
			if (!m_instance)
			{
				m_instance = FindObjectOfType(typeof(Board)) as Board;
				if(!m_instance)
					Debug.LogError("Board -- ERROR no instance found");
			}

			return m_instance;
		}

		public List<GameObject> Pieces
		{
			get {return m_pieces;}
		}

	} // endof class Board
} // endof namespace GameModel
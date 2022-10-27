using System.Collections.Generic;
using GameModel;
using GameView;
using UnityEditor.Animations;
using UnityEngine;
using Board = GameModel.Board;
using Intersection = GameModel.Intersection;

// TODO:
// (Prsntr?)- Implement/Figure out 'JumpPiece()' and automatic jump managing
// (Prsntr?)- Implement Game Over, etc
// (Prsntr) - Implement/Figure out 'StopPiece()' for trigger // Maybe StopPiece() and ProtectPiece() will be live managed by the board or Presenter ?
// (Prsntr) - Implement/Figure out 'ProtectPiece()' for trigger
// (Prsntr) - Implement/Figure out ErrorLog & GameLog (GameLog = Current moves summary)
// (Prsntr) - Implement/Figure out free moves and pass free moves !
// 			- Implement Intersection Highlight/Selection

// TODO: Figure out the intersection management -> selection


namespace GamePresenter
{
    /// <summary>
    /// /* SINGLETON */
    /// <para> Reads input (callbacks) from the View, (translate) calls the appropriate methods in Model, then apply/modify the view </para>
    /// </summary>

    public class IOManager : MonoBehaviour
    {
        private static IOManager m_instance;

        private GameModel.GameLogic m_gameLogic;
        private GameView.GameDrawer m_gameDrawer;

        private Queue<ActionType> m_actionQueue = new Queue<ActionType>();
        private Queue<GameModel.Piece> m_pieceQueue = new Queue<GameModel.Piece>();

        private List<GameView.Square> m_highlightedSqrs = new List<GameView.Square>();

        private bool m_hasActionInQueue;

        //private bool GetSwapIntersection(GameView.Square vSqr)
        //{
        //    GameModel.Square toSqr = new GameModel.Square();

        //    toSqr.X = (int)vSqr.X;
        //    toSqr.Y = (int)vSqr.Y;

        //    List<GameModel.Intersection> freeIntr = m_gameLogic.Board.GetFreeIntersections(toSqr);

        //    foreach(GameModel.Intersection intr in freeIntr)
        //    {
        //        foreach(GameModel.Square sqr in intr.ToSquares)
        //        {
        //            m_gameDrawer.Board.GetSquare(sqr).SetHightlight(true, out List<Intersection> freeIntr);
        //        }
        //    }
        //}

        /// <summary> Execute last selected action on last selected piece on last selected Square </summary>
        public void ExecutePendingAction(GameView.Square vSqr)
        {
            GameModel.Piece piece = DequeuePiece();
            GameModel.Square fromSqr = m_gameLogic.Board.GetPieceState(piece).Square;
            GameModel.Square toSqr = new GameModel.Square();


            toSqr.X = (int)vSqr.X;
            toSqr.Y = (int)vSqr.Y;

            switch (m_actionQueue.Peek())
            {
                case ActionType.Create:
                    m_gameLogic.Board.CreateGlobule(piece, toSqr);
                    m_gameDrawer.Board.CreateGlobule(piece, toSqr);
                    break;
                case ActionType.Move:
                    if (m_gameLogic.Board.GetPieceOnSqr(toSqr) != null)
                    {
                        m_gameLogic.Board.RemovePiece(toSqr);
                        m_gameDrawer.Board.RemovePiece(toSqr);
                    }

                    m_gameLogic.Board.MovePiece(piece, fromSqr, toSqr);
                    break;
                case ActionType.Swap:
                    m_gameLogic.Board.SwapPiece(piece, m_gameLogic.Board.GetPieceOnSqr(toSqr));
                    m_gameDrawer.Board.SwapPiece(piece, m_gameLogic.Board.GetPieceOnSqr(toSqr));
                    break;

            }
        }

        /// <summary> Execute last selected action on last selected piece on last selected Intersection </summary>
        /// <remarks> vSqr represents the Square being selected inside that Intersection (relevant only for Pentaglobe swap) </remarks>
        public void ExecutePendingAction(GameView.Intersection vIntr, GameView.Square vSqr = null)
        {
            GameModel.Piece piece = DequeuePiece();

            GameModel.Intersection fromIntr = new GameModel.Intersection();
            GameModel.Square fromSqr = new GameModel.Square();

            if (piece.Type == PieceType.Tetraglobe)
                fromIntr = m_gameLogic.Board.GetPieceState(piece).Intersection;
            else
                fromSqr = m_gameLogic.Board.GetPieceState(piece).Square;

            GameModel.Intersection toIntr = new GameModel.Intersection();
            GameModel.Square toSqr = new GameModel.Square();

            toIntr.A = (int)vIntr.A;
            toIntr.B = (int)vIntr.B;

            if (vSqr != null)
            {
                toSqr.X = (int)vSqr.X;
                toSqr.Y = (int)vSqr.Y;
            }

            switch (m_actionQueue.Peek())
            {
                case ActionType.Move:
                    if (m_gameLogic.Board.GetPiecesOnIntr(toIntr) != null)
                    {
                        m_gameLogic.Board.RemovePiece(toIntr);
                        m_gameDrawer.Board.RemovePiece(toIntr);
                    }

                    m_gameLogic.Board.MovePiece(piece, fromIntr, toIntr);
                    break;
                case ActionType.Swap:

                    // TODO: PLAYER MUST CHOOSE BOTH DESTINATIONS !
                    // TODO: I can use something like the GameLogic (MoveValidator) for the possible Intr :)
                    // Now we need to highlight the Intr for the TTG to move on it
                    // Then we call SwapPiece(piece, vSqr, intr);
                    break;

            }
        }

        public void OnPieceClicked(GameModel.Piece piece)
        {
            
            if(m_highlightedSqrs.Contains(GameView.Board.GetPieceState(piece).GetComponent<PieceScript>().Square))

            // Un-highlighting squares
            if (m_highlightedSqrs.Count != 0)
                foreach (GameView.Square sqr in m_highlightedSqrs)
                    sqr.SetHightlight();

            List<ActionType> actionsList = GameLogic.GetPossibleActions(piece, m_gameLogic.CheckPower(piece));

            m_pieceQueue.Enqueue(piece);
            AppManagers.UIManager.OpenActionMenu(piece, actionsList);
        }

        public void QueueAction(ActionType action)
        {
            List<GameModel.Square> highlightedSqr =
                m_gameLogic.GetLegalSquares(action, m_pieceQueue.Peek(), m_gameLogic.CheckPower(m_pieceQueue.Peek()));

            List<GameModel.Intersection> highlightedIntr = new List<Intersection>();

            if (m_pieceQueue.Peek().Type == PieceType.Tetraglobe)
            {
                highlightedIntr =
                    m_gameLogic.GetLegalIntersections(action, m_pieceQueue.Peek(), m_gameLogic.CheckPower(m_pieceQueue.Peek()));
            }

            GameModel.Square _buffSqr = new GameModel.Square();

            m_actionQueue.Enqueue(action);

            for (int i = 0; i < m_gameDrawer.Squares.transform.childCount - 1; i++)
            {
                _buffSqr.X = (int)m_gameDrawer.Squares.transform.GetChild(i).GetComponent<GameView.Square>().X;
                _buffSqr.Y = (int)m_gameDrawer.Squares.transform.GetChild(i).GetComponent<GameView.Square>().Y;

                // If we are working with Intersections, we need to group the Squares related to those Intersections
                if (highlightedIntr.Count != 0)
                {
                    // Checking each intersections
                    foreach (Intersection item in highlightedIntr)
                    {
                        // If the current Intersection contains the actual Square
                        if (item.ToSquares.Contains(_buffSqr))
                        {

                            foreach (Transform childSqr in m_gameDrawer.Squares.transform)
                            {

                                // Get the view Square related to those coordinates and sets its Intersection reference
                                if ((int)childSqr.gameObject.GetComponent<GameView.Square>().X == _buffSqr.X
                                    && (int)childSqr.gameObject.GetComponent<GameView.Square>().Y == _buffSqr.Y)
                                {
                                    childSqr.gameObject.GetComponent<GameView.Square>().Intersection.A = item.A;
                                    childSqr.gameObject.GetComponent<GameView.Square>().Intersection.B = item.B;
                                }
                            }
                        }

                    }
                }

                // Highlights all the legal Squares (Intersections-related as well)
                if (highlightedSqr.Contains(_buffSqr))
                {
                    if (m_pieceQueue.Peek().Type != PieceType.Tetraglobe)
                        m_gameDrawer.Squares.transform.GetChild(i).GetComponent<GameView.Square>().SetHightlight();
                    else
                        m_gameDrawer.Squares.transform.GetChild(i).GetComponent<GameView.Square>().Intersection.SetHighlight();

                    m_highlightedSqrs.Add(m_gameDrawer.Squares.transform.GetChild(i).GetComponent<GameView.Square>());
                }
            }
        }

        public ActionType DequeueAction()
        {
            return m_actionQueue.Dequeue();
        }

        public ActionType PeekAction()
        {
            return m_actionQueue.Peek();
        }

        public void QueuePiece(GameModel.Piece piece)
        {
            m_pieceQueue.Enqueue(piece);
        }

        public GameModel.Piece DequeuePiece()
        {
            return m_pieceQueue.Dequeue();
        }

        public GameModel.Piece PeekPiece()
        {
            return m_pieceQueue.Peek();
        }

        /* UNITY METHODS */

        private void Awake()
        {
            if (m_instance == null)
                m_instance = this;
            if (m_instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);

            m_hasActionInQueue = false;
        }

        /* ACCESSORS */

        public GameModel.GameLogic GameLogic
        {
            get
            {
                return m_gameLogic;
            }
            set
            {
                m_gameLogic = value;
            }
        }

        public GameView.GameDrawer GameDrawer
        {
            get
            {
                return m_gameDrawer;
            }
            set
            {
                m_gameDrawer = value;
            }
        }

        public bool HasActionInQueue
        {
            get
            {
                return m_hasActionInQueue;
            }
            set
            {
                m_hasActionInQueue = value;
            }
        }

        public static IOManager Instance
        {
            get
            {
                return m_instance;
            }
        }

        public List<GameView.Square> HighlightedSqrs
        {
            get { return m_highlightedSqrs; }
        }

    } // endof class IOManager
} // endof namespace GamePresenter

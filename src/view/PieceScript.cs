using UnityEngine;

// Represents a visual piece, gives different accesses to a piece's GameObject
namespace GameView
{
    public class PieceScript : MonoBehaviour
    {
        private Sprite m_sprite1; // Black square
        private Sprite m_sprite2; // White square
        private Sprite m_spriteGlow1; // Black square glowing sprite
        private Sprite m_spriteGlow2; // White square glowing sprite

        private PieceType m_type;
        private Color m_color;
        private GameModel.Piece m_piece;

        public PieceScript(PieceType type, Color color, GameModel.Square square)
        {
            m_type = type;
            m_color = color;
            
            gameObject.transform.position = Board.ToUnityCoord(square);

            if (!InitSprites())
            {
                Debug.LogError("Error initializating sprites for " + m_type);
                return;
            }

            gameObject.GetComponent<SpriteRenderer>().sprite = (square.X + square.Y) % 2 == 0 ? m_sprite1 : m_sprite2;

            gameObject.SetActive(true);
        }

        public PieceScript(PieceType type, Color color, GameModel.Intersection intersection)
        {
            m_type = type;
            m_color = color;
            
            gameObject.transform.position = Board.ToUnityCoord(intersection);

            if (!InitSprites())
            {
                Debug.LogError("Error initializating sprites for " + m_type);
                return;
            }

            SpriteRenderer.sprite = (intersection.A + intersection.B) % 2 == 0 ? m_sprite1 : m_sprite2;

            gameObject.SetActive(true);
        }

        public PieceScript(PieceType type, Color color, GameModel.Piece piece, Vector3 position)
        {
            m_type = type;
            m_color = color;
            
            gameObject.transform.position = position;

            if (!InitSprites())
            {
                Debug.LogError("Error initializating sprites for " + GameModel.GetString.GetStr(piece.Type));
                return;
            }

            CheckSprite();

            gameObject.SetActive(true);
        }

        // Init the piece's sprites depending on its type, color and position
        public bool InitSprites()
        {
            string spriteName = "";

            spriteName += m_type.ToString();
            spriteName += m_color == Color.Black ? " A" : " B";
            spriteName += 1;
            spriteName += ".png";

            // Loading black square sprite
            m_sprite1 = Resources.Load<Sprite>(spriteName);
            if (m_sprite1 == null)
                return false;

            // Changing sprite to white square
            spriteName = spriteName.Replace((char) 1, (char) 2);

            // Loading white square sprite
            m_sprite2 = Resources.Load<Sprite>(spriteName);
            if (m_sprite2 == null)
                return false;

            // Changing sprite to white square glowing
            spriteName = spriteName.Replace(".png", "glow.png");

            // Loading white square glowing sprite
            m_spriteGlow2 = Resources.Load<Sprite>(spriteName);
            if (m_spriteGlow2 == null)
                return false;

            // Changing sprite to black square glowing
            spriteName = spriteName.Replace((char) 2, (char) 1);

            // Loading black square glowing sprite
            m_spriteGlow1 = Resources.Load<Sprite>(spriteName);
            if (m_spriteGlow1 == null)
                return false;            
        }

        // Checks and uses the adapted sprite, regarding to the row+column alignment (meaning: the actual square color)
        public void CheckSprite()
        {
            Sprite = (this.Square.X + this.Square.Y) % 2 == 0 ? m_sprite2 : m_sprite1;
        }
        
        /* UNITY METHODS */
        
        // When the mouse is pressed then released over the piece
        private void OnMouseUpAsButton()
        {
            GamePresenter.IOManager.OnPieceClicked(this.m_piece);
        }

        /* ACCESSORS */
        public PieceType Type { get; set; }
        public Color Color { get; set; }
        public SpriteRenderer SpriteRenderer { get; set; }
        public BoxCollider2D Collider { get; set; }
        public GameModel.Piece Piece { get; set; }
        
        public Sprite Sprite
        {
            get { return gameObject.GetComponent<SpriteRenderer>().sprite; }
            set { gameObject.GetComponent<SpriteRenderer>().sprite = value; }
        }
        
        public GameModel.Square Square
        {
            get { return Board.ToModelSqr(gameObject.transform.position); }
            set { gameObject.transform.position = Board.ToUnityCoord(value); }
        }

        public GameModel.Intersection Intersection
        {
            get { return Board.ToModelIntr(gameObject.transform.position); }
            set { gameObject.transform.position = Board.ToUnityCoord(value); }
        }
    }
}
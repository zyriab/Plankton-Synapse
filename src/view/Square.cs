using UnityEngine;

// Represents a visual square, gives methods to modify its visual state (highlighted, triggered, etc)
namespace GameView
{
	public class Square : MonoBehaviour
	{
		private float m_x;
		private float m_y;
		private bool m_isTriggered;
		private bool m_isHiglighted;

		// When the square operates a a quarter of an intersection, holds a reference to that intersection
		private GameView.Intersection m_intersection;

		private Sprite m_normalSprite;
		[SerializeField] private Sprite m_highLightSprite;
		[SerializeField] private Sprite m_selectedSprite;

		public Square(float x, float y)
		{
			m_x = x;
			m_y = y;
		}

		public Square()
		{
			m_x = -1;
			m_y = -1;
		}

		// Enable/Disable highlighting (for help purpose) of this square
		public void SetHightlight()
		{
			this.GetComponent<SpriteRenderer>().sprite = m_isHiglighted ? m_normalSprite : m_highLightSprite;

			m_isHiglighted = !m_isHiglighted;
		}

		/* UNITY METHODS */
		private void Start()
		{
			m_x = Board.ToModelSqr(this.transform.position).X;
			m_y = Board.ToModelSqr(this.transform.position).Y;

			m_isTriggered = false;
			m_isHiglighted = false;
			
			m_intersection = new Intersection();

			m_normalSprite = this.GetComponent<SpriteRenderer>().sprite;
		}

		// If the mouse hover this square while it's highlighted, changes slightly its color (UX purpose)
		private void OnMouseEnter()
		{
			if (m_isHiglighted)
				this.GetComponent<SpriteRenderer>().sprite = m_selectedSprite;
		}

		// If the mouse hovering exits the square boundaries, put sprite back to normal highlight (if highlighted)
		private void OnMouseExit()
		{
			if (m_isHiglighted)
				this.GetComponent<SpriteRenderer>().sprite = m_highLightSprite;
		}

		// If the mouse is released over this square while it was pressed over this square as well
		// Calls the Input/Output Manager from the presenter to execute the (valitated) player's previously chosen action
		private void OnMouseUpAsButton()
		{
			if (m_isHiglighted)
			{
				if(AppManagers.IOManager.HasActionInQueue)
					AppManagers.IOManager.ExecutePendingAction(this);
			}
		}
		
		/* ACCESSORS */
		
		public float X { get; set; }
		public float Y { get; set; }
		public GameView.Intersection Intersection { get; set; }
		public bool IsTriggered { get; set; }
		public bool IsHiglighted { get; set; }
	} // endof class Square
} // endof namespace GameView
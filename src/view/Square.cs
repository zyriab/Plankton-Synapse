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

		private Sprite m_normalSprite;
		[SerializeField] private Sprite m_highLightSprite;
		[SerializeField] private Sprite m_selectedSprite;

		// Enable/Disable highlighting (for help purpose) of this square
		public void SetHightlight()
		{
			if (m_isHiglighted)
			{
				m_isHiglighted = false;
				this.GetComponent<SpriteRenderer>().sprite = m_normalSprite;
			}
			else
			{
				m_isHiglighted = true;
				this.GetComponent<SpriteRenderer>().sprite = m_highLightSprite;
			}
		}

		/* UNITY METHODS */
		private void Start()
		{
			m_x = Board.ToModelSqr(this.transform.position).X;
			m_y = Board.ToModelSqr(this.transform.position).Y;

			m_isTriggered = false;
			m_isHiglighted = false;

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
				if(GamePresenter.IOManager.HasActionInQueue)
					GamePresenter.IOManager.ExecutePendingAction();
			}
		}
	}
}
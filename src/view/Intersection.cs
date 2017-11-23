using System.Collections.Generic;
using UnityEngine;

// ReSharper disable CompareOfFloatsByEqualityOperator

namespace GameView
{
    public class Intersection
    {
        private float m_a;
        private float m_b;
        private bool m_isTriggered;
        private bool m_isHighlighted;

        private List<GameView.Square> m_squares;

        public Intersection(float a, float b)
        {
            m_a = a;
            m_b = b;

            foreach (Transform child in AppManagers.IOManager.GameDrawer.Board.GetComponent<Board>().transform)
            {
                if (child.gameObject.GetComponent<Square>().X == a || child.gameObject.GetComponent<Square>().X == a + 1
                    && child.gameObject.GetComponent<Square>().Y == b ||
                    child.gameObject.GetComponent<Square>().Y == b + 1)
                    m_squares.Add(child.gameObject.GetComponent<Square>());
            }
        }

        public Intersection()
        {
            m_a = -1;
            m_b = -1;
        }

        public void SetHighlight()
        {
            foreach (Square sqr in m_squares)
            {
                sqr.SetHightlight();
            }

            m_isHighlighted = !m_isHighlighted;
        }
        
        /* ACCESSORS */
        
        public float A { get; set; }
        public float B { get; set; }
        public bool IsHighlighted { get; set; }

        public bool IsTriggered
        {
            get
            {
                foreach (Square sqr in m_squares)
                {
                    if (sqr.IsTriggered)
                        return true;
                }

                return false;
            }
        }
    } // endof class Intersection
} // endof namespace GameView

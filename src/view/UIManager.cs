using System.Collections.Generic;
using System.Linq;
using GameModel;
using UnityEngine;
using UnityEngine.UI;

// SINGLETON GameObject whose purpose is to give access to the game UI mechanics and components
namespace GameView
{
    // ReSharper disable once InconsistentNaming
    public class UIManager : MonoBehaviour
    {
        // 0 = 1 action, 1 = 2 actions, 2 = 3 action, 3 = 4 actions
        [SerializeField] private GameObject[] m_menuArray = new GameObject[4];
        [SerializeField] private GameObject m_textArea;
        [SerializeField] private GameObject m_whiteCaptpuredPieces;
        [SerializeField] private GameObject m_blackCapturedPieces;

        private UIManager m_instance;

        // Displays text inside the dedicated text scrollable area
        public void DisplayText(string str)
        {
            m_textArea.GetComponent<Text>().text += "\n" + str;
        }

        public void DisplayText(int playerNum, ActionType action, GameModel.Piece piece, GameModel.Square toSqr)
        {
            if (action != ActionType.Create || action != ActionType.Move)
            {
                Debug.LogError("Wrong function called to display given actions");
                return;
            }

            m_textArea.GetComponent<Text>().text =
                "\n" + "Player " + playerNum + " "
                + GetString.GetStr(action) + " "
                + GetString.GetStr(piece.Type) + " to "
                + GetString.GetStr(toSqr);
        }

        public void DisplayText(int playerNum, ActionType action)
        {
            if (action != ActionType.DeleteLastSwapped || action != ActionType.SkipTurn ||
                action != ActionType.DeleteThis)
            {
                Debug.LogError("Wrong function called to display given action");
                return;
            }

            m_textArea.GetComponent<Text>().text =
                "\n" + "Player " + playerNum + " "
                + GetString.GetStr(action);
        }
        
        public void DisplayText(int playerNum, ActionType action, GameModel.Piece piece1, GameModel.Piece piece2)
        {
            if (action != ActionType.Swap || action != ActionType.Transform)
            {
                Debug.LogError("Wrong function called to display given actions");
                return;
            }
            m_textArea.GetComponent<Text>().text += "\n" + "Player " + playerNum + " "
                             + GetString.GetStr(action) + " "
                             + GetString.GetStr(piece1.Type)
                             + (action == ActionType.Swap ? " and " : " to ")
                             + GetString.GetStr(piece2.Type);
        }

        // Adds a miniature of a given piece to the its enemy's captured pieces panel
        public void AddCapturedPiece(GameModel.Piece piece)
        {
            GameObject childObj = null;
            string captPieceSpritePath = GetString.GetStr(piece.Type) + " ";
            string captPieceText = "";
            int numPiece;

            if (piece.Color == Color.Black)
            {
                captPieceSpritePath += "A1.png";

                for (int i = 0; i < m_blackCapturedPieces.transform.childCount; i++)
                {
                    childObj = m_blackCapturedPieces.transform.GetChild(i).gameObject;
                    captPieceText = childObj.transform.GetChild(0).gameObject.GetComponent<Text>().text;
                    numPiece = captPieceText[1];

                    if (childObj.GetComponent<Image>().sprite.name == "blackPiece.png")
                    {
                        childObj.GetComponent<Image>().sprite = Resources.Load<Sprite>(captPieceSpritePath);
                        childObj.SetActive(true);
                    }
                    else if (childObj.GetComponent<Image>().sprite.name == captPieceSpritePath)
                    {
                        captPieceText = captPieceText.Replace((char) numPiece, (char) ++numPiece);
                        if(!childObj.transform.GetChild(0).gameObject.activeInHierarchy)
                            childObj.transform.GetChild(0).gameObject.SetActive(true);
                    }
                }
            }
            
            if(piece.Color == Color.White)
            {
                captPieceSpritePath += "B1.png";
                
                for (int i = 0; i < m_whiteCaptpuredPieces.transform.childCount; i++)
                {
                    childObj = m_whiteCaptpuredPieces.transform.GetChild(i).gameObject;
                    captPieceText = childObj.transform.GetChild(0).gameObject.GetComponent<Text>().text;
                    numPiece = captPieceText[1];

                    if (childObj.GetComponent<Image>().sprite.name == "blackPiece.png")
                    {
                        childObj.GetComponent<Image>().sprite = Resources.Load<Sprite>(captPieceSpritePath);
                        childObj.SetActive(true);
                    }
                    else if (childObj.GetComponent<Image>().sprite.name == captPieceSpritePath)
                    {
                        captPieceText = captPieceText.Replace((char) numPiece, (char) ++numPiece);
                        if(!childObj.transform.GetChild(0).gameObject.activeInHierarchy)
                            childObj.transform.GetChild(0).gameObject.SetActive(true);
                    }
                }
                   
            }

            if (childObj != null)
                childObj.transform.GetChild(0).GetComponent<Text>().text = captPieceText;
            else
            {
                Debug.LogError("Error in AddCapturedPiece() : ChildObj cannot be null");
                // ReSharper disable once RedundantJumpStatement
                return;
            }
        }

        // Opens an action menu (from 1 to 4 actions) on front/around a given piece, containing given actions
        public void OpenActionMenu(GameModel.Piece piece, List<ActionType> actions)
        {
            int menuType = actions.Count - 1;

            // Setting buttons' sprites and tag
            for(int i = 0; i < menuType; i++)
            {
                m_menuArray[menuType].transform.GetChild(i).GetComponent<ActionButton>().Type = actions[i];
            }

            m_menuArray[menuType].transform.position = Board.Instance().GetPieceState(piece).transform.position + Vector3.one;
            m_menuArray[menuType].SetActive(true);
        }

        // Closes any opened menu (normally only one menu can exist at a time, anyway)
        public void CloseActionMenu()
        {
            foreach (GameObject menu in m_menuArray)
            {
                if(menu.activeInHierarchy)
                    menu.SetActive(false);
            }
        }
        
        /* UNITY METHODS */
        private void Awake()
        {
            if(m_instance == null)
                m_instance = this;
            if(m_instance != this)
                Destroy(gameObject);
            
            foreach (GameObject item in m_menuArray)
                item.SetActive(false);
            for (int i = 0; i < m_whiteCaptpuredPieces.transform.childCount; i++)
            {
                m_whiteCaptpuredPieces.transform.GetChild(i).gameObject.SetActive(false);
                m_whiteCaptpuredPieces.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(false);
            }
            for (int i = 0; i < m_blackCapturedPieces.transform.childCount; i++)
            {
                m_blackCapturedPieces.transform.GetChild(i).gameObject.SetActive(false);
                m_blackCapturedPieces.transform.GetChild(i).transform.GetChild(i).gameObject.SetActive(false);
            }
        }


        /* ACCESSORS */
        public UIManager Instance()
        {
            if (!m_instance)
            {
                m_instance = FindObjectOfType(typeof(UIManager)) as UIManager;
                if (!m_instance)
                    Debug.LogError("UIManager : ERROR -- No instance found");
            }

            return m_instance;
        }

        public List<GameObject> ActionMenuList
        {
            get { return m_menuArray.ToList(); }
            set { m_menuArray = value.ToArray(); }
        }
    }
}
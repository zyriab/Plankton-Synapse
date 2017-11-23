using UnityEngine;
using UnityEngine.UI;

// Represents a button (UnityEngine.UI) in the action menu
// Clicking this button would call the Input/Ouput Manager (Presenter) to perform verifications & all about the action
// Would then close the menu
// If the action is legal, will be pending in queue until a destination square is selected
namespace GameView
{
    public class ActionButton : MonoBehaviour
    {
        private ActionType m_type;
        private Button m_button;
        private Image m_image;
        
        /* UNITY METHODS */
        private void Start()
        {
            m_button = this.GetComponent<Button>();
            m_image = this.GetComponent<Image>();
        }

        /* ACCESSORS */
        
        // Setting the button's type changes its GameObject tag, button sprite and clear+update its listeners
        public ActionType Type
        {
            get { return m_type; }

            set
            {
                m_type = value;
                m_button.onClick.RemoveAllListeners();

                switch (value)
                {
                    case ActionType.Create:
                        m_image.sprite = Resources.Load<Sprite>("Assets/Sprites/Create.png");
                        this.gameObject.tag = "Create";
                        break;
                    case ActionType.Move:
                        m_image.sprite = Resources.Load<Sprite>("Assets/Sprites/Move.png");
                        this.gameObject.tag = "Move";
                        break;
                    case ActionType.Swap:
                        m_image.sprite = Resources.Load<Sprite>("Assets/Sprites/Move.png");
                        this.gameObject.tag = "Swap";
                        break;
                    case ActionType.Transform:
                        m_image.sprite = Resources.Load<Sprite>("Assets/Sprites/Transform.png");
                        this.gameObject.tag = "Transform";
                        break;
                    case ActionType.DeleteThis:
                        m_image.sprite = Resources.Load<Sprite>("Assets/Sprites/Apoptosis.png");
                        this.gameObject.tag = "DeleteThis";
                        break;
                    case ActionType.DeleteLastSwapped:
                        m_image.sprite = Resources.Load<Sprite>("Assets/Sprites/DeleteLastSwapped.png");
                        break;
                    case ActionType.SkipTurn:
                        m_image.sprite = Resources.Load<Sprite>("Assets/Sprites/Pass.png");
                        this.gameObject.tag = "SkipTurn";
                        break;
                    default:
                        Debug.LogError("Error in defining the actions icon. -- Invalid action type");
                        return;
                }
                
                // Here we add the action to the queue (IOManager), then close the action menu
                // QueueAction() will queue the action waiting then for the player to select a destination square
                // Note that all the actions here have already been verified and are therefore legal
                m_button.onClick.AddListener( () => { AppManagers.IOManager.QueueAction(value); }); // calling presenter action-related method
                m_button.onClick.AddListener(AppManagers.UIManager.CloseActionMenu); // closing action menu
            }
        }        
    } // endof class ActionButton
} // endof namespace GameView
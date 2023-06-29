using UnityEngine;
using UnityEngine.UI;

namespace Game.Inventory
{
    public class InventoryUIController : MonoBehaviour
    {
        public Image background;
        public Slot[] slots;
    
        private bool _canInput = true, _isOpen;

        private InventoryManager _inventoryManager;

        private void Start()
        {
            _inventoryManager = GetComponent<InventoryManager>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab) && _canInput)
            {
                if (_isOpen)
                {
                    Close();
                }
                else
                {
                    Open();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && _isOpen && _canInput)
            {
                Close();
            }
        }

        private void Open()
        {
            background.gameObject.SetActive(true);
        
            // 커서 표시
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            _isOpen = true;
            _canInput = true;
        }

        private void Close()
        {
            background.gameObject.SetActive(false);
        
            // 커서 끄기
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            _isOpen = false;
            _canInput = true;
        }
    }
}
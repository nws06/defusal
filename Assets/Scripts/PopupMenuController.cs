using UnityEngine;

public class PopupMenuController : MonoBehaviour
{
    [SerializeField] private Canvas menu;   // given in inspector

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
            ToggleMenu();
    }

    void ToggleMenu()
    {
        menu.enabled = !menu.enabled;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class MenuButtonDirection
{
    public Image menuButton;
    public Direction direction;
}

[System.Serializable]
public class MenuButton
{
    public Image button;
    public bool isAvailable = true;
    public string name;
    public List<MenuButtonDirection> nextButtons;
}
public class GenericMenu : MonoBehaviour
{

    public List<MenuButton> buttons = new List<MenuButton>();

    protected MenuButton selectedButton;
    public bool isBlocked = false;
    private float minTimeChangeMenu;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (buttons.Count > 0)
        {
            selectedButton = buttons[0];
            selectedButton.button.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(isBlocked)
        {
            return;
        }

        minTimeChangeMenu += Time.deltaTime;

        if(minTimeChangeMenu > 0.3f)
        {
            int x = (int)Input.GetAxisRaw("Horizontal");
            int y = (int)Input.GetAxisRaw("Vertical");

            if (x != 0)
            {
                SelectButton(x > 0 ? Direction.RIGHT : Direction.LEFT);
                
            }

            if (y != 0)
            {
                SelectButton(y < 0 ? Direction.DOWN : Direction.UP);
            }
        }
        

        if (Input.GetButtonDown("Fire1"))
        {
            PressMenu();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            CancelMenu();
        }

        foreach (MenuButton bt in buttons)
        {
            if(bt != null)
            {
                bt.button.GetComponentInChildren<TMP_Text>().color = bt.isAvailable ? Color.black : Color.gray;
            }
        }
    }

    public void SelectButton(Direction direction)
    {
        minTimeChangeMenu = 0;
        MenuButtonDirection nextButtonDirection = selectedButton.nextButtons.Find((bt) => bt.direction == direction);

        MenuButton nextButton = buttons.Find((bt) => bt.button == nextButtonDirection.menuButton && bt.isAvailable);

        if (nextButton != null)
        {
            if(nextButton != null)
            {
                selectedButton.button.enabled = false;
                nextButtonDirection.menuButton.enabled = true;
                selectedButton = buttons.Find((bt) => bt.button == nextButtonDirection.menuButton);
            } 
            
        } else
        {
            MenuButton newSelectedButtonAvailable = buttons.Find((bt) => bt.button.name != selectedButton.button.name && bt.isAvailable);

            if (newSelectedButtonAvailable != null)
            {
                selectedButton.button.enabled = false;
                selectedButton = newSelectedButtonAvailable;
                selectedButton.button.enabled = true;
            }
        }

    }

    public void ResetSelection()
    {
        if(selectedButton != null)
        {
            selectedButton.button.enabled = false;
            selectedButton = buttons[0];
            selectedButton.button.enabled = true;
        }
    }

    public void PressMenu()
    {
        OnMenuPressed(selectedButton);
    }

    public void CancelMenu()
    {
        OnMenuCanceled();
    }

    protected virtual void OnMenuPressed(MenuButton button) { }
    protected virtual void OnMenuCanceled() { }

}

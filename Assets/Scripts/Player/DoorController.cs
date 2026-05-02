using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject door;
    public bool isLeftDoor = true;
    public bool startClosed = false; 

    public bool isDoorClosed = false;

    void Start()
    {
        isDoorClosed = startClosed;
        door.SetActive(isDoorClosed);
    }

    public void ToggleDoor()
    {
        isDoorClosed = !isDoorClosed;
        door.SetActive(isDoorClosed);
        if (isLeftDoor)
            GameManager.Instance.ToogleDoor1(isDoorClosed);
        else
            GameManager.Instance.ToogleDoor2(isDoorClosed);
    }

    public void ForceOpen()
    {
        isDoorClosed = false;
        door.SetActive(false);
        if (isLeftDoor)
            GameManager.Instance.ToogleDoor1(false);
        else
            GameManager.Instance.ToogleDoor2(false);
    }

    public bool IsClosed() => isDoorClosed;
}
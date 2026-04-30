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

        if (isLeftDoor)
            GameManager.Instance.isDoor1Closed = isDoorClosed;
        else
            GameManager.Instance.isDoor2Closed = isDoorClosed;
    }

    public void ToggleDoor()
    {
        isDoorClosed = !isDoorClosed;
        door.SetActive(isDoorClosed);

        if (isLeftDoor)
            GameManager.Instance.isDoor1Closed = isDoorClosed;
        else
            GameManager.Instance.isDoor2Closed = isDoorClosed;
    }

    public void ForceOpen(){
        isDoorClosed = false;
        door.SetActive(false);
        if (isLeftDoor)
            GameManager.Instance.isDoor1Closed = false;
        else
            GameManager.Instance.isDoor2Closed = false;
    }

    public bool IsClosed() => isDoorClosed;
}
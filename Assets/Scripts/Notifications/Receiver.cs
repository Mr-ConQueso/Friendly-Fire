using UnityEngine;

public class Receiver : MonoBehaviour
{
    private void On_ReceiveMessage(string message)
    {
        Debug.Log(message);
    }
}

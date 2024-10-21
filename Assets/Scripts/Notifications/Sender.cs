using UnityEngine;

public class Sender : MonoBehaviour
{
    [SerializeField] private Transform receiver;
    
    void Update()
    {
        if (Input.anyKey)
        {
            receiver.SendMessage("On_ReceiveMessage","you are gay", SendMessageOptions.DontRequireReceiver);
        }
    }
}

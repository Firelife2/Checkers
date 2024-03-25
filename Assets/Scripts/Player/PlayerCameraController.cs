using Mirror;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] GameObject myCamera;
    [SerializeField] Vector3 whiteCameraPosition, blackCameraPosition, 
        whiteCameraRotation, blackCameraRotation; 

    void Start()
    {
        if (LocalGameManager.Instance) return;
        myCamera.SetActive(true);
        if (NetworkManager.singleton.numPlayers == 0)
        {
            myCamera.transform.position = blackCameraPosition;
            myCamera.transform.rotation = Quaternion.Euler(blackCameraRotation);
        }
        else
        {
            myCamera.transform.position = whiteCameraPosition;
            myCamera.transform.rotation = Quaternion.Euler(whiteCameraRotation);
        }
    }
}

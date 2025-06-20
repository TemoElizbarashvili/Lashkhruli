using UnityEngine;

public class CheckPointSystem : MonoBehaviour
{
    #region Private Variables

    private GameObject player;
    private GameObject checkPoint;

    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // At the starting of the scene, get the starting point "checkPoint" and spawn player there
        checkPoint = GameObject.FindGameObjectWithTag("StartPoint");
        player.transform.position = checkPoint.transform.position;
    }

    public void RespawnPlayer()
    {
        // Respawn player at the checkpoint position
        player.transform.position = checkPoint.transform.position;
    }

    public void SetCheckPoint(GameObject newCheckPoint)
        => checkPoint = newCheckPoint;
}

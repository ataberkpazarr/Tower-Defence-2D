using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour //VEctor 3 positions in space
{
    [SerializeField] private Vector3[] points;

    private Vector3 _currentPosition; // store the current position of our waypoint game object
    private bool _gameStarted; // if we are in play mode or not

    public Vector3[] Points =>points; // private points maden reachable by outside of this class
    public Vector3 currentPosition => _currentPosition; // private currentposition maden reachable by outside of this class

    // Start is called before the first frame update
    void Start()
    {
        _gameStarted = true;
        _currentPosition = transform.position;
    }

    private void OnDrawGizmos() // a line should be drawn by one position to next position in path, except the last position since it will not have a next position 
    {
        if (!_gameStarted && transform.hasChanged) // if game has not started and if the transform has changed which means if the waypoint game object dragged into somewhere else
        {
            _currentPosition = transform.position;
        }
        
        for (int i = 0; i < points.Length; i++)
        {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(points[i] + _currentPosition, 0.5f);

                if (i < points.Length - 1) // dont want to draw line which will start from last position 
                {
                    Gizmos.color = Color.gray;
                    Gizmos.DrawLine(points[i] + _currentPosition, points[i + 1]+ _currentPosition);
                }
        }
    }

    public Vector3 GetWaypointPosition(int index) // returning the index certain waypoint which corresponds to next position in enemy's movement
    {

        return currentPosition + Points[index];
    }
}

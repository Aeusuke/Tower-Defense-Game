using UnityEngine;

// Represents a point which enemy move towards
public class MovementPoint : MonoBehaviour
{
    [SerializeField] int pointNumber = 0; // Used for representing the order in which the enemy moves toward the points
    public int PointNumber { get { return pointNumber; }}

}

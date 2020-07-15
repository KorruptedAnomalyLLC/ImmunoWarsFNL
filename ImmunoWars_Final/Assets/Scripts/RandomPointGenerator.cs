using UnityEngine;

public class RandomPointGenerator : MonoBehaviour
{
    /// <summary>
    /// This script generates a random point based on the range box provided
    /// </summary>
    
    [SerializeField]
    private Transform rangeBox;

    private Vector2 rangeX, rangeY, valueToPass;


    /// <summary>
    /// Returns a Vector3 within the defined rangeBox
    /// </summary>
    public Vector3 GeneratePoint()
    {
        CalculateRange();

        valueToPass.x = Random.Range(rangeX.y, rangeX.x);
        valueToPass.y = Random.Range(rangeY.y, rangeY.x);

        return new Vector3(valueToPass.x, valueToPass.y, 0);
    }

    /// <summary>
    /// Calculates the range in world space that the points can be generated in
    /// The rangeBox determines where that range is
    /// </summary>
    private void CalculateRange()
    {
        rangeX.x = rangeBox.position.x + rangeBox.localScale.x / 2;
        rangeX.y = rangeBox.position.x - rangeBox.localScale.x / 2;

        rangeY.x = rangeBox.position.y + rangeBox.localScale.y / 2;
        rangeY.y = rangeBox.position.y - rangeBox.localScale.y / 2;
    }
}

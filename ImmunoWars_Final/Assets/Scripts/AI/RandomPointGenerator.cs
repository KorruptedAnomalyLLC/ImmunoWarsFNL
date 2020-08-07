using UnityEngine;

public class RandomPointGenerator : MonoBehaviour
{
    /// <summary>
    /// This script generates a random point based on the range box provided
    /// </summary>
    
    [SerializeField]
    private Transform rangeBox;

    private LocalBlackboard _localBlackboard;

    private Vector2 rangeX, rangeY, valueToPass;


    public void Setup()
    {
        _localBlackboard = GetComponent<LocalBlackboard>();

        if(_localBlackboard == null)
            Debug.LogError("No LocalBlackboard script attached, please attach one to ", this.gameObject);

    }

    /// <summary>
    /// Returns a Vector3 within the defined rangeBox
    /// </summary>
    public Vector3 GeneratePoint()
    {
        CalculateRange();

        valueToPass.x = Random.Range(rangeX.y, rangeX.x);
        valueToPass.y = Random.Range(rangeY.y, rangeY.x);

        return new Vector3(valueToPass.x, GlobalBlackboard.Instance.playfieldHeight, valueToPass.y);
    }

    /// <summary>
    /// Calculates the range in world space that the points can be generated in
    /// The rangeBox determines where that range is
    /// </summary>
    private void CalculateRange()
    {
        rangeX.x = rangeBox.position.x + rangeBox.localScale.x / 2;
        rangeX.y = rangeBox.position.x - rangeBox.localScale.x / 2;

        rangeY.x = rangeBox.position.z + rangeBox.localScale.z / 2;
        rangeY.y = rangeBox.position.z - rangeBox.localScale.z / 2;
    }
}

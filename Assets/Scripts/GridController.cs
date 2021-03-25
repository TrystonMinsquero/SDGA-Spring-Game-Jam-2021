using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Vector2Int gridSize;
    public float cellRadius = 0.5f;
    public FlowField curFlowField;
    public Transform target;

    void Start()
    {
        InvokeRepeating("UpdateGrid", 0, 0.5f);
    }

    private void InitializeFlowField()
    {
        curFlowField = new FlowField(cellRadius, gridSize);
        curFlowField.CreateGrid();
    }

    public void UpdateGrid()
    {
            InitializeFlowField();
            curFlowField.CreateCostField();

            Cell destinationCell = curFlowField.GetCellFromWorldPos(target.position);
            curFlowField.CreateIntegrationField(destinationCell);

            curFlowField.CreateFlowField();

    }
}

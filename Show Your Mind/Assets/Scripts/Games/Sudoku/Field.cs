using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class Field : MonoBehaviour
{
    private TouchAction playerInput;

    private Cell[,] _cells = new Cell[9, 9];

    private void Start()
    {
        foreach (Cell cell in GetComponentsInChildren<Cell>())
        {
            int xIndex = (int)cell.transform.position.x + 4;
            int yIndex = 4 - (int)cell.transform.position.y;

            _cells[xIndex, yIndex] = cell;
        }
        playerInput = new TouchAction();
        playerInput.Enable();
    }

    private void Update()
    {
        Vector2 touchPosition = playerInput.Gameplay.TouchPosition.ReadValue<Vector2>();

        if (touchPosition != Vector2.zero)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, Camera.main.nearClipPlane));

            int xIndex = Mathf.FloorToInt(worldPosition.x + 4.5f);
            int yIndex = Mathf.FloorToInt(4.5f - worldPosition.y);
            _cells[xIndex, yIndex].SetText(yIndex + " " + xIndex);
        }
    }
}

using UnityEngine;

public class Cell : MonoBehaviour
{
    private enum color
    {
        None,
        Red,
        Green,
        Blue,
        Yellow,
        Purple,
        Orange
    }

    private int row;
    private int column;
    private int colorIndex;
    private bool hasCat;
    private bool isChecked;

    public void Initialize(int row, int column, int colorIndex, bool hasCat)
    {
        this.row = row;
        this.column = column;
        this.colorIndex = colorIndex;
        this.hasCat = hasCat;
        // Set the cell's color based on the color index
        GetComponent<SpriteRenderer>().color = GetColorFromIndex(colorIndex);
    }

    private Color GetColorFromIndex(int index)
    {
        switch ((color)index)
        {
            case color.None:
                return Color.white;
            case color.Red:
                return Color.red;
            case color.Green:
                return Color.green;
            case color.Blue:
                return Color.blue;
            case color.Yellow:
                return Color.yellow;
            case color.Purple:
                return new Color(0.5f, 0f, 0.5f); // Purple
            case color.Orange:
                return new Color(1f, 0.5f, 0f); // Orange
            default:
                return Color.white;
        }
    }
}

using UnityEngine;

public class NewDokuManager : MonoBehaviour
{
    [SerializeField] private GameObject GameBoard;
    [SerializeField] private GameObject CellContiner;
    [SerializeField] private GameObject Cell;

    private void Start()
    {
        for(int i = 0; i < 5; i++)
        {
            GameObject container = Instantiate(CellContiner, GameBoard.transform);
            for(int j = 0; j < 5; j++)
            {
                GameObject cell = Instantiate(Cell, container.transform);
            }

        }
    }

}

using UnityEngine;

public class NewDokuManager : Singleton<NewDokuManager>
{
    [SerializeField] private GameObject GameBoard;
    [SerializeField] private GameObject CellContiner;
    [SerializeField] private GameObject Cell;

    private CLICKMODE clickMode = CLICKMODE.PLACE_EX;


    private enum CLICKMODE
    {
        NONE,
        PLACE_EX,
        PLACE_CAT,
        TEMP_CAT,
    }

    private void Start()
    {
        for(int i = 0; i < 5; i++)
        {
            GameObject container = Instantiate(CellContiner, GameBoard.transform);
            for(int j = 0; j < 5; j++)
            {
                int cellnumber = i * 5 + j;
                GameObject cell = Instantiate(Cell, container.transform);

                cell.name = "Cell_" + cellnumber;
                cell.GetComponent<Cell>().Initialize(i, j-i*5, Random.Range(1, 7), Random.value > 0.5f);
            }
        }
    }

    public void OnClickCell(Cell cell)
    {
        switch(clickMode)
        {
            case CLICKMODE.PLACE_EX:
                cell.PlaceEx();
                break;
            case CLICKMODE.PLACE_CAT:
                cell.PlaceCat();
                break;
            case CLICKMODE.TEMP_CAT:
                cell.PlaceTempCat();
                break;
        }
    }

    #region ClickMode
    public void ChangeModetoPlaceEx()
    {
        this.clickMode = CLICKMODE.PLACE_EX;
    }
    public void ChangeModetoPlaceCat()
    {
        this.clickMode = CLICKMODE.PLACE_CAT;
    }
    public void ChangeModetoTempCat()
    {
        this.clickMode = CLICKMODE.TEMP_CAT;
    }

    #endregion


}

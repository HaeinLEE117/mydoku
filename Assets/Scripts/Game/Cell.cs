using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] GameObject ImageEx;
    [SerializeField] GameObject ImageCat;
    [SerializeField] GameObject ImageTempCat;

    private Button buttonComponent;

    // 셀 상태 enum
    public enum CellState
    {
        Empty,      // 아무것도 없음
        Ex,         // X 표시
        Cat,        // 고양이
        TempCat     // 임시 고양이
    }

    private CellState currentState = CellState.Empty;

    private enum color
    {
        NONE,
        Red,
        Green,
        Blue,
        Yellow,
        Purple,
        Orange,
        MAX
    }

    private int row;
    private int column;
    private int colorIndex;
    private bool hasCat;

    private void Start()
    {
        buttonComponent = GetComponent<Button>();
        buttonComponent.onClick.AddListener(() => NewDokuManager.Instance.OnClickCell(this));
    }

    public void Initialize(int row, int column, int colorIndex, bool hasCat)
    {
        this.row = row;
        this.column = column;
        this.colorIndex = colorIndex;
        this.hasCat = hasCat;

        GetComponent<Image>().color = GetColorFromIndex(colorIndex);

        // 초기 상태는 Empty
        SetCellState(CellState.Empty);
    }

    /// <summary>
    /// 셀 상태를 변경하고 해당하는 이미지만 활성화
    /// </summary>
    public void SetCellState(CellState newState)
    {
        currentState = newState;

        // 모든 이미지 비활성화
        ImageEx.SetActive(false);
        ImageCat.SetActive(false);
        ImageTempCat.SetActive(false);

        // 현재 상태에 맞는 이미지만 활성화
        switch (currentState)
        {
            case CellState.Ex:
                ImageEx.SetActive(true);
                break;
            case CellState.Cat:
                ImageCat.SetActive(true);
                break;
            case CellState.TempCat:
                ImageTempCat.SetActive(true);
                break;
            case CellState.Empty:
            default:
                // 모두 비활성화 상태 유지
                break;
        }
    }

    /// <summary>
    /// 현재 셀 상태 반환
    /// </summary>
    public CellState GetCellState()
    {
        return currentState;
    }


    private Color GetColorFromIndex(int index)
    {
        switch ((color)index)
        {
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

    #region ClickMode에 따라서 버튼 처리

    /// <summary>
    /// X 표시 토글
    /// </summary>
    public void PlaceEx()
    {
        if (currentState == CellState.Ex)
        {
            SetCellState(CellState.Empty); // 이미 X가 있으면 제거
        }
        else
        {
            SetCellState(CellState.Ex); // X 표시
        }
    }

    /// <summary>
    /// 임시 고양이 토글
    /// </summary>
    public void PlaceTempCat()
    {
        if (currentState == CellState.TempCat)
        {
            SetCellState(CellState.Empty); // 이미 임시 고양이가 있으면 제거
        }
        else
        {
            SetCellState(CellState.TempCat); // 임시 고양이 배치
        }
    }

    /// <summary>
    /// 실제 고양이 배치 (정답 체크)
    /// </summary>
    public bool PlaceCat()
    {
        if (hasCat)
        {
            // 정답 위치에 고양이 배치
            SetCellState(CellState.Cat);
            buttonComponent.enabled = false;
            Debug.Log("고양이를 배치했습니다.");
            return true;
        }
        else
        {
            // 틀린 위치
            SetCellState(CellState.Ex);
            ImageEx.GetComponent<Image>().color = Color.red;
            Debug.Log("틀린 위치");
            buttonComponent.enabled = false;
            return false;
        }
    }

    #endregion
}

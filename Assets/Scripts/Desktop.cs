using UnityEngine;

public class Desktop : MonoBehaviour
{
    Minesweeper minesweeper;
    public void MinesweeperExe()
    {
        if (minesweeper == null)
            minesweeper = Main.Resource.Instantiate("Minesweeper", null).GetComponent<Minesweeper>();

        minesweeper.gameObject.SetActive(true);
        minesweeper.GameSetting();

    }

}

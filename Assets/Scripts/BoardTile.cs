using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoardTile : MonoBehaviour
{

    [SerializeField] Material tileMaterial;
    //Astar
    [SerializeField] TMP_Text totalCostText;
    [SerializeField] TMP_Text hCostText;
    [SerializeField] TMP_Text gCostText;

    public void SetColor(Color color)
    {
        tileMaterial.SetColor("_Color",color);
    }

    public void SetAStarText(int hcost, int gcost, int totalcost)
    {
        hCostText.text = hcost.ToString();
        gCostText.text = gcost.ToString();
        totalCostText.text = totalcost.ToString();
    }
}

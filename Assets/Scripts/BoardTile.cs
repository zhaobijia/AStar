using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoardTile : MonoBehaviour
{
    public Vector2Int Index;
    [SerializeField] Material tileMaterial;
    //Astar
    [SerializeField] TMP_Text totalCostText;
    [SerializeField] TMP_Text hCostText;
    [SerializeField] TMP_Text gCostText;

    [SerializeField] MeshRenderer quadRenderer;

    private void Awake()
    {
        ShowEmptyText();
    }
    public void SetColor(Material mat)
    {
        quadRenderer.material = mat;
    }

    public void SetAStarText(int hcost, int gcost, int totalcost)
    {
        hCostText.text = hcost.ToString();
        gCostText.text = gcost.ToString();
        totalCostText.text = totalcost.ToString();
    }

    public void ShowEmptyText()
    {

        hCostText.enabled = false;
        gCostText.enabled = false;
        totalCostText.enabled = false;
    }

    public void ShowTotalCost()
    {
        hCostText.enabled = false;
        gCostText.enabled = false;
        totalCostText.enabled = true;
    }

    public void ShowAllCost()
    {
        hCostText.enabled = true;
        gCostText.enabled = true;
        totalCostText.enabled = true;
    }
}

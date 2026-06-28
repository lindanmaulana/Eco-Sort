using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic; 
using UnityEngine.UI;          
using TMPro;

public class Tips : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject panelOrganic;
    [SerializeField] private GameObject panelInorganic;
    [SerializeField] private GameObject panelB3;

    [Header("Prefab Setup (Beda Warna)")]
    [SerializeField] private GameObject cardOrganicPrefab;   
    [SerializeField] private GameObject cardInorganicPrefab; 
    [SerializeField] private GameObject cardB3Prefab;

    [Header("Grid & Prefab Setup")] 
    [SerializeField] private Transform organicGrid;     
    [SerializeField] private Transform inorganicGrid; 
    [SerializeField] private Transform b3Grid;        

    [Header("Data List (Scriptable Objects)")]
    [SerializeField] private List<GarbageData> organicGarbageList;  
    [SerializeField] private List<GarbageData> inorganicGarbageList; 
    [SerializeField] private List<GarbageData> b3GarbageList;    

    public void HandleClickPanelOrganic()
    {
        HandleClosePanels();

        if (panelOrganic != null)
        {
            panelOrganic.SetActive(true);
            GenerateOrganicCards();
        } 
    }

    public void HandleClickPanelInorganic()
    {
        HandleClosePanels();

        if (panelInorganic != null) 
        {
            panelInorganic.SetActive(true);
            GenerateInorganicCards(); 
        }
    }

    public void HandleClickPanelB3()
    {
        HandleClosePanels();

        if (panelB3 != null) 
        {
            panelB3.SetActive(true);
            GenerateB3Cards(); 
        }
    }

    public void HandleClosePanels()
    {
        if (panelOrganic != null) panelOrganic.SetActive(false);
        if (panelInorganic != null) panelInorganic.SetActive(false);
        if (panelB3 != null) panelB3.SetActive(false);
    }
    private void GenerateOrganicCards()
    {
        foreach (Transform child in organicGrid)
        {
            Destroy(child.gameObject);
        }

        foreach (GarbageData data in organicGarbageList)
        {
            if (data == null) continue;

            GameObject cardGo = Instantiate(cardOrganicPrefab, organicGrid);
            AppCardTrash controller = cardGo.GetComponent<AppCardTrash>();

            if (controller != null)
            {
                controller.InitializeCard(data);
            }
        }
    }

    private void GenerateInorganicCards()
    {
        foreach (Transform child in inorganicGrid)
        {
            Destroy(child.gameObject);
        }

        foreach (GarbageData data in inorganicGarbageList)
        {
            if (data == null) continue;

            GameObject cardGo = Instantiate(cardInorganicPrefab, inorganicGrid);
            AppCardTrash controller = cardGo.GetComponent<AppCardTrash>();

            if (controller != null)
            {
                controller.InitializeCard(data);
            }
        }
    }

    private void GenerateB3Cards()
    {
        foreach (Transform child in b3Grid)
        {
            Destroy(child.gameObject);
        }

        foreach (GarbageData data in b3GarbageList)
        {
            if (data == null) continue;

            GameObject cardGo = Instantiate(cardB3Prefab, b3Grid);
            AppCardTrash controller = cardGo.GetComponent<AppCardTrash>();

            if (controller != null)
            {
                controller.InitializeCard(data);
            }
        }
    }
}
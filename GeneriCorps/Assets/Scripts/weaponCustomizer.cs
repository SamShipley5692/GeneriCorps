using UnityEngine;
using System.Collections;

public class weaponCustomizer : MonoBehaviour
{
    public string CurrentMenu;

    public GameObject ACR_Weapon;

    public GameObject ACR_Irons;
    public GameObject ACR_EoTech;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentMenu = "Hub";
        ACR_EoTech.SetActive(false);
        ACR_Irons.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NavigateTo(string nextmenu)
    {
        CurrentMenu = nextmenu;
    }


    private void OnGUI()
    {
        if (CurrentMenu == "Hub")
            Hub();
        if (CurrentMenu == "ACR")
            ACR();
        if (CurrentMenu == "ACR_Opics")
            ACR_Op();

    }

    private void Hub()
    {
        if (GUI.Button(new Rect(10, 10, 200, 50), "ACR"))
        {
            NavigateTo("ACR");
        }
    }

    private void ACR()
    {
        if (GUI.Button(new Rect(10, 10, 200, 50), "Back"))
        {
            NavigateTo("Hub");
        }
        if (GUI.Button(new Rect(10, 70, 200, 50), "Optics"))
        {
            NavigateTo("ACR Optics");
        }
    }

    private void ACR_Op()
    {
        if (GUI.Button(new Rect(10, 10, 200, 50), "Back"))
        {
            NavigateTo("Hub");
        }
        if (GUI.Button(new Rect(10, 70, 200, 50), "None"))
        {
            ACR_EoTech.SetActive(false);
            ACR_Irons.SetActive(false);
        }
        if (GUI.Button(new Rect(10, 130, 200, 50), "Irons"))
        {
            ACR_EoTech.SetActive(false);
            ACR_Irons.SetActive(true);
        }
        if (GUI.Button(new Rect(10, 130, 200, 50), "EoTech"))
        {
            ACR_EoTech.SetActive(true);
            ACR_Irons.SetActive(false);
        }






    }

}

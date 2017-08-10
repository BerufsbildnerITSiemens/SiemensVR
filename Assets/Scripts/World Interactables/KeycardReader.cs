using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(VRObjectStand))]
public class KeycardReader : MonoBehaviour
{
    private VRObjectStand stand;
    private Keycard insertedCard;
    public VRButton[] doorButtons;
    public GameObject[] lights;
    public Material on;
    public Material off;

    // Use this for initialization
    void Start()
    {
        stand = GetComponent<VRObjectStand>();
        stand.keycardInserted += OnCardInsert;
        stand.keycardEjected += OnCardEject;

        foreach (var item in doorButtons)
        {
            item.buttonPressed += OnButtonPress;
        }

        UpdateLights();
    }

    private void OnButtonPress(int value)
    {
        if (insertedCard != null)
        {
            if (value < 6 && value > 0)
            {
                changeCardDooState(value);
            }
        }
    }
    private void changeCardDooState(int id)
    {
        if (insertedCard.permittedRooms.Contains(id))
        {
            insertedCard.permittedRooms.Remove(id);
        }
        else
        {
            insertedCard.permittedRooms.Add(id);
        }
        UpdateLights();
    }


    private void OnCardInsert(Keycard card)
    {
        if (insertedCard != null)
        {
            throw new System.Exception("Fatal Error");
        }
        insertedCard = card;
        UpdateLights();

    }

    private void OnCardEject(Keycard card)
    {
        insertedCard = null;
        foreach (var item in lights)
        {
            item.GetComponent<MeshRenderer>().material = off;
        }
    }

    private void UpdateLights()
    {
        if (insertedCard != null)
        {
            for (int x = 0; x < lights.Length; x++)
            {
                if (insertedCard.permittedRooms.Contains(x + 1))
                {
                    lights[x].GetComponent<MeshRenderer>().material = on;
                }
                else
                {
                    lights[x].GetComponent<MeshRenderer>().material = off;
                }
            }
        }
    }





}

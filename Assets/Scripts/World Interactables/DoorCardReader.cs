using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCardReader : MonoBehaviour {
    public enum CardReaderStatus { locked = 0, unlocked = 1};

    public GameObject statuslight;
    public Material lightMaterialUnlocked;
    public Material lightMaterialLocked;
    private CardReaderStatus status;
    public int relatedRoom;
    public GameObject relatedDoor;
    private bool lockflag;
    


	// Use this for initialization
	void Start () {
        status = CardReaderStatus.locked;
        lockflag = false;
	}
	
	// Update is called once per frame
	void Update () {
        //if(relatedDoor.transform.localRotation.z <= -180)
        //{
        //    Debug.Log("Should Lock Door");
        //    LockRelatedDoor();
        //}
        if(relatedDoor.GetComponent<HingeJoint>().angle < -5)
        {
            lockflag = true;
        }

            //Debug.Log(relatedDoor.GetComponent<HingeJoint>().angle.ToString());
        if(relatedDoor.GetComponent<HingeJoint>().angle >= 0 && lockflag)
        {
            //Debug.Log("Should Lock Door");
            LockRelatedDoor();
            lockflag = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger Entered");
        if (other.GetComponent<Keycard>())                                                                          // check if colliding Object is a Keycard
        {
            //Debug.Log("other is Keycard");
            foreach (int permittedRoom in other.GetComponent<Keycard>().permittedRooms)
            {
                if(permittedRoom == relatedRoom)                                                                    // check if keycard has the to the cardreader related room in its permittedRooms-List
                {
                    //Debug.Log("Keycards permittedRoom is relatedRoom");
                    switch (status)
                    {
                        case CardReaderStatus.locked:                                                               // if status of CardReader is "locked" set it to "unlocked", change material and unfreeze the doors rotation
                            UnlockRelatedDoor();
                            break;
                        case CardReaderStatus.unlocked:                                                             // if status of CardReader is "unlocked" set it to "locked", change material and freeze the doors rotation
                            LockRelatedDoor();
                            break;
                    }
                }
            }
        }
    }

    public void LockRelatedDoor()
    {
        status = CardReaderStatus.locked;
        statuslight.GetComponent<MeshRenderer>().material = lightMaterialLocked;
        relatedDoor.GetComponent<Rigidbody>().freezeRotation = true;
    }

    public void UnlockRelatedDoor()
    {
        status = CardReaderStatus.unlocked;
        statuslight.GetComponent<MeshRenderer>().material = lightMaterialUnlocked;
        relatedDoor.GetComponent<Rigidbody>().freezeRotation = false;
    }
}

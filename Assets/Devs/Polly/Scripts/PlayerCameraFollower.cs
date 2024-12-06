using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraFollower : MonoBehaviour
{
    [SerializeField] float zoomDistance;
    [SerializeField] float zoomSpeed;
    public bool playersDoneSpawing = false;
    // Update is called once per frame
    void FixedUpdate()
    {
        if(PlayerDataStorage.connectedPlayerObjects.Count > 0 && playersDoneSpawing)
        {
            List<GameObject> objects = new();
            List<float> distances = new();
            List<GameObject> activePlayerObjects = new();
            activePlayerObjects.Clear();
            objects.Clear();
            for(int i = 0; i < PlayerDataStorage.connectedPlayerObjects.Count; i++)
            {
                if (PlayerDataStorage.connectedPlayerObjects[i].activeSelf) activePlayerObjects.Add(PlayerDataStorage.connectedPlayerObjects[i]);
                else if (PlayerDataStorage.connectedPlayerObjects[i].GetComponent<InputHandler>().spawnedRagdoll != null) activePlayerObjects.Add(PlayerDataStorage.connectedPlayerObjects[i].GetComponent<InputHandler>().spawnedRagdoll.transform.GetChild(4).GetChild(0).gameObject);
            }
            // Algorithm that gets the 2 furthest objects from the camera from all player objects present.
            for(int i = 0; i < activePlayerObjects.Count; i++)
            {
                GameObject curObj = activePlayerObjects[i];
                if(curObj.GetComponent<InputHandler>().isRagdolling) curObj = curObj.GetComponent<InputHandler>().spawnedRagdoll.transform.GetChild(4).GetChild(0).gameObject;
                if(distances.Count > 0)
                {
                    if (Vector3.Distance(curObj.transform.position, transform.position) > distances[0])
                    {
                        distances.Insert(0, Vector3.Distance(curObj.transform.position, transform.position));
                        objects.Insert(0, curObj);
                    } 
                    else if(distances.Count > 1)
                    {
                        if(Vector3.Distance(curObj.transform.position, transform.position) > distances[1])
                        {
                            distances.Insert(1, Vector3.Distance(curObj.transform.position, transform.position));
                            objects.Insert(1, curObj);
                    }
                    }
                    else
                    {
                        distances.Add(Vector3.Distance(curObj.transform.position, transform.position));
                        objects.Add(curObj);
                    }
                }
                else
                {
                    distances.Add(Vector3.Distance(curObj.transform.position, transform.position));
                    objects.Add(curObj);
                }
            }

            // If there's only 1 player, the camera just focuses on them.
            if (activePlayerObjects.Count == 1)
            {
                foreach (GameObject g in GameObject.FindGameObjectsWithTag("Ragdoll"))
                {
                    if (g.transform.root.gameObject.GetComponent<SpawnedRagdoll>().originPlayer == activePlayerObjects[0]) activePlayerObjects[0] = g.transform.root.GetChild(4).GetChild(0).gameObject;
                }
                transform.position = activePlayerObjects[0].transform.position - transform.forward * zoomDistance * 10;
            }
            // If there's MORE than 1 player, however...
            // It gets the total distance between the 2 furthest player objects, then moves the camera back based on that distance and the difference between those
            // 2 objects' z coordinates. I love coding. Coding is awesome. This definitely didn't take me an hour to write.
            else if(activePlayerObjects.Count > 1)
            {
                Vector3 middlePosition = new();
                for (int i = 0; i < activePlayerObjects.Count; i++)
                {
                    middlePosition += activePlayerObjects[i].transform.position;
                }
                middlePosition /= activePlayerObjects.Count;
                // This line of code took a solid 10 years off my life span.
                transform.position = Vector3.Slerp(transform.position, middlePosition - transform.forward * 2 - transform.forward * (zoomDistance * Vector3.Distance(objects[0].transform.position, objects[1].transform.position)) - transform.forward * (objects[0].transform.position.z - objects[1].transform.position.z) / 3.5f, 0.1f);
            }
        }
    }
}

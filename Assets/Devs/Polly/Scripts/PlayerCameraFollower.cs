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
    float zoomDistance = 0.8f;
    [SerializeField] float zoomSpeed;
    public bool playersDoneSpawing = false;
    public Vector3 intendedPos;

    private void Start()
    {
        intendedPos = transform.position;
    }
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
                if (PlayerDataStorage.connectedPlayerObjects[i].GetComponent<InputHandler>().spawnedRagdoll == null)
                {
                    if (PlayerDataStorage.connectedPlayerObjects[i].activeSelf)
                    activePlayerObjects.Add(PlayerDataStorage.connectedPlayerObjects[i]);
                }
                else
                {
                    activePlayerObjects.Add(PlayerDataStorage.connectedPlayerObjects[i].GetComponent<InputHandler>().spawnedRagdoll.transform.GetChild(4).GetChild(0).gameObject);
                }
            }
            Vector3 middlePosition = new();
            for (int i = 0; i < activePlayerObjects.Count; i++)
            {
                middlePosition += activePlayerObjects[i].transform.position;
            }
            middlePosition /= activePlayerObjects.Count;

            // Algorithm that gets the 2 furthest objects from the camera from all player objects present.
            if (activePlayerObjects.Count > 2)
            {
                for (int i = 0; i < activePlayerObjects.Count; i++)
                {
                    GameObject curObj = activePlayerObjects[i];
                    if (distances.Count > 0)
                    {
                        if (Vector3.Distance(curObj.transform.position, middlePosition) > distances[0])
                        {
                            distances.Insert(0, Vector3.Distance(curObj.transform.position, middlePosition));
                            objects.Insert(0, curObj);
                        }
                        else if (distances.Count > 1)
                        {
                            if (Vector3.Distance(curObj.transform.position, middlePosition) > distances[1])
                            {
                                distances.Insert(1, Vector3.Distance(curObj.transform.position, middlePosition));
                                objects.Insert(1, curObj);
                            }
                        }
                        else
                        {
                            distances.Add(Vector3.Distance(curObj.transform.position, middlePosition));
                            objects.Add(curObj);
                        }
                    }
                    else
                    {
                        distances.Add(Vector3.Distance(curObj.transform.position, middlePosition));
                        objects.Add(curObj);
                    }
                }
            }
            // If there's 2 players, the camera focuses on the point between the two.
            if(activePlayerObjects.Count == 2)
            {
                Vector3 averagePos = (activePlayerObjects[0].transform.position + activePlayerObjects[1].transform.position) / 2;
                float playerDist = Vector3.Distance(activePlayerObjects[0].transform.position, activePlayerObjects[1].transform.position);
                playerDist = Mathf.Clamp(playerDist, 10, 100000000);
                float baseZoomDist = 0;
                for(int i = 0; i < activePlayerObjects.Count; i++)
                {
                    if (activePlayerObjects[i].transform.position.z > baseZoomDist)
                    {
                        baseZoomDist = activePlayerObjects[i].transform.position.z;
                    }
                }
                float zoomDist = baseZoomDist / 2f;
                intendedPos = averagePos - transform.forward * 2 - transform.forward * (zoomDistance * playerDist) - transform.forward * zoomDist;
            }
            
            // If there's only 1 player, the camera just focuses on them.
            if (activePlayerObjects.Count == 1)
            {
                foreach (GameObject g in GameObject.FindGameObjectsWithTag("Ragdoll"))
                {
                    if (g.transform.root.gameObject.GetComponent<SpawnedRagdoll>().originPlayer == activePlayerObjects[0]) activePlayerObjects[0] = g.transform.root.GetChild(4).GetChild(0).gameObject;
                }
                intendedPos = activePlayerObjects[0].transform.position - transform.forward * zoomDistance * 10;
            }
            // If there's MORE than 2 players, however...
            // It gets the total distance between the 2 furthest player objects, then moves the camera back based on that distance and the difference between those
            // 2 objects' z coordinates. I love coding. Coding is awesome. This definitely didn't take me an hour to write.
            if(activePlayerObjects.Count > 2)
            {
                // This line of code took a solid 10 years off my life span.
                float playerDistance = Vector3.Distance(objects[0].transform.position, objects[1].transform.position);
                playerDistance = Mathf.Clamp(playerDistance, 10, 1000000);
                float baseZoomDist = 0;
                for (int i = 0; i < activePlayerObjects.Count; i++)
                {
                    if (activePlayerObjects[i].transform.position.z > baseZoomDist)
                    {
                        baseZoomDist = activePlayerObjects[i].transform.position.z;
                    }
                }
                float zoomDist = baseZoomDist / 2f;
                intendedPos = middlePosition - transform.forward * 2 - transform.forward * (zoomDistance * playerDistance) - transform.forward * zoomDist;
            }
            transform.position = Vector3.Lerp(transform.position, intendedPos, 0.05f);
        }
    }
}

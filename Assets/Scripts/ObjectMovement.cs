using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    void Update()
    {
        if (GameManager._canMove) // Start & Stop
        {
            transform.position -= new Vector3(0f, 0f, GameManager._worldSpeed * Time.deltaTime); //Remove 0 from X, 0 from Y and movespeed * time from Z axis.
        }

        if (transform.position.z < GameObject.Find("Deletion Threshold").transform.position.z) //If an object reach the deletion pos.
        {
            if (gameObject.tag.Equals("Collactable"))
            {
                int randomPos = Random.Range(4, 20);
                transform.position = (GameObject.Find("Generation Threshold").transform.position + (new Vector3(0, 1, -1) * randomPos));
                transform.GetChild(0).gameObject.SetActive(true);
            }
            else if (gameObject.tag.Equals("Obstacles"))
            {
                int randomZ = Random.Range(30, 60); //to prevent the collision between new generated objects and reused ones
                transform.position = new Vector3(transform.position.x, 0f, randomZ);
                gameObject.SetActive(false);
            }
            else if (gameObject.tag.Equals("Rocks"))
            {
                transform.position += Vector3.forward * 170f; //Repositioning the trees
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}

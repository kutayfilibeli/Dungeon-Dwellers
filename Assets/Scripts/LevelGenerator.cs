using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public GameObject layoutRoom;
    public int distanceToEnd;
    public bool includeShop;
    public int minDistanceToShop, maxDistanceToShop;

    public Color startColor, endColor, shopColor;

    public Transform generatorPoint;

    public enum Direction { up, right, down, left };

    public Direction selectedDirection;
    
    public float xOffset = 18f;
    public float yOffset = 10f;

    public LayerMask roomLayer;

    private GameObject endRoom,shopRoom;

    private List<GameObject> layoutRoomObjects = new List<GameObject>();

    public RoomPrefabs rooms;

    private List<GameObject> generatedOutlines = new List<GameObject>();

    public RoomFloor startFloor, endFloor, shopFloor;
    public RoomFloor[] potentialFloors;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(layoutRoom,generatorPoint.position, generatorPoint.rotation ).GetComponent<SpriteRenderer>().color = startColor;

        selectedDirection = (Direction)Random.Range(0, 4);
        MoveGenerationPoint();

        for(int i = 0; i < distanceToEnd; i++)
        {
           GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);

            layoutRoomObjects.Add(newRoom);

            if(i+1 == distanceToEnd)
            {
                newRoom.GetComponent<SpriteRenderer>().color = endColor;

                layoutRoomObjects.RemoveAt(layoutRoomObjects.Count - 1);

                endRoom = newRoom;
            }

            selectedDirection = (Direction)Random.Range(0, 4);
            MoveGenerationPoint();

            while (Physics2D.OverlapCircle(generatorPoint.position, .2f, roomLayer))
            {
                MoveGenerationPoint();
            }



        }

        if (includeShop)
        {
            int shopSelector = Random.Range(minDistanceToShop, maxDistanceToShop + 1);
            shopRoom = layoutRoomObjects[shopSelector];
            layoutRoomObjects.RemoveAt(shopSelector);
            shopRoom.GetComponent<SpriteRenderer>().color = shopColor;
        }

        CreateRoomOutline(Vector3.zero);
        foreach(GameObject room in layoutRoomObjects)
        {

            CreateRoomOutline(room.transform.position);
        }
        CreateRoomOutline(endRoom.transform.position);
        if (includeShop)
        {
            CreateRoomOutline(shopRoom.transform.position);
        }

        foreach(GameObject outline in generatedOutlines)
        {
            bool generateFloor = true;

            if(outline.transform.position == Vector3.zero)
            {
                Instantiate(startFloor, outline.transform.position, transform.rotation).room = outline.GetComponent<Room>();
                generateFloor = false;
            }

            if(outline.transform.position == endRoom.transform.position)
            {
                Instantiate(endFloor, outline.transform.position, transform.rotation).room = outline.GetComponent<Room>();
                generateFloor = false;
            }

            if (includeShop)
            {
                if(outline.transform.position == shopRoom.transform.position)
                {
                    Instantiate(shopFloor, outline.transform.position, transform.rotation).room = outline.GetComponent<Room>();
                    generateFloor = false;
                }
            }

            if (generateFloor)
            {
                int floorSelect = Random.Range(0, potentialFloors.Length);

                Instantiate(potentialFloors[floorSelect], outline.transform.position, transform.rotation).room = outline.GetComponent<Room>();
            }
            


        }
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR

        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
#endif
    }

    public void MoveGenerationPoint()
    {
        switch(selectedDirection)
        {
            case Direction.up:
                generatorPoint.position += new Vector3(0f, yOffset, 0f);
                break;
            case Direction.down:
                generatorPoint.position += new Vector3(0f, -yOffset, 0f);
                break;
            case Direction.right:
                generatorPoint.position += new Vector3(xOffset, 0f, 0f);
                break;
            case Direction.left:
                generatorPoint.position += new Vector3(-xOffset, 0f, 0f);
                break;

        }
    }
    public void CreateRoomOutline(Vector3 roomPosition)
    {
        bool roomAbove = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, yOffset, 0f), .2f, roomLayer);
        bool roomBelow = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, -yOffset, 0f), .2f, roomLayer);
        bool roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0f, 0f), .2f, roomLayer);
        bool roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0f, 0f), .2f, roomLayer);

        int directionCount = 0;
        if (roomAbove)
        {
            directionCount++;
        }
        if (roomBelow)
        {
            directionCount++;
        }
        if (roomRight)
        {
            directionCount++;
        }
        if (roomLeft)
        {
            directionCount++;
        }
        switch (directionCount)
        {
            case 1:
                if (roomAbove)
                {
                 generatedOutlines.Add(Instantiate(rooms.singleUp, roomPosition, transform.rotation));
                }

                if (roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleDown, roomPosition, transform.rotation));
                }

                if (roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleLeft, roomPosition, transform.rotation));
                }

                if (roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleRight, roomPosition, transform.rotation));
                }
                break;
            case 2:
                if (roomAbove && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpDown, roomPosition, transform.rotation));
                }

                if (roomLeft && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftRight, roomPosition, transform.rotation));
                }

                if(roomAbove && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpRight, roomPosition, transform.rotation));
                }

                if(roomBelow && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleDownLeft, roomPosition, transform.rotation));
                }

                if(roomAbove && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpLeft, roomPosition, transform.rotation));
                }

                if(roomBelow && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleDownRight, roomPosition, transform.rotation));
                }
                break;
            case 3:
                if(roomAbove && roomBelow && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleUpRightDown, roomPosition, transform.rotation));
                }

                if(roomRight && roomBelow && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleRightDownLeft, roomPosition, transform.rotation));
                }

                if(roomLeft && roomBelow && roomAbove)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleDownLeftUp, roomPosition, transform.rotation));
                }

                if(roomLeft && roomRight && roomAbove)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleLeftUpRight, roomPosition, transform.rotation));
                }

                break;
            case 4:
                if(roomAbove && roomBelow && roomRight && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.quadrupleDoorRoom, roomPosition, transform.rotation));
                }

                break;
        }
    }
}
[System.Serializable]
public class RoomPrefabs
{
    public GameObject singleUp, singleDown, singleRight, singleLeft,
        doubleUpDown, doubleLeftRight, doubleUpRight, doubleDownRight, doubleDownLeft, doubleUpLeft,
        tripleUpRightDown, tripleRightDownLeft, tripleDownLeftUp, tripleLeftUpRight,
        quadrupleDoorRoom;
}

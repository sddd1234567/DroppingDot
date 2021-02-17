using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour {
  //  float rightBorder = 2.81f;
    float leftBorder = -2.81f;
    public GameObject floorWater;
    public GameObject[] floorGrounds;
    public GameObject floorWall;
    public List<List<Floor>> floors;

    int nowCreateFloor = 0;

    public float floorStartY = -5.35f;
    public float floorYDistance = -10f;

    int floorGrounMax = 5;
    int floorGroundMin = 2;

    public List<int> floorColor;
    public int nowFloorColor = 0;
    

	// Use this for initialization
	void Start () {
        floors = new List<List<Floor>>();
        createFloor();
        createFloor();
        createFloor();
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void createFloor() {
        if (nowCreateFloor > 30 && floorGrounMax == 5)
        {
            floorGrounMax = 4;
        }
        else if (nowCreateFloor > 60 && floorGroundMin == 2)
        {
            floorGroundMin = 1;
        }

        if ((nowCreateFloor - 1) > 0 && (nowCreateFloor - 1) % 5 == 0) {
            nowFloorColor++;
            if (nowFloorColor > 6)
                nowFloorColor = 0;
        }
            
        randomFloorGround(Random.Range(floorGroundMin,floorGrounMax), floorColor[nowFloorColor]);
    }


    public void randomFloorGround(int count, int nowColor) {
        List<int> a;

        a = new List<int>();
        for (int i = 0; i < count; i++)
        {
            int s = Random.Range(0, 5);

            bool flag = false;
            for (int n = 0; n < a.Count; n++)
            {
                if (s == a[n])
                {
                    flag = true;
                    break;
                }
            }

            if (!flag)
            {
                a.Add(s);
            }
            else
                i--;
        }


        StartCoroutine(createFloorGround(a, nowColor, nowCreateFloor));
    }

    public IEnumerator createFloorGround(List<int> a, int nowColor, int nowFloor) {
        nowCreateFloor++;
        floors.Add(new List<Floor>());
        int nowFloorListNum = floors.Count;
        yield return null;        
        for (int i = 0; i < 5; i++)
        {
            bool flag = false;
            for (int n = 0; n < a.Count; n++)
            {
                if (i == a[n])
                {
                    flag = true;
                    break;
                }
            }
            GameObject obj;
            if (!flag)
            {
                obj = Instantiate(floorGrounds[nowColor], new Vector3(leftBorder + (i * 1.124f), floorGrounds[nowColor].transform.position.y - (nowFloor * 10f), floorGrounds[nowColor].transform.position.z), floorGrounds[nowColor].transform.rotation);
                Floor fl = obj.GetComponent<Floor>();
                floors[nowFloorListNum - 1].Add(fl);
                fl.floorNum = nowFloor;
                fl.type = 1;
            }
            else
            {
                obj = Instantiate(floorWater, new Vector3(leftBorder + (i * 1.124f), floorWater.transform.position.y - (nowFloor * 10f), floorWater.transform.position.z), floorWater.transform.rotation);
                Floor fl = obj.GetComponent<Floor>();
                floors[nowFloorListNum - 1].Add(fl);
                fl.floorNum = nowFloor;
                fl.type = 0;
            }
            yield return null;
        }        
    }

    public void addFloorWall(int floor, int type) {
        for (int i = 0; i < floors[floor].Count; i++) {
            if (floors[floor][i].type == type)
                floors[floor][i].wall.SetActive(true);
        }
    }

    public void deleteFloor(int floorNum) {
        if (floorNum < 0)
            return;


        int k = floors[floorNum].Count;
        for (int i = 0; i < k; i++) {
            if(floors[floorNum][i] != null)
                Destroy(floors[floorNum][i].gameObject);
        }
    }

    public void deleteFloorWall(int floorNum)
    {
        //Debug.Log(floorNum);
        //if (floorNum < 0)
        //    return;


        int k = floors[floorNum].Count;
        Debug.Log(k);
        for (int i = 0; i < k; i++)
        {
            if (floors[floorNum][i] != null)
            {
                floors[floorNum][i].wall.SetActive(false);
                //Debug.Log("destroy wall");
            }
        }
    }
}

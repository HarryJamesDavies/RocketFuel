using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance = null;

    public int m_chunks = 10;
    public float m_cellWidth = 20;
    public float m_cellHeight = 100;
    public Vector3 m_origin;

    private int m_currentSection = 0;
    private Level m_level;
    public float m_wait = 3.0f;

    private List<Vector3> m_sectionSpawns = new List<Vector3>();

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        GlobalEventBoard.Instance.SubscribeToEvent(Events.Event.LEV_TransitionSection, Ev_TransitionSection);

        InitialiseLevel(); 
    }

    void OnDestroy()
    {
        GlobalEventBoard.Instance.UnsubscribeToEvent(Events.Event.LEV_TransitionSection, Ev_TransitionSection);
    }

    public bool InitialiseLevel()
    {
        m_level = GetComponent<Level>();
        m_level.InitaliseLevel(m_chunks, m_cellWidth, m_cellHeight, m_origin);
        return true;
    }

    public void Ev_TransitionSection(object _data = null)
    {
        StartCoroutine(Delay());
        m_currentSection++;
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(m_wait);
        m_level.TransitionSection();
    }

    public int GetCurrentSectionIndex()
    {
        return m_currentSection;
    }

    public int GetGenerateSectionCount()
    {
        return m_level.m_sectionsGenerated;
    }

    public void AddSectionSpawn(Vector3 _spawnPosition)
    {
        m_sectionSpawns.Add(_spawnPosition);
    }

    public Vector3 GetSectionSpawn(int _sectionIndex)
    {
        return m_sectionSpawns[_sectionIndex];
    }
}



//    [Serializable]
//    public class Count
//    {
//        public int minimum;
//        public int maximum;

//        public Count(int min, int max)
//        {
//            minimum = min;
//            maximum = max;
//        }
//    }
//    [SerializeField]
//    Gradient backgroundColour;

//    public int columns = 30;
//    public int rows = 200;
//    public bool hardMode = false;

//    public Count barrierCount = new Count(20, 25);
//    public Count barrierSizeX = new Count(5, 10);
//    public Count barrierSizeY = new Count(2, 4);
//    public Count lightCount = new Count(10, 15);
//    public Count LightSizeX = new Count(2, 4);
//    public Count LightSizeY = new Count(5, 10);
//    public Count mineCount = new Count(20, 25);
//    public float minimumBlockHeight = 10;

//    public GameObject OuterWall;
//    public GameObject barrier;
//    public GameObject lightRay;
//    public GameObject mine;
//    public GameObject player;
//	public GameObject deathCollider;
//	public GameObject victoryCollider;
//    public GameObject EnemyManager;
//    public GameObject SideEnemeies;
//    public GameObject MainCamera;
//    public GameObject derpy;

//    Camera myCam;

//    ParticleSystem[] fireParticles;
//	public AudioSource Fire;

//	void update()
//	{
//		GetComponent<AudioSource>().Play();
//	}

//    private Transform levelHolder;
//    private Transform barrierHolder;
//    private Transform lightHolder;
//    private Transform mineHolder;
//    private List<Vector3> gridPositions = new List<Vector3>();

//    void InitialiseList()
//    {
//        gridPositions.Clear();

//        for (int x = 1; x < columns - 1; x++)
//        {
//            for (int y = 1; y < rows - 1; y++)
//            {
//                gridPositions.Add(new Vector3(x, y, 0.0f));
//            }
//        }
//    }

//    void LevelSetup()
//    {
//        levelHolder = new GameObject("Level").transform;
//        barrierHolder = new GameObject("Barrier").transform;
//        barrierHolder.transform.SetParent(levelHolder);
//        lightHolder = new GameObject("Light").transform;
//        lightHolder.transform.SetParent(levelHolder);
//        mineHolder = new GameObject("Mine").transform;
//        mineHolder.transform.SetParent(levelHolder);
//        Transform wallHolder = new GameObject("wallHolder").transform;
//        wallHolder.transform.SetParent(levelHolder);

//        for (int x = -1; x < columns + 1; x++)
//        {
//            for (int y = -1; y < rows + 1; y++)
//            {
//                if (x == -1 || x == columns)
//                {
//                    GameObject toInstantiate = OuterWall;
//                    GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0.0f), Quaternion.identity) as GameObject;

//                    instance.transform.SetParent(wallHolder);
//                }
//            }
//        }
//    }

//    Vector3 RandomPosition()
//    {
//        int randomIndex = Random.Range(0, gridPositions.Count);

//        Vector3 randomPosition = gridPositions[randomIndex];
//        gridPositions.RemoveAt(randomIndex);
//        return randomPosition;
//    }

//    void LayoutObjectAtRandom(GameObject tile, int minimum, int maximum, int _heightMin, int _heightMax, int _widthMin, int _widthMax)
//    {
//        int objectCount = Random.Range(minimum, maximum + 1);

//        for (int i = 0; i < objectCount; i++)
//        {
//            int objectHeight = Random.Range(_heightMin, _heightMax);
//            int objectWidth = Random.Range(_widthMin, _widthMax);

//            Vector3 randomPosition = Vector3.zero;
//            do
//            {
//                randomPosition = RandomPosition();
//            } while (randomPosition.y < minimumBlockHeight);

//            CreateClump(tile, randomPosition, objectHeight, objectWidth);
//        }
//    }

//    void CreateClump(GameObject tile, Vector3 _currentPosition, int _width, int _height)
//    {
//        Transform holder = new GameObject(tile.name + "holder").transform;
//        holder.transform.SetParent(levelHolder);
//        holder.transform.position = _currentPosition;

//        if (tile == lightRay)
//        {
//            holder.gameObject.AddComponent<BoxCollider2D>();
//            holder.GetComponent<BoxCollider2D>().isTrigger = true;
//            holder.GetComponent<BoxCollider2D>().size = new Vector2(_width, _height);
//            holder.GetComponent<BoxCollider2D>().offset = new Vector2(1.5f, (_height / 2) - 0.5f);
//            holder.transform.SetParent(lightHolder);
//            holder.gameObject.tag = "RayOfLight";
//        }
//        else if(tile == barrier)
//        {
//            holder.transform.SetParent(barrierHolder);
//        }
//        else if (tile == mine)
//        {
//            holder.transform.SetParent(mineHolder);
//            holder.gameObject.AddComponent<DestroyChildren>();
//        }
//        holder.tag = "RayOfLight";


//        for (int y = 0; y < _height; y++)
//        {
//            //Debug.Log("Loop!");
//            for (int x = 0; x < _width; x++)
//            {
//                Vector3 newPosition = new Vector3(_currentPosition.x + x, _currentPosition.y + y, 0.0f);
//                if(newPosition.x >= 0 && newPosition.x <= columns)
//                {
//                    GameObject instance = Instantiate(tile, newPosition, Quaternion.identity) as GameObject;

//                    instance.transform.SetParent(holder);
//                }
//            }
//        }
//    }

//    void Update()
//    {
//        UpdateCamBackground();
//    }

//    void UpdateCamBackground()
//    {
//        float avgPercentage = myCam.GetComponent<CameraScript>().avgPercentage;
//        myCam.backgroundColor = backgroundColour.Evaluate(avgPercentage);

//        if (avgPercentage > 0.8f)
//        {
//            for (int i = 0; i < fireParticles.Length; ++i)
//            {
//				Destroy (Fire);
//                fireParticles[i].Stop();
//            }
//        }

//        if (avgPercentage > 0.8f)
//        {
//            myCam.GetComponentInChildren<UnityStandardAssets.ImageEffects.SunShafts>().enabled = true;
//        }

//    }


//    public void SetupScene()
//    {
//        LevelSetup();
//        InitialiseList();
//        GameObject player1 = (GameObject)Instantiate(player, new Vector3((columns / 2) - 6.0f, 30.0f, 0.0f), Quaternion.identity);
//        player1.gameObject.name = "Player 1";
//        GameObject player2 = (GameObject)Instantiate(player, new Vector3((columns / 2) + 6.0f, 30.0f, 0.0f), Quaternion.identity);
//        player2.GetComponent<PlayerMovement>().playerOne = false;
//        player2.gameObject.name = "Player 2";
//        player2.gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(0.0f, 255.0f, 33.0f);
//        GameObject cam = Instantiate(MainCamera, new Vector3((columns / 2) - 0.5f, 30.0f, -30.0f), Quaternion.identity) as GameObject;
//        myCam = cam.GetComponentInChildren<Camera>();

//        fireParticles = new ParticleSystem[50];
//        fireParticles = myCam.GetComponentsInChildren<ParticleSystem>();

//        GameObject tempDeathCollider = (GameObject)Instantiate(deathCollider, new Vector3((columns / 2) - 1.0f, 0.0f, 0.0f), Quaternion.identity);
//        tempDeathCollider.gameObject.transform.parent = Camera.main.gameObject.transform; 
//        tempDeathCollider.GetComponent<GameOver>().setup(gameObject);
//        GameObject tempWinCollider = (GameObject)Instantiate(victoryCollider, new Vector3 ((columns / 2) - 1.0f, rows, 0.0f), Quaternion.identity);
//        tempWinCollider.GetComponent<GameOver>().setup(gameObject);

//        LayoutObjectAtRandom(barrier, barrierCount.minimum, barrierCount.maximum, barrierSizeX.minimum, barrierSizeX.maximum, barrierSizeY.minimum, barrierSizeY.maximum);
//        LayoutObjectAtRandom(lightRay, lightCount.minimum, lightCount.maximum, LightSizeX.minimum, LightSizeX.maximum, LightSizeY.minimum, LightSizeY.maximum);
//        if (PlayerPrefs.GetInt("HardMode") == 0)
//        {
//            LayoutObjectAtRandom(mine, mineCount.minimum, mineCount.maximum, 2, 2, 2, 2);
//        }
//        else
//        {
//            LayoutObjectAtRandom(derpy, 10, 15, 1, 1, 1, 1);
//        }

//        EnemyManager.GetComponent<EnemyManager>().setDerpyShooterPosition(new Vector3((columns / 2) - 10.0f, 50.0f, 0.0f));
//        EnemyManager.GetComponent<EnemyManager>().setTrackShooterPosition(new Vector3((columns / 2) + 10.0f, 50.0f, 0.0f));
//        EnemyManager.GetComponent<EnemyManager>().initialiseEnemies();
//    }


//    //bool CheckSurroundings(int _index, int _height, int _width)
//    //{
//    //    //Debug.Log("Hi!");
//    //    for (int y = -_height / 2; y < _height/2; y++)
//    //    {
//    //        //Debug.Log("Loop!");
//    //        for (int x = -_width / 2; x < _width/2; x++)
//    //        {
//    //            int newIndex = _index + ((rows * y) + ((y * -2) - 1) - ((y + 1) * rows) + (-1 + ((y + 1) * 3)) + (rows * (x + 1)) + ((x * -2) - 1));

//    //            if (newIndex <= gridPositions.Count && newIndex >= 0)
//    //            {
//    //                //string log = "New Index: " + newIndex + "   Index: " + _index;
//    //                //Debug.Log(log);

//    //                Vector3 currentPosition = gridPositions[_index];
//    //                Vector3 newPosition = new Vector3(currentPosition.x + x, currentPosition.y + y, 0.0f);

//    //                string log = "New Index: " + newIndex + "   Index: " + _index + "\nNew Vector: " + newPosition.x + ", " + newPosition.y
//    //                    + " Current Vector: " + currentPosition.x + ", " + currentPosition.y + " Check Vector: " + gridPositions[newIndex].x + ", " + gridPositions[newIndex].y;
//    //                Debug.Log(log);

//    //                if (gridPositions[newIndex] != newPosition)
//    //                {
//    //                    //Debug.Log("Too Close!");
//    //                    return false;
//    //                }
//    //            }
//    //            else
//    //            {
//    //                return false;
//    //            }
//    //        }
//    //    }

//    //    return true;
//    //}
//}

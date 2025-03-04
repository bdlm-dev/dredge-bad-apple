using System.Collections;
using System.Reflection.Emit;
using UnityEngine;
using Winch.Core;

namespace DredgeBadApple;

internal class PotManager : MonoBehaviour
{
    DataLoader? dataLoader;

    private Vector3 root_position = new Vector3(356f, 0f, 130f);

    private int pot_count;

    private static GameObject? container;
    private List<GameObject> pots = new();
    private GameObject? sample_pot;

    private bool executing = false; 

    public static PotManager Instance(DataLoader _dataLoader)
    {
        container = new GameObject("BadApple_PotManager");
        return container.AddComponent<PotManager>().Init(_dataLoader);
    }

    public PotManager Init(DataLoader _dataLoader)
    {
        dataLoader = _dataLoader;

        sample_pot = CreateFirstCrabPot();

        WinchCore.Log.Debug("Created sample_pot gameObject " + sample_pot.name);

        return this;
    }

    public void GeneratePots()
    {
        Vector2 resolution = dataLoader!.resolution;

        float offset = 1f;

        pot_count = (int)(resolution.x * resolution.y);

        WinchCore.Log.Debug($"Resolution is ({resolution.x},{resolution.y}), generating {pot_count} pots");

        for (int x = 0; x < resolution.x; x++) {
            for (int z = 0; z < resolution.y; z++) {
                GameObject newObject = Instantiate(sample_pot!);
                SimpleBuoyantObject buoyancy = newObject.GetComponent<SimpleBuoyantObject>();
                buoyancy.transformHelper.x += x * offset;
                buoyancy.transformHelper.z += z * -offset;
                newObject.transform.parent = container!.transform;
                pots.Add(newObject);
            }
        }
    }

    public void CleanupPots()
    {
        container!.transform.DestroyAllChildren();
        WinchCore.Log.Debug("Cleaned up all BadAppleGeneratedHarvestPOI");
    }

    public IEnumerator Execute(IEnumerable<int[][]> generator)
    {
        Vector2 resolution = dataLoader.resolution;
        // iterate over enumerator, moving/disabling crab pots by frame data
        WinchCore.Log.Debug("Executing animation.");

        foreach (int[][] frame in generator) {
            // iterate over every coordinate in frame
            // if 1, disable LIGHT

            var y = 0;
            foreach (int[] slice in frame)
            {
                var x = 0;
                foreach (int val in slice)
                {
                    // index of this pot
                    // is at (y * resolution.y) + x
                    int index = (int) (y * resolution.y) + x;

                    // pots[index].SetActive(val == 1);

                    Transform mesh = pots[index].transform.GetChild(0).GetChild(0);

                    if (val == 1)
                    {
                        mesh.GetChild(1).gameObject.SetActive(false);
                        mesh.GetChild(3).gameObject.SetActive(true);
                    } else
                    {
                        mesh.GetChild(1).gameObject.SetActive(true);
                        mesh.GetChild(3).gameObject.SetActive(false);
                    }

                    x++;
                }
                y++;
            }

            yield return new WaitForSeconds(0.1f);
        }

        executing = false;
    }

    private GameObject CreateFirstCrabPot()
    {
        // create sample crab pot

        ItemManager _itemManager = GameManager.Instance.ItemManager;
        SpatialItemInstance pot_instance = _itemManager.CreateItem<SpatialItemInstance>(_itemManager.allItems.Where(i => i.itemType == ItemType.EQUIPMENT && i.itemSubtype == ItemSubtype.POT).First());
        SerializedCrabPotPOIData poi_data = new SerializedCrabPotPOIData(pot_instance, root_position.x, root_position.z);

        // GameSceneInitializer.Instance.CreatePlacedHarvestPOI not work :(( 

        GameObject createdObject = Instantiate<GameObject>(
            GameSceneInitializer.Instance.placedPOIPrefab,
            new Vector3(root_position.x, 0f, root_position.z),
            Quaternion.identity,
            null
            );

        Thread.Sleep(1000);

        createdObject.name = "BadAppleGeneratedHarvestPOI";

        // Destroy(createdObject.transform.GetChild(0).gameObject);
        Transform buoy = createdObject.transform.GetChild(0);

        buoy.GetChild(0).GetChild(1).gameObject.SetActive(false);
        buoy.GetChild(0).GetChild(2).gameObject.SetActive(false);

        createdObject.RemoveComponent<Cullable>();

        container.transform.parent = createdObject.transform;
        container.transform.parent = null;

        return createdObject;
    }

    public void Awake()
    {
        WinchCore.Log.Debug("Pot Manager is awake.");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            WinchCore.Log.Debug("P Pressed");
            if (!executing)
            {
                executing = true;
                StartCoroutine(Execute(dataLoader!.GenerateFrames()));
            }
            // play animation
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            WinchCore.Log.Debug("V Pressed");
            GeneratePots();
            // generate pots
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            WinchCore.Log.Debug("U Pressed");
            CleanupPots();
            // cleanup pots
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
	public int mapRow, mapCol;
	public int roomRow, roomCol;
	public int currentStage = 0;
	private MapLoader mapLoader;
	[SerializeField] public Transform mapParent;
	[SerializeField] public Transform enemyParent;

	[SerializeField] private UnityEngine.UI.Button debugBTN; // go nextStage

	// Start is called before the first frame update
	void Start()
	{
		mapLoader = this.GetComponent<MapLoader>();

		GoNextStage();

		debugBTN.onClick.AddListener(GoNextStage);
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void GoNextStage()
	{
		foreach (Transform transform in mapParent) Destroy(transform.gameObject);
		foreach (Transform transform in enemyParent) Destroy(transform.gameObject);
		currentStage++;
		mapLoader.GenerateStage(mapRow, mapCol);
	}
}

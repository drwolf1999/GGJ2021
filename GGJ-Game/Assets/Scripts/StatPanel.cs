using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatPanel : MonoBehaviour
{
	private float canvasHeight, canvasWidth;

	[SerializeField] private GameObject togglePrefab;
	[SerializeField] private GameObject buttonPrefab;
	[SerializeField] private Transform objectParent;
	[SerializeField] private ToggleGroup getToggleParent;
	[SerializeField] private ToggleGroup lossToggleParent;

	private GameObject saveBtn;
	private GameObject textInsteadSaveBtn;

	[HideInInspector]
	public string lossStat;
	[HideInInspector]
	public string getStat;

	private List<string> stat = new List<string>()
	{
		"Health",
		"Attack",
		"Defense",
		"MovementSpeed",
		"AttackSpeed",
		"CriticalRate",
		"CriticalDamage",
		"Penetration"
	};
	// Start is called before the first frame update
	void Start()
	{
		LoadCanvasSize();
		LoadStat();
	}

	// Update is called once per frame
	void Update()
	{
		ButtonActive();
	}

	private void LoadStat()
	{
		Vector2 secondElementPos = new Vector2(canvasWidth * 3f / 12f + 100f, 0),
				thirdElementPos = new Vector2(canvasWidth * 5f / 12f, 0);
		Font font = Resources.GetBuiltinResource<Font>("Arial.ttf");
		Vector2 anchor = new Vector2(0.5f, 0.5f);
		/// show label
		GameObject label = new GameObject();
		label.transform.SetParent(objectParent, false);
		label.layer = 5;
		label.name = "label";

		RectTransform labelRT = label.AddComponent<RectTransform>();
		labelRT.anchorMin = labelRT.anchorMax = labelRT.pivot = anchor;
		labelRT.localPosition = Vector3.zero;

		GameObject getLabel = new GameObject();
		getLabel.name = "get label";
		getLabel.transform.parent = label.transform;
		Text getText = getLabel.AddComponent<Text>();
		getText.text = "get stat";
		getText.font = font;
		getText.fontSize = 40;
		RectTransform getRT = getLabel.GetComponent<RectTransform>();
		getRT.localPosition = secondElementPos;

		GameObject lossLabel = new GameObject();
		lossLabel.name = "loss label";
		lossLabel.transform.parent = label.transform;
		Text lossText = lossLabel.AddComponent<Text>();
		lossText.text = "loss stat";
		lossText.font = font;
		lossText.fontSize = 40;
		RectTransform lossRT = lossLabel.GetComponent<RectTransform>();
		lossRT.localPosition = thirdElementPos;
		/// content
		for (int i = 0; i < stat.Count; i++)
		{

			GameObject obj = new GameObject();
			obj.transform.SetParent(objectParent, false);
/*			obj.transform.parent = objectParent;*/
			obj.layer = 5; // UI layer
			obj.name = stat[i];

			RectTransform objRT = obj.AddComponent<RectTransform>();
			obj.transform.localPosition = Vector2.zero;
			objRT.anchorMin = anchor;
			objRT.anchorMax = anchor;
			objRT.pivot = anchor;

			// Explain Stat
			GameObject statName = new GameObject();
			statName.name = "statName";
			Text statText = statName.AddComponent<Text>();
			statText.color = Color.white;
			statName.transform.parent = obj.transform;
			statName.transform.localPosition = Vector2.zero;
			statText.text = stat[i];
			statText.fontSize = 50;
			statText.font = font;
			statText.alignment = TextAnchor.MiddleLeft;
			RectTransform statRT = statName.GetComponent<RectTransform>();
			statRT.sizeDelta = new Vector2(canvasWidth * 4f / 6f, 100f);
			statRT.localPosition = new Vector2(-canvasWidth * 1f / 12f, 0f);

			// get stat btn
			GameObject getBtn = Instantiate(togglePrefab);
			getBtn.name = "get_" + stat[i];
			getBtn.transform.parent = obj.transform;

			RectTransform getBtnRT = getBtn.GetComponent<RectTransform>();
			getBtnRT.anchorMin = anchor;
			getBtnRT.anchorMax = anchor;
			getBtnRT.pivot = anchor;
			getBtnRT.localPosition = Vector3.zero;
			getBtn.transform.localPosition = secondElementPos;

			getBtn.GetComponent<Toggle>().group = getToggleParent;

			// loss stat btn
			GameObject lossBtn = Instantiate(togglePrefab);
			lossBtn.name = "loss_" + stat[i];
			lossBtn.transform.parent = obj.transform;

			RectTransform lossBtnRT = lossBtn.GetComponent<RectTransform>();
			lossBtnRT.localPosition = Vector3.zero;
			lossBtnRT.anchorMin = anchor;
			lossBtnRT.anchorMax = anchor;
			lossBtnRT.pivot = anchor;
			lossBtnRT.localPosition = thirdElementPos;

			lossBtn.GetComponent<Toggle>().group = lossToggleParent;
		}
		/// save btn
		GameObject save = Instantiate(buttonPrefab);
		save.transform.SetParent(objectParent, false);
		save.layer = 5; // UI layer
		save.name = "save";

		RectTransform saveRT = save.GetComponent<RectTransform>();
		saveRT.pivot = anchor;
		/*saveRT.anchorMin = saveRT.anchorMax = anchor;*/
		saveRT.localPosition = Vector3.zero;
		saveRT.sizeDelta = new Vector2(500, 100);

		save.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Save";
		save.SetActive(false);
		saveBtn = save;

		/// text instead save btn
		GameObject textOBJ = new GameObject();
		textOBJ.transform.SetParent(objectParent, false);
		textOBJ.layer = 5;
		textOBJ.name = "instead text";
		Text text = textOBJ.AddComponent<Text>();
		text.text = "You must choose different stats";
		text.font = font;
		text.fontSize = 50;
		RectTransform textRT = textOBJ.GetComponent<RectTransform>();
		textRT.localPosition = Vector3.zero;
		textRT.sizeDelta = new Vector2(800, 100);
		textOBJ.SetActive(false);

		textInsteadSaveBtn = textOBJ;
	}

	private void LoadCanvasSize()
	{
		RectTransform canvas = transform.parent.parent.GetComponent<Canvas>().GetComponent<RectTransform>();
		canvasHeight = canvas.rect.height;
		canvasWidth = canvas.rect.width;
	}

	private void ButtonActive()
	{
		foreach (Toggle toggle in getToggleParent.ActiveToggles())
		{
			getStat = toggle.name;
		}
		foreach (Toggle toggle in lossToggleParent.ActiveToggles())
		{
			lossStat = toggle.name;
		}
		string[] getS = getStat.Split('_');
		string[] lossS = lossStat.Split('_');

		if (getS.Length != 2 || lossS.Length != 2)
		{
			saveBtn.SetActive(false);
			textInsteadSaveBtn.SetActive(true);
		}
		getStat = getStat.Split('_')[1];
		lossStat = lossStat.Split('_')[1];
		if (getStat.Equals(lossStat))
		{
			saveBtn.SetActive(false);
			textInsteadSaveBtn.SetActive(true);
		}
		else
		{
			saveBtn.SetActive(true);
			textInsteadSaveBtn.SetActive(false);
		}
	}
}

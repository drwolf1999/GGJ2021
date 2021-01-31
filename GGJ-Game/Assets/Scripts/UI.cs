using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
	[SerializeField] private GameObject subMenu;
	[SerializeField] private GameObject statPanel;


	public bool uiActive { get => subMenu.activeSelf || statPanel.activeSelf; }

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			SubMenuToggle();
		}
	}

	public void SubMenuToggle()
	{
		subMenu.SetActive(!subMenu.activeSelf);
	}

	public void StatPanelToggle()
	{
		statPanel.SetActive(!statPanel.activeSelf);
	}
}

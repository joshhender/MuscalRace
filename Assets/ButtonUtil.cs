using UnityEngine;
using System.Collections;

public class ButtonUtil : MonoBehaviour {

	public void StartGame()
	{
		GameManager.instance.StartGame ();
	}

	public void GoUpOne()
	{
		GameManager.instance.ChangeCurrentNote (true);
	}

	public void GoDownOne()
	{
		GameManager.instance.ChangeCurrentNote (false);
	}

	public void CheckAnswer()
	{
		GameManager.instance.CheckAnswer ();
	}
}

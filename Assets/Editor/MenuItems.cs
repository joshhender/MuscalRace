using UnityEngine;
using UnityEditor;

public class MenuItems {

	[MenuItem("Tools/Clear PlayerPrefs %#C")]
	private static void NewMenuOption()
	{
		PlayerPrefs.DeleteAll();
		Debug.Log ("PlayerPrefs cleared!");
	}

	[MenuItem("GameObject/2D Object/Player", false, 2)]
	private static void Create2DPlayer()
	{
		GameObject player = GameObject.CreatePrimitive (PrimitiveType.Quad);
		GameObject.DestroyImmediate (player.GetComponent<MeshCollider>());
		GameObject.DestroyImmediate (player.GetComponent<MeshFilter>());
		GameObject.DestroyImmediate (player.GetComponent<MeshRenderer>());
		player.AddComponent<SpriteRenderer>();
		player.AddComponent <Rigidbody2D>();
		player.AddComponent<BoxCollider2D> ().size = new Vector2 (1, 1);
		player.name = "Player";
	}
}

using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {

	public GameObject[] shapeTypes;

    public void Spawn()
	{
		// Random Shape
   		 int i = GameController.Instance.random.Next(0, shapeTypes.Length);

		// Spawn Group at current Position
		GameObject temp = Instantiate(shapeTypes[i]) ;
        Managers.Game.currentShape = temp.GetComponent<TetrisShape>();
        temp.transform.parent = Managers.Game.blockHolder;
    }
}

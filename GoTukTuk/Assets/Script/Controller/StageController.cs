using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour {
	public GameController.EStage stage;


	// Use this for initialization
	void Start () {
		loadGame ();
	}

	void loadGame(){
		string saveName = "stage" + stage;

		if (ES2.Exists ("game.dat?tag=" + saveName + "&encrypt=true&password=gotuktukbahagia")) {
			Dictionary<string, Level> levels = ES2.LoadDictionary<string, Level> ("game.dat?tag=" + saveName + "&encrypt=true&password=gotuktukbahagia");
			List<Transform> TLevels = getAllLevel();
			int i = 0;
			while (i < levels.Count) {
				Level level = levels ["" + ((GameController.ELevel)i + 1)];
				if (level.isLevelCompleted) {
					List<Transform> star = new List<Transform> ();
					foreach(Transform child in TLevels[i].FindChild("starRes")){
						star.Add(child);
					}
					for(int j = 0; j < level.star; j++){
						star[j].gameObject.SetActive(true);
					}
				}
				i++;
			}
		}
	}

	List<Transform> getAllLevel (){
		GameObject container = GameObject.Find ("LevelsContainer");
		List<Transform> result = new List<Transform> ();
		foreach (Transform child in container.transform) {
			result.Add(child);
		}
		return result;
	}

	// Update is called once per frame
	void Update () {
		
	}
}

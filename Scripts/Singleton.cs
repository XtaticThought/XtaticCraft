using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

	private static T instance;

	public static T Instance {
		get {
			//Check if instance already exists
			if (instance == null){
				//if not, set instance to of type T
				instance = FindObjectOfType<T>();
			//If instance already exists and it's not this:
			} else if (instance != FindObjectOfType<T>()){
				//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
				Destroy(FindObjectOfType<T>());
			}
			//Sets this to not be destroyed when reloading scene
			// DontDestroyOnLoad(FindObjectOfType<T>());
			return instance;
		} 
	}
}

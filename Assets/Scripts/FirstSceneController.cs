using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Sun-Yet-Sun Universsity,School of Software,Jiajun Zheng*/
/*In MVC model , controller and model has been defined here.Controller can directly modify the model(GameObject).
For example, OnBoard and OffBoard can change the (Devil)Priest's position.And MoveBoat() can change the Boat's
position.Besides,getCurrentState() can tell UI the Game's information.And getTimeLeft() can also give the UI
string to render.*/

public class FirstSceneController : MonoBehaviour,ISceneController,IUserAction {
	public CCActionManager actionManager;
	int n = 0;
	/*GameObejct Of the game.*/
	public GameObject FirstPriest;
	public GameObject SecondPriest;
	public GameObject ThirdPriest;
	public GameObject FirstDevil;
	public GameObject SecondDevil;
	public GameObject ThirdDevil;
	public GameObject Boat;
	public GameObject Riverside;
	public GameObject River;

	/*We need six contents to keep the number of the Priests and Devils */
	List<GameObject> StartPriests = new List<GameObject>();
	List<GameObject> StartDevils = new List<GameObject>();

	List<GameObject> EndPriests = new List<GameObject>();
	List<GameObject> EndDevils = new List<GameObject>();

	List<GameObject> BoatPriests = new List<GameObject>();
	List<GameObject> BoatDevils = new List<GameObject>();

	/*Visually,The Lists below keep the state of the positions.
	By the mean of it , We can distinguish whether the position can be used.*/
	List<Vector3> OccupiedStartPosition = new List<Vector3> ();
	List<Vector3> OccupiedEndPosition = new List<Vector3> ();
	List<Vector3> OccupiedBoatPosition = new List<Vector3> ();
	List<Vector3> UnOccupiedStartPosition = new List<Vector3> ();
	List<Vector3> UnOccupiedEndPosition = new List<Vector3> ();
	List<Vector3> UnOccupiedBoatPosition = new List<Vector3> ();

	/*All the position*/
	Vector3 StartPositionOne = new Vector3 (5.55f,-0.48f,0);
	Vector3 StartPositionTwo = new Vector3 (6.65f,-0.48f,0);
	Vector3 StartPositionThree = new Vector3 (7.75f,-0.48f,0);
	Vector3 StartPositionFour = new Vector3 (8.85f,-0.48f,0);
	Vector3 StartPositionFive = new Vector3 (9.95f,-0.48f,0);
	Vector3 StartPositionSix = new Vector3 (11.05f,-0.48f,0);
	Vector3 PersonOneOnBoat = new Vector3 (1.44f,-2.385f,0);
	Vector3 PersonTwoOnBoat = new Vector3 (3.01f,-2.385f,0);
	Vector3 BoatPositionOne = new Vector3 (2.35f,-0.04f,0);
	Vector3 BoatPositionTwo = new Vector3 (-2.37f,-0.04f,0);
	Vector3 EndPositionOne = new Vector3 (-2.88f,-0.48f,0);
	Vector3 EndPositionTwo = new Vector3 (-3.98f,-0.48f,0);
	Vector3 EndPositionThree = new Vector3 (-5.08f,-0.48f,0);
	Vector3 EndPositionFour = new Vector3 (-6.18f,-0.48f,0);
	Vector3 EndPositionFive = new Vector3 (-7.28f,-0.48f,0);
	Vector3 EndPositionSix = new Vector3(-8.38f,-0.48f,0);
	Vector3 RiverPosition = new Vector3(-5.83f,0.79f,0);
	Vector3 RiverSidePosition = new Vector3 (0.69f,0.0965f,0);
	Vector3 PositionBoatWillGoTo;

	/*When the Boat is running ,it will be set to true.*/
	bool IsBoatRunning = false;

	/*The enum can represent the state of the game.*/
	enum result {Failed,Playing,Succeed};

	/*By default, the game is in running.*/
	result GameCurrentState = result.Playing;

	/*Time count down from 60, that means player need to finish the game in a minute.*/
	float TimeLeft = 60.0f;

	/*initial period*/
	void Awake() {
		SSDirector director = SSDirector.getInstance ();
		director.setFPS (60);
		director.currentScenceController = this;
		director.currentScenceController.GenGameObject ();
		actionManager = GetComponent<CCActionManager> ();
	}

	/*initial function, we will generate the object here ,and reset the Lists.*/
	public void GenGameObject() {
		FirstPriest = Instantiate<GameObject> (
			Resources.Load<GameObject> ("Prefabs/Priest"), 
			StartPositionOne, Quaternion.identity);
		FirstPriest.name = "FirstPriest";

		SecondPriest = Instantiate<GameObject> (
			Resources.Load<GameObject> ("Prefabs/Priest"), 
			StartPositionTwo, Quaternion.identity);
		SecondPriest.name = "SecondPriest";
		
		ThirdPriest = Instantiate<GameObject> (
			Resources.Load<GameObject> ("Prefabs/Priest"), 
			StartPositionThree, Quaternion.identity);
		ThirdPriest.name = "ThirdPriest";
		
		FirstDevil = Instantiate<GameObject> (
			Resources.Load<GameObject> ("Prefabs/Devil"), 
			StartPositionFour, Quaternion.identity);
		FirstDevil.name = "FirstDevil";
		
		SecondDevil = Instantiate<GameObject> (
			Resources.Load<GameObject> ("Prefabs/Devil"), 
			StartPositionFive, Quaternion.identity);
		SecondDevil.name = "SecondDevil";
		
		ThirdDevil = Instantiate<GameObject> (
			Resources.Load<GameObject> ("Prefabs/Devil"), 
			StartPositionSix, Quaternion.identity);
		ThirdDevil.name = "ThirdDevil";

		River = Instantiate<GameObject> (
			Resources.Load<GameObject>("Prefabs/River"),
			RiverPosition,Quaternion.identity);
		River.name = "River";

		Riverside = Instantiate<GameObject> (
			Resources.Load<GameObject> ("Prefabs/Riverside"),
			RiverSidePosition, Quaternion.identity);
		Riverside.name = "Riverside";

		Boat = Instantiate<GameObject> (
			Resources.Load<GameObject>("Prefabs/Boat"),
		BoatPositionOne,Quaternion.identity);
		Boat.name = "Boat";

		initGame ();

	}

	/*initGame() mainly do reset and clean */
	public void initGame() {
		OccupiedEndPosition.Clear ();
		OccupiedStartPosition.Add (StartPositionOne);
		OccupiedStartPosition.Add (StartPositionTwo);
		OccupiedStartPosition.Add (StartPositionThree);
		OccupiedStartPosition.Add (StartPositionFour);
		OccupiedStartPosition.Add (StartPositionFive);
		OccupiedStartPosition.Add (StartPositionSix);

		OccupiedEndPosition.Clear ();

		UnOccupiedEndPosition.Clear ();
		UnOccupiedEndPosition.Add (EndPositionOne);
		UnOccupiedEndPosition.Add (EndPositionTwo);
		UnOccupiedEndPosition.Add (EndPositionThree);
		UnOccupiedEndPosition.Add (EndPositionFour);
		UnOccupiedEndPosition.Add (EndPositionFive);
		UnOccupiedEndPosition.Add (EndPositionSix);

		UnOccupiedStartPosition.Clear ();

		UnOccupiedBoatPosition.Clear ();
		UnOccupiedBoatPosition.Add (PersonOneOnBoat);
		UnOccupiedBoatPosition.Add (PersonTwoOnBoat);

		OccupiedBoatPosition.Clear ();

		StartPriests.Clear ();
		StartPriests.Add(FirstPriest);
		StartPriests.Add (SecondPriest);
		StartPriests.Add (ThirdPriest);

		StartDevils.Clear ();
		StartDevils.Add (FirstDevil);
		StartDevils.Add (SecondDevil);
		StartDevils.Add (ThirdDevil);

		EndDevils.Clear ();

		EndPriests.Clear ();

		BoatDevils.Clear ();

		BoatPriests.Clear ();
	}

	/*The logic of the Restart Button in this part we need do init .
	Besides, we have to solve the relation between parent and child.
	Also to relocate the object to the origin place and reset the clock.*/
	public void Restart() { 
		Debug.Log (IsBoatRunning);
		if (IsBoatRunning == false) {
			
			initGame ();

			FirstPriest.transform.position = StartPositionOne;
			FirstPriest.transform.SetParent (null);
			SecondPriest.transform.position = StartPositionTwo;
			SecondPriest.transform.SetParent (null);
			ThirdPriest.transform.position = StartPositionThree;
			ThirdPriest.transform.SetParent (null);

			FirstDevil.transform.position = StartPositionFour;
			FirstDevil.transform.SetParent (null);
			SecondDevil.transform.position = StartPositionFive;
			SecondDevil.transform.SetParent (null);
			ThirdDevil.transform.position = StartPositionSix;
			ThirdDevil.transform.SetParent (null);

			Boat.transform.position = BoatPositionOne;

			TimeLeft = 60.0f;

			GameCurrentState = result.Playing;
		}

	}
		
	public bool MoveBoat() {
		int totalOnBoat;
		totalOnBoat = BoatDevils.Count + BoatPriests.Count;

		/*when the boat move we should think about two aspects.
		One is the place where you start from.In this case , you should count the 
		Priests and Devils on it.
		The other one is the place where you want to reach.For this , you should 
		count the Priests and Devils over there and both on boat. 
		Below lists the two cases.The first is from east to west.(BoatPositionTwo<-BoatPositionOne)
		The second is from west to east(BoatPositionTwo->BoatPositionOne)*/
		if (Boat.transform.position.Equals (BoatPositionOne)) {
			totalOnBoat = BoatDevils.Count + BoatPriests.Count;
			IsBoatRunning = false;
			if (totalOnBoat > 0 && totalOnBoat <= 2) {
				IsBoatRunning = true;
				PositionBoatWillGoTo = BoatPositionTwo;
				CCActionMoveBoat moveBoat = CCActionMoveBoat.GetSSAction (Boat, PositionBoatWillGoTo,0.25f);
				actionManager.RunAction (moveBoat, null);
//				Boat.transform.position = Vector3.MoveTowards (Boat.transform.position,PositionBoatWillGoTo,0.25f);

				/*in MoveBoat() method may appear failed result so we need to check it */
				checkResult ();
			}
		} else if (Boat.transform.position.Equals (BoatPositionTwo)) {
			totalOnBoat = BoatDevils.Count + BoatPriests.Count;
			IsBoatRunning = false;
			if (totalOnBoat > 0 && totalOnBoat <= 2) {
				IsBoatRunning = true;
				PositionBoatWillGoTo = BoatPositionOne;
				CCActionMoveBoat moveBoat = CCActionMoveBoat.GetSSAction (Boat, PositionBoatWillGoTo,0.25f);
				actionManager.RunAction (moveBoat, null);
//				Boat.transform.position = Vector3.MoveTowards (Boat.transform.position,PositionBoatWillGoTo,0.25f);
				checkResult ();
			}
		}
		Debug.Log ("Direction:" + PositionBoatWillGoTo.x + "  StartPriest:" + StartPriests.Count + "  StartDevils:" + StartDevils.Count +
			"  BoatPriests" + BoatPriests.Count + "  BoatDevils:" + BoatDevils.Count + "  EndPriests:" + EndPriests.Count + "  EndDevils:" + EndDevils.Count);
		return false;
	}

	/*OnBoard()mainly solve four cases. These cases' codes are very similar.
	And the four cases are :Priest OnBoard from Start;Priest OnBoard from End;
	Devil OnBoard From Start ;Devil OnBoard from End*/
	public bool OnBoard() {
		RaycastHit rayHit;

		/*we use rayHit to catch the clicked object*/
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		/*choose the layer 8 because I set the Priest and Devil in Layer 8*/
		if (Physics.Raycast (ray, out rayHit,1 << 8)) {
			Debug.Log (rayHit.transform.gameObject.tag);

			/*I use tag to distinguish Priest and Devil*/
			if (rayHit.transform.gameObject.tag.Equals("Priest")) {
				/*And I use axis X to know whether is Start or End.If x is positive then Start,otherwise End*/
				if (rayHit.collider.gameObject.transform.position.x>0) {
					/*IF object want to Board from Start,the boat must park at BoatPositionOne
					and there is free position on Boat.*/
					if (Boat.transform.position.Equals (BoatPositionOne) && (BoatDevils.Count + BoatPriests.Count) < 2) {
						/*Just use the list as stack ,choose the top posiiton of list */
						int indexOfPositionOnBoat = UnOccupiedBoatPosition.Count;
						/*Because The father object is empty,the ray will hit the child so we should get the parent of the child*/
						Transform father = FindRootParent(rayHit.collider.gameObject.transform);
						Vector3 ObjectOriginalPosition = father.position;
						/*When the object OnBoard ,it will move together with the boat.So let it be the child of the boat.*/
						father.SetParent(Boat.transform);
						CCActionOnBoard onBoard = CCActionOnBoard.GetSSAction (father.gameObject,UnOccupiedBoatPosition [indexOfPositionOnBoat - 1]);
						actionManager.RunAction (onBoard, null);
						/*OccupiedBoatposition will increase and the UnOccupiedBoatposition will decrease.The StartPosition is as the same*/
						OccupiedBoatPosition.Add(UnOccupiedBoatPosition[indexOfPositionOnBoat-1]);
						UnOccupiedBoatPosition.RemoveAt(indexOfPositionOnBoat-1);
						UnOccupiedStartPosition.Add (ObjectOriginalPosition);
						OccupiedStartPosition.Remove (ObjectOriginalPosition);
						/*Priest move from Start to Boat so we need to keep such information on Lists*/
						StartPriests.Remove (father.gameObject);
						BoatPriests.Add (father.gameObject);
						return true;
					}

				} else {
					/*Priest OnBoard from End the detail is as the same as above*/
					if (Boat.transform.position.Equals (BoatPositionTwo) && (BoatDevils.Count + BoatPriests.Count) < 2) {
						int indexOfPositionOnBoat = UnOccupiedBoatPosition.Count;
						Transform father = FindRootParent(rayHit.collider.gameObject.transform);
						Vector3 ObjectOriginalPosition = father.position;
						father.SetParent(Boat.transform);
						CCActionOnBoard onBoard = CCActionOnBoard.GetSSAction (father.gameObject,UnOccupiedBoatPosition [indexOfPositionOnBoat - 1]);
						actionManager.RunAction (onBoard,  null);
//						father.localPosition= UnOccupiedBoatPosition[indexOfPositionOnBoat-1];
						OccupiedBoatPosition.Add(UnOccupiedBoatPosition[indexOfPositionOnBoat-1]);
						UnOccupiedBoatPosition.RemoveAt(indexOfPositionOnBoat-1);
						UnOccupiedEndPosition.Add (ObjectOriginalPosition);
						OccupiedEndPosition.Remove (ObjectOriginalPosition);
						EndPriests.Remove (father.gameObject);
						BoatPriests.Add (father.gameObject);
						return true;
					}
				}
			}else if (rayHit.collider.gameObject.tag.Equals("Devil")) {
				/*Devil OnBoard from Start*/
				if (rayHit.collider.gameObject.transform.position.x>0) {
					if (Boat.transform.position.Equals (BoatPositionOne) && (BoatDevils.Count + BoatPriests.Count) < 2) {
						int indexOfPositionOnBoat = UnOccupiedBoatPosition.Count;
						Transform father = FindRootParent(rayHit.collider.gameObject.transform);
						Vector3 ObjectOriginalPosition = father.position;
						father.SetParent(Boat.transform);
						CCActionOnBoard onBoard = CCActionOnBoard.GetSSAction (father.gameObject,UnOccupiedBoatPosition [indexOfPositionOnBoat - 1]);
						actionManager.RunAction (onBoard, null);
//						father.localPosition= UnOccupiedBoatPosition[indexOfPositionOnBoat-1];
						OccupiedBoatPosition.Add(UnOccupiedBoatPosition[indexOfPositionOnBoat-1]);
						UnOccupiedBoatPosition.RemoveAt(indexOfPositionOnBoat-1);
						UnOccupiedStartPosition.Add (ObjectOriginalPosition);
						OccupiedStartPosition.Remove (ObjectOriginalPosition);
						StartDevils.Remove (father.gameObject);
						BoatDevils.Add (father.gameObject);
						return true;
					}

				} else {
					/*Devil OnBoard from End*/
					if (Boat.transform.position.Equals (BoatPositionTwo) && (BoatDevils.Count + BoatPriests.Count) < 2) {
						int indexOfPositionOnBoat = UnOccupiedBoatPosition.Count;
						Transform father = FindRootParent(rayHit.collider.gameObject.transform);
						Vector3 ObjectOriginalPosition = father.position;
						father.SetParent(Boat.transform);
						CCActionOnBoard onBoard = CCActionOnBoard.GetSSAction (father.gameObject,UnOccupiedBoatPosition [indexOfPositionOnBoat - 1]);
						actionManager.RunAction (onBoard, null);
//						father.localPosition= UnOccupiedBoatPosition[indexOfPositionOnBoat-1];
						OccupiedBoatPosition.Add(UnOccupiedBoatPosition[indexOfPositionOnBoat-1]);
						UnOccupiedBoatPosition.RemoveAt(indexOfPositionOnBoat-1);
						UnOccupiedEndPosition.Add (ObjectOriginalPosition);
						OccupiedEndPosition.Remove (ObjectOriginalPosition);
						EndDevils.Remove (father.gameObject);
						BoatDevils.Add (father.gameObject);
						return true;
					}
				}
			} 
				
		}
		Debug.Log ("Direction:" + PositionBoatWillGoTo.x + "  StartPriest:" + StartPriests.Count + "  StartDevils:" + StartDevils.Count +
			"  BoatPriests" + BoatPriests.Count + "  BoatDevils:" + BoatDevils.Count + "  EndPriests:" + EndPriests.Count + "  EndDevils:" + EndDevils.Count);
		return false;
	}
	/*the content of OffBoard is also very similar to OnBoard*/
	public bool OffBoard() {
		n++;
		Debug.Log (n);
		RaycastHit rayHit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out rayHit, 1 << 8)) {
			/*Priest OffBoard to Start*/
			if (rayHit.collider.gameObject.tag.Equals("Priest")) {
				if (rayHit.collider.gameObject.transform.position.x>0) {
					/*How to specify the object is on boat ,just check if it is the child of the boat*/
					if (Boat.transform.position.Equals (BoatPositionOne) && rayHit.collider.gameObject.transform.IsChildOf(Boat.transform)) {
						int indexOfPositionOnStart = UnOccupiedStartPosition.Count;
						Transform father = FindRootParent(rayHit.collider.gameObject.transform);
						Vector3 ObjectOriginalPosition = father.localPosition;
						/*delete the child of boat,otherwise boat will Onland*/
						father.SetParent(null);
						CCActionOffBoard offBoard = CCActionOffBoard.GetSSAction (father.gameObject,UnOccupiedStartPosition[indexOfPositionOnStart-1]);
						actionManager.RunAction (offBoard, null);
//						father.position= UnOccupiedStartPosition[indexOfPositionOnStart-1];
						OccupiedStartPosition.Add(UnOccupiedStartPosition[indexOfPositionOnStart-1]);
						UnOccupiedStartPosition.RemoveAt(indexOfPositionOnStart-1);
						UnOccupiedBoatPosition.Add (ObjectOriginalPosition);
						OccupiedBoatPosition.Remove (ObjectOriginalPosition);
						StartPriests.Add (father.gameObject);
						BoatPriests.Remove (father.gameObject);
						return true;
					}

				} else {
					/*Priest OffBoard to End*/
					if (Boat.transform.position.Equals (BoatPositionTwo) && rayHit.collider.gameObject.transform.IsChildOf(Boat.transform)) {
						int indexOfPositionOnEnd = UnOccupiedEndPosition.Count;
//						Transform father = rayHit.collider.gameObject.transform.parent;
						Transform father = FindRootParent(rayHit.collider.gameObject.transform);
						Vector3 ObjectOriginalPosition = father.localPosition;
						father.SetParent(null);
						CCActionOffBoard offBoard = CCActionOffBoard.GetSSAction (father.gameObject,UnOccupiedEndPosition[indexOfPositionOnEnd-1]);
						actionManager.RunAction (offBoard, null);
//						father.position= UnOccupiedEndPosition[indexOfPositionOnEnd-1];
						OccupiedEndPosition.Add(UnOccupiedEndPosition[indexOfPositionOnEnd-1]);
						UnOccupiedEndPosition.RemoveAt(indexOfPositionOnEnd-1);
						UnOccupiedBoatPosition.Add (ObjectOriginalPosition);
						OccupiedBoatPosition.Remove (ObjectOriginalPosition);
						EndPriests.Add (father.gameObject);
						BoatPriests.Remove (father.gameObject);
						if (EndDevils.Count == 3 && EndPriests.Count == 3) {
							checkResult ();
						}
						return true;
					}
				}
			}else if (rayHit.collider.gameObject.tag.Equals("Devil")) {
				/*Devil OffBoard to Start*/
				if (rayHit.collider.gameObject.transform.position.x>0) {
					if (Boat.transform.position.Equals (BoatPositionOne) && rayHit.collider.gameObject.transform.IsChildOf(Boat.transform)) {
						int indexOfPositionOnStart = UnOccupiedStartPosition.Count;
//						Transform father = rayHit.collider.gameObject.transform.parent;
						Transform father = FindRootParent(rayHit.collider.gameObject.transform);
						Vector3 ObjectOriginalPosition = father.localPosition;
						father.SetParent(null);
						CCActionOffBoard offBoard = CCActionOffBoard.GetSSAction (father.gameObject,UnOccupiedStartPosition[indexOfPositionOnStart-1]);
						actionManager.RunAction (offBoard, null);
//						father.position= UnOccupiedStartPosition[indexOfPositionOnStart-1];
						OccupiedStartPosition.Add(UnOccupiedStartPosition[indexOfPositionOnStart-1]);
						UnOccupiedStartPosition.RemoveAt(indexOfPositionOnStart-1);
						UnOccupiedBoatPosition.Add (ObjectOriginalPosition);
						OccupiedBoatPosition.Remove (ObjectOriginalPosition);
						StartDevils.Add (father.gameObject);
						BoatDevils.Remove (father.gameObject);
						return true;
					}

				} else {
					/*Devil OffBoard to End*/
					if (Boat.transform.position.Equals (BoatPositionTwo) && rayHit.collider.gameObject.transform.IsChildOf(Boat.transform)) {
						int indexOfPositionOnEnd = UnOccupiedEndPosition.Count;
//						Transform father = rayHit.collider.gameObject.transform.parent;
						Transform father = FindRootParent(rayHit.collider.gameObject.transform);
						Vector3 ObjectOriginalPosition = father.localPosition;
						father.SetParent(null);
						CCActionOffBoard offBoard = CCActionOffBoard.GetSSAction (father.gameObject,UnOccupiedEndPosition[indexOfPositionOnEnd-1]);
						actionManager.RunAction (offBoard, null);
//						father.position= UnOccupiedEndPosition[indexOfPositionOnEnd-1];
						OccupiedEndPosition.Add(UnOccupiedEndPosition[indexOfPositionOnEnd-1]);
						UnOccupiedEndPosition.RemoveAt(indexOfPositionOnEnd-1);
						UnOccupiedBoatPosition.Add (ObjectOriginalPosition);
						OccupiedBoatPosition.Remove (ObjectOriginalPosition);
						EndDevils.Add (father.gameObject);
						BoatDevils.Remove (father.gameObject);
						if (EndDevils.Count == 3 && EndPriests.Count == 3) {
							checkResult ();
						}
						return true;
					}
				}
			} 

		}
		Debug.Log ("Direction:" + PositionBoatWillGoTo.x + "  StartPriest:" + StartPriests.Count + "  StartDevils:" + StartDevils.Count +
		"  BoatPriests" + BoatPriests.Count + "  BoatDevils:" + BoatDevils.Count + "  EndPriests:" + EndPriests.Count + "  EndDevils:" + EndDevils.Count);
		return false;
	}

	public float getTimeLeft() {
		/*get the clock value and show it to player*/
		return TimeLeft;
	}


	public void checkResult() {
		/*When move to East we need to check the west side 's Devils' number(west just end)*/
		if (PositionBoatWillGoTo == BoatPositionOne) {
			if (EndDevils.Count > EndPriests.Count && EndPriests.Count != 0) {
				Debug.Log ("Result:Failed");
				GameCurrentState = result.Failed;
				return;
			} else if (StartDevils.Count + BoatDevils.Count > StartPriests.Count + BoatPriests.Count &&
			           StartPriests.Count + BoatPriests.Count > 0) {
				/*When move to East we also need to check the sum of East side and OnBoat*/
				Debug.Log ("Result:Failed");
				GameCurrentState = result.Failed;
				return ;
			}
			/*When move to west we need to check the east side's Devils' number (east just start)*/
		} else if (PositionBoatWillGoTo == BoatPositionTwo) {
			if (StartDevils.Count > StartPriests.Count && StartPriests.Count != 0) {
				Debug.Log ("Result:Failed");
				GameCurrentState = result.Failed;
				return ;
			} else if (EndDevils.Count + BoatDevils.Count > EndPriests.Count + BoatPriests.Count &&
			           EndPriests.Count + BoatPriests.Count > 0) {
				/*move to west we need to check the sum of West(End) and Boat*/
				Debug.Log ("Result:Failed");
				GameCurrentState = result.Failed;
				return ;
			} else if (EndPriests.Count == 3 && EndDevils.Count == 3) {
				Debug.Log ("Result:Succeed");
				GameCurrentState = result.Succeed;
				return ;
			}
		}
		Debug.Log ("Result:Playing");
		GameCurrentState = result.Playing;
		return ;
	}

	/*return state to UI*/
	public int getCurrentState() {
		if (GameCurrentState == result.Failed)
			return 0;
		if (GameCurrentState == result.Playing)
			return 1;
		return 2;
	}
	/*Find the parent through Recursive method*/
	Transform FindRootParent(Transform child){
		if (child.parent != null&&child.parent.name!="Boat") {
			return FindRootParent (child.parent);
		}
		return child;
	}



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
//		if (IsBoatRunning) {
//			Boat.transform.position = Vector3.MoveTowards (Boat.transform.position,PositionBoatWillGoTo,0.25f);
//		}
		if (Boat.transform.position.Equals (PositionBoatWillGoTo)) {
			IsBoatRunning = false;
		}
		if (TimeLeft > 0.0f&&GameCurrentState==result.Playing) {
			TimeLeft -= Time.deltaTime;
		} else if (TimeLeft <=0.0f){
			TimeLeft = 0.0f;
			GameCurrentState = result.Failed;
		}
	}
}

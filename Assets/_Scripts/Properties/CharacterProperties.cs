using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public  class CharacterProperties : Properties {

	//Bools
		//Allomancy


	public bool ironActive{ get; set; }
	public bool steelActive{ get; set; }
	public bool tinActive{ get; set; }
	public bool pewterActive{ get; set; }
	public bool zincActive{ get; set; }
	public bool brassActive{ get; set; }
	public bool copperActive{ get; set; }
	public bool bronzeActive{ get; set; }
	public bool chromiumActive { get; set; }
	public bool duraluminActive{ get; set; }
	public bool atiumActive{ get; set; }

	public bool sightActive{ get; set; }


		//allomancy utilities
	public bool turnOffPewter{ get; set; } //disable pewter next frame




	public bool isFlaring{ get; set; }

	//Ints
	public  int coins{ get; set; }
	public  int bookmarksCounter{ get; set; }

	//Doubles
	public double stamina{ get; set;}
	public double mStamina{ get; set;}
	
	public double speed{ get; set; } //current movement speed
	public double mSpeed{get;set;} //max speeed
	public double oMSpeed{ get; set; } //original max speed

	public double jHeight{ get; set; } //jump height, to solve weird things with dif sizes
	public double oJHeight{ get; set; }

	public double flareTimer{ get; set; } //how long flare lasts
	public double flareBurnRate{ get; set; } //multiply by burn rate for rate during flare

	public double weaponSkill{ get; set; }
	public double oWeaponSkill{ get; set; }

	public double uFlareCD{ get; set; } //universal cd between flares

	public double mArsenal = 1000; //maximum of single metal storable


	public double feStFlareMulti{ get; set; }

	//distance handling
	
	public float distModifier{ get; set; }
	public float distanceFalloff{ get; set; }
	public float interval{ get; set; } //for each of this interval, reduce total acceleration by x percent
	public float reductionPercent{ get; set; }
	
	

	
	public float maxVel = 100; //max velocity before taking damage on impact
	
	
	
	//Arrays 
	public double[] Arsenal = new double[11];
	public double[] BurnRate = new double[11]; //lower is better //=   //references the enum order in Properties
	public double[] AllomanticStrength = new double[11];//higher is better // 
	public double[] FlareCD = new double[11]; //flare cd per element
	public List<GameObject> Bookmarks;
	public List<GameObject> PushList;
	public List<GameObject> PullList;





}

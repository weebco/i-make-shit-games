using UnityEngine;
using System.Collections;

public  class Properties : MonoBehaviour{

	//Enums
	public enum Element{
	NONE=50, //not metal (characters, dirt, trees, glass)
	IMPURE=100, //metal but not allomantic (environment)
	IRON=0, //Pull
	STEEL=1, //Push
	TIN=2,  //Senses
	PEWTER=3, //Strength
	ZINC=4, //Riot
	BRASS=5, //Sooth
	COPPER=6, //Smoker
	BRONZE=7, //Seeker 
	CHROMIUM=8, //Destroy others metals
	DURALUMIN=9,  //Boost next flare, consume all supply
	ATIUM=10  //God
	};
	Element element{ get; set;}

	protected const int IRON = 0;
	protected const int STEEL = 1;
	protected const int TIN = 2;
	protected const int PEWTER = 3;
	protected const int ZINC = 4;
	protected const int BRASS = 5;
	protected const int COPPER = 6;
	protected const int BRONZE = 7;
	protected const int CHROMIUM = 8;
	protected const int DURALUMIN = 9;
	protected const int ATIUM = 10;

	public double[] uBurnRate = {1.7,1.7,1.8,5.0,1.0,1.0,1.4,2.0,2.0,10,5};



	//Bools
	public  bool isMetal{ get; set; }
	public  bool isDestructible{ get; set; }
	public bool isAirborne{ get; set; }


	//Ints
	public  double weight{ get; set; } //in kilograms

	public  double health{ get; set; }
	public  double mHealth{ get; set; }

}

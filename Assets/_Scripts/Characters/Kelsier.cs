using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System;


public class Kelsier : CharacterProperties {

	public Animator anime;
	public PhysicMaterial playerPhysics;
	public AnimatorStateInfo currentAnimeState;		// 
	public AnimatorStateInfo previousAnimeState;
	public Rigidbody rb;
	public Text arsenalText;  //Top left UI text
	public int x;
	public string currentArsenalText;
	public Vector3 down;
	public Vector3 fwd;
	const float radius = .6f;
	public const int jumpCD = 60;  //60 recommended: 1 boosted jump per seconds
	public int jumpTimer = 0;
	public const int pushCD = 10;
	public int pushTimer = 0;

	public float speedX = 0;
	public float speedY = 0;

	public float cSpeed = 0;

	public bool jumping = false;

	public  Vector3 emptyVec = new Vector3 (0f, 0f, 0f);






	GameObject lockedObj;



	// Use this for initialization
	void Start () {

		Application.targetFrameRate = 60; //frame rate lock

		anime = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody>();
		down = new Vector3 (0, -10, 0);
		fwd = new Vector3 (0, 0, 10);


		bookmarksCounter = 0;

		//bools
	    isMetal = false;  //replace with tags?
		isDestructible = false; //replace with tags?
		//set initial states inactive
		ironActive = false;
		steelActive = false;
		tinActive = false;
		pewterActive = false;
		zincActive = false;
		brassActive = false;
		copperActive = false;
		bronzeActive = false;
		chromiumActive = false;
		duraluminActive = false;
		atiumActive = false;

		//doubles
		coins = 25;

		weight = 60; // in kilos, 132~ lb

		mHealth = 125;
		health = mHealth;

		mStamina = 125;
		stamina = mStamina;

		speed = 0;
		mSpeed = 25;
		oMSpeed = 25;

		jHeight = 72;
		oJHeight = 72;

		flareTimer = 5;
		flareBurnRate = 3;

		weaponSkill = 1.1;
		oWeaponSkill = weaponSkill;

		uFlareCD = 0;

		isFlaring = false;

		feStFlareMulti = 1;

		//distance handling

		  distModifier = 1;
		  distanceFalloff = 5f;
		 interval = .1f; //for each of this interval, reduce total acceleration by x percent
		 reductionPercent = 10f;



		//currentAnimeState = anime.GetCurrentAnimatorStateInfo (0);



		//arrays
		// these will not work because the inspector overrides with its own values and id rather work in scripts.
		//double[] BurnRate = {1,1,1,.9,1,1,1,1,1,.8}; //atium, pewter bonus
		//double[] AllomanticStrength = {1.3,1.3,1.1,1.0,.8,.8,.9,1.0,0.6,0.6,1.5};
		//double[] FlareCD = {5,5,5,20,5,5,30,30,60,20,120};
		//bonuses to pull/push, senses, atium.  
		//penalties to soothe, riot, smoker, destroy other, flare boost



				}//end start function

	public bool shift(){
		return (Input.GetKey (KeyCode.LeftShift)|| Input.GetKey(KeyCode.RightShift));
	}

	public bool braced(Rigidbody body, Vector3 dir){
		RaycastHit hit;
		Rigidbody rigidTarg = body.GetComponent<Rigidbody>();
		if (Physics.Raycast (body.position, dir, out hit, .51f)) {
			Rigidbody rigidBrace = hit.collider.attachedRigidbody;
			Debug.DrawRay(body.position, dir,Color.yellow);
			if(rigidBrace == null)
			{
				Debug.Log ("no rigidbody.");
				return true;
			}
			if(rigidBrace.mass + rigidTarg.mass > 1.5*(rb.mass))
			{
				Debug.Log ("Braced");
				return true;
			}
		}
		Debug.Log ("Not braced");
		return false;
	}


	public void shootCoin()
	{
		if (coins > 0) {


		}
	}

	public bool checkGrounded(){
		jumping = false;
		return Physics.Raycast (rb.position, down, .1f);
	}

	public bool metalUnder(){
		RaycastHit hit;
		if (Physics.Raycast (rb.position, down, out hit, 9.5f)) {  //9.5f distance recommended
			//Debug.Log(hit.collider.gameObject.name);
			return(hit.collider.CompareTag("Metallic"));
		}
		return false;


	}



	void Update(){
		isAirborne = false;

		if (!Physics.Raycast (rb.position, down, .6f)) {  //casts ray directly underneath to check for ground.
			isAirborne = true;
		}

		Vector3 adjustPosition = new Vector3(rb.position.x, rb.position.y + .85f, rb.position.z);





		if (x < 1) {   //build initial loadout, overwrites inspector
			Arsenal.SetValue (1000, IRON);
			Arsenal.SetValue (500, STEEL);
			Arsenal.SetValue (50, TIN);
			Arsenal.SetValue (500, PEWTER);
			Arsenal.SetValue (50, ZINC);
			Arsenal.SetValue (50, BRASS);
			Arsenal.SetValue (50, COPPER);
			Arsenal.SetValue (50, BRONZE);
			Arsenal.SetValue (0, CHROMIUM);
			Arsenal.SetValue (25, DURALUMIN);
			Arsenal.SetValue (1, ATIUM);  //somehow not bugged to shit
		
			BurnRate.SetValue(.8, IRON);
			BurnRate.SetValue(.8, STEEL);
			BurnRate.SetValue(1, TIN);
			BurnRate.SetValue(.8, PEWTER);
			BurnRate.SetValue(1, ZINC);
			BurnRate.SetValue(1, BRASS);		
			BurnRate.SetValue(1, COPPER);
			BurnRate.SetValue(1, BRONZE);
			BurnRate.SetValue(1, CHROMIUM);
			BurnRate.SetValue(1, DURALUMIN);
			BurnRate.SetValue(.75, ATIUM); //bugged to shit //NOT ANYMORE NIGGAAAAAA
		//WHAT WE LEARNED FROM THE ABOVE BEING BUGGED TO SHIT:
			// CHECK ARRAY SIZES IN INSPECTOR, SCRIPT DOESNT ALWAYS OVERRIDE PROPERLY
			//IF BUGGED JUST MAKE SURE ARRAY SIZE IS RIGHT IN INSPECTOR, IF NOT, INCREASE THERE
			//NO NEED TO FUCK AROUND HERE
			AllomanticStrength.SetValue(1.5, IRON);
			AllomanticStrength.SetValue(1.5, STEEL);
			AllomanticStrength.SetValue(1.2, TIN);
			AllomanticStrength.SetValue(1.2, PEWTER);
			AllomanticStrength.SetValue(0.8, ZINC);
			AllomanticStrength.SetValue(0.8, BRASS);
			AllomanticStrength.SetValue(0.8, COPPER);
			AllomanticStrength.SetValue(1.0, BRONZE);
			AllomanticStrength.SetValue(0.5, CHROMIUM);
			AllomanticStrength.SetValue(0.5, DURALUMIN);
			AllomanticStrength.SetValue(1.5, ATIUM);  //also bugged to shit

			coins = 25; //TODO create wallet object that holds coins, not player
			x=2;
		}



		//PEWTER EFFECTS

		if (Input.GetKeyDown("t")) { //only triggers once when key pressed and wont reset till released

			if(pewterActive){
				turnOffPewter = true;
			}
			if(!turnOffPewter)
			{
				pewterActive = true;
			}
		}

		if (Input.GetKeyDown("r")) { //WORKS
			if(!isFlaring  && uFlareCD == 0)
			{
				isFlaring = true;
			}
			else if(isFlaring){
				isFlaring = false;
			}

		}


		if (Input.GetKeyDown ("e")) {
			if(sightActive == false && !feStDepleted())
			{
				sightActive = true;
			}
			else{
				sightActive = false;
			}
		}






			if (feStDepleted())
			{
				sightActive = false;
			}

		//Following logic governs ironsight, push and pull and is mildly complicated
		//dont mess with it.
			 //TODO swap to lists for dynamic sizing.
			GameObject[] objs2 = new GameObject[500];
			GameObject[] objs = new GameObject[500];
			//add objects to arrays by tags
			objs = GameObject.FindGameObjectsWithTag("Metallic");
			objs2 = GameObject.FindGameObjectsWithTag("Pick Up[IRON]");
			//combine arrays using linq
			GameObject[] finalObjs = new GameObject[(objs.Length + objs2.Length)]; //array is sized to hold exactly how many elements constitute the others.  Artificially dynamic
			finalObjs = objs.Concat(objs2).ToArray();
			//Debug.Log("finalObjs length: " + finalObjs.Length);
			objs = null;
			objs2 = null;
			//iterate through array drawing line to each
			//if obj is destroyed or set inactive the line will disappear next frame
		if (sightActive) {
			foreach (GameObject obj in finalObjs){
				Color lineColor;
				lineColor = Color.cyan; /* //no rigidbody, just testing, all should be blue in real
				
				if (obj.GetComponent<Rigidbody>())
				{
					Rigidbody objRB = obj.GetComponent<Rigidbody>();
					if(objRB.mass > rb.mass){
						lineColor = Color.blue;  //heavier
					}
					if(objRB.mass < rb.mass){
						lineColor = Color.cyan; //lighter
					}
					if(objRB.mass == rb.mass){
						lineColor = Color.magenta;  //equal
					}
				}*/
				Debug.DrawLine(adjustPosition, obj.transform.position, lineColor);

		}

				               }
			List<float> distances = new List<float>();
			GameObject targetObject;
			foreach (GameObject obj in finalObjs)
			{
				//get distance
				distances.Add(Vector3.Distance(obj.transform.position, rb.position));              //          
			}

		float dist = distances.Min();
	//	Debug.Log ("Shortest distance: " + dist);

		int closestIndex  =  distances.IndexOf(dist); //check for position in index

		targetObject = finalObjs[closestIndex]; //~salamander wiggle~

		if (sightActive) {
			Debug.DrawLine(adjustPosition, targetObject.transform.position, Color.green);

		}
		if (lockedObj != null && lockedObj.activeSelf == false ) {
			lockedObj = null;
		}
		if (lockedObj != null) {
			targetObject = lockedObj;    //OVERRIDE PROXIMITY TARGET WITH LOCKED
		}

		Vector3 direction = (rb.position - targetObject.transform.position).normalized; //calculate direction
		//Debug.Log (direction);

		if(Input.GetKeyDown("n"))
		{
			if(!shift()){
				if(lockedObj = null)
				{
					lockedObj = targetObject;

				}
				else{
					lockedObj = finalObjs[closestIndex];
				}
				Debug.Log("Locking to target object... " + lockedObj.name);
			}
			if(shift()){
				if (Bookmarks.Contains(targetObject)){
					Bookmarks.Remove(targetObject);
				}
				else{
					Bookmarks.Add(targetObject);
					Debug.Log("Added to bookmarks..." + targetObject.name);
					
				}

			}


		}

/*
		if (Input.GetKeyDown (KeyCode.Tab) && Bookmarks.Count > 0) {
			bookmarksCounter

		}
*/

		if (sightActive) {
			foreach (GameObject obj in Bookmarks)
			{
				if(obj.activeSelf)
				{
				Debug.DrawLine(adjustPosition, obj.transform.position, Color.magenta);
				}
				else
				{
					Bookmarks.Remove(obj);
				}
			}
		}


		if (Input.GetKeyDown ("m")) {
			lockedObj = null;
			Debug.Log("Unlocked from target object...");
		}

		
		if (sightActive) {
			if(lockedObj != null){
			//	Debug.Log ("trying to drawline to locked obj...");
				Debug.DrawLine(adjustPosition, lockedObj.transform.position, Color.red);
			}
		}
		
		

			if(Input.GetKey("v") && Arsenal[IRON] > 0){
				//determine closest obj
					Arsenal.SetValue(Arsenal[IRON] - .1*(BurnRate[IRON]*uBurnRate[IRON]), IRON);







			if (targetObject.GetComponent<Rigidbody>()) {                                        //counter force
				Rigidbody targetObjRB = targetObject.GetComponent<Rigidbody>();
				targetObjRB.AddForce((float)feStFlareMulti*(direction*10.5f*(float)AllomanticStrength[IRON]/(dist/15)));
			}
			rb.AddForce((float)feStFlareMulti*(-direction*10.5f*(float)AllomanticStrength[IRON]/(dist/15)));
			//Debug.Log((float)feStFlareMulti*(-direction*10.5f*(float)AllomanticStrength[IRON]/(dist/10)).magnitude);
				}
			
			if(Arsenal[IRON] < 0)
			{
				Arsenal.SetValue(0,IRON);
				ironActive = false;
			}

			if (steelActive){// burn iron if thats whats active, slower drain than if using fully
				
				Arsenal.SetValue(Arsenal[STEEL] - .003*(BurnRate[STEEL]*uBurnRate[STEEL]), STEEL);
			}
				if(Input.GetKey("c") && Arsenal[STEEL]> 0 && pushTimer == 0){

				//FUNCTION WAS MOVED TO RUN REGARDLESS OF ACTIVATION OF IRON/STEEL.  REENABLE THIS ONLY IF YOU DISABLE THE ABOVE BECAUSE OF PERFORMANCE ISSUES, AND BE SURE TO COPY TO IRON METHOD AS WELL
			/*		//determine closest obj 
					List<float> distances = new List<float>();
					GameObject targetObject;
					foreach (GameObject obj in finalObjs)
					{
						//get distance
						distances.Add(Vector3.Distance(obj.transform.position, rb.position));              //             new Vector3(Mathf.Abs(obj.transform.position.x - rb.position.x), Mathf.Abs(obj.transform.position.y - rb.position.y), Mathf.Abs (obj.transform.position.z - rb.position.z)));
					}
					float dist = distances.Min();
					int closestIndex  =  distances.IndexOf(dist); //System.Array.IndexOf(distances, dist) ; // .IndexOf(distances, dist);  //check for position in index
					targetObject = finalObjs[closestIndex];
					Vector3 direction = (rb.position - targetObject.transform.position).normalized; //calculate direction */




			pushTimer = pushCD;
			if (targetObject.GetComponent<Rigidbody>()) {                                        //counter force
				Rigidbody targetObjRB = targetObject.GetComponent<Rigidbody>();
				//targetObjRB.AddForce((float)feStFlareMulti*(-direction*10.5f*(float)AllomanticStrength[IRON]/(dist)));
			


				if(dist > distanceFalloff)
				{//INSTRUCTIONS FOR DIST MODIFIER: For each interval amount over distanceFalloff, reduce the total acceleration generated by reductionPercent.
					//the function will return 0 at reduction%*100*interval + distanceFalloff

					distModifier = 1f - ((dist/interval)/(reductionPercent*100f));
					if (distModifier < 0)
					{
						distModifier = 0;
					}
				}



				//Debug.Log(distModifier);
				float A; //total acceleration
				A = (float)distModifier*(((float)AllomanticStrength[STEEL]*1000*rb.mass) / (((rb.mass)*(rb.mass))));//TODO
				//A = (float)(((float)AllomanticStrength[STEEL]*10000*rb.mass) / ((dist*dist)*((rb.mass+targetObjRB.mass)*(rb.mass+targetObjRB.mass))));//TODO
				//Debug.Log(A);
				if ( A > 1000 && A < 100000)
				{
					A = A*(1-2*(A/1000));
				//	Debug.Log ( " Adjusting acceleration");
				}
				// total acceleration = (strength*mass)/ (Distance^2(mass+targetMass)^2))
				float totalMass = rb.mass + targetObjRB.mass;
				float PercTarg = rb.mass/totalMass;
				float PercRB = targetObjRB.mass/totalMass;
				Arsenal.SetValue(Arsenal[STEEL] - .1*(BurnRate[STEEL]*uBurnRate[STEEL]) , STEEL);

				
				if(Arsenal[STEEL] < 0)
				{
					Arsenal[STEEL] = 0;
				}



				if(braced(targetObjRB, -direction ))
				{
					PercRB = 1f;
					PercTarg = 0f;
				}
				if(braced(rb, direction ))
				{
					PercRB = .05f;
					PercTarg = 1.1f;
					Debug.Log("You are braced!");
					if(braced(targetObjRB, -direction ))
					{
						PercRB = .5f;
						PercTarg = .5f;
					}
				}
				if(pewterActive)
					
				{
					PercRB = PercRB*.75f;
				}
				Debug.Log ("Self: " + PercRB + " Target: " + PercTarg);
				float Arb = A*PercRB;
				float AObjRB = A*PercTarg;
				//Debug.Log (" Accelerations: " + Arb + " Targ " + AObjRB);
				float FmagRb = rb.mass*Arb;
				float FmagTargRB = targetObjRB.mass*AObjRB;
			//	Debug.Log (" Force magnitudes: " + FmagRb + "  Targ:" + FmagTargRB);
				rb.AddForce(direction*Math.Abs(FmagRb));
				//Debug.Log("RB Force: " + direction*FmagRb) ;
				targetObjRB.AddForce(-direction*Math.Abs(FmagTargRB));
				//	Debug.Log("Target RB Force: " + direction*FmagTargRB + "RB Force: " + direction*FmagRb) ;

			}

		//	rb.AddForce((float)feStFlareMulti*(direction*10.5f*(float)AllomanticStrength[STEEL]/(dist)));

				}
	
		





		Vector3 jForce = new Vector3 (0, (float)jHeight, 0);

		if (Input.GetKey ("space") ) { //jump nigga
			RaycastHit hit;
			if( metalUnder() && Arsenal[STEEL] > 0 && jumpTimer == 0){
				//Debug.Log("jumpTimer is " + jumpTimer + " and has been set to: " + jumpCD );
					rb.AddForce(new Vector3 ( 0, 750*(float)AllomanticStrength[STEEL],0  ));
				jumpTimer = jumpCD;

			}
			else {
				if(checkGrounded()){
				//	anime.Play("JUMP");
					rb.AddForce(jForce);
					jumpTimer = 5;
					jumping = true;
				}

			}
		}

		if (jumpTimer > 0) {
			jumpTimer--;
			//	Debug.Log(jumpTimer);
		} else {
			jumping = false;
		}

		if (pushTimer > 0) {
			pushTimer--;
			//Debug.Log (pushTimer);
		}


		if (pewterActive) {   //WORKS
			if(isFlaring){
				Arsenal.SetValue(Arsenal[PEWTER] - .01*AllomanticStrength[PEWTER]* AllomanticStrength[PEWTER]*(BurnRate[PEWTER]*uBurnRate[PEWTER]), PEWTER);
				
				if(Arsenal[PEWTER] < 0)
				{
					Arsenal.SetValue(0,PEWTER);
					turnOffPewter = true;
				}
				
				//distorary physical enhancements
				jHeight = oJHeight * 3.9 * AllomanticStrength[PEWTER]* AllomanticStrength[PEWTER]* AllomanticStrength[PEWTER];
				mSpeed = oMSpeed * 1.2 * AllomanticStrength[PEWTER]* AllomanticStrength[PEWTER]* AllomanticStrength[PEWTER];
				weaponSkill = oWeaponSkill * 1.25 * AllomanticStrength[PEWTER]* AllomanticStrength[PEWTER]* AllomanticStrength[PEWTER];
			}
			else{ //pewter on but not flared
			Arsenal.SetValue(Arsenal[PEWTER] - .005*(BurnRate[PEWTER]*uBurnRate[PEWTER]), PEWTER);

			if(Arsenal[PEWTER] < 0)
			{
				Arsenal.SetValue(0,PEWTER);
				turnOffPewter = true;
			}

			//distorary physical enhancements
			jHeight = oJHeight * 1.9 * AllomanticStrength[PEWTER];
			mSpeed = oMSpeed * 1.9 * AllomanticStrength[PEWTER];
			weaponSkill = oWeaponSkill * 1.15 * AllomanticStrength[PEWTER];
			} //end else

			if (turnOffPewter){  //disable pewter and reset all physical buffs
				turnOffPewter = false;
				jHeight = oJHeight;
				mSpeed = oMSpeed;
				pewterActive=false;

			}
		}

				//UI AND TIMERS ____________________________________

		if (metalUnder ()) {

			Debug.DrawRay(rb.position, new Vector3(0, 5, 0), Color.yellow);
		}


	//	Debug.Log (rb.velocity);


		//flare timers
		if (isFlaring) {
			uFlareCD +=1.5;
			feStFlareMulti = 1.5;
		}

		if (uFlareCD > 500) {
			isFlaring = false;
		}
		if (!isFlaring) {
			feStFlareMulti = 1;
			if (uFlareCD < 0)
			{
				uFlareCD = 0;
			}
			if(uFlareCD > 0)
			{
				uFlareCD --;
			}
		}

		anime.SetBool ("Jumping", jumping);
		//Debug.Log (jumping);
		if (!anime.IsInTransition(0)) {
			jumping = false;
		}


		if(Mathf.Abs(rb.velocity.x) < .1f && Mathf.Abs(rb.velocity.y) < .1f && Mathf.Abs(rb.velocity.z) < .1f)

		{
			rb.velocity = (emptyVec);
		}

		//build string
		currentArsenalText = "Press e to use vision and enable Iron/Steel" + "\n(V)Iron: " + Arsenal.GetValue(IRON) + "\n(C)Steel: " + Arsenal.GetValue(STEEL) + "\nTin: " + Arsenal.GetValue(TIN)+ "\n(T) Pewter: " + Arsenal.GetValue(PEWTER)+ "\nZinc: " + Arsenal.GetValue(ZINC)+ "\nBrass: " + Arsenal.GetValue(BRASS)+ "\nCopper: " + Arsenal.GetValue(COPPER)+ "\nBronze: " + Arsenal.GetValue(BRONZE)+ "\nChromium: " + Arsenal.GetValue(CHROMIUM)+ "\nDuralumin: " + Arsenal.GetValue(DURALUMIN)+ "\nAtium: " + Arsenal.GetValue(ATIUM) + "\nCoins: " + coins + "\nPewter: " + pewterActive + "\nMspeed: " + mSpeed + "\nBurnRate: " + BurnRate[PEWTER] +"\n(R)Flaring: " + isFlaring + "\nFlare CD: " + uFlareCD + "\n (F)Jump";
		arsenalText.text = currentArsenalText ; //display remaining metals

	}

	//END UPDATE/// <summary	/// </summary>



	
	void FixedUpdate ()
	{
		cSpeed = rb.velocity.magnitude;

		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		if (moveVertical < 0) {
			mSpeed *=.80;
		}
		if (moveVertical == 0) {
			mSpeed *= .85;
		}
		if (rb.velocity.magnitude > .001) {
			anime.SetFloat ("Speed", cSpeed);
		} else {
			anime.SetFloat ("Speed", 0);
			rb.velocity = emptyVec;
		}

		anime.SetBool ("Running", (speed > 5));
		anime.SetFloat ("HSpeed", moveHorizontal);
		anime.SetFloat ("VSpeed", moveVertical);
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		//Debug.Log (moveHorizontal);

		playerPhysics.dynamicFriction = 0;
		if (moveHorizontal ==0 && moveVertical == 0) {
			if(rb.velocity.magnitude < 1)
			{
				rb.velocity = emptyVec;
			}
			playerPhysics.dynamicFriction = 1.9f;
		}

		speed = mSpeed;

	


		if (checkGrounded ()) {
			if ( rb.velocity.magnitude + ((float)mSpeed*movement.magnitude/(float)rb.mass) > 18.5f && movement.magnitude != 0)  //limits speed
			{
				mSpeed = ((float)rb.mass*(22.5f-(float)rb.velocity.magnitude))/(float)movement.magnitude;
			}
			try{
			/*	Debug.Log (movement);
				Debug.Log (mSpeed);
				Debug.Log(movement*(float)mSpeed);  */
				rb.AddForce (movement * (float)mSpeed); //full speed movement on ground//TODO
			}
			catch{
				Debug.Log("rb.addforce fucked up: 715");
			}
			//	Debug.Log ("movin: " + rb.velocity.magnitude + (movement*(float)mSpeed).magnitude );
			//	Debug.Log (rb.velocity);
			
		} else if (!checkGrounded ()) {
			rb.AddForce (movement * (float)oMSpeed / 8f);  //directional influence when falling
		}


		Vector3 lookTo = new Vector3 ((float)((double)rb.transform.position.x + (double)rb.velocity.x / 20000), rb.position.y, (float)((double)rb.transform.position.z + (double)rb.velocity.z / 20000));
		Debug.Log (lookTo);


		//	if (rb.velocity != emptyVec) {
				//	transform.position = rb.transform.position;
				transform.LookAt (lookTo); //controls rotation
		//transform.LookAt (new Vector3 (0f, rb.position.y, 0f)); //controls rotation
		//}

		mSpeed = oMSpeed;

	}


	public void addToArsenal (int element, double amount){  //WORKS
		Arsenal [element] += amount;
		if (Arsenal[element] > mArsenal) {
			Arsenal[element] = mArsenal;
		}
	}

	public void addToArsenal (Element element, double amount){ //WORKS
		Arsenal[(int)element] += amount;
		if (Arsenal[(int)element] > mArsenal) {
			Arsenal[(int)element] = mArsenal;
		}
	}

	public bool feStDepleted(){
		return (Arsenal[IRON] + Arsenal[STEEL] == 0);
	}
	
	public void ApplyForce(Vector3 force) //WORKS
	{
		rb.AddForce (force);
	}
	
	void OnTriggerEnter(Collider other) //WORKS
	{
		if (other.gameObject.CompareTag ("Pick Up[IRON]")) //just testing
		{
			addToArsenal(Element.PEWTER, 200); //lol, they think iron is pewter
			other.gameObject.SetActive (false);
		}
		if (other.gameObject.CompareTag ("Jump")) //WORKS
		{
			rb.AddForce(new Vector3(0f,(float)jHeight*15,0f));
		}

	}
	

}


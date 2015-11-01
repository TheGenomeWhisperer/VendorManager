/*  Merchant (Vendor Manager)
|   Author: @Sklug  aka TheGenomeWhisperer
|
|   To Be Used with "InsertContinentName.cs" and "Merchant.cs"
|	For use in collaboration with the Rebot API
|
|   Last Update 25th Oct, 2015 */


public class DraenorMerchants {
	
	public static Fiber<int> Fib; 
    public static bool IsSpecialPathingNeeded;
	
	// Default Constructor()
	public DraenorMerchants() {}
	
	// Method:		"getMerchantInfo(int,bool,int)
	// 				vendorType represented by numbers 1-3
	//				1 = Food && Drink
	//				2 = Repair
	public static List<object> getMerchantInfo(int zoneID, bool factionIsHorde, int vendorType) {
			// Default list in case of no result
			List<object> result = new List<object>();
			if (vendorType < 1 || vendorType > 3) {
				vendorType = 1;
			}
            // Frostfire Ridge (and caves and phases and Garrison)
            if (zoneID == 6720 || zoneID == 6868 || zoneID == 6745 || zoneID == 6849 || zoneID == 6861 || zoneID == 6864 || zoneID == 6848 || zoneID == 6875 || zoneID == 6939 || zoneID == 7005 || zoneID == 7209 || zoneID == 7004 || zoneID == 7327 || zoneID == 7328 || zoneID == 7329) {
                return getFFR(factionIsHorde,vendorType);
            }

            // Gorgrond (and caves and phases)
            if (zoneID == 6721 || zoneID == 6885 || zoneID == 7160 || zoneID == 7185) {
                return getGorgrond(factionIsHorde,vendorType);
            }

            // Talador (and caves and phases)
            if (zoneID == 6662 || zoneID == 6980 || zoneID == 6979 || zoneID == 7089 || zoneID == 7622) {
                IsSpecialPathingNeeded = true;
                return getTalador(factionIsHorde,vendorType);
            }

            // Spires of Arak
            if (zoneID == 6722) {
                return getSpires(factionIsHorde,vendorType);
            }

            // Nagrand (and phased caves)
            if (zoneID == 6755 || zoneID == 7124 || zoneID == 7203 || zoneID == 7204 || zoneID == 7267) {
                return getNagrand(factionIsHorde,vendorType);
            }

            // Shadowmoon Valley (and caves and phases)
            if (zoneID == 6719 || zoneID == 6976 || zoneID == 7460 || zoneID == 7083 || zoneID == 7078 || zoneID == 7327 || zoneID == 7328 || zoneID == 7329) {
                return getSMV(factionIsHorde,vendorType);
            }

            // Tanaan Jungle
            if (zoneID == 6723) {
                return getTanaan(factionIsHorde,vendorType);
            }

            // Ashran (and mine)
            if (zoneID == 6941 || zoneID == 7548) {
                IsSpecialPathingNeeded = true;
                return getAshran(factionIsHorde,vendorType);
            }

            // Warspear
            if (zoneID == 7333) {
                return getWarspear(factionIsHorde,vendorType);
            }
			     
        // No matches
        return result;
    }
	
	// Return for all of these will be in the following format (XYZ = V3 coordinates):  List<object> locations = {FPName,X,Y,Z,npcID,bool... FPName,X,Y,Z,npcID, ...)
    private static List<object> getWarspear(bool factionIsHorde, int vendorType)
    {
        List<object> locations = new List<object>();
		if (factionIsHorde) {
			// Food & Drink
			if (vendorType == 1) {
				List<object> list0 = new List<object>(){};
				locations.AddRange(list0);
			}
			// Repair
			else if(vendorType == 2) {
				
			}
		}
        else
		{
			// Alliance locations
		}
		// Add Neutral locations
		// All Here...
		//
        return locations;  
    }

    private static List<object> getAshran(bool factionIsHorde, int vendorType)
    {
		List<object> locations = new List<object>();
		if (factionIsHorde) {
			// Food & Drink
			if (vendorType == 1) {
				List<object> list0 = new List<object>(){};
				locations.AddRange(list0);
			}
			// Repair
			else if(vendorType == 2) {
				
			}
		}
        else
		{
			// Alliance locations
		}
		// Add Neutral locations
		// All Here...
		//
        return locations; 
    }

	
	private static List<object> getTanaan(bool factionIsHorde, int vendorType)
    {
        List<object> locations = new List<object>();
		if (factionIsHorde) {
			// Food & Drink
			if (vendorType == 1) {
				List<object> list0 = new List<object>(){};
				locations.AddRange(list0);
			}
			// Repair
			else if(vendorType == 2) {
				
			}
		}
        else
		{
			// Alliance locations
		}
		// Add Neutral locations
		// All Here...
		//
        return locations; 
    }

    private static List<object> getSMV(bool factionIsHorde, int vendorType)
    {
        List<object> locations = new List<object>();
		if (factionIsHorde) {
			// Food & Drink
			if (vendorType == 1) {
				List<object> list0 = new List<object>(){};
				locations.AddRange(list0);
			}
			// Repair
			else if(vendorType == 2) {
				
			}
		}
        else
		{
			// Alliance locations
		}
		// Add Neutral locations
		// All Here...
		//
        return locations; 
    }

    private static List<object> getNagrand(bool factionIsHorde, int vendorType)
    {
        List<object> locations = new List<object>();
        if (factionIsHorde) {
			// Food & Drink
			if (vendorType == 1) {
				List<object> list0 = new List<object>(){};
				locations.AddRange(list0);
			}
			// Repair
			else if(vendorType == 2) {
				
			}
		}
        else
		{
			// Alliance locations
		}
		// Add Neutral locations
		// All Here...
		//
        return locations; 
    }

    private static List<object> getSpires(bool factionIsHorde, int vendorType)
    {
        List<object> locations = new List<object>();
        if (factionIsHorde) {
			// Food & Drink
			if (vendorType == 1) {
				List<object> list0 = new List<object>(){};
				locations.AddRange(list0);
			}
			// Repair
			else if(vendorType == 2) {
				
			}
		}
        else
		{
			// Alliance locations
		}
		// Add Neutral locations
		// All Here...
		//
        return locations;   
    }

    private static List<object> getTalador(bool factionIsHorde, int vendorType)
    {
        List<object> locations = new List<object>();
        if (factionIsHorde) {
			// Food & Drink
			if (vendorType == 1) {
				List<object> list0 = new List<object>(){};
				locations.AddRange(list0);
			}
			// Repair
			else if(vendorType == 2) {
				
			}
		}
        else
		{
			// Alliance locations
		}
		// Add Neutral locations
		// All Here...
		//
        return locations; 
    }

    private static List<object> getGorgrond(bool factionIsHorde, int vendorType)
    {
        List<object> locations = new List<object>();
        if (factionIsHorde) {
			// Food & Drink
			if (vendorType == 1) {
                
				List<object> list1 = new List<object>(){};
				locations.AddRange(list1);
			}
			// Repair
			else if(vendorType == 2) {
				if (Merchant.API.IsQuestCompleted(35151)) {
                    List<object> rep0 = new List<object>(){5779.6f, 1287.2f, 107.5f,82732,IsSpecialPathingNeeded};
				    locations.AddRange(rep0);
                }
			}
            // Both
            List<object> both0 = new List<object>(){6597.7f, 1276.3f, 64.7f, 84234, IsSpecialPathingNeeded};
            locations.AddRange(both0);
            if (Merchant.API.Me.Level > 99) {
                List<object> both1 = new List<object>(){7603.2f, 1804.1f, 81f, 85821, IsSpecialPathingNeeded};
                locations.AddRange(both1);
            }
            
		}
        else
		{
			// Alliance locations
		}
		// Add Neutral locations
		// All Here...
		//
        return locations; 
    }
	
	private static List<object> getFFR(bool factionIsHorde, int vendorType) {
        List<object> locations = new List<object>();
		if (factionIsHorde) {
			// Food & Drink
			if (vendorType == 1) {
                // Garrison Locations
                if (GetGarrisonLevel() > 1) {
                    List<object> list0 = new List<object>(){5568.0f, 4594.3f, 141.7f, 80151, IsSpecialPathingNeeded};
				    locations.AddRange(list0);
                }
                else {
                    List<object> list0 = new List<object>(){5569.9f, 4513.0f, 129.9f, 80151, IsSpecialPathingNeeded};
				    locations.AddRange(list0);
                }
                // All the Rest
                // Spire has a quest completion conditional to be added
                if (Merchant.API.IsQuestCompleted(33657)) {
                    List<object> list1 = new List<object>(){6789.5f, 5865.6f, 258.7f, 76746, IsSpecialPathingNeeded};
                    locations.AddRange(list1);
                }
                List<object> list2 = new List<object>(){7860.1f, 5551.3f, 111.1f, 78769, IsSpecialPathingNeeded};
                locations.AddRange(list2);
			}
			//  // Repair
			else if (vendorType == 2) {
				List<object> vend0 = new List<object>(){6141.0f, 5050.2f, 133.9f, 77456, IsSpecialPathingNeeded};
                locations.AddRange(vend0);
                List<object> vend1 = new List<object>(){6118.6f, 4991.5f, 133.5f, 78989, IsSpecialPathingNeeded};
                locations.AddRange(vend1);
                List<object> vend2 = new List<object>(){5910.6f, 6227.8f, 50.0f, 79448, IsSpecialPathingNeeded};
                locations.AddRange(vend2);
                List<object> vend3 = new List<object>(){5867.8f, 6340.8f, 62.9f, 77462, IsSpecialPathingNeeded};
                locations.AddRange(vend3);
                if (Merchant.API.IsQuestCompleted(33657)) {
                    List<object> vend4 = new List<object>(){6788.9f, 5867.3f, 258.7f, 74471, IsSpecialPathingNeeded};
                    locations.AddRange(vend4);
                    List<object> vend5 = new List<object>(){6734.4f, 5811.0f, 258.7f, 77470, IsSpecialPathingNeeded};
                    locations.AddRange(vend5);
                }
                List<object> vend6 = new List<object>(){7759.9f, 5588.7f, 110.9f, 78770, IsSpecialPathingNeeded};
                locations.AddRange(vend6);
                List<object> vend7 = new List<object>(){7429.7f, 4330.3f, 120.2f, 85545, IsSpecialPathingNeeded};
                locations.AddRange(vend7);
                
			}
            //  BOTH
            if (GetGarrisonLevel() > 1) {
                List<object> both0 = new List<object>(){5626.8f, 4629.7f, 139.3f, 76872, IsSpecialPathingNeeded};
                locations.AddRange(both0);
            }
            else {
                List<object> both0 = new List<object>(){5651.4f, 4527.0f, 119.1f, 76872, IsSpecialPathingNeeded};
                locations.AddRange(both0);
            }
		}
        else
		{
			// Alliance locations
		}
		// Add Neutral locations
		// All Here...
		//
        return locations;
	}
    
    public static List<int> getFood() {
        List<int> allFood = new List<int>() {115351,115352,115353,115354,115355,117457,117454};
        // Food IDs
        
        return allFood;
    }
    
    public static List<int> getWater() {
        List<int> allDrinks = new List<int>() {117452,117475};
        
        return allDrinks;
    }
	
	
    public static IEnumerable<int> doSpecialPathing() {
        int zoneID = Merchant.API.Me.ZoneId; 
        
        if (Merchant.API.Me.IsHorde) {
            // TALADOR ZONE SPECIAL PATHING!!!!!
            // Each Special condition is labeled.
            if (zoneID == 6662 || zoneID == 6980 || zoneID == 6979 || zoneID == 7089 || zoneID == 7622)
            {
                // To Navigate out of Gordal Fortress and safely get around the energy Barrier.
                // Initial logic is a positional check to see if player is inside the Fortress.
                // LOCATION 1
                Vector3 gord1 = new Vector3(1410f, 1728.5f, 310.3f);
                if (Merchant.API.Me.Distance2DTo(gord1) < 390)
                {
                    Vector3 gord2 = new Vector3(1666.5f, 1743.6f, 298.6f);
                    if ((Merchant.API.Me.Position.Z > 302.4) || ((Merchant.API.Me.Position.Z > 296.0) && (Merchant.API.Me.Distance2DTo(gord2) > 47.05)))
                    {
                        Merchant.API.Print("It Appears that You are in Gordal Fortress! Navigating Out...");
                        // Guided pathing out of Gordal Fortress
                        Vector3 gord3 = new Vector3(1645.4f, 1767.4f, 312.5f);
                        Vector3 gord4 = new Vector3(1674.5f, 1729.1f, 291.4f);
                        while (!Merchant.API.MoveTo(gord3))
                        {
                            yield return 100;
                        }
                        Merchant.API.Print("Let's Avoid that Energy Barrier!");
                        while(!Merchant.API.CTM(gord4))
                        {
                            yield return 100;
                        }
                        Merchant.API.Print("Alright! Back on Track!!!");
                    }
                    yield break;
                }
                
                // Navigate out of Zangarra Properly.
                // LOCATION 2
                Vector3 zang1 = new Vector3(3187.2f, 788.7f, 77.7f);
                Vector3 zang2 = new Vector3(3035.2f,  954.0f,  105.5f);
                if (Merchant.API.Me.Distance2DTo(zang1) < 302 && Merchant.API.Me.Distance2DTo(zang2) > 75)
                {
                    Merchant.API.Print("Let's First Get Out of Zangarra!");
                    // These quick 'Z' height checks are for some tight turns the mesh sometimes handles poorly.
                    if (Merchant.API.Me.Position.Z < 17)
                    {
                        Vector3 zang3 = new Vector3(3316.2f, 950.4f, 17.4f);
                        while(!Merchant.API.MoveTo(zang3))
                        {
                            yield return 100;
                        }
                    }
                    if (Merchant.API.Me.Position.Z < 32)
                    {
                        Vector3 zang4 = new Vector3(3286.9f, 1013.4f, 38.1f);
                        while(!Merchant.API.MoveTo(zang4))
                        {
                            yield return 100;
                        }
                    }
                    if (Merchant.API.Me.Position.Z < 44)
                    {
                        Vector3 zang5 = new Vector3(3206.1f, 918.8f, 42.2f);
                        while(!Merchant.API.MoveTo(zang5))
                        {
                            yield return 100;
                        }
                    }
                    Vector3 zang6 = new Vector3(3198.8f, 836.9f, 83.2f);
                    while(!Merchant.API.MoveTo(zang6))
                    {
                        yield return 100;
                    }
                    // Brief pause for aggro
                    yield return 2500;
                    Vector3 zang7 = new Vector3(3199.5f, 843.6f, 84.3f);
                    
                    while (Merchant.API.Me.Distance2DTo(zang7) < 30)
                    {
                        foreach (var unit in Merchant.API.GameObjects)
                        {
                            if (unit.EntryID == 230874)
                            {
                                while(!Merchant.API.MoveTo(unit.Position))
                                {
                                    yield return 100;
                                }
                                unit.Interact();
                                yield return 10000;
                            }
                        }
                    }
                    Merchant.API.Print("Alright, Let's Continue!");
                    yield break;
                }
                
                // Navigate out of Voljin's Pride Arsenal
                // LOCATION 3
                Vector3 arsenal = new Vector3(3217.1f, 1606.4f, 166.1f);
                if (Merchant.API.Me.Distance2DTo(arsenal) < 15)
                {
                    while(!Merchant.API.CTM(3226.4f, 1600.0f, 166.0f))
                    {
                        yield return 100;
                    }
                    while(!Merchant.API.CTM(3241.7f, 1589.6f, 163.2f))
                    {
                        yield return 100;
                    }
                    yield break;
                }
                
                // Navigational pathing too Shattrath City Spire Flightpath.
                // LOCATION 4
                Vector3 shatt1 = new Vector3(2604.9f, 2797.0f, 242.1f);
                Vector3 shatt2 = new Vector3(2943.0f, 3351.9f, 53.0f);
                if (Merchant.API.Me.Level > 99 && Merchant.API.Me.Distance2DTo(shatt2) < 430 && Merchant.API.Me.Position.Z < 125)
                {
                    Merchant.API.Print("Let's Move out of Shattrath. The elevator in the Sha'tari Market District Looks Good...");
                    var check = new Fiber<int>(TakeElevator(231934,7,2687.2f,3017.5f,69.5f,2682.8f,2995.0f,233.9f));
                    while (check.Run())
                    {
                        yield return 100;
                    }
                    Merchant.API.Print("Let's Get to that Flightpath and Get Out of Here!");
                    yield break;
                }
                yield break;     
            }
            // End Talador
            
            
            // ASHRAN SPECIAL PATHING!
            //
            // BEGIN
            if (zoneID == 6941 || zoneID == 7548)
            {
                Merchant.API.Print("Woah! Let's Get Out of Ashran Before Some Alliance Find You!");
                Vector3 ash = new Vector3(5090.1f, -3982.3f, 20.8f);
                while(!Merchant.API.MoveTo(ash))
                {
                    yield return 100;
                }
                Vector3 ash2 = new Vector3(5141.9f, -3964.1f, 2.2f);
                while(!Merchant.API.MoveTo(ash2))
                {
                    yield return 100;
                }
                yield break;
            }
            
            // Enter any additional pathing.
            yield break;
        }
        else {
            // Alliance Special pathing.
        }
    }
    
    // Method:          "GetGarrisonLevel();
    // What it Does:    Returns the current rank of the player garrison, 1-3
    // Purpose:         When dealing with various pathing at the Garrison, it is important to note that object
    //                  location often varies based on the level and size of the ggarrison. This helps filter it all.
    public static int GetGarrisonLevel()
    {
        return Merchant.API.ExecuteLua<int>("local level = C_Garrison.GetGarrisonInfo(); return level;");
    }
	
    // Method:          TakeElevator(int,int,float,float,float,float,float,float)
    // What it Does:    Allows the navigation of any elevator! This Elevator method allows the input of a starting position!
    // Purpose:         At times in the script, transversing an elevator effectively can be a cumbersome to program
    //                  and as such I wrote a scalable method... the only key thing needed is for the developer to
    //                  time how long it takes the elevator to go from the bottom to the top, or the other way around.
    //                  Also, the position you would like the player to exit the elevator and travel to.  The travel time
    //                  was kind of a rough solution because it appears that while on the elevator, the API freezes all return values
    //                  thus I cannot seem to get an accurate positional check, so the timing allows me to enter, then determine exit time.
    public static IEnumerable<int> TakeElevator(int ElevatorID, int elevatorTravelTime, float startX, float startY, float startZ, float x, float y, float z) 
    {
        double position;
        double position2;
        bool elevatorFound = false;
        // Starting position to navigate to and wait for elevator (PLACE AT SAME LEVEL AS Elevator)
        Vector3 start = new Vector3(startX,startY,startZ);
        Vector3 destination = new Vector3(x,y,z);
        
        while (!Merchant.API.MoveTo(start))
        {
            yield return 100;
        }
        foreach (var unit in Merchant.API.GameObjects)
        {
            // This first determines if the elevator is properly identified.
        	if (unit.EntryID == ElevatorID)
        	{
                elevatorFound = true;
                Merchant.API.SetFacing(unit);
                // The choice to disable combat is because once on the elevator, player should not attempt to leave it
                // or it could mess up the passing as the bot remembers its last spot before combat starts then returns to it
                Merchant.API.DisableCombat = true;
                Merchant.API.Print("Waiting For the Elevator...");
                position = Math.Sqrt(Merchant.API.Me.DistanceSquaredTo(unit));
                yield return 100;
                position2 = Math.Sqrt(Merchant.API.Me.DistanceSquaredTo(unit));
                yield return 100;
                
                // The two positional checks right after each other are to determine movement of the elevator.
                // if they are equal, elevator is not moving, but if they are different, like the second location is further than the first,
                // then it can easily be determined it is moving away from you.
                // This first check holds position until the elevator moves.  This is actually really critical because what if
                // player arrives at the elevator and the elevator is at location already.  The method would recognize this then quickly try to jump on.
                // This could create a problem though because what if the elevator was only going to be there a split second more, then player tries to
                // traverse then ends up missing it.  This just helps avoid that... Long explanation I know.
                while (position == position2)
                {
                    position = Math.Sqrt(Merchant.API.Me.DistanceSquaredTo(unit));
                    yield return 100;
                    position2 = Math.Sqrt(Merchant.API.Me.DistanceSquaredTo(unit));
                    yield return 100;
                }
                // Meaning it is moving away from you or it is at least 10 yrds away.
                if (position != position2 || Math.Sqrt(Merchant.API.Me.DistanceSquaredTo(unit)) > 10.0) 
                {
                    Merchant.API.Print("Elevator is Moving...");
                    if (position > position2) 
                    {
                        Merchant.API.Print("Elevator is Moving Towards Us... Almost Here!");
                    }
                    else 
                    {
                        Merchant.API.Print("Elevator is Moving Away! Patience!");
                        while(position != position2) 
                        {
                            position = Math.Sqrt(Merchant.API.Me.DistanceSquaredTo(unit));
                            yield return 100;
                            position2 = Math.Sqrt(Merchant.API.Me.DistanceSquaredTo(unit));
                            yield return 100;
                        }
                        Merchant.API.Print("Elevator Has Stopped at Other Side.  Let's Wait For It To Return!");
                        while(position == position2) 
                        {
                            position = Math.Sqrt(Merchant.API.Me.DistanceSquaredTo(unit));
                            yield return 100;
                            position2 = Math.Sqrt(Merchant.API.Me.DistanceSquaredTo(unit));
                            yield return 100;
                        }
                        Merchant.API.Print("Alright, It Is Coming Back to us. Get Ready!");
                    }
                    while(unit.Position.Z > (startZ + 1.0)) 
                    {
                        yield return 100;
                    }
                }
                Merchant.API.Print("Ah, Excellent! Elevator is Here! Hop On Quick!");
                Merchant.API.CTM(unit.Position);
                // The 4 seconds is added here to account for the stoppage of when you enter the elevator and it is stationary
                yield return ((elevatorTravelTime + 4) * 1000);
                while(!Merchant.API.CTM(destination)) 
                {
                    yield return 200;
                }
                Merchant.API.Print("You Have Successfully Beaten the Elevator Boss... Congratulations!!!");
        	}
        }
        if (!elevatorFound) 
        {
            Merchant.API.Print("No Elevator Found. Please Be Sure elevator ID is Entered Properly and You are Next to It");
            yield break;
        }
        Merchant.API.DisableCombat = false;
    }
	
}


// Create a method that returns BOTH vendor types

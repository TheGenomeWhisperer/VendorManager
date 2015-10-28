/*  Merchant (Vendor Manager)
|   Author: @Sklug  aka TheGenomeWhisperer
|
|   To Be Used with "InsertContinentName.cs" and "Merchant.cs"
|	For use in collaboration with the Rebot API
|
|   Last Update 25th Oct, 2015 */


public class DreanorMerchants {
	
	public static Fiber<int> Fib;
    public static bool IsSpecialPathingNeeded;
	
	// Default Constructor()
	public DreanorMerchants() {}
	
	// Method:		"getMerchantInfo(int,bool,int)
	// 				vendorType represented by numbers 1-3
	//				1 = Food && Drink
	//				2 = Repair
	public static List<object> getMerchantInfo(int zoneID, bool factionIsHorde, int vendorType) {
			// Default list in case of no result
			List<object> result = new List<object>();
			if (vendorType < 1 || vendorType > 3) {
				Merchant.API.Print("Please use a 1-3 to represent Vendor Type (1 = Food & Drink, 2 = Repair");
				return result;
			}
            // Frostfire Ridge (and caves and phases and Garrison)
            if (zoneID == 6720 || zoneID == 6868 || zoneID == 6745 || zoneID == 6849 || zoneID == 6861 || zoneID == 6864 || zoneID == 6848 || zoneID == 6875 || zoneID == 6939 || zoneID == 7005 || zoneID == 7209 || zoneID == 7004 || zoneID == 7327 || zoneID == 7328 || zoneID == 7329) {
                IsSpecialPathingNeeded = true;
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
			if 
			
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
        locations.Add(getFood());
        locations.Add(getDrink());
        return locations; 
    }

    private static List<object> getSpires(bool factionIsHorde, int vendorType)
    {
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
	
	private static List<object> getFFR(bool factionIsHorde, int vendorType) {
		if (factionIsHorde) {
			// Food & Drink
			if (vendorType == 1) {
                if (QH.GetGarrisonLevel() > 1) {
                    List<object> list0 = new List<object>(){5568.0f, 4594.3f, 141.7f, 80151, IsSpecialPathingNeeded};
				    locations.AddRange(list0);
                }
                else {
                    List<object> list0 = new List<object>(){5569.9f, 4513.0f, 129.9f, 80151, IsSpecialPathingNeeded};
				    locations.AddRange(list0);
                }
				
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
    
    public static List<int> getFood() {
        List<int> allFood = new List<int>();
        // Food IDs
        
        return allFood;
    }
    
    public static List<int> getWater() {
        List<int> allDrinks = new List<int>();
        
        return allDrinks;
    }
	
	
    public static IEnumerable<int> doSpecialPathing() {
        int zoneID = QH.API.Me.ZoneId;
        
        // FROSTFIRE RIDGE ZONE SPECIAL PATHING!
        //
        if (QH.API.IsInGarrison)
        {
            var check = new Fiber<int>(QH.GTownHallExit());
            while (check.Run()){
                yield return 100;
            }
        }       
        
        // TALADOR ZONE SPECIAL PATHING!!!!!
        //
        // Each Special condition is labeled.
        if (zoneID == 6662 || zoneID == 6980 || zoneID == 6979 || zoneID == 7089 || zoneID == 7622)
        {
            // To Navigate out of Gordal Fortress and safely get around the energy Barrier.
            // Initial logic is a positional check to see if player is inside the Fortress.
            // LOCATION 1
            Vector3 gord1 = new Vector3(1410f, 1728.5f, 310.3f);
            if (QH.API.Me.Distance2DTo(gord1) < 390)
            {
                Vector3 gord2 = new Vector3(1666.5f, 1743.6f, 298.6f);
                if ((QH.API.Me.Position.Z > 302.4) || ((QH.API.Me.Position.Z > 296.0) && (QH.API.Me.Distance2DTo(gord2) > 47.05)))
                {
                    QH.API.Print("It Appears that You are in Gordal Fortress! Navigating Out...");
                    // Guided pathing out of Gordal Fortress
                    Vector3 gord3 = new Vector3(1645.4f, 1767.4f, 312.5f);
                    Vector3 gord4 = new Vector3(1674.5f, 1729.1f, 291.4f);
                    while (!QH.API.MoveTo(gord3))
                    {
                        yield return 100;
                    }
                    QH.API.Print("Let's Avoid that Energy Barrier!");
                    while(!QH.API.CTM(gord4))
                    {
                        yield return 100;
                    }
                    QH.API.Print("Alright! Back on Track!!!");
                }
                yield break;
            }
            
            // Navigate out of Zangarra Properly.
            // LOCATION 2
            Vector3 zang1 = new Vector3(3187.2f, 788.7f, 77.7f);
            Vector3 zang2 = new Vector3(3035.2f,  954.0f,  105.5f);
            if (QH.API.Me.Distance2DTo(zang1) < 302 && QH.API.Me.Distance2DTo(zang2) > 75)
            {
                QH.API.Print("Let's First Get Out of Zangarra!");
                // These quick 'Z' height checks are for some tight turns the mesh sometimes handles poorly.
                if (QH.API.Me.Position.Z < 17)
                {
                    Vector3 zang3 = new Vector3(3316.2f, 950.4f, 17.4f);
                    while(!QH.API.MoveTo(zang3))
                    {
                        yield return 100;
                    }
                }
                if (QH.API.Me.Position.Z < 32)
                {
                    Vector3 zang4 = new Vector3(3286.9f, 1013.4f, 38.1f);
                    while(!QH.API.MoveTo(zang4))
                    {
                        yield return 100;
                    }
                }
                if (QH.API.Me.Position.Z < 44)
                {
                    Vector3 zang5 = new Vector3(3206.1f, 918.8f, 42.2f);
                    while(!QH.API.MoveTo(zang5))
                    {
                        yield return 100;
                    }
                }
                Vector3 zang6 = new Vector3(3198.8f, 836.9f, 83.2f);
                while(!QH.API.MoveTo(zang6))
                {
                    yield return 100;
                }
                // Brief pause for aggro
                yield return 2500;
                Vector3 zang7 = new Vector3(3199.5f, 843.6f, 84.3f);
                
                while (QH.API.Me.Distance2DTo(zang7) < 30)
                {
                    foreach (var unit in QH.API.GameObjects)
                    {
                        if (unit.EntryID == 230874)
                        {
                            while(!QH.API.MoveTo(unit.Position))
                            {
                                yield return 100;
                            }
                            unit.Interact();
                            yield return 10000;
                        }
                    }
                }
                QH.API.Print("Alright, Let's Continue!");
                yield break;
            }
            
            // Navigate out of Voljin's Pride Arsenal
            // LOCATION 3
            Vector3 arsenal = new Vector3(3217.1f, 1606.4f, 166.1f);
            if (QH.API.Me.Distance2DTo(arsenal) < 15)
            {
                while(!QH.API.CTM(3226.4f, 1600.0f, 166.0f))
                {
                    yield return 100;
                }
                while(!QH.API.CTM(3241.7f, 1589.6f, 163.2f))
                {
                    yield return 100;
                }
                yield break;
            }
            
            // Navigational pathing too Shattrath City Spire Flightpath.
            // LOCATION 4
            Vector3 shatt1 = new Vector3(2604.9f, 2797.0f, 242.1f);
            Vector3 shatt2 = new Vector3(2943.0f, 3351.9f, 53.0f);
            if (QH.API.Me.Level > 99 && QH.API.Me.Distance2DTo(shatt2) < 430 && QH.API.Me.Position.Z < 125)
            {
                QH.API.Print("Let's Move out of Shattrath. The elevator in the Sha'tari Market District Looks Good...");
                var check = new Fiber<int>(QH.TakeElevator(231934,7,2687.2f,3017.5f,69.5f,2682.8f,2995.0f,233.9f));
                while (check.Run())
                {
                    yield return 100;
                }
                QH.API.Print("Let's Get to that Flightpath and Get Out of Here!");
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
            QH.API.Print("Woah! Let's Get Out of Ashran Before Some Alliance Find You!");
            Vector3 ash = new Vector3(5090.1f, -3982.3f, 20.8f);
            while(!QH.API.MoveTo(ash))
            {
                yield return 100;
            }
            Vector3 ash2 = new Vector3(5141.9f, -3964.1f, 2.2f);
            while(!QH.API.MoveTo(ash2))
            {
                yield return 100;
            }
            yield break;
        }
        
        // Enter any additional pathing.
        yield break;
    }
	
	
}


// Create a method that returns BOTH vendor types
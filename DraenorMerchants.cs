/*  Merchant (Vendor Manager)
|   Author: @Sklug  aka TheGenomeWhisperer
|
|   To Be Used with "InsertContinentName.cs" and "Merchant.cs"
|	For use in collaboration with the Rebot API
|
|   Last Update Nov. 23rd, 2015 */


public class DraenorMerchants {
	
	public static Fiber<int> Fib; 
    public static bool IsSpecialPathingNeeded;
	
	// Default Constructor()
	public DraenorMerchants() {}
	
	// Method:		"getMerchantInfo(int,bool,int)
	// 				vendorType represented by numbers 1-3
	//				1 = Food && Drink
	//				2 = Repair
    //              3 = Generic Vendor
	public static List<object> getMerchantInfo(int zoneID, bool factionIsHorde, int vendorType) {
			// Default list in case of no result
			List<object> result = new List<object>();
			if (vendorType < 1 || vendorType > 3) {
				vendorType = 1;
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
    private static List<object> getWarspear(bool factionIsHorde, int vendorType) {
        List<object> locations = new List<object>();
		if (factionIsHorde) {
			// Food & Drink
			if (vendorType == 1) {
				List<object> list0 = new List<object>(){5307.4f, -3955.0f, 19.8f,86046,IsSpecialPathingNeeded};
				locations.AddRange(list0);
			}
			// Repair
			else if(vendorType == 2) {
				List<object> rep0 = new List<object>(){5257.8f, -3931.4f, 17.7f,89281,IsSpecialPathingNeeded};
				locations.AddRange(rep0);
			}
            // Generic Vendor (No repair, no food)
            else if(vendorType == 3) {
                
            }
            
            // Both Repair and Food
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

    private static List<object> getAshran(bool factionIsHorde, int vendorType) {
		List<object> locations = new List<object>();
		if (factionIsHorde) {
			// Food & Drink
			if (vendorType == 1) {
				// None Known
			}
			// Repair
			else if(vendorType == 2) {
				List<object> rep0 = new List<object>(){5027.2f, -4175.4f, 46.6f,82882,IsSpecialPathingNeeded};
				locations.AddRange(rep0);
			}
            // Generic Vendor (No repair, no food)
            else if(vendorType == 3) {
                
            }
            
            // Both Repair and Food
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

	
	private static List<object> getTanaan(bool factionIsHorde, int vendorType) {
        List<object> locations = new List<object>();
		if (factionIsHorde) {
			// Food & Drink
			if (vendorType == 1) {
				List<object> list0 = new List<object>(){};
				locations.AddRange(list0);
			}
			// Repair
			else if(vendorType == 2) {
				List<object> rep0 = new List<object>(){4562.1f, 346.8f, 220.9f, 93178,IsSpecialPathingNeeded};
                locations.AddRange(rep0);
			}
            // Generic Vendor (No repair, no food)
            else if(vendorType == 3) {
                List<object> vend0 = new List<object>(){3309.9f, -1176.7f, 57.9f, 92805,IsSpecialPathingNeeded};
                locations.AddRange(vend0);
            }
            
            // Both Repair and Food
		}
        else
		{
			// Alliance locations
		}
		// Add Neutral locations of BOTH Repair and Food
		List<object> neut0 = new List<object>(){3518.7f, -776.9f, 39.3f, 92069,IsSpecialPathingNeeded};
        locations.AddRange(neut0);
		//
        return locations; 
    }

    private static List<object> getSMV(bool factionIsHorde, int vendorType) {
        List<object> locations = new List<object>();
		if (factionIsHorde) {
			// Food & Drink
			if (vendorType == 1) {
				// None Known
			}
			// Repair
			else if(vendorType == 2) {
				List<object> rep0 = new List<object>(){-1494.6f, 969.0f, 7.4f, 76198,IsSpecialPathingNeeded};
                locations.AddRange(rep0);
                List<object> rep1 = new List<object>(){1570.3f, -4.7f, 55.8f, 84907,IsSpecialPathingNeeded};
                locations.AddRange(rep1);
                List<object> rep3 = new List<object>(){1192.3f, -1800.8f, 37.3f, 84907,IsSpecialPathingNeeded};
                locations.AddRange(rep3);
                
                if (Merchant.API.Me.Level > 99) {
                    List<object> rep2 = new List<object>(){-793.3f, -671.3f, 106.8f, 81614,IsSpecialPathingNeeded};
                    locations.AddRange(rep2);
                }
			}
            // Generic Vendor (No repair, no food)
            else if(vendorType == 3) {
                
            }
            
            // Both Repair and Food
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

    private static List<object> getNagrand(bool factionIsHorde, int vendorType)  {
        List<object> locations = new List<object>();
        if (factionIsHorde) {
			// Food & Drink
			if (vendorType == 1) {
				List<object> list0 = new List<object>(){3007.7f, 4780.8f, 128.2f,79199,IsSpecialPathingNeeded};
				locations.AddRange(list0);
                List<object> list1 = new List<object>(){3218.1f, 4594.8f, 142.8f, 82345,IsSpecialPathingNeeded};
				locations.AddRange(list1);
                List<object> list2 = new List<object>(){3128.2f, 6539.9f, 13.1f, 82341,IsSpecialPathingNeeded};
				locations.AddRange(list2);
                
                if (Merchant.API.Me.Level > 99) {
                    List<object> list3 = new List<object>(){4462.6f, 6144.4f, 107.5f, 85067,IsSpecialPathingNeeded};
                    locations.AddRange(list3);
                }
			}
			// Repair
			else if(vendorType == 2) {
				List<object> rep0 = new List<object>(){3099.9f, 4836.1f, 127.9f, 79310,IsSpecialPathingNeeded};
                locations.AddRange(rep0);
                List<object> rep1 = new List<object>(){3218.1f, 4594.8f, 142.8f, 82343,IsSpecialPathingNeeded};
                locations.AddRange(rep1);
                
                if (Merchant.API.Me.Level > 99) {
                    List<object> rep3 = new List<object>(){4424.3f, 6177.2f, 107.4f, 84902,IsSpecialPathingNeeded};
                    locations.AddRange(rep3);
                }
                
                List<object> rep4 = new List<object>(){3343.3f, 6465.7f, 17.3f, 87396,IsSpecialPathingNeeded};
                locations.AddRange(rep4);
                List<object> rep5 = new List<object>(){3421.9f, 7359.3f, 11.1f, 88137,IsSpecialPathingNeeded};
                locations.AddRange(rep5);
                
			}
            // Generic Vendor (No repair, no food)
            else if(vendorType == 3) {
                
            }
            
            // Both Repair and Food
            List<object> both0 = new List<object>(){3118.0f, 6542.6f, 12.9f, 82344, IsSpecialPathingNeeded};
            locations.AddRange(both0);
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

    private static List<object> getSpires(bool factionIsHorde, int vendorType) {
        List<object> locations = new List<object>();
        if (factionIsHorde) {
			// Food & Drink
			if (vendorType == 1) {
				List<object> list0 = new List<object>(){-396.6f, 1869.1f, 51.4f, 86386,IsSpecialPathingNeeded};
				locations.AddRange(list0);
                List<object> list1 = new List<object>(){-332.8f, 919.6f, 75.0f, 80777,IsSpecialPathingNeeded};
				locations.AddRange(list1);
                List<object> list2 = new List<object>(){-1557.5f, 945.2f, 7.8f, 82516,IsSpecialPathingNeeded};
				locations.AddRange(list2);
                List<object> list3 = new List<object>(){-668.6f, 2416.1f, 34.3f, 82432,IsSpecialPathingNeeded};
				locations.AddRange(list3);
			}
			// Repair
			else if(vendorType == 2) {
                // Axefall Repair Vendors.
				if (Merchant.API.IsQuestCompleted(35277)) {
                    List<object> rep0 = new List<object>(){-377.2f, 2241.9f, 26.1f, 82613,IsSpecialPathingNeeded};
                    locations.AddRange(rep0);
                    List<object> rep1 = new List<object>(){-353.8f, 2202.4f, 27.7f, 82622,IsSpecialPathingNeeded};
                    locations.AddRange(rep1);
                }
                
                List<object> rep2 = new List<object>(){-452.2f, 1856.9f, 41.2f, 87775,IsSpecialPathingNeeded};
                locations.AddRange(rep2);
                List<object> rep3 = new List<object>(){-1494.6f, 969.0f, 7.4f, 82183,IsSpecialPathingNeeded};
                locations.AddRange(rep3);
                List<object> rep4 = new List<object>(){-643.4f, 1538.2f, 36.5f, 88045,IsSpecialPathingNeeded};
                locations.AddRange(rep4);
                List<object> rep5 = new List<object>(){-2354.5f, 1138.1f, 23.1f, 82182,IsSpecialPathingNeeded};
                locations.AddRange(rep5);
			}
            // Generic Vendor (No repair, no food)
            else if(vendorType == 3) {
                List<object> generic0 = new List<object>(){-670.6f, 2414.9f, 34.3f, 82156,IsSpecialPathingNeeded};
                locations.AddRange(generic0);
            }
            
            // Both Repair and Food
            

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

    private static List<object> getTalador(bool factionIsHorde, int vendorType) {
        List<object> locations = new List<object>();
        // Zangarra coordinates
        Vector3 zang1 = new Vector3(3187.2f, 788.7f, 77.7f);
        Vector3 zang2 = new Vector3(3035.2f,  954.0f,  105.5f);
        Vector3 shatt1 = new Vector3(2604.9f, 2797.0f, 242.1f);
        Vector3 shatt2 = new Vector3(2943.0f, 3351.9f, 53.0f);
        Vector3 shatt3 = new Vector3(2575.1f, 2825.5f, 245.2f);
        if (factionIsHorde) {
			// Food & Drink
			if (vendorType == 1) {
				List<object> list0 = new List<object>(){3246.7f, 1558.4f, 162.4f, 81359, IsSpecialPathingNeeded};
				locations.AddRange(list0);
                
                // If player is in Zangarra will these only be added.
                if (Merchant.API.Me.Distance2DTo(zang1) < 302 && Merchant.API.Me.Distance2DTo(zang2) > 75) {
                    List<object> list1 = new List<object>(){3173.8f, 764.7f, 81.3f, 80931, IsSpecialPathingNeeded};
				    locations.AddRange(list1);
                }
                
                // Ensuring only to add the NPCs that are at the Spire of Light since special pathing.
                if ((Merchant.API.Me.Level > 99 && Merchant.API.Me.Distance2DTo(shatt2) < 430 && Merchant.API.Me.Position.Z < 125) || (Merchant.API.Me.Distance2DTo(shatt3) < 200 && Merchant.API.Me.Position.Z > 225)) {
                    List<object> list2 = new List<object>(){2605.7f, 2799.3f, 242.2f, 82636, IsSpecialPathingNeeded};
				    locations.AddRange(list2);
                }
                
                List<object> list3 = new List<object>(){1215.5f, 3041.8f, 133.7f, 81742, IsSpecialPathingNeeded};
				locations.AddRange(list3);
			}
			// Repair
			else if(vendorType == 2) {
				List<object> rep0 = new List<object>(){4008.1f, 2128.6f, 117.0f, 77029,IsSpecialPathingNeeded};
                locations.AddRange(rep0);
                List<object> rep1 = new List<object>(){3230.2f, 1563.1f, 161.6f, 81359,IsSpecialPathingNeeded};
                locations.AddRange(rep1);
                
                // Only add if in Zangarra
                if (Merchant.API.Me.Distance2DTo(zang1) < 302 && Merchant.API.Me.Distance2DTo(zang2) > 75) {
                    List<object> rep2 = new List<object>(){3198.6f, 822.6f, 81.3f, 80930, IsSpecialPathingNeeded};
				    locations.AddRange(rep2);
                }
                
                // Ensuring only to add the NPCs that are at the Spire of Light since challenging pathing.
                if ((Merchant.API.Me.Level > 99 && Merchant.API.Me.Distance2DTo(shatt2) < 430 && Merchant.API.Me.Position.Z < 125) || (Merchant.API.Me.Distance2DTo(shatt3) < 200 && Merchant.API.Me.Position.Z > 225)) {
                    List<object> rep3 = new List<object>(){2606.6f, 2812.3f, 242.5f, 82635, IsSpecialPathingNeeded};
				    locations.AddRange(rep3);
                    List<object> rep4 = new List<object>(){2659.8f, 2795.4f, 246.2f, 86317, IsSpecialPathingNeeded};
				    locations.AddRange(rep4);
                }
                
                List<object> rep5 = new List<object>(){2801.6f, 2523.0f, 121.1f, 81093,IsSpecialPathingNeeded};
                locations.AddRange(rep5);
                List<object> rep6 = new List<object>(){1754.0f, 2552.6f, 133.7f, 81095,IsSpecialPathingNeeded};
                locations.AddRange(rep6);
                List<object> rep7 = new List<object>(){1393.0f, 3289.5f, 133.7f, 81886,IsSpecialPathingNeeded};
                locations.AddRange(rep7);
                List<object> rep8 = new List<object>(){1541.2f, 2220.5f, 139.0f, 81951,IsSpecialPathingNeeded};
                locations.AddRange(rep8);
			}
            // Generic Vendor (No repair, no food)
            else if(vendorType == 3) {
                // No Just plain generic vendors found
            }
            
            // Both Repair and Food
            
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

    private static List<object> getGorgrond(bool factionIsHorde, int vendorType)  {
        List<object> locations = new List<object>();
        if (factionIsHorde) {
			// Food & Drink
			if (vendorType == 1) {
                if (Merchant.API.Me.Level > 99) {
                    List<object> list0 = new List<object>(){8373.6f, 84.4f, 79.6f, 86994,IsSpecialPathingNeeded};
                    locations.AddRange(list0);
                }
			}
			// Repair
			else if(vendorType == 2) {
				if (Merchant.API.IsQuestCompleted(35151)) {
                    List<object> rep0 = new List<object>(){5779.6f, 1287.2f, 107.5f,82732,IsSpecialPathingNeeded};
				    locations.AddRange(rep0);
                }
                if (Merchant.API.Me.Level > 99) {
                    List<object> rep1 = new List<object>(){8395.8f, 80.2f, 80.0f, 86998,IsSpecialPathingNeeded};
                    locations.AddRange(rep1);
                }
			}
            // Generic Vendor (No repair, no food)
            else if(vendorType == 3) {
                // Unable to identify
            }
            
            // Both Repair and Food
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
            // Generic Vendor (No repair, no food)
            else if(vendorType == 3) {
                // Unable to find generic vendors (probably some in garrison but not needed to be included.
            }
            
            // Both Repair and Food
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
        int level = Merchant.API.Me.Level;
        List<int> allFood = new List<int>();
        
        // Adding all Mana Conjured items to the list (based on % so any level is Good)
        List<int> mageConjured = new List<int>() {65500,65515,65516,65517,43518,43523,65499};
        allFood.AddRange(mageConjured);
        
        // Draenor Known Food IDs       
        // Draenor 90+ food
        if (level > 89 && level < 101) {
            List<int> draenor = new List<int> {115351,115352,115353,115354,115355,117457,117454,118269,117471,130192,117470,118272,118268,111544,118416,128219,128498,118051,118050,117472,118270,117469,117474,86994,111456};
            allFood.AddRange(draenor);
            
            // For Tanaan jungle Draenor intro only... and before Garrison is established to buy food.
            if (!Merchant.API.IsQuestCompleted(34378)) {
                allFood.Add(112449);
            }
            
            // Higher level Crafted Food
            if (level > 90) {
                List<int> draenor2 = new List<int>() {111449,111447,111434,122346,111442,111433,122343,111452,111431,111436,111446,122348,122345,111453,111441,122347,111454,111438,111439,111450,111445,122344,111444,111437,118428,118269};
                allFood.AddRange(draenor2);
            }
        }
        
        
        
        // Proving grounds only food/drink item: 120293
        return allFood;
    }
    
    public static List<int> getWater() {
        int level = Merchant.API.Me.Level;
        List<int> allDrinks = new List<int>();
        
        // Adding all Mana Conjured items to the list (based on % so any level is Good)
        List<int> mageConjured = new List<int>() {65500,65515,65516,65517,43518,43523,65499};
        allDrinks.AddRange(mageConjured);
        
        // Draenor Known Drink IDs
        // Draenor 90+ drinks
        if (level > 89 && level < 101) {
            List<int> draenor = new List<int>() {117452,117475,128385,118269,130192,118272,118268,111544,118416,111455,118270,118269};
            allDrinks.AddRange(draenor);
            
            // For Tanaan jungle Draenor intro only... and before Garrison is established to buy food.
            if (!Merchant.API.IsQuestCompleted(34378)) {
                allDrinks.Add(112449);
            }
            
            // Higher level Crafted drinks
            if (Merchant.API.Me.Level > 90) {
                List<int> betterDrinks = new List<int>() {111449,111447,111434,122346,111442,111433,122343,111452,111431,111436,111446,122348,122345,111453,111441,122347,111454,111438,111439,111450,111445,122344,111444,111437,118428};
                allDrinks.AddRange(betterDrinks);
            }
        }
            
        
        
        return allDrinks;
    }
	
	
    public static IEnumerable<int> doSpecialPathing() {
        int zoneID = Merchant.API.Me.ZoneId; 
        
        if (Merchant.API.Me.IsHorde) {
            
                // FROSTFIRE RIDGE ZONE SPECIAL PATHING!
            if (zoneID == 6720 || zoneID == 6868 || zoneID == 6745 || zoneID == 6849 || zoneID == 6861 || zoneID == 6864 || zoneID == 6848 || zoneID == 6875 || zoneID == 6939 || zoneID == 7005 || zoneID == 7209 || zoneID == 7004 || zoneID == 7327 || zoneID == 7328 || zoneID == 7329) {
                
                Vector3 location = new Vector3(5419.3f, 4900.0f, 55.3f);
                Vector3 location2 = new Vector3(5312.3f, 5009.5f, 5.0f);
                Vector3 location3 = new Vector3(5411.1f, 5012.9f, 3f);
                
                // At the docks
                if (Merchant.API.Me.Distance2DTo(location2) < 90 || Merchant.API.Me.Distance2DTo(location3) < 40 && !Merchant.API.IsInGarrison) {
                    var check = new Fiber<int>(HearthToGarrison());
                    while (check.Run()) {
                        yield return 100;
                    }
                    yield return 1000;
                    if (!Merchant.API.IsInGarrison) {
                        Merchant.API.Print("WARNING!!! Serious mesh issues at the docks. Please move up to the garrison manually and restart.");
                    }
                }
                
                // Moving up from the docks - compensating for mesh issues.
                if (Merchant.API.Me.Distance2DTo(location) < 200 && !Merchant.API.IsInGarrison) {
                    Merchant.API.Print("Heading from the Shipyard Back to Your Garrison!");
                    
                    while (!Merchant.API.MoveTo(5475.7f, 4878.2f, 76.6f)) {
                        yield return 100;
                    }
                    
                    while (!Merchant.API.MoveTo(5473.5f, 4808.1f, 113.2f)) {
                        yield return 100;
                    }
                    
                    while (!Merchant.API.MoveTo(5464.6f, 4773.1f, 124.3f)) {
                        yield return 100;
                    }
                    
                    while (!Merchant.API.MoveTo(5545.9f, 4723.0f, 151.3f)) {
                        yield return 100;
                    }
                    // To Pause and let mesh load.
                    yield return 2500;
                    
                    while (!Merchant.API.CTM(5564.7f, 4653.8f, 148.7f)) {
                        yield return 100;
                    }
                }
                
                // If already in Garrison
                if (Merchant.API.IsInGarrison)
                {
                    var check = new Fiber<int>(GTownHallExit());
                    while (check.Run()){
                        yield return 100;
                    }
                }
            }
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
                
                // Navigate to vendors if in Zangarra Properly.
                // LOCATION 2
                Vector3 zang1 = new Vector3(3187.2f, 788.7f, 77.7f);
                Vector3 zang2 = new Vector3(3035.2f,  954.0f,  105.5f);
                if (Merchant.API.Me.Distance2DTo(zang1) < 302 && Merchant.API.Me.Distance2DTo(zang2) > 75) {
                    // These quick 'Z' height checks are for some tight turns the mesh sometimes handles poorly.
                    if (Merchant.API.Me.Position.Z < 17) {
                        Vector3 zang3 = new Vector3(3316.2f, 950.4f, 17.4f);
                        while(!Merchant.API.MoveTo(zang3)) {
                            yield return 100;
                        }
                    }
                    if (Merchant.API.Me.Position.Z < 32) {
                        Vector3 zang4 = new Vector3(3286.9f, 1013.4f, 38.1f);
                        while(!Merchant.API.MoveTo(zang4)) {
                            yield return 100;
                        }
                    }
                    if (Merchant.API.Me.Position.Z < 44) {
                        Vector3 zang5 = new Vector3(3206.1f, 918.8f, 42.2f);
                        while(!Merchant.API.MoveTo(zang5)) {
                            yield return 100;
                        }
                    }
                    Vector3 zang6 = new Vector3(3184.1f, 818.7f, 80.2f);
                    while(!Merchant.API.MoveTo(zang6)) {
                        yield return 100;
                    }
                    yield break;
                }
                
                // Navigate out of Voljin's Pride Arsenal
                // LOCATION 3
                Vector3 arsenal = new Vector3(3217.1f, 1606.4f, 166.1f);
                if (Merchant.API.Me.Distance2DTo(arsenal) < 15) {
                    while(!Merchant.API.CTM(3226.4f, 1600.0f, 166.0f)) {
                        yield return 100;
                    }
                    while(!Merchant.API.CTM(3241.7f, 1589.6f, 163.2f)) {
                        yield return 100;
                    }
                    yield break;
                }
                
                // Navigational pathing to Shattrath City Spire Flightpath.
                // LOCATION 4
                Vector3 shatt1 = new Vector3(2604.9f, 2797.0f, 242.1f);
                Vector3 shatt2 = new Vector3(2943.0f, 3351.9f, 53.0f);
                if (Merchant.API.Me.Level > 99 && Merchant.API.Me.Distance2DTo(shatt2) < 430 && Merchant.API.Me.Position.Z < 125) {
                    Merchant.API.Print("Let's Move out of Shattrath. The elevator in the Sha'tari Market District Looks Good...");
                    var check = new Fiber<int>(TakeElevator(231934,7,2687.2f,3017.5f,69.5f,2682.8f,2995.0f,233.9f));
                    while (check.Run()) {
                        yield return 100;
                    }
                    Merchant.API.Print("Let's Get to that Vendor and Get Out of Here!");
                    yield break;
                }
                yield break;     
            }
            // End Talador
            
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
    public static int GetGarrisonLevel() {
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
    public static IEnumerable<int> TakeElevator(int ElevatorID, int elevatorTravelTime, float startX, float startY, float startZ, float x, float y, float z)  {
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
                
                // Checking initial position of the elevator.
                position = Math.Sqrt(Merchant.API.Me.DistanceSquaredTo(unit));
                yield return 100;
                position2 = Math.Sqrt(Merchant.API.Me.DistanceSquaredTo(unit));
                yield return 100;
                
                // Some Added Redundancy to not attempt to take the elevator if it just arrived
                // Lest you try to hop on right before it moves away.
                while (Math.Sqrt(Merchant.API.Me.DistanceSquaredTo(unit)) <= 20.0) {
                    yield return 100;
                }
                
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
    
    // Method:          "GTownHallExit()"
    // What it does:    Navigates out of a lvl 2 or 3 Garrison Town Hall (Horde)
    // Purpose:         Rebot has serious Mesh issues when starting a script within a Garrison
    //                  But, even worse, starting within a town hall.  This solves this issue
    //                  by using Click-To-Move actions to navigate out of the town hall successfully.
    //                  This is best used in the "initialization" stage of a script, or to be implemented
    //                  immediately after hearthing to the Garrison.
    public static IEnumerable<int> GTownHallExit()
    {
        int tier = Merchant.API.ExecuteLua<int>("local level = C_Garrison.GetGarrisonInfo(); return level;");
        string zone = Merchant.API.ExecuteLua<string>("local zone = GetMinimapZoneText(); return zone;");
        Vector3 location = new Vector3(5559.2f, 4604.8f, 141.7f);
        Vector3 location2 = new Vector3(5562.576f, 4601.484f, 141.7169f);
        Vector3 location3 = new Vector3(5576.729f, 4584.367f, 141.0846f);
        Vector3 location4 = new Vector3(5591.181f, 4569.721f, 136.2159f);
        
        if (Merchant.API.Me.Distance2DTo(location) < 25 && Merchant.API.IsInGarrison && (tier == 2 || tier == 3))
        {
            // If I do not disable Flightmaster discovery, then it tries to run to flightmaster BEFORE executing CTM actions
            // which with the lack of a mesh, often results in the player just running helplessly into the wall with mesh errors spamming.
            Merchant.API.GlobalBotSettings.FlightMasterDiscoverRange = 0.0f;
            while(Merchant.API.Me.Distance2DTo(location2) > 5)
            {
                Merchant.API.CTM(location2);
                yield return 100;
            }
            while(Merchant.API.Me.Distance2DTo(location3) > 5)
            {
                Merchant.API.CTM(location3);
                yield return 100;
            }
            while(Merchant.API.Me.Distance2DTo(location4) > 5)
            {
                Merchant.API.CTM(location4);
                yield return 100;
            }
            Merchant.API.GlobalBotSettings.FlightMasterDiscoverRange = 75.0f;
        }
        yield break;
    }
    
    // Method:          "HearthToGarrison()"
    // What it Does:    Exactly as described, uses the Garrison hearthstone.
    // Purpose:         Extraordinarily useful for speed.  If player needs to pickup a quest that starts in the garrison and they are not there
    //                  by adding this option it will hearth them back, likewise with turning in something.
    //                  This method is invaluable and used incredibly frequently to maximize the efficiency of player scripts and believable player
    //                  behavior.
    public static IEnumerable<int> HearthToGarrison()
    {
        // Error check to avoid use if flying
        if (Merchant.API.Me.IsOnTaxi)
        {
            // Waits to use until OFF the IsOnTaxi.
            Merchant.API.Print("Player is Currently on a Taxi, Please Be Patient And Enjoy the Scenery!");
            int count = 0;
            while (Merchant.API.Me.IsOnTaxi)
            {
                yield return 1000;
                count++;
            }
            Merchant.API.Print("Player Exited the Flightpath after " + count + " seconds!");
            yield return 1000;
        }
        
        if (!Merchant.API.IsInGarrison) 
        {
            // Verifying Garrison hearthstone is not on Cooldown.
            if (Merchant.API.ItemCooldown(110560) == 0) 
            {
                if (Merchant.API.ExecuteLua<bool>("local name = GetMerchantItemInfo(1); if name ~= nil then return true else return false end"))
                {
                    Merchant.API.Print("Player is Interacting With a Vendor. Closing Window Before Attempting to Hearth, lest the Bot Will Attempt to Sell G-Hearthstone!");
                    Merchant.API.ExecuteLua("CloseMerchant()");
                    yield return 1000;
                }
                
                // This is a check to verify player has moved, lest it will re-attempt to hearth.
                Vector3 startPos = Merchant.API.Me.Position;
                while (Merchant.API.Me.Distance2DTo(startPos) < 50)
                {
                    Merchant.API.Print("Hearthing to Garrison");
                    Merchant.API.UseItem(110560);
                    yield return 1000;
                    // This keeps the player from attempting the next action until the Garrison hearthstone is successfully used
                    while(Merchant.API.Me.IsCasting) 
                    {
                        yield return 100;
                    }
                    yield return 1000;
                    if (Merchant.API.Me.Distance2DTo(startPos) >= 50)
                    {
				        break;
                    }
                    else
                    {
                        Merchant.API.Print("Player Failed to Hearth. Trying Again...");
                    }
                }
                // Waiting for loading screen!
                while (!Merchant.API.IsInGarrison)
                {
                    yield return 1000;
                }
                // Sometimes mesh errors occur by trying to CTM because it tries as soon as loading screen goes away but maybe some assets
                // are not fully loaded in the world.  This gives a slight delay to ensure no error.  Really depends on player PC and connection.
                Merchant.API.Print("One Moment, Giving Mesh a Chance to Catchup!");
                yield return 5000;
            }
            else 
            {
                // Assumedly, in instances like this, a 2ndary logic route is given as backup, either the mesh or by Flightpath.
                Merchant.API.Print("Player Wanted to Hearth to Garrison, but it is on Cooldown...");
                // Apply Flight Logic soon...
                yield break;
            }
        }
        yield break;
    }
	
}


// Create a method that returns ALL vendor types
// All previous zones before nagrand need generic vendors added
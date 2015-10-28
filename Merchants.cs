/*  VendorManager
|   Author: @Sklug  aka TheGenomeWhisperer
|
|   To Be Used with "InsertContinentName.cs" and "Localization.cs" class
|   For use in collaboration with the Rebot API 
|
|   Last Update 26th Oct, 2015 */


public class Merchant
{
    public static Fiber<int> Fib;
    
	public static List<int> FoodIDs;
	public static Lust<int> DrinkIDs;
    // Default Constructor
    public Merchant() {}
	
	// Method:		"BuyFood()"
	public static IEnumberable<int> BuyFood(List<int> foodIDs) {
		string itemID;
		string temp;
		int ID;
		for (int i = 1; i < QH.API.ExecuteLua<int>("return GetMerchantNumItems()"); i++)
		{
			itemID = QH.API.ExecuteLua<string>("return GetMerchantItemLink(" + i + ");");
			temp = itemID.Substring(itemID.IndexOf(':') + 1);
			itemID = itemID.Substring(itemID.IndexOf(':') + 1, temp.IndexOf(':'));
			ID = int.Parse(itemID);
			foreach (int unit in foodIDs) {
				if (unit == ID) {
					int totalToBuy = MaxFoodToBuy();
					int BuyTwenty = totalToBuy / 20;
					int remainder = totalToBuy % 20;
					// j = Multiples of 20
					for (int j = 0; j < BuyTwenty; j++) {
						QH.API.ExecuteLua("BuyMerchantItem(" + i + ", 20)");
						yield return 500;
					}
					QH.API.ExecuteLua("BuyMerchantItem(" + i + "," + remainder + ")");
					yield return 500;
				}
			}
		}
	}
	
	// Method:		"BuyDrink()"
	public static IEnumberable<int> BuyDrink(List<int> drinkIDs) {
		string itemID;
		string temp;
		int ID;
		for (int i = 1; i < QH.API.ExecuteLua<int>("return GetMerchantNumItems()"); i++)
		{
			itemID = QH.API.ExecuteLua<string>("return GetMerchantItemLink(" + i + ");");
			temp = itemID.Substring(itemID.IndexOf(':') + 1);
			itemID = itemID.Substring(itemID.IndexOf(':') + 1, temp.IndexOf(':'));
			ID = int.Parse(itemID);
			foreach (int unit in drinkIDs) {
				if (unit == ID) {
					int totalToBuy = MaxDrinkToBuy();
					int BuyTwenty = totalToBuy / 20;
					int remainder = totalToBuy % 20;
					// j = Multiples of 20
					for (int j = 0; j < BuyTwenty; j++) {
						QH.API.ExecuteLua("BuyMerchantItem(" + i + ", 20)");
						yield return 500;
					}
					QH.API.ExecuteLua("BuyMerchantItem(" + i + "," + remainder + ")");
					yield return 500;
				}
			}
		}
	}
	
	public static List<object> GetClosestMerchant(int TypeofMerchant) {
		List<object> vendor = new List<object>();
        List<object> result = new List<object>();
		
		// Obtaining the List with the NPC info and location.
		vendor = getVendorInfo(TypeofMerchant);
		
		if (vendor.Count == 0) {
			float closestDistance;
			float tempDistance;
			int npcID;
			bool IsSpecialPathingNeeded;
			Vector3 closestVector3;
			Vector3 position;
		
			// Setting the initial Merchant to the closest distance
			closestVector3 = new Vector3((float)vendor[0], (float)vendor[1], (float)vendor[2]);
			closestDistance = QH.API.Me.Distance2DTo(closestVector3);
			npcID = (int)vendor[3];
			IsSpecialPathingNeeded = (bool)vendor[4];
			
	
			// Filtering for Closest Merchant now.
			for (int i = 0; i < vendor.Count - 4; i = i + 5) {
				// Setting first values
				position = new Vector3((float)vendor[i], (float)vendor[i + 1], (float)vendor[i + 2]);
				tempDistance = QH.API.Me.Distance2DTo(position);
				// Changing values if new ones are closer
				if (tempDistance < closestDistance) {
					closestDistance = tempDistance;
					closestVector3 = position;
					closestZone = (string)vendor[i];
					npcID = (int)vendor[i+3];
					IsSpecialPathingNeeded = (bool)vendor[i+4];
				}
			}
			 // Creating list with the Vector3 position of closest Merchant, and food list and water list on the end.
            List<object> final = new List<object>(){closestVector3,closestDistance,npcID,IsSpecialPathingNeeded};
            result.AddRange(final);
		}
		return result;		
	}
	
	public static List<object> getVendorInfo(int TypeofMerchant) {
		List<object> vendor = new List<object>();
        int continentID = QH.API.Me.ContinentID;
        int zoneID = QH.API.Me.ZoneId;
        bool factionIsHorde = QH.API.Me.IsHorde;
        
        // Draenor Continent
        if (continentID == 1116)
        {
            vendor = DraenorMerchants.getMerchantInfo(zoneID, factionIsHorde, TypeofMerchant);
        }
        
        // All Continents Eventually to be Added
        return vendor;
	}
	
	
	public static void InteractWithMerchant() {
		if (QH.API.Me.Focus != null) {
			while(!IsVendorOpen()) {
				QH.API.Me.Focus.Interact();
				yield return 1500;
				if (!IsVendorOpen()) {
					MerchantGossip();
					yield return 2000;
				}
			}
		}
	}
	
	// Method:		"IsVendorOpen()"
	public static bool IsVendorOpen() {
		return QH.API.ExecuteLua<bool>("local name = GetMerchantItemInfo(1); if name ~= nil then return true else return false end;");
	}
	
	public static bool IsFoodOrDrinkNeeded() {
		// 2D array structure, First value of each List of Lists will be food ID and amount owned
		// If player owns none, value defaults to zero.  This creates a check on what a player owns.
		List<List<int>> owned = new List<List<int>>();
		
		if (QH.API.Me.ContinentID == 1116) {
			FoodIDs = DraenorMerchants.getFood();
			DrinkIDs = DraenorMerchants.getWater();
		}
		
		// else if() To be added for other continents.
		// Now, finding all items in my bags that much these known items to use, and counting how many in possession.
		foreach (int item in Inventory.items) {
			foreach (int foodItemID in FoodIDs) {
				if (foodItemID == item.ItemID) {
					List<int> list = new List<int>() {item.ID, item.StackCount};
					owned.Add(list);
					break;
				}
			}
		}
		
		QH.API.Print("Player Needs to Purchase Some Resting Refreshments!")
		return true;
	}
	
	public static bool IsRepairNeeded() {
		QH.API.Print("Player is in Need of Repair.  Heading to nearest Vendor!");
		return false;
	}
	
	public static int MaxDrinkToBuy(int max) {
		int total = 0;
		return total;
	}
	
	public static int MaxFoodToBuy(int max) {
		for (int i = 0; i < GlobalBotSettings.FoodItemIds.Count; i++) {
			GlobalBotSettings.FoodItemIds.RemoveAt(0);
		}
		int total = 0;
		return total;
	}
	
	public static void MerchantGossip() {
		// Initializing Function
        string title = "title0";
        string luaCall = ("local title0,_ = GetGossipOptions(); if title0 ~= nil then return title0 else return \"nil\" end");
        string result = API.ExecuteLua<string>(luaCall);
        // Now Ready to Iterate through All Gossip Options!
        // The reason "1" is used instead of the conventional "0" is because Gossip options start at 1.
        int i = 1;
        string num = "";
        while (result != null) {
            if (result.Equals("Let me browse your goods.") || result.Equals("Deja que eche un vistazo a tus mercancías.") || result.Equals("Deixe-me dar uma olhada nas suas mercadorias.") || result.Equals("Ich möchte ein wenig in Euren Waren stöbern.") || result.Equals("Deja que eche un vistazo a tus mercancías.") || result.Equals("Permettez-moi de jeter un œil à vos biens.") || result.Equals("Fammi vedere la tua merce.") || result.Equals("Позвольте взглянуть на ваши товары.") || result.Equals("물건을 살펴보고 싶습니다.") || result.Equals(" 讓我看看你出售的貨物。") || result.Equals("让我看看你出售的货物。")) {
                API.ExecuteLua("SelectGossipOption(" + i + ");");
                break;
            }
            else {
                // This builds the new string to be added into an Lua API call.
                num = i.ToString();
                title = (title.Substring(0,title.Length-1) + num);
                luaCall = ("local " + title + ",_," + luaCall.Substring(6,luaCall.Length-6));
                result = API.ExecuteLua<string>(luaCall);
                i++;
            }
        }
	}
	
	public IEnumberable<int> MoveToMerchant(List<object> merchantInfo) {
		// If Empty Result, Zone not known.
		if (merchantInfo.Count == 0) {
			QH.API.Print("Unfortunately the Zone Your Are in Does Not Have the Food and Drink NPCs Mapped yet!");
			QH.API.Print("It Would Be Amazing if You Could Report Back on the Forums This Issue! Thank you!");
			yield break;
		}
		
		// Casting all the Object to Types
        Vector3 destination = (Vector3) result[0];
        float distance = (float) result[1];
        distance = (int)Math.Ceiling(distance);
        int npcID = (int) result[2];
        bool IsSpecialPathingNeeded = (bool) result[3];
		
        string yards = "Yards";
        // String from plural to non. QoL thing only...
        if (distance == 1) {
            yards = "Yard";
        }
        QH.API.Print("Traveling Roughly " + distance + " " + yards + " to Get There...");
		
		// This is where to add special pathing considerations.
        if (IsSpecialPathingNeeded) {
            if (QH.API.Me.ContinentID == 1116) {
                var check = new Fiber<int>(DraenorMerchants.doSpecialPathing());
                while (check.Run()) {
                    yield return 100;
                }
            }
            
            // Add connections to other Classes for other continents here...
            //  else if (API.Me.ContinentID == 1) {
            //	var check = new Fiber<int>(Kalimdor.doSpecialPathing());
			//	while(check.Run()) 
			//		yield retun 100;
            //  }
        }
		
		// Ok, time to move!
        while (!QH.API.MoveTo(destination)) {
            yield return 100;
        }
        
        // Targeting the Merchant!
        foreach (var unit in QH.API.Units) {
            if (unit.EntryID == npcID) {
                QH.API.Me.SetFocus(unit);
                QH.API.Me.SetTarget(unit);
                break;
            }
        }
        
        // Edging closer to the Merchant!
        while(QH.API.Me.Focus != null && !QH.API.MoveTo(QH.API.Me.Focus.Position)) {
            yield return 100;
        }
	}
	
	public static void Repair() {
		// To be filled later
	}
	
	public static IEnumberable<int> RepairCheck() {
		yield break;
	}
	
	public static IEnumberable<int> RestingCheck(){
		List<object> result = new List<object>();
		if (IsFoodOrDrinkNeeded()) {
			// Identifying Merchant and moving to it.
			var check = new Fiber<int>(MoveToMerchant(GetClosestMerchant(1)));
			while (check.Run()) {
				yield return 100;
			}
			
			// Interacting with Merchant
			InteractWithMerchant();
			// Buy Water if Needed
			if (QH.Me.Class.Equals("Paladin") || QH.Me.Class.Equals("Priest") || QH.Me.Class.Equals("Shaman") || QH.Me.Class.Equals("Mage") || QH.Me.Class.Equals("Warlock") || QH.Me.Class.Equals("Druid") || QH.Me.Class.Equals("Monk")) {
            	var check = new Fiber<int>(BuyDrink(DrinkIDs));
				while (check.Run()) {
					yield return 100;
				}
        	}
			// Buying food
			var check2 = new Fiber<int>(BuyFood(FoodIDs));
			while (check2.Run()) {
				yield return 100;
			}
			
			
			
			
		}
		yield break;
	}
	
	
}
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
	public static List<int> DrinkIDs;
	public static List<int[]> InventoryFood;
	public static List<int[]> InventoryWater;
	public static int FoodIDToBuy;
	public static int DrinkIDToBuy;
	
    // Default Constructor
    public Merchant() {}
	
	// Method:		"BuyFood()"
	public static IEnumberable<int> BuyFood() {
		int totalToBuy = MaxFoodToBuy();
		int BuyTwenty = totalToBuy / 20;
		int remainder = totalToBuy % 20;
		// parsing through vendor
		string itemID;
		string temp;
		int ID;
		for (int i = 1; i < QH.API.ExecuteLua<int>("return GetMerchantNumItems()"); i++) {
			itemID = QH.API.ExecuteLua<string>("return GetMerchantItemLink(" + i + ");");
			temp = itemID.Substring(itemID.IndexOf(':') + 1);
			itemID = itemID.Substring(itemID.IndexOf(':') + 1, temp.IndexOf(':'));
			ID = int.Parse(itemID);
			if (FoodIDToBuy == ID) {
				// i = Multiples of 20
				for (int i = 0; i < BuyTwenty; i++) {
					QH.API.ExecuteLua("BuyMerchantItem(" + i + ", 20)");
					yield return 500;
				}
				QH.API.ExecuteLua("BuyMerchantItem(" + i + "," + remainder + ")");
				yield return 500;
				yield break;
			}
		}
		yield break;
	}
	
	// Method:		"BuyDrink()"
	public static IEnumberable<int> BuyDrink() {
		int totalToBuy = MaxDrinkToBuy();
		int BuyTwenty = totalToBuy / 20;
		int remainder = totalToBuy % 20;
		// parsing through vendor
		string itemID;
		string temp;
		int ID;
		for (int i = 1; i < QH.API.ExecuteLua<int>("return GetMerchantNumItems()"); i++) {
			itemID = QH.API.ExecuteLua<string>("return GetMerchantItemLink(" + i + ");");
			temp = itemID.Substring(itemID.IndexOf(':') + 1);
			itemID = itemID.Substring(itemID.IndexOf(':') + 1, temp.IndexOf(':'));
			ID = int.Parse(itemID);
			if (DrinkIDToBuy == ID) {
				// i = Multiples of 20
				for (int i = 0; i < BuyTwenty; i++) {
					QH.API.ExecuteLua("BuyMerchantItem(" + i + ", 20)");
					yield return 500;
				}
				QH.API.ExecuteLua("BuyMerchantItem(" + i + "," + remainder + ")");
				yield return 500;
				yield break;
			}
		}
		yield break;
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
	
	public static List<int[]> getFoodInInventory() {
		List<int[]> owned = new List<int[]>();
		var inventory = QH.API.Inventory.Items;
		// I need to initialize the bags...
		QH.API.Inventory.Refresh();
		
		int count = 0;
		foreach (var item in inventory) {
			for (int i = 0; i < FoodIDs.Count; i++) {
				if (FoodIDs[i] == item.ItemId) {
					int[] list = new int[2] {item.ItemId,item.StackCount};
					owned.Add(list);
					break;
				}
			}
		}
		// Remove Duplicates, add total item count into one ID.
		int count2;
		int numInstances;
		int id = 0;
		List<int[]> copy = owned;
		for (int i = 0; i < owned.Count; i++) {
			count2 = 0;
			numInstances = 0;
			for (int j = 0; j < copy.Count; j++) {
				if (copy[j][0] == owned[i][0]) {
					count2 = count2 + copy[j][1];
					numInstances++;
					if (numInstances > 1 && numInstances < 3)
					{
						id = copy[j][0];
					}
				}
			}
			if (numInstances > 1) {
				for (int j = owned.Count - 1; j >= 0; j--) {
					if (numInstances > 1) {
						if (owned[j][0] == id) {
							owned.RemoveAt(j);
							numInstances--;
						}
					}
					else if (numInstances == 1){
						if (owned[j][0] == id) {
							owned[j][1] = count2;
						}
					}
				}
			}
		}
		return owned;
	}
	
	
	public static List<int[]> getDrinkInInventory() {
		List<int[]> owned = new List<int[]>();
		var inventory = QH.API.Inventory.Items;
		// I need to initialize the bags...
		QH.API.Inventory.Refresh();
		
		int count = 0;
		foreach (var item in inventory) {
			for (int i = 0; i < DrinkIDs.Count; i++) {
				if (DrinkIDs[i] == item.ItemId) {
					int[] list = new int[2] {item.ItemId,item.StackCount};
					owned.Add(list);
					break;
				}
			}
		}
		// Remove Duplicates, add total item count into one ID.
		int count2;
		int numInstances;
		int id = 0;
		List<int[]> copy = owned;
		for (int i = 0; i < owned.Count; i++) {
			count2 = 0;
			numInstances = 0;
			for (int j = 0; j < copy.Count; j++) {
				if (copy[j][0] == owned[i][0]) {
					count2 = count2 + copy[j][1];
					numInstances++;
					if (numInstances > 1 && numInstances < 3)
					{
						id = copy[j][0];
					}
				}
			}
			if (numInstances > 1) {
				for (int j = owned.Count - 1; j >= 0; j--) {
					if (numInstances > 1) {
						if (owned[j][0] == id) {
							owned.RemoveAt(j);
							numInstances--;
						}
					}
					else if (numInstances == 1){
						if (owned[j][0] == id) {
							owned[j][1] = count2;
						}
					}
				}
			}
		}
		return owned;
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
	
	public static bool IsFoodOrDrinkNeeded(int numMinimumFood, int numMinimumWater) {
		
		if (QH.API.Me.ContinentID == 1116) {
			FoodIDs = DraenorMerchants.getFood();
			DrinkIDs = DraenorMerchants.getWater();
		}
		
		// else if() To be added for other continents.
		
		// Now, finding all food and drinks in my bags that much these known items to use, and counting how many in possession.
		InventoryFood = getFoodInInventory();
		int highestAmount = 0;
		foreach(int[] foodAmount in InventoryFood) {
			if (highestAmount < foodAmount[1]){
				highestAmount = foodAmount[1];
			}
		}
		
		if (highestAmount < numMinimumFood) {
			QH.API.Print("Player is Low on Food! Heading to Restock!!!");
			return true;
		}
		else {
			InventoryWater = getDrinkInInventory();
			int highestAmountWater = 0;
			foreach(int[] drinkAmount in InventoryWater) {
				if (highestAmountWater < drinkAmount[1]) {
					highestAmountWater = drinkAmount[1];
				}
			}
			if (highestAmountWater < numMinimumWater) {
				QH.API.Print("Player is Low on Water! Head to Restock!!!");
				return true;
			}
		}
		return false;
	}
	
	public static bool IsRepairNeeded() {
		QH.API.Print("Player is in Need of Repair.  Heading to nearest Vendor!");
		return false;
	}
	
	public static int MaxDrinkToBuy(int max) {
		List<int[]> allVendorDrinkItems = new List<int>();
		int total;
		
		// Parse through vendor items, collect all items that match item in bags.
		string itemID;
		string temp;
		int ID;
		for (int i = 1; i < QH.API.ExecuteLua<int>("return GetMerchantNumItems()"); i++) {
			itemID = QH.API.ExecuteLua<string>("return GetMerchantItemLink(" + i + ");");
			temp = itemID.Substring(itemID.IndexOf(':') + 1);
			itemID = itemID.Substring(itemID.IndexOf(':') + 1, temp.IndexOf(':'));
			ID = int.Parse(itemID);
			foreach (int[] unit in InventoryWater) {
				if (unit[0] == ID) {
					allVendorDrinkItems.Add(unit);
					break;
				}
			}
		}
		// If at least 1 match
		if (allVendorDrinkItems.Count > 0) {
			int highest = 0;
			foreach (int[] item in allVendorDrinkItems) {
				if (highest < item[1]) {
					DrinkIDToBuy = item[0];
					highest = item[1];
				}
			}
			total = max - highest;
		}
		else {
			total = 100;
		}
		return total;
	}
	
	public static int MaxFoodToBuy(int max) {
		
		List<int[]> allVendorFoodItems = new List<int>();
		int total;
		
		// Parse through vendor items, collect all items that match item in bags.
		string itemID;
		string temp;
		int ID;
		for (int i = 1; i < QH.API.ExecuteLua<int>("return GetMerchantNumItems()"); i++) {
			itemID = QH.API.ExecuteLua<string>("return GetMerchantItemLink(" + i + ");");
			temp = itemID.Substring(itemID.IndexOf(':') + 1);
			itemID = itemID.Substring(itemID.IndexOf(':') + 1, temp.IndexOf(':'));
			ID = int.Parse(itemID);
			foreach (int[] unit in InventoryFood) {
				if (unit[0] == ID) {
					allVendorFoodItems.Add(unit);
					break;
				}
			}
		}
		// If at least 1 match
		if (allVendorFoodItems.Count > 0) {
			int highest = 0;
			foreach (int[] item in allVendorFoodItems) {
				if (highest < item[1]) {
					FoodIDToBuy = item[0];
					highest = item[1];
				}
			}
			total = max - highest;
		}
		else {
			total = 100;
			FoodIDToBuy = allVendorFoodItems[0][0];
		}
		return total;
	}
	
	public static void MerchantGossip() {
		// Initializing Function
        string title = "title0";
        string luaCall = ("local title0,_ = GetGossipOptions(); if title0 ~= nil then return title0 else return \"nil\" end");
        string result = QH.API.ExecuteLua<string>(luaCall);
        // Now Ready to Iterate through All Gossip Options!
        // The reason "1" is used instead of the conventional "0" is because Gossip options start at 1.
        int i = 1;
        string num = "";
        while (result != null) {
            if (result.Equals("Let me browse your goods.") || result.Equals("Deja que eche un vistazo a tus mercancías.") || result.Equals("Deixe-me dar uma olhada nas suas mercadorias.") || result.Equals("Ich möchte ein wenig in Euren Waren stöbern.") || result.Equals("Deja que eche un vistazo a tus mercancías.") || result.Equals("Permettez-moi de jeter un œil à vos biens.") || result.Equals("Fammi vedere la tua merce.") || result.Equals("Позвольте взглянуть на ваши товары.") || result.Equals("물건을 살펴보고 싶습니다.") || result.Equals(" 讓我看看你出售的貨物。") || result.Equals("让我看看你出售的货物。")) {
                QH.API.ExecuteLua("SelectGossipOption(" + i + ");");
                break;
            }
            else {
                // This builds the new string to be added into an Lua API call.
                num = i.ToString();
                title = (title.Substring(0,title.Length-1) + num);
                luaCall = ("local " + title + ",_," + luaCall.Substring(6,luaCall.Length-6));
                result = QH.API.ExecuteLua<string>(luaCall);
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
            	var check = new Fiber<int>(BuyDrink());
				while (check.Run()) {
					yield return 100;
				}
        	}
			// Buying food
			var check2 = new Fiber<int>(BuyFood());
			while (check2.Run()) {
				yield return 100;
			}
			// Updating Food and Water Lists
			setUsableFood();
			setUsableWater();
			QH.API.Print("Refreshments Replenished! Back to Work!!!");
			QH.API.ExecuteLua("CloseMerchant()");
		}
		yield break;
	}
	
	public static void setUsableFood() {
		InventoryFood = getFoodInInventory();
		for (int i = 0; i < InventoryFood.Count; i++) {
			QH.API.GlobalBotSettings.FoodItemIds.Add(InventoryFood[i][0]);
		}
	}
	
	public static void setUsableWater() {
		InventoryWater = getDrinkInInventory();
		for (int i = 0; i < InventoryWater.Count; i++) {
			QH.API.GlobalBotSettings.DrinkItemIds.Add(InventoryFood[i][0]);
		}
	}
}

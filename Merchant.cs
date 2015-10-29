/*  VendorManager
|   Author: @Sklug  aka TheGenomeWhisperer
|
|   To Be Used with "InsertContinentName.cs" and "Localization.cs" class
|   For use in collaboration with the Rebot API 
|
|   Last Update 26th Oct, 2015 */


public class Merchant
{
	public static ReBotAPI API;
    public static Fiber<int> Fib;
	// List of all potential food/drink items good for
	public static List<int> DrinkIDs;
	public static List<int> FoodIDs;
	// List of all food items and drink items on the given vendor.
	public static List<int> vendorDrinks;
	public static List<int> vendorFood;
	// List of Arrays, containing in position 0, item ID Number; position 1, quantity owned in bags.
	public static List<int[]> InventoryFood;
	public static List<int[]> InventoryWater;
	// Eventual ID of the food/water player will purchase from the vendor.
	public static int FoodIDToBuy;
	public static int DrinkIDToBuy;
	// Max amount of food/water to hold, easily adjustable
	public static int FoodCap = 36;
	public static int DrinkCap = 36;
	// Min amount of food/water to possess before activating vendor logic again.
	public static int MinFood = 25;
	public static int MinWater = 25;
	
    // Default Constructor
    public Merchant() {}
		
	public static List<int[]> getFoodInInventory() {
		List<int[]> owned = new List<int[]>();
		var Inv = Inventory.Items;
		// I need to initialize the bags...
		Inventory.Refresh();
		
		int count = 0;
		foreach (var item in Inv) {
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
		var Inv = Inventory.Items;
		// I need to initialize the bags...
		Inventory.Refresh();
		
		int count = 0;
		foreach (var item in Inv) {
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
	
	public static bool IsFoodOrDrinkNeeded(int numMinimumFood, int numMinimumWater) {		
		// Now, finding all food and drinks in my bags that much these known items to use, and counting how many in possession.
		int highestAmount = 0;
		foreach(int[] foodAmount in InventoryFood) {
			if (highestAmount < foodAmount[1]){
				highestAmount = foodAmount[1];
			}
		}
		
		if (highestAmount < numMinimumFood) {
			API.Print("Player is Low on Food! Heading to Restock!!!");
			return true;
		}
		else {
			int highestAmountWater = 0;
			foreach(int[] drinkAmount in InventoryWater) {
				if (highestAmountWater < drinkAmount[1]) {
					highestAmountWater = drinkAmount[1];
				}
			}
			if (highestAmountWater < numMinimumWater) {
				API.Print("Player is Low on Water! Head to Restock!!!");
				return true;
			}
		}
		return false;
	}
	
	public static List<object> getVendorInfo(int TypeofMerchant) {
		List<object> vendor = new List<object>();
        int continentID = API.Me.ContinentID;
        int zoneID = API.Me.ZoneId;
        bool factionIsHorde = API.Me.IsHorde;
        
        // Draenor Continent
        if (continentID == 1116)
        {
            vendor = DraenorMerchants.getMerchantInfo(zoneID, factionIsHorde, TypeofMerchant);
        }
        
        // All Continents Eventually to be Added
        return vendor;
	}
	
	public static List<int> getMerchantFoodList() {
		List<int> items = new List<int>();
		if (IsVendorOpen()) {
			string itemID;
			string temp;
			int ID;
			for (int i = 1; i < API.ExecuteLua<int>("return GetMerchantNumItems()"); i++) {
				itemID = API.ExecuteLua<string>("return GetMerchantItemLink(" + i + ");");
				temp = itemID.Substring(itemID.IndexOf(':') + 1);
				itemID = itemID.Substring(itemID.IndexOf(':') + 1, temp.IndexOf(':'));
				ID = int.Parse(itemID);
				foreach (int foodItem in FoodIDs) {
					if (ID == foodItem) {
						items.Add(ID);
						break;
					}
				}
			}
		}
		else {
			API.Print("Error, Merchant Window Not Open.  Failure to Collect Merchant Trade info.");
			items.Add(0);
		}
		return items;
	}
	
	public static List<int> getMerchantDrinkList() {
		List<int> items = new List<int>();
		if (IsVendorOpen()) {
			string itemID;
			string temp;
			int ID;
			for (int i = 1; i < API.ExecuteLua<int>("return GetMerchantNumItems()"); i++) {
				itemID = API.ExecuteLua<string>("return GetMerchantItemLink(" + i + ");");
				temp = itemID.Substring(itemID.IndexOf(':') + 1);
				itemID = itemID.Substring(itemID.IndexOf(':') + 1, temp.IndexOf(':'));
				ID = int.Parse(itemID);
				foreach (int drinkItem in DrinkIDs) {
					if (ID == drinkItem) {
						items.Add(ID);
						break;
					}
				}
			}
		}
		else {
			API.Print("Error, Merchant Window Not Open.  Failure to Collect Merchant Trade info.");
			items.Add(0);
		}
		return items;
	}
	
	public static IEnumerable<int> MoveToMerchant(List<object> merchantInfo) {
		// If Empty Result, Zone not known.
		if (merchantInfo.Count == 0) {
			API.Print("Unfortunately the Zone Your Are in Does Not Have the Food and Drink NPCs Mapped yet!");
			API.Print("It Would Be Amazing if You Could Report Back on the Forums This Issue! Thank you!");
			yield break;
		}
		
		// Casting all the Object to Types
        Vector3 destination = (Vector3) merchantInfo[0];
        float distance = (float) merchantInfo[1];
        distance = (int)Math.Ceiling(distance);
        int npcID = (int) merchantInfo[2];
        bool IsSpecialPathingNeeded = (bool) merchantInfo[3];
		
        string yards = "Yards";
        // String from plural to non. QoL thing only...
        if (distance == 1) {
            yards = "Yard";
        }
        API.Print("Traveling Roughly " + distance + " " + yards + " to Get to the Closest VendorManager...");
		
		// This is where to add special pathing considerations.
        if (IsSpecialPathingNeeded) {
            if (API.Me.ContinentID == 1116) {
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
        while (!API.MoveTo(destination)) {
            yield return 100;
        }
        
        // Targeting the Merchant!
        foreach (var unit in API.Units) {
            if (unit.EntryID == npcID) {
                API.Me.SetFocus(unit);
                API.Me.SetTarget(unit);
                break;
            }
        }
        
        // Edging closer to the Merchant!
        while(API.Me.Focus != null && !API.MoveTo(API.Me.Focus.Position)) {
            yield return 100;
        }
	}
	
	public static List<object> GetClosestMerchant(int TypeofMerchant) {
		List<object> vendor = new List<object>();
        List<object> result = new List<object>();
		
		// Obtaining the List with the NPC info and location.
		vendor = getVendorInfo(TypeofMerchant);
		
		if (vendor.Count != 0) {
			float closestDistance;
			float tempDistance;
			int npcID;
			bool IsSpecialPathingNeeded;
			Vector3 closestVector3;
			Vector3 position;
		
			// Setting the initial Merchant to the closest distance
			closestVector3 = new Vector3((float)vendor[0], (float)vendor[1], (float)vendor[2]);
			closestDistance = API.Me.Distance2DTo(closestVector3);
			npcID = (int)vendor[3];
			IsSpecialPathingNeeded = (bool)vendor[4];
			
			// Filtering for Closest Merchant now.
			for (int i = 0; i < vendor.Count - 4; i = i + 5) {
				// Setting first values
				position = new Vector3((float)vendor[i], (float)vendor[i + 1], (float)vendor[i + 2]);
				tempDistance = API.Me.Distance2DTo(position);
				// Changing values if new ones are closer
				if (tempDistance < closestDistance) {
					closestDistance = tempDistance;
					closestVector3 = position;
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
	
	public static IEnumerable<int> InteractWithMerchant() {
		if (API.Me.Focus != null) {
			while(!IsVendorOpen()) {
				API.Me.Focus.Interact();
				yield return 1500;
				if (!IsVendorOpen()) {
					MerchantGossip();
					yield return 2000;
				}
			}
		}
		yield break;
	}
	
	// Method:		"IsVendorOpen()"
	public static bool IsVendorOpen() {
		return API.ExecuteLua<bool>("local name = GetMerchantItemInfo(1); if name ~= nil then return true else return false end;");
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
	
	public static void setUsableFood() {
		for (int i = 0; i < InventoryFood.Count; i++) {
			API.GlobalBotSettings.FoodItemIds.Add(InventoryFood[i][0]);
		}
	}
	
	public static void setUsableWater() {
		InventoryWater = getDrinkInInventory();
		for (int i = 0; i < InventoryWater.Count; i++) {
			API.GlobalBotSettings.DrinkItemIds.Add(InventoryWater[i][0]);
		}
	}
	
	// Method:		"BuyFood()"
	public static IEnumerable<int> BuyFood(int max) {
		int totalToBuy = MaxFoodToBuy(max);
		if (totalToBuy < 1) {
			totalToBuy = 0;
		}
		int BuyTwenty = totalToBuy / 20;
		int remainder = totalToBuy % 20;
		// parsing through vendor
		string itemID;
		string temp;
		int ID;
		API.Print(FoodIDToBuy);
		for (int i = 1; i < API.ExecuteLua<int>("return GetMerchantNumItems()"); i++) {
			itemID = API.ExecuteLua<string>("return GetMerchantItemLink(" + i + ");");
			temp = itemID.Substring(itemID.IndexOf(':') + 1);
			itemID = itemID.Substring(itemID.IndexOf(':') + 1, temp.IndexOf(':'));
			ID = int.Parse(itemID);
			if (FoodIDToBuy == ID) {
				// j = Multiples of 20
				for (int j = 0; j < BuyTwenty; j++) {
					API.ExecuteLua("BuyMerchantItem(" + i + ", 20)");
					yield return 500;
				}
				if (remainder > 0) {
					API.ExecuteLua("BuyMerchantItem(" + i + "," + remainder + ")");
					yield return 500;
				}
				yield break;
			}
		}
		yield break;
	}
	
	public static int MaxFoodToBuy(int max) {
		List<int[]> allVendorFoodItems = new List<int[]>();
		vendorFood = getMerchantFoodList();
		int total;
		// Parse through vendor items, collect all items that match item in bags.
		foreach (int vendorItem in vendorFood) {
			foreach (int[] unit in InventoryFood) {
				if (unit[0] == vendorItem) {
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
			total = FoodCap;
			FoodIDToBuy = vendorFood[0];
		}
		return total;
	}
	
	public static int MaxDrinkToBuy(int max) {
		List<int[]> allVendorDrinkItems = new List<int[]>();
		vendorDrinks = getMerchantDrinkList();
		int total;
		// Parse through vendor items, collect all items that match item in bags.
		foreach (int drinks in vendorDrinks) {
			foreach (int[] unit in InventoryWater) {
				if (unit[0] == drinks) {
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
			total = DrinkCap;
			DrinkIDToBuy = vendorDrinks[0];
		}
		return total;
	}
	
	// Method:		"BuyDrink()"
	public static IEnumerable<int> BuyDrink(int max) {
		int totalToBuy = MaxDrinkToBuy(max);
		if (totalToBuy < 1) {
			totalToBuy = 0;
		}
		int BuyTwenty = totalToBuy / 20;
		int remainder = totalToBuy % 20;
		// parsing through vendor
		string itemID;
		string temp;
		int ID;
		for (int i = 1; i < API.ExecuteLua<int>("return GetMerchantNumItems()"); i++) {
			itemID = API.ExecuteLua<string>("return GetMerchantItemLink(" + i + ");");
			temp = itemID.Substring(itemID.IndexOf(':') + 1);
			itemID = itemID.Substring(itemID.IndexOf(':') + 1, temp.IndexOf(':'));
			ID = int.Parse(itemID);
			if (DrinkIDToBuy == ID) {
				// j = Multiples of 20
				for (int j = 0; j < BuyTwenty; j++) {
					API.ExecuteLua("BuyMerchantItem(" + i + ", 20)");
					yield return 500;
				}
				if (remainder > 0) {
					API.ExecuteLua("BuyMerchantItem(" + i + "," + remainder + ")");
					yield return 500;
				}
				yield break;
			}
		}
		yield break;
	}
		
	public static void Repair() {
	// To be filled later
	}
	
	public static IEnumerable<int> RepairCheck() {
		yield break;
	}
	
	public static bool IsRepairNeeded() {
		API.Print("Player is in Need of Repair.  Heading to nearest Vendor!");
		return false;
	}
	
	public static IEnumerable<int> RestingCheck(){
				// Continent Selection
		FoodIDs = new List<int>();
		DrinkIDs = new List<int>();
		if (API.Me.ContinentID == 1116) {
			FoodIDs = DraenorMerchants.getFood();
			DrinkIDs = DraenorMerchants.getWater();
		}
		
		//default
		InventoryFood = getFoodInInventory();
		InventoryWater = getDrinkInInventory();
		
		// else if() To be added for other continents.
		
		List<object> closest = GetClosestMerchant(1);
		if (closest.Count > 0) {
			if (IsFoodOrDrinkNeeded(MinFood,MinWater)) {
				// Identifying Merchant and moving to it.
				var check = new Fiber<int>(MoveToMerchant(closest));
				while (check.Run()) {
					yield return 100;
				}
				
				// Interacting with Merchant
				var check2 = new Fiber<int>(InteractWithMerchant());
				while (check2.Run()) {
					yield return 100;
				}
				
				// Buy Water if Needed
				if (API.Me.Class.ToString().Equals("Paladin") || API.Me.Class.ToString().Equals("Priest") || API.Me.Class.ToString().Equals("Shaman") || API.Me.Class.ToString().Equals("Mage") || 
				API.Me.Class.ToString().Equals("Warlock") || API.Me.Class.ToString().Equals("Druid") || API.Me.Class.ToString().Equals("Monk")) {
					var check3 = new Fiber<int>(BuyDrink(DrinkCap));
					while (check3.Run()) {
						yield return 100;
					}
				}
				// Buying food
				var check4 = new Fiber<int>(BuyFood(FoodCap));
				while (check4.Run()) {
					yield return 100;
				}
				// Updating Food and Water Lists
				setUsableFood();
				setUsableWater();
				API.Print("Refreshments Replenished! Back to Work!!!");
				API.ExecuteLua("CloseMerchant()");
			}
		}
		yield break;
	}
}
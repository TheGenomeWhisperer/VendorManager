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
	private static List<int> DrinkIDs;
	private static List<int> FoodIDs;
	// List of all food items and drink items on the given vendor.
	private static List<int> vendorDrinks;
	private static List<int> vendorFood;
	// List of Arrays, containing in position 0, item ID Number; position 1, quantity owned in bags.
	private static List<int[]> InventoryFood;
	private static List<int[]> InventoryWater;
	// Eventual ID of the food/water player will purchase from the vendor.
	private static int FoodIDToBuy;
	private static int DrinkIDToBuy;
	// Max amount of food/water to hold, easily adjustable
	public static int FoodCap = 100;
	public static int DrinkCap = 50;
	// Min amount of food/water to possess before activating vendor logic again.
	public static int MinFood = 5;
	public static int MinWater = 5;
	// Repair information
	public static int MinDurability = 20;  // Percentage Gear damage remaining before heading to a vendor, this would be 20%.
	
    // Default Constructor
    public Merchant() {}
	
	// Method:		"InitializeMerchant()"
	// 				Helps Reduce overhead so only need to initialize the one time.
	//				THIS NEEDS TO BE RUN AT THE VERY BEGINNING OF ALL PROFILES JUST ONCE!!! ("Merchant.Initialize();")
	public static void Initialize() {
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
	}
	
	// Method:			"getFoodInInventory()"
	// What it Does:	This returns a List of Arrays, each array including first (position 0), the itemID in your bags that is found to be a match
	//					for the given zone.
	private static List<int[]> getFoodInInventory() {
		List<int[]> owned = new List<int[]>();
		// I need to initialize the bags...
		Inventory.Refresh();
		var Inv = Inventory.Items;
						
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
	
	private static List<int[]> getDrinkInInventory() {
		List<int[]> owned = new List<int[]>();
		// I need to initialize the bags...
		Inventory.Refresh();
		var Inv = Inventory.Items;

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
	
	private static List<object> getVendorInfo(int TypeofMerchant) {
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
	
	private static List<int> getMerchantFoodList() {
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
	
	private static List<int> getMerchantDrinkList() {
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
	
	private static IEnumerable<int> MoveToMerchant(List<object> merchantInfo) {
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
	
	private static List<object> GetClosestMerchant(int TypeofMerchant) {
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
	
	// Method:		"InteractWithMerchant()"
	private static IEnumerable<int> InteractWithMerchant() {
		if (API.Me.Focus != null) {
			while(!IsVendorOpen()) {
				API.Me.Focus.Interact();
				yield return 1500;
				if (!IsVendorOpen()) {
					API.Print("test");
					MerchantGossip();
					yield return 2000;
				}
			}
		}
		yield break;
	}
	
	// Method:		"IsVendorOpen()"
	private static bool IsVendorOpen() {
		return API.ExecuteLua<bool>("local name = GetMerchantItemInfo(1); if name ~= nil then return true else return false end;");
	}
	
	private static void MerchantGossip() {
		// Initializing Function
        string title = "title0";
        string luaCall = ("local title0,_ = GetGossipOptions(); if title0 ~= nil then return title0 else return \"nil\" end");
        string result = API.ExecuteLua<string>(luaCall);
        // Now Ready to Iterate through All Gossip Options!
        // The reason "1" is used instead of the conventional "0" is because Gossip options start at 1.
        int i = 1;
        string num = "";
        while (result != null) {
            if ((result.Equals("Let me browse your goods.") || result.Equals("I want to browse your goods.")) || (result.Equals("Deja que eche un vistazo a tus mercancías.") || result.Equals("Quiero ver tus mercancías.")) || (result.Equals("Deixe-me dar uma olhada nas suas mercadorias.") || result.Equals("Quero ver o que você tem à venda.")) || (result.Equals("Ich möchte ein wenig in Euren Waren stöbern.") || result.Equals("Ich sehe mich nur mal um.")) || (result.Equals("Deja que eche un vistazo a tus mercancías.") || result.Equals("Quiero ver tus mercancías.")) || (result.Equals("Permettez-moi de jeter un œil à vos biens.") || result.Equals("Je voudrais regarder vos articles.")) || (result.Equals("Fammi vedere la tua merce.") || result.Equals("Voglio dare un'occhiata alla tua merce.")) || (result.Equals("Позвольте взглянуть на ваши товары.") || result.Equals("Я хочу посмотреть на ваши товары.")) || (result.Equals("물건을 살펴보고 싶습니다.") || result.Equals("물건을 보고 싶습니다.")) || (result.Equals(" 讓我看看你出售的貨物。") || result.Equals("我想要看看你賣的貨物。")) || (result.Equals("让我看看你出售的货物。") || result.Equals("我想要看看你卖的货物。"))) {
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
	
	private static void setUsableFood() {
		InventoryFood = getFoodInInventory();
		bool found;
		bool found2;
		for (int i = 0; i < InventoryFood.Count; i++) {
			found = false;
			foreach (int foodID in API.GlobalBotSettings.FoodItemIds) {
				if (foodID == InventoryFood[i][0]) {
					found = true;
				}
			}
			if (!found) {
				API.GlobalBotSettings.FoodItemIds.Add(InventoryFood[i][0]);
			}
			// We should also add these to the "Protected" list to not sell.
			found2 = false;
			foreach (int protectedID in API.GlobalBotSettings.ProtectedItemIds) {
				if (protectedID == InventoryFood[i][0]) {
					found2 = true;
				}
			}
			if (!found2) {
				API.GlobalBotSettings.ProtectedItemIds.Add(InventoryFood[i][0]);
			}
		}
	}
	
	private static void setUsableWater() {
		InventoryWater = getDrinkInInventory();
		bool found;
		bool found2;
		for (int i = 0; i < InventoryWater.Count; i++) {
			found = false;
			foreach (int drinkID in API.GlobalBotSettings.DrinkItemIds) {
				if (drinkID == InventoryWater[i][0]) {
					found = true;
				}
			}
			if (found == false) {
				API.GlobalBotSettings.DrinkItemIds.Add(InventoryWater[i][0]);
			}
			// We should also add these to the "Protected" list to not sell.
			found2 = false;
			foreach (int protectedID in API.GlobalBotSettings.ProtectedItemIds) {
				if (protectedID == InventoryWater[i][0]) {
					found2 = true;
				}
			}
			if (!found2) {
				API.GlobalBotSettings.ProtectedItemIds.Add(InventoryWater[i][0]);
			}
		}
	}
	// Method:		"BuyFood()"
	private static IEnumerable<int> BuyFood(int max) {
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
	
	private static int MaxFoodToBuy(int max) {
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
	
	private static int MaxDrinkToBuy(int max) {
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
	private static IEnumerable<int> BuyDrink(int max) {
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
			
	public static IEnumerable<int> RestingCheck() {
		// Initialize the Script and pulls all info from appropriate pathing
		// Determins if necessary to start resting Script.
		if (IsFoodOrDrinkNeeded(MinFood,MinWater)) {
			List<object> closest = GetClosestMerchant(1);
			if (closest.Count > 0) {
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
				API.Me.Class.ToString().Equals("Warlock") || API.Me.Class.ToString().Equals("Druid") || API.Me.Class.ToString().Equals("Monk") || API.Me.Class.ToString().Equals("Hunter")) {
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
				API.Print("Refreshments Replenished!");
				// Updating Food and Water Lists
				setUsableFood();
				setUsableWater();
				
				// And, since we are at the vendor already, let's clear some loot.
				SellLootedItems();
				
				// And, since we are already in town, let's see if it's worth the effort to repair.
				// Quick repair if at vendor
				Repair();
				if (IsRepairVendorNearby() && getDurability() <= 75 || NumItemsBroken() > 0) {
					var check5 = new Fiber<int>(RepairCheck());
					while (check5.Run()) {
						yield return 100;
					}
				}
				API.Print("The Player is Fully Stocked and Ready to Go. Let's Get Back to Work!!!");
				API.ExecuteLua("CloseMerchant()");
			}
		}
		yield break;
	}
	
	///////////////////////////////////////////
	/////							      /////
	/////   ALL SELLING/VENDOR METHODS    /////
	/////								  /////
	///////////////////////////////////////////
	
	private static void Repair() {
		if (IsVendorOpen() && API.ExecuteLua<bool>("return CanMerchantRepair()")) {
			API.ExecuteLua("RepairAllItems()");
		}
	}
		
	// Method		"IsRepairNeeded()"
	//				20% or less equals
	public static bool IsRepairNeeded() {
		if (NumItemsBroken() > 0 || getDurability() <= MinDurability) {
			API.Print("Player is in Need of Repair.  Heading to nearest Vendor!");
			return true;
		}
		return false;
	}
	
	private static List<int> getItemsToSell() {
		List<int> itemsToSell = new List<int>();
		
		// Verifying no protected items in list, and if so, removing.
		bool toAdd;
		foreach (int toSellID in API.GlobalBotSettings.Sell_LootedItems) {
			toAdd = true;
			foreach (int protectedID in API.GlobalBotSettings.ProtectedItemIds) {
				if (protectedID == toSellID) {
					toAdd = false;
				}
			}
			// Adding it to sell list.
			if (toAdd == true) {
				itemsToSell.Add(toSellID);
			}
		} 
		// Note: Since these lists are pulled directly from Rebot, it auto-filters duplicates already
		// so we do not need to check for duplicates and filter them here.
		return itemsToSell;
	}
	
	// Method:		"SellLootedItems(List<int>)"
	public static void SellLootedItems() {
		// For future use to sell specific "types" of gear.
		List<int> itemsToSell = getItemsToSell();
		string grey = "ff9d9d9d";
		string white = "ffffffff";
		string green = "ff1eff00";
		string blue = "ff0070dd";
		string purple = "ffa335ee";
		
		if (IsVendorOpen()) {
			// Sell Grey Items
			API.Print("Selling Any \"junk\" items in your Inventory.");
			API.ExecuteMacro("/run for b=0,4 do for s=1,GetContainerNumSlots(b)do local n=GetContainerItemLink(b,s)if n and strfind(n,\"" + grey + "\") then print(\"Selling \"..n) UseContainerItem(b,s)end end end");
			
			// Sell All "Looted Items"
			bool found;
			if (itemsToSell.Count > 0) {
				API.Print("Selling All Items that You Have Looted and are Not on the \"Protected\" list");
			}
			foreach (int itemID in getItemsToSell()) {
				found = false;
				found = API.ExecuteLua<bool>("local match = false; for i=0,4 do for j=1,GetContainerNumSlots(i)do local n=GetContainerItemID(i,j)if n == " + itemID + " then local s=GetContainerItemLink(i,j)print(\"Selling \"..s)match = true; end end end return match");
				if (found) {
					// Action to sell item.
					API.UseItem(itemID);
				}
			}
			// Let's clear the list of stored items to sell.
			API.GlobalBotSettings.Sell_LootedItems.Clear();
		}
		else {
			API.Print("Player Failed to Sell goods. For Some Reason, Interaction with Vendor Failed.");
			API.Print("Please Report this On The Forums So We Can Fix it!");
		}
	}
	
	// Method:		"getDurability()"
	// Purpose:		Returns the % of character durability.
	private static int getDurability() {
		int count = 9;
		float durability = 0;
		durability += API.ExecuteLua<float>("local durability,max = GetInventoryItemDurability(1) local result = durability/max; return result");
		durability += API.ExecuteLua<float>("local durability,max = GetInventoryItemDurability(3) local result = durability/max; return result");
		for (int i = 5; i <= 10; i++) {
			durability += API.ExecuteLua<float>("local durability,max = GetInventoryItemDurability(" + i + ") local result = durability/max; return result");
		}
		durability += API.ExecuteLua<float>("local durability,max = GetInventoryItemDurability(16) local result = durability/max; return result");
		
		if (API.ExecuteLua<bool>("local durability = GetInventoryItemDurability(17); if durability ~= nil then return true else return false end")) {
			durability += API.ExecuteLua<float>("local durability,max = GetInventoryItemDurability(17) local result = durability/max; return result");
			count++;
		}
		durability = durability/count * 100;
		return (int) durability;
	}
	
	// Method		"NumItemsBroken()"
	// Purpose		Returns if player has any Broken (stored) items
	private static int NumItemsBroken() {
		int count = 0;
		if (API.ExecuteLua<float>("local durability,max = GetInventoryItemDurability(1) local result = durability/max; return result") < 0.1f) {
			count++;
		}
		if (API.ExecuteLua<float>("local durability,max = GetInventoryItemDurability(3) local result = durability/max; return result") < 0.1f) {
			count++;
		}
		for (int i = 5; i <= 10; i++) {
			if (API.ExecuteLua<float>("local durability,max = GetInventoryItemDurability(" + i + ") local result = durability/max; return result") < 0.1f) {
				count++;
			}
		}
		if (API.ExecuteLua<float>("local durability,max = GetInventoryItemDurability(16) local result = durability/max; return result") < 0.1f) {
			count++;
		}
		if (API.ExecuteLua<bool>("local durability = GetInventoryItemDurability(17); if durability ~= nil then return true else return false end")) {
			if (API.ExecuteLua<float>("local durability,max = GetInventoryItemDurability(17) local result = durability/max; return result") < 0.1f) {
				count++;
			}
		}
		if (count > 0) {
			API.Print("It Appears You've Taken Some Heavy Damage. Let's Get to a Vendor and Repair!");
		}
		return count;
	}
	
	// To verify before running repair Action.
	private static bool IsRepairVendorNearby() {
		foreach (var unit in API.Units) {
			if (!unit.IsDead && unit.HasFlag(UnitNPCFlags.CanRepair)) {
				return true;
			}
		}
		return false;
	}
	
	private static bool IsFoodVendorNearby() {
		foreach (var unit in API.Units) {
			if (!unit.IsDead && unit.HasFlag(UnitNPCFlags.SellsFood)) {
				return true;
			}
		}
		return false;
	}
	
	// Method:		"RepairCheck()"
	// Purpose:		This is the main method to be run on a recursive loop, ideally, or within something
	public static IEnumerable<int> RepairCheck() {
		if (IsRepairNeeded()) {
			// The number 2 returns a list of known Repair Vendors, and their locations.
			List<object> closest = GetClosestMerchant(2);
			if (closest.Count > 0) {
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
				
				// Clearing our Loot first
				SellLootedItems();
				
				// Repairing
				Repair();
				
				API.ExecuteLua("CloseMerchant()");
			}
		}
		yield break;
	}
}


/*  VendorManager
|   Author: @Sklug  aka TheGenomeWhisperer
|
|   To Be Used with "InsertContinentName.cs" and "Localization.cs" class
|   For use in collaboration with the Rebot API 
|
|   Last Update: December 17th, 2015
*/

public class Merchant
{
	// All Instance Variables
	// Rebot API Access and Integration
	public static ReBotAPI API;
	public static Fiber<int> Fib;

	// Max amount of food/water to hold, easily adjustable
	public static int FoodCap = 100;
	public static int DrinkCap = 50;

	// Min amount of food/water to possess before activating vendor logic again.
	public static int MinFood = 2;
	public static int MinWater = 2;

	// Repair information
	public static int MinDurability = 20;  // Percentage Gear damage remaining before heading to a vendor, this would be 20%.

	// Once the bag has this few of slots it will go Vendor goods.
	public static int MinFreeSlots = 5;

	// Once Bags are Full and No items are part of a list to be vendored, rather than rechecking the vendor constantly, this disables rechecking. This can also manually disable Vendor action.
	public static bool IgnoreVendor = false;

	// Initialization boolean check before all PUBLIC available API for error prevention on use of API. (Fool-proofing).
	public static bool IsScriptInitialized = false;
	public static int CurrentZoneID;

	// List of all potential food/drink items good for
	public static List<int> DrinkIDs = new List<int>();
	public static List<int> FoodIDs = new List<int>();

	// List of all food items and drink items on the given vendor.
	public static List<int> vendorDrinks;
	public static List<int> vendorFood;

	// List of Arrays, containing in position 0, item ID Number; position 1, quantity owned in bags.
	public static List<int[]> InventoryFood;
	public static List<int[]> InventoryWater;

	// Eventual ID of the food/water player will purchase from the vendor.
	public static int FoodIDToBuy;
	public static int DrinkIDToBuy;


	// Default Constructor
	public Merchant() { }



	///////////////////////////////////////////
	/////							      /////
	/////      All Methods Related to     /////
	/////      Food/Water Restocking      /////
	/////								  /////
	///////////////////////////////////////////



	// Method:		"InitializeMerchant()"
	// 				Helps Reduce overhead so only need to initialize the one time.
	//				THIS NEEDS TO BE RUN AT THE VERY BEGINNING OF ALL PROFILES JUST ONCE!!! ("Merchant.Initialize();")
	//				Also note, this is only to be used initially if not changing zones... leave this alone if you are going to be dynamic in use and it will
	//				be automatically initialized.
	public static void Initialize()
	{
		IsScriptInitialized = true;
		CurrentZoneID = API.Me.ZoneId;

        // Disabling of the Rebot built-in feature.
        API.GlobalBotSettings.BuyNumFood = 0;
        API.GlobalBotSettings.BuyNumDrink = 0;
        API.GlobalBotSettings.VendorOnMinDurability = 0;
        API.GlobalBotSettings.VendorOnNumDestroyedItems = 9;
		API.GlobalBotSettings.MinimumFreeInventorySlots = 0;

        // Continent Selection
        if (API.Me.ContinentID == 1116 || API.Me.ContinentID == 1265)
		{
			FoodIDs = DraenorMerchants.getFood();
			DrinkIDs = DraenorMerchants.getWater();
		}
		// else if() To be added for other continents.

		//default
		InventoryFood = getFoodInInventory();
		InventoryWater = getDrinkInInventory();

		// Ensure all food/drink items in inventory are established to be used.
		setUsableFood();
		setUsableWater();
	}

	// Method:			"BlacklistNPC(int)"
	// 					Uses the NPCs default List "GlobalBotSettings.DestroyItemIds" as a collection of NPCs RemoveBlacklistedVendors
	//					So as to not conflict them with actual items to be stored, I store their NPC ID on a multiple of 20, thus it remains in the list, but is hidden.
	//					In other words:        BlaclistID = EntryID * 20; And, then it is stored.
	public static void BlacklistNPC(int npcID)
	{
		int blacklistID = npcID * 20;
		int count = 0;
		// verify not adding a duplicate
		foreach (int id in API.GlobalBotSettings.DestroyItemIds)
		{
			if (id == blacklistID)
			{
				count++;
				break;
			}
		}
		// if no match was found
		if (count == 0)
		{
			API.GlobalBotSettings.DestroyItemIds.Add(blacklistID);
		}
	}

	// This returns the list of all available zone NPCs and the removes the ones that are matched to the blacklist.
	private static List<object> RemoveBlacklistedVendors(List<object> vendorNPCs)
	{
		// If both Lists are not empty, so as not to waste time parsing.
		int id;
		if (API.GlobalBotSettings.DestroyItemIds.Count > 0 && vendorNPCs.Count > 0)
		{
			for (int i = 0; i < vendorNPCs.Count - 4; i = i + 5)
			{
				foreach (int blacklistID in API.GlobalBotSettings.DestroyItemIds)
				{
					id = blacklistID / 20;
					if (id == (int)vendorNPCs[i + 3])
					{
						// match found... removing object.
						for (int j = 0; j < 5; j++)
						{
							vendorNPCs.RemoveAt(i);
						}
						i = i - 5;
					}
				}
			}
		}
		return vendorNPCs;
	}

	// Method:			"getFoodInInventory()"
	// What it Does:	This returns a List of Arrays, each array including first (position 0), the itemID in your bags that is found to be a match
	//					for the given zone.
	public static List<int[]> getFoodInInventory()
	{
		// Initialization check
		// If I have changed zones, re-initialize.
		if (API.Me.ZoneId != CurrentZoneID)
		{
			Initialize();
		}
		List<int[]> owned = new List<int[]>();
		// I need to initialize the bags...
		Inventory.Refresh();
		var Inv = Inventory.Items;

		int count = 0;
		foreach (var item in Inv)
		{
			for (int i = 0; i < FoodIDs.Count; i++)
			{
				if (FoodIDs[i] == item.ItemId)
				{
					int[] list = new int[2] { item.ItemId, item.StackCount };
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
		for (int i = 0; i < owned.Count; i++)
		{
			count2 = 0;
			numInstances = 0;
			for (int j = 0; j < copy.Count; j++)
			{
				if (copy[j][0] == owned[i][0])
				{
					count2 = count2 + copy[j][1];
					numInstances++;
					if (numInstances > 1 && numInstances < 3)
					{
						id = copy[j][0];
						numInstances = 0;
					}
				}
			}
			if (numInstances > 1)
			{
				for (int j = owned.Count - 1; j >= 0; j--)
				{
					if (numInstances > 1)
					{
						if (owned[j][0] == id)
						{
							owned.RemoveAt(j);
							numInstances--;
						}
					}
					else if (numInstances == 1)
					{
						if (owned[j][0] == id)
						{
							owned[j][1] = count2;
						}
					}
				}
			}
		}
		return owned;
	}

	public static List<int[]> getDrinkInInventory()
	{
		// Initialization check
		// If I have changed zones, re-initialize.
		if (API.Me.ZoneId != CurrentZoneID)
		{
			Initialize();
		}
		List<int[]> owned = new List<int[]>();
		// I need to initialize the bags...
		Inventory.Refresh();
		var Inv = Inventory.Items;

		int count = 0;
		foreach (var item in Inv)
		{
			for (int i = 0; i < DrinkIDs.Count; i++)
			{
				if (DrinkIDs[i] == item.ItemId)
				{
					int[] list = new int[2] { item.ItemId, item.StackCount };
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
		for (int i = 0; i < owned.Count; i++)
		{
			count2 = 0;
			numInstances = 0;
			for (int j = 0; j < copy.Count; j++)
			{
				if (copy[j][0] == owned[i][0])
				{
					count2 = count2 + copy[j][1];
					numInstances++;
					if (numInstances > 1 && numInstances < 3)
					{
						id = copy[j][0];
					}
				}
			}
			if (numInstances > 1)
			{
				for (int j = owned.Count - 1; j >= 0; j--)
				{
					if (numInstances > 1)
					{
						if (owned[j][0] == id)
						{
							owned.RemoveAt(j);
							numInstances--;
						}
					}
					else if (numInstances == 1)
					{
						if (owned[j][0] == id)
						{
							owned[j][1] = count2;
						}
					}
				}
			}
		}
		return owned;
	}

	public static bool IsFoodOrDrinkNeeded(int numMinimumFood, int numMinimumWater)
	{
		// Initialization check
		// If I have changed zones, re-initialize.
		if (API.Me.ZoneId != CurrentZoneID)
		{
			Initialize();
		}
		// Now, finding all food and drinks in my bags that much these known items to use, and counting how many in possession.
		int highestAmount = 0;
		foreach (int[] foodAmount in InventoryFood)
		{
			if (highestAmount < foodAmount[1])
			{
				highestAmount = foodAmount[1];
			}
		}

		if (highestAmount < numMinimumFood)
		{
			return true;
		}
		else if (API.Me.Class.ToString().Equals("Paladin") || API.Me.Class.ToString().Equals("Priest") || API.Me.Class.ToString().Equals("Shaman") || API.Me.Class.ToString().Equals("Mage") ||
				API.Me.Class.ToString().Equals("Warlock") || API.Me.Class.ToString().Equals("Druid") || API.Me.Class.ToString().Equals("Monk"))
		{
			int highestAmountWater = 0;
			foreach (int[] drinkAmount in InventoryWater)
			{
				if (highestAmountWater < drinkAmount[1])
				{
					highestAmountWater = drinkAmount[1];
				}
			}
			if (highestAmountWater < numMinimumWater)
			{
				return true;
			}
		}
		return false;
	}

	private static List<object> getVendorInfo(int TypeofMerchant)
	{
		List<object> vendor = new List<object>();
		int continentID = API.Me.ContinentID;
		int zoneID = API.Me.ZoneId;
		bool factionIsHorde = API.Me.IsHorde;

		// Draenor Continent
		if (continentID == 1116 || continentID == 1265)
		{
			vendor = DraenorMerchants.getMerchantInfo(zoneID, factionIsHorde, TypeofMerchant);
		}

		// All Continents Eventually to be Added
		return vendor;
	}

	public static List<int> getMerchantFoodList()
	{
		// Initialization check
		// If I have changed zones, re-initialize.
		if (API.Me.ZoneId != CurrentZoneID)
		{
			Initialize();
		}
		List<int> items = new List<int>();
		if (IsVendorOpen())
		{
			string itemID;
			string temp;
			int ID;
			for (int i = 1; i < API.ExecuteLua<int>("return GetMerchantNumItems()"); i++)
			{
				itemID = API.ExecuteLua<string>("return GetMerchantItemLink(" + i + ");");
				temp = itemID.Substring(itemID.IndexOf(':') + 1);
				itemID = itemID.Substring(itemID.IndexOf(':') + 1, temp.IndexOf(':'));
				ID = int.Parse(itemID);
				foreach (int foodItem in FoodIDs)
				{
					if (ID == foodItem)
					{
						items.Add(ID);
						break;
					}
				}
			}
		}
		else
		{
			API.Print("Error, Merchant Window Not Open.  Failure to Collect Merchant Trade info.");
			items.Add(0);
		}
		return items;
	}

	public static List<int> getMerchantDrinkList()
	{
		// Initialization check
		// If I have changed zones, re-initialize.
		if (API.Me.ZoneId != CurrentZoneID)
		{
			Initialize();
		}
		List<int> items = new List<int>();
		if (IsVendorOpen())
		{
			string itemID;
			string temp;
			int ID;
			for (int i = 1; i < API.ExecuteLua<int>("return GetMerchantNumItems()"); i++)
			{
				itemID = API.ExecuteLua<string>("return GetMerchantItemLink(" + i + ");");
				temp = itemID.Substring(itemID.IndexOf(':') + 1);
				itemID = itemID.Substring(itemID.IndexOf(':') + 1, temp.IndexOf(':'));
				ID = int.Parse(itemID);
				foreach (int drinkItem in DrinkIDs)
				{
					if (ID == drinkItem)
					{
						items.Add(ID);
						break;
					}
				}
			}
		}
		else
		{
			API.Print("Error, Merchant Window Not Open.  Failure to Collect Merchant Trade info.");
			items.Add(0);
		}
		return items;
	}

	public static IEnumerable<int> MoveToMerchant(List<object> merchantInfo)
	{
		// Initialization check
		// If I have changed zones, re-initialize.
		if (API.Me.ZoneId != CurrentZoneID)
		{
			Initialize();
		}
		// If Empty Result, Zone not known.
		if (merchantInfo.Count == 0)
		{
			API.Print("Unfortunately the Zone Your Are in Does Not Have the Necessary NPCs Mapped yet!");
			API.Print("It Would Be Amazing if You Could Report Back on the Forums This Issue! Thank you!");
			yield break;
		}

		// Casting all the Object to Types
		Vector3 destination = (Vector3)merchantInfo[0];
		float distance = (float)merchantInfo[1];
		distance = (int)Math.Ceiling(distance);
		int npcID = (int)merchantInfo[2];
		bool IsSpecialPathingNeeded = (bool)merchantInfo[3];

		string yards = "Yards";
		// String from plural to non. QoL thing only...
		if (distance == 1)
		{
			yards = "Yard";
		}
		API.Print("Traveling Roughly " + distance + " " + yards + " to Get to the Closest NPC...");

		// This is where to add special pathing considerations.
		if (IsSpecialPathingNeeded)
		{
			if (API.Me.ContinentID == 1116 || API.Me.ContinentID == 1265)
			{
				var check = new Fiber<int>(DraenorMerchants.doSpecialPathing());
				while (check.Run())
				{
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
		while (!API.MoveTo(destination))
		{
			yield return 100;
		}

		// Targeting the Merchant!
		foreach (var unit in API.Units)
		{
			if (unit.EntryID == npcID)
			{
				API.Me.SetFocus(unit);
				API.Me.SetTarget(unit);
				break;
			}
		}

		// Edging closer to the Merchant!
		// Dismounting First!
		API.Dismount();
		while (API.Me.Focus != null && !API.MoveTo(API.Me.Focus.Position))
		{
			yield return 100;
		}
	}

	public static List<object> GetClosestMerchant(int TypeofMerchant)
	{
		// Initialization check
		// If I have changed zones, re-initialize.
		if (API.Me.ZoneId != CurrentZoneID)
		{
			Initialize();
		}
		List<object> vendor = new List<object>();
		List<object> result = new List<object>();

		// Obtaining the List with the NPC info and location.
		// Also Blacklists any known problem NPCs
		vendor = RemoveBlacklistedVendors(getVendorInfo(TypeofMerchant));

		if (vendor.Count != 0)
		{
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
			for (int i = 0; i < vendor.Count - 4; i = i + 5)
			{
				// Setting first values
				position = new Vector3((float)vendor[i], (float)vendor[i + 1], (float)vendor[i + 2]);
				tempDistance = API.Me.Distance2DTo(position);
				// Changing values if new ones are closer
				if (tempDistance < closestDistance)
				{
					closestDistance = tempDistance;
					closestVector3 = position;
					npcID = (int)vendor[i + 3];
					IsSpecialPathingNeeded = (bool)vendor[i + 4];
				}
			}
			// Creating list with the Vector3 position of closest Merchant, and food list and water list on the end.
			List<object> final = new List<object>() { closestVector3, closestDistance, npcID, IsSpecialPathingNeeded };
			result.AddRange(final);
		}
		return result;
	}

	public static List<object> GetClosestMerchant(List<object> allVendors)
	{
		// Initialization check
		// If I have changed zones, re-initialize.
		if (API.Me.ZoneId != CurrentZoneID)
		{
			Initialize();
		}
		List<object> vendor = new List<object>();
		List<object> result = new List<object>();

		// Obtaining the List with the NPC info and location.
		// Also Blacklists any known problem NPCs
		vendor = allVendors;

		if (vendor.Count != 0)
		{
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
			for (int i = 0; i < vendor.Count - 4; i = i + 5)
			{
				// Setting first values
				position = new Vector3((float)vendor[i], (float)vendor[i + 1], (float)vendor[i + 2]);
				tempDistance = API.Me.Distance2DTo(position);
				// Changing values if new ones are closer
				if (tempDistance < closestDistance)
				{
					closestDistance = tempDistance;
					closestVector3 = position;
					npcID = (int)vendor[i + 3];
					IsSpecialPathingNeeded = (bool)vendor[i + 4];
				}
			}
			// Creating list with the Vector3 position of closest Merchant, and food list and water list on the end.
			List<object> final = new List<object>() { closestVector3, closestDistance, npcID, IsSpecialPathingNeeded };
			result.AddRange(final);
		}
		return result;
	}

	// Method:		"InteractWithMerchant()"
	public static IEnumerable<int> InteractWithMerchant()
	{
		if (API.Me.Focus != null)
		{
			while (!IsVendorOpen())
			{
				API.Me.Focus.Interact();
				yield return 1500;
				if (!IsVendorOpen())
				{
					MerchantGossip();
					yield return 2000;
				}
			}
		}
		yield break;
	}

	// Method:		    "IsVendorOpen()"
    // What it Does:    Returns true if the Vendor window is open and you are currently able to buy/selling
    // Purpose:         Many vendors have gossip options on interact.  This could be useful on interaction checking to ensure
    //                  the vendor window is open, and if it is not yet open, parsing through gossip options and selecting the correct one.
	public static bool IsVendorOpen()
	{
		return API.ExecuteLua<bool>("local name = GetMerchantItemInfo(1); if name ~= nil then return true else return false end;");
	}

	// Method:          "MerchantGossip()"
    // What it Does:    Interacts with the appropriate Gossip option on a vendor to open the buy/sell window.
    // Purpose:         If the player is at a Merchant (Refreshment, Vendor, or any generic merchant), at times on first interaction the Merchant may have various
    //                  gossip options rather than opening the buy/sell window immediately.  In these cases, it would be necessary to determine which gossip option would be the
    //                  correct one to open the vendor.  While most of the time it is the first option, I have found a few unique cases where this is not true, and as such
    //                  to encompass all possibilities, this is a universal method to coose the correct option always.
    //                  Example of when to use, after interacting with a merchant:  if (!IsVendorOpen) {MerchantGossip()};
	private static void MerchantGossip()
	{
		// Initializing Function
		string title = "title0";
		string luaCall = ("local title0,_ = GetGossipOptions(); if title0 ~= nil then return title0 else return \"nil\" end");
		string result = API.ExecuteLua<string>(luaCall);
		// Now Ready to Iterate through All Gossip Options!
		// The reason "1" is used instead of the conventional "0" is because Gossip options start at 1.
		int i = 1;
		string num = "";
		while (result != null)
		{
			if ((result.Equals("Let me browse your goods.") || result.Equals("I want to browse your goods.")) || (result.Equals("Deja que eche un vistazo a tus mercancías.") || result.Equals("Quiero ver tus mercancías.")) || (result.Equals("Deixe-me dar uma olhada nas suas mercadorias.") || result.Equals("Quero ver o que você tem à venda.")) || (result.Equals("Ich möchte ein wenig in Euren Waren stöbern.") || result.Equals("Ich sehe mich nur mal um.")) || (result.Equals("Deja que eche un vistazo a tus mercancías.") || result.Equals("Quiero ver tus mercancías.")) || (result.Equals("Permettez-moi de jeter un œil à vos biens.") || result.Equals("Je voudrais regarder vos articles.")) || (result.Equals("Fammi vedere la tua merce.") || result.Equals("Voglio dare un'occhiata alla tua merce.")) || (result.Equals("Позвольте взглянуть на ваши товары.") || result.Equals("Я хочу посмотреть на ваши товары.")) || (result.Equals("물건을 살펴보고 싶습니다.") || result.Equals("물건을 보고 싶습니다.")) || (result.Equals(" 讓我看看你出售的貨物。") || result.Equals("我想要看看你賣的貨物。")) || (result.Equals("让我看看你出售的货物。") || result.Equals("我想要看看你卖的货物。")))
			{
				API.ExecuteLua("SelectGossipOption(" + i + ");");
				break;
			}
			else
			{
				// This builds the new string to be added into an Lua API call.
				num = i.ToString();
				title = (title.Substring(0, title.Length - 1) + num);
				luaCall = ("local " + title + ",_," + luaCall.Substring(6, luaCall.Length - 6));
				result = API.ExecuteLua<string>(luaCall);
				i++;
			}
		}
	}

	public static void setUsableFood()
	{
		// Initialization check
		// If I have changed zones, re-initialize.
		if (API.Me.ZoneId != CurrentZoneID)
		{
			Initialize();
		}
		InventoryFood = getFoodInInventory();
		bool found;
		bool found2;
		for (int i = 0; i < InventoryFood.Count; i++)
		{
			found = false;
			foreach (int foodID in API.GlobalBotSettings.FoodItemIds)
			{
				if (foodID == InventoryFood[i][0])
				{
					found = true;
				}
			}
			if (!found)
			{
				API.GlobalBotSettings.FoodItemIds.Add(InventoryFood[i][0]);
			}
			// We should also add these to the "Protected" list to not sell.
			found2 = false;
			foreach (int protectedID in API.GlobalBotSettings.ProtectedItemIds)
			{
				if (protectedID == InventoryFood[i][0])
				{
					found2 = true;
				}
			}
			if (!found2)
			{
				API.GlobalBotSettings.ProtectedItemIds.Add(InventoryFood[i][0]);
			}
		}
	}

	public static void setUsableWater()
	{
		// Initialization check
		// If I have changed zones, re-initialize.
		if (API.Me.ZoneId != CurrentZoneID)
		{
			Initialize();
		}
		InventoryWater = getDrinkInInventory();
		bool found;
		bool found2;
		for (int i = 0; i < InventoryWater.Count; i++)
		{
			found = false;
			foreach (int drinkID in API.GlobalBotSettings.DrinkItemIds)
			{
				if (drinkID == InventoryWater[i][0])
				{
					found = true;
				}
			}
			if (found == false)
			{
				API.GlobalBotSettings.DrinkItemIds.Add(InventoryWater[i][0]);
			}
			// We should also add these to the "Protected" list to not sell.
			found2 = false;
			foreach (int protectedID in API.GlobalBotSettings.ProtectedItemIds)
			{
				if (protectedID == InventoryWater[i][0])
				{
					found2 = true;
				}
			}
			if (!found2)
			{
				API.GlobalBotSettings.ProtectedItemIds.Add(InventoryWater[i][0]);
			}
		}
	}
	// Method:		"BuyFood()"
	public static IEnumerable<int> BuyFood(int max)
	{
		// Initialization check
		// If I have changed zones, re-initialize.
		if (API.Me.ZoneId != CurrentZoneID)
		{
			Initialize();
		}
		int totalToBuy = MaxFoodToBuy(max);
		if (totalToBuy < 1)
		{
			totalToBuy = 0;
		}
		int BuyTwenty = totalToBuy / 20;
		int remainder = totalToBuy % 20;
		// parsing through vendor
		string itemID;
		string temp;
		int ID;
		for (int i = 1; i < API.ExecuteLua<int>("return GetMerchantNumItems()"); i++)
		{
			itemID = API.ExecuteLua<string>("return GetMerchantItemLink(" + i + ");");
			temp = itemID.Substring(itemID.IndexOf(':') + 1);
			itemID = itemID.Substring(itemID.IndexOf(':') + 1, temp.IndexOf(':'));
			ID = int.Parse(itemID);
			if (FoodIDToBuy == ID)
			{
				// j = Multiples of 20
				for (int j = 0; j < BuyTwenty; j++)
				{
					API.ExecuteLua("BuyMerchantItem(" + i + ", 20)");
					yield return 500;
				}
				if (remainder > 0)
				{
					API.ExecuteLua("BuyMerchantItem(" + i + "," + remainder + ")");
					yield return 500;
				}
				yield break;
			}
		}
		yield break;
	}

	private static int MaxFoodToBuy(int max)
	{
		List<int[]> allVendorFoodItems = new List<int[]>();
		vendorFood = getMerchantFoodList();
		int total;
		// Parse through vendor items, collect all items that match item in bags.
		foreach (int vendorItem in vendorFood)
		{
			foreach (int[] unit in InventoryFood)
			{
				if (unit[0] == vendorItem)
				{
					allVendorFoodItems.Add(unit);
					break;
				}
			}
		}
		// If at least 1 match
		if (allVendorFoodItems.Count > 0)
		{
			int highest = 0;
			foreach (int[] item in allVendorFoodItems)
			{
				if (highest < item[1])
				{
					FoodIDToBuy = item[0];
					highest = item[1];
				}
			}
			total = max - highest;
		}
		else if (vendorFood.Count > 0)
		{
			total = FoodCap;
			FoodIDToBuy = vendorFood[0];
		}
		else
		{
			API.Print("No Food Items Found on Vendor to Purchase. Blacklisting this NPC in the future!");
			if (API.Me.Focus != null)
			{
				BlacklistNPC(API.Me.Focus.EntryID);
			}
			total = 0;
		}
		return total;
	}

	private static int MaxDrinkToBuy(int max)
	{
		List<int[]> allVendorDrinkItems = new List<int[]>();
		vendorDrinks = getMerchantDrinkList();
		int total;
		// Parse through vendor items, collect all items that match item in bags.
		foreach (int drinks in vendorDrinks)
		{
			foreach (int[] unit in InventoryWater)
			{
				if (unit[0] == drinks)
				{
					allVendorDrinkItems.Add(unit);
					break;
				}
			}
		}
		// If at least 1 match
		if (allVendorDrinkItems.Count > 0)
		{
			int highest = 0;
			foreach (int[] item in allVendorDrinkItems)
			{
				if (highest < item[1])
				{
					DrinkIDToBuy = item[0];
					highest = item[1];
				}
			}
			total = max - highest;
		}
		else if (vendorDrinks.Count > 0)
		{
			total = DrinkCap;
			DrinkIDToBuy = vendorDrinks[0];
		}
		else
		{
			API.Print("No Drink Items Found on Vendor to Purchase. Blacklisting this NPC in the future!");
			if (API.Me.Focus != null)
			{
				BlacklistNPC(API.Me.Focus.EntryID);
			}
			total = 0;
		}
		return total;
	}

	// Method:		"BuyDrink()"
	public static IEnumerable<int> BuyDrink(int max)
	{
		// Initialization check
		// If I have changed zones, re-initialize.
		if (API.Me.ZoneId != CurrentZoneID)
		{
			Initialize();
		}
		int totalToBuy = MaxDrinkToBuy(max);
		if (totalToBuy != 0)
		{
			int BuyTwenty = totalToBuy / 20;
			int remainder = totalToBuy % 20;
			// parsing through vendor
			string itemID;
			string temp;
			int ID;
			for (int i = 1; i < API.ExecuteLua<int>("return GetMerchantNumItems()"); i++)
			{
				itemID = API.ExecuteLua<string>("return GetMerchantItemLink(" + i + ");");
				temp = itemID.Substring(itemID.IndexOf(':') + 1);
				itemID = itemID.Substring(itemID.IndexOf(':') + 1, temp.IndexOf(':'));
				ID = int.Parse(itemID);
				if (DrinkIDToBuy == ID)
				{
					// j = Multiples of 20
					for (int j = 0; j < BuyTwenty; j++)
					{
						API.ExecuteLua("BuyMerchantItem(" + i + ", 20)");
						yield return 500;
					}
					if (remainder > 0)
					{
						API.ExecuteLua("BuyMerchantItem(" + i + "," + remainder + ")");
						yield return 500;
					}
					yield break;
				}
			}
		}
		yield break;
	}

	public static IEnumerable<int> RestingCheck()
	{
		// Initialization check
		// If I have changed zones, re-initialize.
		if (API.Me.ZoneId != CurrentZoneID)
		{
			Initialize();
		}
		// Initialize the Script and pulls all info from appropriate pathing
		// Determins if necessary to start resting Script.
		if (IsFoodOrDrinkNeeded(MinFood, MinWater))
		{
			List<object> closest = GetClosestMerchant(1);
			if (closest.Count > 0)
			{
				API.Print("Player is Low on Refreshments! Heading to Restock!!!");
				// Identifying Merchant and moving to it.
				var check = new Fiber<int>(MoveToMerchant(closest));
				while (check.Run())
				{
					yield return 100;
				}

				// Interacting with Merchant
				var check2 = new Fiber<int>(InteractWithMerchant());
				while (check2.Run())
				{
					yield return 100;
				}
				// Buy Water if Needed
				if (API.Me.Class.ToString().Equals("Paladin") || API.Me.Class.ToString().Equals("Priest") || API.Me.Class.ToString().Equals("Shaman") || API.Me.Class.ToString().Equals("Mage") ||
				API.Me.Class.ToString().Equals("Warlock") || API.Me.Class.ToString().Equals("Druid") || API.Me.Class.ToString().Equals("Monk"))
				{
					var check3 = new Fiber<int>(BuyDrink(DrinkCap));
					while (check3.Run())
					{
						yield return 100;
					}
				}
				// Buying food
				var check4 = new Fiber<int>(BuyFood(FoodCap));
				while (check4.Run())
				{
					yield return 100;
				}
				API.Print("Refreshments Replenished!");
				// Updating Food and Water Lists
				setUsableFood();
				setUsableWater();

				// And, since we are at the vendor already, let's clear some loot.
				SellLootedItems();
				
				// Destroy any BoP toys in bags that have no vendor value that player already knows.
				DestroyKnownToys();

				// And, since we are already in town, let's see if it's worth the effort to repair.
				// Quick repair if at vendor
				Repair();
				if (IsRepairVendorNearby() && getDurability() <= 90 || NumItemsBroken() > 0)
				{
					var check5 = new Fiber<int>(RepairCheck());
					while (check5.Run())
					{
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
	/////   ALL SELLING/REPAIR METHODS    /////
	/////								  /////
	///////////////////////////////////////////

	public static void Repair()
	{
		if (IsVendorOpen() && API.ExecuteLua<bool>("return CanMerchantRepair()"))
		{
			API.ExecuteLua("RepairAllItems()");
		}
	}

	// Method		"IsRepairNeeded()"
	//				20% or less equals
	public static bool IsRepairNeeded()
	{
		if (NumItemsBroken() > 0 || getDurability() <= MinDurability)
		{
			return true;
		}
		return false;
	}

	public static List<int> getItemsToSell()
	{
		// Initialization check
		// If I have changed zones, re-initialize.
		if (API.Me.ZoneId != CurrentZoneID)
		{
			Initialize();
		}
		List<int> itemsToSell = new List<int>();

		// Gathering Looted Items to the Sell List
		bool toAdd;
		foreach (int toSellID in API.GlobalBotSettings.Sell_LootedItems)
		{
			toAdd = true;
			// Verifying no protected items in list, and if so, removing.
			foreach (int protectedID in API.GlobalBotSettings.ProtectedItemIds)
			{
				if (protectedID == toSellID)
				{
					toAdd = false;
				}
			}
			// Adding it to sell list.
			if (toAdd == true)
			{
				itemsToSell.Add(toSellID);
			}
		}
		
		// Gathering all inventory items that are Soulbound and not a gear item
		Inventory.Refresh();
		int vendorPrice;
		bool soulBound;
		bool isEquipable;
		string quality;
		var ItemList = Inventory.Items;
		foreach (var item in ItemList)
		{
			vendorPrice = item.ItemInfo.VendorPrice;
			soulBound = IsItemSoulbound(item.ItemId);
			isEquipable = item.ItemInfo.IsEquipable;
			quality = item.ItemInfo.Quality.ToString();
			if (soulBound && vendorPrice != 0 && !isEquipable && (quality.Equals("Green") || quality.Equals("Blue")))
			{
				// Ok, now before adding it to the SELL LIST, let's make sure it's not protected.
				toAdd = true;
				foreach (int protectedID in API.GlobalBotSettings.ProtectedItemIds)
				{
					if (protectedID == item.ItemId)
					{
						toAdd = false;
					}
				}
				// Adding it to sell list.
				if (toAdd == true)
				{
					foreach (int id in itemsToSell) 
					{
						// ensuring no duplicates.
						if (id == item.ItemId)
						{
							toAdd = false;
						}
					}
					if (toAdd == true)
					{
						itemsToSell.Add(item.ItemId);
					}
				}
			}
		}
		
		// Gathering all inventory items that are soulbound, a gear item, 5 levels or more below current player level. 
		bool itemIsTooWeak;
		int minItemLevel;
		foreach (var item in ItemList)
		{
			vendorPrice = item.ItemInfo.VendorPrice;
			soulBound = IsItemSoulbound(item.ItemId);
			isEquipable = item.ItemInfo.IsEquipable;
			minItemLevel = GetItemMinLevel(item.ItemId);
			quality = item.ItemInfo.Quality.ToString();
			if ((API.Me.Level - 3) > minItemLevel && minItemLevel != 0)
			{
				itemIsTooWeak = true;
			}
			else
			{
				itemIsTooWeak = false;
			}
			if (soulBound && vendorPrice != 0 && isEquipable && itemIsTooWeak && (quality.Equals("Green") || quality.Equals("Blue")))
			{
				itemsToSell.Add(item.ItemId);
			}
		}
		// Note: Since these lists are pulled directly from Rebot, it auto-filters duplicates already
		// so we do not need to check for duplicates and filter them here.
		return itemsToSell;
	}
	
	public static void DestroyKnownToys()
	{
		Inventory.Refresh();
		int vendorPrice;
		bool soulBound;
		bool isToy;
		bool playerHasToy;
		var ItemList = Inventory.Items;
		// Items to Destroy - Toys that are soulbound, already known, and not vendorable.
		foreach (var item in ItemList)
		{
			// Gear items that have no value
			vendorPrice = item.ItemInfo.VendorPrice;
			soulBound = IsItemSoulbound(item.ItemId);
			isToy = IsItemAToy(item.ItemId);
			playerHasToy = API.ExecuteLua<bool>("return PlayerHasToy(" + item.ItemId + ")");
			if (soulBound && vendorPrice == 0 && isToy && playerHasToy)
			{
				// Destroy the item...
				API.Print("Destroying the Following Item:  " + item.Name);
				API.DeleteItem(item.ItemId);
			}
		}
	}

	// Method:		"SellLootedItems(List<int>)"
	public static void SellLootedItems()
	{
		// Initialization check
		// If I have changed zones, re-initialize.
		if (API.Me.ZoneId != CurrentZoneID)
		{
			Initialize();
		}
		// Let's count how many Grey Items to Sell
		int numGrey = API.ExecuteLua<int>("local count = 0; for bag = 0,4,1 do for slot = 1, GetContainerNumSlots(bag), 1 do local name = GetContainerItemLink(bag,slot); if name and string.find(name,\"ff9d9d9d\") then count = count + 1; end; end; end return count");
		// For future use to sell specific "types" of gear.
		List<int> itemsToSell = getItemsToSell();
		if (!IgnoreVendor)
		{
			int potentialFreeSlots = API.GetFreeBagSlots() + itemsToSell.Count + numGrey;
			if (potentialFreeSlots < MinFreeSlots)
			{
				IgnoreVendor = true;
			}
		}

		if (IsVendorOpen())
		{
			if (numGrey > 0)
			{
				API.Print("Selling:   " + numGrey + " Gray Items...");
			}
			// Spams Macro til all greys sold.
			while (numGrey > 0)
			{
				API.ExecuteLua("for bag = 0,4,1 do for slot = 1, GetContainerNumSlots(bag), 1 do local name = GetContainerItemLink(bag,slot); if name and string.find(name,\"ff9d9d9d\") then DEFAULT_CHAT_FRAME:AddMessage(\"Selling \"..name); UseContainerItem(bag,slot) end; end; end");
				numGrey = API.ExecuteLua<int>("local count = 0; for bag = 0,4,1 do for slot = 1, GetContainerNumSlots(bag), 1 do local name = GetContainerItemLink(bag,slot); if name and string.find(name,\"ff9d9d9d\") then count = count + 1; end; end; end return count");
			}
			
			// Sell All "Looted Items"
			bool found;
			string itemName;
			foreach (int itemID in itemsToSell)
			{
				found = false;
				found = API.ExecuteLua<bool>("local match = false; for i=0,4 do for j=1,GetContainerNumSlots(i)do local n=GetContainerItemID(i,j)if n == " + itemID + " then local s=GetContainerItemLink(i,j)print(\"Selling \"..s)match = true; end end end return match");
				if (found)
				{
					// Action to sell item.
					itemName = API.ExecuteLua<string>("local name = GetItemInfo(" + itemID + "); return name;");
					API.Print("Selling:   " + itemName);
					API.UseItem(itemID);
				}
			}
			// Let's clear the list of stored items to sell.
			API.GlobalBotSettings.Sell_LootedItems.Clear();
		}
		else
		{
			API.Print("Player Failed to Sell goods. For Some Reason, Interaction with Vendor Failed.");
			API.Print("Please Report this On The Forums So We Can Fix it!");
		}
	}

	// Method:		"getDurability()"
	// Purpose:		Returns the % of character durability.
	public static int getDurability()
	{
		int count = 9;
		float durability = 0;
		durability += API.ExecuteLua<float>("local durability,max = GetInventoryItemDurability(1) if durability ~= nil then local result = durability/max; return result else return 1.0 end");
		durability += API.ExecuteLua<float>("local durability,max = GetInventoryItemDurability(3) if durability ~= nil then local result = durability/max; return result else return 1.0 end");
		for (int i = 5; i <= 10; i++)
		{
			durability += API.ExecuteLua<float>("local durability,max = GetInventoryItemDurability(" + i + ") if durability ~= nil then local result = durability/max; return result else return 1.0 end");
		}
		durability += API.ExecuteLua<float>("local durability,max = GetInventoryItemDurability(16) if durability ~= nil then local result = durability/max; return result else return 1.0 end");

		if (API.ExecuteLua<bool>("local durability = GetInventoryItemDurability(17); if durability ~= nil then return true else return false end"))
		{
			durability += API.ExecuteLua<float>("local durability,max = GetInventoryItemDurability(17) local result = durability/max; return result");
			count++;
		}
		durability = durability / count * 100;
		return (int)durability;
	}

	// Method		"NumItemsBroken()"
	// What it Does:    Returns if player has any Broken ("RED") items equipped.
    // Purpose:         Could be useful if at a vendor to institute a brief repair.
	public static int NumItemsBroken()
	{
		int count = 0;
		if (API.ExecuteLua<float>("local durability,max = GetInventoryItemDurability(1) if durability ~= nil then local result = durability/max; return result else return 1.0 end") < 0.1f)
		{
			count++;
		}
		if (API.ExecuteLua<float>("local durability,max = GetInventoryItemDurability(3) if durability ~= nil then local result = durability/max; return result else return 1.0 end") < 0.1f)
		{
			count++;
		}
		for (int i = 5; i <= 10; i++)
		{
			if (API.ExecuteLua<float>("local durability,max = GetInventoryItemDurability(" + i + ") if durability ~= nil then local result = durability/max; return result else return 1.0 end") < 0.1f)
			{
				count++;
			}
		}
		if (API.ExecuteLua<float>("local durability,max = GetInventoryItemDurability(16) if durability ~= nil then local result = durability/max; return result else return 1.0 end") < 0.1f)
		{
			count++;
		}
		if (API.ExecuteLua<bool>("local durability = GetInventoryItemDurability(17); if durability ~= nil then return true else return false end"))
		{
			if (API.ExecuteLua<float>("local durability,max = GetInventoryItemDurability(17) local result = durability/max; return result") < 0.1f)
			{
				count++;
			}
		}
		return count;
	}

	// Method:          "IsRepairVendorNearby()"
    // What it Does:    Returns true if a player is within 100 yards of a Repair vendor
    // Purpose:         Could be a useful check if player wanted to search for a vendor if passing through town
	public static bool IsRepairVendorNearby()
	{
		foreach (var unit in API.Units)
		{
			if (!unit.IsDead && unit.HasFlag(UnitNPCFlags.CanRepair))
			{
				return true;
			}
		}
		return false;
	}

	// Method:          "IsFoodVendorNearby()"
    // What it Does:    Returns true if a player is within 100 yards of a food vendor
    // Purpose:         Could be a useful check if player wanted to search for a vendor if passing through town
	public static bool IsFoodVendorNearby()
	{
		foreach (var unit in API.Units)
		{
			if (!unit.IsDead && unit.HasFlag(UnitNPCFlags.SellsFood))
			{
				return true;
			}
		}
		return false;
	}

	// Method:			"HasRepairMount();
	// Purpose:			return true if you have any of the repair mounts.
	public static bool HasRepairMount()
	{
		// Traveler's Tundra or
		if (API.HasMount(61447) || API.HasMount(122708))
		{
			return true;
		}
		return false;
	}

	// Method:			"UseRepairMount()"
	// Purpose:			To save time it uses either the Traveler's Tundra Mount or the Grand Expedition Yak
	//					to access a vendor.
	public static void UseRepairMount()
	{
		int tundra = 61447;
		int yak = 122708;
		if (API.ExecuteLua<bool>("return IsOutdoors();"))
		{
			if (API.HasMount(tundra))
			{
				API.Print("Player is Using the Traveler's Tundra to Access Vendor!");
				// No need to use if already on the mounts.
				if (!API.HasAura(tundra))
				{
					API.SummonMount(tundra);
				}

			}
			else if (API.HasMount(yak))
			{
				API.Print("Player is Using the Grand Expedition Yak to Access Vendor!");
				if (!API.HasAura(yak))
				{
					API.SummonMount(yak);
				}
			}
		}
		else
		{
			API.Print("Player Wanted to Use their Repair Mount but They Were Indoors.");
		}
	}

	// Method:		"RepairCheck()"
	// Purpose:		This is the main method to be run on a recursive loop, ideally, or within something
	public static IEnumerable<int> RepairCheck()
	{
		// Initialization check
		// If I have changed zones, re-initialize.
		if (API.Me.ZoneId != CurrentZoneID)
		{
			Initialize();
		}
		if (IsRepairNeeded())
		{
			API.Print("test");
			// The number 2 returns a list of known Repair Vendors, and their locations.
			List<object> closest = GetClosestMerchant(2);
			if (closest.Count > 0 || (HasRepairMount() && API.ExecuteLua<bool>("return IsOutdoors();")))
			{
				if (HasRepairMount())
				{
					yield return 1000; // 1 second delay to stop player from moving.
					UseRepairMount();
					yield return 2000; // Delay for vendors to Appears.
										// Identifying Vendor for Chosen Mount
					int unitID = 0;
					if (API.HasAura(61447))
					{
						unitID = 32641;
					}
					else if (API.HasAura(122708))
					{
						unitID = 62822;
					}
					// Setting focus to the vendor.
					foreach (var unit in API.Units)
					{
						if (unit.EntryID == unitID)
						{
							API.Me.SetFocus(unit);
							API.Me.SetTarget(unit);
							break;
						}
					}
				}
				else
				{
					API.Print("Player is in Need of Repair.  Heading to nearest Vendor!");
					// Identifying Merchant and moving to it.
					var check = new Fiber<int>(MoveToMerchant(closest));
					while (check.Run())
					{
						yield return 100;
					}
				}

				// Interacting with Merchant
				var check2 = new Fiber<int>(InteractWithMerchant());
				while (check2.Run())
				{
					yield return 100;
				}

				// Clearing our Loot first
				SellLootedItems();
				
				// Destroy any BoP toys in bags that have no vendor value that player already knows.
				DestroyKnownToys();
				
				// Repairing
				Repair();

				API.ExecuteLua("CloseMerchant()");
			}
		}
		yield break;
	}


	///////////////////////////////////////////
	/////							      /////
	/////     All Methods 4 Vendoring     /////
	/////								  /////
	///////////////////////////////////////////


	// Method:				"InventoryIsFull"
	// Purpose:				Returns true if the number of free slots in your bag is equal to or less Thank
	//						the given minNumFreeSlots
	public static bool InventoryIsFull()
	{
		if (API.GetFreeBagSlots() >= MinFreeSlots)
		{
			return false;
		}
		return true;
	}

	// Method:			"getAllVendors()"
	// Purpose:			Returns all vendors, Repair, food, and generic into one List of Objects
	public static List<object> getAllVendors()
	{
		// Initialization check
		// If I have changed zones, re-initialize.
		if (API.Me.ZoneId != CurrentZoneID)
		{
			Initialize();
		}
		List<object> allOfEm = new List<object>();

		// Combine all possible vendor types
		allOfEm = getVendorInfo(1);
		allOfEm.AddRange(getVendorInfo(2));
		allOfEm.AddRange(getVendorInfo(3));

		// Remove any duplicates
		int numInstances;
		int id = 0;
		List<object> copy = allOfEm;
		if (allOfEm.Count > 0)
		{
			for (int i = 0; i < allOfEm.Count - 4; i = i + 5)
			{
				numInstances = 0;
				for (int j = 0; j < copy.Count - 4; j = j + 5)
				{
					if ((int)allOfEm[i + 3] == (int)copy[j + 3])
					{
						// match found, removing
						numInstances++;
					}
					if (numInstances > 1 && numInstances < 3)
					{
						id = (int)allOfEm[j + 3];
						numInstances = 0;
					}
				}
				if (numInstances > 1)
				{
					for (int j = allOfEm.Count - 5; j >= 4; j = j - 5)
					{
						
						if (numInstances > 1)
						{
							if ((int)allOfEm[j + 3] == id)
							{
								for (int k = 0; k < 5; k++)
								{
									allOfEm.RemoveAt(j);
								}
								numInstances--;
							}
						}
					}
				}
			}
		}
		return allOfEm;
	}

	// Method:		"PlayerShouldStopVendoring()";
	// Purpose		To create a boolean check to return true of the player is no longer able to clear out his bags, 
	//				to ignore VendorCheck action.
	public static bool PlayerShouldStopVendoring()
	{
		if (!IgnoreVendor)
		{
			int potentialFreeSlots = API.GetFreeBagSlots() + getItemsToSell().Count;
			if (potentialFreeSlots < MinFreeSlots)
			{
				IgnoreVendor = true;
				return true;
			}
		}
		else
		{
			return true;
		}
		return false;
	}

	// Method:			"InventoryCheck()"
	// Purpose:			This is the main implementation of the Inventory check logic.
	public static IEnumerable<int> InventoryCheck()
	{
		// Initialization check
		// If I have changed zones, re-initialize.
		if (API.Me.ZoneId != CurrentZoneID)
		{
			Initialize();
		}
		if (InventoryIsFull())
		{
			// The number 2 returns a list of known Repair Vendors, and their locations.
			List<object> closest = GetClosestMerchant(getAllVendors());
			if (closest.Count > 0 || (HasRepairMount() && API.ExecuteLua<bool>("return IsOutdoors();")))
			{
				if (HasRepairMount())
				{
					yield return 1000; // 1 second delay to stop player from moving.
					UseRepairMount();
					yield return 2000; // Delay for vendors to Appears.
										// Identifying Vendor for Chosen Mount
					int unitID = 0;
					// Tundra NPC
					if (API.HasAura(61447))
					{
						unitID = 32641;
					}
					// YAK NPC
					else if (API.HasAura(122708))
					{
						unitID = 62822;
					}
					// Setting focus to the vendor.
					foreach (var unit in API.Units)
					{
						if (unit.EntryID == unitID)
						{
							API.Me.SetFocus(unit);
							API.Me.SetTarget(unit);
							break;
						}
					}
				}
				else
				{
					API.Print("Player's Inventory is Full! Heading to nearest Vendor!");
					// Identifying Merchant and moving to it.
					var check = new Fiber<int>(MoveToMerchant(closest));
					while (check.Run())
					{
						yield return 100;
					}
				}

				// Interacting with Merchant
				var check2 = new Fiber<int>(InteractWithMerchant());
				while (check2.Run())
				{
					yield return 100;
				}

				// Clearing our Loot first
				SellLootedItems();
				
				// Destroy any BoP toys in bags that have no vendor value that player already knows.
				DestroyKnownToys();
				
				// Repairing
				Repair();

				API.ExecuteLua("CloseMerchant()");
			}
		}
		yield break;
	}
	
	// Method:          "IsItemSoulbound(int)"
    // What it Does:    Returns whether the given item in a player bags is SoulBound.  If the given item is not, or not owned, it returns false
    // Purpose:         Mainly for vendor filtering.  If item is SoulBound, not equippable, and has a vendor value > 0, it might be worth selling.
	public static bool IsItemSoulbound(int ID)
	{
		bool result;
		int container = -1;
		int slot = -1;
		// parsing through inventory items to match ID
		foreach (var item in Inventory.Items)
		{
			if (item.ItemId == ID)
			{
				container = item.ContainerId; // Establishing bag position
				slot = item.SlotId;
			}
		}
		
		// Parsing tooltip for Soulbound info
		if (container >= 0)
		{
			result = API.ExecuteLua<bool>("local tooltip; local function create() local tip, leftside = CreateFrame(\"GameTooltip\"), {} for i = 1, 3 do local L,R = tip:CreateFontString(), tip:CreateFontString() L:SetFontObject(GameFontNormal) R:SetFontObject(GameFontNormal) tip:AddFontStrings(L,R) leftside[i] = L end tip.leftside = leftside return tip end; local function Is_Soulbound(bag, slot) tooltip = tooltip or create() tooltip:SetOwner(UIParent,\"ANCHOR_NONE\") tooltip:ClearLines() tooltip:SetBagItem(bag, slot) local s = tooltip.leftside[2]:GetText() local t = tooltip.leftside[3]:GetText() tooltip:Hide() if (s == ITEM_SOULBOUND or t == ITEM_SOULBOUND) then return true; else return false; end end return Is_Soulbound(" + container + "," + slot + ")");
		}
		else
		{
			result = false;
		}
		return result;
	}
	
	public static int GetItemMinLevel(int ID)
	{
		return API.ExecuteLua<int>("local _,_,_,_,itemMinLevel = GetItemInfo(" + ID + "); return itemMinLevel;");
	}
	
	public static bool IsItemAToy(int ID)
	{
		bool result;
		int container = -1;
		int slot = -1;
		// parsing through inventory items to match ID
		foreach (var item in Inventory.Items)
		{
			if (item.ItemId == ID)
			{
				container = item.ContainerId; // Establishing bag position
				slot = item.SlotId;
			}
		}
		
		// Parsing tooltip for Soulbound info
		if (container >= 0)
		{
			result = API.ExecuteLua<bool>("local tooltip; local function create() local tip, leftside = CreateFrame(\"GameTooltip\"), {} for i = 1, 5 do local L,R = tip:CreateFontString(), tip:CreateFontString() L:SetFontObject(GameFontNormal) R:SetFontObject(GameFontNormal) tip:AddFontStrings(L,R) leftside[i] = L end tip.leftside = leftside return tip end; local function Is_Toy(bag, slot) tooltip = tooltip or create() tooltip:SetOwner(UIParent,\"ANCHOR_NONE\") tooltip:ClearLines() tooltip:SetBagItem(bag, slot) local s = tooltip.leftside[2]:GetText() local t = tooltip.leftside[3]:GetText() u = tooltip.leftside[4]:GetText() tooltip:Hide() if (s == TOY or t == TOY or u == TOY) then return true; else return false; end end return Is_Toy(" + container + "," + slot + ")");
		}
		else
		{
			result = false;
		}
		return result;
	}
}


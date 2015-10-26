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
    
    // Default Constructor
    public Merchant() {}
	
	// Method:		"BuyFood()"
	public static void BuyFood() {
		
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
			closestDistance = QH.API.Me.Distance2DTo(closestVector3);
			npcID = (int)vendor[3];
			IsSpecialPathingNeeded = (bool)vendor[4];
			
	
			// Filtering for Closest Merchant now.
			for (int i = 0; i < vendor.Count - 4; i = i + 5)
			{
				position = new Vector3((float)vendor[i], (float)vendor[i + 1], (float)vendor[i + 2]);
				tempDistance = QH.API.Me.Distance2DTo(position);
				if (tempDistance < closestDistance)
				{
					closestDistance = tempDistance;
					closestVector3 = position;
					closestZone = (string)vendor[i];
					npcID = (int)vendor[i+3];
					IsSpecialPathingNeeded = (bool)vendor[i+4];
				}
			}
			 // Creating list with the Vector3 position of closest Merchant
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
        return result;
	}
	
	// Method:		"GoToMerchant()"
	//				Type of Merhant:  1 = Food and Drink; 2 = Repair
	public static IEnumberable<int> GoToMerchant(int TypeOfMerchant) {
		yield break;
	}
	
	public static void InteractWithMerchant() {
		
	}
	
	// Method:		"IsVendorOpen()"
	public static bool IsVendorOpen() {
		
	}
	
	public static bool IsFoodNeeded() {
		QH.API.Print("Player Needs to Purchase Some Resting Refreshments!")
	}
	
	public static bool IsRepairNeeded() {
		QH.API.Print("Player is in Need of Repair.  Heading to nearest Vendor!");
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
            
            // Add connections to other Classes There...
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
        foreach (var unit in QH.API.Units)
        {
            if (unit.EntryID == npcID)
            {
                QH.API.Me.SetFocus(unit);
                QH.API.Me.SetTarget(unit);
                break;
            }
        }
        
        // Edging closer to the Merchant!
        while(QH.API.Me.Focus != null && !QH.API.MoveTo(QH.API.Me.Focus.Position))
        {
            yield return 100;
        }
	}
	
	public static void Repair() {
		
	}
	
	public static IEnumberable<int> RepairCheck() {
		
	}
	
	public static IEnumberable<int> RestingCheck(){
		List<object> result = new List<object>();
		if (IsFoodNeeded()) {
			// Identifying Merchant and moving to it.
			var check = new Fiber<int>(MoveToMerchant(GetClosestMerchant(1)));
			while(check.Run()) {
				yield return 100;
			}
			
			
		}
		yield break;
	}
	
	
}
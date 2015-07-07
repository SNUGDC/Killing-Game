namespace KillingGame.CrimeScene
{
	public struct Condition
	{
		public string name;
		public bool isActive;
	}
	public class ObjectInfo
	{
		public string name;
		public bool isActive;		
		public class Interaction
		{
			public string name;
			public bool isActive;
			public float timeConsumption;
			public Condition[] activeConditions;
			public Condition[] results;
			
			public void checkActive()
			{
				
			}
		}
		public Interaction[] interactions;
		public Condition[] activeConditions;
		
		public void checkActive()
		{
			
		}
		public void showActiveInteractions()
		{
			
		}
	}
	
	public class StageData
	{
		ObjectInfo[] stageObjects;
	}
}
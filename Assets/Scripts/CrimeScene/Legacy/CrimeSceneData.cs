namespace KillingGame.CrimeScene
{
	public struct Condition
	{
		public string name;
		public bool isActive;
	}
	public abstract class InteractObject
	{
		public string name;
		public bool isActive;
		public Condition[] activeConditions;
		public void checkActive()
		{
			
		}
	}
	public class Interaction : InteractObject
	{
		public float timeConsumption;
		public Condition[] results;
		public void execute()
		{
			
		}
	}
	public class ObjectInfo
	{

		public Interaction[] interactions;
		public void showActiveInteractions()
		{
			
		}
	}
	
	public class StageData
	{
		ObjectInfo[] stageObjects;
	}
}
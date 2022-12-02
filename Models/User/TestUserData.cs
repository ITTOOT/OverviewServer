namespace OverviewServer.Models.User
{
	public class TestUserData
	{
		//TO DO
		public string? Timestamp { get; set; }
		public int? Code { get; set; }

		public TestUserData()
		{
			this.Timestamp = DateTime.Now.ToString();
			this.Code = 1234;
		}
	}
}

using RockwellAdapter;
using static RockwellAdapter.Connections;

namespace OverviewServer.Services
{
	public class RockwellConnectionService
	{
		//Get messages from storage
		public async Task<List<List<bool>>> Getconnection()
		{
			//Connecton to PLC
			List<Connection> connectionList = data();
			GetTags getTags = new GetTags();
			//OP40
			var bitTagsLists = await getTags.BoolTags(connectionList[0]);

			return bitTagsLists;
		}
	}
}

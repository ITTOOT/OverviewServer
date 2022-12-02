using LinqFileParser;
using Newtonsoft.Json.Linq;
using NuGet.Packaging.Signing;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using static OverviewServer.Services.Connections;

namespace OverviewServer.Services
{
    public class EquipmentMessagesService
    {
        //Get messages from storage
        public async Task<EquipmentMessage> GetMessage(DateTime timestamp, int messageId, string path)
        {
            ////Connecton to PLC
            //List<StandardPath> pathList = dataPath();

            //Timestamp
            //List<EquipmentMessage> message = new List<EquipmentMessage>();
            EquipmentMessage message = new EquipmentMessage();
            timestamp = timestamp.ToLocalTime();

            ////Path to messages
            //FilePaths filePaths = new FilePaths();
            //var PATHS = filePaths.PathData(pathList);

            //Get file
            StorageToList storageToList = new StorageToList();
            ////var messages = await storageToList.GetFromCsv(PATHS[0]);
            //List<IEnumerable<MessageData>> messagesList = new List<IEnumerable<MessageData>>();
            //foreach (var path in PATHS)
            //{
            //    messagesList.Add(await storageToList.GetFromCsv(path));
            //}
            var messages = await storageToList.GetFromCsv(path);
            //Query the messages
            LinqExecutor linqExecutor = new LinqExecutor();
            //foreach (var messages in messagesList)
            //{
            //    message.Add(linqExecutor.StationMessage(messages, messageId));
            //}
            //message = message.Select(x => { x.Timestamp = timestamp.ToLongTimeString(); return x; }).ToList();

            message = linqExecutor.StationMessage(messages, messageId);
            message.Timestamp = timestamp.ToLongTimeString();

            return message;
        }

        //Get messages from storage
        public async Task<List<EquipmentMessage>> GetMessages(DateTime timestamp, List<int> messageIds, string path)
        {
            //Timestamp
            //List<EquipmentMessage> message = new List<EquipmentMessage>();
            List<EquipmentMessage> message = new List<EquipmentMessage>();
            timestamp = timestamp.ToLocalTime();

            //Get file
            StorageToList storageToList = new StorageToList();
            var messages = await storageToList.GetFromCsv(path);

            //Query the messages
            LinqExecutor linqExecutor = new LinqExecutor();
            foreach (var id in messageIds)
            {
                message.Add(linqExecutor.StationMessage(messages, id));
            }
            message = message.Select(x => { x.Timestamp = timestamp.ToLongTimeString(); return x; }).ToList();

            return message;
        }
    }
}

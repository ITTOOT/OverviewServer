using LinqFileParser;
using OverviewServer.Utilities;
using static OverviewServer.Services.Connections;

namespace OverviewServer.Services.Background.Workers
{
    //Background task - implementation returns a Task that represents the
    //entire lifetime of the background service.  No further services are
    //started until ExecuteAsync becomes asynchronous.
    public class RockwellDataService : BackgroundService
    {
        //Dec
        private readonly ILogger<RockwellDataService> _logger;
        public List<List<EquipmentMessage>> messages = new List<List<EquipmentMessage>>();
        private EquipmentMessage errMessage = new EquipmentMessage();
        public List<List<string>> positions = new List<List<string>>();
        private List<int> _positions = new List<int>();
        private List<List<EquipmentMessage>>? _messages = new List<List<EquipmentMessage>>();

        public RockwellDataService(ILogger<RockwellDataService> logger)
        {
            _logger = logger;
        }

        // IMPORTANT: `await`.
        // Otherwise, current method would continue before Task.Run completes. 
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //Dec
            bool started = false;

            while (!stoppingToken.IsCancellationRequested)
            {
                //Info
                if (!started)
                    _logger.LogInformation("Background Services are starting...");
                started = true;

                // Prevent BackgroundService from locking before Startup.Configure()
                await Task.Yield();

                //Background processes
                await BackgroundProcessing(stoppingToken);
            }
        }

        //Backgronud Processor
        private async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
            /// WORKERS ////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////
            /// 
            //Dec


            //Get Rockwell PLC data
            while (!stoppingToken.IsCancellationRequested)
            {
                //SHOULD BE FROM SCREEN
                int delayTime = 1000;
                await Task.Delay(delayTime);

                try
                {
                    //Dec
                    int multiplier = 10;
                    bool highestHasPriority = true;
                    int highestListPosition = 16;
                    int plcListGroup = 0;
                    int maxGroups = 0;
                    DateTime timestamp = DateTime.UtcNow;

                    //Equipment connection
                    _logger.LogInformation("Retrieving PLC data...");
                    RockwellConnectionService rockwellConnectionService = new RockwellConnectionService();
                    var messageBoolGroups = await rockwellConnectionService.Getconnection();

                    //
                    if (messageBoolGroups == null || messageBoolGroups.Count == 0)
                    {
                        _logger.LogError("No data returned from PLC...");
                        errMessage.Payload = "PLC tags not read, possible connection error!";
                        errMessage.Timestamp = timestamp.ToString();
                        _positions.Insert(0, 0000);
                        //positions.Add(_positions.ConvertAll<string>(x => x.ToString()));
                        //ITERATE _messages[i]
                        //_messages[0].Insert(0, errMessage);
                        //messages = _messages[0];
                    }
                    else
                    {
                        //int[] messageId = new int[messageBoolGroups.Count()];

                        List<List<int?>> lastMessageId = new List<List<int?>>();
                        int j = 0;
                        int listNo = 0;
                        _messages.Add(new List<EquipmentMessage>(messageBoolGroups.Count()));
                        messages.Add(new List<EquipmentMessage>(messageBoolGroups.Count()));
                        positions.Add(new List<string>(messageBoolGroups.Count()));

                        //Connecton to PLC
                        List<StandardPath> pathList = dataPath();
                        //Path to messages
                        FilePaths filePaths = new FilePaths();
                        var PATHS = filePaths.PathData(pathList);
                        //List<string> pos = new List<string>();

                        foreach (var messageBoolGroup in messageBoolGroups)
                        {
                            // Bit list to position number
                            _logger.LogInformation("Converting PLC data...");
                            PlcTypeConverter plcTypeConverter = new PlcTypeConverter();
                            _positions = plcTypeConverter.Position(messageBoolGroup); //Message group position and message csv must match
                            _logger.LogInformation("LIST NO: {0}", listNo);
                            listNo += 1;

                            var messageId = plcTypeConverter.Priority(_positions, highestHasPriority);

                            //Position number to message
                            _logger.LogInformation("Retrieving messages...");
                            EquipmentMessagesService equipmentMessagesService = new EquipmentMessagesService();

                            int k = 0;
                            foreach (var p in _positions)
                            {
                                //Get message
                                EquipmentMessage message = await equipmentMessagesService.GetMessage(timestamp, p, PATHS[j]);

                                //Add message to list
                                if (message.Id != lastMessageId.ElementAt(j)[k])
                                {
                                    //Add messages to list and fifo
                                    if (_messages[j].Count() > (highestListPosition - 1))
                                    {
                                        for (int ii = _messages[j].Count - 1; ii-- > 0;)
                                        {
                                            _messages[j][ii + 1] = _messages[j][ii];
                                        }
                                        _messages[j].Insert(0, message);
                                        //_messages[j].RemoveRange(highestListPosition, 1);
                                    }
                                    else
                                    {
                                        _messages[j].Insert(0, message); //ADD PARAM FOR INITIAL MESSAGE
                                    }
                                }
                                //Store last ID
                                lastMessageId.ElementAt(j)[k] = message.Id;
                                k++;
                            }
                        }

                        //Update public variables
                        messages[j] = _messages[j];
                        var pos = _positions.ConvertAll<string>(x => x.ToString());
                        positions[j] = pos;

                        j++;
                    }
                }
                catch (FileNotFoundException ex)
                {
                    _logger.LogError(ex, "Error: Background tasks file not found!");
                }
                catch (TimeoutException ex)
                {
                    _logger.LogError(ex, "Error: Timeout!");
                }
                catch (TaskCanceledException ex)
                {
                    _logger.LogError(ex, "Error: Task cancelled, get Rockwell PLC data!!");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Error: Occurred during execution of background tasks!");
                }
            }
        }
    }
}


using LinqFileParser;

namespace OverviewServer.Services
{
    //Conection data required for Rockwell PLC's
    public static class Connections
    {
        //Connection list for a single PLC
        public static List<StandardPath> dataPath()
        {
            //Dec
            List<string> driveName = new List<string>();
            List<string> dirPath = new List<string>();
            List<string> fileName = new List<string>();

            List<StandardPath> pathList = new List<StandardPath>();

            //SHOULD BE ON SCREEN
            //#0
            driveName.Add("C:/");
            dirPath.Add("Users/Player_1/Desktop/WORK/OverviewServer/Datasets/OP40/");
            fileName.Add("Op40_Step_Messages.csv");

            //#0
            driveName.Add("C:/");
            dirPath.Add("Users/Player_1/Desktop/WORK/OverviewServer/Datasets/OP40/");
            fileName.Add("Op40_Input_Messages.csv");

            //#0
            driveName.Add("C:/");
            dirPath.Add("Users/Player_1/Desktop/WORK/OverviewServer/Datasets/OP40/");
            fileName.Add("Op40_Tool_Return_Messages.csv");

            //#0
            driveName.Add("C:/");
            dirPath.Add("Users/Player_1/Desktop/WORK/OverviewServer/Datasets/OP40/");
            fileName.Add("Op40_Output_Messages.csv");

            //#0
            driveName.Add("C:/");
            dirPath.Add("Users/Player_1/Desktop/WORK/OverviewServer/Datasets/OP40/");
            fileName.Add("Op40_Control_Messages.csv");

            //Add a connection for locations listed location
            for (int i = 0; i < driveName.Count(); i++)
            {
                //Add a connection
                pathList.Add(new StandardPath());

                //Connections
                pathList[i].Drive = driveName[i];
                pathList[i].Relative = dirPath[i];
                pathList[i].File = fileName[i];
            }

            //Return the populated list
            return pathList;
        }
    }
}
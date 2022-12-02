namespace OverviewServer.Utilities
{
    public class PlcTypeConverter
    {
        //Return the number for each position in the list
        public List<int> Position(List<bool> bList, int multiplier = 0, int startPos = 0)
        {
            //Dec
            List<int> ilist = new List<int>();
            int i = 0;
            int m = 1;
            int value = 0;

            //Multiply
            if (multiplier > 0) { m = multiplier; }

            //Assign number to position in list
            foreach (bool b in bList)
            {
                //Multiply
                if (i == 0)
                {
                    value = i;
                }
                else
                {
                    value = i * m;
                }

                //Update list
                if (b)
                {
                    ilist.Add(value);
                }

                i++;
            }

            return ilist;
        }

        //Most downstream active element
        public int Priority(List<int> iList, bool high = true)
        {
            int j = 0;

            if (iList.Count > 0)
            {
                foreach (int i in iList)
                {
                    if (i > j) { j = i; }
                }
            }

            return j;
        }

        //Order the last "n" active elements
        public List<int> OrderActive(List<int> iList, int max, bool decending)
        {
            //Order the list
            iList.Sort();
            if (decending)
                iList.Reverse();

            List<int> l = iList;

            //Limit list size
            l.Capacity = max;

            return l;
        }
    }
}

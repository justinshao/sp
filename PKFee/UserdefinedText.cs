using System;
namespace PKFee
{
    public class UserdefinedText
    {
        public decimal CalcFee(DateTime starttime, DateTime endtime)
        {

            decimal money = 0;
            TimeSpan ts = endtime - starttime;
            long min = ts.Days * 24 * 60 + ts.Hours * 60 + ts.Minutes;
            if (min < 60)
            {
                return 0;
            }
            money += ts.Days * 20;
         
            long min2 = min % (24 * 60);
            if (min2 < 240)
            {
                money += 5;
            }else
            if (min2 >= 240 && min2 < 12 * 60)
            {
                money += 8;
            }else 
            if (min2 >= 12*60 && min2 < 16 * 60)
            {
                money += 13;
            }else
            if (min2 >=16 * 60)
            {
                money += 20;
            }
            return money;
        }
    }
}

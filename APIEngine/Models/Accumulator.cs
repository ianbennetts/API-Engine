using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIEngine.Models
{
    public class Accumulator
    {
        private int _numberOfReturns;
        private int _totalValue;
        private int _requiredQtyReturns;
        private static readonly object _accumulatorLock=new object();
        public Accumulator(int numberSites)
        {
            _requiredQtyReturns = numberSites;
        }
        public void Addthis(int value)
        {
            lock (_accumulatorLock)
            {
                _totalValue += value;
                _numberOfReturns++;
            }
        }
        public bool IsCompleted()
        {
            return (_numberOfReturns == _requiredQtyReturns);
        }
        public int GetTotal()
        {
            return _totalValue;
        }
    }

}
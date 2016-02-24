using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;    

namespace SecFileDel.Core
{
    class SFD
    {
        //Public properties 
        public RemovalMode Mode { get { return _mode; } }
        public int Rounds { get { return _rounds; } }

        //Private properties 
        private RemovalMode _mode; 
        private int _rounds;

        public enum RemovalMode
        {
            NormalDelete = 0, //regular built in delete methods...
            RandomDelete, //overwrite the file with random data 
            NullDelete //overwrite the file with null data 
        }

        /// <summary>
        /// Create a new instance of the file deleter
        /// </summary>
        /// <param name="mode">The type of delete you would like to perform.</param>
        /// <param name="rounds">The amount of times you would like to overwrite data(1, 3, 7, 11, etc)</param>
        public SFD(RemovalMode mode, int rounds = 1)
        {
            //pass the parameters... 
            this._mode = mode;
            this._rounds = rounds; 
        }



    }
}

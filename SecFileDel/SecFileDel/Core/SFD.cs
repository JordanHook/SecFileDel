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
        private Random _random; 

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
        public SFD(RemovalMode mode, int rounds = 1, int randomSeed = 102498573)
        {
            //pass the parameters... 
            this._mode = mode;
            this._rounds = rounds;
            this._random = new Random(randomSeed); //intalize the random seed 
        }

        /// <summary>
        /// Call this method to delete a new file
        /// </summary>
        /// <param name="path">The path of the file you would like to delete.</param>
        /// <returns>Returns true if the file was deleted successfully</returns>
        public bool delete(string path)
        {
            //If the file does not exist, throw and error
            if (!File.Exists(path))
                throw new Exception("File " + path + " does not exists!"); 

            if(this.Mode == RemovalMode.NormalDelete) //no special actions will be required... just try to delete the file... 
            {
                return NativeMethods.DeleteFile(path); //delete the file and return the result (true if deleted...) 
            }
            else //special removal method... 
            {
                for(int i = 0; i < this.Rounds; i++) //loop for the amount of overwrites... 
                {
                    //Create a new file stream to overwrite the file with...
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.None))
                    {
                        byte[] buffer = new byte[512]; //we will be doing 512 bytes at a time... 
                        long sectors = fs.Length / buffer.Length; //get the amount of times we will need to loop 

                        for(var x = 0; x < sectors; x++) //loop through the whole file (give or take a few bytes depending on the size... 
                        {
                            if(Mode == RemovalMode.RandomDelete) //if random mode is enabled... 
                                this._random.NextBytes(buffer); //create the random data for this write... 

                            fs.Write(buffer, 0, buffer.Length); //write the buffer (it will be empty :: 0's if in any other mode...) 
                        }

                    }
                } //end of rounds...

                //move the file (this will mess up some of the file information in programs like recurva) 
                NativeMethods.MoveFileEx(path, path + ".tmp", NativeMethods.MoveFileFlags.ReplaceExisting);

                //Return the results once the file is delete
                return NativeMethods.DeleteFile(path); 
            }
        }


    }
}

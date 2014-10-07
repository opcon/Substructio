using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Substructio.IO
{
    public static class ASCIIFileHelper
    {

        private static FileStream _fileStream;
        private static StreamReader _streamReader;
        private static StreamWriter _streamWriter;

        public static string ReadFileToEnd(string f)
        {
            _fileStream = File.OpenRead(f);
            _streamReader = new StreamReader(_fileStream);

            var result = _streamReader.ReadToEnd();

            _streamReader.Close();

            return result;
        }

        public static void WriteString(string f, string input)
        {
            _fileStream = File.OpenWrite(f);
            _streamWriter = new StreamWriter(_fileStream);

            _streamWriter.Write(input);

            _streamWriter.Close();
        }


        //TODO:Write a method for loading a string from a path, and return the string and a FileBase object.
        //		public void ReadFileToEnd(out string result)
        //		{
        //			
        //			
        //		}
    }
}

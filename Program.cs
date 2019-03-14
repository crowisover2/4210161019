using System;
using System.ComponentModel.Design;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace ConsoleApp2
{


    /*
     Belum Selesai karena Masih mencari bagaimana cara encapsulasi data pada C#
     */

    public class byteData
    {
        public byte[] Data;

        public static class SeDeData
        {
            public static M serializable(object O)
            {
                using (var memoryStream = new MemoryStream())
                {
                    (new BinaryFormatter()).Serialize(memoryStream, O);
                    SerializableAttribute serializableAttribute = new SerializableAttribute
                    return new SerializableAttribute(Data = memoryStream.ToArray());
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
    
}

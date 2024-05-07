using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicornRecorderLSLReceiver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // wait until an EEG stream shows up
            Console.WriteLine("Scanning for LSL streams...");
            LSL.liblsl.StreamInfo[] results = null;
            while (results == null || results.Length == 0)
            {
                results = LSL.liblsl.resolve_streams();
                //results = LSL.liblsl.resolve_stream("type", "Data");
                //results = LSL.liblsl.resolve_stream("name='UN-'");
            }

            Console.WriteLine();
            Console.WriteLine("[id] streamname");
            Console.WriteLine("---------------");
            for (int i = 0; i < results.Length; i++)
            {
                Console.WriteLine("[{0}] {1} ", i, results[i].name());
            }
            Console.WriteLine();
            Console.WriteLine("Select available LSL stream by the [id] and press enter");
            string ids = Console.ReadLine();
            int id = int.Parse(ids);

            LSL.liblsl.StreamInlet inlet = new LSL.liblsl.StreamInlet(results[id]);
            LSL.liblsl.StreamInfo streaminfo = inlet.info();
            int numberOfChannels = streaminfo.channel_count();

            float[] sample = new float[numberOfChannels];
            while (!Console.KeyAvailable)
            {
                inlet.pull_sample(sample);
                for (int i = 0; i < sample.Length; i++)
                {
                    if (i == 0)
                        Console.Write("{0}", sample[i]);
                    else
                        Console.Write(",{0}", sample[i]);
                }
                Console.WriteLine();
            }
        }
    }
}

using System;
using System.IO;
using NAudio.Wave;

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter path to first audio file:");
        string file1 = Console.ReadLine();

        Console.WriteLine("Enter path to second audio file:");
        string file2 = Console.ReadLine();

        if (!File.Exists(file1) || !File.Exists(file2))
        {
            Console.WriteLine("Error: One or both files do not exist.");
            return;
        }

        Console.WriteLine("Enter crossfade duration in seconds (e.g. 5):");
        int fadeSeconds = int.Parse(Console.ReadLine());

        Console.WriteLine("\nResult will be saved as: output.wav");
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();

        using var reader1 = new AudioFileReader(file1);
        using var reader2 = new AudioFileReader(file2);

        var format = reader1.WaveFormat;
        using var output = new WaveFileWriter("output.wav", format);

        int channels = format.Channels;
        int sampleRate = format.SampleRate;

        int fadeSamples = sampleRate * fadeSeconds * channels;

        float[] buffer = new float[1024];
        int read;

        // 1

        long fadeBytes = fadeSamples * sizeof(float);
        long track1FadeStart = reader1.Length - fadeBytes;

        while (reader1.Position < track1FadeStart)
        {
            read = reader1.Read(buffer, 0, buffer.Length);

            if (reader1.Position > track1FadeStart)
            {
                read -= (int)((reader1.Position - track1FadeStart) / sizeof(float));
            }

            output.WriteSamples(buffer, 0, read);
        }

        // read fade parts

        float[] fadeBuffer1 = new float[fadeSamples];
        float[] fadeBuffer2 = new float[fadeSamples];

        reader1.Read(fadeBuffer1, 0, fadeSamples);
        reader2.Read(fadeBuffer2, 0, fadeSamples);

        // crossfade

        for (int i = 0; i < fadeSamples; i++)
        {
            float t = (float)i / fadeSamples;

            float fadeOut = (float)Math.Cos(t * Math.PI / 2);
            float fadeIn  = (float)Math.Sin(t * Math.PI / 2);

            float sample =
                fadeBuffer1[i] * fadeOut +
                fadeBuffer2[i] * fadeIn;

            output.WriteSample(sample);
        }

        // rest of track

        while ((read = reader2.Read(buffer, 0, buffer.Length)) > 0)
        {
            output.WriteSamples(buffer, 0, read);
        }

        Console.WriteLine("Done! File saved as output.wav");
    }
}
# Audio Crossfade Tool (C#)

Simple C# console application for creating smooth crossfade transitions between two WAV audio files.

## Features
- Crossfade between two tracks
- Adjustable transition duration
- Basic DSP logic (fade-in / fade-out)
- Built with NAudio

## How it works
The program mixes the end of the first track with the beginning of the second track using linear interpolation:

sample = A * (1 - t) + B * t

Where:
- A = sample from track 1 (fade out)
- B = sample from track 2 (fade in)
- t = transition progress (0 → 1)

## Usage

1. Place two WAV files in the project folder:
   - `1.wav`
   - `2.wav`

2. Run the program:

```bash
dotnet run

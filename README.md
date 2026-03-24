# Audio Crossfade Tool (C#)

Simple C# console application for creating smooth crossfade transitions between two WAV audio files.

## Features
- Crossfade between two tracks
- Adjustable transition duration
- Equal-power crossfade (perceptually smooth)
- Built with NAudio

## How it works

The program blends audio samples from two tracks using an equal-power crossfade:

A * cos(t * π/2) + B * sin(t * π/2)

Where:
- A = sample from track 1 (fade out)
- B = sample from track 2 (fade in)
- t = transition progress (0 → 1)

This approach preserves perceived loudness during the transition and avoids volume drop typical for linear crossfade.

## Usage

1. Place two WAV files in the project folder:
   - `1.wav`
   - `2.wav`

2. Run the program:

```bash
dotnet run

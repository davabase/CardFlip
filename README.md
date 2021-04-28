# Card Flip

This is an example of how to export planar points from an animation in blender for use with a 2d quad in monogame.

![Card Flip Animation](card_flip.gif)

The Blender file has several playing card animations described in the Action Editor.
The accompanying python script, `card_exporter.py`, is run from within Blender to calculate the four corners of the card mesh in each frame, from the camera's perspective, and write them to a file.
The exported animation file is then imported into the content pipeline with a Build Action of "Copy" since we are reading the file as plain text.
The monogame program then reads the points in each frame as a C# List and draws a rectangular user primitive using the coordinates from each of the frames in the exported animation file.

We use Blender as an animation tool and to do the heavy lifting for perspective rendering. This allows us to bake in the 3d coordinates in a 2d view and use just 2d drawing to get a 3d perspective.

Additionally we can add the ability to generate in-between frames by linearly interpolating between frames.

The code in this repository is CC0, public domain.
The images are public domain from Pixabay.
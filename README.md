# ConsoleRenderingFramework

This project helps in putting things into the console

The different parts of the project will provide tools for simpler Printing, management of the console and stuff like that

I will list all of the individual Parts here and their purpose. There is not a tutorial, the testing projects and stuff should show thngs good enough


## ConsoleRenderingFramework

This is the Library with the main content for the rendering. It provides the general framework and manages the console itself (or if you prefer the experimental windows forms)

All other parts of the system interact with the ConsoleRenderingFramework(CRF for short)

## BasicRenderProviders

Util Classes for getting things like inked backgrounds or text fitted into a box

## MultiSplitScreenManager

A bunch of different ScreenManagers for use with the CRF. Names are selfexplainatory and allow for combination to create hirachy based rendering. (a Splitscreen with multiple WindowScreens)

## CRFAnalyticsPackage

This should have become a package for creating visuals for rendering. But it was abandond like a valve title shortly after start of the project. 

## CRFInput

The Input management in a console sounds a bit out of place as we already have Console.ReadLine() right?

WRONG. With this package you are able to get direct Keyboard inputs as well as information about the location of the mouse.

Sadly i was not able to get it to work with relativ position to the window. 

But it is possible to get multiple button presses at once to play 2 player games on one keyboard

## CardGame

Started out as a card game, but became the tech demo for the WindowScreenManager, which should have been used for the game itselfe. But is looks great, Layering works and changing of the textboxes does too

## DebugSystem

The project includes a simple Classic live data viewer debugger for the console (might be a bit slow)
But just so you can show some data for you bosses or so i guess

### OtherTesting

Testing the debugger with the failur of getting screen location of the mouse relative to where the window is

## TestingProj

Testing the abilities of the CRF, generation, rendering

## Pong

Not operational

You can test the paralell input 

## Doom

You know that the old 3d games where not real 3d right? Here is a tech demo of me rendering a scene like in wolfenstein 3d (and early doom) without real 3d. Here you can see how slow the windows console realy is.

but it shows the scan lines very well (i like that a bit). Project ended when i ran into a roadblock when trying to get enemies to work.

### Controls
- WASD for movement
- Arrows for looking
- C for crouch

### Notes

Dont keep your button pressed. It will input buffer because it does not use the CRF input system (was created before that)

## RayMarching

so that is my proudes piece of work. It is a full Raymaching implementation for the windows console ( or buggy for forms)

It renders a 3d scene with the technology of RayMarching and applies simple shading with the limited 16 colors available for the console. Currently Spheres and Quads are the only possible objects beeing renderd. 

### Controls

- WASD movement
- Arrows for looking
- Space --> up
- C --> down

### Notes

The code is set up to be able to run on a GMU or a GMUF

the only difference should be the setup. I should do something about that i think

## All the others

They are either unused or have never been filled. Nonetheless we love them of course

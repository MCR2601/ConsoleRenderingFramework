readme of Doom
======================



How will it work?



The world will consist of "wallblocks" that are placed on a grid

the player will have a position on this 2d grid.
the player will have a vector that shows the direction he is looking

the will be more vectors based on the resolution of the screen and FOV
theses vectors will have a small angular ofsett to the center 



How are multiple Things rendered?
	Due to the nature of the GMU you are able to override everything multiple times --> it is not required to calculate everything
	We can just create a stack for things to render from our position to the final wall
	Then we will just call the stack from the top with stored distances.
	This process will start by creating the background and override everything in the way of the background

Different Kind of Renderable Objects:
1) Object3D
	these are Objects that are Rendered after the fixed bounds and will appear 3D due to rendering
	Examples: Wall, Table
2) ObjectSprite
	these Objects dont have any 3D elements. They are just a representation of a sprite in 3D space
	These are usually transparent, not background. They will not be rendered depending on how you look at them.
	Instead they will allways look the same
	Examples: Enemy, Granade, Weapon, Textbox

Interfaces:
	- IRenderable:
		This interface describes Renderable Objects. There can be a lot of different Renderable objects so the Interface is kept simple

	

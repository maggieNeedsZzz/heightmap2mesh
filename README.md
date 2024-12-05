# Heightmap2Mesh
Unity Package for generating terrain meshes from heightmap images. 


## Setup 

The package can be imported into Unity by going to Window -> Package Manager -> + (in the Top-Left Corner) -> Add Package from git URL 

## Usage

After importing the package, the *TerrainSpawner* window can be opened by going to Tools -> TerrainSpawner. Then: 

<div style="text-align: center;">
    <span style="padding: 10px;">
        <i>Terrain can be created by placing a heightmap in the <b>TerrainSpawner</b> window and pressing the <b>Spawn</b> button.</i>
    </span>
</div>

<img src="assets/usage.gif" alt="Texture Import" style="padding: 10px; clip-path: inset(0 0 116px 0); margin-bottom: -116px;">



<div style="text-align: center;">
    <span style="padding: 10px;">
        <i>Terrain can also be textured.</i>
    </span>
</div>

<img src="assets/texture.gif" alt="Texture Import" style="padding: 10px; clip-path: inset(0 0 114px 0); margin-bottom: -114px;">



<div style="text-align: center;">
    <span style="padding: 10px;">
        <i>And the terrain chunk resolution can be set by the user. </i>
    </span>
</div>

<img src="assets/chunk.gif" alt="Texture Import" style="padding: 10px; clip-path: inset(0 0 114px 0); margin-bottom: -114px;">


## Importing Images

For the package to successfully process the images, they must be imported using the right settings. Most importantly, all images used for terrain generation must have the ``Read/Write`` setting ticked.

### Heightmaps

Heightmap images need to be 1-channel grayscale images.
They must be imported with the following settings:
- _Texture Type_ must be set to ``Single Channel``
- _Channel_ must be set to ``Alpha``
- _Alpha Source_ must be set to ``From Gray Scale``


<img src="assets/heightmap-import-v2.png" alt="Heightmap Import Settings" style="width: 70%;">


**Alternativelly**, a folder named **Heightmaps** can be created anywhere inside the *Assets* folder in the project. Any images placed within will be automatically imported with the correct settings. *Make sure not to place any textures in this folder.*

### Textures

For textures, all images are supported.

<img src="assets/texture-import-v2.png" alt="Heightmap Import Settings" style="width: 70%;">


## Sample Scene

The package also comes with a sample scene which has some example heightmaps and textures.
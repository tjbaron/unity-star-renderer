
# Unity Star Renderer

This code is used to render a 3D star field into you scene using the [HYG star database](https://github.com/astronexus/HYG-Database).

In this animation you can see Orion and Pleiades as we fly along at 1 lightyear per second!

![Orion](./Documentation/Orion.webp)

It is also simple to keep the sky rendered from a fixed vantage point. Whether that is from Earth/Sol or... Betelgeuse?

If you plan to use this as a Skybox from the perspective of Earth's surface it is possible to specify a Latitude, Longitude,
Day of year (0-365) and time (0-24). The stars will rotate accordingly with +Z being north and +X being east. In this case
you'll want to set "Viewer Position" to 0, 0, 0 (the position of our sun, Sol) and disable "Viewer to Camera Position" (so
you don't fly 1 parsec for every unit you move the camera).

[WebGL demo available on simmer.io](https://simmer.io/@tjbaron/star-renderer)

## Adding to your project

### As a read only package

1. In Unity, got to "Window > Package Manager".
2. Click "+" and choose "Add package from git URL..."
3. Paste "https://github.com/tjbaron/unity-star-renderer.git" and press "Add"

Note: If you have trouble, ensure that both git and git-lfs are installed (on Mac this can be done via HomeBrew).

### As a git submodule

1. Ensure your project is already a git repo
2. Go to your projects root folder on command line
3. Run `git submodule add https://github.com/tjbaron/unity-star-renderer.git Packages/com.baroncreations.star-renderer`

You should now have the submodule added to the directory `Packages/com.baroncreations.star-renderer`

## Viewing the demo scenes

If you took the read only approach you will need to copy the example scenes to your "Assets" folder before opening:

1. In Unity "Projects" go to "Packages > Star Renderer > Samples"
2. Drag the scenes to the "Assets" folder to copy them over
3. Double click the scenes in you "Assets" folder to open

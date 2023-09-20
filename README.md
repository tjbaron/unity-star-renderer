
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

1. In Unity, got to "Window > Package Manager".
2. Click "+" and choose "Add package from git URL..."
3. Paste "https://github.com/tjbaron/unity-star-renderer" and press "Add"

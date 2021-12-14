# Publish

Steps I use to publish our WebGL demo:

1. Build for WebGL (with this repo added into an actual Unity project)
2. Copy result into a folder called `Demo` in this repo
3. run `npx push-dir --dir=Demo --branch=gh-pages`

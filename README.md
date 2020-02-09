# Debugging ASP.Net Core + VS Code + docker-compose + Remote Container Extension + Multiple Applications

## Debugging the Application

1. Open the directory that contains the `docker-compose.base.yml` and other docker compose files with `VSCode`.
2. Press `F1`, and type and select the command `Remote-Containers: Open Folder in Container...`.
3. A prompt should open for your select a folder. Select the `WebApi` folder.
4. This will re-open `VSCode` with the container.
5. Put breakpoints, and press `F5` to run the applicaton. Alternatively you can go to the `Debug` tab and press the green `play` button.
6. Repeat steps 1-5, but select `WebApp` folder instead for step 3.
7. Browse to:
   * [http://localhost:5000](http://localhost:5000) for the web application, and
   * [http://localhost:5002](http://localhost:5002) for the web api. You can browse to the [Weather Forecast](http://localhost:5002/WeatherForecast) api endpoint.

## Releasing the Application

1. Open up a terminal and change the working directory to where the project's `docker-compose.base.yml` & `docker-compose.release.yml`.
2. Edit the `.env` file. There's various variables you may want to configure to suit your purposes. You can change whether it deploys using development or production configs, or whether it'll set up HTTPS.
3. Run the below command:
   ```bash
   docker-compose -f docker-compose.base.yml -f docker-compose.release.yml build
   ```
   This will build the production-ready image(s), which you can release or push to a docker hub or use for your CI/CD pipeline.
4. **(Optional)** To test if the application & image works under release configuration, run the below command to run the containers locally:
   ```bash
   docker-compose -f docker-compose.base.yml -f docker-compose.release.yml up -d
   ```
   Now browse to [http://localhost:5000](http://localhost:5000)

   Note the above port would depend on what you've set for in your `.env` file.

## Problem

The problem with multi-container setup with `docker-compose` is you can't really use a shared `.env` file for your `docker-compose`

Imagine if you have this structure:

* .docker
* WebApi
  * .devcontainer.json
  * ... Other source code files
* WebApp
  * .devcontainer.json
  * ... Other source code files
* .env
* docker-compose.base.yml
* docker-compose.devcontainer.yml
* docker-compose.release.yml

With the `remote - containers` extension, if you follow the official instructions [here](https://code.visualstudio.com/docs/remote/containers-advanced#_connecting-to-multiple-containers-at-once), where you'd use the command `Remote-Containers: Open Folder in Container...`, it won't actually work and would just error out. The problem is it'll open the folder where the `.devcontainer.json` file resides. The `docker-compose` command's working directory would also be where this `.devcontainer.json` file resides.

The implications of this is it'll try to find and use the `.env` file from the directory where the `.devcontainer.json` file resides (which of course, doesn't exists). As it doesn't exist in that directory, the `.env` file isn't used, meaning the environment variables doesn't get imported, causing the extension to fail. 

Thus, currently the only way to get it to work is you'll need to copy your `.env` file to be where the `.devcontainer.json` file resides. That is, you'd have the below structure where you `.env` file is duplicated.

* .docker
* WebApi
  * .devcontainer.json
  * `.env`
  * ... Other source code files
* WebApp
  * .devcontainer.json
  * `.env`
  * ... Other source code files
* docker-compose.base.yml
* docker-compose.devcontainer.yml
* docker-compose.release.yml

This can be resolved if the extension lets us specify the option `--project-directory` for `docker-compose`.

For more information on `--project-directory`, see:
* https://github.com/docker/compose/issues/5481#issuecomment-547083786

# First stage build, responsible for
# building the application
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
ARG buildconfig
ARG pathtoproject

# Make the directories
# to copy over the files
RUN mkdir -p /app

# Copy the directories and files
WORKDIR /app
COPY . .

# Publish the projects
WORKDIR /app/${pathtoproject}
RUN dotnet restore
RUN if [ "${buildconfig}" = "Debug" ]; then \
        dotnet publish -o /publish -c Debug --no-restore; \
    else \
        dotnet publish -o /publish -c Release --no-restore; \
    fi

# Second stage build, responsible for
# running the application. More lightweight
# compared to the first stage build.
# Uses the runtime image, opposed to the SDK image.
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
ARG csprojbasename
ARG entrypointfile
ARG launchapp
ENV APPLICATION_DDL_FILE=${csprojbasename}.dll
ENV LAUNCH_APP=${launchapp}
WORKDIR /publish
# Copy from the previous image the
# published files to the current
# image to run the application
COPY --from=build-env /publish .

# Set entrypoint so that
# when containers are up,
# the web server is started
COPY ${entrypointfile} /publish/docker-entrypoint.sh
RUN chmod +x /publish/docker-entrypoint.sh
ENTRYPOINT ["/publish/docker-entrypoint.sh"]
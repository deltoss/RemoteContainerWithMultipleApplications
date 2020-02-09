#!/bin/bash

# On Windows, make sure you
# use LF line endings instead of
# the default CRLF as that won't
# run in Linux, breaking the docker
# container

if [ "$LAUNCH_APP" = true ]; then
  dotnet "$APPLICATION_DDL_FILE"
else
  sleep infinity
fi

# Azure Dev Ops
Help link
https://docs.microsoft.com/en-us/azure/devops/boards/github/connect-to-github?view=azure-devops

## Create the project
Create the azure dev ops project (Skills)
Connect it to the GitHub via "project settings/GitHub connections"

## Connect the gitHub repo to azure devops
In Azure dev ops "Repos" menu, import the github repo to see commits, branches, etc in the azure dev ops.

## Create pipelines
###Create build pipeline
Run the build and check that it is green
If needed, modify the yaml and put the platform: '' to use any cpu

Modify the yaml to add the test task, coverage

See link : https://docs.microsoft.com/en-us/azure/devops/pipelines/languages/dotnet-core?view=azure-devops


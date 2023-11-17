#!/bin/bash

readonly REPO_REGEX="^[a-zA-Z0-9]{6}_ADT_2023241$"

# Check repo name
echo "github repo name: $GITHUB_REPO_NAME"

if ! [[ $GITHUB_REPO_NAME =~ $REPO_REGEX ]]; then
    echo "github repo name does not follow the required format of ABC123_ADT_2023241"
    exit 1
fi

# Check all project names

readonly project_names=("Client" "Endpoint" "Logic" "Models" "Test" "Repository")

# cd ../

matching_projects=0

# check for bin/debug folders
for name in ${project_names[@]}; do
    echo $(pwd)
    echo "$(pwd)/$GITHUB_REPO_NAME.$name"
    if [[ -d $(pwd)/$GITHUB_REPO_NAME.$name ]]; then
     ((matching_projects+=1))
     echo "matching project for: $name"
     # Navigate to the project directory
     cd $(pwd)/$GITHUB_REPO_NAME.$name
     if [[ -d "bin" || -d "obj" ]]; then
        echo "bin or obj folders found in project: $name"
        echo "it must not be part of the repository!"
        echo "you need to delete them from your repo then upload (commit & push) your changes"
        exit 1
     fi
     # move back
     cd ../
    fi

done

if [[ $matching_projects != 6 ]]; then
    echo "One or more project names / locations are not valid! (All projects should be at the root of the repository)"
    exit 1
fi

# Check .gitignore (not it's contents though!)

if ! [[ -f .gitignore ]]; then
    echo $(pwd)
    ls -lah
    echo "No .gitignore file was found at the root of the repository!"
    exit 1
fi

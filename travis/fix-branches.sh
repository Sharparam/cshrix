#!/usr/bin/env bash

git fetch origin +refs/heads/*:refs/remotes/origin/* --unshallow

git remote set-branches --add origin master
git remote set-branches --add origin develop

for branch in $(git branch --all | grep '^\s*remotes' | grep -v HEAD); do
    git remote set-branches --add origin ${branch#remotes/origin/}
    git branch --track -f ${branch#remotes/origin/} ${branch}
done

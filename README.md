# Project Boilerplate

This is a boilerplate for a project. It is a good starting point for a new project, can apply for any project, good better for JS.  
![workflow status](https://github.com/DungGramer/project-boilerplate/actions/workflows/lint-code.yml/badge.svg)
[![MIT License](https://img.shields.io/apm/l/atomic-design-ui.svg?)](https://github.com/tterb/atomic-design-ui/blob/master/LICENSEs)
[![contributions welcome](https://img.shields.io/badge/contributions-welcome-brightgreen.svg?style=flat)](https://github.com/DungGramer/project-boilerplate/issues)

## Feature

- `Eslint`: Statically analyzes your code to quickly find problems
- `Prettier`: Sormats your code to be more readable
- `Commitlint`: Analyzes your commit messages to make sure they are correct. Read more rule in [Rule Commit](./Rules-commit.md)
- `Github Actions`: Runs tests lastly before pushing to Github
- `Husky`: Runs linting, tests, and code formatting before each commit
- `lint-staged`: Runs linting and tests on staged files
- `commitizen`: Help you to create a correct commit message
- `cz-conventional-changelog`: Generates a changelog based on the changes in your working directory
- `standard-version`: Auto generate release changelog
- `gitattributes`: Handle line endings automatically for files detected

## Workflow for managing releases

1. Create your features and run `git add .`
2. Run `npm run lint` to check your code and format it
3. Execute the `npm run commit` in the command line to make a commit with Commitizen
4. Run `npm run release` to create a changelog and a semantic versioning-based release

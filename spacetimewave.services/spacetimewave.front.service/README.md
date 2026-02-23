# React Template

[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/javierhersan/react-template/LICENSE) [![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)](https://github.com/javierhersan/react-template/CONTRIBUTING.md)

This project is a React template designed to accelerate web development. The primary goal is to enable developers to bring their ideas to the market as quickly as possible, with an integrated go to market strategy.

React Template empowers developers to create interactive and dynamic user interfaces, built with a focus on declarative programming, component-based architecture, and versatility.

## Features

Version 0.2.0 of the project, released on February 23, 2025, introduces a robust set of features designed to streamline the development and maintenance of a React-based web application.

At the core of this project is the integration of Vite and esbuild as the chosen build tools. This choice aims to enhance development experience and build times. The project is developed in JavaScript, although in future versions a typescript project willbe included as well.

The project includes basic page templates to establish a clear and organized page hierarchy. The integration of routing enables a seamless navigation within the application.

Also a component library of reusable components has been included, contributing to modular and maintainable code. This approach not only accelerates development speed but also ensures consistency throughout the application.

The template includes authentication and protected routes adding a layer of security and control over the application.

Global state management has been implemented using Zustand stores. Stores improve DX because they are easy to configure.

The project also includes service modules for external interactions, facilitating communication with APIs and external data sources.

Environment configurations for development, user acceptance testing, and production environments are set up, enabling seamless transitions between different deployment stages.

The project adopts CSS Modules for structured, isolated, and modular styling. This approach prevents style conflicts and enhances maintainability by encapsulating styles within individual components.

To maintain code quality, the project incorporates tools such as ESLint for code consistency and Prettier for consistent code formatting. These tools enforce coding standards and contribute to a clean and readable codebase.

## Installation

This is the installation guide.

1. First, install NodeJS v22 or greater.
2. Second install a package manager (npm, pnpm or yarn)
3. Clone the project in your local machine

```console
$ git clone https://github.com/javierhersan/react-template
```

4. Install all the dependencies of the project.

```console
$ pnpm install
```

5. Run the project

```console
$ npm start
```

## Configuration

This is the configuration guide. In case you want to start a fresh project based on our configurations.

### React + Vite

Vite configuration for React.

```console
$ pnpm create vite
√ Project name: ... front
√ Select a framework: » React
√ Select a variant: » Typescript
$ pnmp start

$ pnmp install
$ pnmp install react react-dom react-router-dom
$ ...
```

### ESLint + Prettier

ESLint and Prettier configuration for React.

1. Install linter as a development dependency.

   ```console
   $ pnmp install eslint -D
   $ pnmp install standard -D
   ```

2. Install VSCode extension: ESLint by Microsoft.

3. Initialize the linter, its configuration, and install its dependencies.

   ```console
   $ pnpm dlx eslint --init
   ```

   ```console
    $ pnpm dlx eslint --init # Option 1
    $ pnpm create @eslint/config # Option 2
    √ How would you like to use ESLint? · style
    √ What type of modules does your project use? · esm
    √ Which framework does your project use? · react
    √ Does your project use TypeScript? · No / Yes
    √ Where does your code run? · browser
    √ How would you like to define a style for your project? · guide
    √ Which style guide do you want to follow? · standard
    √ What format do you want your config file to be in? · JSON
    Checking peerDependencies of eslint-config-standard@latest
    The config that you've selected requires the following dependencies:

    eslint-plugin-react@latest eslint-config-standard@latest eslint@^8.0.1 eslint-plugin-import@^2.25.2 eslint-plugin-n@^15.0.0 || ^16.0.0 eslint-plugin-promise@^6.0.0
    √ Would you like to install them now? · No / Yes
    √ Which package manager do you want to use? · npm
    Installing eslint-plugin-react@latest, eslint-config-standard@latest, eslint@^8.0.1, eslint-plugin-import@^2.25.2, eslint-plugin-n@^15.0.0 || ^16.0.0 , eslint-plugin-promise@^6.0.0
   ```

4. Change the configuration: .eslintrc.json

5. Show linting errors.

   ```console
   $ pnpm dlx eslint .
   $ pnpm dlx eslint src
   ```

6. Fix linting errors

   ```console
   $ pnpm dlx eslint . --fix
   $ pnpm dlx eslint src --fix
   ```

7. Install prettier as a development dependency.

   ```console
   $ pnpm install prettier -D
   ```

8. Install VSCode extension: Prettier by Prettier

9. Create Prettier config: .prettierrc.json

10. Format code with Prettier (show the changes to be made).

    ```console
    $ pnpm prettier .
    $ pnpm prettier src
    ```

11. Format code with Prettier (apply changes).

    ```console
    $ npx prettier . --write
    $ npx prettier src --write
    ```

12. Add to VSCode "Format Document With..." > "Configure Default Formatter.. " > "Prettier"

13. Add to VSCode "Settings" > "Format On Save" and "Format On Save Mode" > "file"

14. Prioritize Prettier rules over ESLint rules.

    Install "eslint-config-prettier" plugin.

    ```console
    $ pnpm install eslint-config-prettier -D
    ```

    Add to "extends" object in .eslintrc.json "eslint-config-prettier".

15. Add to ".prettierignore" file.

    ```console
    dist
    package-lock.json
    node_modules
    ```

16. Add to ".eslintignore" file.

    ```console
    dist
    node_modules
    ```

17. Add commands to "scripts" object in "package.json".

    ```console
    "format": "prettier --write ."
    "lint": "eslint --fix . --ext .js,.jsx"
    ```

18. Add to ".eslintrc.json" React version detection.

    ```console
    "settings": {
      "react":{
        "version":"detect"
      }
    }
    ```

19. Add eslint to Vite console.

    Install "vite-plugin-eslint" plugin.

    ```console
    $ pnpm install vite-plugin-eslint -D
    ```

    Add "vite-plugin-eslint" plugin to Vite. Edit "vite.config.js" file.

    ```console
    import { defineConfig } from 'vite'
    import react from '@vitejs/plugin-react-swc'
    import checker from 'vite-plugin-checker'
    export default defineConfig({
      plugins: [
        react(),
        checker({
          eslint: {
            lintCommand: 'eslint "./src/**/*.{js,jsx}"',
          },
        }),
      ]
    })
    ```

## Documentation

Not available.

## Contributing

React Template welcomes contributions from the community. Follow these steps to get involved:

- Review the Code of Conduct.
- Check the [Contributing Guide](https://github.com/javierhersan/react-template/CONTRIBUTING.md) for instructions on proposing bugfixes and improvements.

## License

React Template is [MIT licensed](https://github.com/javierhersan/react-template/LICENSE).

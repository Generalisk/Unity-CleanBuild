<div align="center">
  
  [![Commit Activity](https://img.shields.io/github/commit-activity/w/Generalisk/Unity-CleanBuild)](https://github.com/Generalisk/Unity-CleanBuild)
  [![Commit Activity](https://img.shields.io/github/commit-activity/m/Generalisk/Unity-CleanBuild)](https://github.com/Generalisk/Unity-CleanBuild)
  [![Commit Activity](https://img.shields.io/github/commit-activity/y/Generalisk/Unity-CleanBuild)](https://github.com/Generalisk/Unity-CleanBuild)
  [![Commit Activity](https://img.shields.io/github/commit-activity/t/Generalisk/Unity-CleanBuild)](https://github.com/Generalisk/Unity-CleanBuild)
  
  [![Version](https://img.shields.io/github/v/release/Generalisk/Unity-CleanBuild)](https://github.com/Generalisk/Unity-CleanBuild/releases/latest)
  [![Release Date](https://img.shields.io/github/release-date/Generalisk/Unity-CleanBuild)](https://github.com/Generalisk/Unity-CleanBuild/releases/latest)
  [![Commits since Latest Release](https://img.shields.io/github/commits-since/Generalisk/Unity-CleanBuild/latest)](https://github.com/Generalisk/Unity-CleanBuild/releases/latest)
  
  [![License](https://img.shields.io/github/license/Generalisk/Unity-CleanBuild)](https://github.com/Generalisk/Unity-CleanBuild/blob/main/LICENSE)
  [![Issues](https://img.shields.io/github/issues/Generalisk/Unity-CleanBuild)](https://github.com/Generalisk/Unity-CleanBuild/issues)
  [![File Size](https://img.shields.io/github/repo-size/Generalisk/Unity-CleanBuild)](https://github.com/Generalisk/Unity-CleanBuild)
  [![Last Commit](https://img.shields.io/github/last-commit/Generalisk/Unity-CleanBuild)](https://github.com/Generalisk/Unity-CleanBuild)
  
  [![Repo Watchers](https://img.shields.io/github/watchers/Generalisk/Unity-CleanBuild)](https://github.com/Generalisk/Unity-CleanBuild)
  [![Repo Stars](https://img.shields.io/github/stars/Generalisk/Unity-CleanBuild)](https://github.com/Generalisk/Unity-CleanBuild)
  [![Repo Forks](https://img.shields.io/github/forks/Generalisk/Unity-CleanBuild)](https://github.com/Generalisk/Unity-CleanBuild)
</div>

<div align="center">
  
  # Clean Build
</div>

<div align="center">
  
  ### Quickly be rid of those pesky unused & debug files and folders hogging up precious disk space with this Unity Package!
</div>

Clean Build automatically gets rid of unnecessary debug files that are automatically generated during your Unity Build.

### Examples of files that will be automatically removed include:
- Burst Compiler Reports
- IL2CPP Build Files
- Program Debug Files (PDBs) which often get automatically generated due to Managed Stripping
- Direct3D12 Binaries (if Graphics API is not included in the project)

# Contents
- [Requirements](#requirements)
- [Installing the Repo](#installing-the-repo)
  - [Method 1](#method-1)
  - [Method 2](#method-2)
  - [Method 3](#method-3)
- [Installing the Package](#installing-the-package)
  - [Method 1](#method-1)
  - [Method 2](#method-2)
- [Licensing](#licensing)
- [Credits](#credits)
- [Contributing](#contributing)
- [Planned Features](#planned-features)

# Requirements
- [Unity](https://unity.com/download) (version 2018.1 or over)

# Installing the Repo
There are multiple ways to set up this repo on your computer:
## Method 1
> [!WARNING]
> You will not be able to push commits/pull requests through directly if you use this method.
- At the top of the Repos' home page, click on the green button labeled `Code`.
- At the bottom of the panel that just popped up, click on the button labeled `Download ZIP`.
- Once the zip file has finished downloading, extract it's contents wherever you want.
## Method 2
> [!NOTE]
> This method requires [Git](https://git-scm.com/downloads) to be installed!
- Enter this command inside your Powershell/Terminal at the directory of your choice:
  ```
  git clone https://github.com/Generalisk/Unity-CleanBuild.git
  ```
## Method 3
> [!NOTE]
> This method requires [GitHub Desktop](https://desktop.github.com/) to be installed!
- Open your repository list and click on `Add > Clone repository...`
- Go to the URL tab.
- Enter `Generalisk/Unity-CleanBuild` into the box and set the local path to wherever you want.

# Installing the Package
There are multiple ways to add this package into your project:
## Method 1
- Install the repository through one of the methods above or via another method.
- Create A folder in your projects `Packages` folder and name it `com.generalisk.editor.quick-build`
- Paste the contents of the repository into the aforementioned folder.
## Method 2
- In Unity, open up your project and open the package manager window.
- Go to the top of the opened window and click on the `+` icon and go to `Install package from git URL...`
- Enter the following into the input field:
  ```
  https://github.com/Generalisk/Unity-CleanBuild.git
  ```

# Licensing
This package is licenced under the `MIT License`, more info can be found [here](../LICENSE).

# Credits
- [List of Contributors](CONTRIBUTORS.md)

# Contributing
Before you go and make A pull request, please make sure that your request follows our [Contribution Guidelines](CONTRIBUTING.md).

# Planned Features
- Remove unused built-in modules (Mono)

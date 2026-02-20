
# Unity Project Setup

## 1. Clone the project to Github Desktop
Go to the repo website, click the green Code button, and select “Open with GitHub Desktop” and clone the template project to your local machine
## 2. Modify the GLOBAL .gitconfig:
i.  Reveal hidden files on your operating system
- Windows 10: Open the File Explorer application and select View > Options > Change folder and search options and then select the View tab and, in Advanced settings, select Show hidden files, folders, and drives and OK.
- Windows 11: Open the File Explorer application and select View > Show > Hidden Items
- MacOS: Click on Finder and press the keyboard combination Shift + Cmd + . (period key)
  
ii. Open the `.git/config` file by navigating to your local repo folder, open the hidden `.git` folder, then open the config file (note: this is different from the `.gitconfig` file in the repo’s root folder)

iii. Identify your version of unity ( `2022.3.62f3`). This will replace the word `VERSION` in the paths commented below depending on your OS. Then, add the following text to the bottom of the file, replacing `<path to UnityYAMLMerge>` with the one specific to your OS and Unity version. For instance, in Unity version 2022.3.62f3 on Windows, the last line should be `cmd = 'C:\\Program Files\\Unity\\Hub\\Editor\\2022.3.62f3\\Editor\\Data\\Tools\\UnityYAMLMerge.exe' merge -p "$BASE" "$REMOTE" "$LOCAL" "$MERGED"`
```bash
[mergetool "unityyamlmerge"]
    trustExitCode = false
    #Replace <path to UnityYAMLMerge> in the next line with the following default locations (may be different depending on your Unity installation location)
    # Installs using the Unity Hub (Default):
    # Win: C:\\Program Files\\Unity\\Hub\\Editor\\VERSION\\Editor\\Data\\Tools\\UnityYAMLMerge.exe
    # MacOS: /Applications/Unity/Hub/Editor/VERSION/Unity.app/Contents/Tools/UnityYAMLMerge
    # Linux: /home/USERNAME/Unity/Hub/Editor/VERSION/Editor/Data/Tools/UnityYAMLMerge
    cmd = '<path to UnityYAMLMerge>' merge -p "$BASE" "$REMOTE" "$LOCAL" "$MERGED"
```
iv. Change the Git Config settings to use your local Git config in the menu bar Repository > Repository Settings > Git Config > Use a local Git config > Save.

## 3. Add pre-commit and post-merge scripts
Download the scripts from the link below and paste them into the hidden folder `<your_repo>/.git/hooks/`
- Pre-commit: https://github.com/NYUGameCenter/Unity-Git-Config/blob/master/pre-commit
- Post-commit: https://github.com/NYUGameCenter/Unity-Git-Config/blob/master/post-merge
On MacOS and Linux, the pre-commit and post-merge files need to be made executable. This involves using terminal (Terminal.app on MacOS) to modify the file permissions for these two files using the following commands.
```bash
> cd <local_repo_folder>/.git/hooks
> chmod 755 pre-commit
> chmod 755 post-merge
```
## 4. Open the Unity Project
The project is created using '`2022.3.62f3`. 

## Video tutorials
1. Github setup: https://youtu.be/iWniKp_O21o?feature=shared
2. Merge conflict resolution: https://youtu.be/vJIjV-GlGFg?feature=shared



   

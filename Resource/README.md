# Language localization

You can create language localization using:
- GS.Language.TextTable asset (You can create one using TextFolder asset)
- GS.Language.TextFolder asset (Create one to this folder)
- GS.Language.TextFiller component (Adds required Text component)

Assets should be placed under Assets/Resources/ folder.
Start with TextFolder asset.
With it you can create TextTable assets to right place.

Recommend paths:
- Assets/Resources/Language for TextFolder assets.
- Assets/Resources/Language/{TextFolder.language} for TextTable assets.

You can get JSON data from assets.

## GS.Language.TextTable

Contains:
- TextTable assets name, file name is not important, just tag here.
- Tagged texts, which are used to find the correct line for the localized language.

Editor has management tools to edit this.
- Add new tag and text.
- Export and import JSON.

## GS.Language.TextFolder

Contains:
- Project name, no real usage, just for you to recognize it for version reasons.
- Path to right folder under Assets/Resources folder.
- Language, which is used to create right folder for that language.
- Array pointers to right Text assets.

Editor has management tools to edit this.
- Add new TextTable asset. Creates missing directory.
- Get TextTable assets from directory.
- Export and Import JSON data (TODO, instead of instanceID give TextTables aswell).
- Copy assets to other language folder (TODO, dunno how yet...).

## GS.Language.TextFiller

Contains:
- Pointer to right TextTable asset.
- Tag to find right text from the asset.

Editor has management tools to edit this.
- Reload to edit link button.
- Select TextTable asset using the name.
- Select tag, which text will appera in text component.

Go to TextFiller.cs to change comments to have support for TextMeshPro.
- RequireComponent between Text and TextMeshPro (Either or none).
- Boolean to check if use either one (If want to use both).
- Force usage to other in UpdateText method (Or use boolean).

Note. Contains GS.EventManager listeners. Just comment or edit OnEnable or OnDisable methods in the end of file if you dont want them. Listens to "Language" tag from GS.EventManager by default.

## GS.Language.SwitchLanguage

Controller to switch between languages.
- On Start() it searches through resource folder and picks their languages to string array.

Requires two UnityEngine.UI.Text components for current language and target language. (Or just go blined with some errors...)

Methods to change languages:
- Before (move option ---)
- After (move option +++)
- Set language (Set languages to target)
- Default language (Set languages to 'default')

UI tool to control current language.
GameScripts/Prefabs has LanguageSwitcher prefab which implements this.

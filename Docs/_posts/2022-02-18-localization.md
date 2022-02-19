---
layout: post
title: "Localization"
date: 2022-02-18 00:07 -0300
badge: docs
math: true
categories: [Documentation, General]
pin: true
---
HunterPie supports different languages other than English, if you want to localize it to another language make sure to read this because it covers everything you need to know to make your own localization file.

## Localizations

The supported languages are:

<script>
    const API_GITHUB_LIST_FILES = "https://api.github.com/repos/Haato3o/HunterPie-v2/git/trees/main?recursive=1"
    const RAW_FILE = "https://raw.githubusercontent.com/Haato3o/HunterPie-v2/main/"
    async function readXmlFile(path) {
        const req = await fetch(`${RAW_FILE}${path}`);
        const res = await req.text();
        const parser = new DOMParser();

        return parser.parseFromString(res, "text/xml");
    }

    async function getLocalizationFiles() {
        const DEFAULT_LOCALIZATION = "HunterPie/Languages/en-us.xml";
        const english = await readXmlFile(DEFAULT_LOCALIZATION)
        const flatEnglish = flatXml(english)

        const filesResponse = await fetch(API_GITHUB_LIST_FILES)
        const response = await filesResponse.json()
        const localizations = response.tree
            .filter(e => e.path.startsWith("HunterPie/Languages/"))
            .flatMap(e => e.path)

        const languages = []

        for (let i = 0; i < localizations.length; i++) {
            let file = localizations[i]
            let xmlFile = await readXmlFile(file)
            let flatXmlFile = flatXml(xmlFile)
            let percentage = compareStringsRecursively(flatEnglish, flatXmlFile)

            languages.push({
                "name": file,
                "percentage": percentage
            })
        }

        // Display results
        const table = document.getElementById("localization-table")
        console.log(languages.length)
        languages.forEach(data => {
            const tr = document.createElement("tr");
            const name = document.createElement("td");
            const percentage = document.createElement("td")

            tr.appendChild(name)
            tr.appendChild(percentage)

            name.innerHTML = data.name
                .replace("HunterPie/Languages/", "")
                .replace(".xml", "")

            percentage.innerHTML = `${(data.percentage * 100).toFixed(2)}%`

            const getGreenToRed = (percent) => {
                const g = 80 + 190 * percent;
                const b = 50 + 41 * percent;
                const r = 160 + (38 * (1 - percent));
                return `rgb(${r}, ${g}, ${b})`;
            }

            percentage.style.color = getGreenToRed(data.percentage)

            table.appendChild(tr)
            console.log(table)
        })
    }

    function flatXml(document) {
        const hashset = new Set()
        const elements = document.getElementsByTagName("*")
        for (let i = 0; i < elements.length; i++) {
            let key = getFullNodePath(elements[i])
            
            if (key === undefined)
                continue
            
            hashset.add(key)
        }
        return hashset
    }

    function getFullNodePath(node) {
        if (node.attributes.Id == undefined)
            return undefined

        let path = node.attributes.Id.value
        while (node.parentElement != null) {
            let parentName = node.parentElement.tagName
            path = `${parentName}.${path}`

            node = node.parentElement
        }
        return path
    }

    function compareStringsRecursively(original, localization) {
        const oLength = original.size;
        const missing = new Set()

        original.forEach(v => {
            if (!localization.has(v))
                missing.add(v)
        })
        return (oLength - missing.size) / oLength
    }

    getLocalizationFiles();
</script>

<div class="table-wrapper"><table>
    <thead>
        <tr>
            <th>Language</th>
            <th>Translated (%)</th>
        </tr>
    </thead>
        <tbody id="localization-table">
        </tbody>
    </table>
</div>

## Getting Started

### Requirements

- A decent text editor, I personally recommend:
    - [Visual Studio Code](https://code.visualstudio.com/)
    - [Sublime Text](https://www.sublimetext.com/)
    - [Notepad++](https://notepad-plus-plus.org/)
- The default strings file, you can find it [here](https://github.com/Haato3o/HunterPie-v2/blob/main/HunterPie/Languages/en-us.xml)

> **Tip**: Do **not** use the default Windows's notepad, that can cause file encoding issues and will make HunterPie fail to load your file. Make sure your encoding file is UTF-8.
{: .prompt-note}

## Localizing

Now thhat you have the requirements, it's time to start translating!
Use the `en-us.xml` you downloaded in the last step as a base file, it contains all the currently supported strings that are used by both the HunterPie client and the Overlay and the integrations.

> **Warning:** Make sure to rename your file to something else so it doesn't conflict with the default files. If you don't rename it, there's a risk your local file will be overwritten during the auto-update process.
{: .prompt-danger }

Do not touch the first line of the XML Document, that is telling our XML parser the version and file encoding we are using. It should **ALWAYS** be the first line of the file and should always look like this:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<!-- Comments can be added ONLY after the tag -->
```
{: file='ja-jp.xml'}

Next step is to set the language name, you can do it in the next Tag by changing the value of the `lang` attribute.

```xml
<!-- Before -->
<Strings lang="English">

<!-- After -->
<Strings lang="Japanese">
```
{: file='ja-jp.xml'}

Now we're can translate the rest of the file, most localization `Tags` have three attributes:

- **Id:** Used internally by HunterPie to query the string and description. **DO NOT TOUCH THIS VALUE**
- **String:** The actual string that will be shown in HunterPie's interface.
- **Description:** This one is only used in places where there can be a tooltip, e.g: Settings, Buttons

> Please, try to keep all translations as accurate as possible with the in-game strings (especially abnormalities and monster names). You can shorten long strings as long as they don't become difficult to understand.
{: .prompt-info }

E.g:

```xml
<!-- Before -->
<Rise>
    <Monster Id="0" String="Rathian"/>
    <Monster Id="1" String="Apex Rathian"/>
    <Monster Id="2" String="Rathalos"/>
    <Monster Id="3" String="Apex Rathalos"/>
    <Monster Id="4" String="Khezu"/>
    <Monster Id="5" String="Basarios"/>
    [...]

<!-- After -->
<Rise>
    <Monster Id="0" String="リオレイア"/>
    <Monster Id="1" String="ヌシ・リオレイア"/>
    <Monster Id="2" String="リオレウス"/>
    <Monster Id="3" String="ヌシ・リオレウス"/>
    <Monster Id="4" String="フルフル"/>
    <Monster Id="5" String="バサルモス"/>
    [...]
```
{: file='ja-jp.xml'}

## XML Special Characters

XML has some characters that should be replaced by their escaped versions in order for it to work. This is because these characters are used in the XML's structure itself, and the parser has a hard time figuring out whether it's a XML character or if it's just a normal character.

Character | Replaced by
:--------:|:-------------------:
&         | `&amp;`
<         | `&lt;`
\>        | `&gt;`
"         | `&quot;`
'         | `&apos;`

E.g:

```xml
<!-- This will give us an error -->
<Abnormality Id="ABNORMALITY_ATTACK_DEF_UP" String="Attack & Def. Up"/>

<!-- This will work -->
<Abnormality Id="ABNORMALITY_ATTACK_DEF_UP" String="Attack &amp; Def. Up"/>
```
{: file='en-us.xml'}

## Sending my localization

Now that you've finished translating all strings, you can open a Pull Request to HunterPie's [repository](https://github.com/Haato3o/HunterPie-v2) or send it in the [#translation-discussion](https://discord.gg/xUnKhFKrbs) channel in HunterPie's official Discord Server.

> Making your own Pull Request is the preferred way, since it will also mark you as one of HunterPie's contributors in GitHub, but if you have no Git experience sending the file directly to me on Discord is also fine.
{: .prompt-note }
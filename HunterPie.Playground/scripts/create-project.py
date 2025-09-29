import tui
import uuid
import os
import json


def create_project():
    print("So... I see you're trying to create a new theme, that's great!")
    print("First, we need some information:")
    print("(don't worry, you can change these at any moment)")

    name = tui.ask_input("What's the name of your theme?")
    description = tui.ask_input("Give it a description!")
    author = tui.ask_input("Author:")
    tags = tui.ask_choices("Pick some tags for your theme", ["overlay", "client", "colors", "misc"])

    print(f"Creating new theme \"{name}\"")

    folder_name = f"./Themes/{name}"
    os.mkdir(folder_name)

    with open(folder_name + "/theme.manifest.json", "w", encoding="utf-8") as manifest:
        json.dump({
            "id": str(uuid.uuid4()),
            "name": name,
            "description": description,
            "version": "1.0.0",
            "author": author,
            "tags": tags
        }, manifest, ensure_ascii=False, indent=2)

if __name__ == "__main__":
    create_project()
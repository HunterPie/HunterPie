import os
from typing import List
from pathlib import Path
import pefile
import zipfile

IGNORE_FILES = [
    Path("config.json"),
    Path("libs/HunterPie.Native.pdb"),
    Path("HunterPie_Log.txt"),
    Path("internal"),
    Path("Assets/Cache"),
    Path("Assets/Monsters/Icons"),
    Path('package.py'),
    Path('deploy')
]

def get_hunterpie_version() -> str:
    file_pe = pefile.PE('HunterPie.exe')

    ms = file_pe.VS_FIXEDFILEINFO[0].ProductVersionMS
    ls = file_pe.VS_FIXEDFILEINFO[0].ProductVersionLS

    major = (ms >> 16) & 0xFFFF
    minor = ms & 0xFFFF
    build = (ls >> 16) & 0xFFFF
    private = ls & 0xFFFF

    return f"{major}.{minor}.{build}.{private}"

def list_files(root: Path) -> List[Path]:
    paths: List[Path] = []

    for entry in os.listdir(root):
        path = root.joinpath(entry)

        if path in IGNORE_FILES:
            continue

        if path.is_dir():
            paths += list_files(path)
            continue

        paths.append(path)

    return paths

def create_package():
    try:
        os.makedirs("deploy")
        print("created deploy folder")
    except:
        pass

    version = get_hunterpie_version()
    files = list_files(Path(''))

    package_path = f"deploy/{version}.zip"

    with zipfile.ZipFile(package_path, "w", zipfile.ZIP_DEFLATED) as package:
        for file in files:
            package.write(file)

    print(f"created package at {package_path}")

if __name__ == "__main__":
    create_package()
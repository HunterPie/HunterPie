import os
from typing import List
from pathlib import Path
import pefile
import zipfile
import sys
import logging

log = logging.getLogger("packager")

log.setLevel("INFO")

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

def get_hunterpie_version(base_path: Path) -> str:
    file_pe = pefile.PE(base_path / 'HunterPie.exe')

    ms = file_pe.VS_FIXEDFILEINFO[0].ProductVersionMS
    ls = file_pe.VS_FIXEDFILEINFO[0].ProductVersionLS

    major = (ms >> 16) & 0xFFFF
    minor = ms & 0xFFFF
    build = (ls >> 16) & 0xFFFF
    private = ls & 0xFFFF

    return f"{major}.{minor}.{build}.{private}"

def list_files(root: Path, ignore_list: List[Path]) -> List[Path]:
    paths: List[Path] = []
    
    for entry in os.listdir(root):
        path = root.joinpath(entry)

        if path in ignore_list:
            log.warning(f"skipped {path} due to it being in the ignored files list")
            continue

        if path.is_dir():
            paths += list_files(path, ignore_list)
            continue

        paths.append(path)

    return paths

def create_package(base_path: Path):
    deploy_path = Path("bin/deploy")
    try:
        os.makedirs(deploy_path)
        print("created deploy folder")
    except:
        pass

    version = get_hunterpie_version(base_path)
    files = list_files(base_path, [
        base_path / p for p in IGNORE_FILES
    ])

    package_path = deploy_path / f"{version}.zip"

    with zipfile.ZipFile(package_path, "w", zipfile.ZIP_DEFLATED) as package:
        for file in files:
            relative_path = file.relative_to(base_path)
            log.info(f"compacting file: {relative_path}")
            package.write(file, relative_path)

    print(f"created package at {package_path}")

if __name__ == "__main__":
    project_path = Path(sys.argv[1]) if len(sys.argv) >= 2 else Path("")

    create_package(project_path)
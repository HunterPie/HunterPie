import pefile
from pathlib import Path
import sys

def get_hunterpie_version(base_path: Path) -> str:
    file_pe = pefile.PE(base_path / 'HunterPie.exe')

    ms = file_pe.VS_FIXEDFILEINFO[0].ProductVersionMS
    ls = file_pe.VS_FIXEDFILEINFO[0].ProductVersionLS

    major = (ms >> 16) & 0xFFFF
    minor = ms & 0xFFFF
    build = (ls >> 16) & 0xFFFF
    private = ls & 0xFFFF

    return f"{major}.{minor}.{build}.{private}"

if __name__ == "__main__":
    project_path = Path(sys.argv[1]) if len(sys.argv) >= 2 else Path("")

    print(get_hunterpie_version(project_path))
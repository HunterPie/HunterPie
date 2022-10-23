import subprocess
from typing import Any, List

import xmltodict
from typing_extensions import Self

# Constants
GIT_DIFF_COMMAND = "git diff origin/main HEAD --name-only --diff-filter=d"

class LocalizationPipeline:
    def __init__(self, files: List[str]):
        self.files = files

    def filter_localizations(self) -> Self:
        self.files = [file for file in self.files if "Languages" in file and file.endswith(".xml")]
        return self

    def validate_files(self) -> Self:
        for file in self.files:
            try:
                with open(file, "r") as xml_file:
                    xmltodict.parse(xml_file.read())
            except Exception as err:
                print(err)
                exit(1)
        return self

    def finish(self):
        exit(0)

def execute_command(command: str) -> List[str]:
    result = subprocess.Popen(
        command,
        shell=True,
        stdin=None,
        stdout=subprocess.PIPE,
        stderr=subprocess.PIPE
    ).stdout

    if result is None:
        return []
    
    return [file.decode("utf-8").replace('\n', '') for file in result.readlines()]

if __name__ == "__main__":
    pipeline = LocalizationPipeline(
        execute_command(GIT_DIFF_COMMAND)
    )

    pipeline.filter_localizations() \
            .validate_files() \
            .finish()
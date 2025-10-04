create/theme:
	py ./scripts/create-project.py


deploy/debug:
	xcopy .\\Themes\\* ..\\HunterPie\\bin\\Debug\\net8.0-windows7.0\\ /E /Y

deploy/release:
	powershell -Command "Copy-Item -Path '.\\Themes\\*' -Destination '..\\HunterPie\\bin\\Release\\net8.0-windows7.0\\' -Recurse -Force"
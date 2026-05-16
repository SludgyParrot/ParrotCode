@echo off
for /d /r . %%D in (obj) do (
    if exist "%%D" (
        echo Deleting folder from path: "%%D"
       : rd /s /q "%%D"
    )
)
pause
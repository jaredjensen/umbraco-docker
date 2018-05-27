FROM microsoft/aspnet:latest
RUN mkdir C:\site
RUN powershell -NoProfile -Command \
    Import-Module IISAdministration; \
    New-IISSite -Name "Site" -PhysicalPath C:\site -BindingInformation "*:8000:"
EXPOSE 8000
ADD ./site /site

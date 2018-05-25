FROM microsoft/mssql-server-windows-express:2017-latest
ARG saPassword
RUN mkdir C:\data
VOLUME "C:/data"
EXPOSE 1433
ENV sa_password=$saPassword
ENV ACCEPT_EULA=Y
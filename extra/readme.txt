descargar e instalar docket desde https://www.docker.com/

Pasos para activar WSL 2:
Abre PowerShell como administrador y ejecuta:

wsl --install

Esto instalará WSL 2 y Ubuntu por defecto (puedes elegir otras distros luego).
Reinicia tu PC si lo pide.
Verifica que WSL 2 esté activo con:

wsl --list --verbose

Ejecutar desde la ruta donde se encuentra el archivo docker-compose.yml el siguiente comando:
docker-compose up --build

el proceso ejecutara lo siguiente:
se descargara una imagen de Sql server.
se descargara una imagen del proyecto ejemplomvc.
se ejecutara el script para la creacion de la base de datos.
se ejecutaran los contenedores

en cuanto termine ingresar a la url: http://localhost:8080/
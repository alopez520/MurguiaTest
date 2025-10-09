## En este test, evaluaremos tus habilidades y conocimientos técnicos.

### ¿Como se te evaluara?
Se evaluará la lógica, técnicas, procesos y buenas prácticas que implementes, así como la forma de solucionar los problemas.

### Descripción del reto
Crear una web app To Do List, en la cual puedas administrar (CRUD) tus tareas y destacar las más importantes.

Las tecnologías más importantes que debes utilizar son .NET (Back), JavaScript, HTML y CSS (Front), siéntete libre de utilizar alguna otra con la que estes familiarizado.
Debes implementarlo preferentemente en arquitectura MVC.
Puedes utilizar un archivo de texto o csv como base de datos.

Debes subir tus cambios a la rama con tu nombre de usuario para poder evaluarte (Ej: test_Alex35)

##Test update


#### test_PaolaBVM
Iniciamos con un login, donde hay un INICIO SESIÓN y un  REGISTRO
*Inician Sesión solo los que ya se encuentran registrados
*Se pueden registrar los usuarios que necesitan acceso al sitio.


Este sistema consiste en dos usuarios: Administrador y Usuario

Rol Administrador (inicia sesión, user: admin@correo.com pass: admin123)
*Agregar tareas asignado usuarios que ya esten registrados en el sistema
*Visualizar todas las tareas agregadas (generales)
*Eliminar tareas
*Editar tareas
*Ver detalle de sus tareas

Rol Usuario (inicia sesión, user:alex@correo.com pass:123456 Y user:paola@correo.com pass: pao123)
*Agregar tareas solo para ellos mismos (ejem: Alex solo puede asiganarse una tarea a si mismo y a nadie más)
*Visualizar solo sus tareas que se fueron asignadas o que el agrego
*Eliminar sus tareas
*Editar sus tareas
*Ver detalle de sus tareas

Puntos a destacar 
* Existe un estatus de prioridad de las tareas, resaltando cuales son las más importantes 
* Solo administrador puede asignar a los usuarios que ya se encuentren registrados en el sistema (select)
* Los usuarios (normales) solo pueden ver sus tareas asignadas y no la de los demás usuarios. 
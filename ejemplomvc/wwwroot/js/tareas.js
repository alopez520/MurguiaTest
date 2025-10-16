async function cargarTareas() {
    showLoading();
    try {

        const response = await fetch('/api/tareas');

        if (!response.ok) {
            const data = await response.json();
            await Swal.fire({
                title: 'Error',
                text: data.mensaje || "Ocurrió un error inesperado",
                icon: 'error',
            });
            return;
        }

        const tareasResponse = await response.json();

        const tbody = document.querySelector('#tareasTable tbody');
        tbody.innerHTML = '';

        if (tareasResponse.datos.length === 0) {
            const fila = document.createElement('tr');
            const celda = document.createElement('td');
            celda.colSpan = 4;
            celda.textContent = 'No hay tareas disponibles.';
            celda.style.textAlign = 'center';
            fila.appendChild(celda);
            tbody.appendChild(fila);
            return;
        }

        tareasResponse.datos.forEach(tarea => {
            const tr = document.createElement('tr');
            tr.setAttribute('data-id', tarea.id);
            if (tarea.destacada) {
                tr.classList.add('tarea-inportante');
            }

            const tdCheck = document.createElement('td');
            if (tarea.completada) {
                tdCheck.style.backgroundColor = "lightgreen";
            }

            //
            const label = document.createElement('label');
            label.className = 'checkbox-custom';

            const checkbox = document.createElement('input');
            checkbox.type = 'checkbox';
            checkbox.checked = tarea.completada;
            checkbox.addEventListener('change', () => toggleCompletada(tarea.id, checkbox.checked));
            const checkmark = document.createElement('span');
            checkmark.className = 'checkmark';
            label.appendChild(checkbox);
            label.appendChild(checkmark);
            tdCheck.appendChild(label);
            tdCheck.style.textAlign = 'center';
            //

            const tdTitulo = document.createElement('td');
            tdTitulo.textContent = tarea.titulo;

            const tdDescripcion = document.createElement('td');
            tdDescripcion.textContent = tarea.descripcion;

            const tdAcciones = document.createElement('td');
            tdAcciones.style.textAlign = 'center';

            const btnEditar = document.createElement('button');
            btnEditar.textContent = 'Editar';
            btnEditar.addEventListener('click', () => abrirModalEdicion(tarea));

            const btnEliminar = document.createElement('button');
            btnEliminar.textContent = 'Eliminar';
            btnEliminar.addEventListener('click', () => dialogConfirmaEliminacion(tarea.id));


            const btnDestacar = document.createElement('button');
            btnDestacar.textContent = 'Destacar';
            btnDestacar.addEventListener('click', () => detacarTarea(tarea.id, tarea.destacada));

            tdAcciones.appendChild(btnEditar);
            tdAcciones.appendChild(btnEliminar);
            tdAcciones.appendChild(btnDestacar);

            tr.appendChild(tdCheck);
            tr.appendChild(tdTitulo);
            tr.appendChild(tdDescripcion);
            tr.appendChild(tdAcciones);

            tbody.appendChild(tr);
        });
    } catch (error) {
        console.error('Error cargando tareas:', error);
    } finally {
        closeLoading();
    }
}

async function toggleCompletada(id, completada) {
    try {
        const completadaResponse = await fetch(`/api/tareas/${id}`, {
            method: "PATCH",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ Completada: completada })
        });

        const data = await completadaResponse.json();
        if (!completadaResponse.ok) {
            Swal.fire({
                title: 'Error',
                text: data.mensaje || "Ocurrió un error inesperado",
                icon: 'error',
            });
            return;
        } else {
            await cargarTareas();
            Swal.fire({
                title: 'Éxito',
                text: data.mensaje,
                icon: 'success',
            });
        }
    } catch (error) {
        console.error('Error actualizando tarea:', error);
    }
}

//manejo del modal y agregar tarea
const modal = document.getElementById('modal');
const btnAgregar = document.getElementById('btnAgregar');
const btnCerrar = document.getElementById('cerrarModal');
const btnGuardar = document.getElementById('guardarTarea');
const inputTitulo = document.getElementById('titulo');
const inputDescripcion = document.getElementById('descripcion');

btnAgregar.addEventListener('click', () => {
    tareaEditandoId = null;
    inputTitulo.value = '';
    inputDescripcion.value = '';
    modal.style.display = 'flex';
});

btnCerrar.addEventListener('click', () => {
    modal.style.display = 'none';
});

btnGuardar.addEventListener('click', async () => {
    agregarEditarTarea(tareaEditandoId);
    modal.style.display = 'none';
    document.getElementById('titulo').value = '';
    document.getElementById('descripcion').value = '';
});

function abrirModalEdicion(tarea) {
    tareaEditandoId = tarea.id;
    inputTitulo.value = tarea.titulo;
    inputDescripcion.value = tarea.descripcion;
    modal.style.display = 'flex';
}

async function agregarEditarTarea(id) {
    const titulo = inputTitulo.value.trim();
    const descripcion = inputDescripcion.value.trim();

    if (!titulo) {
        Swal.fire({
            title: 'Error',
            text: 'El título es obligatorio',
            icon: 'error',
        });
        return;
    }

    try {
        showLoading();
        var addUpdateResponse;
        if (id) {
            // Editar tarea
            addUpdateResponse = await fetch(`/api/tareas/${id}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ id: id, titulo, descripcion, completada: false })
            });
        } else {
            // Crear nueva tarea
            addUpdateResponse = await fetch('/api/tareas', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ id: 0, titulo, descripcion, completada: false })
            });
        }
        const data = await addUpdateResponse.json();
        await closeLoading();
        if (!addUpdateResponse.ok) {
            await cargarTareas();
            Swal.fire({
                title: 'Error',
                text: data.mensaje || "Ocurrió un error inesperado",
                icon: 'error',
            });
            return;
        } else {
            await cargarTareas();
            scrollToTarea(id);
            Swal.fire({
                title: 'Éxito',
                text: data.mensaje,
                icon: 'success',
            });
        }
    } catch (error) {
        console.error('Error agregando tarea:', error);
    }
}

async function dialogConfirmaEliminacion(id) {
    Swal.fire({
        title: '¿Deseas eliminar esta tarea?',
        text: "No podrás recuperarla después.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            eliminarTarea(id);
        }
    });
}

async function eliminarTarea(id) {
    try {
        showLoading();
        const eliminarResponse = await fetch(`/api/tareas/${id}`, { method: 'DELETE' });
        const data = await eliminarResponse.json();
        await closeLoading();
        if (!eliminarResponse.ok) {
            Swal.fire({
                title: 'Error',
                text: data.mensaje || "Ocurrió un error inesperado",
                icon: 'error',
            });
            return;
        } else {
            await cargarTareas();
            Swal.fire({
                title: 'Éxito',
                text: data.mensaje,
                icon: 'success',
            });
        }

    } catch (error) {
        console.error('Error eliminando tarea:', error);
    }
}

async function detacarTarea(id, destacada) {
    try {
        const completadaResponse = await fetch(`/api/tareas/${id}`, {
            method: "PATCH",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ Destacada: !destacada })
        });

        const data = await completadaResponse.json();
        if (!completadaResponse.ok) {
            Swal.fire({
                title: 'Error',
                text: data.mensaje || "Ocurrió un error inesperado",
                icon: 'error',
            });
            return;
        } else {
            await cargarTareas();
            scrollToTarea(id,true);
            await Swal.fire({
                title: 'Éxito',
                text: data.mensaje,
                icon: 'success',
            });
            
        }
    } catch (error) {
        console.error('Error actualizando tarea:', error);
    }
}
function showLoading() {
    Swal.fire({
        title: 'Cargando...',
        allowOutsideClick: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });
}
function closeLoading() {
    Swal.close();
}

function scrollToTarea(id,scroll) {
    const fila = document.querySelector(`tr[data-id="${id}"]`);
    if (fila && scroll) {
        fila.scrollIntoView({
            behavior: 'smooth',   
            block: 'center'
        });
    }
    fila.classList.add('resaltado');
    setTimeout(() => fila.classList.remove('resaltado'), 1000);
}

cargarTareas();
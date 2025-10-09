// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", () => {
    const table = document.getElementById("taskContainer");
    const btnAgregar = document.getElementById("btnAdd");
    const btnEditar = document.getElementById("btnEdit");
    const searchBar = document.getElementById("searchForm");
    const modal = bootstrap.Modal.getInstance(document.getElementById("miModalAdd"));

    getTable();

    btnAgregar.addEventListener("click", () => {
        const modal = bootstrap.Modal.getInstance(document.getElementById("miModalAdd"));
        const titleView = document.getElementById("title").value;
        const descriptionView = document.getElementById("description").value;
        const dueDateView = document.getElementById("duedate").value;

        if (!titleView || !descriptionView || !dueDateView) {
            Swal.fire({
                icon: "warning",
                title: "Campos requeridos",
                text: "Por favor, completa todos los campos antes de guardar."
            });
            return;
        }

        fetch("/Task/Create", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                Title: titleView,
                Description: descriptionView,
                DueDate: dueDateView
            })
        }).then(response => {
            if (!response.ok) throw new Error("Error en la petición");
            return response.json();
        })
            .then(data => {
                if (data.success) {
                    console.log("Usuario creado:", data);
                    modal.hide();
                    Swal.fire({
                        title: "¡Tarea Creada!",
                        text: "La tarea se creo con éxito.",
                        icon: "success",
                        confirmButtonText: "Aceptar"
                    });
                    getTable();
                    document.getElementById('formUsuario').reset();
                }
                else {
                    modal.hide();
                    Swal.fire({
                        title: "¡Error!",
                        text: data.message,
                        icon: "error",
                        confirmButtonText: "Aceptar"
                    });
                    getTable();
                    document.getElementById('formUsuario').reset();
                }
            })
            .catch(error => {
                console.error("Hubo un error:", error);
            });
        modal.hide();
    });

    table.addEventListener("click", (event) => {
        const icon = event.target;
        const id = event.target.dataset.id;

        // Estrella (importante)
        if (icon.classList.contains("bi-star") || icon.classList.contains("bi-star-fill")) {
            const item = icon.closest(".accordion-item");
            const isImportant = icon.classList.contains("bi-star");

            if (isImportant) {
                icon.classList.remove("bi-star", "text-secondary");
                icon.classList.add("bi-star-fill", "text-warning");
                item.dataset.important = "true";
                editImportant(true);

            } else {
                icon.classList.remove("bi-star-fill", "text-warning");
                icon.classList.add("bi-star", "text-secondary");
                item.dataset.important = "false";
                editImportant(false);
            }
            function editImportant(important) {
                fetch(`/Task/EditImportant/${id}`, {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify(important)
                })
                    .then(response => response.json())
                    .then(data => {
                        if (data.success) {
                            console.log(data.message)
                            getTable();
                        }
                        else {
                            Swal.fire({
                                title: "¡Error!",
                                text: data.message,
                                icon: "error",
                                confirmButtonText: "Aceptar"
                            });
                        }
                    })
                    .catch(error => console.error(error))
            }
        }

        // Completado
        if (icon.classList.contains("bi-circle") || icon.classList.contains("bi-check-circle-fill")) {
            const item = icon.closest(".accordion-item");
            const isCompleted = icon.classList.contains("bi-circle");

            if (isCompleted) {
                icon.classList.remove("bi-circle", "text-secondary");
                icon.classList.add("bi-check-circle-fill", "text-success");
                item.dataset.completed = "true";
                editCompleted(true);
            } else {
                icon.classList.remove("bi-check-circle-fill", "text-success");
                icon.classList.add("bi-circle", "text-secondary");
                item.dataset.completed = "false";
                editCompleted(false);
            }
            function editCompleted(completed) {
                fetch(`/Task/EditCompleted/${id}`, {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify(completed)
                })
                    .then(response => response.json())
                    .then(data => {
                        if (data.success) {
                            console.log(data.message)
                            getTable();
                        }
                        else {
                            Swal.fire({
                                title: "¡Error!",
                                text: data.message,
                                icon: "error",
                                confirmButtonText: "Aceptar"
                            });
                        }
                    })
                    .catch(error => console.error(error))
            }
        }

        if (icon.matches(".editar-btn")) {
            const editTitle = document.getElementById("E_title");
            const editDescription = document.getElementById("E_description");
            const editDueDate = document.getElementById("E_duedate");
            const editId = document.getElementById("E_id");
            fetch(`/Task/GetId/${id}`)
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        console.log(data);
                        editTitle.value = data.obj.title;
                        editDescription.value = data.obj.description;
                        editDueDate.value = data.obj.fecha;
                        editId.value = data.obj.id;
                    }
                    else {
                        Swal.fire({
                            title: "¡Error!",
                            text: data.message,
                            icon: "error",
                            confirmButtonText: "Aceptar"
                        });
                    }
                })
                .catch(error => {
                    console.error(error);
                })
            console.log(id);
        }

        if (icon.matches(".eliminar-btn")) {
            console.log(event);
            console.log(id);
            Swal.fire({
                title: "¿Estás seguro?",
                text: "Esta acción eliminará la tarea permanentemente.",
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#d33",
                cancelButtonColor: "#3085d6",
                confirmButtonText: "Sí, eliminar",
                cancelButtonText: "Cancelar"
            }).then((result) => {
                if (result.isConfirmed) {

                    fetch(`/Task/Delete/${id}`, { method: "POST" })
                        .then(response => response.json())
                        .then(data => {
                            if (data.success) {
                                console.log(data);
                                getTable();
                                Swal.fire({
                                    title: "¡Elimnado!",
                                    text: "La tarea se Elimino con éxito.",
                                    icon: "success",
                                    confirmButtonText: "Aceptar"
                                });
                            }
                            else {
                                Swal.fire({
                                    title: "¡Error!",
                                    text: data.message,
                                    icon: "error",
                                    confirmButtonText: "Aceptar"
                                });
                            }
                        })
                        .catch(error => console.error("Error al eliminar:", error));
                }
            });
        }
    });

    btnEditar.addEventListener("click", () => {
        const modal = bootstrap.Modal.getInstance(document.getElementById("miModalEdit"));
        const editTitle = document.getElementById("E_title").value;
        const editDescription = document.getElementById("E_description").value;
        const editDueDate = document.getElementById("E_duedate").value;
        const editId = document.getElementById("E_id").value;

        if (!editTitle || !editDescription || !editDueDate) {
            Swal.fire({
                icon: "warning",
                title: "Campos requeridos",
                text: "Por favor, completa todos los campos antes de guardar."
            });
            return;
        }

        fetch("/Task/Edit", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                id: editId,
                Title: editTitle,
                Description: editDescription,
                DueDate: editDueDate
            })
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    console.log(data);
                    modal.hide();
                    Swal.fire({
                        title: "¡Tarea Actualizada!",
                        text: "La tarea se Actualizo con éxito.",
                        icon: "success",
                        confirmButtonText: "Aceptar"
                    });
                    getTable();
                }
                else {
                    modal.hide();
                    Swal.fire({
                        title: "¡Error!",
                        text: data.message,
                        icon: "error",
                        confirmButtonText: "Aceptar"
                    });
                    getTable();
                    //document.getElementById('formUsuario').reset();
                }
            })
            .catch(error => console.error("Error al eliminar:", error));
    });

    searchBar.addEventListener("submit", function (e) {
        e.preventDefault();
        const query = document.getElementById("searchInput").value;
        if (query.trim() === "") {
            getTable();
            return;
        }
        fetch(`/Task/Search?datos=${encodeURIComponent(query)}`)
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    console.log(data);
                    table.innerHTML = data.obj.map(stringTask).join('');
                }
                else {
                    Swal.fire({
                        title: "¡Error!",
                        text: data.message,
                        icon: "error",
                        confirmButtonText: "Aceptar"
                    });
                }
            })
            .catch(error => console.error("Error en búsqueda:", error));
    })

    function getTable() {
        fetch("/Task/Get")
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    console.log(data);
                    table.innerHTML = data.obj.map(stringTask).join('');
                }
                else {
                    Swal.fire({
                        title: "¡Error!",
                        text: data.message,
                        icon: "error",
                        confirmButtonText: "Aceptar"
                    });
                }
            })
            .catch(error => {
                console.error("Hubo un error:", error);
            })
    }
    function stringTask(task) {
        return `
    <div class="accordion-item border rounded shadow-sm mb-2" data-id="${task.id}" data-important="${task.important}" data-completed="${task.completed}">
    <h2 class="accordion-header d-flex align-items-center justify-content-between px-3 py-2 bg-light">
        <!-- Check a la izquierda -->
        <i class="bi ${task.completed ? 'bi-check-circle-fill text-success' : 'bi-circle text-secondary'} fs-5" data-id="${task.id}" style="cursor: pointer;" title="Marcar como completada"></i>

        <!-- Botón del accordion -->
        <button class="accordion-button collapsed flex-grow-1 mx-3 bg-light border-0" type="button"
                data-bs-toggle="collapse" data-bs-target="#collapse-${task.id}"
                aria-expanded="false" aria-controls="collapse-${task.id}">
            <div class="d-flex flex-column text-start">
                <span class="fw-semibold">${task.title}</span>
                <small class="text-muted">
                    <i class="bi bi-calendar-event me-1"></i> ${task.fecha}
                </small>
            </div>
        </button>

        <!-- Estrella a la derecha -->
        <i class="bi ${task.important ? 'bi-star-fill text-warning' : 'bi-star text-secondary'} fs-5" data-id="${task.id}" style="cursor: pointer;" title="Marcar como importante"></i>
    </h2>

    <div id="collapse-${task.id}" class="accordion-collapse collapse" data-bs-parent="#accordionExample">
        <div class="accordion-body">
            <p><strong>Descripción:</strong> ${task.description}</p>

            <div class="d-flex gap-2 mt-3">
                <button class="btn btn-outline-primary btn-sm editar-btn" data-bs-toggle="modal" data-bs-target="#miModalEdit" data-id="${task.id}">
                    <i class="bi bi-pencil-square"></i> Editar
                </button>
                <button class="btn btn-outline-danger btn-sm eliminar-btn" data-id="${task.id}">
                    <i class="bi bi-trash"></i> Eliminar
                </button>
            </div>
        </div>
    </div>
    </div>`;
    }
})
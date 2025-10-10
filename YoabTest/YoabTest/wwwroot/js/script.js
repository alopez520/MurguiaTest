function editTask(id) {
    const row = document.querySelector(`tr td button[onclick='editTask(${id})']`).closest("tr");

    //Ellena el formulario con los datos existentes de la fila
    document.querySelector('#taskForm input[name="Id"]').value = id;
    document.querySelector('#taskForm input[name="Title"]').value = row.cells[0].innerText;
    document.querySelector('#taskForm textarea[name="Description"]').value = row.cells[1].innerText;
    document.querySelector('#taskForm button[type="submit"]').textContent = "Actualizar";

    const dateText = row.cells[2].dataset.date;
    if (dateText) {
        document.querySelector('#taskForm input[name="Deadline"]').value = dateText;
    }

    document.querySelector('#taskForm input[name="IsImportant"]').checked = row.classList.contains("important");
}

async function toggleImportant(id) {
    try {
        const response = await fetch(`/Task/ToggleImportant/${id}`, {
            method: "POST",
            headers: {
                "X-Requested-With": "XMLHttpRequest",
                "Content-Type": "application/json"
            }
        });

        const result = await response.json();

        if (result.success) {
            const button = document.querySelector(`button[onclick='toggleImportant(${id})']`);
            if (button) {
                const isNowImportant = button.classList.toggle("important");
                button.textContent = isNowImportant ? "★ Importante" : "☆ Importante";
            }
        } else {
            console.error("Error al actualizar la importancia.");
        }
    } catch (error) {
        console.error("Error al conectar con el servidor:", error);
    }
}

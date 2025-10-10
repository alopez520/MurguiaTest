function editTask(id) {
    const button = document.querySelector(`button[onclick='editTask(${id})']`);
    if (!button)
        return;

    const row = button.closest("tr");
    if (!row)
        return;

    document.querySelector('#taskForm input[name="Id"]').value = id;
    document.querySelector('#taskForm input[name="Title"]').value = row.cells[0].innerText.trim();
    document.querySelector('#taskForm textarea[name="Description"]').value = row.cells[1].innerText;
    document.querySelector('#taskForm button[type="submit"]').textContent = "Actualizar";
}
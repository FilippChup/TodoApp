
console.log("JS подключен!");
document.addEventListener("DOMContentLoaded", async function () {
    const addButton = document.getElementById('addButton');
    const deleteTask = document.getElementsByClassName('delete-task');
    const taskInput = document.getElementById('taskInput');
    const tbody = document.getElementById('tbody');
    // const pendingTasks = document.querySelector('.pendingTasks');

    // Функция для активации/деактивации кнопки Add
    function toggleAddButton() {
        // Включаем кнопку, если есть текст в поле ввода, иначе отключаем
        if (taskInput.value.trim() !== '') {
            console.log('1')
            addButton.disabled = false;
        } else {
            console.log('2')
            addButton.disabled = true;
        }
    }

    // Обработчик изменения текста в поле ввода
    taskInput.addEventListener('input', toggleAddButton);

    // Инициализация: проверка текста в поле ввода при загрузке страницы
    toggleAddButton(); // Вызываем функцию для первоначальной активации кнопки

    // Функция для обновления количества задач
    // function updatePendingTasksCount() {
    //     const taskCount = tbody.querySelectorAll('tr').length;
    //     pendingTasks.textContent = taskCount;
    // }
    // Обработчик нажатия на кнопку Add
    addButton.addEventListener('click', async function () {
        const taskName = taskInput.value.trim();
        if (!taskName) return;  // Если поле пустое, не делать ничего

        // Создаём задачу
        const newTodo = {
            Name: taskName,
            Priority: "High",  // По умолчанию задаём приоритет "High"
            IsComplete: false
        };

        try {
            console.log("hello world");
            const response = await fetch('/api/todoitems', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(newTodo)
            });
            console.log("hello body");
            if (!response.ok) {
                alert("Ошибка при добавлении задачи");
                return;
            }

            const savedTodo = await response.json(); // Получаем добавленную задачу
            // Добавляем задачу в таблицу
            const row = document.createElement('tr');
            row.dataset.id = savedTodo.id;
            row.classList.add('task-row');
            row.innerHTML = `
                <td>${savedTodo.id}</td>
                <td>${savedTodo.name}</td>
                <td>${savedTodo.priority}</td>
                <td class="status-cell" style="cursor:pointer">${savedTodo.IsComplete ? "✅" : "❌"}</td>
                <td><button class="delete-task">Delete</button></td>
            `;
            tbody.appendChild(row);

            // Обновляем количество задач


            // Очищаем поле ввода и деактивируем кнопку
            taskInput.value = '';
            toggleAddButton(); // Деактивируем кнопку после добавления
        } catch (err) {
            console.error(err);
            alert("Не удалось подключиться к серверу");
        }
    })

    // Обработчик для изменения статуса задачи
    // tbody.addEventListener('click', async function(e) {
    //     if (e.target.classList.contains('status-cell')) {
    //         const row = e.target.closest('tr');
    //         const id = row.dataset.id;
    //
    //         try {
    //             // Запрос на изменение статуса задачи
    //             const response = await fetch(`/api/${id}/toggle`, {
    //                 method: 'PUT'
    //             });
    //
    //             if (!response.ok) {
    //                 alert("Ошибка при изменении статуса");
    //                 return;
    //             }
    //
    //             const updatedTodo = await response.json();
    //             e.target.textContent = updatedTodo.isComplete ? "✅" : "❌";
    //         } catch (err) {
    //             console.error(err);
    //             alert("Ошибка соединения с сервером");
    //         }
    //     }
    //
        // Удаление задачи
        document.addEventListener("click", async function (e) {
            if (e.target.classList.contains("delete-task")) {
                const row = e.target.closest("tr");
                const id = row.dataset.id;
                try {
                    // Отправка запроса на удаление задачи
                    
                    console.log("Мы находимся перед запросом");
                    console.log("ID перед отправкой:", id);
                    const response = await fetch(`/api/todoitems/${id}`, {
                        method: 'DELETE'
                    });

                    console.log("Мы находимся после запроса");
                    if (!response.ok) {
                        alert("Ошибка при удалении задачи");
                    }

                    console.log("Что-то пошло не так");
                    row.remove();
                    console.log("task deleted")
                    // updatePendingTasksCount();
                    debugger
                    const rows = document.querySelectorAll("#tbody tr");
                    rows.forEach((r, index) => {
                        r.querySelector("td:first-child").textContent = index + 1;
                    });
                    
                } catch (err) {
                    console.error(err);
                    alert("Ошибка соединения с сервером");
                }
            }
        })
    });
    // Инициализируем количество задач
    // updatePendingTasksCount();
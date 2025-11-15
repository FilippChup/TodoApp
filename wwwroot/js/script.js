
console.log("JS подключен!");
document.addEventListener("DOMContentLoaded", function() {
    const addButton = document.getElementById('addButton');
    const taskInput = document.getElementById('taskInput');
    const tbody = document.getElementById('myTableBody');
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
    addButton.addEventListener('click', async function() {
        const taskName = taskInput.value.trim();
        if (!taskName) return;  // Если поле пустое, не делать ничего

        // Создаём задачу
        const newTodo = {
            Name: taskName,
            Priority: "High",  // По умолчанию задаём приоритет "High"
            IsComplete: false
        };

        try {
            // Отправка запроса на сервер для добавления задачи
            const response = await fetch('/api/', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(newTodo)
            });

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
            // updatePendingTasksCount();

            // Очищаем поле ввода и деактивируем кнопку
            taskInput.value = '';
            toggleAddButton(); // Деактивируем кнопку после добавления
        } catch (err) {
            console.error(err);
            alert("Не удалось подключиться к серверу");
        }
    });

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
    //     // Удаление задачи
    //     if (e.target.classList.contains('delete-task')) {
    //         const row = e.target.closest('tr');
    //         const id = row.dataset.id;
    //
    //         try {
    //             // Отправка запроса на удаление задачи
    //             const response = await fetch(`/api/${id}`, {
    //                 method: 'DELETE'
    //             });
    //
    //             if (!response.ok) {
    //                 alert("Ошибка при удалении задачи");
    //                 return;
    //             }
    //
    //             row.remove();
    //             updatePendingTasksCount();
    //         } catch (err) {
    //             console.error(err);
    //             alert("Ошибка соединения с сервером");
    //         }
    //     }
    // });

    // Инициализируем количество задач
    updatePendingTasksCount();
});
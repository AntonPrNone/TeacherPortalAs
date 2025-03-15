window.resizeImage = function (element, maxWidth, maxHeight) {
    const img = element;
    if (img.naturalWidth > maxWidth || img.naturalHeight > maxHeight) {
        const ratio = Math.min(maxWidth / img.naturalWidth, maxHeight / img.naturalHeight);
        img.style.width = (img.naturalWidth * ratio) + 'px';
        img.style.height = (img.naturalHeight * ratio) + 'px';
    }
};

// Функция для анимации изменения вида
window.animateViewChange = function (viewId) {
    const view = document.getElementById(viewId);
    if (view) {
        view.style.opacity = '0';
        view.style.transform = 'translateY(10px)';
        
        setTimeout(() => {
            view.style.opacity = '1';
            view.style.transform = 'translateY(0)';
        }, 50);
    }
};

window.makeTextareasAutoResize = function() {
    // Находим только те textarea, которые находятся внутри наших компонентов
    document.querySelectorAll('.question-item textarea, .answer-item textarea, .stages-editor textarea').forEach(textarea => {
        if (!textarea.classList.contains('auto-resize-added')) {
            textarea.classList.add('auto-resize-added');
            
            // Базовый метод авторесайза: автоматически регулирует высоту текстового поля
            function adjustHeight() {
                // Сначала сброс до минимума
                textarea.style.height = '0';
                // Затем установка по содержимому
                textarea.style.height = (textarea.scrollHeight) + 'px';
            }
            
            // Применяем сразу
            adjustHeight();
            
            // Добавляем обработчик события
            textarea.addEventListener('input', adjustHeight);
            
            // Повторяем регулировку при фокусе
            textarea.addEventListener('focus', adjustHeight);
        }
    });
}; 
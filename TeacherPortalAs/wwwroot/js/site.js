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

// Улучшенная функция для проверки необходимости разворачивания
window.checkContainerNeedsExpand = function(containerId) {
    try {
        const container = document.getElementById(containerId);
        if (!container) {
            console.log(`Контейнер ${containerId} не найден`);
            return false;
        }
        
        // Важно: проверяем, был ли контейнер когда-либо развернут
        const wasExpanded = container.dataset.wasExpanded === 'true';
        
        // Более точное определение переполнения
        const scrollHeight = container.scrollHeight;
        const clientHeight = container.clientHeight;
        const needExpand = scrollHeight > clientHeight + 10;
        
        console.log(`${containerId}: scrollHeight=${scrollHeight}, clientHeight=${clientHeight}, needExpand=${needExpand}, wasExpanded=${wasExpanded}`);
        
        // Всегда показываем кнопку, если контент был когда-либо развернут или нуждается в этом
        if (needExpand || wasExpanded) {
            return true;
        }
        return false;
    } catch (error) {
        console.error("Ошибка при проверке контейнера", error);
        return true; // Показываем кнопку в случае ошибки
    }
};

// Пусть эта функция останется для совместимости
window.checkNeedExpand = function() {
    console.log("Проверка выполнена через Blazor logics");
};

// Добавьте эту функцию
window.checkElementExists = function(id) {
    return document.getElementById(id) !== null;
};

// Функция для установки data-атрибута, показывающего, что контейнер был развернут
window.setExpandedState = function(containerId, expanded) {
    const container = document.getElementById(containerId);
    if (container) {
        container.dataset.wasExpanded = expanded;
        console.log(`Установлен data-wasExpanded=${expanded} для ${containerId}`);
    }
};

// Добавьте эту функцию в ваш существующий site.js
window.smoothlyExpand = function (elementId) {
    const element = document.getElementById(elementId);
    if (!element) return;
    
    // Сохраняем текущую высоту
    const startHeight = element.offsetHeight;
    
    // Временно убираем ограничения для измерения полной высоты
    element.style.maxHeight = 'none';
    element.style.transition = 'none';
    
    // Измеряем реальную высоту
    const targetHeight = element.offsetHeight;
    
    // Возвращаем начальное состояние
    element.style.maxHeight = startHeight + 'px';
    
    // Триггерим перерисовку
    void element.offsetWidth;
    
    // Устанавливаем плавный переход и новую высоту
    element.style.transition = 'max-height 0.5s ease-in-out';
    element.style.maxHeight = targetHeight + 'px';
    
    // После завершения анимации убираем ограничения
    setTimeout(() => {
        element.style.maxHeight = 'none';
    }, 500);
};

// Функция для плавного сворачивания
window.smoothlyCollapse = function (elementId, collapsedHeight) {
    const element = document.getElementById(elementId);
    if (!element) return;

    // Измеряем текущую высоту
    const startHeight = element.offsetHeight;
    
    // Устанавливаем начальную высоту
    element.style.maxHeight = startHeight + 'px';
    
    // Триггерим перерисовку
    void element.offsetWidth;
    
    // Устанавливаем плавный переход и конечную высоту
    element.style.transition = 'max-height 0.5s ease-in-out';
    element.style.maxHeight = collapsedHeight + 'px';
}; 
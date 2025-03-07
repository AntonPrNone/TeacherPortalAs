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
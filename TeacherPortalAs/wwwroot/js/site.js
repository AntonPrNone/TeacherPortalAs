window.resizeImage = function (element, maxWidth, maxHeight) {
    const img = element;
    if (img.naturalWidth > maxWidth || img.naturalHeight > maxHeight) {
        const ratio = Math.min(maxWidth / img.naturalWidth, maxHeight / img.naturalHeight);
        img.style.width = (img.naturalWidth * ratio) + 'px';
        img.style.height = (img.naturalHeight * ratio) + 'px';
    }
}; 
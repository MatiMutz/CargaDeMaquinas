function SetCloseOnClickOutside(element) {
    document.addEventListener('mousedown', function (e) {
        if (!element.contains(e.target)) {
            if (e.target.nodeName != "IMG") {
                // para evitar que el click sobre el scrollbar oculte el elemento
                //if ((e.target.nodeName == 'HTML' && e.clientX >= document.documentElement.offsetWidth) == false) {
                //}
                element.style.display = 'none';
            }
        }
    }.bind(this));
}

function SetElementFocus(element) {
    element.focus();
}

function SetElementDisplayBlock(element) {
    element.style.display = 'block';
}

function ScrollToTopOfWindow() {
    window.scrollTo(0, 0);
}
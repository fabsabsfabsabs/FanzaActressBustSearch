class Actress {
    static changeMovie(id, nextId) {
        const triggerId = document.getElementById("trigger" + id);
        const triggerNextId = document.getElementById("trigger" + nextId);
        triggerId.checked = false;
        triggerNextId.checked = true;
        Actress.changeTrigger(id);
        Actress.changeTrigger(nextId);
    }
    static changeTrigger(id) {
        const trigger = document.getElementById("trigger" + id);
        if (trigger.checked) {
            let size = "s";
            if (window.innerWidth > 768)
                size = "l";
            else if (window.innerWidth > 576)
                size = "m";
            const movie = document.getElementById("movie" + id + size);
            Actress.remakeElement(movie, movie.getAttribute('data-src'));
        }
        else {
            const movieL = document.getElementById("movie" + id + "l");
            const movieM = document.getElementById("movie" + id + "m");
            const movieS = document.getElementById("movie" + id + "s");
            Actress.remakeElement(movieL, 'about:blank');
            Actress.remakeElement(movieM, 'about:blank');
            Actress.remakeElement(movieS, 'about:blank');
        }
    }
    static changeCheckbox(id) {
        document.cookie = "OnlySingle=" + document.getElementById("OnlySingle").checked + ";Path=/";
        document.cookie = "OnlyWithMovie=" + document.getElementById("OnlyWithMovie").checked + ";Path=/";
        location.href = "/actress/" + id;
    }
    static remakeElement(element, attribute) {
        //removeAttribute('src')だとブラウザの履歴に追加されてしまう
        const parentElement = element.parentElement;
        element.remove();
        element.setAttribute('src', attribute);
        parentElement.appendChild(element);
        while (element.firstChild) {
            element.removeChild(element.firstChild);
        }
    }
}
//# sourceMappingURL=actress.js.map
class Actress
{
    public static changeMovie(id: string, nextId: string): void {
        const triggerId = document.getElementById("trigger" + id) as HTMLInputElement;
        const triggerNextId = document.getElementById("trigger" + nextId) as HTMLInputElement;
        triggerId.checked = false;
        triggerNextId.checked = true;
        Actress.changeTrigger(id);
        Actress.changeTrigger(nextId);
    }

    public static changeTrigger(id: string): void {
        const trigger = document.getElementById("trigger" + id) as HTMLInputElement;
        if (trigger.checked) {
            let size = "s";
            if (window.innerWidth > 768) size = "l";
            else if (window.innerWidth > 576) size = "m";
            const movie = document.getElementById("movie" + id + size);
            Actress.remakeElement(movie, movie.getAttribute('data-src'));
        } else {
            const movieL = document.getElementById("movie" + id + "l");
            const movieM = document.getElementById("movie" + id + "m");
            const movieS = document.getElementById("movie" + id + "s");
            Actress.remakeElement(movieL, 'about:blank');
            Actress.remakeElement(movieM, 'about:blank');
            Actress.remakeElement(movieS, 'about:blank');
        }
    }

    public static changeCheckbox(id: number): void {
        document.cookie = "OnlySingle=" + (document.getElementById("OnlySingle") as HTMLInputElement).checked + ";Path=/";
        document.cookie = "OnlyWithMovie=" + (document.getElementById("OnlyWithMovie") as HTMLInputElement).checked + ";Path=/";
        location.href = "/actress/" + id;
    }

    private static remakeElement(element: HTMLElement, attribute: string): void {
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

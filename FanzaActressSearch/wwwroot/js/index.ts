function changeArrow(arrow: string): void {
    document.cookie = `ActressViewArrow=${arrow === "up" ? "down" : "up"};Path=/`;
    location.reload();
}

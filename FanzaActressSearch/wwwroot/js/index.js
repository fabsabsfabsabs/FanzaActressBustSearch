function changeArrow(arrow) {
    document.cookie = `ActressViewArrow=${arrow === "up" ? "down" : "up"};Path=/`;
    location.reload();
}
//# sourceMappingURL=index.js.map
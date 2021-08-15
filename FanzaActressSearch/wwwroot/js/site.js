//現状のURLからpageは抜いてリロード
function reloadWithoutPage(id) {
    const value = document.getElementById(id).value;
    const url = new URL(location.href);
    url.searchParams.delete('page');
    document.cookie = `${id}=${value};Path=/`;
    location.href = url.href;
}
function searchWordClick(searchText) {
    const url = new URL(location.href);
    url.searchParams.delete('page');
    location.href = `/?searchText=${encodeURIComponent(searchText)}`;
}
function changeBust(id) {
    const value = document.getElementById(id).value;
    searchWordClick(value);
}
//# sourceMappingURL=site.js.map
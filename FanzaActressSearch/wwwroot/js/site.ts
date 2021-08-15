//現状のURLからpageは抜いてリロード
function reloadWithoutPage(id: string) {
    const value = (document.getElementById(id) as HTMLInputElement).value;
    const url = new URL(location.href);
    url.searchParams.delete('page');
    document.cookie = `${id}=${value};Path=/`;
    location.href = url.href;
}

function searchWordClick(searchText: string) {
    const url = new URL(location.href);
    url.searchParams.delete('page');
    location.href = `/?searchText=${encodeURIComponent(searchText)}`;
}

function changeBust(id: string) {
    const value = (document.getElementById(id) as HTMLInputElement).value;
    searchWordClick(value);
}

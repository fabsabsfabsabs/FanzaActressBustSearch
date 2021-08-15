function getCookieArray(): string[] {
    const array: string[] = new Array();

    if (document.cookie === '') return array;
    const tmp = document.cookie.split('; ');
    for (let i = 0; i < tmp.length; i++) {
        const data = tmp[i].split('=');
        array[data[0]] = decodeURIComponent(data[1]);
    }
    return array;
}

//年齢認証は人間だけにする
const cookieArray = getCookieArray();
if (cookieArray['Verification'] !== 'Ok') {
    document.cookie = "Verification=Notyet;Path=/";
    if (location.pathname.indexOf("/actress") !== -1) {
        location.href = `verification?url=${location.href}`;
    }
}

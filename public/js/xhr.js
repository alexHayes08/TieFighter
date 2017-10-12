function XMLHttpRequestPromise (method, url) {
    return new Promise(function (resolve, reject) {
        var request = new XMLHttpRequest();
        request.addEventListener("load", function () {
            if (request.status == 200) {
                resolve(request.response);
            } else {
                reject(request.response);
            }
        });
        request.open(method, url);
        request.send();
    });
}
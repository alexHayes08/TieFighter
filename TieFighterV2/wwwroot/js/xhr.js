function XMLHttpRequestPromise (method, url, json) {
    return new Promise(function (resolve, reject) {
        var request = new XMLHttpRequest();
        request.withCredentials = true;

        request.addEventListener("load", function () {
            if (request.status == 200) {
                resolve(request.response);
            } else {
                reject(request.response);
            }
        });

        request.open(method, url);
        if (json) {
            request.setRequestHeader("Content-type", "application/json");
        }
        request.send(json);
    });
}
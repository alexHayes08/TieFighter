function XMLHttpRequestPromise(method, url, data, contentType) {
    if (contentType == null) {
        contentType = "application/json";
    }

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
        if (data && (contentType != false)) {
            request.setRequestHeader("Content-type", contentType);
        }
        request.send(data);
    });
}
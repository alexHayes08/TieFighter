function isIdValid(id) {
    if (id == null || id == "") {
        throw new Error("Expected an argument, none was provided");
    }

    XMLHttpRequestPromise("POST", window.location.origin + `/Medals/IsIdAvailable/${id}`)
        .then(function (response) {
            console.log(response);
        }).catch(function (error) {
            console.log(error);
        });
}

function updateMedal(formEl) {
    XMLHttpRequestPromise(
        "POST",
        window.location.origin + `/Medals/Update`,
        formEl.serializeArray())
        .then(function (response) {
            console.log(response);
        }).catch(function (error) {
            console.log(error);
        })  
}

function createMedal(formEl) {
    console.log("Not yet working...");
}
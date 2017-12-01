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

/**
 * 
 * @param {any} id The id of the medal
 * @param {any} formEl Must be an instance of FormData
 */
function updateMedal(id, formEl) {
    return XMLHttpRequestPromise(
        "POST",
        window.location.origin + `/Admin/Medals/Update/${id}`,
        formEl,
        false
        //"multipart/form-data"
    );
}

function createMedal(formEl) {
    console.log("Not yet working...");
}

function showErrorMsg(error) {
    console.log(`Error: ${error}`);
}
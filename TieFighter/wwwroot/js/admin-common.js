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
 * Adds or removes the disabled attributes to all inputs passed in.
 * @param {any} $inputs A jQuery array of elements
 * @param {any} boolean Whether to enable or disable the inputs
 */
function setInputsTo($inputs, boolean) {
    if (boolean) {
        $inputs.removeAttr("disabled");
    } else {
        $inputs.attr("disabled", "");
    }
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

function setupDataTable(tableCssSelector, selectAllCssSelector, deleteAllCssSelector) {
    var $table = $(tableCssSelector);
    var lastCol = $table.find("tbody > tr:first-child > td").length - 1;
    var datatable = $table.DataTable({
        columnDefs: [
            {
                orderable: false,
                targets: 0
            },
            {
                orderable: false,
                targets: lastCol
            }
        ],
        select: {
            selector: "td:first-child",
            items: "row"
        },
        order: [[1, 'asc']],
        info: false
    })[0];

    $table.find("> tbody > tr td:first-child input[type='checkbox']").on("change", function () {
        if ($(this).prop("checked")) {
            $($(this).parents()[3]).addClass("selected");
        } else {
            $($(this).parents()[3]).removeClass("selected");
        }
    });

    if (selectAllCssSelector != null || selectAllCssSelector != "") {
        $(selectAllCssSelector).on("change", function () {
            var inputs = $table.find("tbody tr input[type='checkbox']");
            if ($(this).prop("checked")) {

                // Select all medals
                inputs.prop("checked", true);
                inputs.change();
            } else {

                // Deselect all medals
                inputs.prop("checked", false);
                inputs.change();
            }
        });
    }

    if (deleteAllCssSelector != null || deleteAllCssSelector != "") {
        $(deleteAllCssSelector).on("click", function () {
            isLoading($table[0], true);
            var selectedRows = $table.find("tbody tr.selected");
            var form = new FormData();
            selectedRows.each(function () {
                form.append($(this).attr("data-bind"), null);
            });
            XMLHttpRequestPromise("POST", `${window.location.origin}/Admin/${window.location.pathname.split("/")[2]}/Delete`, form, false)
                .then(function (response) {
                    console.log(response);
                    var res = JSON.parse(response);
                    if (!res.succeeded) {
                        throw JSON.stringify(new Error("Failed to delete"));
                    } else {
                        console.log(datatable);
                        for (var i = 0; i < datatable.rows.length; i++) {
                            if (datatable.rows[i].classList.contains("selected")) {
                                datatable.deleteRow(i);
                            }
                        }
                    }
                    isLoading($table[0], false);
                }).catch(function (error) {
                    //var res = JSON.parse(error);
                    console.log(error);
                    isLoading($table[0], false);
                });
        });
    }

    return datatable;
}
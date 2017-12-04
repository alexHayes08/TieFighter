$(function () {

    var datatable = null;
    $(function () {
        $(document).ready(function () {
            datatable = $('#table').DataTable({
                columnDefs: [
                    {
                        orderable: false,
                        targets: 0
                    },
                    {
                        orderable: false,
                        targets: 1
                    },
                    {
                        orderable: false,
                        targets: 4
                    }
                ],
                select: {
                    selector: "td:first-child",
                    items: "row"
                },
                order: [[1, 'asc']],
                info: false
            })[0];
        });
    });

    $("#deleteSelected").on("click", function () {
        var selectedRows = $("#table tbody tr.selected");
        var form = new FormData();
        selectedRows.each(function () {
            form.append($(this).attr("data-bind"), null);
        });
        XMLHttpRequestPromise("POST", `${window.location.origin}/Admin/Medals/Delete`, form)
            .then(function (response) {
                console.log(response);
                var res = JSON.parse(response);
                if (!res.succeeded) {
                    throw JSON.stringify(new Error("Failed to delete"));
                } else {
                    console.log("Removing all selected columns");
                    console.log(datatable);
                    for (var i = 0; i < datatable.rows.length; i++) {
                        if (datatable.rows[i].classList.contains("selected")) {
                            datatable.deleteRow(i);
                        }
                    }
                }
            }).catch(function (error) {
                //var res = JSON.parse(error);
                console.log(error);
            });
    });
    $("#selectAll").on("change", function () {

        var inputs = $("#table tbody tr input[type='checkbox']");
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

    $("#table > tbody > tr td:first-child input[type='checkbox']").on("change", function () {
        if ($(this).prop("checked")) {
            $($(this).parents()[3]).addClass("selected");
        } else {
            $($(this).parents()[3]).removeClass("selected");
        }
    });
});
$(function () {
    var datatable = null;
    var datatable = $("#usersTable").DataTable({
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
        order: [[2, 'asc']],
        info: false
    })[0];

    $("#usersTable > tbody > tr td:first-child input[type='checkbox']").on("change", function () {
        if ($(this).prop("checked")) {
            $($(this).parents()[3]).addClass("selected");
        } else {
            $($(this).parents()[3]).removeClass("selected");
        }
    });

    $("#selectAll").on("change", function () {

        var inputs = $("#usersTable tbody tr input[type='checkbox']");
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
});